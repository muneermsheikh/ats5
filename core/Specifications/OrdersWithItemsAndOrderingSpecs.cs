using System;
using core.Entities.Orders;

namespace core.Specifications
{
    public class OrdersWithItemsAndOrderingSpecs: BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingSpecs(int id) : base(o => o.Id == id)
        {
            AddInclude(o => o.OrderItems);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpecs(string customerid) 
            : base(o => o.CustomerId == Convert.ToInt32(customerid))
        {
            AddInclude(o => o.OrderItems);
        }
        public OrdersWithItemsAndOrderingSpecs()             
        {
            AddInclude(o => o.OrderItems);
            AddOrderBy(o => o.OrderNo);
        }

    }
}