using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities;
using core.Entities.Admin;
using core.Entities.Identity;
using core.Entities.Users;
using core.ParamsAndDtos;
using core.Specifications;

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