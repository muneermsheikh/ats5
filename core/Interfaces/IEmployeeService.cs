using System.Threading.Tasks;
using core.Entities.Admin;
using core.Params;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IEmployeeService
    {
         Task<bool> EditEmployee(Employee employee);
         Task<bool> DeleteEmployee(Employee employee);
         Task<Employee> AddNewEmployee(Employee employee);
         Task<Pagination<Employee>> GetEmployeePaginated(EmployeeSpecParams empParams);
    }
}