using System;
using core.Entities.Orders;

namespace core.Specifications
{
    public class OrdersWithItemsAndOrderingForCountSpecs: BaseSpecification<Order>
    {
        public OrdersWithItemsAndOrderingForCountSpecs(int id) : base(o => o.Id == id)
        {
        }

        public OrdersWithItemsAndOrderingForCountSpecs(string customerid) 
            : base(o => o.CustomerId == Convert.ToInt32(customerid))
        {
        }
        public OrdersWithItemsAndOrderingForCountSpecs()             
        {
        }
    }
}