using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using core.Entities.MasterEntities;
using core.Entities.Orders;
using core.Entities.Process;
using core.Entities.Users;

namespace core.Entities.HR
{
    public class CVRef: BaseEntity
    {
        public CVRef()
        {
        }


          public CVRef(int orderItemId, int categoryId, int orderId, int orderNo, string customerName, 
                string categoryName, int candidateId, int applicationNo, string candidateName, DateTime referredOn, 
                int charges)
          {
               OrderItemId = orderItemId;
               CategoryId = categoryId;
               OrderId = orderId;
               OrderNo = orderNo;
               CustomerName = customerName;
               CategoryName = categoryName;
               CandidateId = candidateId;
               ApplicationNo = applicationNo;
               CandidateName = candidateName;
               ReferredOn = referredOn;
               Charges = charges;
          }

        public int OrderItemId { get; set; }
        public int CategoryId {get; set;}
        public int OrderId {get; set;}
        public int OrderNo {get; set;}
        public string CustomerName {get; set;}
        public string CategoryName {get; set;}
        public int CandidateId { get; set; }
        public int ApplicationNo {get; set;}
        public string CandidateName {get; set;}
        public DateTime ReferredOn { get; set; } = DateTime.Now;
        public int? DeployStageId { get; set; }
        public int Charges {get; set;}
        public string PaymentIntentId {get; set;}
        public EnumCVRefStatus RefStatus { get; set; }=EnumCVRefStatus.Referred;
        public DeployStage DeployStage {get; set;}
        public DateTime? DeployStageDate {get; set;}
        public ICollection<Candidate> Candidates {get; set;}
        [ForeignKey("OrderItemId")]
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Deploy> Deploys {get; set;}
        public SelectionDecision SelectionDecision {get; set;}
        
    }
}