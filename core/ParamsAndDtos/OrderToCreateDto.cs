using System;
using System.Collections.Generic;
using core.Entities.Orders;

namespace core.Interfaces
{
     public class OrderToCreateDto
     {
          public DateTime OrderDate { get; set; }
          public string OrderRef { get; set; }
          public int CustomerId { get; set; }
          public DateTime CompleteBy { get; set; }
          public int? SalesmanId { get; set; }
          public string Remarks { get; set; }
          public ICollection<OrderItem> OrderItems { get; set; }
     }
}