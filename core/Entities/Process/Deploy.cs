using System;
using System.ComponentModel.DataAnnotations.Schema;
using core.Entities.HR;

namespace core.Entities.Process
{
    public class Deploy: BaseEntity
    {
        public int CVRefId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int DeployStatusId { get; set; }
        public int DeployStageId { get; set; }
        public int NextDeployStageId { get; set; }
        [ForeignKey("CVRefId")]
        public CVRef CVRef {get; set;}
        public DateTime NextDeployStageEstimatedDate { get; set; }
    }
}