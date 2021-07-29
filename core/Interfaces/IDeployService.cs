using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.HR;
using core.Entities.Process;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IDeployService
    {
        Task<IReadOnlyList<CommonDataDto>> GetPendingDeployments();
        Task<ICollection<CVRef>> GetDeploymentsOfOrderItemId(int orderItemId);
        Task<ICollection<CVRef>> GetDeploymentsOfACandidate(int candidateId);
        Task<CVRef> GetDeploymentsById(int cvrefid);
        Task<CVRef> GetDeploymentsByCandidateAndOrderItem(int candidateId, int orderItemId);
        Task<bool> AddDeploymentTransaction (Deploy deploy);
        Task<bool> EditDeploymentTransaction (Deploy deploy);
        Task<bool> DeleteDeploymentTransactions (Deploy deploy);
        
    }
}