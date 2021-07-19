using System;
using core.Entities.HR;

namespace core.Entities.Process
{
    public class CVDeploy: BaseEntity
    {
        public int CVRefId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int DeployStageId { get; set; }
        public int NextDeployStageId { get; set; }
        public CVRef CVRef {get; set;}
        public DateTime NextDeployStageEstimatedDate { get; set; }
    }
}