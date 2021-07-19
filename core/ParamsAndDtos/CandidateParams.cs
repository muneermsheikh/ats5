using System;

namespace core.ParamsAndDtos
{
    public class CandidateParams: ParamPages
    {
        public int? CandidateId { get; set; }
        public string AppUserId {get; set;}
        public string Email {get; set;}
        public string MobileNo {get; set;}
        public int? ProfessionId {get; set;}
        public DateTime? RegisteredDate { get; set; }
        public DateTime? RegisteredUpto {get; set;}
        
    }
}