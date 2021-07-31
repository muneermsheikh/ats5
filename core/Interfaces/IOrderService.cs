using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.Orders;
using core.Params;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IOrderService
        {
            Task<Order> CreateOrderAsync(OrderToCreateDto dto);
            Task<bool> EditOrder(Order order);
            Task<bool> DeleteOrder(Order order);
            Task<Pagination<OrderToReturnDto>> GetOrdersAllAsync(OrdersSpecParams orderSpecParams);

        //order items
            Task<IReadOnlyList<OrderItem>> GetOrderItemsByOrderIdAsync(int OrderId);
            Task<OrderItem> GetOrderItemByOrderItemIdAsync(int Id);
            
            void AddOrderItem(OrderItem orderItem);
            void EditOrderItem(OrderItem orderItem);
            bool EditOrderItemWithNavigationObjects(OrderItem modelItem);
            Task<bool> DeleteOrderItem (OrderItem orderItem);

        //Job descriptions
            Task<ICollection<JobDescription>> GetJobDescriptionsByOrderIdAsync(int Id);
            Task<JobDescription> GetJobDescriptionByOrderItemIdAsync(int Id);
            
            void AddJobDescription(JobDescription jobDescription);
            void EditJobDescription(JobDescription jobDescription);
            void DeleteJobDescription (JobDescription jobDescription);

    // Remuneations
            Task<IReadOnlyList<Remuneration>> GetRemunerationsByOrderIdAsync(int Id);
            Task<Remuneration> GetRemunerationOfOrderItemAsync(int Id);
            
            Task<Remuneration> AddRemuneration(Remuneration remuneration);
            Task<bool> EditRemuneration(Remuneration remuneration);
            Task<bool> DeleteRemuneration (Remuneration remuneration);
    }
}