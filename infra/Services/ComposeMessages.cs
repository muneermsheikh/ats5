using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using core.Entities;
using core.Entities.EmailandSMS;
using core.Entities.HR;
using core.Entities.Identity;
using core.Entities.Orders;
using core.Entities.Tasks;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace infra.Services
{
     public class ComposeMessages : IComposeMessages
     {
          private readonly ATSContext _context;
          private const string _smsNewLine = "<smsbr>";
          private readonly IStatsService _statsService;
          private readonly ICommonServices _commonServices;
          private readonly IConfiguration _confg;
          private readonly IEmployeeService _empService;
          private readonly UserManager<AppUser> _userManager;
          private readonly string _senderEmailAddress; 
          private readonly string _senderUserName;

          public ComposeMessages(ATSContext context, IStatsService statsService, ICommonServices commonServices,
                    IConfiguration confg, IEmployeeService empService, UserManager<AppUser> userManager)
          {
               _userManager = userManager;
               _empService = empService;
               _confg = confg;
               _commonServices = commonServices;
               _statsService = statsService;
               _context = context;
               _senderEmailAddress = _confg.GetSection("EmailSenderEmailId").Value;
               _senderUserName = _confg.GetSection("EmailSenderDisplayName").Value;
                         
          }

          public async Task<EmailMessage> AckToCandidateByEmail(CandidateMessageParamDto candidateParam)
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
                         ? await TableOfRelevantOpenings(
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

          public async Task<SMSMessage> AckToCandidateBySMS(CandidateMessageParamDto candidateParam)
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

          public async Task<ICollection<EmailMessage>> AdviseSelectionStatusToCandidateByEmail(ICollection<SelectionDecisionMessageParamDto> selectionsDto)
          {
               var sels = selectionsDto.Select(x => x.SelectionDecision).ToList();
               var qry = (from s in sels 
                    join c in _context.Candidates on s.CandidateId equals c.Id
                    join i in _context.OrderItems on s.OrderItemId equals i.Id
                    join e in _context.Employees on i.HrExecId equals e.Id
                    select new {s, c.Gender, c.Email, c.KnownAs, HRExecKnownAs=e.KnownAs, 
                         HRExecEmail=e.Email, c.AppUserId})
                    .ToList();

               string msg = "";
               var msgs = new List<EmailMessage>();
               string subject = "";

               foreach(var sel in qry)
               {
                    if (string.IsNullOrEmpty(sel.Email)) continue;

                    var title = sel.Gender.ToLower() == "m" ? "Mr." : "Ms.";
                    subject = "<b><u>Subject: </b>Your selection as " + sel.s.CategoryName + " for " + sel.s.CustomerName + "</u>";
                    msg = string.Format("{0: dd-MMMM-yyyy}", DateTime.Today) + "<br><br>" + 
                         title + " " + sel.s.CandidateName + "email: " + sel.Email + "<br><br>" + 
                         "copy: " + sel.HRExecKnownAs + ", email: " + sel.HRExecEmail + "<br><br>Dear " + title + " " + sel.s.CandidateName + ":" +
                         "<br><br>" + subject + "<br><br>";

                    //MessageComposeSources contains collection of static text lines for each type of message.
                    var msgLines = await _context.MessageComposeSources
                         .Where(x => x.MessageType.ToLower() == "selectionadvisetocandidate" && x.Mode == "mail")
                         .OrderBy(x => x.SrNo).ToListAsync();
                    foreach (var m in msgLines)
                    {
                         //if m.LineText equals "<tableofselectiondetails>", then it is a dynamic data, to be
                         //retreived from SelectionDecision, else accept the static data m.LineText
                         msg += m.LineText == "<tableofselectiondetails>" ? GetSelectionDetails(sel.s) : m.LineText;
                    }
                    msg += "<br>Best Regards<br>HR Supervisor";

                    var emailMessage = new EmailMessage
                    {
                         RecipientId = sel.AppUserId,
                         RecipientUserName = sel.s.CandidateName,
                         RecipientEmailAddress = sel.Email + ", " + sel.HRExecEmail,       //TODO - HRExecEmail included in Recipient, as CC and BCC not working
                         CcEmailAddress = sel.HRExecEmail,
                         BccEmailAddress = "",
                         Subject = subject,
                         Content = msg,
                         MessageTypeId = (int)EnumMessageType.SelectionAdvisebyemail
                    };

                    msgs.Add(emailMessage);
               }

               if (msgs.Count > 0) return msgs;
               return null;
          }
          public async Task<SMSMessage> AdviseSelectionStatusToCandidateBySMS(SelectionDecisionMessageParamDto selectionParam)
          {
               var selection = selectionParam.SelectionDecision;
               var candidate = await (from cvref in _context.CVRefs
                                   where cvref.Id == selection.CVRefId
                                   join cand in _context.Candidates on cvref.CandidateId equals cand.Id
                                   select cand).FirstOrDefaultAsync();

               var candidateName = candidate.KnownAs;
               var title = candidate.Gender.ToLower() == "m" ? "Mr." : "Ms.";

               string msg = "";

               var mobileNo = candidate.UserPhones.Where(x => x.IsMain && x.IsValid).Select(x => x.MobileNo).FirstOrDefault();
               if (string.IsNullOrEmpty(mobileNo)) return null;

               msg = "Dear " + title + " " + candidateName + _smsNewLine + _smsNewLine;
               var msgssms = await _context.MessageComposeSources.Where(x => x.MessageType.ToLower() == "selectionadvisetocandidate" && x.Mode == "sms")
                    .OrderBy(x => x.SrNo).ToListAsync();
               foreach (var m in msgssms)
               {
                    msg += m.LineText == "<tableofselectiondetailssms>" ? GetSelectionDetailsBySMS(selection) : m.LineText;
               }
               msg += msg + "<br><br>HR Supervisor";

               var smsMessage = new SMSMessage
               {
                    UserId = candidate.AppUserId,
                    PhoneNo = mobileNo,
                    SMSText = msg
               };

               return smsMessage;
          }

          public ICollection<EmailMessage> AdviseRejectionStatusToCandidateByEmail(ICollection<RejDecisionToAddDto> rejectionsDto)
          {
               //var rejs = rejectionsDto.Select(x => x.SelectionDecision).ToList();
               var qry = (from r in rejectionsDto
                    join rf in _context.CVRefs on r.CVRefId equals rf.Id
                    join c in _context.Candidates on rf.CandidateId equals c.Id
                    join i in _context.OrderItems on rf.OrderItemId equals i.Id
                    join e in _context.Employees on i.HrExecId equals e.Id
                    select new {rf.CandidateName, rf.CategoryName, rf.CustomerName, r.SelectionStatusId,
                         c.Gender, c.Email, c.KnownAs, c.FullName, 
                         HRExecKnownAs = e.KnownAs, HRExecEmail = e.Email, c.AppUserId, 
                         CandidateEmail = c.Email})
                    .ToList();

               var msg = "";
               var msgs = new List<EmailMessage>();
               var subject = "";

               foreach(var rej in qry)
               {
                    if (string.IsNullOrEmpty(rej.Email)) continue;

                    var title = rej.Gender.ToLower() == "m" ? "Mr. " : "Ms. ";
                    subject = "<u><b>Subject</b>: Your candidature for the position of " + rej.CategoryName + 
                         " <b>has not been approved</b> by " + rej.CustomerName + "</u>";
                    msg = string.Format("{0: dd-MMMM-yyyy}", DateTime.Today) + "<br><br>" + title + " " + 
                         rej.CandidateName +  ", email: " + rej.Email + "<br><br>Dear " + title + " " + rej.CandidateName +
                         "<br><br>" + subject + "<br><br>";
                    msg += "We regret to inform you that M/S " + rej.CustomerName + " have not approved of your candidature for the position " +
                         "of " + rej.CategoryName + " giving following reason:<br><ul><li>" + rej.SelectionStatusId + "</li></ul><br>";
                    msg += "The rejection by our Customer only indicates your profile did not meet their specific requirements; it " +
                         "does not reflect your suitability for the position in general. We will therefore be continuing to look for " +
                         "alternate opportunities for you and hope to revert to you as soon as possible.<br><br>" + 
                         "In case you do not want us to continue looking for opportunities for you, please do mark yourself as unavailable on our website https://idealsoln.in/candidates/notavailable" + 
                              "so as not to include your profile in our searches<br><br>Best regards/HR Supervisor";
                    msg += "<br><br>This is a system generated message";

                    var emailMessage = new EmailMessage
                    {
                         RecipientId = rej.AppUserId,
                         RecipientUserName = rej.Email,
                         RecipientEmailAddress = rej.Email,
                         CcEmailAddress = "",
                         BccEmailAddress = "",
                         Subject = subject,
                         Content = msg,
                         MessageTypeId = (int)EnumMessageType.RejectionAdviseByMail
                    };

                    msgs.Add(emailMessage);
               }

               if (msgs.Count > 0) return msgs;
               return null;
               
          }
          public async Task<SMSMessage> AdviseRejectionStatusToCandidateBySMS(SelectionDecisionMessageParamDto selectionParam)
          {
               var selection = selectionParam.SelectionDecision;
               var candidate = await (from cvref in _context.CVRefs
                                   where cvref.Id == selection.CVRefId
                                   join cand in _context.Candidates on cvref.CandidateId equals cand.Id
                                   select cand).FirstOrDefaultAsync();

               var candidateName = candidate.KnownAs;
               var title = candidate.Gender.ToLower() == "m" ? "Mr." : "Ms.";

               string msg = "";

               var mobileNo = candidate.UserPhones.Where(x => x.IsMain && x.IsValid).Select(x => x.MobileNo).FirstOrDefault();
               if (string.IsNullOrEmpty(mobileNo)) return null;

               msg = "Yr candidature for the position of " + selection.CategoryName + _smsNewLine;
               var msgssms = await _context.MessageComposeSources.Where(x => x.MessageType.ToLower() == "rejectionadvisetocandidate" && x.Mode == "sms")
                    .OrderBy(x => x.SrNo).ToListAsync();
               foreach (var m in msgssms)
               {
                    msg += m.LineText;
               }
               msg = msg + "<br><br>HR Supervisor";

               var smsMessage = new SMSMessage
               {
                    UserId = candidate.AppUserId,
                    PhoneNo = mobileNo,
                    SMSText = msg
               };

               return smsMessage;
          }

          public async Task<EmailMessage> AdviseProcessTransactionUpdatesToCandidateByEmail(DeployMessageParamDto deployParam)
          {
               var deploy = deployParam.Deploy;
               var candidate = await (from cvref in _context.CVRefs
                                   where cvref.Id == deploy.CVRefId
                                   join cand in _context.Candidates on cvref.CandidateId equals cand.Id
                                   select cand).FirstOrDefaultAsync();

               var candidateName = candidate.FullName;
               var email = candidate.Email;
               var title = candidate.Gender.ToLower() == "m" ? "Mr." : "Ms.";
               var subject = "Your Process Transaction Update";
               var msg = "";
               msg = title + " " + candidateName + "<br>Application No.:" + candidate.ApplicationNo + ", PP No.:" + candidate.PpNo;
               msg += "email: " + candidate.Email + "<br><br>Dear Sir:<br><br>please be advised of the following updates to your departure formalities<br><br>";
               msg += "<tab>Date: " + deploy.TransactionDate.Date;
               msg += "<br><tab>Transaction: " + _commonServices.DeploymentStageNameFromStageId(deploy.StageId);
               msg += deploy.NextStageId > 0 ? "<br><tab>Next Stage:" + _commonServices.DeploymentStageNameFromStageId(deploy.NextStageId) + ", scheduled by " + deploy.NextEstimatedStageDate.Date : "";
               msg += "<br><br>Regards/Processing Divn";

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
                    MessageTypeId = (int)EnumMessageType.MedicalExaminationAdvisebyEmail       //todo - change to individual process types
               };

               return emailMessage;
          }

          public async Task<SMSMessage> AdviseProcessTransactionUpdatesToCandidateBySMS(DeployMessageParamDto deployParam)
          {
               var deploy = deployParam.Deploy;
               var candidate = await (from cvref in _context.CVRefs
                                   where cvref.Id == deploy.CVRefId
                                   join cand in _context.Candidates on cvref.CandidateId equals cand.Id
                                   select cand).FirstOrDefaultAsync();

               var candidateName = candidate.KnownAs;
               var title = candidate.Gender.ToLower() == "m" ? "Mr." : "Ms.";

               var msg = "";

               var mobileNo = candidate.UserPhones.Where(x => x.IsMain && x.IsValid).Select(x => x.MobileNo).FirstOrDefault();
               if (string.IsNullOrEmpty(mobileNo)) return null;

               msg = title + " " + candidateName + ", PP No.:" + candidate.PpNo;
               msg += "pl be advised of flg updates to yr departure formalities<br><br>";
               msg += "<tab>Date: " + deploy.TransactionDate.Date;
               msg += "<br><tab>Transaction: " + _commonServices.DeploymentStageNameFromStageId(deploy.StageId);
               msg += deploy.NextStageId > 0 ? "<br><tab>Next Stage:" + _commonServices.DeploymentStageNameFromStageId(deploy.NextStageId) + ", scheduled by " + deploy.NextEstimatedStageDate.Date : "";
               msg += "<br><br>Regards/Processing Divn";

               var smsMessage = new SMSMessage
               {
                    UserId = candidate.AppUserId,
                    PhoneNo = mobileNo,
                    SMSText = msg
               };

               return smsMessage;
          }

          public async Task<EmailMessage> AckEnquiryToCustomer(OrderMessageParamDto orderMessageDto)
          {
               var order = orderMessageDto.Order;
               var customer = await _context.Customers.Where(x => x.Id == orderMessageDto.Order.CustomerId)
                    .Include(x => x.CustomerOfficials).FirstOrDefaultAsync();
               if (customer==null) throw new Exception("failed to retrieve customer data for customer no. " + orderMessageDto.Order.CustomerId);
               var OrderItems = orderMessageDto.Order.OrderItems.OrderBy(x => x.SrNo).ToList();
               var projectManagerId = order.ProjectManagerId == 0 ? 1021 : order.ProjectManagerId;
               EmployeeDto projManager = await _empService.GetEmployeeFromIdAsync(projectManagerId);

               string[] officialDepts = { "main contact", "hr", "accounts", "logistics" };
               CustomerOfficial official = null;
               foreach (var off in officialDepts)
               {
                    official = customer.CustomerOfficials.Where(x => x.Divn?.ToLower() == off).FirstOrDefault();
                    if (official != null) break;
               }
               bool HasException = false;
               var msg = DateTime.Now.Date + "<br><br>M/S" + customer.CustomerName;
               if (!string.IsNullOrEmpty(customer.Add)) msg += "<br>" + customer.Add;
               if (!string.IsNullOrEmpty(customer.Add2)) msg += "<br>" + customer.Add2;
               msg += "<br>" + customer.City + ", " + customer.Country + "<br><br>";
               msg += official == null ? "" : "Kind Attn : " + official.Title + official.OfficialName + ", " + official.Designation + "<br><br>";
               msg += "Dear " + official?.Gender == "F" ? "Madam:" : "Sir:" + "<br><br>";
               msg += "Thank you very much for your manpower enquiry dated " + order.OrderDate.Date + " for following personnel: ";
               msg += "<br><br>" + ComposeOrderItems(order.OrderNo, OrderItems, HasException) + "<br><br>";
               msg += HasException == true
                    ? "Please note the exceptions mentioned under the column <i>Exceptions</i> and respond ASAP.  " +
                         "We will initiate execution of the wroks at this end on receipt of your clarificatins.<br><br>"
                    : "We have initiated the works, and will revert to you soon with our delivery plan.<br><br>";
               msg += "Your point of contact for this order execution shall be the undersigned<br><br>";
               msg += "Please feel free to reach me for any clarification.<br><br>Best regards<br><br>" +
                    projManager.EmployeeName + "<br>" + projManager.Position + "<br>" + _confg.GetSection("IdealUserName").Value;
               msg += string.IsNullOrEmpty(projManager.OfficialPhoneNo) == true ? "" : "<br>Phone: " + projManager.OfficialPhoneNo;
               msg += string.IsNullOrEmpty(projManager.OfficialMobileNo) == true ? "" : "<br>Mobile: " + projManager.OfficialMobileNo;
               msg += string.IsNullOrEmpty(projManager.OfficialEmailAddress) == true ? "" : "<br>Email: " + projManager.OfficialEmailAddress;

               var senderEmailAddress = _confg["EmailSenderEmailId"] ?? "";
               var senderUserName = _confg["EmailSenderDisplayName"] ?? "";
               var recipientUserName = customer.CustomerName ?? "";
               var recipientEmailAddress = official?.Email ?? "";
               var ccEmailAddress = _confg["EmailCCandAck"] ?? "";
               var bccEmailAddress = _confg["EmailBCCandAck"] ?? "";
               var subject = "Your enquiry dated " + order.OrderDate.Date + " is registered by us under Serial No. " + order.OrderNo;
               var messageTypeId = (int)EnumMessageType.OrderAcknowledgement;
               
               var emailMessage = new EmailMessage
               {
                    SenderEmailAddress = senderEmailAddress,
                    SenderUserName = senderUserName,
                    RecipientUserName = recipientUserName,
                    RecipientEmailAddress = recipientEmailAddress,
                    CcEmailAddress = ccEmailAddress,
                    BccEmailAddress = bccEmailAddress,
                    Subject = subject,
                    Content = msg,
                    MessageTypeId = messageTypeId
               };

               return emailMessage;
          }
          private string ComposeOrderItems(int orderNo, ICollection<OrderItem> orderItems, bool hasException)
          {
               string ex = "";
               string items = "<table border='1'><tr><th>Our reference</th><th>Category</th><th>Quantity</th><th>ECNR</th><th>Source<br>Country</th><th>" +
                    "Work<br>hours</th><th>salary</th><th>Accommodation</th><th>Food</th>Transport</th><th>" +
                    "Other<br>Allowances</th><th>Exceptions</th></tr>";
               foreach (var item in orderItems)
               {
                    items += "<tr><td>" + orderNo + "-" + item.SrNo + "</td><td>" + item.CategoryName + "</td><td>" + item.Quantity;
                    items += "</td><td>" + item.Ecnr + "</td><td>" + item.SourceFrom;
                    items += "</td><td>" + item.Remuneration.WorkHours;
                    items += "</td><td>" + item.Remuneration.SalaryCurrency + item.Remuneration.SalaryMin;
                    items += item.Remuneration.SalaryMax > 0 ? " - " + item.Remuneration.SalaryMax : "";

                    items += item.Remuneration.HousingNotProvided == true ? "Not Provided"
                         : item.Remuneration.HousingProvidedFree == true ? "Free"
                         : item.Remuneration.HousingAllowance == 0 ? "??" : item.Remuneration.HousingAllowance;
                    items += item.Remuneration.FoodNotProvided == true ? "Not Provided"
                         : item.Remuneration.FoodProvidedFree == true ? "Free"
                         : item.Remuneration.FoodAllowance == 0 ? "??" : item.Remuneration.FoodAllowance;
                    items += item.Remuneration.TransportNotProvided == true ? "Not Provided"
                         : item.Remuneration.TransportProvidedFree == true ? "Free"
                         : item.Remuneration.TransportAllowance == 0 ? "??" : item.Remuneration.TransportAllowance;

                    items += "</td><td>" + item.Remuneration.OtherAllowance + "</td><td>";

                    ex += item.Remuneration.WorkHours == 0 ? "<font color='red'><b>Working hours not provided</b><br></font>" : "";
                    ex += item.Remuneration.SalaryMin == 0 ? "<font color='red'><b>Salary not provided</b><br></font>" : "";
                    ex += !item.Remuneration.HousingProvidedFree && item.Remuneration.HousingAllowance == 0
                              && !item.Remuneration.HousingNotProvided ? "<font color='red'><b>Housing provision not provided</b></font><br>" : "";
                    ex += !item.Remuneration.FoodProvidedFree && item.Remuneration.FoodAllowance == 0
                              && !item.Remuneration.HousingNotProvided ? "<font color='red'><b>Food provision not provided</font></b><br>" : "";
                    ex += !item.Remuneration.TransportProvidedFree && item.Remuneration.TransportAllowance == 0
                              && !item.Remuneration.TransportNotProvided ? "<font color='red'><b>Transport provision not provided</b></font><br>" : "";
                    items += ex + "</td></tr>";
                    hasException = ex.Length > 0;
               }
               items += "</table>";
               return items;
          }

          public async Task<EmailMessage> ForwardEnquiryToHRDept(Order order)
          {
               string msg = "";
               var HRSup = _confg.GetSection("EmpHRSupervisorId").Value;
               int HRSupId = HRSup == null ? 0 : Convert.ToInt32(HRSup);
               var hrObj = await _empService.GetEmployeeFromIdAsync(HRSupId);
               int projMgrId = order.ProjectManagerId == 0 ? 1 : order.ProjectManagerId;       //todo - correct this
               var projMgr = await _empService.GetEmployeeFromIdAsync(projMgrId);
               var cust = await _commonServices.CustomerBriefDetailsFromCustomerId(order.CustomerId);

               msg = DateTime.Now.Date + "<br><br>" + hrObj.EmployeeName + ", " + hrObj.Position + "<br>" +
                    "<br>HR Supervisor<br>Email: " + hrObj.OfficialEmailAddress + "<br><br>";
               if (order.ForwardedToHRDeptOn == null || ((DateTime)(order.ForwardedToHRDeptOn)).Date.Year < 200)
               {
                    msg += "Following requirement is forwarded to you for execution within time periods shown:<br><br>";
               }
               else
               {
                    msg += "Following requirement forwarded to you on " +
                         ((DateTime)order.ForwardedToHRDeptOn).Date + " <b><font color='blue'>is revised</font></b>as follows:<br><br>";
               }
               msg += "Order No.:" + order.OrderNo + " dated " + order.OrderDate +
                    "<br>Customer:" + cust.CustomerName + ", " + cust.City + ", Place of work: " + order.CityOfWorking;
               msg += "<br><br>Overall Project Completion target: " + order.CompleteBy.Date + "<br><br>";
               msg += "Requirement details are as follows.  For Job Description, click the relevant link<br>";

               var itemids = await _context.OrderItems.Where(x => x.OrderId == order.Id).Select(x => x.Id).ToListAsync();
               var tbl = TableOfOrderItemsContractReviewedAndApproved(itemids);
               msg += tbl;
               msg += "<br><br>Task for this requirement is also assigned to you.<br><br>" + projMgr.KnownAs +
                    "<br>Project Manager-Order" + order.OrderNo;

               var emailMsg = new EmailMessage("forwardToHR", projMgr.EmployeeId, hrObj.EmployeeId, projMgr.OfficialEmailAddress,
                    projMgr.UserName, hrObj.UserName, hrObj.OfficialEmailAddress, "", "", "New Requirement No. " + order.OrderNo,
                    msg, (int)EnumMessageType.RequirementForwardToHRDept);
               return emailMsg;
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
                         content, (int)EnumMessageType.OrderAssessmentQDesigning);
                    emailmessages.Add(emailMsg);
               }

               return emailmessages;

          }
          public async Task<ICollection<EmailMessage>> ComposeMessagesToHRExecToSourceCVs(ICollection<OrderAssignmentDto> orderItemIds, LoggedInUserDto loggedIn)
          {
               //todo - verify the tasks are not already assigned.  If so, advise to conclude existing tasks before assigning to new HR Executives
              /* var hrTasks = await _context.OrderItems.Where(x => orderItemIds.Contains(x.Id))
                    .Select(x => new {x.HrExecId, x.Id, x.OrderId, x.CategoryId, x.CategoryName, x.Quantity, x.JobDescription})
                    .ToListAsync();

               var order = await _context.Orders.Where(x => x.Id == orderItemIds.Select(x => x.OrderId).FirstOrDefault() )
                    .Select(x => new {x.OrderNo, x.OrderDate, x.ProjectManagerId, x.CustomerId, x.CityOfWorking})
                    .FirstOrDefaultAsync();               
               */
               var hrExecIds = orderItemIds.Select(x => x.HRExecId).Distinct().ToList();
               var order = orderItemIds.Select(x => new {x.OrderNo, x.CustomerId, x.OrderDate, x.ProjectManagerId, x.CityOfWorking}).FirstOrDefault();
               var msgs = new List<EmailMessage>();

               int projMgrId = order.ProjectManagerId == 0 ? 1 : order.ProjectManagerId;       //todo - correct this
               var projMgr = await _empService.GetEmployeeFromIdAsync(projMgrId);
               var cust = await _commonServices.CustomerBriefDetailsFromCustomerId(order.CustomerId);

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
                    
                    var ids = orderItemIds.Where(x => x.HRExecId == hrExecId).Select(x => x.OrderItemId).ToList();
                    var orderitems = await _context.OrderItems.Where(x => ids.Contains(x.Id) && x.Remuneration != null)
                         .Include(x => x.Remuneration).ToListAsync();
                    
                    var tbl = await TableOfOrderItemsContractReviewedAndApproved(
                         orderItemIds.Where(x => x.HRExecId == hrExecId).Select(x => x.OrderItemId).ToList());
                    msg += tbl + "<br><br>Please also check for your task dashboard for these tasks";
                    msg += "<br>end of system generated message";

                    var emailMsg = new EmailMessage("AssignTasksToHRExec", projMgr.AppUserId, hrExec.AppUserId, 
                         projMgr.OfficialEmailAddress, projMgr.UserName, hrExec.UserName, hrExec.OfficialEmailAddress, "", "", 
                         "Tasks to mobilize suitable candidates - Order No. " + order.OrderNo,
                         msg, (int)EnumMessageType.TaskAssignmentToHRExecToShortlistCV);
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
                         var tbl = TableOfCVsSubmittedByHRExecutives(ownerAndAssignees);

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
                              msg, (int)EnumMessageType.TaskAssignmentToHRExecToShortlistCV);
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
                         var tbl = TableOfCVsSubmittedByHRSup(ownerAndAssignees);

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
                              msg, (int)EnumMessageType.TaskAssignmentToHRExecToShortlistCV);
                         msgs.Add(emailMsg);
                    }
               }

               return msgs;

          }

          public Task<ICollection<EmailMessage>> ComposeMessagesToDocControllerAdminToForwardCVs(ICollection<CVsSubmittedDto> cvsSubmitted, LoggedInUserDto loggedIn)
          {
               throw new NotImplementedException();
          }

          public async Task<EmailMessage> Publish_CVReadiedToForwardToClient(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId)
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
                    (int)EnumMessageType.CVForwardingToDocControllerToFwdCVToClient);
               
               return email;
          }

          public async Task<EmailMessage> Publish_CVReviewedByHRSup(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId)
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
                    (int)EnumMessageType.Publish_CVReviewedByHRSup);
               
               return email;
          }

          public async Task<EmailMessage> Publish_CVReviewedByHRManager(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId)
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
                    (int)EnumMessageType.Publish_CVReviewedByHRManager);
               
               return email;
          
          }
     
          public async Task<EmailMessage> ComposeAppplicationTaskMessage (int taskId)
          {
               var task = await _context.Tasks.FindAsync(taskId);

               if (task == null) return null;

               var email = new EmailMessage();

               var addressee = await _empService.GetEmployeeBriefAsyncFromEmployeeId(task.AssignedToId);
               var sender = await _empService.GetEmployeeBriefAsyncFromEmployeeId(task.TaskOwnerId);

               var mailBody = DateTime.Now.Date + "<br><br>" + addressee.Gender == "M" ? "Mr. " : "Ms, " + addressee.EmployeeName + "<br>";
               mailBody += "Target Date: " + task.CompleteBy.Date + "<br>";
               mailBody += string.IsNullOrEmpty(addressee.Position)==true ? "" : addressee.Position + "<br>";
               mailBody += "Email: " +addressee.OfficialEmailAddress + "<br><br>Following task is assigned to you:<br><br>";
               mailBody += "Date of the task: " + task.TaskDate.Date + "<br>";
               mailBody += "Assigned By: " + sender.EmployeeName;
               mailBody += string.IsNullOrEmpty(sender.Position)==true ? "" : ", " + ", Position:" + sender.Position;
               mailBody += "<br>Phone No.: " + sender.OfficialPhoneNo;
               mailBody += "<br><br><b>Task Description</b><br>:" + task.TaskDescription;

               email.Content=mailBody;
               email.RecipientEmailAddress = addressee.OfficialEmailAddress;
               email.MessageTypeId = task.TaskTypeId;
               email.RecipientUserName = addressee.UserName;
               email.RecipientId = addressee.AppUserId;
               email.SenderEmailAddress = sender.OfficialEmailAddress;
               email.SenderId = sender.AppUserId;
               email.SenderUserName = sender.UserName;
               email.Subject = task.Id + " dated " + task.TaskDate.Date;

             return email;
          }

          public async Task<EmailMessage> Publish_CVSubmittedToHRSup(ICollection<CommonDataDto> commonDataDtos, LoggedInUserDto loggedInDto, int recipientId)
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
                    (int)EnumMessageType.Publish_CVReviewedByHRSup);
               
               return email;

          }

          private string GetSelectionDetailsBySMS(SelectionDecision selection)
          {
               string strToReturn = "";
               strToReturn = "Pleased to advise you hv been selected by " + selection.CustomerName + " as " + selection.CategoryName;
               strToReturn += " at a basic salary of " + selection.SalaryCurrency + " " + selection.FoodAllowance;
               strToReturn += " plus perks.  Please visit us to review and sign your offer letter and to initiate your joining formalities";
               return strToReturn;
          }
          private string GetSelectionDetails(SelectionDecision selection)
          {
               string strToReturn = "";
               strToReturn = "<ul><li><b>Employee Name:</b> " + selection.CandidateName + "(Application No.:" + selection.ApplicationNo + ")</li>" +
                    "<li><b>Employer</b>: " + selection.CustomerName + "</li>" +
                    "<li><b>Selected as:</b> " + selection.CategoryName + 
                    "<li><b>Contract Period:</b>" + selection.ContractPeriodInMonths + " months</li>" +
                    "<li><b>Basic Salary:</b>" + selection.SalaryCurrency + " " + selection.Salary + "</li>" +
                    "<li><b>Housing Provision: </b>";
                    if (selection.HousingProvidedFree) { strToReturn += "Provided Free"; }
                    else { strToReturn += selection.HousingAllowance > 0 
                         ? selection.SalaryCurrency + " " + selection.HousingAllowance : "Not provided"; }
               strToReturn += "</li>" +
                    "<li><b>Food Provision:</b>";
                    if (selection.FoodProvidedFree) { strToReturn += "Provided Free"; }
                    else {strToReturn += selection.FoodAllowance > 0 ? 
                         selection.SalaryCurrency + " " + selection.FoodAllowance : "Not Provided"; }
               strToReturn += "</li>" +
                    "<b><li>Transport Provision:</b> ";
                    if (selection.TransportProvidedFree) { strToReturn += "Provided Free"; }
                    else { strToReturn += selection.TransportAllowance > 0 
                         ? selection.SalaryCurrency + " " + selection.TransportAllowance : "Not provided"; }
               strToReturn += "</li>";
               if (selection.OtherAllowance > 0) strToReturn += "<li><b>Other Allowances:</b>" + selection.SalaryCurrency + " " + selection.OtherAllowance + "</li>";
               return strToReturn + "</ul>";
          }
               
          private async Task<string> TableOfRelevantOpenings(List<int> Ids)
          {
               var stringIds = string.Join(",", Ids);
               var statsParams = new StatsParams { ProfessionIds = stringIds };

               string strToReturn = "";
               var openings = await _statsService.GetCurrentOpenings(statsParams);
               if (openings != null)
               {
                    strToReturn = "<Table border='1'><tr><th>Order No</th><th>Category Name</th><th>Customer</th></tr>";
                    foreach (var item in openings)
                    {
                         strToReturn += "<tr><td>" + item.OrderNo + "</td><td>" + item.ProfessionName + "</td><td>" + item.CustomerName + "</td></tr>";
                    }
               }
               return string.IsNullOrEmpty(strToReturn) 
                    ? "Currently, no opportunities available matching your professions" + "</table>"
                    : strToReturn + "</table";
          }
          private async Task<string> TableOfOrderItemsContractReviewedAndApproved(ICollection<int> itemIds)
          {
               string strToReturn = "";
          /*     var itemIds = await _context.ContractReviewItems
                    .Where(x => x.ReviewItemStatus == (int)EnumReviewItemStatus.Accepted && x.OrderId == orderId)
                    .Select(x => x.OrderItemId)
                    .ToListAsync();
               if (itemIds.Count() == 0) return "";
          */
               var orderitems = await _context.OrderItems.Where(x => itemIds.Contains(x.Id) && x.Remuneration != null)
                    .Include(x => x.Remuneration)
                    .OrderBy(x => x.SrNo)
                    .ToListAsync();
               if (orderitems == null || orderitems.Count() == 0) return "";

               strToReturn = "<Table border='1'><tr><th>Sr No</th><th>Category Name</th><th>Quantity</th><th>Source<br>country" +
                    "</th><th>ECNR</th><th>Assessmt<br>Sheet</th><th>Work<br>Hours</th><th>Complete By" +
                    "</th><th>Salary<br>Currency</th><th>Salary</th><th>Food</th><th>Housing</th><th>Transport" +
                    "</th><th>Other<br>Allowance</th><th>Contract Period</th></tr>";

               foreach (var item in orderitems)
               {
                    var catname = string.IsNullOrEmpty(item.CategoryName) ? "undefined" : item.CategoryName;
                    var sourcefrom = string.IsNullOrEmpty(item.SourceFrom) ? "India" : item.SourceFrom;
                    strToReturn += "<tr><td>" + item.SrNo + 
                         "</td><td>" + catname + 
                         "</td><td>" + item.Quantity +
                         "</td><td>" + sourcefrom + "</td><td>";
                    strToReturn += (item.Ecnr ? "Y" : "N") + "</td><td>";
                    strToReturn += item.RequireAssess ? "Y" : "N";

                    if (item.Remuneration != null)
                    {
                         strToReturn += "</td><td>" + item.Remuneration.WorkHours;
                         if (item.CompleteBefore.Year < 2000)
                         {
                              strToReturn += "</td><td>Not Known";
                         }
                         else
                         {
                              strToReturn += "</td><td>" + item.CompleteBefore.ToString("dd-MMMM-yyyy", CultureInfo.InvariantCulture);
                         }
                         strToReturn += "</td><td>" + item.Remuneration.SalaryCurrency + "</td><td>" + item.Remuneration.SalaryMin;
                         if (item.Remuneration.SalaryMax > 0) strToReturn += "-" + item.Remuneration.SalaryMax;

                         if (item.Remuneration.FoodNotProvided)
                         {
                              strToReturn += "</td><td>Not Provided";
                         }
                         else if (item.Remuneration.FoodAllowance > 0)
                         {
                              strToReturn += "</td><td>" + item.Remuneration.FoodAllowance;
                         }
                         else
                         {
                              strToReturn += "</td><td>Provided Free";
                         }

                         if (item.Remuneration.HousingNotProvided)
                         {
                              strToReturn += "</td><td>Not Provided";
                         }
                         else if (item.Remuneration.HousingAllowance > 0)
                         {
                              strToReturn += "</td><td>" + item.Remuneration.HousingAllowance;
                         }
                         else
                         {
                              strToReturn += "</td><td>Provided Free";
                         }

                         if (item.Remuneration.TransportNotProvided)
                         {
                              strToReturn += "</td><td>Not Provided";
                         }
                         else if (item.Remuneration.TransportAllowance > 0)
                         {
                              strToReturn += "</td><td>" + item.Remuneration.TransportAllowance;
                         }
                         else
                         {
                              strToReturn += "</td><td>Provided Free";
                         }
                         
                         if (item.Remuneration.OtherAllowance > 0)
                         { strToReturn += "</td><td>" + item.Remuneration.OtherAllowance; }
                         else
                         { strToReturn += "</td><td>Not Provided"; }

                         strToReturn += "</td><td>" + item.Remuneration.ContractPeriodInMonths + " months</td></tr>";
                    }
               }
               strToReturn += "</table>";
               return strToReturn;
          }

          private async Task<string> TableOfCVsSubmittedByHRExecutives(ICollection<CVsSubmittedDto> cvsSubmitted)
          {
               string strToReturn = "<Table border='1'><tr><th>Category Ref</th><th>Category</th><th>" +
                    "Application No</th><th>Candidate Name</th><th>PP No. </th><th>HR Executive" +
                    "</th><th>Submitted On</th></tr>";

               foreach(var cv in cvsSubmitted)
               {
                    var item = await _context.OrderItems.Where(x => x.Id == cv.OrderItemId).Select(
                              x => new {x.OrderId, x.SrNo, x.CategoryName, x.Quantity, x.RequireAssess}).FirstOrDefaultAsync();
                    var order = await _context.Orders.Where(x => x.Id == item.OrderId).Select(x => x.OrderNo).FirstOrDefaultAsync();

                    var candidate = await _context.Candidates.Where(x => x.Id == cv.CandidateId)
                         .Select(x => new{x.FullName, x.ApplicationNo, x.PpNo}).FirstOrDefaultAsync();
                    var owner = await _empService.GetEmployeeBriefAsyncFromEmployeeId(cv.TaskOwnerId);
                    var categoryName = item.CategoryName;
                    var categorySrNo = item.SrNo;
                    var quantity = item.Quantity;
                    var orderNo = order;
                    strToReturn += "<tr><td>" + order + "-" + item.SrNo + "</td><td>" + item.CategoryName + "</td><td>" +
                         candidate.ApplicationNo + "</td><td>" + candidate.FullName + "</td><td>" +
                         candidate.PpNo + "</td><td>" + owner.EmployeeName + "</td></tr>";
               }
               return strToReturn + "</table>";
               
          }

          private async Task<string> TableOfCVsSubmittedByHRSup(ICollection<CVsSubmittedDto> cvsSubmitted)
          {
               string strToReturn = "<table border='1'><tr><th>Category Ref</th><th>Category</th><th>" +
                    "Application No</th><th>Candidate Name</th><th>PP No. </th><th>HR Executive" +
                    "</th><th>Submitted On</th></tr>";

               foreach(var cv in cvsSubmitted)
               {
                    var item = await _context.OrderItems.Where(x => x.Id == cv.OrderItemId).Select(
                              x => new {x.OrderId, x.SrNo, x.CategoryName, x.Quantity, x.RequireAssess}).FirstOrDefaultAsync();
                    var order = await _context.Orders.Where(x => x.Id == item.OrderId).Select(x => x.OrderNo).FirstOrDefaultAsync();

                    var candidate = await _context.Candidates.Where(x => x.Id == cv.CandidateId)
                         .Select(x => new{x.FullName, x.ApplicationNo, x.PpNo}).FirstOrDefaultAsync();
                    var owner = await _empService.GetEmployeeBriefAsyncFromEmployeeId(cv.TaskOwnerId);
                    var categoryName = item.CategoryName;
                    var categorySrNo = item.SrNo;
                    var quantity = item.Quantity;
                    var orderNo = order;
                    strToReturn += "<tr><td>" + order + "-" + item.SrNo + "</td><td>" + item.CategoryName + "</td><td>" +
                         candidate.ApplicationNo + "</td><td>" + candidate.FullName + "</td><td>" +
                         candidate.PpNo + "</td><td>" + owner.EmployeeName + "</td></tr>";
               }
               return strToReturn + "</table>";
               
          }

          public ICollection<EmailMessage> ComposeCVFwdMessagesToClient(ICollection<CVRef> cvsReferred, LoggedInUserDto loggedIn)
          {
               DateTime dateTimeNow = DateTime.Now;
               var emails = new List<EmailMessage>();

               var cvsref = from r in cvsReferred
                    join i in _context.OrderItems on r.OrderItemId equals i.Id
                    join cat in _context.Categories on i.CategoryId equals cat.Id
                    join o in _context.Orders on i.OrderId equals o.Id
                    join cand in _context.Candidates on r.CandidateId equals cand.Id
                    join cust in _context.Customers on o.CustomerId equals cust.Id
                    join off in _context.CustomerOfficials on cust.Id equals off.CustomerId 
                         where off.IsValid && off.Divn.ToLower()=="hr"
                    select new CVFwdMsgDto {
                         CustomerId = o.CustomerId, CustomerName = cust.CustomerName, City = cust.City, 
                         OfficialId = off.Id, OfficialTitle = off.Title, OfficialName = off.OfficialName, 
                         Designation = off.Designation, OfficialEmail = off.Email, OfficialUserId = off.AppUserId,
                         OrderNo = o.OrderNo, OrderDated = o.OrderDate.Date, ItemSrNo = i.SrNo, CategoryName = cat.Name,
                         ApplicationNo = cand.ApplicationNo, PPNo = cand.PpNo, CandidateName = cand.FullName,
                         CumulativeSentSoFar = CumulativeCountForwardedSoFar(r.OrderItemId).Result,
                         AssessmentGrade = AssessmentGrade(r.CandidateId, r.OrderItemId).Result
                    };
               //var result = await cvsref.AsQueryable().ToListAsync(); //TODO - this does not work
               //the source 'IQueryable' doesn't implement 'IAsyncEnumerable<core.ParamsAndDtos.CVFwdMsgDto>'. Only sources that implement 'IAsyncEnumerable' can be used for Entity Framework asynchronous operations
               var result = cvsref.ToList();

               var officials = result.Select(x => new 
                    {x.OfficialId, x.OfficialTitle, x.OfficialName, x.CustomerName, 
                         x.City, x.OfficialEmail, x.Designation, x.OfficialUserId}).ToList();

               foreach(var off in officials)
               {
                    var msg = dateTimeNow.Date + "<br><br>"+  off.OfficialTitle + " " + off.OfficialName + ", " + 
                         off.Designation + "<br>M/S " + off.CustomerName + ", " + off.City + "<br>Email:" + off.OfficialEmail +
                         "We are pleased to enclose following CVs for your consideration agaist your requirements mentioned.<br><br>" +
                         "<table border='1'><tr><th>Order ref and dated</th><th>Category</th>Application No</th><th>Candidate Name</th><th>" + 
                         "Passport No</th><th>Attachments</th><th>Forwarded<br>so far</th><th>Our assessment<br>Grade</th><th>Salary Expectation</th></tr>";
                    var filtered = result.Where(x => x.OfficialId == off.OfficialId).ToList();
                    int counter=0;
                    foreach(var item in filtered)
                    {
                         msg += "<tr><td>" + item.OrderNo +"-"+ item.ItemSrNo + "/<br>" + item.OrderDated + "</td><td>" +
                              item.CategoryName + "</td><td>" + item.ApplicationNo + "</td><td>" + item.CandidateName + 
                              "</td><td>"+ item.PPNo + "</td><td></td><td>" + item.CumulativeSentSoFar + "</td><td>" + 
                              item.AssessmentGrade + "</td></tr>";
                         counter+=1;
                    }

                    msg +="</table><br><br>Please review the CVs and provide us your feedback at the very earliest.<br><br>" +
                         "While we try to retain candidates as long as possible, due to the dynamic market conditions, " +
                         "candidates availability becomes volatile, and it is always preferable to keep candidates positively " +
                         "engaged.  While you may take a little longer to make up your minds for selections, the candidates " +
                         "that you are not interested in can be advised to us, so that they may be released for other opportunities.";
                    msg += "While rejecting a candidate, if you also advise us reasons for the rejection, it will help us " +
                         "adjust our criteria for shortlistings, which will ultimately help in minimizing rejections at your end.";
                    msg +="<br><br>Regards<br><br>This is system generated message";

                    var email = new EmailMessage("cv forward", loggedIn.LoggedInAppUserId, off.OfficialUserId, 
                         loggedIn.LoggedInAppUserEmail, loggedIn.LoggedIAppUsername, off.OfficialName, off.OfficialEmail, "", "", 
                         counter + " CVs forwarded against your requirement", msg, (int)EnumMessageType.CVForwardingToClient);
                    emails.Add(email);
               }

               return emails;
          
          }

          private async Task<OrderItemReviewStatusDto> CumulativeCountForwardedSoFar(int orderitemId)
          {
               return await( _context.CVRefs.Where(x => x.Id == orderitemId)
                    .GroupBy(x => new { x.OrderItemId })
                    .Select(x => new OrderItemReviewStatusDto
                    {
                         OrderItemId = orderitemId,
                         CtOfSelected = x.Count(x => x.RefStatus == (int) EnumCVRefStatus.Selected),
                         CtOfNotReviewed = x.Count(x => x.RefStatus == (int) EnumCVRefStatus.Referred),
                         CtOfRejected = x.Count(x => x.RefStatus != (int)EnumCVRefStatus.Selected &&
                              x.RefStatus != (int) EnumCVRefStatus.Referred)
                    })
               ).FirstOrDefaultAsync();
          }

          private async Task<EnumCandidateAssessmentResult> AssessmentGrade(int candidateId, int orderitemId)
          {
               return await _context.CandidateAssessments
                    .Where(x => x.CandidateId == candidateId && x.OrderItemId == orderitemId)
                    .Select(x => x.AssessResult)
                    .FirstOrDefaultAsync();
          }    
          
     }
}