using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using core.Entities.HR;
using core.Entities.Process;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;

namespace infra.Services
{
     public class SelectionDecisionService : ISelectionDecisionService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ICommonServices _commonServices;
          private readonly IDeployService _deployService;
          public SelectionDecisionService(IUnitOfWork unitOfWork, ICommonServices commonServices, IDeployService deployService)
          {
               _deployService = deployService;
               _commonServices = commonServices;
               _unitOfWork = unitOfWork;
          }

          public async Task<bool> DeleteSelection(SelectionDecision selectionDecision)
          {
               _unitOfWork.Repository<SelectionDecision>().Delete(selectionDecision);
               return await _unitOfWork.Complete() > 0;
          }

          public async Task<bool> EditSelection(SelectionDecision selectionDecision)
          {
               _unitOfWork.Repository<SelectionDecision>().Update(selectionDecision);
               return await _unitOfWork.Complete() > 0;
          }

          public async Task<Pagination<SelectionDecision>> GetSelectionDecisions (SelDecisionSpecParams specParams)
          {
               var spec = new SelectionDecisionSpecs(specParams);
               var specCount = new SelectionDecisionForCountSpecs(specParams);
               var decisions = await _unitOfWork.Repository<SelectionDecision>().ListAsync(spec);
               var ct = await _unitOfWork.Repository<SelectionDecision>().CountAsync(specCount);

               return new Pagination<SelectionDecision>(specParams.PageIndex, specParams.PageSize, ct, decisions);

          }

          public async Task<IReadOnlyList<SelectionDecision>> RegisterSelections(ICollection<SelectionDecisionToRegisterDto> selectionDecisions)
          {
               var updatedRegisterDto = await _commonServices.PopulateSelectionDecisionsToRegisterDto(selectionDecisions);
               if (updatedRegisterDto == null || updatedRegisterDto.Count == 0) return null;
               foreach (var dto in updatedRegisterDto)
               {
                    var employment = await _commonServices.PopulateEmploymentFromCVRefId(dto.CVRefId, dto.Salary, dto.Charges, dto.DecisionDate);
                    var selDecision = new SelectionDecision(dto.CVRefId, dto.OrderItemId, dto.CategoryId, dto.CategoryName,
                        employment.OrderId, dto.OrderNo, dto.ApplicationNo, dto.CandidateId, dto.CandidateName, dto.DecisionDate,
                        dto.SelectionStatusId, dto.Remarks, employment);
                    
                    var deployTrans = new Deploy(dto.CVRefId, dto.DecisionDate, (int)EnumDeployStatus.Selected);
                    await _deployService.AddDeploymentTransaction(deployTrans);

                    _unitOfWork.Repository<SelectionDecision>().Add(selDecision);

                    //_unitOfWork.Repository<Employment>().Add(employment);
                    //todo - issue advisories to candidates and hr staff, accounts, process
               }

               if (await _unitOfWork.Complete() > 0) {
                    var lst = selectionDecisions.Select(x => x.CVRefId).ToArray();
                    var sparams = new SelDecisionSpecParams{CVRefIds = lst};
                    var specs = new SelectionDecisionSpecs(sparams);
                    var data = await _unitOfWork.Repository<SelectionDecision>().ListAsync(specs);
                    return  data;
               } else {
                    return null;
               }

          }
     }
}