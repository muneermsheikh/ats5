using System.Collections.Generic;
using System.Threading.Tasks;
using api.Errors;
using api.Extensions;
using core.Entities.HR;
using core.Entities.Identity;
using core.Entities.MasterEntities;
using core.Interfaces;
using core.ParamsAndDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Net;
using System.Text;
using core.Entities.Orders;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
     public class ChecklistController : BaseApiController
     {
          private readonly IChecklistService _checklistService;
          private readonly UserManager<AppUser> _userManager;
          private readonly IEmployeeService _empService;

          public ChecklistController(IChecklistService checklistService, UserManager<AppUser> userManager, 
               IEmployeeService empService)
          {
               _empService = empService;
               _userManager = userManager;
               _checklistService = checklistService;

          }

          [HttpPost("{candidateid}/{orderitemid}")]
          public async Task<ActionResult<bool>> AddNewChecklist(int candidateid, int orderitemid)
          {
               var loggedInUserDto = await GetLoggedInUserDto();
               if (loggedInUserDto == null) return BadRequest(new ApiResponse(404, "this option requires log in"));

               var checklist = await _checklistService.AddNewChecklistHR(candidateid, orderitemid, loggedInUserDto);
               
               if (checklist == null) return BadRequest(new ApiResponse(404, "failed to save the checklist data"));

               return Ok();
          }

          [HttpPut("checklisthr")]
          public async Task<ActionResult<bool>> EditChecklistHRAsync(ChecklistHR checklistHR)
          {
               var loggedInUserDto = await GetLoggedInUserDto();
               if (loggedInUserDto == null) return BadRequest(new ApiResponse(404, "User not logged in"));
               
               var edit = await _checklistService.EditChecklistHR(checklistHR, loggedInUserDto);
               if (edit) {
                    return Ok();} 
               else {
                    return BadRequest(new ApiResponse(404, "Failed to edit the checklist"));
               }
          }

          
          [Authorize]
          [HttpGet("checklisthr/{candidateid}/{orderitemid}")]
          public async Task<ActionResult<ChecklistHR>> GetChecklistHR(int candidateid, int orderitemid)
          {
               var loggedInUserDto = await GetLoggedInUserDto();
               if (loggedInUserDto == null) return BadRequest(new ApiResponse(404, "this option requires log in"));

               var checklist = await _checklistService.GetChecklistHR(candidateid, orderitemid, loggedInUserDto);
               if (checklist == null) return BadRequest(new ApiResponse(404, "No data returned"));

               return Ok(checklist);
          }
          
          [HttpDelete("hrchecklist")]
          public async Task<ActionResult<bool>> DeleteChecklistHRAsync(ChecklistHR checklistHR)
          {
               var loggedInUserDto = await GetLoggedInUserDto();
               return await _checklistService.DeleteChecklistHR(checklistHR, loggedInUserDto);
          }

     //master data
          
          [HttpDelete("hrparameter")]
          public async Task<bool> DeleteChecklistHRDataAsync(ChecklistHRData checklistHRData)
          {
               return await _checklistService.DeleteChecklistHRDataAsync(checklistHRData);
          }

          //checklistHR - job card for HR Executives
          [HttpPost("newhrparameter/{checklist}")]
          public async Task<ChecklistHRData> AddChecklistHRParameter(string checklist)
          {
               return await _checklistService.AddChecklistHRParameter(checklist);
          }

          [HttpDelete]
          public async Task<bool> DeleteChecklistHRData(ChecklistHRData checklistHRData)
          {
               return await _checklistService.DeleteChecklistHRDataAsync(checklistHRData);
          }
          
          [HttpPut("hrchecklistdata")]
          public async Task<bool> EditChecklistHRDataAsync(ChecklistHRData checklistHRData)
          {
               return await _checklistService.EditChecklistHRDataAsync(checklistHRData);
          }

          [HttpGet("hrdata")]
          public async Task<IReadOnlyList<ChecklistHRData>> GetChecklistHRDataListAsync()
          {
               return await _checklistService.GetChecklistHRDataListAsync();
          }

          private async Task<LoggedInUserDto> GetLoggedInUserDto()
          {
               var loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
               if (loggedInUser == null) return null;
               
               var empId = await _empService.GetEmployeeIdFromAppUserIdAsync(loggedInUser.Id);
               var loggedInUserDto = new LoggedInUserDto{
                    LoggedIAppUsername = loggedInUser.UserName, LoggedInAppUserEmail=loggedInUser.Email, LoggedInAppUserId = loggedInUser.Id,
                    LoggedInEmployeeId = empId
               };
               
               return loggedInUserDto;
          }
     }
}