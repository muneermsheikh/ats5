using System;
using System.Collections.Generic;
using core.Entities.Identity;

namespace core.Entities.Orders
{
    public class Order: BaseEntity
    {
          private List<OrderItem> items;
          private string buyerEmail;
          private int subtotal;

          public Order()
          {
          }

          public Order(int orderNo, int customerId, string customerName, string cityOfWorking, string orderRef, int salesmanId, 
            int estimatedRevenue, DateTime completeBy, OrderAddress orderAddress,ICollection<OrderItem> orderItems)
          {
               OrderNo = orderNo;
               CustomerId = customerId;
               CustomerName = customerName;
               CityOfWorking = cityOfWorking;
               OrderAddress = orderAddress;
               OrderRef = orderRef;
               SalesmanId = salesmanId;
               CompleteBy = completeBy;
               EstimatedRevenue = estimatedRevenue;
               OrderItems = orderItems;
          }

        public int OrderNo { get; set; }
        public OrderAddress OrderAddress { get; set; }
        public DateTimeOffset OrderDate { get; set; }=DateTimeOffset.Now;
        public int CustomerId { get; set; }
        public string CustomerName {get; set;}
        public string BuyerEmail {get; set;}
        public string OrderRef { get; set; }
        public int? SalesmanId { get; set; }
        public string SalesmanName { get; set; }
        //public string AppUserId { get; set; }
        public DateTime CompleteBy { get; set; }
        public string Country { get; set; }
        public string CityOfWorking { get; set; }
        public int EstimatedRevenue {get; set;}
        public Customer Customer {get; set;}
        public EnumOrderStatus Status { get; set; } = EnumOrderStatus.AwaitingReview;
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}