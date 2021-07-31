using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.HR;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface ICommonServices
    {
         Task<string> CategoryNameFromCategoryId(int categoryId);
         Task<string> CustomerNameFromOrderDetailId(int orderDetailId);
         Task<CommonDataDto> CommonDataFromCVRefId(int cvrefid);
         Task<CommonDataDto> CommonDataFromOrderDetailIdAndCandidateId(int orderDetailId, int candidateId);
         Task<ICollection<SelectionDecisionToRegisterDto>> PopulateSelectionDecisionsToRegisterDto(ICollection<SelectionDecisionToRegisterDto> dto);
         Task<Employment> PopulateEmploymentFromCVRefId(int cvrefid, int salary, int charges, DateTime selectedOn);
         Task<CommonDataDto> PendingDeployments();
    }
}