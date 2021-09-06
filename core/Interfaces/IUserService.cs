using System.Threading.Tasks;
using core.Entities;
using core.Entities.Admin;
using core.Entities.EmailandSMS;
using core.Entities.Users;
using core.ParamsAndDtos;

namespace core.Interfaces
{
     public interface IUserService
    {
        Task<Candidate> CreateCandidateAsync(RegisterDto registerDto);
        Task<Employee> CreateEmployeeAsync(RegisterDto registerDto);
        Task<CustomerOfficial> CreateCustomerOfficialAsync(RegisterDto registerDto);
        Task<Candidate> GetCandidateByIdAsync(int id);
        Task<Candidate> GetCandidateBySpecsUserIdAsync(int userId);
        Task<Candidate> GetCandidateBySpecsIdentityIdAsync(string identityUserId);
    }
}