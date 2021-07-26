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

          public CVRef(int orderItemId, int candidateId, int charges, DateTime referredOn)
          {
              OrderItemId = orderItemId;
              CandidateId = candidateId;
              Charges = charges;
              ReferredOn  = referredOn;
          }

        public int OrderItemId { get; set; }
        public int CandidateId { get; set; }
        public DateTime ReferredOn { get; set; } = DateTime.Now;
        public int? DeployStageId { get; set; }
        public int Charges {get; set;}
        public string PaymentIntentId {get; set;}
        public EnumCVRefStatus RefStatus { get; set; }=EnumCVRefStatus.Referred;
        public DeployStage DeployStage {get; set;}
        public DateTime? DeployStageDate {get; set;}
        public Candidate Candidate {get; set;}
        [ForeignKey("OrderItemId")]
        public OrderItem OrderItem { get; set; }
        public ICollection<CVDeploy> Deploys {get; set;}
        
    }
}