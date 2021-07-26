using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.HR;
using core.Interfaces;
using core.ParamsAndDtos;
using infra.Data;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class CVRefService : ICVRefService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ATSContext _context;
          public CVRefService(IUnitOfWork unitOfWork, ATSContext context)
          {
               _context = context;
               _unitOfWork = unitOfWork;
          }

        public async Task<bool> EditCVRef(CVRef cVRef)
        {
            var refStatus = await _context.CVRefs
                .Where(x => x.Id == cVRef.Id).Select(x => x.RefStatus)
                .FirstOrDefaultAsync();
            if (refStatus != EnumCVRefStatus.Referred) return false;    //ref status changed, so no edits

            _unitOfWork.Repository<CVRef>().Update(cVRef);

            return (await _unitOfWork.Complete() > 0);
        }

        public async Task<ICollection<CVRef>> GetReferralsOfOrderItemId(int orderItemId)
        {
            return await _context.CVRefs.Where(x => x.OrderItemId == orderItemId)
                .OrderBy(x => x.ReferredOn).ToListAsync();   
        }

        public async Task<CVRef> PostCVRef(CVRefToAddDto dto)
        {
            var restricted = await _context.CVRefRestrictions
                .Where(x => x.OrderItemId == dto.OrderItemId && x.RestrictionLifted != true)
                .Select(x => x.RestrictedOn)
                .FirstOrDefaultAsync();
            if (restricted != null) {
                throw new System.Exception("The Order category is barred for CV Referrals on " + restricted);
            }

            var cvref = new CVRef(dto.OrderItemId, dto.CandidateId, dto.Charges, dto.ReferredOn);
            _unitOfWork.Repository<CVRef>().Add(cvref);

            if (await _unitOfWork.Complete() > 0) {
                return await _context.CVRefs.Where(x => x.OrderItemId == dto.OrderItemId && x.CandidateId == dto.CandidateId)
                    .FirstOrDefaultAsync();
            } else {
                return null;
            }

        }
    }
}