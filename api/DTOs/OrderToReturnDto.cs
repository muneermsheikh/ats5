using System;
using System.Collections.Generic;
using core.Entities.Orders;

namespace api.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string CustomerName {get; set;}
        public string CityOfEmployment {get; set;}
        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }
        public int Subtotal { get; set; }
        public EnumOrderStatus Status { get; set; }
        public int Total { get; set; }
    }
}