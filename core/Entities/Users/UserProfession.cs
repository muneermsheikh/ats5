namespace core.Entities.Users
{
    public class UserProfession:  BaseEntity
    {
        public UserProfession()
        {
        }

        public UserProfession(int candidateId, int categoryId, int industryId, bool isMain)
        {
            CandidateId = candidateId;
            CategoryId = categoryId;
            IndustryId = industryId;
            IsMain = isMain;
        }
        public UserProfession(int categoryId, int industryId, bool isMain)
        {
            CategoryId = categoryId;
            IndustryId = industryId;
            IsMain = isMain;
        }


        public int CandidateId { get; set; }
        public int CategoryId { get; set; }
        public int IndustryId { get; set; }
        public bool IsMain { get; set; }

        //public Candidate Candidate {get; set;}
    }
}