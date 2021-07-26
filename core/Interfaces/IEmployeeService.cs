using System.Threading.Tasks;
using core.Entities.Admin;

namespace core.Interfaces
{
    public interface IEmployeeService
    {
         void EditEmployee(Employee employee);
         void DeleteEmployee(Employee employee);
    }
}