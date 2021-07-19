using System;

namespace core.Entities.Tasks
{
    public class TaskItem: BaseEntity
    {
        public int TaskId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TaskStatus { get; set; }
        public string TaskItemDescription {get; set;}
        public int UserId {get; set;}
        public DateTime? NextFollowupOn {get; set;}
        public int? NextFollowupById {get; set;}
        public Task Task {get; set;}
    }
}