using System.Collections.Generic;
using System.Threading.Tasks;
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

          [Authorize(Policy = "OrderAssessmentQRole")]
          [HttpPost("copystddq/{orderitemid}")]
          public async Task<OrderItemAssessment> CopyStddQToOrderItemAssessment(int orderitemid)
          {
               return await _orderAssessmentService.CopyStddQToOrderAssessmentItem(orderitemid);
          }

          [Authorize(Policy = "AssessmentQBankRole")]
          [HttpGet("stddqsbysubject")]
          public async Task<ActionResult<Pagination<AssessmentQBank>>> GetStddQsBySubject(AssessmentStddQsParams stddQParams)
          {
               var data = await _orderAssessmentService.GetAssessmentQsFromBankBySubject(stddQParams);
               return Ok(new Pagination<AssessmentQBank>(stddQParams.PageIndex, stddQParams.PageSize, data.Count, data));
          }

          [Authorize(Policy = "OrderAssessmentQRole")]          
          [HttpGet("itemassessment/{orderitemid}")]
          public async Task<ActionResult<OrderItemAssessment>> GetOrderItemAssessmentQs(int orderitemid)
          {
               var itemassessment = await _orderAssessmentService.GetOrderAssessmentItemQs(orderitemid);
               return itemassessment;
          }

          [Authorize(Policy = "OrderAssessmentQRole")]
          [HttpPut("editorderitemassessment")]
          public async Task<ActionResult<bool>> EditOrderItemAssessment(OrderItemAssessment orderItemAssessment)
          {
               return await _orderAssessmentService.EditOrderAssessmentItem(orderItemAssessment);
          }

     }
}