using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Errors;
using core.Entities.HR;
using core.Entities.Process;
using core.Interfaces;
using core.ParamsAndDtos;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class DeployController : BaseApiController
     {
        private readonly IDeployService _deployService;
        public DeployController(IDeployService deployService)
        {
            _deployService = deployService;
        }
        
        [HttpGet("pending")]
        public async Task<ActionResult<Pagination<CommonDataDto>>> GetPendingDeployments (DeploymentParams depParams)
        {
            var pendings = await _deployService.GetPendingDeployments();
            return Ok(new Pagination<CommonDataDto>(depParams.PageIndex,
               depParams.PageSize, pendings.Count(), pendings));
        }
        
        [HttpGet("{orderItemId}")]
        public async Task<ActionResult<ICollection<CVRef>>> GetDeploymentsOfOrderItemId(int orderItemId)
        {
            var cvrefs = await _deployService.GetDeploymentsOfOrderItemId(orderItemId);
            if (cvrefs != null) return Ok(cvrefs);
            return NotFound(new ApiResponse(404, "No referrals exist for the selected order category"));
        }

        [HttpGet("candidateid/{candidateId}")]
        public async Task<ActionResult<ICollection<CVRef>>> GetDeploymentsOfACandidate(int candidateId)
        {
            var cvrefs = await _deployService.GetDeploymentsOfACandidate(candidateId);
            if (cvrefs != null) return Ok(cvrefs);
            return NotFound(new ApiResponse(404, "No referrals exist for the selected candidate"));
        }
        
        [HttpGet("bycvrefid/{cvrefid}")]
        public async Task<ActionResult<CVRef>> GetDeploymentsById(int cvrefid)
        {
            var cvref = await _deployService.GetDeploymentsById(cvrefid);
            if (cvref == null) return NotFound(new ApiResponse(404, "No referrals exist against the selected cvref"));
            return Ok(cvref);
        }
        
        [HttpGet("{candidateId}/{orderItemId}")]
        public async Task<ActionResult<CVRef>> GetDeploymentsByCandidateAndOrderItem(int candidateId, int orderItemId)
        {
            var cvref = await _deployService.GetDeploymentsByCandidateAndOrderItem(candidateId, orderItemId);
            if (cvref == null) return NotFound(new ApiResponse(404, "No record found"));
            return Ok(cvref);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddDeploymentTransaction (Deploy deploy)
        {
            return await _deployService.AddDeploymentTransaction(deploy);
        }

        [HttpPut]
        public async Task<ActionResult<bool>> EditDeploymentTransaction (Deploy deploy)
        {
            return await _deployService.EditDeploymentTransaction(deploy);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteDeploymentTransactions (Deploy deploy)
        {
            return await _deployService.DeleteDeploymentTransactions(deploy);
        }

     }
}