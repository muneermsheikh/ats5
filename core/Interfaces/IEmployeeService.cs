using System.Collections.Generic;
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
         Task<ICollection<Employee>> AddNewEmployees(ICollection<EmployeeToAddDto> employees);
         Task<Pagination<Employee>> GetEmployeePaginated(EmployeeSpecParams empParams);
         Task<EmployeeDto> GetEmployeeFromIdAsync(int employeeId);
         Task<int> GetEmployeeIdFromAppUserIdAsync(int appUserId);
         Task<EmployeeDto> GetEmployeeBriefAsyncFromAppUserId(int appUserId);
        Task<EmployeeDto> GetEmployeeBriefAsyncFromEmployeeId(int id);
        Task<string> GetEmployeeNameFromEmployeeId(int id);
    }
}