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

          public Deploy(int cVRefId, DateTime transactionDate, int statusId)
          {
               CVRefId = cVRefId;
               TransactionDate = transactionDate;
               StatusId = statusId;
          }

          public int CVRefId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int StatusId { get; set; }
        public int NextStatusId { get; set; }
        [ForeignKey("CVRefId")]
        public DateTime NextEstimatedStatusDate { get; set; }
        public CVRef CVRef {get; set;}
    }
}