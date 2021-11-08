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
    public class UserContactedController : BaseApiController
    {
        private readonly IUserContactService _userContactService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmployeeService _empService;
        public UserContactedController(IUserContactService userContactService, UserManager<AppUser> userManager, IEmployeeService empService)
        {
            _empService = empService;
            _userManager = userManager;
            _userContactService = userContactService;
        }

        [HttpPut]
        public async Task<ActionResult<UserContact>> EditUserContacted(UserContact userContact)
        {
            var loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
            if (loggedInUser == null) return Unauthorized("Access allowed to authorized loggin user only");
            
            if(userContact.DateOfContact.Year < 2000) userContact.DateOfContact = DateTime.Now;

            var empId = await _empService.GetEmployeeIdFromAppUserIdAsync(loggedInUser.Id);
            if (empId == 0) return Unauthorized("Employee Id not on record");
            userContact.LoggedInUserId = empId;
            return await _userContactService.EditUserContact(userContact);
        }
        
        [HttpPost]
        public async Task<ActionResult<UserContact>> AddNewUserContacted(UserContact userContact)
        {
            var loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
            if (loggedInUser == null) return Unauthorized("Access allowed to authorized loggin user only");
            
            if(userContact.DateOfContact.Year < 2000) userContact.DateOfContact = DateTime.Now;

            var empId = await _empService.GetEmployeeIdFromAppUserIdAsync(loggedInUser.Id);
            if (empId == 0) return Unauthorized("Employee Id not on record");

            userContact.LoggedInUserId = empId;
            
            return await _userContactService.AddUserContact(userContact);
        }
        
        
        [HttpDelete("{userContactId}")]
        public async Task<bool> DeleteUserContactById(int userContactId)
        {
            return await _userContactService.DeleteUserContactById(userContactId);
        }

        [HttpGet("fromparams")]
        public async Task<ActionResult<Pagination<UserContactDto>>> GetUserContactsFromParams(UserContactSpecParams specParams)
        {
            var result = await _userContactService.GetUserContactsFromParams(specParams);
            if (result == null || result.Count == 0) return NotFound(new ApiResponse(402, "Failed to find matching records"));
            return Ok(result);
        }

    /*
        [HttpGet("{CandidateId}/{orderitemid}")]
        public async Task<ActionResult<ICollection<UserContactDto>>> GetUserContactsOfACandidateForAnOrderItem(int candidateId, int orderitemid)
        {
            var result = await _userContactService.GetUserContactsOfACandidateForOrderItem(candidateId, orderitemid);
            if (result == null || result.Count == 0) return NotFound(new ApiResponse(402, "Failed to find matching records"));
            return Ok(result);
        }
        
        [HttpGet("usercontactsonadate/{contactdate}")]
        public async Task<ActionResult<ICollection<UserContactDto>>> GetUserContactsOnADate(DateTime contactdate)
        {
            var result = await _userContactService.GetUserContactsForADate(contactdate);
            if (result == null || result.Count == 0) return NotFound(new ApiResponse(402, "Failed to find matching records"));
            return Ok(result);
        }

        [HttpGet("orderitem/{orderitemid}")]
        public async Task<ActionResult<ICollection<UserContactDto>>> GetUserContactsOfAnOrderItem(int orderitemid)
        {
            var result = await _userContactService.GetUserContactsOfAnOrderItem(orderitemid);
            if (result == null || result.Count == 0) return NotFound(new ApiResponse(402, "Failed to find matching records"));
            return Ok(result);
        }
        
        [HttpGet("order/{orderid}")]
        public async Task<ActionResult<ICollection<UserContactDto>>> GetUserContactsOfAnOrder(int orderid)
        {
            var result = await _userContactService.GetUserContactsOfAnOrder(orderid);
            if (result == null || result.Count == 0) return NotFound(new ApiResponse(402, "Failed to find matching records"));
            return Ok(result);
        }
        
        [HttpGet("{candidateId}")]
        public async Task<ActionResult<ICollection<UserContactDto>>> GetUserContactsOfACandidate(int candidateId)
        {
            var result = await _userContactService.GetUserContacts(candidateId);
            if (result == null || result.Count == 0) return NotFound(new ApiResponse(402, "Failed to find matching records"));
            return Ok(result);
        }
    */

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