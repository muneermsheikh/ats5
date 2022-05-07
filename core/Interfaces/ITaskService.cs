using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.EmailandSMS;
using core.Entities.Orders;
using core.Entities.Tasks;
using core.Params;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface ITaskService
    {
        //Task<ApplicationTask> GetHRExecutiveTask(int candidateId, int orderitemid);
        Task<Pagination<ApplicationTask>> GetApplicationTasksPaginated(TaskParams taskParams);
        Task<Pagination<ApplicationTask>> GetApplicationPendingTasksPaginated(string taskStatus, int pageIndex, int pageSize);
        Task<Pagination<ApplicationTask>> GetApplicationPendingTasksOfAUserPaginated(int userid, int pageIndex, int pageSize);
        Task<ICollection<TaskDashboardDto>> GetDashboardTasks(int loggedInEmployeeId);
        Task<MessagesDto> CreateNewApplicationTask(ApplicationTask task, int LoggedInEmployeeId);
        /*
        Task<ICollection<CVSubmitToHRSupDto>> CreateAndSaveTaskForCVRvwByHRSup(
            LoggedInUserDto loggedInDto, ICollection<CVSubmitToHRSupDto> cvsSubmitted);
        Task<ICollection<CVSubmitToHRMDto>> CreateAndSaveTaskForCVRvwByHRM(
            LoggedInUserDto loggedInDto, ICollection<CVSubmitToHRMDto> cvsSubmitted);
        */
        Task<MessagesDto> EditApplicationTask(ApplicationTask task, int employeeId);
        Task<bool> DeleteApplicationTask(ApplicationTask task);
        Task<TaskItem> CreateNewTaskItem(TaskItem taskItem);
        Task<TaskItem> EditTaskItem(TaskItem taskItem);
        Task<bool> DeleteTaskItem(TaskItem taskItem);
        Task<bool> SetApplicationTaskStatus(int ApplicationTaskId, DateTime dateOfStatus, 
               string TaskStatus, string UserName, int AppUserId);
        //Task<ICollection<EmailMessage>> CreateTaskForHRExecAssignment(ICollection<int> OrderItemIds, LoggedInUserDto loggedInDto);
        Task<ICollection<EmailMessage>> CreateTaskForHRExecOnOrderItemIds(ICollection<OrderAssignmentDto> assignments, int loggedInUserEmployeeId);
        Task<ApplicationTask> GetHRExecTaskForCVCompiling(int orderitemId, int candidateId);   
        Task<ApplicationTask> GetHRSupTaskForCVCompiling(int orderitemId, int candidateId);
        
    }
}