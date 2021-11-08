using System;
using System.Linq;
using core.Entities;
using core.Entities.Users;
using core.Params;

namespace core.Specifications
{
    public class CandidateForCountSpecs: BaseSpecification<Candidate>
    {
        public CandidateForCountSpecs(CandidateSpecParams candParams)
            : base(x => 
                (!candParams.CandidateId.HasValue || x.Id == candParams.CandidateId)
                && (string.IsNullOrEmpty(candParams.Search) || 
                  x.FirstName.ToLower().Contains(candParams.Search) || x.FamilyName.ToLower().Contains(candParams.Search))
                && ((!candParams.ApplicationNoFrom.HasValue && !candParams.ApplicationNoUpto.HasValue)||
                    x.ApplicationNo >= candParams.ApplicationNoFrom &&
                    x.ApplicationNo <= candParams.ApplicationNoUpto)               
                &&(!candParams.RegisteredFrom.HasValue && !candParams.RegisteredUpto.HasValue || 
                    x.Created.Date >= candParams.RegisteredFrom &&
                    x.Created.Date <= candParams.RegisteredUpto) 
                && (!candParams.IndustryId.HasValue || 
                    x.UserProfessions.Select(x => x.IndustryId).Contains(Convert.ToInt32(candParams.IndustryId))) 
                && (!candParams.ProfessionId.HasValue || 
                    x.UserProfessions.Select(x => x.CategoryId).Contains(Convert.ToInt32(candParams.ProfessionId))) 
                && (!candParams.AppUserId.HasValue || x.AppUserId == candParams.AppUserId)
                //&& (string.IsNullOrEmpty(candParams.CandidateStatus) || candParams.CandidateStatus.Split(",").ToList().Contains(x.CandidateStatus) )
                /*&& (string.IsNullOrEmpty(candParams.City) ||
                  x.City.ToLower().Contains(candParams.City.ToLower())) */
                && (string.IsNullOrEmpty(candParams.City) ||
                  x.EntityAddresses.Select(x => x.City.ToLower()).Contains(candParams.City.ToLower()))
                  //.Select(x => x.City).FirstOrDefault().ToLower().Contains(candParams.City.ToLower())) 
                /*&& (!string.IsNullOrEmpty(candParams.District) ||
                  x.EntityAddresses.Where(x => x.District.ToLower().Contains(candParams.District.ToLower()))
                  .Select(x => x.District).FirstOrDefault().ToLower().Contains(candParams.District)) 
                && (!string.IsNullOrEmpty(candParams.State) ||
                  x.EntityAddresses.Where(x => x.IsMain)
                  .Select(x => x.District).FirstOrDefault().ToLower().Contains(candParams.District)) 
                && (!string.IsNullOrEmpty(candParams.Email) ||
                  x.Email.ToLower() == candParams.Email.ToLower()) 
            */
            )
                
        {
        }

        public CandidateForCountSpecs(int id, string dummy ) 
            : base(x => x.Id == id)
        {
        }

        public CandidateForCountSpecs(int appUserId)
        : base(x => x.AppUserId == appUserId)
        {
        }
  
    }
}