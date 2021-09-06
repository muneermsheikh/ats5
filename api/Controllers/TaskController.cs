using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Errors;
using api.Extensions;
using core.Entities.Tasks;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class TaskController : BaseApiController
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ITaskService _taskService;
          private readonly IEmailService _emailService;
          public TaskController(IUnitOfWork unitOfWork, ITaskService taskService, IEmailService emailService)
          {
               _emailService = emailService;
               _taskService = taskService;
               _unitOfWork = unitOfWork;
          }

          [HttpPost]
          public async Task<ActionResult<ICollection<EmailAndSmsMessagesDto>>> CreateNewApplicationTask(ApplicationTask task)
          {
               //verify object data
               if (task.TaskDate.Year < 2000) return BadRequest(new ApiResponse(404, "Task Date not set"));
               if (task.CompleteBy.Year < 2000) return BadRequest(new ApiResponse(404, "Task Completion Date not set"));
               if (task.TaskOwnerId == 0) return BadRequest(new ApiResponse(404, "Bad Request - Task Owner not defined"));
               if (string.IsNullOrEmpty(task.TaskStatus)) return BadRequest(new ApiResponse(404, "Bad Request - task status not provided"));
               if (string.IsNullOrEmpty(task.TaskDescription)) return BadRequest(new ApiResponse(404, "Task Description cannot be blank"));
               if (task.AssignedToId == 0) return BadRequest(new ApiResponse(404, "Task not assigned to any one"));

               //TODO - verify assignedToId and TaskOwnerId exist
               var loggedIn = new LoggedInUserDto
               {
                    LoggedIAppUsername = User.GetUsername(),
                    LoggedInAppUserEmail = User.GetIdentityUserEmailId(),
                    LoggedInAppUserId = User.GetIdentityUserId()
               };

               var emailMessages = await _taskService.CreateNewApplicationTask(task, loggedIn);
               var AttachmentFilePaths = new List<string>();
               if (emailMessages != null &&
                   task.PostTaskAction != EnumPostTaskAction.OnlyComposeEmailAndSMSMessages
                   && task.PostTaskAction != EnumPostTaskAction.OnlyComposeEmailMessage)   //Send involved
               {
                    foreach (var msg in emailMessages)
                    {
                        _emailService.SendEmail(msg, AttachmentFilePaths);
                    }
               }

               return Ok(emailMessages);

          }

          [HttpPut]
          public async Task<ActionResult<ApplicationTask>> EditApplicationTask(ApplicationTask task)
          {
               return await _taskService.EditApplicationTask(task);
          }

          [HttpDelete]
          public async Task<bool> DeleteApplicationTask(ApplicationTask task)
          {
               return await _taskService.DeleteApplicationTask(task);
          }

          [HttpGet]
          public async Task<ActionResult<Pagination<ApplicationTask>>> GetApplicationTask(TaskParams taskParams)
          {
               var emps = await _taskService.GetApplicationTasksPaginated(taskParams);
               if (emps == null || emps.Count == 0) return BadRequest(new ApiResponse(404, "Failed to retrieve any tasks"));

               return Ok(emps);
          }


          [HttpGet("pendingtasks/{taskstatus}/{pageIndex}/{pageSize}")]
          public async Task<ActionResult<Pagination<ApplicationTask>>> GetPendingTasks(string taskstatus, int pageIndex, int pageSize)
          {
               var emps = await _taskService.GetApplicationPendingTasksPaginated(taskstatus, pageIndex, pageSize);
               if (emps == null || emps.Count == 0) return BadRequest(new ApiResponse(404, "Failed to retrieve any tasks"));

               return Ok(emps);
          }

          [Authorize]
          [HttpPut("applicationtask/{applicationTaskId}/{dateCompleted}")]
          public async Task<ActionResult<bool>> SetApplicationTaskAsCompleted(int applicationTaskId, DateTime dateCompleted)
          {
               var userName = User.GetUsername();
               var userId = User.GetIdentityUserId();
               return await _taskService.SetApplicationTaskStatus(applicationTaskId, dateCompleted, "Completed", userName, userId);
          }

          [Authorize]
          [HttpPut("applicationtask/{applicationTaskId}/{dateOfCancellation}")]
          public async Task<ActionResult<bool>> SetApplicationTaskAsCanceled(int applicationTaskId, DateTime dateOfCancellation)
          {
               var userName = User.GetUsername();
               var userId = User.GetIdentityUserId();
               return await _taskService.SetApplicationTaskStatus(applicationTaskId, dateOfCancellation, "Canceled", userName, userId);
          }

          [HttpPost("item")]
          public async Task<ActionResult<TaskItem>> AddATaskItem(TaskItem taskItem)
          {
               var item = await _taskService.CreateNewTaskItem(taskItem);
               if (item != null) return Ok(item);
               return BadRequest(new ApiResponse(404, "Failed to add the task item"));
          }

          [HttpPut("item")]
          public async Task<ActionResult<TaskItem>> EditTaskItem(TaskItem taskItem)
          {
               var t = await _taskService.EditTaskItem(taskItem);

               if (t == null) return BadRequest(new ApiResponse(404, "Failed to edit the task item"));

               return Ok(t);
          }

          [HttpDelete("item")]
          public async Task<bool> DeleteTaskItem(TaskItem taskItem)
          {
               return await _taskService.DeleteTaskItem(taskItem);
          }


     }
}