using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.EmailandSMS;
using core.Entities.HR;
using core.Entities.Identity;
using core.Entities.MasterEntities;
using core.Entities.Orders;
using core.Interfaces;
using core.ParamsAndDtos;
using infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class OrderAssessmentService : IOrderAssessmentService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ATSContext _context;
          private readonly ITaskService _taskService;
          public OrderAssessmentService(IUnitOfWork unitOfWork, ATSContext context, ITaskService taskService)
          {
               _taskService = taskService;
               _context = context;
               _unitOfWork = unitOfWork;
          }


          public Task<ICollection<EmailMessage>> AssignTasksToHRExecutives(ICollection<HRTaskAssignmentDto> assignmentsDto)
          {
               throw new System.NotImplementedException();
          }

        
          public async Task<OrderItemAssessment> CopyStddQToOrderAssessmentItem(int orderitemid)
          {
               var assessmentitem = await _context.OrderItemAssessments.Where(x => x.OrderItemId == orderitemid).FirstOrDefaultAsync();
               if (assessmentitem != null) return assessmentitem;

               var qs = await _context.AssessmentStandardQs.OrderBy(x => x.QNo).ToListAsync();
               var lst = new List<OrderItemAssessmentQ>();
               foreach (var q in qs)
               {
                    lst.Add(new OrderItemAssessmentQ(orderitemid, q.QNo, q.AssessmentParameter, q.Question, q.MaxPoints));
               }
               var orderitem = await _context.OrderItems.Where(x => x.Id == orderitemid)
                    .Select(x => new { x.OrderId, x.CategoryName, x.CategoryId }).FirstOrDefaultAsync();
               var orderNo = await _context.Orders.Where(x => x.Id == orderitem.OrderId).Select(x => x.OrderNo).FirstOrDefaultAsync();
               assessmentitem = new OrderItemAssessment(orderitemid, orderitem.OrderId, orderNo, orderitem.CategoryId,
                    orderitem.CategoryName, lst);

               _unitOfWork.Repository<OrderItemAssessment>().Add(assessmentitem);

               if (await _unitOfWork.Complete() > 0)
               {
                    return await _context.OrderItemAssessments.Where(x => x.OrderItemId == orderitemid).FirstOrDefaultAsync();
               }
               else
               {
                    return null;
               }
          }

          public async Task<bool> DeleteAssessmentItemQ(int orderitemid)
          {
               var assessmentItem = await _context.OrderItemAssessments.Where(x => x.OrderItemId == orderitemid).FirstOrDefaultAsync();
               _unitOfWork.Repository<OrderItemAssessment>().Delete(assessmentItem);

               return await _unitOfWork.Complete() > 0;
          }

          public async Task<bool> EditOrderAssessmentItem(OrderItemAssessment assessmentItem)
          {
               _unitOfWork.Repository<OrderItemAssessment>().Update(assessmentItem);

               foreach (var q in assessmentItem.OrderItemAssessmentQs)
               {
                    _unitOfWork.Repository<OrderItemAssessmentQ>().Update(q);
               }

               return await _unitOfWork.Complete() > 0;
          }

          public async Task<IReadOnlyList<AssessmentQBank>> GetAssessmentQsFromBankBySubject(AssessmentStddQsParams qsParams)
          {
               var qs = await _context.AssessmentQBank
                    .Include(x => x.AssessmentQBankItems
                         .Where(x => x.AssessmentParameter.ToLower() == qsParams.Subject.ToLower())
                         .OrderBy(x => x.QNo)
                         )
                    .OrderBy(x => x.CategoryName)
                    .ToListAsync();
               return qs;
          }

          public async Task<OrderItemAssessment> GetOrderAssessmentItemQs(int orderItemId)
          {
               return await _context.OrderItemAssessments.Where(x => x.OrderItemId == orderItemId)
                    .Include(x => x.OrderItemAssessmentQs.OrderBy(x => x.QuestionNo))
                    .FirstOrDefaultAsync();
          }

         
     }
}
