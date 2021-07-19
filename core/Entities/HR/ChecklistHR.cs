using System;
using System.Collections.Generic;
using core.Entities.Orders;
using core.Entities.Users;

namespace core.Entities.HR
{
    public class ChecklistHR: BaseEntity
    {
         public ChecklistHR()
          {
          }

          public ChecklistHR(int candidateId, int orderItemId, int userId, DateTime checkedOn, ICollection<ChecklistItemHR> checklistItemHRs)
          {
               CandidateId = candidateId;
               OrderItemId = orderItemId;
               UserId = userId;
               CheckedOn = checkedOn;
               ChecklistItemHRs = checklistItemHRs;
          }

        public int CandidateId { get; set; }
        public int OrderItemId { get; set; }
        public int UserId { get; set; }
        public DateTime CheckedOn {get; set;}
        public ICollection<ChecklistItemHR> ChecklistItemHRs {get; set;}
        public Candidate Candidate {get; set;}
        public OrderItem OrderItem {get; set;}
    }
}