using System.Threading.Tasks;
using core.Entities.HR;

namespace core.Interfaces
{
    public interface IEmploymentService
    {
         Task<Employment> AddEmployment (Employment employment);
         Task<bool> EditEmployment (Employment employment);
         Task<bool> DeleteEmployment (Employment employment);
         
    }
}