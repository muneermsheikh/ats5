using System;
using core.Entities.Admin;
using core.Params;

namespace core.Params
{
    public class UserHistoryParams
    {
        public int? Id {get; set;}
        public string PersonType {get; set;}
        public string PersonName { get; set; }
        public int? PersonId {get; set;}
        public int? ApplicationNo {get; set;}
        public string EmailId {get; set;}
        public string MobileNo {get; set;}
        public bool CreateNewIfNull {get; set;}=false;
    }
}