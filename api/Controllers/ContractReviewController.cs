using System.Threading.Tasks;
using api.Errors;
using api.Extensions;
using core.Entities.EmailandSMS;
using core.Entities.Identity;
using core.Entities.Orders;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class ContractReviewController : BaseApiController
    {
        private readonly IContractReviewService _reviewService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public ContractReviewController(IContractReviewService reviewService, UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _reviewService = reviewService;
        }


        [Authorize] //(Roles = "ContractReviewRole")]
        [HttpPost("createobject/{orderId}")]
        public async Task<ContractReview> CreateContractReviewObject(int orderId)
        {
            var loggedInAppUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
            var cReview = await _reviewService.CreateContractReviewObject(orderId, loggedInAppUser.Id);
            return cReview;
        }

        [Authorize] //(Roles = "ContractReviewRole")]
        [HttpPut("update")]
        public async Task<ActionResult<EmailMessage>> UpdateContractReview(ContractReview contractReview)
        {
            var msgDto = await _reviewService.EditContractReview(contractReview);
            if (msgDto == null) {
                return BadRequest(new ApiResponse(404, "Failed to update the Contract Review"));
            } else {
                return msgDto.EmailMessage;
            }
        }


        [Authorize] //(Roles = "ContractReviewRole")]
        [HttpGet("dto/{orderid}")]
        public async Task<ContractReview> GetContractReviewDtoByOrderId(int orderid)
        {
            return await _reviewService.GetContractReviewDtoByOrderIdAsync(orderid);
        }

        [Authorize] //(Roles = "ContractReviewRole")]
        [HttpGet("orderitemdto/{orderitemid}")]
        public async Task<ContractReviewItemDto> GetContractReviewItemDto(int orderitemid)
        {
            var rvwitem = await _reviewService.GetContractReviewItemWithOrderDetails(orderitemid);
            return rvwitem;
        }

        [Authorize] //(Roles = "ContractReviewRole")]
        [HttpDelete("{orderid}")]           //deletes contractreview and all children
        public async Task<ActionResult<bool>> DeleteContractReview(int orderid)
        {
            if (!await _reviewService.DeleteContractReview(orderid))  return BadRequest(new ApiResponse(404, "Failed to delete the record"));
            return Ok(true);
        }

        [Authorize] //(Roles = "ContractReviewRole")]
        [HttpDelete("item/{orderitemid}")]      //deletes contractReviewItem and all reviewitems
        public async Task<ActionResult<bool>> DeleteContractReviewItem(int orderitemid)
        {
            if (!await _reviewService.DeleteContractReviewItem(orderitemid))  return BadRequest(new ApiResponse(404, "Failed to delete the record"));
            return Ok(true);
        }

        [Authorize] //(Roles = "ContractReviewRole")]
        [HttpDelete("reviewitem/{id}")]
        public async Task<ActionResult<bool>> DeleteContractReviewReviewItem(int id)
        {
            if (!await _reviewService.DeleteReviewReviewItem(id)) return BadRequest(new ApiResponse(404, "Failed to delete the record"));
            return Ok(true);
        }

        
        
    }
}