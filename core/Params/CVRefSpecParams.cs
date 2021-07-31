using System;
using core.Entities.HR;
using core.ParamsAndDtos;

namespace core.Params
{
    public class CVRefSpecParams: ParamPages
    {
        public int? Id { get; set; }
        public int? OrderItemId { get; set; }
        public int? CandidateId {get; set;}
        public int? CategoryId {get; set;}
        public int? OrderId { get; set; }
        public int? OrderNo { get; set; }
        public int? ApplicationNo { get; set; }
        public string CustomerName {get; set;}
        public string CategoryName {get; set;}
        public string CandidateName {get; set;}
        public DateTime? ReferredOn { get; set; }
        public int[] Ids {get; set;}
        public bool IncludeDeployments { get; set; }
        public bool IncludeSelection { get; set; }
        public bool IncludeEmployment { get; set; }
    }
}