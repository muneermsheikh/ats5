using System.Collections.Generic;
using core.Entities.Users;

namespace core.ParamsAndDtos
{
    public class CandidateInBriefDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public int ApplicationNo { get; set; }
        public string PassportNo { get; set; }
        public ICollection<UserProfession> UserProfessions {get; set;}
        public ICollection<UserPhone> UserPhones { get; set; }
    }
}