using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Errors;
using api.Extensions;
using core.Entities.HR;
using core.Entities.Identity;
using core.Entities.Process;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     [Authorize]     //(Policy = "ProcessEmployeeRole")]
     public class DeployController : BaseApiController
     {
          private readonly IDeployService _deployService;
          private readonly IUnitOfWork _unitOfWork;
          private readonly IEmployeeService _empService;
          private readonly UserManager<AppUser> _userManager;
          public DeployController(IDeployService deployService, IUnitOfWork unitOfWork, 
                UserManager<AppUser> userManager, IEmployeeService empService)
          {
               _userManager = userManager;
               _empService = empService;
               _unitOfWork = unitOfWork;
               _deployService = deployService;
          }

          /*
          [HttpGet("pending")]
          public async Task<ActionResult<Pagination<CommonDataDto>>> GetPendingDeployments (DeploymentParams depParams)
          {
              var pendings = await _deployService.GetPendingDeployments(depParams);
              return Ok(new Pagination<CommonDataDto>(depParams.PageIndex,
                  depParams.PageSize, pendings.Count(), pendings));
          }
          */

          [HttpGet("pending/{pageIndex}/{pageSize}")]
          public async Task<ActionResult<Pagination<DeploymentPendingDto>>> GetPendingDeployments(int pageIndex, int pageSize)
          {
               /* var specParams = new CVRefSpecParams { PageSize = pageSize, PageIndex = pageIndex };
               var specs = new CVRefSpecs(specParams);
               var pendings = await _deployService.GetPendingDeployments(pageIndex, pageSize);
               var totalItems = await _deployService.CountOfPendingDeployments();
               return Ok(new Pagination<DeploymentPendingDto>(pageIndex, pageSize, totalItems, pendings));
               */

               var ret = await _deployService.GetPendingDeployments();
               
               return Ok(new Pagination<DeploymentPendingDto>(pageIndex,pageSize, ret.Count(), ret));
          }

          [Authorize]       //(Policy = "ViewDeploymentRole")]
          [HttpGet("{orderItemId}")]
          public async Task<ActionResult<ICollection<CVRef>>> GetDeploymentsOfOrderItemId(int orderItemId)
          {
               var cvrefs = await _deployService.GetDeploymentsOfOrderItemId(orderItemId);
               if (cvrefs != null) return Ok(cvrefs);
               return NotFound(new ApiResponse(404, "No referrals exist for the selected order category"));
          }

          [Authorize]       //(Policy = "Employee, ViewDeploymentRole")]
          [HttpGet("candidateid/{candidateId}")]
          public async Task<ActionResult<ICollection<CVRef>>> GetDeploymentsOfACandidate(int candidateId)
          {
               var cvrefs = await _deployService.GetDeploymentsOfACandidate(candidateId);
               if (cvrefs != null) return Ok(cvrefs);
               return NotFound(new ApiResponse(404, "No referrals exist for the selected candidate"));
          }

          [Authorize]       //(Policy = "Employee")]
          [HttpGet("bycvrefid/{cvrefid}")]
          public async Task<ActionResult<CVRef>> GetDeploymentByCVRefId(int cvrefid)
          {
               var cvref = await _deployService.GetDeploymentsById(cvrefid);
               if (cvref == null) return NotFound(new ApiResponse(404, "No referrals exist against the selected cvref"));
               return Ok(cvref);
          }

          [Authorize]       //(Policy = "Employee")]
          [HttpGet("{candidateId}/{orderItemId}")]
          public async Task<ActionResult<CVRef>> GetDeploymentsByCandidateAndOrderItem(int candidateId, int orderItemId)
          {
               var cvref = await _deployService.GetDeploymentsByCandidateAndOrderItem(candidateId, orderItemId);
               if (cvref == null) return NotFound(new ApiResponse(404, "No record found"));
               return Ok(cvref);
          }

          [Authorize]
          [HttpPost("{cvrefId}/{stageId}/{transDate}")]
       
          /*
          public async Task<ActionResult<Deploy>> AddDeploymentTransaction(int cvrefId, EnumDeployStatus stageId, DateTime? transDate)
          {
               var loggedInDto = await GetLoggedInUserDto();
               if (loggedInDto == null) return BadRequest(new ApiResponse(401, "this option requires logged in User"));

               if (!transDate.HasValue) transDate = DateTime.Now;

               var dep = await _deployService.AddDeploymentTransaction(cvrefId, loggedInDto.LoggedInEmployeeId, stageId, transDate);

               if (dep == null) return BadRequest(new ApiResponse(401, "Failed to register the deployment transaction"));

               return Ok(dep);
          }
          */
          
          [Authorize]
          [HttpPost("posts")]
          public async Task<ActionResult<ICollection<Deploy>>> AddDeploymentTransactions(ICollection<DeployPostDto> deployPostsDto)
          {
               var loggedInDto = await GetLoggedInUserDto();
               if (loggedInDto == null) return BadRequest(new ApiResponse(401, "this option requires logged in User"));
               
               foreach(var dto in deployPostsDto)
               {
                    if(dto.CVRefId == 0 || dto.StageId==0 || dto.TransDate.Year < 2000 ) return BadRequest(new ApiResponse(402, "Deploy Id or Status or Deployment date not provided"));
               }

               var dep = await _deployService.AddDeploymentTransactions(deployPostsDto, loggedInDto.LoggedInEmployeeId);

               if (dep == null) return BadRequest(new ApiResponse(401, "Failed to register the deployment transaction"));

               return Ok(dep);
          }

          [HttpPut]
          public async Task<ActionResult<bool>> EditDeploymentTransaction(Deploy deploy)
          {
               return await _deployService.EditDeploymentTransaction(deploy);
          }

          [HttpDelete]
          public async Task<ActionResult<bool>> DeleteDeploymentTransactions(Deploy deploy)
          {
               return await _deployService.DeleteDeploymentTransactions(deploy);
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