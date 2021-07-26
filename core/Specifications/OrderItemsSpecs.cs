using System;
using core.Entities.Orders;

namespace core.Specifications
{
    public class OrderItemsSpecs: BaseSpecification<OrderItem>
    {
        public OrderItemsSpecs(int id) : base(o => o.Id == id)
        {
        }

        public OrderItemsSpecs(int orderid, int dummy) 
            : base(o => o.OrderId == orderid)
        {
        }
    }
}