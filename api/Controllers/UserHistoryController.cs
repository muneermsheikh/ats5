using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Errors;
using api.Extensions;
using core.Entities.Admin;
using core.Entities.Identity;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class UserHistoryController : BaseApiController
    {
        private readonly IUserHistoryService _userContactService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmployeeService _empService;
        public UserHistoryController(IUserHistoryService userContactService, UserManager<AppUser> userManager, IEmployeeService empService)
        {
            _empService = empService;
            _userManager = userManager;
            _userContactService = userContactService;
        }

        [HttpGet("bycandidateid/{candidateid}")]
        public async Task<ActionResult<UserHistoryDto>> GetUserHistoryDataByCandidateId(int candidateid)
        {
            var specParams = new UserHistorySpecParams();
            specParams.CandidateId = candidateid;
            var data = await _userContactService.GetOrAddUserHistoryByParams(specParams);

            if (data != null) return Ok(data);

            return NotFound(new ApiResponse(400, "Your search parameters did not yield any result"));
        }

        [HttpGet]
        public async Task<ActionResult<UserHistoryDto>> GetUserHistoryData(UserHistorySpecParams specParams)
        {
            var data = await _userContactService.GetOrAddUserHistoryByParams(specParams);

            if (data != null) return Ok(data);

            return NotFound(new ApiResponse(400, "Your search parameters did not yield any result"));
        }

        [HttpPost("newusercontact")]
        public async Task<ActionResult<UserHistory>> AddNewUserContact(UserHistory userContact)
        {
            var loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
            if (loggedInUser == null) return Unauthorized("Access allowed to authorized loggin user only");
            
            //if(userContact.DateOfContact.Year < 2000) userContact.DateOfContact = DateTime.Now;

            var empId = await _empService.GetEmployeeIdFromAppUserIdAsync(loggedInUser.Id);
            if (empId == 0) return Unauthorized("Employee Id not on record");

            //userContact.LoggedInUserId = empId;
            if (userContact.CandidateId == 0 && userContact.CustomerOfficialId == 0) {
                return BadRequest(new ApiResponse(400, "Either Candidate Id or customer official Id should be provided"));
            }
            return await _userContactService.AddUserContact(userContact);
        }
        
        
        [HttpDelete("{userContactId}")]
        public async Task<bool> DeleteUserContactById(int userContactId)
        {
            return await _userContactService.DeleteUserContactById(userContactId);
        }


        [HttpGet("contactresults")]
        public async Task<ActionResult<ICollection<ContactResult>>> GetContactResults()
        {
            var results = await _userContactService.GetContactResults();
            if (results != null ) return Ok(results);
            return BadRequest(new ApiResponse(404, "No contact result data available"));
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateContactHistory(UserHistory userhistory)
        {
            var loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);   
            //if (loggedInUser == null) return Unauthorized("Access allowed to authorized loggedin user only");
            var succeeded = await _userContactService.EditContactHistory(userhistory, loggedInUser);
            if (succeeded) return Ok(true);
            return BadRequest(new ApiResponse(402, "Failed to Update the transactions"));
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