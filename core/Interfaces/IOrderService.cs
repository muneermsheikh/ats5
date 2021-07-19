using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.Orders;

namespace core.Interfaces
{
    public interface IOrderService
        {
            Task<Order> CreateOrderAsync(OrderToCreateDto dto);
            
            Task<IReadOnlyList<Order>> GetOrdersForUserAsync(int customerId);
            Task<IReadOnlyList<Order>> GetOrdersAllAsync();
            Task<Order> GetOrderByIdAsync(int id);
           
    }
}