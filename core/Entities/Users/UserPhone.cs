using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace core.Entities.Users
{
    public class UserPhone: BaseEntity
    {
        public UserPhone()
        {
        }
        
        public UserPhone(string phoneNo, string mobileNo, bool isMain)
        {
            PhoneNo = phoneNo;
            MobileNo = mobileNo;
            IsMain = isMain;
        }

        public UserPhone(int candidateId, string phoneNo, string mobileNo, bool isMain)
        {
            PhoneNo = phoneNo;
            IsMain = isMain;
            MobileNo=mobileNo;
            CandidateId = candidateId;
        }

        public int CandidateId { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public bool IsMain {get; set;}=false;
        public bool IsValid { get; set; }=true;
        //public Candidate Candidate {get; set;}
    }
}