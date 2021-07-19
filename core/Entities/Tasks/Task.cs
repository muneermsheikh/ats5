using System;
using System.Collections.Generic;

namespace core.Entities.Tasks
{
    public class Task: BaseEntity
    {
        public string TaskType { get; set; }
        public DateTime TaskDate { get; set; } = DateTime.Now;
        public int TaskOwnerId {get; set;}
        public int AssignedToId {get; set;}
        public int? OrderId {get; set;}
        public int? OrderItemId {get; set;}
        public string TaskDescription {get; set;}
        public DateTime CompleteBy {get; set;}
        public string TaskStatus {get; set;}
        public ICollection<TaskItem> TaskItems {get; set;}

    }
}