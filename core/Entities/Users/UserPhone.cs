using System.ComponentModel.DataAnnotations;

namespace core.Entities.Users
{
    public class UserPhone
    {
        public UserPhone()
        {
        }

        public UserPhone(int candidateId, string phoneNo, bool isMain, bool isValid)
        {
            PhoneNo = phoneNo;
            IsMain = isMain;
            IsValid = isValid;
            CandidateId = candidateId;
        }

        public int CandidateId { get; set; }
        public int Id { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public bool IsMain {get; set;}
        public bool IsValid { get; set; }=true;

        public Candidate Candidate {get; set;}
    }
}