using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using core.Entities.Orders;

namespace core.ParamsAndDtos
{
     public class OrderToCreateDto
     {
          [Required]
          public DateTime OrderDate { get; set; }
          public string OrderRef { get; set; }
          [Required]
          public int CustomerId { get; set; }
          public string CustomerName { get; set; }
          public string CityOfEmployment { get; set; }
          public DateTime CompleteBy { get; set; }
          public int? SalesmanId { get; set; }
          public OrderAddress OrderAddress { get; set; }
          public string Remarks { get; set; }
          public ICollection<OrderItemDto> OrderItems { get; set; }
     }
}