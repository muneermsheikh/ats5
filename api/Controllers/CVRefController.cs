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
     public class CVRefController : BaseApiController
     {
          private readonly ICVRefService _cvrefService;
          private readonly IUnitOfWork _unitOfWork;
          private readonly IMapper _mapper;
          public CVRefController(ICVRefService cvrefService, IUnitOfWork unitOfWork, IMapper mapper)
          {
               _mapper = mapper;
               _unitOfWork = unitOfWork;
               _cvrefService = cvrefService;
          }

          [HttpGet("orderitem/{orderitemid}")]
          public async Task<ActionResult<ICollection<CVRef>>> GetReferralsOfOrderItemId(int orderitemid)
          {
               var refs = await _cvrefService.GetReferralsOfOrderItemId(orderitemid);
               if (refs == null) return NotFound(new ApiResponse(404, "No record found"));
               return Ok(refs);
          }

          [HttpGet("referralsofcandidate/{candidateid}")]
          public async Task<ActionResult<ICollection<CVRef>>> GetReferralsOfACandidate(int candidateid)
          {
               var refs = await _cvrefService.GetReferralsOfACandidate(candidateid);
               if (refs == null) return NotFound(new ApiResponse(404, "No record found"));
               return Ok(refs);
          }

          [HttpGet("cvref/{cvrefid}")]
          public async Task<ActionResult<CVRef>> GetCVRef(int cvrefid)
          {
               var cvref = await _cvrefService.GetReferralById(cvrefid);
               if (cvref == null)
               {
                    return NotFound(new ApiResponse(404, "Not Found"));
               }
               else
               {
                    return Ok(cvref);
               }
          }

          [HttpGet("{candidateid}/{orderitemid}")]
          public async Task<ActionResult<CVRef>> GetReferralsOfCandidateAndOrderItem(int candidateid, int orderitemid)
          {
               var cvref = await _cvrefService.GetReferralByCandidateAndOrderItem(candidateid, orderitemid);
               if (cvref == null) return NotFound(new ApiResponse(404, "No record found"));
               return Ok(cvref);
          }

          [HttpPost]
          public async Task<ActionResult<CVRefPostedDto>> MakeAReferral(CVRefToAddDto dto)
          {
               var cv = await _cvrefService.PostReferral(dto);
               return _mapper.Map<CVRef, CVRefPostedDto>(cv);
          }

          [HttpPut]
          public async Task<ActionResult<bool>> EditAReferral(CVRef cvref)
          {
               return await _cvrefService.EditReferral(cvref);
          }

          [HttpGet]
          public async Task<ActionResult<Pagination<SelectionsPendingDto>>> GetSelectionDecisions(SelectionsPendingParams pendingParams)
          {
               var spec = new SelectionsPendingSpecs(pendingParams);
               var specCount = new SelectionsPendingForCountSpecs(pendingParams);
               var decisions = await _unitOfWork.Repository<CVRef>().ListAsync(spec);
               var ct = await _unitOfWork.Repository<CVRef>().CountAsync(specCount);
               var mappedToDto = _mapper.Map<IReadOnlyList<CVRef>, IReadOnlyList<SelectionsPendingDto>>(decisions);
               return Ok(new Pagination<SelectionsPendingDto>(pendingParams.PageIndex,
                   pendingParams.PageSize, ct, mappedToDto));

          }
     }
}