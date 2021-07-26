using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.HR;
using core.Entities.Orders;
using core.Interfaces;
using infra.Data;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class CandidateAssessmentService : ICandidateAssessmentService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ATSContext _context;
          public CandidateAssessmentService(IUnitOfWork unitOfWork, ATSContext context)
          {
               _context = context;
               _unitOfWork = unitOfWork;
          }

          public async Task<CandidateAssessment> AssessNewCandidate(int candidateId, int orderItemId, int loggedInUserId, DateTime dateAssessed)
          {
                var itemassessment = await _context.OrderItemAssessments
                    .Where(x => x.OrderItemId == orderItemId)
                    .Include(x => x.OrderItemAssessmentQs.OrderBy(x => x.QuestionNo))
                    .FirstOrDefaultAsync();
                if (itemassessment == null) throw new System.Exception("Order Category assessment parameters not defined");

                //check if the assessment already exists
                var candassessment = await _context.CandidateAssessments
                    .Where(x => x.CandidateId == candidateId && x.OrderItemId == orderItemId)
                    .FirstOrDefaultAsync();
                
                if (candassessment != null) return candassessment;

                var items = new List<CandidateAssessmentItem>();
                foreach(var item in itemassessment.OrderItemAssessmentQs)
                {
                    items.Add(new CandidateAssessmentItem(item.QuestionNo, item.Subject, item.Question, item.IsMandatory, item.MaxMarks));
                }

                //create the assessment record
                var candidateassessment = new CandidateAssessment(candidateId, orderItemId, loggedInUserId, dateAssessed, items);

                _unitOfWork.Repository<CandidateAssessment>().Add(candidateassessment);

                if (await _unitOfWork.Complete() > 0) {
                    return await _context.CandidateAssessments
                        .Include(x => x.CandidateAssessmentItems.OrderBy(x => x.QuestionNo))
                        .Where(x => x.CandidateId == candidateId && x.OrderItemId == orderItemId)
                        .FirstOrDefaultAsync();
                } else {
                    return null;
                }
          }

          public async Task<bool> DeleteCandidateAssessment(CandidateAssessment candidateAssessment)
          {
               _unitOfWork.Repository<CandidateAssessment>().Delete(candidateAssessment);
               return await _unitOfWork.Complete() > 0;
          }

          public async Task<bool> EditCandidateAssessment(CandidateAssessment candidateAssessment)
          {
               _unitOfWork.Repository<CandidateAssessment>().Update(candidateAssessment);
               foreach(var item in candidateAssessment.CandidateAssessmentItems)
               {
                   _unitOfWork.Repository<CandidateAssessmentItem>().Update(item);
               }
               return (await _unitOfWork.Complete() > 0);
          }

     }
}