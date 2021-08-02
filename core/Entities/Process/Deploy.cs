using System;
using System.ComponentModel.DataAnnotations.Schema;
using core.Entities.HR;

namespace core.Entities.Process
{
    public class Deploy: BaseEntity
    {
          public Deploy()
          {
          }

          public Deploy(int cVRefId, DateTime transactionDate, int stageId)
          {
               CVRefId = cVRefId;
               TransactionDate = transactionDate;
               StageId = stageId;
          }

        public int CVRefId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int StageId { get; set; }
        public int NextStageId { get; set; }
        [ForeignKey("CVRefId")]
        public DateTime NextEstimatedStageDate { get; set; }
        public CVRef CVRef {get; set;}
    }
}