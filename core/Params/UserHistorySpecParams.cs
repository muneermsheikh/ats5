using System;
using core.Entities.Admin;
using core.Params;

namespace core.Params
{
    public class UserHistorySpecParams: ParamPages
    {
        public int? Id {get; set;}
        public string PartyName { get; set; }
        public string AadharNo {get; set;}
        public int? CandidateId {get; set;}
        public int? CustomerOfficialId {get; set;}
        public int? ApplicationNo {get; set;}
        public string EmailId {get; set;}
        public string PhoneNo {get; set;}
        public bool CreateNewIfNull {get; set;}=false;
    }
}