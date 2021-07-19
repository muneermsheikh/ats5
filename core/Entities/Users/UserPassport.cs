using System;

namespace core.Entities.Users
{
    public class UserPassport: BaseEntity
    {
        public UserPassport()
        {
        }

        public UserPassport(string passportNo, string nationality, DateTime issuedOn, 
            DateTime validity, bool isValid)
        {
            PassportNo = passportNo;
            Nationality = nationality;
            IssuedOn = issuedOn;
            Validity = validity;
            IsValid = isValid;
        }

        public int CandidateId { get; set; }
        public string PassportNo { get; set; }
        public string Nationality {get; set;}
        public DateTime IssuedOn { get; set; }
        public DateTime Validity { get; set; }
        public bool IsValid { get; set; }

        //public Candidate Candidate {get; set;}
    }
}