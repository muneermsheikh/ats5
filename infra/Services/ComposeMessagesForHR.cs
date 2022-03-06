using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.EmailandSMS;
using core.Entities.HR;
using core.Entities.Identity;
using core.Interfaces;
using core.ParamsAndDtos;
using infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace infra.Services
{
    public class ComposeMessagesForHR: IComposeMessagesForHR
{
        private const string _smsNewLine = "<smsbr>";
        private const string _WAPNewLine = "<smsbr>";
        private readonly IEmployeeService _empService;
        private readonly ATSContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICommonServices _commonServices;
        private readonly IComposeMessages _composeMessages;
        private readonly IConfiguration _confg;
        private readonly IUnitOfWork _unitOfWork;
        public ComposeMessagesForHR(IEmployeeService empService, ATSContext context, 
            UserManager<AppUser> userManager, ICommonServices commonServices, 
            IComposeMessages composeMessages, IConfiguration confg, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _confg = confg;
            _composeMessages = composeMessages;
            _commonServices = commonServices;
            _userManager = userManager;
            _context = context;
            _empService = empService;
        }

        public async Task<ICollection<EmailMessage>> ComposeMessagesToDesignOrderAssessmentQs(int orderId, LoggedInUserDto loggedIn )
          {
               //Order.OrderItems can have different HR Supervisors who are tasked with designing assessment Q
               //So there will be one email message for each HR Supervisor
               //So the object to return will be collection of EmailMessage
               
               //compile data for use in the email message
               var loggedInUserObj = await _empService.GetEmployeeBriefAsyncFromAppUserId(loggedIn.LoggedInAppUserId);

               //details of the ORder Items for use in the email message table
               var orderItemDetails = await _context.OrderItems.Where(x => x.OrderId == orderId && x.RequireAssess==true)
                    .Select(x => new {
                         x.HrSupId, x.Id, x.OrderNo, x.SrNo, x.CategoryId, x.CategoryName, x.Quantity,
                         x.Ecnr, x.IndustryName, x.JobDescription})
                    .OrderBy(x => x.HrSupId).ThenBy(x => x.SrNo).ToListAsync();

               //find distinct HR Supervisors, each value will have a email message
               var RecipientIds = orderItemDetails.Select(x => x.HrSupId).Distinct().ToList();
               var ord = await _context.Orders.Where(x => x.Id == orderId)
                    .Select(x => new { x.OrderDate, x.OrderNo, x.CustomerId, x.CityOfWorking, x.CustomerName }).FirstOrDefaultAsync();

               var categoriesToDesignQ = new List<OrderAssessmentQObj>();       //the order categories for which assessment Q to design
               foreach (var recipientId in RecipientIds)
               {
                    var assessmentDetails = new List<OrderAssessmentQDetails>();
                    var dtls = orderItemDetails.Where(x => x.HrSupId == recipientId).ToList();

                    foreach (var d in dtls)
                    {
                         assessmentDetails.Add(new OrderAssessmentQDetails {
                              OrderItemId = d.Id,CategoryName = d.CategoryName, Quantity = d.Quantity, JDUrl = "todo"
                         });
                    }
                    var hruser = await _empService.GetEmployeeBriefAsyncFromEmployeeId((int)recipientId);
                    if (hruser==null) throw new Exception("HR Supervisor Id value for the category not defined");

                    var hrUserObj = await _userManager.FindByIdAsync(hruser.AppUserId.ToString());

                    categoriesToDesignQ.Add(new OrderAssessmentQObj{
                         EmployeeId = (int)recipientId, AppUserId = hrUserObj.Id, AppUserEmail = hrUserObj.Email, AppUserName = hrUserObj.UserName,
                         EmployeeFullName = hruser.EmployeeName + ", " + hruser.Position, OrderId = orderId,
                         OrderDate = ord.OrderDate.Date, OrderNo = ord.OrderNo, CustomerName = ord.CustomerName,  
                         City = ord.CityOfWorking, AssessmentQDetails = assessmentDetails
                    });
               };

               
               var senderObj = await _empService.GetEmployeeBriefAsyncFromAppUserId(loggedIn.LoggedInAppUserId);
               var emailmessages = new List<EmailMessage>();
               
               foreach (var item in categoriesToDesignQ)
               {
                    var content = DateTime.Now.Date + "<br><br>" + item.EmployeeFullName + "<br><br>" +
                         "Following categories require Shortlisted Candidate Assessment to accompany their profiles.  " +
                         "Please design questions for the same for approval of the undersigned before entrusting the HR Executives " +
                         "to assess shortlisted candidates accordingly.<br>";
                    content += "Customer: " + item.CustomerName + "<br>City of employment: " + item.City +
                         "<br>Order No.: " + item.OrderNo + " dated " + item.OrderDate + "<br>";
                    
                    content += "<table border='1'><tr><th>Sr No</th><th>Category Name</th><th> Qnty</th><th>Job Desc Url</th></tr>";
                    foreach (var i in item.AssessmentQDetails)
                    {
                         content += "<tr><td>" + i.SrNo + "</td><td>" + i.CategoryName + "</td><td>" + i.Quantity +
                              "</td><td>" + i.JDUrl + "</td></tr>";
                    }
                    content += "</table><br><br>For any clarification, refer the Job Description available at the above given url or consult the undersigned" +
                         "<br><br>Regards<br><br>" + senderObj.EmployeeName + "<br>" + senderObj.Position;
                    var emailMsg = new EmailMessage("HR", loggedIn.LoggedInAppUserId, item.AppUserId, senderObj.Email,
                         senderObj.EmployeeName, item.AppUserName, item.AppUserEmail, "", "",
                         "Task to design assessment Questions",
                         content, (int)EnumMessageType.OrderAssessmentQDesigning, 3);
                    emailmessages.Add(emailMsg);
               }

               return emailmessages;

          }
    
        public async Task<ICollection<EmailMessage>> ComposeMessagesToHRExecToSourceCVs(ICollection<OrderAssignmentDto> dtos)
        //, LoggedInUserDto loggedIn )
        {
            //todo - verify the tasks are not already assigned.  If so, advise to conclude existing tasks before assigning to new HR Executives
            /* var hrTasks = await _context.OrderItems.Where(x => orderItemIds.Contains(x.Id))
                .Select(x => new {x.HrExecId, x.Id, x.OrderId, x.CategoryId, x.CategoryName, x.Quantity, x.JobDescription})
                .ToListAsync();

            var order = await _context.Orders.Where(x => x.Id == orderItemIds.Select(x => x.OrderId).FirstOrDefault() )
                .Select(x => new {x.OrderNo, x.OrderDate, x.ProjectManagerId, x.CustomerId, x.CityOfWorking})
                .FirstOrDefaultAsync();               
            */
            var hrExecIds = dtos.Select(x => x.HrExecId).Distinct().ToList();
            var order = dtos.Select(x => new {x.OrderNo, x.CustomerId, x.OrderDate, x.ProjectManagerId, x.CityOfWorking}).FirstOrDefault();
            var msgs = new List<EmailMessage>();

            int projMgrId = order.ProjectManagerId == 0 ? 1 : order.ProjectManagerId;       //todo - correct this
            var projMgr = await _empService.GetEmployeeFromIdAsync(projMgrId);
            var cust = await _commonServices.CustomerBriefDetailsFromCustomerId(order.CustomerId);
            var postAction = dtos.Select(x => x.PostTaskAction).FirstOrDefault();

            foreach(var hrExecId in hrExecIds)
            {
                var hrExec = await _empService.GetEmployeeFromIdAsync((int)hrExecId);

                string msg = DateTime.Now.ToString("dd-MMMM-yyyy", CultureInfo.InvariantCulture)  + "<br><br>" + hrExec.EmployeeName;
                if (!string.IsNullOrEmpty(hrExec.Position)) msg += ", " + hrExec.Position + "<br>";
                msg += "Email: " + hrExec.OfficialEmailAddress + "<br><br>";
                
                msg += "Following tasks are assigned to you for mobilizing candidates as per their job descriptions:<br><br>";
                msg += "<tab><b>Order No.:</b>" + order.OrderNo + " dated " + order.OrderDate.ToString("dd-MMMM-yy", CultureInfo.InvariantCulture) +"</tab>" +
                        "<br><tab><b>Customer:</b>" + cust.CustomerName + ", " + cust.City + "</tab><br>, Place of work: " + order.CityOfWorking;
                msg += "<br><tab>For Job Descriptions, click the relevant link</tab><br>";
                
                var ids = dtos.Where(x => x.HrExecId == hrExecId).Select(x => x.OrderItemId).ToList();
                var orderitems = await _context.OrderItems.Where(x => ids.Contains(x.Id) && x.Remuneration != null)
                        .Include(x => x.Remuneration).ToListAsync();
                
                var tbl = await _composeMessages.TableOfOrderItemsContractReviewedAndApproved(
                        dtos.Where(x => x.HrExecId == hrExecId).Select(x => x.OrderItemId).ToList());
                msg += tbl + "<br><br>Please also check for your task dashboard for these tasks";
                msg += "<br>end of system generated message";

                var emailMsg = new EmailMessage("AssignTasksToHRExec", projMgr.AppUserId, hrExec.AppUserId, 
                        projMgr.OfficialEmailAddress, projMgr.UserName, hrExec.UserName, hrExec.OfficialEmailAddress, "", "", 
                        "Tasks to mobilize suitable candidates - Order No. " + order.OrderNo,
                        msg, (int)EnumMessageType.TaskAssignmentToHRExecToShortlistCV, postAction);
                msgs.Add(emailMsg);
            }
            return msgs;
        }

        public async Task<ICollection<EmailMessage>> ComposeMessagesToHRSupToReviewCVs(ICollection<CVsSubmittedDto> cvsSubmitted, LoggedInUserDto loggedIn)
        {
            //todo - verify the tasks are not already assigned.  If so, advise to conclude existing tasks before assigning to new HR Executives
            var msgs = new List<EmailMessage>();

            var orderitems = await _context.OrderItems.Where(x => cvsSubmitted.Select(x => x.OrderItemId).ToList().Contains(x.Id))
                .Select(x => new {x.HrExecId, x.Id, x.OrderId, x.HrSupId, x.CategoryId, x.CategoryName, x.Quantity, x.JobDescription})
                .ToListAsync();

            var order = await _context.Orders.Where(x => x.Id == orderitems.Select(x => x.OrderId).FirstOrDefault())
                .Select(x => new {x.Id, x.OrderNo, x.OrderDate, x.ProjectManagerId, x.CustomerId, x.CityOfWorking})
                .FirstOrDefaultAsync();

            var assignedToIds = orderitems.Select(x => x.HrSupId).Distinct().ToList();
            var ownerids = orderitems.Select(x => x.HrExecId).Distinct().ToList();
            var cust = await _commonServices.CustomerBriefDetailsFromCustomerId(order.CustomerId);

            foreach(var ownerid in ownerids)
            {
                var owner = await _empService.GetEmployeeBriefAsyncFromEmployeeId((int)ownerid);
                //select HRExecIds for all records where HRSupId is assignedToId
                var assigneeids = orderitems.Where(x => x.HrExecId == ownerid).Select(x => x.HrSupId).Distinct().ToList();

                foreach(var i in assigneeids)
                {
                        var ownerAndAssignees = cvsSubmitted.Where(x => x.AssignedToId == i && x.TaskOwnerId == ownerid).ToList();
                        var tbl = _composeMessages.TableOfCVsSubmittedByHRExecutives(ownerAndAssignees);

                        var assignee = await _empService.GetEmployeeFromIdAsync((int)i); //task owner

                        var msg = DateTime.Now.Date + "<br><br>" + assignee.EmployeeName + ", " + assignee.Position + "<br>" +
                            "<br>Email: " + assignee.OfficialEmailAddress + "<br><br><b>Task: To review cVs submitted by HR Executives.</b>";
                        msg += "<br><br>Please review following CVs submitted by HR Executives for your approval.<br>";
                        msg += "Order No.:" + order.OrderNo + " dated " + order.OrderDate +
                            "<br>Customer:" + cust.CustomerName + ", " + cust.City + ", Place of work: " + order.CityOfWorking;
                        msg += tbl;
                        msg += "<br><br>Please also check for your task dashboard for these tasks";

                        var emailMsg = new EmailMessage("AssignTasksToHRExec", owner.AppUserId, owner.AppUserId, 
                            owner.OfficialEmailAddress, owner.UserName, assignee.UserName, assignee.OfficialEmailAddress, "", "", 
                            "Tasks to mobilize suitable candidates - Order No. " + order.OrderNo,
                            msg, (int)EnumMessageType.TaskAssignmentToHRExecToShortlistCV, 3);
                        msgs.Add(emailMsg);
                }
            }

            return msgs;
        }

        public async Task<ICollection<EmailMessage>> ComposeMessagesToHRMToReviewCVs(ICollection<CVsSubmittedDto> cvsSubmitted, LoggedInUserDto loggedIn)
        {
            //todo - verify the tasks are not already assigned.  If so, advise to conclude existing tasks before assigning to new HR Executives
            var msgs = new List<EmailMessage>();

            var orderitems = await _context.OrderItems.Where(x => cvsSubmitted.Select(x => x.OrderItemId).ToList().Contains(x.Id))
                .Select(x => new {x.Id, x.OrderId, x.HrSupId, x.HrmId, x.CategoryId, x.CategoryName, x.Quantity, x.JobDescription})
                .ToListAsync();

            var order = await _context.Orders.Where(x => x.Id == orderitems.Select(x => x.OrderId).FirstOrDefault())
                .Select(x => new {x.Id, x.OrderNo, x.OrderDate, x.ProjectManagerId, x.CustomerId, x.CityOfWorking})
                .FirstOrDefaultAsync();

            var assignedToIds = orderitems.Select(x => x.HrmId).Distinct().ToList();
            var ownerids = orderitems.Select(x => x.HrSupId).Distinct().ToList();
            var cust = await _commonServices.CustomerBriefDetailsFromCustomerId(order.CustomerId);

            foreach(var ownerid in ownerids)
            {
                var owner = await _empService.GetEmployeeBriefAsyncFromEmployeeId((int)ownerid);
                //select HRExecIds for all records where HRSupId is assignedToId
                var assigneeids = orderitems.Where(x => x.HrSupId == ownerid).Select(x => x.HrmId).Distinct().ToList();

                foreach(var i in assigneeids)
                {
                        var ownerAndAssignees = cvsSubmitted.Where(x => x.AssignedToId == i && x.TaskOwnerId == ownerid).ToList();
                        var tbl = _composeMessages.TableOfCVsSubmittedByHRSup(ownerAndAssignees);

                        var assignee = await _empService.GetEmployeeFromIdAsync((int)i); //task owner

                        var msg = DateTime.Now.Date + "<br><br>" + assignee.EmployeeName + ", " + assignee.Position + "<br>" +
                            "<br>Email: " + assignee.OfficialEmailAddress + "<br><br><b>Task: To review cVs submitted by HR Executives.</b>";
                        msg += "<br><br>Please review following CVs submitted by HR Executives for your approval.<br>";
                        msg += "Order No.:" + order.OrderNo + " dated " + order.OrderDate +
                            "<br>Customer:" + cust.CustomerName + ", " + cust.City + ", Place of work: " + order.CityOfWorking;
                        msg += tbl;
                        msg += "<br><br>Please also check for your task dashboard for these tasks";

                        var emailMsg = new EmailMessage("AssignTasksToHRExec", owner.AppUserId, owner.AppUserId, 
                            owner.OfficialEmailAddress, owner.UserName, assignee.UserName, assignee.OfficialEmailAddress, "", "", 
                            "Tasks to mobilize suitable candidates - Order No. " + order.OrderNo,
                            msg, (int)EnumMessageType.TaskAssignmentToHRExecToShortlistCV,3);
                        msgs.Add(emailMsg);
                }
            }

            return msgs;

        }

        public Task<ICollection<EmailMessage>> ComposeMessagesToDocControllerAdminToForwardCVs(ICollection<CVsSubmittedDto> cvsSubmitted, LoggedInUserDto loggedIn)
        {
            throw new NotImplementedException();
        }
        public async Task<EmailMessage> ComposeHTMLToAckToCandidateByEmail(CandidateMessageParamDto candidateParam)
        {
            var candidate = candidateParam.Candidate;
            string msg = "";
            var candidateName = candidate.FullName;
            var email = candidate.Email;
            var title = candidate.Gender.ToLower() == "m" ? "Mr." : "Ms.";

            string subject = "";

            subject = "Your Application is registered with us under Sr. No. " + candidate.ApplicationNo;
            msg = DateTime.Today + "<br><br>" + title + " " + candidateName + "<br><br>Dear " + title + " " + candidateName;
            msg += "Dear " + title + " " + candidateName + "<br><br>" + subject + "<br><br>";
            var msgs = await _context.MessageComposeSources.Where(x => x.MessageType.ToLower() == "acknowledgementtocandidate" && x.Mode == "mail")
                .OrderBy(x => x.SrNo).ToListAsync();
            foreach (var m in msgs)
            {
                msg += m.LineText == "tableofrelevantopenings"
                        ? await _composeMessages.TableOfRelevantOpenings(
                            candidate.UserProfessions.Select(x => x.CategoryId).ToList())
                        : m.LineText;
            }
            msg = msg + "<br><br>HR Supervisor";

            var emailMessage = new EmailMessage
            {
                SenderEmailAddress = _confg["EmailSenderEmailId"],
                SenderUserName = _confg["EmailSenderDisplayName"],
                RecipientUserName = candidate.KnownAs,
                RecipientEmailAddress = email,
                CcEmailAddress = _confg["EmailCCandAck"],
                BccEmailAddress = _confg["EmailBCCandAck"],
                Subject = subject,
                Content = msg,
                MessageTypeId = (int)EnumMessageType.CVAcknowledgementByEMail
            };

            return emailMessage;
        }

        public async Task<SMSMessage> ComposeMsgToAckToCandidateBySMS(CandidateMessageParamDto candidateParam)
          {
               var candidate = candidateParam.Candidate;
               string msg = "";
               var candidateName = candidate.KnownAs;
               var title = candidate.Gender.ToLower() == "m" ? "Mr." : "Ms.";

               string subject = "";

               var mobileNo = candidate.UserPhones.Where(x => x.IsMain && x.IsValid).Select(x => x.MobileNo).FirstOrDefault();
               if (string.IsNullOrEmpty(mobileNo)) return null;

               subject = "Your Application registered with us under Sr. No. " + candidate.ApplicationNo;
               msg = DateTime.Today + _smsNewLine + _smsNewLine + title + " " + candidateName + _smsNewLine + _smsNewLine + "Dear " + title + " " + candidateName;
               var sms = await _context.MessageComposeSources.Where(x => x.MessageType.ToLower() == "acknowledgementtocandidate" && x.Mode == "sms")
                    .OrderBy(x => x.SrNo).ToListAsync();
               foreach (var m in sms)
               {
                    msg += m;
               }
               msg += _smsNewLine + _smsNewLine + "HR Supervisor";

               var smsMessage = new SMSMessage
               {
                    UserId = candidate.AppUserId,
                    PhoneNo = mobileNo,
                    SMSText = msg
               };

               return smsMessage;
          }

        public async Task<EmailMessage> ComposeHTMLToPublish_CVReadiedToForwardToClient(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId)
          {
               var emp = await _empService.GetEmployeeBriefAsyncFromAppUserId(loggedInDto.LoggedInAppUserId);
               var recipient = await _empService.GetEmployeeBriefAsyncFromEmployeeId(recipientId);

               var msg = DateTime.Now.Date + "<br><br>"+ emp.EmployeeName + "<br>" + emp.Position + "<br>email:"+ emp.Email +
                    "Following Applications are cleared for forwarding to respective clients by the Document Controller:<br><br>" +
                    "<table border='1'><tr><th>App No</th><th>Candidate Name</th><th>Category Ref</th><th>Customer</th><th>Submitted by</th></tr>";
               foreach(var item in commonDataDtos)
               {
                    msg += "<tr><td>" + item.ApplicationNo + "</td><td>" + item.CandidateName + "</td><td>" + 
                         item.CategoryName+"</td><td>" + item.CustomerName + "</td><td>" + emp.EmployeeName + "</td></tr>";
               }

               msg +="</table>Regards<br><br>This is system generated message";

               var email = new EmailMessage("advisory", loggedInDto.LoggedInAppUserId, recipient.AppUserId, loggedInDto.LoggedInAppUserEmail, 
                    loggedInDto.LoggedIAppUsername, recipient.KnownAs, recipient.Email, "", "", 
                    "Applications readied to forward to client - initiated by " + emp.EmployeeName, msg, 
                    (int)EnumMessageType.CVForwardingToDocControllerToFwdCVToClient,3);
               
               return email;
          }

        public async Task<EmailMessage> ComposeHTMLToPublish_CVSubmittedToHRSup(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId)
          {
               var emp = await _empService.GetEmployeeBriefAsyncFromAppUserId(loggedInDto.LoggedInAppUserId);
               var recipient = await _empService.GetEmployeeBriefAsyncFromEmployeeId(recipientId);

               var msg = DateTime.Now.Date + "<br><br>"+ emp.EmployeeName + "<br>" + emp.Position + "<br>email:"+ emp.OfficialEmailAddress +
                    "Following Applications are submitted to the HR Supervisor for his review:<br><br>" +
                    "<table border='1'><tr><th>App No</th><th>Candidate Name</th><th>Category Ref" + 
                    "</th><th>Customer</th><th>Submitted by</th></tr>";
               foreach(var item in commonDataDtos)
               {
                    msg += "<tr><td>" + item.ApplicationNo + "</td><td>" + item.CandidateName + "</td><td>"+item.CategoryName + 
                         "</td><td>" + item.CustomerName + "</td><td>" + emp.EmployeeName + "</td></tr>";
               }

               msg +="</table>Regards<br><br>This is system generated message";

               var email = new EmailMessage("advisory", loggedInDto.LoggedInAppUserId, recipient.AppUserId, loggedInDto.LoggedInAppUserEmail, 
                    loggedInDto.LoggedIAppUsername, recipient.KnownAs, recipient.Email, "", "", 
                    "Applications submitted to HR Supervisor for review, initiated by " + emp.EmployeeId, msg, 
                    (int)EnumMessageType.Publish_CVReviewedByHRSup, 3);
               
               return email;

          }

        public async Task<EmailMessage> ComposeHTMLToPublish_CVReviewedByHRSup(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId)
          {
               var emp = await _empService.GetEmployeeBriefAsyncFromAppUserId(loggedInDto.LoggedInAppUserId);
               var recipient = await _empService.GetEmployeeBriefAsyncFromEmployeeId(recipientId);

               var msg = DateTime.Now.Date + "<br><br>"+ emp.EmployeeName + "<br>" + emp.Position + "<br>email:"+ emp.Email +
                    "Following CVs are reviewed by the HR Supervisor:<br><br>" +
                    "<table border='1'><tr><th>App No</th><th>Candidate Name</th><th>Category Ref" + 
                    "</th><th>Customer</th><th>Submitted by</th><th>Review Result</th></tr>";
               foreach(var item in commonDataDtos)
               {
                    msg += "<tr><td>" + item.ApplicationNo + "</td><td>" + item.CandidateName + "</td><td>"+item.CategoryName + 
                         "</td><td>" + item.CustomerName + "</td><td>" + emp.EmployeeName + "</td><td>" + item.ReviewResultId + "</tr>";
               }

               msg +="</table><br><br>Regards<br><br>This is system generated message";

               var email = new EmailMessage("advisory", loggedInDto.LoggedInAppUserId, recipient.AppUserId, loggedInDto.LoggedInAppUserEmail, 
                    loggedInDto.LoggedIAppUsername, recipient.KnownAs, recipient.Email, "", "", 
                    "Applications submitted to HR Supervisor for review, initiated by " + emp.EmployeeId, msg, 
                    (int)EnumMessageType.Publish_CVReviewedByHRSup, 3);
               
               return email;
          }

        public async Task<EmailMessage> ComposeHTMLToPublish_CVReviewedByHRManager(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId)
        {
            var emp = await _empService.GetEmployeeBriefAsyncFromAppUserId(loggedInDto.LoggedInAppUserId);
            var recipient = await _empService.GetEmployeeBriefAsyncFromEmployeeId(recipientId);
            if (recipient == null) return null;

            var msg = DateTime.Now.Date + "<br><br>"+ emp.EmployeeName + "<br>" + emp.Position + "<br>email:"+ emp.Email +
                "Following Applications are reviewed by HR Manager and approved for forwarding to respective customers:<br><br>" +
                "<table border='1'><tr><th>App No</th><th>Candidate Name</th><th>Category Ref</th><th>" + 
                "Customer</th><th>Submitted by</th><th>Review Result</th></tr>";
            foreach(var item in commonDataDtos)
            {
                msg += "<tr><td>" + item.ApplicationNo + "</td><td>" + item.CandidateName + "</td><td>"+item.CategoryName + 
                        "</td><td>" + item.CustomerName + "</td><td>" + emp.EmployeeName + "</td><td>" + item.ReviewResultId + "</td></tr>";
            }

            msg +="</table><br><br>Regards<br><br>This is system generated message";

            var email = new EmailMessage("advisory", loggedInDto.LoggedInAppUserId, recipient.AppUserId, loggedInDto.LoggedInAppUserEmail, 
                loggedInDto.LoggedIAppUsername, recipient.KnownAs, recipient.Email, "", "", 
                "Applications readied for forwarding to Customers" + emp.EmployeeId, msg, 
                (int)EnumMessageType.Publish_CVReviewedByHRManager, 3);
            
            return email;
        }
     
        private ICollection<EmailMessage> ComposeHTMLForwards(OrderItemsAndAgentsToFwdDto itemsAndAgents,  EmployeeDto senderObj, int loggedInUserId)
        {
            
            var catTable = ComposeCategoryTableForDLFwd(itemsAndAgents.Items);
            
            var msgs = new List<EmailMessage>();
            foreach(var agent in itemsAndAgents.Agents) {
                var hdr = DateTime.Now + "<br><br>" + agent.Title + " " + agent.CustomerOfficialName;
                if(!string.IsNullOrEmpty(agent.OfficialDesignation)) hdr +=", " + agent.OfficialDesignation;
                hdr +="<br>" + agent.CustomerName + "<br>" + agent.CustomerCity;
                hdr += "email: " + agent.OfficialEmailId + "<br><br>";
                hdr += "Sub.: Manpower requirement for " + agent.CustomerCity;
                hdr += "<br><br>We have following manpower requirement.  If you have interested candidates, please refer them to us<br><br>";
                hdr += catTable + "<br><br>Regards<br><br>" + senderObj.EmployeeName + "<br>" + senderObj.Position;

                var message = new EmailMessage("DLFwdToAgent", loggedInUserId,
                        agent.CustomerOfficialId, agent.OfficialEmailId,
                        senderObj.OfficialEmailAddress, agent.CustomerName,
                        agent.OfficialEmailId, "","", "Requirement", hdr, 
                        (int)EnumMessageType.DLForwardToAgents,0 );
                msgs.Add(message);
            }

            return msgs;
        }

        private ICollection<SMSMessage> ComposeSMSForwards(OrderItemsAndAgentsToFwdDto itemsAndAgents, EmployeeDto senderObj, int loggedInUserId)
        {
            var catTable = ComposeCategoryTableForDLFwdBySMS(itemsAndAgents.Items);
            string deliveryresult ="";
            var msgs = new List<SMSMessage>();
            foreach(var agent in itemsAndAgents.Agents) {
                var hdr = agent.Title + " " + agent.CustomerOfficialName + _smsNewLine;
                hdr += _smsNewLine + "We hv flg manpower requirement.  If u hv interested candidates, pl refer them to us" + _smsNewLine + _smsNewLine;
                hdr += catTable + "Rgds" + _smsNewLine + senderObj.EmployeeName + _smsNewLine + senderObj.Position;

                var message = new SMSMessage(loggedInUserId, itemsAndAgents.DateForwarded, agent.Phoneno, hdr, deliveryresult);
                _unitOfWork.Repository<SMSMessage>().Add(message);
                msgs.Add(message);
            }
            
            return null;
        }

        private ICollection<SMSMessage> ComposeWhatsAppForwards(OrderItemsAndAgentsToFwdDto itemsAndAgents, EmployeeDto senderObj, int loggedInUserId)
        {
            var catTable = ComposeCategoryTableForDLFwdByWhatsApp(itemsAndAgents.Items);
            string deliveryresult ="";
            var msgs = new List<SMSMessage>();
            foreach(var agent in itemsAndAgents.Agents) {
                var hdr = agent.Title + " " + agent.CustomerOfficialName + _smsNewLine;
                hdr += _smsNewLine + "We hv flg manpower requirement.  If u hv interested candidates, pl refer them to us" + _smsNewLine + _smsNewLine;
                hdr += catTable + "Rgds" + _smsNewLine + senderObj.EmployeeName + _smsNewLine + senderObj.Position;

                var message = new SMSMessage(loggedInUserId, itemsAndAgents.DateForwarded, agent.Phoneno, hdr, deliveryresult);
                _unitOfWork.Repository<SMSMessage>().Add(message);
                msgs.Add(message);
            }
            return null;
        }

        public async Task<MessagesToReturnDto> ComposeHTMLToForwardEnquiryToAgents(OrderItemsAndAgentsToFwdDto itemsAndagents, int LoggedInUserId)
        {
            var senderObj = await _empService.GetEmployeeBriefAsyncFromEmployeeId(
                itemsAndagents.Items.Select(x => x.ProjectManagerId).FirstOrDefault());
            if (senderObj == null) return null;

            var emailmsgs = new List<EmailMessage>();
            var smsmsgs = new List<SMSMessage>();
            var whatsappmsgs= new List<SMSMessage>();
            
            var msgsToReturnDto = new MessagesToReturnDto();

            var orderitemsandagentstofwd = new OrderItemsAndAgentsToFwdDto();
            var agentsByEmail = itemsAndagents.Agents.Where(x => x.Checked==true).ToList();
            if (agentsByEmail != null && agentsByEmail.Count > 0) {
                orderitemsandagentstofwd.Agents = agentsByEmail;
                orderitemsandagentstofwd.Items = itemsAndagents.Items;
                emailmsgs  = (List<EmailMessage>)ComposeHTMLForwards(orderitemsandagentstofwd, senderObj, LoggedInUserId);
                if (emailmsgs !=null && emailmsgs.Count() > 0) msgsToReturnDto.EmailMessages = emailmsgs;
            }
            
            var agentsBySMS = itemsAndagents.Agents.Where(x => x.CheckedPhone==true).ToList();
            if (agentsBySMS != null && agentsBySMS.Count > 0) {
                orderitemsandagentstofwd = new OrderItemsAndAgentsToFwdDto();
                orderitemsandagentstofwd.Agents = agentsBySMS;
                orderitemsandagentstofwd.Items = itemsAndagents.Items;
                smsmsgs  = (List<SMSMessage>)ComposeSMSForwards(orderitemsandagentstofwd, senderObj, LoggedInUserId);
                if (smsmsgs != null && smsmsgs.Count() > 0) msgsToReturnDto.SMSMessages = smsmsgs;
            }

            var agentsByWhatsApp = itemsAndagents.Agents.Where(x => x.CheckedMobile==true).ToList();
            if (agentsByWhatsApp != null && agentsByWhatsApp.Count > 0) {
                orderitemsandagentstofwd = new OrderItemsAndAgentsToFwdDto();
                orderitemsandagentstofwd.Agents = agentsByWhatsApp;
                orderitemsandagentstofwd.Items = itemsAndagents.Items;
                smsmsgs  = (List<SMSMessage>)ComposeWhatsAppForwards(orderitemsandagentstofwd, senderObj, LoggedInUserId);
                if (smsmsgs !=null && smsmsgs.Count() > 0) msgsToReturnDto.WhatsAppMessages = smsmsgs;
            }
            return msgsToReturnDto;
        }
        public async Task<ICollection<SMSMessage>> ForwardEnquiryToAgentsBySMS(OrderItemsAndAgentsToFwdDto itemsAndagents, int LoggedInUserId)
        {
            var senderObj = await _empService.GetEmployeeBriefAsyncFromEmployeeId(
                itemsAndagents.Items.Select(x => x.ProjectManagerId).FirstOrDefault());
            if (senderObj == null) return null;

            var catTable = ComposeCategoryTableForDLFwdBySMS(itemsAndagents.Items);
            string deliveryresult ="";
            var msgs = new List<SMSMessage>();
            foreach(var agent in itemsAndagents.Agents) {
                var hdr = agent.Title + " " + agent.CustomerOfficialName + _smsNewLine;
                hdr += _smsNewLine + "We hv flg manpower requirement.  If u hv interested candidates, pl refer them to us" + _smsNewLine + _smsNewLine;
                hdr += catTable + "Rgds" + _smsNewLine + senderObj.EmployeeName + _smsNewLine + senderObj.Position;

                var message = new SMSMessage(LoggedInUserId, itemsAndagents.DateForwarded, agent.Phoneno, hdr, deliveryresult);
                _unitOfWork.Repository<SMSMessage>().Add(message);
                msgs.Add(message);
            }
            
            if (await _unitOfWork.Complete() > 0) return (ICollection<SMSMessage>)msgs;
            return null;
        }

        private string ComposeCategoryTableForDLFwd(ICollection<OrderItemToFwdDto> orderitems) {
               
               var para = "<Table border='1'><tr><th>Order Ref</th><th>Customer</th>" +
                    "<th>Category</th><th>Quantity</th><th>Basic Salary</th>" +
                    "<th>Job Description</th>" +
                    "<th>Remuneration and Facilities</th></tr>";
               
               foreach(var item in orderitems ) {
                    para += "<tr><td>" + item.CategoryRef + "</td><td>" + item.CustomerName + "</td><td>" +
                         item.CategoryName + "</td><td>" + item.Quantity + "</td><td>" +
                         item.SalaryCurrency + " " + 
                         item.BasicSalary  + "<td><td>" +
                         item.JobDescriptionURL + "</td><td>" + 
                         item.RemunerationURL + "</td></tr>";
               }
               para += "</table>";

               return para;
          }

        private string ComposeCategoryTableForDLFwdBySMS(ICollection<OrderItemToFwdDto> orderitems) {
               
            int srno=0;              
            string para="";
            foreach(var item in orderitems ) {
                para += ++srno + ". Ref:" + item.CategoryRef + item.CategoryName + _smsNewLine +
                        "Customer: " + item.CustomerName + _smsNewLine +
                        "Qnty: " + item.Quantity + _smsNewLine +
                        "Salary: " + item.SalaryCurrency + " " + item.BasicSalary  + _smsNewLine +
                        "Job Description: " + item.JobDescriptionURL + _smsNewLine + 
                        "Remuneration: " + item.RemunerationURL  + _smsNewLine;
            }

            return para;
        }

        private string ComposeCategoryTableForDLFwdByWhatsApp(ICollection<OrderItemToFwdDto> orderitems) {
               
            int srno=0;              
            string para="";
            foreach(var item in orderitems ) {
                para += ++srno + ". Ref:" + item.CategoryRef + item.CategoryName + _WAPNewLine +
                        "Customer: " + item.CustomerName + _WAPNewLine +
                        "Qnty: " + item.Quantity + _WAPNewLine +
                        "Salary: " + item.SalaryCurrency + " " + item.BasicSalary  + _WAPNewLine +
                        "Job Description: " + item.JobDescriptionURL + _WAPNewLine + 
                        "Remuneration: " + item.RemunerationURL  + _WAPNewLine;
            }

            return para;
        }

    }
    
}