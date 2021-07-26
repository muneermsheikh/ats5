using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.Orders;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IOrderService
        {
            Task<Order> CreateOrderAsync(OrderToCreateDto dto);
            void EditOrder(Order order);
            void DeleteOrder(Order order);
            Task<IReadOnlyList<Order>> GetOrdersForUserAsync(int customerId);
            Task<IReadOnlyList<Order>> GetOrdersAllAsync();
            Task<Order> GetOrderByIdAsync(int id);
        //order items
            Task<IReadOnlyList<OrderItem>> GetOrderItemsByOrderIdAsync(int OrderId);
            Task<OrderItem> GetOrderItemByOrderItemIdAsync(int Id);
            
            void AddOrderItem(OrderItem orderItem);
            void EditOrderItem(OrderItem orderItem);
            bool EditOrderItemWithNavigationObjects(OrderItem modelItem);
            void DeleteOrderItem (OrderItem orderItem);

        //Job descriptions
            Task<ICollection<JobDescription>> GetJobDescriptionsByOrderIdAsync(int Id);
            Task<JobDescription> GetJobDescriptionByOrderItemIdAsync(int Id);
            
            void AddJobDescription(JobDescription jobDescription);
            void EditJobDescription(JobDescription jobDescription);
            void DeleteJobDescription (JobDescription jobDescription);

    // Remuneations
            Task<RemunerationDto> GetRemunerationsByOrderIdAsync(int Id);
            Task<Remuneration> GetRemunerationOfOrderItemAsync(int Id);
            
            void AddRemuneration(Remuneration remuneration);
            void EditRemuneration(Remuneration remuneration);
            void DeleteRemuneration (Remuneration remuneration);
    }
}