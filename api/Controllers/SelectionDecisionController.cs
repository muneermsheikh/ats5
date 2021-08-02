using System.Collections.Generic;
using System.Threading.Tasks;
using api.Errors;
using AutoMapper;
using core.Entities.HR;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class SelectionDecisionController : BaseApiController
     {
          private readonly ISelectionDecisionService _service;
          private readonly IMapper _mapper;
          public SelectionDecisionController(ISelectionDecisionService service, IMapper mapper)
          {
               _mapper = mapper;
               _service = service;
          }

          [HttpGet]
          public async Task<ActionResult<Pagination<SelectionDecision>>> GetSelectionDecisions(SelDecisionSpecParams selDecisionParams)
          {
               var decs = await _service.GetSelectionDecisions(selDecisionParams);
               if (decs != null) return Ok(decs);
               return NotFound(new ApiResponse(404, "no records found"));
          }

          [HttpPost]
          public async Task<ActionResult<IReadOnlyList<SelectionDecisionToReturnDto>>> RegisterSelectionDecisions(ICollection<SelectionDecisionToRegisterDto> dtos)
          {
               var decs = await _service.RegisterSelections(dtos);

               if (decs != null) return Ok(_mapper.Map<
                    IReadOnlyList<SelectionDecision>, IReadOnlyList<SelectionDecisionToReturnDto>>(decs));

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