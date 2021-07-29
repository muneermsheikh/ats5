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
          private readonly ICommonServices _commonService;
          public CVRefService(IUnitOfWork unitOfWork, ATSContext context, ICommonServices commonService)
          {
               _commonService = commonService;
               _context = context;
               _unitOfWork = unitOfWork;
          }

          public async Task<bool> EditReferral(CVRef cVRef)
          {
               var refStatus = await _context.CVRefs
               .Where(x => x.Id == cVRef.Id).Select(x => x.RefStatus)
               .FirstOrDefaultAsync();

               if (refStatus != EnumCVRefStatus.Referred) return false;    //ref status changed, so no edits

               _unitOfWork.Repository<CVRef>().Update(cVRef);

               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<CVRef> GetReferralById(int cvrefid)
          {
               return await _context.CVRefs.FindAsync(cvrefid);
          }
          public async Task<CVRef> GetReferralByCandidateAndOrderItem(int candidateId, int orderItemId)
          {
               return await _context.CVRefs.Where(x => x.CandidateId == candidateId && x.OrderItemId == orderItemId)
                   .FirstOrDefaultAsync();
          }

          public async Task<ICollection<CVRef>> GetReferralsOfACandidate(int candidateId)
          {
               return await _context.CVRefs.Where(x => x.CandidateId == candidateId)
                   .OrderBy(x => x.ReferredOn)
                   .ToListAsync();
          }

          public async Task<ICollection<CVRef>> GetReferralsOfOrderItemId(int orderItemId)
          {
               return await _context.CVRefs.Where(x => x.OrderItemId == orderItemId)
                   .OrderBy(x => x.ReferredOn).ToListAsync();
          }


          public async Task<CVRef> PostReferral(CVRefToAddDto dto)
          {
               var restricted = await _context.CVRefRestrictions
                   .Where(x => x.OrderItemId == dto.OrderItemId && x.RestrictionLifted != true)
                   .Select(x => x.RestrictedOn)
                   .FirstOrDefaultAsync();
               /* 
               if (restricted.Date > new System.DateTime(2000,1,1)) {
                   throw new System.Exception("The Order category is barred for CV Referrals on " + restricted);
               }
               */

               var commonData = await _commonService.CommonDataFromOrderDetailIdAndCandidateId(dto.OrderItemId, dto.CandidateId);
               var cvref = new CVRef(dto.OrderItemId, commonData.CategoryId, commonData.OrderId, commonData.OrderNo,
                    commonData.CustomerName, commonData.CategoryName, dto.CandidateId, commonData.ApplicationNo,
                    commonData.CandidateName, dto.ReferredOn, dto.Charges);
               
               _unitOfWork.Repository<CVRef>().Add(cvref);

               if (await _unitOfWork.Complete() > 0)
               {
                    return await _context.CVRefs.Where(x => x.OrderItemId == dto.OrderItemId && x.CandidateId == dto.CandidateId)
                    .FirstOrDefaultAsync();
               }
               else
               {
                    return null;
               }
          }

          public async Task<bool> DeleteReferral(CVRef cvref)
          {
               if (await _context.Deploys.Where(x => x.CVRefId == cvref.Id).ToListAsync() != null)
               {
                    throw new System.Exception("The referral has related records in Deployments");
               }
               _unitOfWork.Repository<CVRef>().Delete(cvref);
               return await _unitOfWork.Complete() > 0;
          }
     }
}