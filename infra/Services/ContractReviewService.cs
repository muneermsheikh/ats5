using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using core.Entities.EmailandSMS;
using core.Entities.MasterEntities;
using core.Entities.Orders;
using core.Entities.Tasks;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace infra.Services
{
     public class ContractReviewService : IContractReviewService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ATSContext _context;
          private readonly IMapper _mapper;
          private readonly IComposeMessages _composeMsg;
          private readonly IOrderService _orderService;
          private readonly int _OperationsManagementId;
          private readonly IEmailService _emailService;
          public ContractReviewService(IUnitOfWork unitOfWork, ATSContext context, IMapper mapper,
               IComposeMessages composeMsg, IOrderService orderService, IConfiguration config, IEmailService emailService)
          {
               _emailService = emailService;
               _OperationsManagementId = Convert.ToInt32(config.GetSection("OperationsManagementId").Value);
               _orderService = orderService;
               _composeMsg = composeMsg;
               _mapper = mapper;
               _context = context;
               _unitOfWork = unitOfWork;
          }

          public void AddReviewItemStatus(string reviewItemStatusName)
     {
          var status = new ReviewItemStatus(reviewItemStatusName);
          _unitOfWork.Repository<ReviewItemStatus>().Add(status);
     }

          public void AddReviewStatus(string reviewStatusName)
          {
               var status = new ReviewStatus { Status = reviewStatusName };
               _unitOfWork.Repository<ReviewStatus>().Add(status);
          }

          public async Task<EmailMessageDto> EditContractReview(ContractReview model)
          {
               //thanks to @slauma of stackoverflow
               var existingObj = await _context.ContractReviews
               .Where(p => p.Id == model.Id)
               .Include(x => x.ContractReviewItems).ThenInclude(x => x.ReviewItems)
               //.AsNoTracking()
               .SingleOrDefaultAsync();

               if (existingObj == null) throw new Exception("The Contract Review model does not exist in the database");
               if (existingObj.ContractReviewItems == null) throw new Exception("The Contract Review Items collection does not exist in the database");
               if (existingObj.ContractReviewItems.Any(x => x.ReviewItems == null)) throw new Exception("Review Parameters in one of the items do not exist");

               _context.Entry(existingObj).CurrentValues.SetValues(model);   //saves only the parent, not children

               //Delete children that exist in existing record, but not in the new model order
               foreach (var existingItem in existingObj.ContractReviewItems.ToList())
               {
                    if (!model.ContractReviewItems.Any(c => c.Id == existingItem.Id && c.Id != default(int)))
                    {
                         _context.ContractReviewItems.Remove(existingItem);
                         _context.Entry(existingItem).State = EntityState.Deleted;
                    }
               }

               //children that are not deleted, are either updated or new ones to be added
               foreach (var itemModel in model.ContractReviewItems)
               {
                    //work on the contractReviewItem
                    var existingItem = existingObj.ContractReviewItems.Where(c => c.Id == itemModel.Id && c.Id != default(int)).SingleOrDefault();
                    if (existingItem != null)       // record exists, update it
                    {
                         _context.Entry(existingItem).CurrentValues.SetValues(itemModel);
                         _context.Entry(existingItem).State = EntityState.Modified;
                    }
                    else            //record does not exist, insert a new record
                    {
                         var newItem = new ContractReviewItem(itemModel.OrderItemId, itemModel.OrderId, itemModel.CategoryName, itemModel.Quantity);
                         existingObj.ContractReviewItems.Add(newItem);
                         _context.Entry(newItem).State = EntityState.Added;
                    }

                    //work on ContractReviewItem.ReviewItems
                    //check if records to be deleted - if exists in DB but not in model
                    foreach (var existingRvwItem in existingItem.ReviewItems.ToList())
                    {
                         if (!itemModel.ReviewItems.Any(c => c.Id == existingRvwItem.Id && c.Id != default(int)))
                         {
                              _context.ReviewItems.Remove(existingRvwItem);
                              _context.Entry(existingRvwItem).State = EntityState.Deleted;
                         }
                    }

                    foreach (var reviewItemModel in itemModel.ReviewItems.ToList())
                    {
                         var existingRvwItem = existingItem.ReviewItems.Where(c => c.Id == reviewItemModel.Id && c.Id != default(int)).SingleOrDefault();
                         if (existingRvwItem != null)
                         {     //record exists
                              _context.Entry(existingRvwItem).CurrentValues.SetValues(reviewItemModel);
                              _context.Entry(existingRvwItem).State = EntityState.Modified;
                         }
                         else
                         {            // add new
                              var newRvw = new ReviewItem
                              {
                                   SrNo = reviewItemModel.SrNo,
                                   ReviewParameter = reviewItemModel.ReviewParameter,
                                   Response = reviewItemModel.Response,
                                   IsMandatoryTrue = reviewItemModel.IsMandatoryTrue,
                                   Remarks = reviewItemModel.Remarks
                              };
                              existingItem.ReviewItems.Add(newRvw);
                              _context.Entry(newRvw).State = EntityState.Added;
                         }
                    }
               }

               //update ContractReview.ReviewStatus
               if (existingObj.ContractReviewItems.Any(x => x.ReviewItemStatus == (int)EnumReviewItemStatus.NotReviewed))
               {
                    existingObj.RvwStatusId = (int)EnumReviewStatus.NotReviewed;
               }
               else if (existingObj.ContractReviewItems.Any(x => x.ReviewItemStatus != (int)EnumReviewItemStatus.Accepted)
               && existingObj.ContractReviewItems.Any(x => x.ReviewItemStatus == (int)EnumReviewItemStatus.Accepted))
               {   //atleast one categry accepted and atleast 1 category not accepted
                    existingObj.RvwStatusId = (int)EnumReviewStatus.AcceptedWithSomeRegrets;
               }
               else {
                    existingObj.RvwStatusId = (int)EnumReviewStatus.Accepted;
               }

               _context.Entry(existingObj).State = EntityState.Modified;

               if (await _context.SaveChangesAsync() > 0)
               {
                    //if reviewed and accepted, forward requirement to HR Dept
                    if ((existingObj.RvwStatusId == (int)EnumReviewStatus.Accepted ||
                         existingObj.RvwStatusId == (int)EnumReviewStatus.AcceptedWithSomeRegrets) &&
                         existingObj.ReleasedForProduction)
                    {
                         //FORWARD REQUIREMENT TO HR DEPT
                         var order = await _context.Orders.Where(x => x.Id == existingObj.OrderId)
                              .Include(x => x.OrderItems).FirstOrDefaultAsync();
                         var emailMsg = await _composeMsg.ForwardEnquiryToHRDept(order);

                         order.ForwardedToHRDeptOn = DateTime.Now;
                         _unitOfWork.Repository<Order>().Update(order);
                         await _unitOfWork.Complete();

                         //create task in the name of the project manager
                         var newTask = new ApplicationTask((int)EnumTaskType.OrderAssignmentToProjectManager,
                              existingObj.ReviewedOn, _OperationsManagementId, order.ProjectManagerId, order.Id,
                              order.OrderNo,  0, "You are assigned to be totally responsible to execute Order No. " + 
                              order.OrderNo + " dt " + order.OrderDate.Date + 
                              ", task for the same is created in your name.  Check your Task Dashboard for details.",
                              ((DateTime)order.ForwardedToHRDeptOn).Date.AddDays(8), "Not Started",0, null);
                         
                         return new EmailMessageDto { EmailMessage = emailMsg, Success = true };
                    }
                    else
                    {
                         return new EmailMessageDto { EmailMessage = null, Success = true };
                    }
               }
               throw new Exception("Failed to save the changes");
          }


          public async Task<ContractReview> GetContractReviewDtoByOrderIdAsync(int orderId)
          {
               var crvw = await _context.ContractReviews.Where(x => x.OrderId == orderId)
                    .Include(x => x.ContractReviewItems)
                    .ThenInclude(x => x.ReviewItems)
                    .FirstOrDefaultAsync();
               return crvw;
          }

          /* 
          public async Task<IReadOnlyList<ContractReviewItemDto>> GetContractReviewItemsByOrderIdAsync(int orderid)
          {
               var items = await _unitOfWork.Repository<ContractReviewItem>().ListAsync(new ContractReviewSpecs(orderid, 0));
               return _mapper.Map<IReadOnlyList<ContractReviewItem>, IReadOnlyList<ContractReviewItemDto>>(items);
          }
          */

          public async Task<ContractReviewItemDto> GetContractReviewItemWithOrderDetails(int orderItemId)
          {
               var rvwItem = await _unitOfWork.Repository<ContractReviewItem>()
                    .GetEntityWithSpec(new ContractReviewSpecs(orderItemId));
               var orderItem = await _context.OrderItems.FindAsync(orderItemId);
               var orderNo = await _context.Orders.Where(x => x.Id == orderItem.OrderId).Select(x => x.OrderNo).FirstOrDefaultAsync();

               var dto = new ContractReviewItemDto
               {
                    Id = rvwItem.Id,
                    ContractReviewId = rvwItem.ContractReviewId,
                    OrderId = orderItem.OrderId,
                    OrderNo = orderNo,
                    OrderItemId = orderItem.Id,
                    SrNo = orderItem.SrNo,
                    ProfessionName = orderItem.CategoryName,
                    Quantity = orderItem.Quantity,
                    Ecnr = orderItem.Ecnr,
                    RequireAssess = orderItem.RequireAssess,
                    CompleteBefore = orderItem.CompleteBefore,
                    ReviewItems = rvwItem.ReviewItems
               };

               return dto;
          }

          public async Task<bool> DeleteContractReview(int orderid)
          {

               var contractReview = await _context.ContractReviews.Where(x => x.OrderId == orderid).FirstOrDefaultAsync();
               if (contractReview == null) throw new Exception("the object to delete does not exist");
               _unitOfWork.Repository<ContractReview>().Delete(contractReview);

               /*
               //contractReview is configured for contractREviewItems to be cascade deleted
               //contractReviewItem is configured for REviewItems to be cascade deleted

               var items = await _context.ContractReviewItems.Where(x => x.OrderId == orderid).ToListAsync();
               foreach (var item in items)
               {
                    _unitOfWork.Repository<ContractReviewItem>().Delete(item);
               }
               */
               if (await _unitOfWork.Complete() == 0) throw new Exception("failed to delete the object");
               return true;
          }

          public async Task<bool> DeleteContractReviewItem(int orderitemid)
          {
               //contractReviewItem is configured to cascade delete ReviewItems
               var item = await _context.ContractReviewItems.Where(x => x.OrderItemId == orderitemid).FirstOrDefaultAsync();
               if (item == null) throw new Exception("the object to delete does not exist");
               _unitOfWork.Repository<ContractReviewItem>().Delete(item);
               if (await _unitOfWork.Complete() == 0) throw new Exception("failed to delete the object");
               return true;
          }

          public async Task<bool> DeleteReviewReviewItem(int id)
          {
               var reviewItem = await _context.ReviewItems.FindAsync(id);
               if (reviewItem == null) throw new Exception("the object to delete does not exist");
               _unitOfWork.Repository<ReviewItem>().Delete(reviewItem);
               if (await _unitOfWork.Complete() == 0) throw new Exception("Failed to delete the review item object");
               return true;
          }

          public async Task<ICollection<ReviewStatus>> GetReviewStatus()
          {
               return await _context.ReviewStatuses.ToListAsync();
          }

          public async Task<IReadOnlyList<ReviewItemStatus>> GetReviewItemStatus()
          {
               return await _unitOfWork.Repository<ReviewItemStatus>().ListAllAsync();
          }

          public async Task<ContractReview> CreateContractReviewObject(int orderId, int LoggedInAppUserId)
          {
               //check if Remuneration for all the order items exist
               var itemIds = await _context.OrderItems
                    .Where(x => x.OrderId == orderId)
                    .Select(x => x.Id)
                    .ToListAsync();
               if (itemIds.Count() == 0) throw new Exception("Order Items not created");
               var orderitems = await _context.OrderItems.Where(x => itemIds.Contains(x.Id))
                    .Include(x => x.Remuneration)
                    .ToListAsync();
               if (orderitems == null || orderitems.Count() == 0) throw new Exception("Remunerations need to be defined for all the items before the contract review");

               //check if the object exists
               var contractReview = await _context.ContractReviews.Where(x => x.OrderId == orderId).Include(x => x.ContractReviewItems).FirstOrDefaultAsync();
               if (contractReview != null) throw new System.Exception("Contract Review Object already exists");

               int loggedInEmployeeId = await _context.Employees.Where(x => x.AppUserId == LoggedInAppUserId).Select(x => x.Id).FirstOrDefaultAsync();

               var order = await _context.Orders.Where(x => x.Id == orderId)
                    .Include(x => x.OrderItems).FirstOrDefaultAsync();
               var contractReviewItems = new List<ContractReviewItem>();
               var reviewData = await _context.ReviewItemDatas.OrderBy(x => x.SrNo).ToListAsync();

               foreach (var item in order.OrderItems)
               {
                    if (string.IsNullOrEmpty(item.CategoryName)) item.CategoryName = await _context.Categories.Where(x => x.Id == item.CategoryId).Select(x => x.Name).FirstOrDefaultAsync();

                    var itemData = new List<ReviewItem>();
                    foreach (var data in reviewData)
                    {
                         itemData.Add(new ReviewItem
                         {
                              SrNo = data.SrNo,
                              OrderItemId = item.Id,
                              ReviewParameter = data.ReviewParameter,
                              IsMandatoryTrue = data.IsMandatoryTrue
                         });
                    }
                    contractReviewItems.Add(new ContractReviewItem
                    {
                         OrderItemId = item.Id,
                         OrderId = item.OrderId,
                         Quantity = item.Quantity,
                         CategoryName = item.CategoryName,
                         Ecnr = item.Ecnr,
                         RequireAssess = item.RequireAssess,
                         SourceFrom = item.SourceFrom,
                         ReviewItems = itemData
                    });
               }

               if (string.IsNullOrEmpty(order.CustomerName)) order.CustomerName = await _context.Customers.Where(x => x.Id == order.CustomerId).Select(x => x.CustomerName).FirstOrDefaultAsync();
               contractReview = new ContractReview
               {
                    OrderNo = order.OrderNo,
                    OrderId = orderId,
                    OrderDate = order.OrderDate.Date,
                    CustomerId = order.CustomerId,
                    CustomerName = order.CustomerName,
                    ReviewedBy = loggedInEmployeeId,
                    ReviewedOn = System.DateTime.Now,
                    ContractReviewItems = contractReviewItems
               };

               _unitOfWork.Repository<ContractReview>().Add(contractReview);

               if (await _unitOfWork.Complete() > 0) return contractReview;

               throw new Exception("failed to create the Contract Review object");
          }

     
     }
}