using System.Threading.Tasks;
using api.Errors;
using api.Extensions;
using core.Entities.HR;
using core.Entities.Identity;
using core.Interfaces;
using core.ParamsAndDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     [Authorize]
     public class CandidateAssessmentController : BaseApiController
     {
          private readonly UserManager<AppUser> _userManager;

          private readonly ICandidateAssessmentService _candidateAssessService;
          public CandidateAssessmentController(ICandidateAssessmentService candidateAssessService, UserManager<AppUser> userManager)
          {
               _userManager = userManager;
               _candidateAssessService = candidateAssessService;
          }


          [HttpPost("assess")]
          [Authorize(Policy = "HRExecutiveRole, HRSupervisorRole, HRManagerRole")]
          public async Task<ActionResult<CandidateAssessment>> AssessNewCandidate(CandidateAssessmentParams assessParams)
          {
               if (!User.IsUserAuthenticated()) return Unauthorized("user is not authenticated");
               //var userid = User.GetIdentityUserId();
               //if(string.IsNullOrEmpty(userid)) return Unauthorized("this function requires authorization");
                var identityuserid  = Identityuserid();
                assessParams.LoggedInIdentityUserId = identityuserid;

               return await _candidateAssessService.AssessNewCandidate(assessParams);
          }

          private string Identityuserid()
          {
               var email = User.GetIdentityUserEmailId();
               var appuser = _userManager.FindByEmailAsync(email);
               return appuser.Id.ToString();
               //return appuser.IdentityUser();
          }
          
          [Authorize(Policy = "HRExecutiveRole, HRSupervisorRole, HRManagerRole")]
          [HttpPut("assess")]
          public async Task<ActionResult<bool>> EditCandidateAssessment(CandidateAssessment candidateAssessment)
          {
               if (!await _candidateAssessService.EditCandidateAssessment(candidateAssessment))
               {
                    return BadRequest(new ApiResponse(400, "failed to edit the candidate assessment"));
               }
               else
               {
                    return Ok();
               }
          }

          [Authorize(Policy = "HRExecutiveRole, HRSupervisorRole, HRManagerRole")]
          [HttpDelete("assess")]
          public async Task<ActionResult<bool>> DeleteCandidateAssessment(CandidateAssessment candidateAssessment)
          {
               if (!await _candidateAssessService.DeleteCandidateAssessment(candidateAssessment))
               {
                    return BadRequest(new ApiResponse(400, "failed to delete the candidate assessment"));
               }
               else
               {
                    return Ok();
               }
          }

          [Authorize(Policy = "HRExecutiveRole, HRSupervisorRole, HRManagerRole")]
          [HttpDelete("assessitem")]
          public async Task<ActionResult<bool>> DeleteCandidateAssessmentItem(CandidateAssessmentItem assessmentItem)
          {
               if (!await _candidateAssessService.DeleteCandidateAssessmentItem(assessmentItem))
               {
                    return BadRequest(new ApiResponse(400, "failed to delete the candidate assessment item"));
               }
               else
               {
                    return Ok();
               }
          }

          [HttpGet("{orderItemId}/{candidateId}")]
          public async Task<ActionResult<CandidateAssessment>> GetCandidateAssessment(int orderItemId, int candidateId)
          {
               var assessment = await _candidateAssessService.GetCandidateAssessment(candidateId, orderItemId);
               if (assessment != null) {
                    return Ok(assessment);
               } else {
                    return BadRequest(new ApiResponse(400, "No Assessment data on record for the candidate and order category selected"));
               }
          }
     }
}