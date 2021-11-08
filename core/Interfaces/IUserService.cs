using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities;
using core.Entities.Admin;
using core.Entities.EmailandSMS;
using core.Entities.Users;
using core.Params;
using core.ParamsAndDtos;

namespace core.Interfaces
{
     public interface IUserService
    {
        Task<Candidate> CreateCandidateAsync(RegisterDto registerDto);
        Task<Employee> CreateEmployeeAsync(RegisterDto registerDto);
        Task<CustomerOfficial> CreateCustomerOfficialAsync(RegisterDto registerDto);
        Task<Pagination<Candidate>> GetCandidates(CandidateSpecParams candidateParams);
        //Task<Candidate> GetCandidateByIdAsync(int id);
        Task<Candidate> UpdateCandidateAsync(Candidate candidate);
        //Task<Candidate> GetCandidateBySpecsUserIdAsync(int userId);
        //Task<Candidate> GetCandidateBySpecsIdentityIdAsync(int identityUserId);
        Task<ICollection<UserProfession>> EditUserProfessions(UserAndProfessions userProfessions);

    }
}