using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface ICustomerService
    {
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        Task<CustomerDto> AddCustomer (RegisterCustomerDto dto);
        void EditCustomer(Customer customer);
        Task<ICollection<CustomerDto>> GetCustomersAsync(string userType);
        Task<ICollection<CustomerDto>> GetCustomersPaginatedAsync(CustomerParams custParam);
        Task<CustomerDto> GetCustomerByIdAsync(int id);
        Task<CustomerDto> GetCustomerByUserNameAsync(string username);
        Task<string> GetCustomerNameFromId (int Id);
        Task<ICollection<CustomerIdAndNameDto>> GetCustomerIdAndName (string customerType);
        Task<ICollection<CustomerIdAndNameDto>> GetCustomerIdAndNames(ICollection<int> customerIds);
        Task<bool> CustomerExistsByIdAsync(int id);
    }
}