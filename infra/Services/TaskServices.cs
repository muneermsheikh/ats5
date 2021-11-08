using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.EmailandSMS;
using core.Entities.HR;
using core.Entities.Orders;
using core.Entities.Tasks;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace infra.Services
{
     public class TaskServices : ITaskService
     {
          private readonly ICommonServices _commonServices;
          private readonly ATSContext _context;
          private readonly int EmployeeIdDocController;
          private readonly int TargetDays_HRSupToReviewCV;
          private readonly int TargetDays_DocControllerToFwdCV;
          public IConfiguration Config { get; set; }
          private readonly IComposeMessages _composeMessages;
          private readonly IUnitOfWork _unitOfWork;
          private readonly IEmailService _emailService;
          public TaskServices(ICommonServices commonServices, ATSContext context, IConfiguration config,
                    IComposeMessages composeMessages, IUnitOfWork unitOfWork, IEmailService emailService)
          {
               _emailService = emailService;
               _unitOfWork = unitOfWork;
               _composeMessages = composeMessages;
               _context = context;
               _commonServices = commonServices;
               EmployeeIdDocController = Convert.ToInt32(config.GetSection("EmpDocControllerAdminId").Value);
               TargetDays_HRSupToReviewCV = Convert.ToInt32(config.GetSection("TargetDays_HRSupToReviewCV").Value);
               TargetDays_DocControllerToFwdCV = Convert.ToInt32(config.GetSection("TargetDays_DocControllerToFwdCV").Value);

          }

     /*
     public async Task<ICollection<CVSubmitToHRSupDto>> CreateAndSaveTaskForCVRvwByHRSup(LoggedInUserDto loggedInDto, ICollection<CVSubmitToHRSupDto> cvsSubmitted)
     {
          ApplicationTask nextTask = null;
          var lstMsg = new List<EmailMessageAndTaskIdDto>();
          foreach (var item in cvsSubmitted)
          {
               if (item.AssignedToId == 0) throw new Exception("Task not assigned to anyone");
               var commonData = item.CommonDataDto;
               var candidateDesc = commonData.CandidateDesc;
               var parentTask = item.ParentTask;

               if(parentTask == null) {
                    parentTask = await GetHRExecTaskForCVCompiling(item.OrderItemId, commonData.HRExecId);

                    if (parentTask == null) throw new Exception("cannot retrieve task for " + loggedInDto.LoggedIAppUsername +
                         " for category " + commonData.OrderNo + "-" + commonData.OrderItemSrNo);
               }

               var newTask = new ApplicationTask();
               var hrcvreview = new CVReview();

               if (commonData.HRSupId == 0)   //save for use later for cv fwd
               {
                    // create task for Doc Controller
                    newTask = new ApplicationTask((int)EnumTaskType.CVForwardToDocControllerAdmin, DateTime.Now,
                         item.TaskOwnerId, EmployeeIdDocController, commonData.OrderId,  commonData.OrderNo, 
                         item.OrderItemId, "Please forward following application to client: " + candidateDesc,
                         DateTime.Today.AddDays(TargetDays_DocControllerToFwdCV), "Open", item.CandidateId, null);

                    _context.Tasks.Add(newTask);
                    await _context.SaveChangesAsync();

                    // insert CVReview
                    hrcvreview = new CVReview
                    {
                         CandidateId = item.CandidateId,
                         OrderId = commonData.OrderId,
                         OrderItemId = item.OrderItemId,
                         HRExecutiveId = commonData.HRExecId,
                         HRSupId = commonData.HRSupId,
                         SubmittedByHRExecOn = DateTime.Now,
                         DocControllerAdminTaskId = newTask.Id,
                         HRExecRemarks = "No review by HR Sup, tasked to Doc Controller to froward CV"
                    };

                    _context.CVReviews.Add(hrcvreview);
               }
               else
               {
                    nextTask = new ApplicationTask((int)EnumTaskType.SubmitCVToHRSupForReview, DateTime.Now, 
                         item.TaskOwnerId, item.AssignedToId, commonData.OrderId, commonData.OrderNo, 
                         item.OrderItemId, "Please review following CV: " + candidateDesc, 
                         DateTime.Today.AddDays(TargetDays_HRSupToReviewCV), "Open", item.CandidateId, null);

                    _context.Tasks.Add(nextTask);
                    await _context.SaveChangesAsync();

                    // update HRCV Processing
                    hrcvreview = await _context.CVReviews.Where(x => x.OrderItemId == item.OrderItemId
                         && x.CandidateId == item.CandidateId).FirstOrDefaultAsync();
                    hrcvreview.HRSupId = commonData.HRMId == 0 ? 0 : nextTask.Id;
                    hrcvreview.DocControllerAdminTaskId = commonData.HRMId == 0 ? nextTask.Id : 0;

                    _context.CVReviews.Update(hrcvreview);

                    //update CVReviewBySup record
                    var supreview = await _context.CVReviews.FindAsync(hrcvreview.CVReviewBySupId);
                    if (supreview == null) throw new Exception("cannot retrieve Supervisor Review record for reviewBySupId " + hrcvreview.CVReviewBySupId);
                    

                    //add taskitems to hr exec assignment task
                    var parentTaskItem = new TaskItem((int)EnumTaskType.AssignTaskToHRExec, parentTask.Id, DateTime.Now, "Open",
                         "CV submitted for review " + candidateDesc, loggedInDto.LoggedInEmployeeId, commonData.OrderId, item.OrderItemId,
                         commonData.OrderNo, loggedInDto.LoggedInAppUserId, null, item.CandidateId, 0, parentTask);
                    _unitOfWork.Repository<TaskItem>().Add(parentTaskItem);
               }

               parentTask.CompletedOn = DateTime.Now;
               _context.Tasks.Update(parentTask);

               await _context.SaveChangesAsync();
          }

          return cvsSubmitted;
     }

     public async Task<ICollection<CVSubmitToHRMDto>> CreateAndSaveTaskForCVRvwByHRM(LoggedInUserDto loggedInDto, ICollection<CVSubmitToHRMDto> cvsSubmitted)
     {
          ApplicationTask nextTask = null;
          var lstMsg = new List<EmailMessage>();
          foreach (var item in cvsSubmitted)
          {
               var commonData = item.CommonDataDto;
               var candidateDesc = commonData.CandidateDesc;

               var parentTask = await GetHRSupTaskForCVCompiling(item.OrderItemId, item.CandidateId);

               if (parentTask == null) throw new Exception("cannot retrieve task for " + loggedInDto.LoggedIAppUsername +
                    " for category " + commonData.OrderNo + "-" + commonData.OrderItemSrNo);

               if (item.ReviewResultId == 1)       //approved by HR Sup
               {
                    nextTask = new ApplicationTask((int)EnumTaskType.CVForwardToDocControllerAdmin, DateTime.Now, 
                         item.TaskOwnerId, EmployeeIdDocController, commonData.OrderId, commonData.OrderNo,
                         item.OrderItemId, "Following CV ready to forward to Client: " + candidateDesc,
                         DateTime.Today.AddDays(TargetDays_DocControllerToFwdCV), "Open", item.CandidateId, null);
                    _context.Tasks.Add(nextTask);
                    await _context.SaveChangesAsync();
               }

               //update existing HRSup processing record
               var CVReviewBySupId = await _context.CVReviews.Where(x => x.CandidateId == item.CandidateId && 
                    x.OrderItemId == item.OrderItemId).Select(x => x.CVReviewBySup.Id).FirstOrDefaultAsync();
               var cvreviewSup = await _context.CVReviewBySups.FindAsync(CVReviewBySupId);

               if (cvreviewSup != null)
               {
                    cvreviewSup.HRMId = item.ReviewResultId == 1 ? nextTask.AssignedToId : 0;
                    cvreviewSup.ReviewedByHRSupOn = DateTime.Now;
                    _context.CVReviewBySups.Update(cvreviewSup);
               }

               parentTask.CompletedOn = DateTime.Now;
               _context.Tasks.Update(parentTask);
          }

          await _context.SaveChangesAsync();

          return cvsSubmitted;

     }
     */
     private async Task<ICollection<OrderAssignmentDto>> GetAssignmentDtoFromItemIds(ICollection<int> orderItemIds)
     {
          var orderassignmentdto = await (from i in _context.OrderItems where orderItemIds.Contains(i.Id)
               join c in _context.Categories on i.CategoryId equals c.Id 
               join o in _context.Orders on i.OrderId equals o.Id 
               join e in _context.Employees on i.HrExecId equals e.Id
               join cust in _context.Customers on o.CustomerId equals cust.Id
               orderby i.SrNo
               select new OrderAssignmentDto{
                    OrderId = o.Id, OrderNo = o.OrderNo, CategoryRef = o.OrderNo + "-" + i.SrNo, OrderItemId = i.Id, CategoryName = c.Name, 
                    OrderDate = o.OrderDate.Date, Quantity = i.Quantity, HRExecId = (int)i.HrExecId, 
                    CompleteBy = i.CompleteBefore.Year < 2000 ? o.CompleteBy.Date : i.CompleteBefore.Date, CustomerName = cust.CustomerName, CustomerId = o.CustomerId, 
                    ProjectManagerId = o.ProjectManagerId, CityOfWorking = o.CityOfWorking,
               }).ToListAsync();
          return orderassignmentdto;
     }
     public async Task<ICollection<EmailMessage>> CreateTaskForHRExecAssignment(ICollection<int> OrderItemIds, LoggedInUserDto loggedInDto)
     {
          var orderassignmentdto = await GetAssignmentDtoFromItemIds(OrderItemIds);

          var tasks = new List<ApplicationTask>();
          foreach(var item in orderassignmentdto)
          {
               var task = new ApplicationTask((int)EnumTaskType.AssignTaskToHRExec, DateTime.Now,
                    loggedInDto.LoggedInEmployeeId, (int)item.HRExecId, item.OrderId, item.OrderNo,
                    item.OrderItemId, "Category Ref " + item.CategoryRef + " " + item.CategoryName +
                    " total " + item.Quantity + " for " + item.CustomerName + " assigned to you", 
                    item.CompleteBy.Date, "Open", 0, null);
               //task.PostTaskAction = EnumPostTaskAction.ComposeAndSendEmail;
               
               _unitOfWork.Repository<ApplicationTask>().Add(task);
          }

          var recordsAffected = await _unitOfWork.Complete();

          if (recordsAffected == 0) throw new Exception("Failed to create the task");

          var msgs = (List<EmailMessage>) await _composeMessages.ComposeMessagesToHRExecToSourceCVs(orderassignmentdto, loggedInDto);
          
          //for this function, the post action is: composeandsendemail
          if(msgs!=null && msgs.Count > 0)
          {
               var attachments = new List<string>();
               foreach(var msg in msgs)
               {
                    _emailService.SendEmail(msg, attachments);
               }
          }
          
          /*   //TODO - test for recipient particulars before allowing direct send
          if (msg != null) {
               var attachments = new List<string>();        // TODO - should this be auto-sent?
               msg = await _emailService.SendEmail(msg, attachments);
          }
          */               

          return msgs;
     }

     public async Task<ICollection<EmailMessage>> CreateNewApplicationTask(ApplicationTask task, LoggedInUserDto loggedInDto)
     {
          string ErrString = ValidateTaskObject(task);
          if (!string.IsNullOrEmpty(ErrString)) throw new Exception(ErrString);

          _unitOfWork.Repository<ApplicationTask>().Add(task);

          var recordsAffected = await _unitOfWork.Complete();

          if (recordsAffected == 0) throw new Exception("Failed to create the task");

          var emailMsgs = new List<EmailMessage>();

          switch (task.TaskTypeId)
          {
               case (int)EnumTaskType.DesignOrderAssessmentQ:
                    emailMsgs = (List<EmailMessage>)
                         await _composeMessages.ComposeMessagesToDesignOrderAssessmentQs((int)task.OrderId, loggedInDto);
                    break;

               case (int)EnumTaskType.AssignTaskToHRExec:
                    var orderitemids = await _context.OrderItems.Where(x => x.OrderId == task.OrderId).Select(x => x.Id).ToListAsync();

                    var assignmentdtos = await GetAssignmentDtoFromItemIds(orderitemids);
                    emailMsgs = (List<EmailMessage>)
                         await _composeMessages.ComposeMessagesToHRExecToSourceCVs((ICollection<OrderAssignmentDto>)orderitemids, loggedInDto);
                    break;

               default:
                    break;
          }

          if ((task.PostTaskAction == EnumPostTaskAction.ComposeAndSendEmail || 
               task.PostTaskAction == EnumPostTaskAction.ComposeAndSendEmailComposeAndSendSMS )
               && emailMsgs != null && emailMsgs.Count > 0)
          {
               var attachments = new List<string>();
               foreach(var msg in emailMsgs)
               {
                    _emailService.SendEmail(msg, attachments);
               }
               
          }
          /*   //TODO - test for recipient particulars before allowing direct send
          if (msg != null) {
               var attachments = new List<string>();        // TODO - should this be auto-sent?
               msg = await _emailService.SendEmail(msg, attachments);
          }
          */
          return emailMsgs;

     }

     public async Task<TaskItem> CreateNewTaskItem(TaskItem taskItem)
     {
          _unitOfWork.Repository<TaskItem>().Add(taskItem);
          if (await _unitOfWork.Complete() > 0) return taskItem;
          return null;
     }

     public async Task<bool> DeleteApplicationTask(ApplicationTask task)
     {
          _unitOfWork.Repository<ApplicationTask>().Delete(task);
          return await _unitOfWork.Complete() > 0;
     }

     public async Task<bool> DeleteTaskItem(TaskItem taskItem)
     {
          _unitOfWork.Repository<TaskItem>().Delete(taskItem);
          return await _unitOfWork.Complete() > 0;
     }

     public async Task<ApplicationTask> EditApplicationTask(ApplicationTask taskModel)
     {
          //thanks to @slauma of stackoverflow
          var existingObj = _context.Tasks.Where(p => p.Id == taskModel.Id)
               .Include(p => p.TaskItems)
               .AsNoTracking()
               .SingleOrDefault();

          if (existingObj == null) return null;

          _context.Entry(existingObj).CurrentValues.SetValues(taskModel);   //saves only the parent, not children

          //the children - taskitems
          //Delete children that exist in database(existingObj), but not in the model 
          foreach (var existingItem in existingObj.TaskItems.ToList())
          {
               if (!taskModel.TaskItems.Any(c => c.Id == existingItem.Id && c.Id != default(int)))
               {
                    _context.TaskItems.Remove(existingItem);
                    _context.Entry(existingItem).State = EntityState.Deleted;
               }
          }

          //children pressent in the model are either updated or new ones to be added
          foreach (var item in taskModel.TaskItems)
          {
               var existingItem = existingObj.TaskItems.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();
               if (existingItem != null)       // Update child
               {
                    _context.Entry(existingItem).CurrentValues.SetValues(item);
                    _context.Entry(existingItem).State = EntityState.Modified;
               }
               else            //insert children as new record
               {
                    var newItem = new TaskItem
                    {
                         ApplicationTaskId = taskModel.Id,
                         TransactionDate = item.TransactionDate,
                         TaskTypeId = item.TaskTypeId,
                         TaskStatus = item.TaskStatus,
                         TaskItemDescription = item.TaskItemDescription,
                         EmployeeId = item.EmployeeId,
                         OrderId = item.OrderId,
                         OrderItemId = item.OrderItemId,
                         OrderNo = item.OrderNo,
                         CandidateId = item.CandidateId,
                         UserId = item.UserId,
                         Quantity = item.Quantity,
                         NextFollowupOn = item.NextFollowupOn,
                         NextFollowupById = item.NextFollowupById
                    };

                    existingObj.TaskItems.Add(newItem);
                    _context.Entry(newItem).State = EntityState.Added;
               }
          }
          _context.Entry(existingObj).State = EntityState.Modified;

          return await _context.SaveChangesAsync() > 0 ? existingObj : null;
     }

     public async Task<TaskItem> EditTaskItem(TaskItem taskItem)
     {
          var existingObj = await _context.TaskItems.FindAsync(taskItem.Id);
          if (existingObj == null) return null;

          _context.Entry(existingObj).CurrentValues.SetValues(taskItem);
          _context.Entry(existingObj).State = EntityState.Modified;

          return await _context.SaveChangesAsync() > 0 ? taskItem : null;
     }

     public async Task<Pagination<ApplicationTask>> GetApplicationTasksPaginated(TaskParams taskParams)
     {
          var specs = new TaskSpecs(taskParams);
          var countSpec = new TaskForCountSpecs(taskParams);
          var tasks = await _unitOfWork.Repository<ApplicationTask>().ListAsync(specs);
          var totalCount = await _unitOfWork.Repository<ApplicationTask>().CountAsync(countSpec);

          return new Pagination<ApplicationTask>(taskParams.PageIndex, taskParams.PageSize, totalCount, tasks);
     }

     public async Task<Pagination<ApplicationTask>> GetApplicationPendingTasksPaginated(string taskStatus, int pageIndex, int pageSize)
     {
          var specs = new TaskSpecs(taskStatus);
          var countSpec = new TaskForCountSpecs(taskStatus);
          var tasks = await _unitOfWork.Repository<ApplicationTask>().ListAsync(specs);
          var totalCount = await _unitOfWork.Repository<ApplicationTask>().CountAsync(countSpec);

          return new Pagination<ApplicationTask>(pageIndex, pageSize, totalCount, tasks);
     }

     public async Task<ICollection<TaskDashboardDto>> GetDashboardTasks(int loggedInEmployeeId)
     {
          var tasks = await _context.Tasks
                    .Where(x => (x.TaskOwnerId == loggedInEmployeeId || x.AssignedToId == loggedInEmployeeId)
                         && x.TaskStatus.ToLower() == "open")
                    .Select(x => new
                    { x.Id, x.TaskTypeId, x.TaskDate, x.AssignedToId, x.TaskOwnerId, x.CompleteBy, x.TaskDescription, x.TaskStatus })
                    .OrderBy(x => x.TaskDate)
                    .ToListAsync();

          if (tasks == null) throw new Exception("No tasks on record for the logged-in user");

          var lst = new List<TaskDashboardDto>();
          foreach (var item in tasks)
          {
               lst.Add(new TaskDashboardDto
               {
                    TaskTypeId = item.TaskTypeId,
                    TaskDate = item.TaskDate.Date,
                    TaskOwnerId = item.TaskOwnerId,
                    AssignedToId = item.AssignedToId,
                    ApplicationTaskId = item.Id,
                    TaskStatus = item.TaskStatus,
                    TaskDescription = item.TaskDescription,
                    CompleteBy = item.CompleteBy
               });
          }
          return lst;
     }

     public async Task<ApplicationTask> GetHRExecTaskForCVCompiling(int orderitemId, int candidateId)
     {
          return await _context.Tasks.Where(x => x.OrderItemId == orderitemId && x.AssignedToId == candidateId
               && x.TaskTypeId == (int)EnumTaskType.AssignTaskToHRExec).FirstOrDefaultAsync();
     }

     public async Task<ApplicationTask> GetHRSupTaskForCVCompiling(int orderitemId, int candidateId)
     {
          return await _context.Tasks.Where(x => x.OrderItemId == orderitemId && x.AssignedToId == candidateId
               && x.TaskTypeId == (int)EnumTaskType.SubmitCVToHRMMgrForReview).FirstOrDefaultAsync();
     }

     public async Task<ApplicationTask> GetHRMTaskForCVCompiling(int orderitemId, int candidateId)
     {
          var hrsuptask = GetHRSupTaskForCVCompiling(orderitemId, candidateId);
          if (hrsuptask == null)
          {
               return null;
          }
          else
          {
               return await _context.Tasks.FindAsync(hrsuptask.Id);
          }
     }
     public async Task<bool> SetApplicationTaskStatus(int ApplicationTaskId, DateTime dateOfStatus, string taskStatus, string UserName, int AppUserId)
     {
          var t = await _unitOfWork.Repository<ApplicationTask>().GetByIdAsync(ApplicationTaskId);
          t.TaskStatus = taskStatus;
          int employeeId = await _context.Employees.Where(x => x.AppUserId == AppUserId)
               .Select(x => x.Id).FirstOrDefaultAsync();
          _context.Entry(t).State = EntityState.Modified;

          var tItem = new TaskItem((int)t.TaskTypeId, t.Id, dateOfStatus, taskStatus,
               "set as " + taskStatus + " on " + dateOfStatus + " by " + UserName, employeeId, (int)t.OrderId,
               (int)t.OrderItemId, (int)t.OrderNo, employeeId, DateTime.Now.AddDays(2), (int)t.CandidateId, 0,0, t);
          //_context.Entry(tItem).State = EntityState.Added;
          t.TaskItems.Add(tItem);

          return await _context.SaveChangesAsync() > 0;
     }

     private string ValidateTaskObject(ApplicationTask task)
     {
          string ErrorString = "";

          switch (task.TaskTypeId)
          {
               case (int)EnumTaskType.OrderRegistration:
                    if (task.OrderId == 0 || task.OrderNo == 0) ErrorString = "Order Id and Order No not provided";
                    break;
               case (int)EnumTaskType.ContractReview:
                    if (task.OrderId == 0 || task.OrderNo == 0) ErrorString = "Order Id and Order No not provided";
                    if (task.OrderItemId == 0) ErrorString = "OrderItem Id not provided";
                    break;
               case (int)EnumTaskType.OrderAssignmentToProjectManager:
                    if (task.OrderId == 0 || task.OrderNo == 0) ErrorString = "Order Id and Order No not provided";
                    break;
               case (int)EnumTaskType.DesignOrderAssessmentQ:
                    if (task.OrderId == 0 || task.OrderNo == 0) ErrorString = "Order Id and Order No not provided";
                    break;
               case (int)EnumTaskType.AssignTaskToHRExec:
                    if (task.OrderItemId == 0) ErrorString = "Order Item Id and Order No not provided";
                    break;
               case (int)EnumTaskType.SubmitCVToHRSupForReview:
                    if (task.OrderId == 0 || task.OrderNo == 0) ErrorString = "Order Id and Order No not provided";
                    break;
               case (int)EnumTaskType.SubmitCVToHRMMgrForReview:
                    if (task.OrderId == 0 || task.OrderNo == 0) ErrorString = "Order Id and Order No not provided";
                    break;
               case (int)EnumTaskType.SubmitCVToDocControllerAdmin:
                    if (task.OrderItemId == 0 || task.OrderNo == 0 || task.CandidateId == 0) ErrorString = "Order Item/Candidate details not provided";
                    break;
               case (int)EnumTaskType.SelectionFollowupByDocControllerAdmin:
                    break;
               case (int)EnumTaskType.SelctionRegistrationByDocControllerAdmin:
                    if (task.OrderId == 0 || task.OrderNo == 0) ErrorString = "Order Id and Order No not provided";
                    break;
               case (int)EnumTaskType.MedicalTestsMobilization:
                    if (task.OrderId == 0 || task.OrderNo == 0) ErrorString = "Order Id and Order No not provided";
                    break;
               case (int)EnumTaskType.VisaDocsKSACompilation:
                    break;
               case (int)EnumTaskType.VisaDocsNonKSACompilation:
                    break;
               case (int)EnumTaskType.EmigrationAppLodging:
                    break;
               case (int)EnumTaskType.TravelTicketBooking:
                    break;
               case (int)EnumTaskType.OrderEditedAdvise:
                    break;
          }

          return ErrorString;
     }
}
}