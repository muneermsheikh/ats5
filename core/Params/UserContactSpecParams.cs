using System;
using core.Entities.Admin;
using core.Params;

namespace core.Params
{
    public class UserContactSpecParams: ParamPages
    {
        public int? CandidateId { get; set; }
        public int? OrderItemId {get; set;}
        public int? OrderId {get; set;}
        public string Subject {get; set;}
        public string UserPhoneNoContacted {get; set;}
        public DateTime? DateOfContactFrom {get; set;}
        public DateTime? DateOfContactUpto {get; set;}
        public EnumContactResult? enumContactResult {get; set;}
        public int? loggedInUserId {get; set;}
    }
}