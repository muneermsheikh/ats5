using System;
using System.Collections.Generic;
using core.Entities.HR;
using core.Entities.Process;

namespace core.ParamsAndDtos
{
    public class CVRefAndDeployDto
    {
        public int Id {get; set;}
        public int OrderItemId { get; set; }
        public int ApplicationNo { get; set; }
        public string CandidateName {get; set;}
        public string OrderNo {get; set;}
        public string CategoryName {get; set;}
        public DateTime ReferredOn { get; set; } = DateTime.Now;
        public EnumCVRefStatus RefStatus { get; set; }
        public ICollection<DeployDto> Deploys {get; set;}
    }

    public class DeployDto
    {
        public int CVRefId { get; set; }
        public DateTime TransactionDate { get; set; }
        public EnumDeployStatus DeployStatus { get; set; }
    }
}