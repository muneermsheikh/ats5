using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace core.ParamsAndDtos
{
    public class UserHistoryDto
    {
        public UserHistoryDto()
        {
        }

        public UserHistoryDto(string partyName, int customerOfficialId, int candidateId, string aadharNo, string emailId, int applicationNo)
        {
            PartyName = partyName;
            CustomerOfficialId = customerOfficialId;
            CandidateId = candidateId;
            AadharNo = aadharNo;
            EmailId = emailId;
            ApplicationNo = applicationNo;
        }

        public UserHistoryDto(int id, string partyName, int customerOfficialId, int candidateId, string aadharNo, string emailId, int applicationNo, ICollection<UserHistoryItemDto> historyitems)
        {
            Id = id;
            PartyName = partyName;
            CustomerOfficialId = customerOfficialId;
            CandidateId = candidateId;
            AadharNo = aadharNo;
            EmailId = emailId;
            ApplicationNo = applicationNo;
            UserHistoryItems = historyitems;
        }

        public int Id {get; set;}
        public string PartyName {get; set;}
        public int CustomerOfficialId {get; set;}
        public int CandidateId {get; set;}
        public string AadharNo {get; set;}
        public string EmailId {get; set;}
        public int ApplicationNo {get; set;}
        public ICollection<UserHistoryItemDto> UserHistoryItems { get; set; }
    }
}