using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.HR;
using core.Entities.Process;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface ICommonServices
    {
         //order and order items
         Task<string> CategoryNameFromCategoryId(int categoryId);
         Task<string> CustomerNameFromOrderDetailId(int orderDetailId);
         Task<CustomerBriefDto> CustomerBriefDetailsFromCustomerId(int customerId);
         Task<string> DeploymentStageNameFromStageId(EnumDeployStatus stageId);
         Task<CommonDataDto> CommonDataFromCVRefId(int cvrefid);
         Task<CommonDataDto> CommonDataFromOrderDetailIdAndCandidateId(int CVReviewId);
         Task<CommonDataDto> CommonDataFromOrderDetailIdAndCandidateId(int OrderItemId, int CandidateId);
         Task<CommonDataForCVRefDto> CommonDataForCVRefFromOrderItemAndCandidateId(int OrderItemId, int candidateId);
         Task<CommonDataDto> CommonDataFromOrderItemCandidateIdWithChecklistId(int OrderItemId, int candidateId);
         Task<ICollection<SelectionDecisionToRegisterDto>> PopulateSelectionDecisionsToRegisterDto(ICollection<SelectionDecisionToRegisterDto> dto);
         Task<Employment> PopulateEmploymentFromCVRefId(int cvrefid, int salary, int charges, DateTime selectedOn);
         Task<CommonDataDto> PendingDeployments();
         Task<OrderAssignmentDto> GetOrderAssignmentDto(int orderId);
    }
}