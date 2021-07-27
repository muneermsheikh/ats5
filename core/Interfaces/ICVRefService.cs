using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.HR;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface ICVRefService
    {
        Task<ICollection<CVRef>> GetReferralsOfOrderItemId(int orderItemId);
        Task<ICollection<CVRef>> GetReferralsOfACandidate(int candidateId);
        Task<CVRef> GetReferralById(int cvrefid);
        Task<CVRef> GetReferralByCandidateAndOrderItem(int candidateId, int orderItemId);
        Task<CVRef> PostReferral (CVRefToAddDto dto);
        Task<bool> EditReferral (CVRef cvref);
        Task<bool> DeleteReferral (CVRef cvref);
    }
}