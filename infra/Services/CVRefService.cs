using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.EmailandSMS;
using core.Entities.HR;
using core.Entities.Identity;
using core.Entities.Orders;
using core.Entities.Tasks;
using core.Interfaces;
using core.ParamsAndDtos;
using infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace infra.Services
{
     public class CVRefService : ICVRefService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ATSContext _context;
          private readonly ICommonServices _commonService;
          private readonly IConfiguration _config;
          private readonly IEmployeeService _empService;
          private readonly int _docControllerAdminId;
          private readonly int _TargetDays_FollowUpWithClientForSelection;
          private readonly bool _ByepassCVReviewLevels;
          private readonly IComposeMessages _composeMsg;
          private readonly IEmailService _emailService;
          public CVRefService(IUnitOfWork unitOfWork, ATSContext context, ICommonServices commonService, IEmailService emailService,
                    IConfiguration config, IComposeMessages composeMsg, IEmployeeService empService)
          {
               _emailService = emailService;
               _empService = empService;
               _composeMsg = composeMsg;
               _config = config;
               _commonService = commonService;
               _context = context;
               _unitOfWork = unitOfWork;
               _docControllerAdminId = Convert.ToInt32(config.GetSection("EmpDocControllerAdminId").Value);
               _ByepassCVReviewLevels = config.GetSection("ByepassCVReviewLevels").Value.ToLower() == "true" ? true : false;
               _TargetDays_FollowUpWithClientForSelection = Convert.ToInt32(config.GetSection("TargetDays_FollowUpWithClientForSelection").Value);
          }

          public async Task<bool> EditReferral(CVRef cVRef)
          {
               var refStatus = await _context.CVRefs
               .Where(x => x.Id == cVRef.Id).Select(x => x.RefStatus)
               .FirstOrDefaultAsync();

               if (refStatus != (int)EnumCVRefStatus.Referred) return false;    //ref status changed, so no edits

               _unitOfWork.Repository<CVRef>().Update(cVRef);

               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<CVRef> GetReferralById(int cvrefid)
          {
               return await _context.CVRefs.FindAsync(cvrefid);
          }
          public async Task<CVRef> GetReferralByCandidateAndOrderItem(int candidateId, int orderItemId)
          {
               return await _context.CVRefs.Where(x => x.CandidateId == candidateId && x.OrderItemId == orderItemId)
               .FirstOrDefaultAsync();
          }

          public async Task<ICollection<CVRef>> GetReferralsOfACandidate(int candidateId)
          {
               return await _context.CVRefs.Where(x => x.CandidateId == candidateId)
               .OrderBy(x => x.ReferredOn)
               .ToListAsync();
          }

          public async Task<ICollection<CVRef>> GetReferralsOfOrderItemId(int orderItemId)
          {
               return await _context.CVRefs.Where(x => x.OrderItemId == orderItemId)
               .OrderBy(x => x.ReferredOn).ToListAsync();
          }

          //post actions - after inserting CVRef record
          //Update owner task of the DocController Task to forward the CV
          public async Task<ICollection<EmailMessage>> MakeReferralsAndCreateTask(ICollection<int> CVRvwIds, LoggedInUserDto loggedInUserDto)
          {
               //todo - implement CVRefRestriction checking

               DateTime dateTimeNow = DateTime.Now;
               var cvreviews = await _context.CVReviews.Where(x => CVRvwIds.Contains(x.Id) && x.DocControllerAdminTaskId > 0).ToListAsync();
               var cvrefs = new List<CVRef>();
               foreach (var rvw in cvreviews)
               {
                    //var commonData = await _commonService.CommonDataFromOrderDetailIdAndCandidateId( dto.OrderItemId, dto.CandidateId);
                    var commonData = await _commonService.CommonDataForCVRefFromOrderItemAndCandidateId(rvw.OrderItemId, rvw.CandidateId);
                    var cvref = new CVRef(rvw.OrderItemId, commonData.CategoryId, commonData.OrderId, commonData.OrderNo,
                         commonData.CustomerName, commonData.CategoryName, rvw.CandidateId, rvw.Ecnr, commonData.ApplicationNo,
                         commonData.CandidateName, dateTimeNow, rvw.Charges, rvw.HRExecutiveId, rvw.Id);
                    string taskItemDescription = commonData.CandidateDesc + " referred on " + dateTimeNow.Date;

                    _unitOfWork.Repository<CVRef>().Add(cvref);
                    cvrefs.Add(cvref);

                    //Update the Doc Controller task as completed
                    var task = await _context.Tasks.FindAsync(commonData.DocControllerAdminTaskId);
                    if (task != null)
                    {
                         task.TaskStatus = "Completed";
                         task.CompletedOn = dateTimeNow;
                         var taskitem = new TaskItem((int)EnumTaskType.SubmitCVToDocControllerAdmin, task.Id, dateTimeNow,
                              "Completed", taskItemDescription, loggedInUserDto.LoggedInEmployeeId, cvref.OrderId, cvref.OrderItemId,
                              cvref.OrderNo, loggedInUserDto.LoggedInAppUserId, dateTimeNow.AddYears(-1000), rvw.CandidateId,
                              0, _docControllerAdminId, task);
                         _unitOfWork.Repository<TaskItem>().Add(taskitem);
                         _unitOfWork.Repository<ApplicationTask>().Update(task);
                    }

                    //create task for DocController to follow up with client for selection
                    var taskForSelection = new ApplicationTask((int)EnumTaskType.SelectionFollowupByDocControllerAdmin,
                         dateTimeNow, _docControllerAdminId, _docControllerAdminId, commonData.OrderId,
                         commonData.OrderNo, rvw.OrderItemId, "Follow up for selection of " + commonData.CandidateDesc +
                         "referred on " + dateTimeNow.Date, dateTimeNow.AddDays(_TargetDays_FollowUpWithClientForSelection),
                         "Open", rvw.CandidateId, rvw.Id);
                    _unitOfWork.Repository<ApplicationTask>().Add(taskForSelection);
               }

               //todo - do we want to advise the candidate about his referral?

               if (await _unitOfWork.Complete() > 0)
               {
                    foreach (var rvw in cvreviews)       //update cvreview.CVRefId             
                    {
                         var cvref = cvrefs.Find(x => x.OrderItemId == rvw.OrderItemId && x.CandidateId == rvw.CandidateId);
                         if (cvref != null) rvw.CVRefId = cvref.Id;
                    }

                    var emailmsgs = _composeMsg.ComposeCVFwdMessagesToClient(cvrefs, loggedInUserDto);
                    await _unitOfWork.Complete();
                    var filePaths = new List<string>();
                    foreach (var msg in emailmsgs)
                    {
                         _emailService.SendEmail(msg, filePaths);
                    }
                    return emailmsgs;
               }
               else
               {
                    return null;
               }
               //todo - update the CVRef object with actual date cv sent - call back from email sent                    
          }

          public async Task<bool> DeleteReferral(CVRef cvref)
          {
               if (await _context.Deploys.Where(x => x.CVRefId == cvref.Id).ToListAsync() != null)
               {
                    throw new System.Exception("The referral has related records in Deployments");
               }
               _unitOfWork.Repository<CVRef>().Delete(cvref);
               return await _unitOfWork.Complete() > 0;
          }

          public async Task<ICollection<CVRefPendingDto>> GetCVsPendingReferralsToCustomer()
          {
               var qry = await (from rvw in _context.CVReviews
                                where rvw.CVRefId == 0 && rvw.DocControllerAdminTaskId > 0
                                join i in _context.OrderItems on rvw.OrderItemId equals i.Id
                                join o in _context.Orders on i.OrderId equals o.Id
                                join c in _context.Candidates on rvw.CandidateId equals c.Id
                                join cat in _context.Categories on i.CategoryId equals cat.Id
                                join cust in _context.Customers on o.CustomerId equals cust.Id
                                select new CVRefPendingDto
                                {
                                     CandidateId = rvw.CandidateId,
                                     CandidateDetails = "App No " + c.ApplicationNo + ", " + c.FullName + " PP No." + c.PpNo,
                                     CategoryRef = o.OrderNo + "-" + i.SrNo,
                                     ToBeReferredToCustomer = cust.CustomerName,
                                     DateCVApprovedToForward =
                                          i.NoReviewBySupervisor ? rvw.SubmittedByHRExecOn.Date
                                          : rvw.HRMId == 0
                                               ? Convert.ToDateTime(rvw.ReviewedBySupOn).Date
                                               : Convert.ToDateTime(rvw.HRMReviewedOn).Date,
                                     CVApprovedByUsername = "",
                                     NoReviewBySupervisor = i.NoReviewBySupervisor,
                                     HRExecutiveId = (int)i.HrExecId,
                                     HRSupId = (int)i.HrSupId,
                                     HRMId = (int)i.HrmId
                                })
               .ToListAsync();

               foreach (var item in qry)
               {
                    item.CVApprovedByUsername = await _empService.GetEmployeeNameFromEmployeeId(
                         item.NoReviewBySupervisor ? item.HRExecutiveId : item.HRMId == 0 ? item.HRSupId : item.HRMId);
               }

               return qry;

          }

          public async Task<ICollection<CustomerReferralsPendingDto>> CustomerReferralsPending(int userId)
          {
               if (userId == 0)
               {
                    return await (from r in _context.CVReviews
                                  where r.CVRefId == 0 && r.DocControllerAdminTaskId > 0
                                  join i in _context.OrderItems on r.OrderItemId equals i.Id
                                  join o in _context.Orders on i.OrderId equals o.Id
                                  join cat in _context.Categories on i.CategoryId equals cat.Id
                                  join cand in _context.Candidates on r.CandidateId equals cand.Id
                                  join c in _context.Customers on o.CustomerId equals c.Id
                                  select new CustomerReferralsPendingDto
                                  {
                                       CVReviewId = r.Id,
                                       CandidateId = r.CandidateId,
                                       ApplicationNo = cand.ApplicationNo,
                                       CandidateName = cand.FullName,
                                       OrderItemId = r.OrderItemId,
                                       CategoryRef = o.OrderNo + "-" + i.SrNo,
                                       CustomerName = c.CustomerName,
                                       DocControllerAdminTaskId = (int)r.DocControllerAdminTaskId,
                                       SentToDocControllerOn = r.NoReviewBySupervisor ? r.SubmittedByHRExecOn.Date :
                                            r.HRMId == 0 ? Convert.ToDateTime(r.ReviewedBySupOn).Date
                                            : Convert.ToDateTime(r.HRMReviewedOn).Date
                                  }).ToListAsync();
               }
               else
               {
                    return await (from r in _context.CVReviews
                                  where r.CVRefId == 0 && r.DocControllerAdminTaskId > 0 && r.DocControllerAdminEmployeeId == userId
                                  join i in _context.OrderItems on r.OrderItemId equals i.Id
                                  join o in _context.Orders on i.OrderId equals o.Id
                                  join cat in _context.Categories on i.CategoryId equals cat.Id
                                  join cand in _context.Candidates on r.CandidateId equals cand.Id
                                  join c in _context.Customers on o.CustomerId equals c.Id
                                  select new CustomerReferralsPendingDto
                                  {
                                       CVReviewId = r.Id,
                                       CandidateId = r.CandidateId,
                                       ApplicationNo = cand.ApplicationNo,
                                       CandidateName = cand.FullName,
                                       OrderItemId = r.OrderItemId,
                                       CategoryRef = o.OrderNo + "-" + i.SrNo,
                                       CustomerName = c.CustomerName,
                                       DocControllerAdminTaskId = (int)r.DocControllerAdminTaskId,
                                       SentToDocControllerOn = r.NoReviewBySupervisor ? r.SubmittedByHRExecOn.Date :
                                            r.HRMId == 0 ? Convert.ToDateTime(r.ReviewedBySupOn).Date
                                            : Convert.ToDateTime(r.HRMReviewedOn).Date
                                  }).ToListAsync();
               }
          }
     }
}