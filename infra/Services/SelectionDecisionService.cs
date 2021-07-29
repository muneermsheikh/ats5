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


          public async Task<bool> RegisterSelections(ICollection<SelectionDecisionToRegisterDto> selectionDecisions)
          {
               var updatedRegisterDto = await _commonServices.PopulateSelectionDecisionsToRegisterDto(selectionDecisions);
               foreach (var dto in updatedRegisterDto)
               {
                    var selDecision = new SelectionDecision(dto.CVRefId, dto.OrderItemId, dto.CategoryId, dto.CategoryName,
                        dto.OrderId, dto.OrderNo, dto.ApplicationNo, dto.CandidateId, dto.CandidateName, dto.DecisionDate,
                        dto.SelectionStatusId, dto.Remarks);

                    var deployTrans = new Deploy(dto.CVRefId, dto.DecisionDate, (int)EnumDeployStatus.Selected);
                    var employment = await _commonServices.PopulateEmploymentFromCVRefId(dto.CVRefId, dto.Salary, dto.Charges, dto.DecisionDate);
                    
                    _unitOfWork.Repository<SelectionDecision>().Add(selDecision);
                    _unitOfWork.Repository<Deploy>().Add(deployTrans);
                    _unitOfWork.Repository<Employment>().Add(employment);
                    //todo - issue advisories to candidates and hr staff, accounts, process
               }

               return await _unitOfWork.Complete() > 0;

          }
     }
}