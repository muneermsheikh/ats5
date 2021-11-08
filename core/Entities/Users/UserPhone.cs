using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace core.Entities.Users
{
    public class UserPhone: BaseEntity
    {
        public UserPhone()
        {
        }
        
        public UserPhone(string mobileNo, bool isMain)
        {
            MobileNo = mobileNo;
            IsMain = isMain;
        }

        public UserPhone(int candidateId, string mobileNo, bool isMain)
        {
            IsMain = isMain;
            MobileNo=mobileNo;
            CandidateId = candidateId;
        }

        public int CandidateId { get; set; }
        //public string PhoneNo { get; set; }   //user will have only mobile no
        [MinLength(10), MaxLength(10), Required]
        public string MobileNo { get; set; }
        public bool IsMain {get; set;}=false;
        public bool IsValid { get; set; }=true;
        public string Remarks { get; set; }
        //public Candidate Candidate {get; set;}
    }
}