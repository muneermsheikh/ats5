using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using core.Entities.MasterEntities;
using core.Entities.Orders;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class ContractReviewController : BaseApiController
     {
          private readonly IContractReviewService _cReviewService;
          private readonly IUnitOfWork _unitOfWork;
          private readonly IMapper _mapper;
          public ContractReviewController(IContractReviewService cReviewService, IUnitOfWork unitOfWork, IMapper mapper)
          {
               _mapper = mapper;
               _unitOfWork = unitOfWork;
               _cReviewService = cReviewService;
          }

          [HttpPost("newcontractreview")]
          public async Task<ActionResult<bool>> CreateContractReview(ContractReview contractReview)
          {
               return await _cReviewService.CreateContractReview(contractReview);
          }

          [HttpGet("contractreviewitems")]
          public async Task<IReadOnlyList<ContractReviewItemDto>> GetContractReviewItems(ContractReviewSpecParams contractReviewSpecParams)
          {
               return await _cReviewService.GetContractReviewItemsAsync(contractReviewSpecParams);
          }

          [HttpGet("contractreviewitemsoforderid")]
          public async Task<IReadOnlyList<ContractReviewItemDto>> GetContractReviewItemsOfOrderId(int orderid)
          {
               return await _cReviewService.GetContractReviewItemsByOrderIdAsync(orderid);
          }

          [HttpGet("contractreview")]
          public async Task<ContractReview> GetContractReview(int orderid)
          {
               return await _cReviewService.GetContractReview(orderid);
          }


          [HttpPut("contractreview")]
          public async Task<bool> EditContractReview(ContractReview contractReview)
          {
               _cReviewService.EditContractReview(contractReview);
               if (await _unitOfWork.Complete() > 0) return true;
               return false;
          }

          [HttpDelete("review")]
          public async Task<bool> DeleteContractReview(int orderid)
          {
               return await _cReviewService.DeleteContractReview(orderid);
          }

          [HttpDelete("reviewitem")]
          public async Task<bool> DeleteContractReviewItem(int orderitemid)
          {
               return await _cReviewService.DeleteContractReviewItem(orderitemid);
          }
          
          [HttpPost("reviewstatus/{reviewstatusname}")]
          public async Task<bool> AddContractReviewStatusName(string reviewstatusname)
          {
               _cReviewService.AddReviewStatus(reviewstatusname);

               return (await _unitOfWork.Complete() > 0);
          }

          [HttpPost("reviewitemstatus/{reviewstatusname}")]
          public async Task<bool> AddContractReviewItemStatusName(string reviewstatusname)
          {
               _cReviewService.AddReviewItemStatus(reviewstatusname);

               return (await _unitOfWork.Complete() > 0);
          }

          [HttpGet("reviewstatuslist")]
          public async Task<ICollection<ReviewStatus>> GetReviewStatus()
          {
               return await _cReviewService.GetReviewStatus();
          }

          [HttpGet("reviewitemstatuslist")]
          public async Task<IReadOnlyList<ReviewItemStatus>> GetReviewItemStatus()
          {
               return await _cReviewService.GetReviewItemStatus();
          }
          
          [HttpGet("reviewitembyorderitemid/{orderitemid}")]
          public async Task<ContractReviewItemDto> GetContractReviewItemByItemId(int orderitemid)
          {
               return await _cReviewService.GetContractReviewItemWithOrderDetails(orderitemid);
          }
          

     }
}