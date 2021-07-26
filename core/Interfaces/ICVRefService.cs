using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.HR;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface ICVRefService
    {
        Task<ICollection<CVRef>> GetReferralsOfOrderItemId(int orderItemId);
        Task<CVRef> PostCVRef (CVRefToAddDto dto);
        Task<bool> EditCVRef (CVRef cVRef);
    }
}