using System;
using System.Linq;
using System.Linq.Expressions;
using core.Entities;
using core.Entities.Users;

namespace core.Specifications
{
     public class CandidateSpecs : BaseSpecification<Candidate>
     {
          public CandidateSpecs(CandidateSpecParams candParams)
            : base(x => 
                (string.IsNullOrEmpty(candParams.Search) || 
                  x.FullName.ToLower().Contains(candParams.Search.ToLower())) &&
                (!candParams.IndustryId.HasValue || 
                  x.UserProfessions.Select(x => x.IndustryId).Contains(Convert.ToInt32(candParams.IndustryId))) &&
                (!candParams.ProfessionId.HasValue || 
                  x.UserProfessions.Select(x => x.CategoryId).Contains(Convert.ToInt32(candParams.ProfessionId))) &&
                (!string.IsNullOrEmpty(candParams.City) ||
                  x.City.ToLower().Contains(candParams.City.ToLower()))
                /* &&
                (!string.IsNullOrEmpty(candParams.District) ||
                  x.Addresses.Where(x => x.District.ToLower().Contains(candParams.District.ToLower()))
                  .Select(x => x.District).FirstOrDefault().ToLower().Contains(candParams.District)) &&
                (!string.IsNullOrEmpty(candParams.State) ||
                  x.Addresses.Where(x => x.IsMain)
                  .Select(x => x.District).FirstOrDefault().ToLower().Contains(candParams.District)) 
                */
                  &&
                (!string.IsNullOrEmpty(candParams.Email) ||
                  x.Email.ToLower() == candParams.Email.ToLower()) 
                )
          {
              //AddInclude(x => x.Addresses);
              AddInclude(x => x.UserAttachments);
              AddInclude(x => x.UserPassports);
              AddInclude(x => x.UserProfessions);
              AddInclude(x => x.UserPhones);
              AddInclude(x => x.UserQualifications);
              AddOrderBy(x => x.ApplicationNo);

              ApplyPaging(candParams.PageSize * (candParams.PageIndex - 1), candParams.PageSize);

              if (!string.IsNullOrEmpty(candParams.Sort)) {
                switch(candParams.Sort.ToLower()) {
                  case "nameasc":
                    AddOrderBy(x => x.FullName);
                    break;
                  case "namddesc":
                    AddOrderByDescending(x => x.FullName);
                    break;
                  
                  case "cityasc":
                    AddOrderBy(x => x.City);
                    break;
                  
                  case "citydesc":
                    AddOrderByDescending(x => x.City);
                    break;
                /*
                  case "distasc":
                    AddOrderBy(x => x.Addresses.Select(x => x.District));
                    break;
                
                  case "distdesc":
                    AddOrderByDescending(x => x.Addresses.Select(x => x.District));
                    break;
                */
                  default: AddOrderBy(x => x.ApplicationNo);
                    break;
                }
              }
          }

          public CandidateSpecs(int id) 
            : base(x => x.Id == id)
          {
              //AddInclude(x => x.Addresses);
              AddInclude(x => x.UserAttachments);
              AddInclude(x => x.UserPassports);
              AddInclude(x => x.UserProfessions);
              AddInclude(x => x.UserPhones);
              AddInclude(x => x.UserQualifications);
              AddOrderBy(x => x.ApplicationNo);
          }
          public CandidateSpecs(string appUserId) 
            : base(x => x.AppUserId == appUserId)
          {
              //AddInclude(x => x.Addresses);
              AddInclude(x => x.UserAttachments);
              AddInclude(x => x.UserPassports);
              AddInclude(x => x.UserProfessions);
              AddInclude(x => x.UserPhones);
              AddInclude(x => x.UserQualifications);
              AddOrderBy(x => x.ApplicationNo);
          }
          
     }
}