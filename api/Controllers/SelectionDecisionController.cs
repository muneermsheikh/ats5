using System.Collections.Generic;
using System.Threading.Tasks;
using api.Errors;
using core.Entities.HR;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class SelectionDecisionController : BaseApiController
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ISelectionDecisionService _service;
          public SelectionDecisionController(IUnitOfWork unitOfWork, ISelectionDecisionService service)
          {
               _service = service;
               _unitOfWork = unitOfWork;
          }

          [HttpGet]
          public async Task<ActionResult<Pagination<SelectionDecision>>> GetSelectionDecisions(SelDecisionSpecParams selDecisionParams)
          {
               var spec = new SelectionDecisionSpecs(selDecisionParams);
               var specCount = new SelectionDecisionForCountSpecs(selDecisionParams);
               var decisions = await _unitOfWork.Repository<SelectionDecision>().ListAsync(spec);
               var ct = await _unitOfWork.Repository<SelectionDecision>().CountAsync(specCount);

               return Ok(new Pagination<SelectionDecision>(selDecisionParams.PageIndex,
                   selDecisionParams.PageSize, ct, decisions));
          }


          [HttpPost]
          public async Task<ActionResult<IReadOnlyList<SelectionDecision>>> RegisterSelectionDecisions(ICollection<SelectionDecisionToRegisterDto> dtos)
          {
               var decs = await _service.RegisterSelections(dtos);

               if (decs != null) return Ok(decs);

               return BadRequest(new ApiResponse(400, "failed to update the selections"));
          }

          [HttpPut]
          public async Task<ActionResult<bool>> EditSelectionDecision(SelectionDecision selectionDecision)
          {
               return await _service.EditSelection(selectionDecision);
          }

          [HttpDelete]
          public async Task<ActionResult<bool>> DeleteSelectionDecision(SelectionDecision selectionDecision)
          {
               return await _service.DeleteSelection(selectionDecision);
          }
     }
}