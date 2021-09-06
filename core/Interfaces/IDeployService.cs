using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.HR;
using core.Entities.Process;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IDeployService
    {
        Task<IReadOnlyList<DeploymentPendingDto>> GetPendingDeployments();
        Task<int> CountOfPendingDeployments();
        Task<ICollection<CVRef>> GetDeploymentsOfOrderItemId(int orderItemId);
        Task<ICollection<CVRef>> GetDeploymentsOfACandidate(int candidateId);
        Task<CVRef> GetDeploymentsById(int cvrefid);
        Task<CVRef> GetDeploymentsByCandidateAndOrderItem(int candidateId, int orderItemId);
        Task<DeployAddedDto> AddDeploymentTransaction(int cvrefId, int loggedInEmployeeId, EnumDeployStatus stageId, DateTime? transDate);
        Task<ICollection<DeployAddedDto>> AddDeploymentTransactions(ICollection<DeployPostDto> deployPostsDto, int loggedInEmployeeId);
        Task<bool> EditDeploymentTransaction (Deploy deploy);
        Task<bool> DeleteDeploymentTransactions (Deploy deploy);
        
    }
}