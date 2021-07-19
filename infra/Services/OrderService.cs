using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.Identity;
using core.Entities.Orders;
using core.Interfaces;
using core.Specifications;
using infra.Data;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class OrderService : IOrderService
     {
          private readonly ATSContext _context;
          private readonly IUnitOfWork _unitOfWork;
          //private readonly IPaymentService _paymentService;
          public OrderService(IUnitOfWork unitOfWork
               //, IPaymentService paymentService
               , ATSContext context)
          {
               _context = context;
               //_paymentService = paymentService;
               _unitOfWork = unitOfWork;
          }

          public async Task<Order> CreateOrderAsync(OrderToCreateDto dto) 
          //DateTime orderDate, string orderRef, int customerId, 
                //DateTime completeBy, int? salesmanId, string remarks, ICollection<OrderItem> orderitems)
          {

               // get items from the product repo
               var subtotal = 0;
               var items = new List<OrderItem>();
               foreach (var item in dto.OrderItems)
               {
                    subtotal = items.Sum(item => item.Charges * item.Quantity) + (items.Sum(item => item.FeeFromClientINR * item.Quantity));
               }

               // create order
               var orderNo = await _context.Orders.MaxAsync(x => x.OrderNo);
               orderNo++;

              var order = new Order(orderNo, dto.CustomerId, dto.OrderRef, (int)dto.SalesmanId, subtotal, dto.CompleteBy, dto.OrderItems);

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

     }
}