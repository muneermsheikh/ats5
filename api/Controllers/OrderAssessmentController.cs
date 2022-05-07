using System.Collections.Generic;
using System.Threading.Tasks;
using api.Errors;
using core.Entities.HR;
using core.Entities.MasterEntities;
using core.Entities.Orders;
using core.Interfaces;
using core.ParamsAndDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class OrderAssessmentController : BaseApiController
     {
          private readonly IOrderAssessmentService _orderAssessmentService;
          public OrderAssessmentController(IOrderAssessmentService orderAssessmentService)
          {
               _orderAssessmentService = orderAssessmentService;
          }

          //[Authorize(Policy = "OrderAssessmentQRole")]
          [HttpPost("copystddq/{orderitemid}")]
          public async Task<OrderItemAssessment> CopyStddQToOrderItemAssessment(int orderitemid)
          {
               return await _orderAssessmentService.CopyStddQToOrderAssessmentItem(orderitemid);
          }

          [HttpPost("{orderId}")]
          public async Task<ActionResult<OrderItemAssessment>> AddAssessmentItemsForNewOrderId(int orderId) 
          {
               var qs = await _orderAssessmentService.CreateNewOrderAssessment(orderId);

               if (qs == null) return BadRequest(new ApiResponse(400, "Failed to insert assessment questions for the Oreer"));

               return Ok(qs);

          }

          //[Authorize(Policy = "AssessmentQBankRole")]
          [HttpGet("stddqsbysubject")]
          public async Task<ActionResult<Pagination<AssessmentQBank>>> GetStddQsBySubject(AssessmentStddQsParams stddQParams)
          {
               var data = await _orderAssessmentService.GetAssessmentQsFromBankBySubject(stddQParams);
               return Ok(new Pagination<AssessmentQBank>(stddQParams.PageIndex, stddQParams.PageSize, data.Count, data));
          }

          //[Authorize(Policy = "OrderAssessmentQRole")]          
          [HttpGet("itemassessment/{orderitemid}")]
          public async Task<ActionResult<OrderItemAssessment>> GetOrderItemAssessmentQs(int orderitemid)
          {
               var itemassessment = await _orderAssessmentService.GetOrAddOrderAssessmentItem(orderitemid);
               return itemassessment;
          }

          [HttpGet("itemassessmentQ/{orderitemid}")]
          public async Task<ICollection<OrderItemAssessmentQ>> GetItemAssessmentQs(int orderitemid)
          {
               var qs = await _orderAssessmentService.GetAssessmentQsOfOrderItemId(orderitemid);
               return qs;
          }

          [HttpGet("orderassessment/{orderid}")]
          public async Task<ActionResult<ICollection<OrderItemAssessment>>> GetOrderAssessment(int orderid)
          {
               var itemassessment = await _orderAssessmentService.GetOrderAssessment(orderid);
               if (itemassessment == null) return BadRequest(new ApiResponse(400, "no assessment questions found matching the order id"));
               
               return Ok(itemassessment);
          }

          

          //[Authorize(Policy = "OrderAssessmentQRole")]
          [HttpPut("editassessment")]
          public async Task<ActionResult<bool>> EditOrderItemAssessment(OrderItemAssessment orderItemAssessment)
          {
               return await _orderAssessmentService.EditOrderAssessmentItem(orderItemAssessment);
          }

          [HttpPut("updateassessmentqs")]
          public async Task<ActionResult<bool>> EditOrderItemAssessments(ICollection<OrderItemAssessmentQ> assessmentQs)
          {
               return await _orderAssessmentService.EditOrderAssessmentQs(assessmentQs);
          }

          [HttpDelete("assessmentq/{assessmentQId}")]
          public async Task<ActionResult<bool>> DeleteOrderItemAssessmentQ(int assessmentQId)
          {
               return await _orderAssessmentService.DeleteAssessmentItemQ(assessmentQId);
          }


          [HttpDelete("assessment/{orderitemid}")]
          public async Task<ActionResult<bool>> DeleteAssessment(int orderitemid)
          {
               return await _orderAssessmentService.DeleteAssessmentItemQ(orderitemid);
          }

     }
}