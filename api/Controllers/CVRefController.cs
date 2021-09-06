using System.Collections.Generic;
using System.Threading.Tasks;
using api.Errors;
using api.Extensions;
using AutoMapper;
using core.Entities.EmailandSMS;
using core.Entities.HR;
using core.Entities.Identity;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace api.Controllers
{
     public class CVRefController : BaseApiController
     {
          private readonly ICVRefService _cvrefService;
          private readonly IUnitOfWork _unitOfWork;
          private readonly IMapper _mapper;
          private readonly ITaskService _taskService;
          private readonly IConfiguration _config;
          private readonly UserManager<AppUser> _userManager;
          private readonly IEmployeeService _empService;
          public CVRefController(ICVRefService cvrefService, IUnitOfWork unitOfWork, IEmployeeService empService,
          IMapper mapper, ITaskService taskService, IConfiguration config, UserManager<AppUser> userManager)
          {
               _empService = empService;
               _userManager = userManager;
               _config = config;
               _taskService = taskService;
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

          [Authorize]
          [HttpPost]
          public async Task<ActionResult<ICollection<EmailMessage>>> MakeReferrals(ICollection<int> CVReviewIds)
          {
               var loggedInDto = await GetLoggedInUserDto();
               if (loggedInDto == null) return Unauthorized(new ApiResponse(401, "this option requires logged in User"));
               /*
               if (!User.IsInRole("Admin")) {
                    if (!User.IsInRole("DocControllerAdminRole")) return Unauthorized(new ApiResponse(401, "Only the Administrator or the Document Controller has the privilege to send CVs to clients"));
               }
               */
               var msgs = await _cvrefService.MakeReferralsAndCreateTask(CVReviewIds, loggedInDto);
               return Ok(msgs);
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

          [HttpGet("cvsreadytoforward")]
          [Authorize]
          public async Task<ActionResult<ICollection<CustomerReferralsPendingDto>>> CustomerReferralsPending()
          {
               var loggedInDto = await GetLoggedInUserDto();
               if (loggedInDto == null) return Unauthorized(new ApiResponse(401, "this option requires logged in User"));
               /*
               if (!loggedInDto.HasAdminPrivilege) {
                    if (!User.IsInRole("DocControllerAdminRole")) return Unauthorized(new ApiResponse(401, "Only the Administrator or the Document Controller has the privilege to send CVs to clients"));
               }
               */

               var pendings = await _cvrefService.CustomerReferralsPending(0);

               if (pendings==null && pendings.Count == 0) return NotFound(new ApiResponse(402, "No CVs pending for forwarding to customers"));

               return Ok(pendings);

          }
          private async Task<LoggedInUserDto> GetLoggedInUserDto()
          {
               var loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
               if (loggedInUser == null) return null;

               var empId = await _empService.GetEmployeeIdFromAppUserIdAsync(loggedInUser.Id);
               var loggedInUserDto = new LoggedInUserDto
               {
                    LoggedIAppUsername = loggedInUser.UserName,
                    LoggedInAppUserEmail = loggedInUser.Email,
                    LoggedInAppUserId = loggedInUser.Id,
                    LoggedInEmployeeId = empId,
                    HasAdminPrivilege = User.IsInRole("Admin")
               };
               return loggedInUserDto;
          }
     }
}