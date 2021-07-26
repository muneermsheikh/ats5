using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.Identity;
using core.Entities.Orders;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class OrderService : IOrderService
     {
          private readonly ATSContext _context;
          private readonly IUnitOfWork _unitOfWork;
          private readonly UserManager<AppUser> _userManager;
          //private readonly IPaymentService _paymentService;
          private readonly IGenericRepository<OrderItem> _orderItemRepo;
          public OrderService(IUnitOfWork unitOfWork
               //, IPaymentService paymentService
               , ATSContext context
               , UserManager<AppUser> userManager
               , IGenericRepository<OrderItem> orderItemRepo)
          {
               _context = context;
               //_paymentService = paymentService;
               _userManager = userManager;
               _unitOfWork = unitOfWork;
               _orderItemRepo = orderItemRepo;
          }

          public async Task<Order> CreateOrderAsync(OrderToCreateDto dto) 
          //DateTime orderDate, string orderRef, int customerId, 
                //DateTime completeBy, int? salesmanId, string remarks, ICollection<OrderItem> orderitems)
          {

               string salesmanName = "";
               if(dto.SalesmanId !=0) salesmanName = await EmployeeNameEmployeeId((int)dto.SalesmanId);
               
               //isnert customer name
               var cus = await _context.Customers.Where(x => x.Id == dto.CustomerId)
                    .Select(x => new {x.CustomerName, x.Add, x.Add2, x.City, x.Pin, x.District, x.State, x.Country, x.Email})
                    .FirstOrDefaultAsync();
               dto.CustomerName = cus.CustomerName;
               dto.OrderAddress = new OrderAddress(cus.CustomerName, cus.Add, cus.Add2, "", cus.City, 
                    cus.District, cus.State, cus.Pin,cus.Country);

               var subtotal = 0;
               var items = new List<OrderItem>();
               foreach (var item in dto.OrderItems)
               {
                    var categoryName = await CategoryNameFromId(item.CategoryId);
                    var industryName = await IndustryNameFromId(item.IndustryId);
                    items.Add(new OrderItem(item.SrNo, item.CategoryId, categoryName, item.IndustryId, 
                         industryName, item.SourceFrom, item.Quantity, item.MinCVs, item.MaxCVs,
                         item.Ecnr, item.RequireAssess, item.CompleteBefore, item.Charges));
                    subtotal = items.Sum(item => item.Charges * item.Quantity) + (items.Sum(item => item.FeeFromClientINR * item.Quantity));
               }

               // create order
               var orderNo = await _context.Orders.MaxAsync(x => (int?)x.OrderNo) ?? 1000;
               orderNo++;

              var order = new Order(orderNo, dto.CustomerId, dto.OrderRef, (int)dto.SalesmanId, subtotal, 
                    dto.CompleteBy, dto.OrderAddress, items);
               order.SalesmanName = salesmanName;
               order.CityOfWorking = cus.City;
               order.Country = cus.Country;
               order.OrderAddress = dto.OrderAddress;
               order.BuyerEmail = cus.Email ?? "not available";
               _unitOfWork.Repository<Order>().Add(order);


               // TODO: save to db
               var result = await _unitOfWork.Complete();

               if (result <= 0) return null;

               // return order
               return order;
          }

          public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
          {
               return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
          }

          public async Task<Order> GetOrderByIdAsync(int id)
          {
               var spec = new OrdersWithItemsAndOrderingSpecs(id);

               return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
          }

          public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(int customerId)
          {
               var spec = new OrdersWithItemsAndOrderingSpecs(customerId);

               return await _unitOfWork.Repository<Order>().ListAsync(spec);
          }

          public async Task<IReadOnlyList<Order>> GetOrdersByEmailAsync(string email)
          {
               var spec = new OrdersWithItemsAndOrderingSpecs(email);

               return await _unitOfWork.Repository<Order>().ListAsync(spec);
          }

          public async Task<IReadOnlyList<Order>> GetOrdersAllAsync()
          {
               var spec = new OrdersWithItemsAndOrderingSpecs();

               return await _unitOfWork.Repository<Order>().ListAsync(spec);
          }

          public void EditOrder(Order order)
          {
               //thanks to @slauma of stackoverflow
               var existingOrder = _context.Orders.Where(p => p.Id == order.Id)
                    .Include(p => p.OrderItems).ThenInclude(p => p.JobDescription)
                    .Include(p => p.OrderItems).ThenInclude(p => p.Remuneration)
                    .AsNoTracking()
                    .SingleOrDefault();

               //check for foreign keys missing
               //if (order.SalesmanId == 0) order.SalesmanId = defaultSalesmanId;
               //if (order.ProjectManagerId == 0) order.ProjectManagerId = defaultProjectManagerId;
               if (existingOrder != null)
               {
                    _context.Entry(existingOrder).CurrentValues.SetValues(order);   //saves only the parent, not children

                    //the children - order items
                    //Delete children that exist in existing record, but not in the new model order
                    foreach (var existingItem in existingOrder.OrderItems.ToList())
                    {
                         if (!order.OrderItems.Any(c => c.Id == existingItem.Id && c.Id != default(int)))
                         {
                         _context.OrderItems.Remove(existingItem);
                         _context.Entry(existingItem).State = EntityState.Deleted;
                         }
                    }

                    //children that are not deleted, are either updated or new ones to be added
                    foreach (var item in order.OrderItems)
                    {
                         var existingItem = existingOrder.OrderItems.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();
                         if (existingItem != null)       // Update child
                         {
                              _context.Entry(existingItem).CurrentValues.SetValues(item);
                              _context.Entry(existingItem).State = EntityState.Modified;
                         } else            //insert children as new record
                         {
                              var jd = item.JobDescription;
                              if (jd == null) jd = new JobDescription();
                              var newJd = new JobDescription(jd.JobDescInBrief ?? "Not Available", 
                                   jd.QualificationDesired ?? "Not Available",
                                   jd.ExpDesiredMin, jd.ExpDesiredMax, jd.MinAge, jd.MaxAge);
                              var rem = item.Remuneration;
                              if (rem == null) rem = new Remuneration();
                              var newRem = new Remuneration(rem.SalaryCurrency ?? "???", rem.SalaryMin, rem.SalaryMax,
                                   rem.ContractPeriodInMonths, rem.HousingProvidedFree, rem.HousingAllowance,
                                   rem.FoodProvidedFree, rem.FoodAllowance, rem.TransportProvidedFree,
                                   rem.TransportAllowance, rem.OtherAllowance, rem.LeavePerYearInDays,
                                   rem.LeaveAirfareEntitlementAfterMonths);

                              var newItem = new OrderItem
                              {
                                   OrderId = existingOrder.Id, SrNo = item.SrNo,
                                   CategoryId = item.CategoryId, SourceFrom = item.SourceFrom,
                                   Quantity = item.Quantity, MinCVs = item.MinCVs,
                                   MaxCVs = item.MaxCVs, Ecnr = item.Ecnr, RequireAssess = item.RequireAssess,
                                   HrExecId = item.HrExecId, HrSupId = item.HrSupId, HrmId = item.HrmId,
                                   AssignedId = item.AssignedId, Charges = item.Charges, Status = item.Status,
                                   JobDescription = newJd, Remuneration = newRem
                              };
                              existingOrder.OrderItems.Add(newItem);
                              _context.Entry(newItem).State = EntityState.Added;
                         }
                }
                _context.Entry(existingOrder).State = EntityState.Modified;
            }
          }

          public void DeleteOrder(Order order)
          {
               _context.Entry(order).State = EntityState.Deleted;
          }
          private async Task<string> CategoryNameFromId(int id)
          {
               return await _context.Categories.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
          }

          private async Task<string> IndustryNameFromId(int id)
          {
               return await _context.Industries.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefaultAsync();
          }

          private async Task<string> EmployeeNameEmployeeId(int id)
          {
               var nm = await _context.Employees.Where(x => x.Id == id)
                    .Select(x => new {x.Person.FirstName, x.Person.FamilyName}).FirstOrDefaultAsync();
               return nm.FirstName + " " + nm.FamilyName;
          }

          public async Task<IReadOnlyList<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
          {
               var spec = new OrderItemsSpecs(orderId,0);
               var items = await _orderItemRepo.ListAsync(spec);
               return items;
          }

          public async Task<OrderItem> GetOrderItemByOrderItemIdAsync(int Id)
          {
               var item = await _orderItemRepo.GetByIdAsync(Id);
               return item;
          }

          public void AddOrderItem(OrderItem orderItem)
          {
               var item = new OrderItem(orderItem.OrderId, orderItem.SrNo, orderItem.CategoryId, orderItem.CategoryName,
                    orderItem.IndustryId, orderItem.IndustryName, orderItem.SourceFrom, orderItem.Quantity, orderItem.MinCVs,
                    orderItem.MaxCVs, orderItem.Ecnr, orderItem.RequireAssess, orderItem.CompleteBefore, orderItem.Charges);
               
               _context.OrderItems.Add(item);
               _context.Entry(item).State = EntityState.Added;
          }

          public void EditOrderItem(OrderItem model)
          {
               var existingItem = _context.OrderItems
                    .Where(x => x.Id == model.Id)
                    .Include(x => x.JobDescription)
                    .Include(x => x.Remuneration)
                    .AsNoTracking().FirstOrDefault();
               
               if (existingItem == null) return;

               _context.Entry(existingItem).CurrentValues.SetValues(model);

               var jd = model.JobDescription;
               if (jd == null) {
                    jd = new JobDescription();
                    var newJd = new JobDescription(jd.JobDescInBrief ?? "Not Available", 
                         jd.QualificationDesired ?? "Not Available",
                         jd.ExpDesiredMin, jd.ExpDesiredMax, jd.MinAge, jd.MaxAge);
                    existingItem.JobDescription = newJd;
                    _context.Entry(newJd).State = EntityState.Added;
               } else {
                    _context.Entry(existingItem.JobDescription).CurrentValues.SetValues(jd);
               }
               
               var rem = model.Remuneration;
               if (rem == null) {
                    rem = new Remuneration();
                    var newRem = new Remuneration(rem.SalaryCurrency ?? "???", rem.SalaryMin, rem.SalaryMax,
                         rem.ContractPeriodInMonths, rem.HousingProvidedFree, rem.HousingAllowance,
                         rem.FoodProvidedFree, rem.FoodAllowance, rem.TransportProvidedFree,
                         rem.TransportAllowance, rem.OtherAllowance, rem.LeavePerYearInDays,
                         rem.LeaveAirfareEntitlementAfterMonths);
                    existingItem.Remuneration = newRem;
                    _context.Entry(newRem).State = EntityState.Added;
               } else {
                    _context.Entry(existingItem.Remuneration).CurrentValues.SetValues(rem);
               }
               
               _context.Entry(existingItem).State = EntityState.Modified;                    
          }

          public bool EditOrderItemWithNavigationObjects(OrderItem modelItem)
          {
               throw new NotImplementedException();
          }

          public void DeleteOrderItem(OrderItem orderItem)
          {
               throw new NotImplementedException();
          }

          public Task<ICollection<JobDescription>> GetJobDescriptionsByOrderIdAsync(int Id)
          {
               throw new NotImplementedException();
          }

          public Task<JobDescription> GetJobDescriptionByOrderItemIdAsync(int Id)
          {
               throw new NotImplementedException();
          }

          public void AddJobDescription(JobDescription jobDescription)
          {
               throw new NotImplementedException();
          }

          public void EditJobDescription(JobDescription jobDescription)
          {
               throw new NotImplementedException();
          }

          public void DeleteJobDescription(JobDescription jobDescription)
          {
               throw new NotImplementedException();
          }

          public Task<RemunerationDto> GetRemunerationsByOrderIdAsync(int Id)
          {
               throw new NotImplementedException();
          }

          public Task<Remuneration> GetRemunerationOfOrderItemAsync(int Id)
          {
               throw new NotImplementedException();
          }

          public void AddRemuneration(Remuneration remuneration)
          {
               throw new NotImplementedException();
          }

          public void EditRemuneration(Remuneration remuneration)
          {
               throw new NotImplementedException();
          }

          public void DeleteRemuneration(Remuneration remuneration)
          {
               throw new NotImplementedException();
          }
     }

}