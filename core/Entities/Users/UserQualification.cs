namespace core.Entities.Users
{
    public class UserQualification: BaseEntity
    {
          public UserQualification()
          {
          }

          public UserQualification(int qualificationId, bool isMain)
          {
               QualificationId = qualificationId;
               IsMain = isMain;
          }

        public int CandidateId { get; set; }
        public int QualificationId { get; set; }
        public bool IsMain { get; set; }
        //public Candidate Candidate {get; set;}
    }
}