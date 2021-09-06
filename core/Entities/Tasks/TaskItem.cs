using System;
using System.ComponentModel.DataAnnotations;
using core.Entities.Orders;
using core.ParamsAndDtos;

namespace core.Entities.Tasks
{
    public class TaskItem: BaseEntity
    {
          public TaskItem()
          {
          }

          public TaskItem(int taskTypeId, int taskId, DateTime transactionDate, string taskStatus, 
                string taskItemDescription, int employeeId, int orderId, int orderItemId, int orderNo, 
                string userId, DateTime? nextFollowupOn, int candidateid, int NextFollowupById, 
                int quantity, ApplicationTask applicationTask)
          {
               TaskTypeId = taskTypeId;
               ApplicationTaskId = taskId;
               TransactionDate = transactionDate;
               TaskStatus = taskStatus;
               TaskItemDescription = taskItemDescription;
               EmployeeId = employeeId;
               OrderId = orderId;
               OrderItemId = orderItemId;
               OrderNo = orderNo;
               UserId = userId;
               CandidateId = candidateid;
               NextFollowupOn = nextFollowupOn;
               ApplicationTask = applicationTask;
               Quantity = quantity;
          }

        [Required]
        public int ApplicationTaskId { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        //public string TaskType { get; set; }
        
        [Required]
        public int TaskTypeId {get; set;}
        [Required]
        public string TaskStatus { get; set; }
        [Required]
        public string TaskItemDescription {get; set;}
        [Required]
        public int EmployeeId {get; set;}
        public int OrderId { get; set; }
        public int OrderItemId { get; set; }
        public int OrderNo { get; set; }
        public int CandidateId { get; set; }
        [Required]
        public string UserId {get; set;}
        public int Quantity { get; set; }=1;
        public DateTime? NextFollowupOn {get; set;}
        public int? NextFollowupById {get; set;}
        public ApplicationTask ApplicationTask {get; set;}
    }
}