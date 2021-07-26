using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.MasterEntities;
using core.Entities.Orders;
using core.Interfaces;
using core.ParamsAndDtos;
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

          [HttpPost("assessmentqbank")]
          public async Task<bool> AddStddAssessmentQs(ICollection<AssessmentQBank> stddQs)
          {
               return await _orderAssessmentService.AddStddQs(stddQs);
          }

          [HttpPut("assessmentqbank")]
          public async Task<bool> EditStddQs(ICollection<AssessmentQBank> stddQs)
          {
               return await _orderAssessmentService.EditStddQs(stddQs);
          }

          [HttpDelete("stddq")]
          public async Task<bool> DeleteStddQ(AssessmentQBank stddq)
          {
               return await _orderAssessmentService.DeleteStddQ(stddq);
          }


          [HttpPost("copystddq/{orderitemid}")]
          public async Task<OrderItemAssessment> CopyStddQToOrderItemAssessment(int orderitemid)
          {
               return await _orderAssessmentService.CopyStddQToOrderAssessmentItem(orderitemid);
          }

          [HttpGet("stddqsbysubject")]
          public async Task<ActionResult<Pagination<AssessmentQBank>>> GetStddQsBySubject(AssessmentStddQsParams stddQParams)
          {
               var data = await _orderAssessmentService.GetAssessmentQsFromBankBySubject(stddQParams);
               return Ok(new Pagination<AssessmentQBank>(stddQParams.PageIndex, stddQParams.PageSize, data.Count, data));
          }

          [HttpGet("itemassessment/{orderitemid}")]
          public async Task<ActionResult<OrderItemAssessment>> GetOrderItemAssessmentQs(int orderitemid)
          {
               var itemassessment = await _orderAssessmentService.GetOrderAssessmentItemQs(orderitemid);
               return itemassessment;
          }

          [HttpPut("editorderitemassessment")]
          public async Task<ActionResult<bool>> EditOrderItemAssessment(OrderItemAssessment orderItemAssessment)
          {
               return await _orderAssessmentService.EditOrderAssessmentItem(orderItemAssessment);
          }

     }
}