using System;
using System.Collections.Generic;
using core.Entities.Orders;

namespace core.Entities.HR
{
    public class CandidateAssessment: BaseEntity
    {
        public CandidateAssessment()
          {
          }

          public CandidateAssessment(int candidateId, int orderItemId, int assessedById, DateTime assessedOn, 
            ICollection<CandidateAssessmentItem> candidateassessmentitems)
          {
               CandidateId = candidateId;
               OrderItemId = orderItemId;
               AssessedById = assessedById;
               AssessedOn = assessedOn;
               CandidateAssessmentItems = candidateassessmentitems;
          }

        public int OrderItemId { get; set; }
        public int AssessedById { get; set; }
        public int CandidateId {get; set;}
        public DateTime AssessedOn { get; set; }
        public string AssessResult { get; set; }
        public string Remarks { get; set; }
        public OrderItem OrderItem { get; set; }
        public ICollection<CandidateAssessmentItem> CandidateAssessmentItems { get; set; }
    }
}