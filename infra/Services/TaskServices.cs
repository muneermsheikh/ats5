using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
          private readonly IComposeMessagesForHR _composeMsgHR;
          private readonly IUnitOfWork _unitOfWork;
          private readonly IEmailService _emailService;
          private readonly IEmployeeService _empService;
          private readonly IMapper _mapper;
          public TaskServices(ICommonServices commonServices, ATSContext context, IConfiguration config, IMapper mapper,
               IComposeMessagesForHR composeMsgHR, IUnitOfWork unitOfWork, IEmailService emailService, IEmployeeService empService)
          {
               _empService = empService;
               _mapper = mapper;
               _emailService = emailService;
               _unitOfWork = unitOfWork;
               _composeMsgHR = composeMsgHR;
               _context = context;
               _commonServices = commonServices;
               EmployeeIdDocController = Convert.ToInt32(config.GetSection("EmpDocControllerAdminId").Value);
               TargetDays_HRSupToReviewCV = Convert.ToInt32(config.GetSection("TargetDays_HRSupToReviewCV").Value);
               TargetDays_DocControllerToFwdCV = Convert.ToInt32(config.GetSection("TargetDays_DocControllerToFwdCV").Value);

          }
     
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
                    OrderDate = o.OrderDate.Date, Quantity = i.Quantity, HrExecId = (int)i.HrExecId, 
                    CompleteBy = i.CompleteBefore.Year < 2000 ? o.CompleteBy.Date : i.CompleteBefore.Date, CustomerName = cust.CustomerName, CustomerId = o.CustomerId, 
                    ProjectManagerId = o.ProjectManagerId, CityOfWorking = o.CityOfWorking,
               }).ToListAsync();
          return orderassignmentdto;
     }

     public async Task<ICollection<EmailMessage>> CreateTaskForHRExecOnOrderItemIds(ICollection<OrderAssignmentDto> assignments, int loggedInEmployeeId)
     {

          var tasks = new List<ApplicationTask>();
          var task = new ApplicationTask();

          foreach(var t in assignments)
          {
               if (string.IsNullOrEmpty(t.CustomerName)) t.CustomerName=await _commonServices.CustomerNameFromCustomerId(t.CustomerId);
               if (string.IsNullOrEmpty(t.CategoryName)) t.CategoryName=await _commonServices.CategoryNameFromCategoryId(t.CategoryId);
               if (string.IsNullOrEmpty(t.ProjectManagerPosition)) t.ProjectManagerPosition=await _commonServices.GetEmployeePositionFromEmployeeId(t.CustomerId);
               var taskitems = new List<TaskItem>();
               
               taskitems.Add(new TaskItem((int)EnumTaskType.AssignTaskToHRExec, DateTime.Now, "not started", "task initiated",  loggedInEmployeeId,
                    t.OrderId, t.OrderItemId, t.OrderNo, loggedInEmployeeId, DateTime.Now.AddDays(7), t.HrExecId,t.Quantity));

               task = new ApplicationTask((int)EnumTaskType.AssignTaskToHRExec, DateTime.Now, t.ProjectManagerId, t.HrExecId, (int)t.OrderId, (int)t.OrderNo,
                    (int)t.OrderItemId, "Task assigned for " + t.Quantity + " CVs of " + t.CategoryName + ", Category Reference " + t.CategoryRef + " for " + t.CustomerName, 
                    t.CompleteBy, "Not Started", 0, taskitems);
               string ErrString = ValidateTaskObject(task);
               if (string.IsNullOrEmpty(ErrString)) tasks.Add(task);

          }
          
          if(tasks==null || tasks.Count==0) return null;

          foreach(var tsk in tasks) {
               _unitOfWork.Repository<ApplicationTask>().Add(tsk);
          }
          
          var recordsAffected = await _unitOfWork.Complete();

          if (recordsAffected == 0) throw new Exception("Failed to create the task");

          var emailMsgs = (List<EmailMessage>) await _composeMsgHR.ComposeMessagesToHRExecToSourceCVs(assignments);
          if (emailMsgs != null && emailMsgs.Count ==0) return null;

          foreach(var msg in emailMsgs)
          {
               if ((msg.PostAction == (int)EnumPostTaskAction.ComposeAndSendEmail || 
                    msg.PostAction == (int)EnumPostTaskAction.ComposeAndSendEmailComposeAndSendSMS ))
               {
                    var attachments = new List<string>();
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
     public async Task<bool> CreateTaskForHRExecOnOrderItemId_s(Order modelOrder, int loggedInEmployeeId)
     {
          //var orderassignmentdto = await GetAssignmentDtoFromItemIds(OrderItemIds);
          var ids = modelOrder.OrderItems.Select(x => x.Id).ToList();
          var existingOrderItems = await _context.OrderItems.Where(x => ids.Contains(x.Id)).ToListAsync();
          var tasks = new List<ApplicationTask>();
          int added=0;
          var orderitemid = modelOrder.OrderItems.Select(x => x.Id).FirstOrDefault();
          var CustomerName = await _context.Customers.Where(x => x.Id == modelOrder.CustomerId).Select(x => x.CustomerName).FirstOrDefaultAsync();

          var cats = await _context.Categories.Where(x => modelOrder.OrderItems.Select(x => x.CategoryId).ToList().Contains(x.Id)).Select(x => new {x.Id, x.Name}).ToListAsync();

          foreach(var model in modelOrder.OrderItems)
          {
               var existingOrderItem = existingOrderItems.Where(x => x.Id == model.Id).FirstOrDefault();
               if (existingOrderItem.HrExecId != model.HrExecId) {
                    existingOrderItem.HrExecId = model.HrExecId;
                    _unitOfWork.Repository<OrderItem>().Update(existingOrderItem);

                    var task = new ApplicationTask((int)EnumTaskType.AssignTaskToHRExec, DateTime.Now,
                         loggedInEmployeeId, (int)model.HrExecId, model.OrderId, model.OrderNo,
                         model.Id, "Category Ref " + modelOrder.OrderNo + "-" + model.SrNo + " " + 
                         cats.Where(x => x.Id == model.CategoryId).Select(x => x.Name).FirstOrDefault() +
                         " total " + model.Quantity + " for " + CustomerName + " assigned to you", 
                         model.CompleteBefore.Date, "Open", 0, null);
                    //task.PostTaskAction = EnumPostTaskAction.ComposeAndSendEmail;
               
                    _unitOfWork.Repository<ApplicationTask>().Add(task);
                    added++;
               }
          }

          if(added == 0) throw new Exception("no valid records found to generate order assignments");
          var recordsAffected = await _unitOfWork.Complete();

          if (recordsAffected == 0) throw new Exception("Failed to create the task");

          var dtos = new List<OrderAssignmentDto>();
          foreach(var model in modelOrder.OrderItems)
          {
               dtos.Add(new OrderAssignmentDto{
                    OrderId = model.OrderId, OrderNo = modelOrder.OrderNo, OrderDate = modelOrder.OrderDate, 
                    CityOfWorking = modelOrder.CityOfWorking, ProjectManagerId = modelOrder.ProjectManagerId, 
                    ProjectManagerPosition="not retrieved from DB", OrderItemId = model.Id,
                    HrExecId = (int)model.HrExecId, CategoryRef=modelOrder.OrderNo + "-" + model.SrNo,
                    CategoryName = cats.Where(x => x.Id == model.CategoryId).Select(x => x.Name).FirstOrDefault(),
                    CustomerId = modelOrder.CustomerId, CustomerName  = CustomerName, CompleteBy = model.CompleteBefore});
          }
          

          //var orderassignmentdto = _mapper.Map<ICollection<OrderItem>, ICollection<OrderAssignmentDto>>(orderitems);
          var msgs = (List<EmailMessage>) await _composeMsgHR.ComposeMessagesToHRExecToSourceCVs(dtos);
          
          //for this function, the post action is: composeandsendemail
          if(msgs!=null && msgs.Count > 0)
          {
               var attachments = new List<string>();
               foreach(var msg in msgs)
               {
                    _emailService.SendEmail(msg, attachments);
               }
          }
          
          /*   ** TODO ** - test for recipient particulars before allowing direct send
          if (msg != null) {
               var attachments = new List<string>();        // TODO - should this be auto-sent?
               msg = await _emailService.SendEmail(msg, attachments);
          }
          */               

          return recordsAffected > 0 ;
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
                         await _composeMsgHR.ComposeMessagesToDesignOrderAssessmentQs((int)task.OrderId, loggedInDto);
                    break;

               case (int)EnumTaskType.AssignTaskToHRExec:
                    var orderitemids = await _context.OrderItems.Where(x => x.OrderId == task.OrderId).Select(x => x.Id).ToListAsync();

                    var assignmentdtos = await GetAssignmentDtoFromItemIds(orderitemids);
                    emailMsgs = (List<EmailMessage>)
                         await _composeMsgHR.ComposeMessagesToHRExecToSourceCVs((ICollection<OrderAssignmentDto>)orderitemids);
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

     public async Task<Pagination<ApplicationTask>> GetApplicationPendingTasksOfAUserPaginated(int userid, int pageIndex, int pageSize)
     {
          var specs = new TaskSpecs(userid, false, pageIndex, pageSize);
          var countSpec = new TaskForCountSpecs(userid, false);
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
               (int)t.OrderItemId, (int)t.OrderNo, employeeId, DateTime.Now.AddDays(2), (int)t.CandidateId, 0,0//, t
          );
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
                    //check for index integrity
                    //check for duplicate key row in object 'dbo.Tasks' for unique index 'IX_Tasks_AssignedToId_OrderItemId_CandidateId_TaskTypeId'. 
                    //The duplicate key value is (1023, 18, 0, 4).

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