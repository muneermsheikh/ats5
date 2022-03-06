using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using core.Entities.EmailandSMS;
using core.Entities.HR;
using core.Entities.Orders;
using core.Entities.Process;
using core.Entities.Tasks;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class SelectionDecisionService : ISelectionDecisionService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ICommonServices _commonServices;
          private readonly IDeployService _deployService;
          private readonly ATSContext _context;
          private readonly IComposeMessages _composeMessages;
          private readonly IEmailService _emailService;
          private readonly IMapper _mapper;
          private readonly IComposeMessagesForAdmin _composeMsgForAdmin;
          public SelectionDecisionService(IUnitOfWork unitOfWork, ATSContext context, ICommonServices commonServices, IComposeMessagesForAdmin composeMsgForAdmin,
          IComposeMessages composeMessages, IDeployService deployService, IEmailService emailService, IMapper mapper)
          {
               _composeMsgForAdmin = composeMsgForAdmin;
               _mapper = mapper;
               _emailService = emailService;
               _composeMessages = composeMessages;
               _context = context;
               _deployService = deployService;
               _commonServices = commonServices;
               _unitOfWork = unitOfWork;
          }

     public async Task<bool> DeleteSelection(SelectionDecision selectionDecision)
     {
          _unitOfWork.Repository<SelectionDecision>().Delete(selectionDecision);
          return await _unitOfWork.Complete() > 0;
     }

     public async Task<bool> EditSelection(SelectionDecision selectionDecision)
     {
          _unitOfWork.Repository<SelectionDecision>().Update(selectionDecision);
          return await _unitOfWork.Complete() > 0;
     }

     public async Task<Pagination<SelectionDecision>> GetSelectionDecisions(SelDecisionSpecParams specParams)
     {
          var spec = new SelectionDecisionSpecs(specParams);
          var specCount = new SelectionDecisionForCountSpecs(specParams);
          var decisions = await _unitOfWork.Repository<SelectionDecision>().ListAsync(spec);
          var ct = await _unitOfWork.Repository<SelectionDecision>().CountAsync(specCount);

          return new Pagination<SelectionDecision>(specParams.PageIndex, specParams.PageSize, ct, decisions);
     }

     public async Task<IReadOnlyList<EmailMessage>> RegisterSelections(ICollection<SelDecisionToAddDto> selDto, int loggedInEmployeeId)
     {
          DateTime dateTimeNow = DateTime.Now;
          var seldecisions = new List<SelectionDecision>();
          var msgs = new List<EmailMessage>();

          var cvrefids = selDto.Select(x => x.CVRefId).ToList();

          SelectionDecision selDecision;
          var cvrefs = await (from cvref in _context.CVRefs
               where cvrefids.Contains(cvref.Id)
               select new CVRef
               {
                    Id = cvref.Id,
                    OrderItemId = cvref.OrderItemId,
                    CategoryId = cvref.CategoryId,
                    CategoryName = cvref.CategoryName,
                    OrderId = cvref.OrderId,
                    OrderNo = cvref.OrderNo,
                    CustomerName = cvref.CustomerName,
                    ApplicationNo = cvref.ApplicationNo,
                    CandidateId = cvref.CandidateId,
                    CandidateName = cvref.CandidateName,
                    RefStatus = cvref.RefStatus,
                    HRExecId = cvref.HRExecId
               }
          ).ToListAsync();

          foreach (var s in selDto)
          {
               var categoryname = "";
               var candidatename = "";
               var appno = 0;
               //var orderid=0;
               //var categoryId=0;

               var cvref = cvrefs.Where(x => x.Id == s.CVRefId).FirstOrDefault();
               if (s.SelectionStatusId == (int)EnumCVRefStatus.Selected)
               {
                    if (string.IsNullOrEmpty(cvref.CandidateName))
                    {
                         var cand = await _context.Candidates.Where(x => x.Id == cvref.CandidateId)
                              .Select(x => new { x.ApplicationNo, x.FullName }).FirstOrDefaultAsync();
                         var category = await _context.Categories.Where(x => x.Id == cvref.CategoryId)
                              .Select(x => x.Name).FirstOrDefaultAsync();
                         //categoryId=cvref.CategoryId;
                         categoryname = category;
                         candidatename = cand.FullName;
                         appno = cand.ApplicationNo;

                    }
                    else
                    {
                         categoryname = cvref.CategoryName;
                         candidatename = cvref.CandidateName;
                         appno = cvref.ApplicationNo;
                    }
                    selDecision = new SelectionDecision(cvref.Id, cvref.OrderItemId, cvref.CategoryId, categoryname,
                         cvref.OrderId, cvref.OrderNo, cvref.CustomerName, appno, cvref.CandidateId,
                         candidatename, s.DecisionDate, s.SelectionStatusId, s.Remarks, s.Charges,
                         s.SalaryCurrency, s.Salary, s.ContractPeriodInMonths, s.HousingProvidedFree,
                         s.HousingAllowance, s.FoodProvidedFree, s.FoodAllowance, s.TransportProvidedFree, s.TransportAllowance,
                         s.OtherAllowance, s.LeavePerYearInDays, s.LeaveAirfareEntitlementAfterMonths);
                    var deployTrans = new Deploy(cvref.Id, s.DecisionDate, EnumDeployStatus.Selected);
                    _unitOfWork.Repository<Deploy>().Add(deployTrans);

                    seldecisions.Add(selDecision);
                    _unitOfWork.Repository<SelectionDecision>().Add(selDecision);

                    var HRExecTask = new ApplicationTask((int)EnumTaskType.OfferLetterAcceptance, dateTimeNow,
                         loggedInEmployeeId, cvref.HRExecId, cvref.OrderId, cvref.OrderNo, cvref.OrderItemId,
                         "Get Candidate's acceptance of the selection term " + "Application No. " + appno + ", " +
                         candidatename + " selected for " + cvref.CustomerName + " on " + dateTimeNow.Date,
                              dateTimeNow.AddDays(2), "Open", cvref.CandidateId, 0);
                    _unitOfWork.Repository<ApplicationTask>().Add(HRExecTask);
               }
               //update CVRef
               cvref.RefStatus = s.SelectionStatusId;
               cvref.DeployStageId = EnumDeployStatus.Selected;
               cvref.DeployStageDate = dateTimeNow;

               _unitOfWork.Repository<CVRef>().Update(cvref);
               //update doc controller task for selection

               var docTask = await _context.Tasks.Where(x => x.CandidateId == cvref.CandidateId &&
                    x.OrderItemId == cvref.OrderItemId && x.TaskTypeId == (int)EnumTaskType.SelectionFollowupByDocControllerAdmin)
                    .FirstOrDefaultAsync();
               if (docTask != null)
               {
                    docTask.TaskStatus = "Completed";
                    docTask.CompletedOn = dateTimeNow;
                    _unitOfWork.Repository<ApplicationTask>().Update(docTask);
               }
          }

          if (await _unitOfWork.Complete() > 0)
          {
               var selDecDto = new List<SelectionDecisionMessageParamDto>();
               //var selected = seldecisions.Where(x => x.SelectionStatusId == (int)EnumSelStatus.Selected).ToList();
               if (seldecisions != null && seldecisions.Count > 0)
               {
                    foreach (var s in seldecisions)
                    {
                         selDecDto.Add(new SelectionDecisionMessageParamDto { SelectionDecision = s, DirectlySendMessage = true });
                    }

                    var msg = await _composeMsgForAdmin.AdviseSelectionStatusToCandidateByEmail(selDecDto);
                    if (msg != null)
                    {
                         foreach (var m in msg) { msgs.Add(m); }
                    }
               }
               var rejectionDto = selDto.Where(x => x.SelectionStatusId != (int)EnumSelStatus.Selected).ToList();
               var rejected = _mapper.Map<ICollection<SelDecisionToAddDto>, ICollection<RejDecisionToAddDto>>(rejectionDto);
               
               //var rejected = new selDto.Where(x => x.SelectionStatusId != (int)EnumSelStatus.Selected).ToList();
               if (rejected != null)
               {
                    var msg = _composeMsgForAdmin.AdviseRejectionStatusToCandidateByEmail(rejected);
                    if (msg != null)
                    { foreach (var m in msg) { msgs.Add(m); } }
               }
               var attachments = new List<string>();
               foreach (var msg in msgs)
               {
                    _emailService.SendEmail(msg, attachments);
               }
               return msgs;
          }
          else
          {
               return null;
          }
     }

     public async Task<IReadOnlyList<SelectionsPendingDto>> GetPendingSelections()
     {
          var qry2 = await (from r in _context.CVRefs
               where r.RefStatus == (int)EnumCVRefStatus.Referred
               join i in _context.OrderItems on r.OrderItemId equals i.Id
               join o in _context.Orders on i.OrderId equals o.Id
               join c in _context.Customers on o.CustomerId equals c.Id
               join p in _context.Categories on i.CategoryId equals p.Id
               join cand in _context.Candidates on r.CandidateId equals cand.Id
               orderby r.ReferredOn
               select new SelectionsPendingDto
               {
                    CVRefId = r.Id,
                    OrderItemId = r.OrderItemId,
                    OrderNo = o.OrderNo,
                    CandidateName = cand.FullName,
                    ApplicationNo = cand.ApplicationNo,
                    CategoryName = p.Name,
                    CustomerName = c.CustomerName,
                    ReferredOn = r.ReferredOn.Date,
                    RefStatus = r.RefStatus
               }
          ).ToListAsync();
          /*
          var qry2 = await (from r in _context.CVRefs where r.RefStatus == (int)EnumCVRefStatus.Referred
               orderby r.ReferredOn
               select new SelectionsPendingDto {
                    CVRefId = r.Id, OrderItemId = r.OrderItemId, CandidateName = r.CandidateName, ApplicationNo = r.ApplicationNo,
                    CategoryName=r.CategoryName, CustomerName=r.CustomerName, ReferredOn = r.ReferredOn.Date,
                    RefStatus = r.RefStatus
               }).ToListAsync();
          */
          return qry2;
     }
}
}