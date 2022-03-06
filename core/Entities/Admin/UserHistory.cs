using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace core.Entities.Admin
{
    public class UserHistory: BaseEntity
    {
        public UserHistory()
        {
        }

        public UserHistory(int candidateid, string aadharno, string partyname, int applicationno, ICollection<UserHistoryItem> items)
        {
            CandidateId = candidateid;
            ApplicationNo = applicationno;
            PartyName = partyname;
            AadharNo = aadharno;
            UserHistoryItems = items;
        }
        public UserHistory(int officialid, string partyname)
        {
            CustomerOfficialId = officialid;
            PartyName = partyname;
        }

        public string PartyName {get; set;}
        public int CustomerOfficialId {get; set;}
        public int CandidateId {get; set;}
        public string AadharNo {get; set;}
        public string PhoneNo {get; set;}
        public string EmailId {get; set;}
        public int ApplicationNo {get; set;}
        public DateTime CreatedOn {get; set;}
        public ICollection<UserHistoryItem> UserHistoryItems {get; set;}
    }
}