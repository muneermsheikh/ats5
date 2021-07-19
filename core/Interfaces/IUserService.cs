using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.Users;
using core.ParamsAndDtos;
using core.Specifications;

namespace core.Interfaces
{
    public interface IUserService
    {
        Task<Candidate> CreateCandidateAsync(CandidateToCreateDto dto);
        
        Task<Candidate> GetCandidateByIdAsync(int id);
        Task<Candidate> GetCandidateBySpecsUserIdAsync(int userId);
        Task<Candidate> GetCandidateBySpecsIdentityIdAsync(string identityUserId);
    }
}