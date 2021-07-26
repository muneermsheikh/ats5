using System;
using System.Linq;
using core.Entities;
using core.Entities.Users;

namespace core.Specifications
{
    public class CandidateForCountSpecs: BaseSpecification<Candidate>
    {
        public CandidateForCountSpecs(CandidateSpecParams candParams)
            : base(x => 
                (string.IsNullOrEmpty(candParams.Search) || 
                  x.FullName.ToLower().Contains(candParams.Search.ToLower())) &&
                /* 
                (!candParams.IndustryId.HasValue || 
                  x.UserProfessions.Select(x => x.IndustryId).Contains(Convert.ToInt32(candParams.IndustryId))) &&
                (!candParams.ProfessionId.HasValue || 
                  x.UserProfessions.Select(x => x.CategoryId).Contains(Convert.ToInt32(candParams.ProfessionId))) &&
                (!string.IsNullOrEmpty(candParams.City) ||
                  x.UserAddresses.Select(x => x.City.ToLower()).Contains(candParams.City.ToLower())) &&
                (!string.IsNullOrEmpty(candParams.District) ||
                  x.UserAddresses.Where(x => x.District.ToLower().Contains(candParams.District.ToLower()))
                  .Select(x => x.District).FirstOrDefault().ToLower().Contains(candParams.District)) &&
                (!string.IsNullOrEmpty(candParams.State) ||
                  x.UserAddresses.Where(x => x.IsMain)
                  .Select(x => x.District).FirstOrDefault().ToLower().Contains(candParams.District)) &&
                */
                (!string.IsNullOrEmpty(candParams.Email) ||
                  x.Email.ToLower() == candParams.Email.ToLower()) 
                )
                
        {
        }

        public CandidateForCountSpecs(int id) 
            : base(x => x.Id == id)
        {
        }

        public CandidateForCountSpecs(string appUserId)
        : base(x => x.AppUserIdNotEnforced == appUserId)
        {
        }
  
    }
}