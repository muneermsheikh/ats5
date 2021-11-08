using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Errors;
using core.Entities.HR;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class InterviewController : BaseApiController
     {
          private readonly IInterviewService _interviewService;
          public InterviewController(IInterviewService interviewService)
          {
               _interviewService = interviewService;
          }

          [HttpPost]
          public async Task<ActionResult<Interview>> AddInterview(InterviewToAddDto dto)
          {
               if (dto.OrderId == 0 || dto.InterviewDateFrom.Year < 2000 || dto.InterviewDateUpto.Year < 2000 
                    || string.IsNullOrEmpty(dto.InterviewVenue)) return BadRequest(new ApiResponse(404, "incomplete data provided"));
               
               if (dto.InterviewDateUpto < dto.InterviewDateFrom) return BadRequest(new ApiResponse(404, "Interview Starting Date should be earlier than ending date"));
               var intvw = await _interviewService.AddInterview(dto);

               if (intvw == null) return BadRequest(new ApiResponse(404, "Bad Request - failed to save the Interview Data"));

               return intvw;
          }

          [HttpPut]
          public async Task<ActionResult<Interview>> EditInterview ([FromQuery] Interview interview)
          {
               var intvw = await _interviewService.EditInterview(interview);

               if (intvw == null) return BadRequest(new ApiResponse(404, "Failed to update the interview data"));

               return intvw;
          }

          [HttpGet("{interviewStatus}")]
          public async Task<ActionResult<ICollection<Interview>>> GetInterviews(string interviewStatus )
          {
               var interviews = await _interviewService.GetInterviews(interviewStatus);
               if (interviews == null) return NotFound(new ApiResponse(404, "No interviews found matching the status"));

               return Ok(interviews);
          }
          
          [HttpGet("openInterviews")]
          public async Task<ActionResult<ICollection<InterviewDto>>> GetAllOpenInterviews()
          {
               var interviews = await _interviewService.GetOpenInterviews();
               if (interviews == null) return NotFound(new ApiResponse(404, "No open interviews found"));

               return Ok(interviews);
          }

          [HttpGet("interviewsWithItems/{interviewStatus}")]
          public async Task<ActionResult<ICollection<Interview>>> GetInterviewsWithItems(string interviewStatus )
          {
               var interviews = await _interviewService.GetInterviews(interviewStatus);
               if (interviews == null) return NotFound(new ApiResponse(404, "No interviews found matching the status"));

               return Ok(interviews);
          }

          [HttpPost("assigncandidates")]
          public async Task<ActionResult<ICollection<InterviewItemCandidate>>> AssignCandidatesToInterviewItem(AssignCandidatesToAddDto dto)
          {
               if (dto.CandidateIds.Count == 0 ) return BadRequest(new ApiResponse(404, "No data provided"));

               int durationinminutes = 25;

               var candidatesAssigned = await _interviewService.AddCandidatesToInterviewItem(dto.InterviewItemId, dto.ScheduledTimeFrom,
                    durationinminutes, dto.InterviewMode, dto.CandidateIds);

               if (candidatesAssigned == null) return BadRequest(new ApiResponse(404, "failed to assign the candidates"));

               return Ok(candidatesAssigned);

          }
     
          [HttpDelete("deleteCandidateAssignments")]
          public async Task<ActionResult<bool>> DeleteCandidateAssignedToInterview ([FromQuery] List<int> candidateInterviewIds)
          {
               if (candidateInterviewIds.Count == 0) return BadRequest(new ApiResponse(404, "Record Ids not provided"));
               return await _interviewService.DeleteFromInterviewItemCandidates(candidateInterviewIds);
          }
          
          [HttpPut("registerCandidateArrived/{candidateInterviewId}")]
          public async Task<ActionResult<bool>> RegisterCandidateArrivedForInterview(int candidateInterviewId, DateTime reportedAt)
          {
               if (candidateInterviewId == 0) return BadRequest(new ApiResponse(404, "Record Id cannot be 0"));

               return await _interviewService.RegisterCandidateReportedForInterview(candidateInterviewId, reportedAt);
          }

          [HttpPut("registerCandidateInterviewed/{candidateInterviewId}/{interviewMode}/{interviewedAt}")]
          public async Task<ActionResult<bool>> RegisterCandidateAsInterviewed (int candidateInterviewId, 
               string interviewMode, DateTime interviewedAt)
          {
               if (candidateInterviewId == 0) return BadRequest(new ApiResponse(404, "Record Id cannot be 0"));
               return await _interviewService.RegisterCandidateAsInterviewed(candidateInterviewId, interviewMode, interviewedAt);
          }

          [HttpPut("registerCandidateInterviewedWithResult/{candidateInterviewId}/{interviewMode}/{interviewedAt}/{selectionStatusId}")]
          public async Task<ActionResult<bool>> RegisterCandidateInterviewedWithResult (int candidateInterviewId, 
               string interviewMode, DateTime interviewedAt, int selectionStatusId)
          {
               if (candidateInterviewId == 0) return BadRequest(new ApiResponse(404, "Record Id cannot be 0"));
               return await _interviewService.RegisterCandidateInterviewedWithResult(
                    candidateInterviewId, interviewMode, interviewedAt, selectionStatusId);
          }

          [HttpGet("interviewAttendance/{orderId}")]
          public async Task<ActionResult<ICollection<InterviewAttendanceDto>>> GetInterviewAttendance(int orderId, [FromQuery] List<int> attendanceStatusIds)
          {
               if (orderId == 0 || attendanceStatusIds.Count == 0) return BadRequest(new ApiResponse(402, "Bad Request"));
               
               var lst = await _interviewService.GetInterviewAttendanceOfAProject(orderId, attendanceStatusIds);

               if (lst == null) return NotFound(new ApiResponse(400, "No interview records found matching the criteria"));

               return Ok(lst);
          }
          
          [HttpGet("candidatesmatchinginterviewcat")]
          public async Task<ActionResult<ICollection<CandidateInBriefDto>>> GetCandidatesMatchingInterviewCategory(InterviewSpecParams interviewParams)
          {
               var cands = await _interviewService.GetCandidatesMatchingInterviewCategory(interviewParams);

               if (cands == null) return NotFound(new ApiResponse(404, "No matching candidates found"));

               return Ok(cands);
          }

     }
}
