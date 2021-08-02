using System;
using core.Entities.Process;

namespace core.ParamsAndDtos
{
    public class CommonDataDto
    {
        public string CustomerName { get; set; }
        public int ApplicationNo {get; set;}
        public string CandidateName { get; set; }
        public int OrderNo { get; set; }
        public int OrderId {get; set;}
        public int CategoryId {get; set;}
        public string CategoryName { get; set; }
        public int DeployStageId {get; set;}
        public DateTime DeployStageDate {get; set;}
    }
}