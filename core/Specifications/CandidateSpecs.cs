using System;
using System.Linq;
using System.Linq.Expressions;
using core.Entities;
using core.Entities.Users;
using core.Params;

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
                  x.EntityAddresses.Select(x => x.City.ToLower()).Contains(candParams.City.ToLower())) &&
                (!string.IsNullOrEmpty(candParams.District) ||
                  x.EntityAddresses.Where(x => x.District.ToLower().Contains(candParams.District.ToLower()))
                  .Select(x => x.District).FirstOrDefault().ToLower().Contains(candParams.District)) &&
                (!string.IsNullOrEmpty(candParams.State) ||
                  x.EntityAddresses.Where(x => x.IsMain)
                  .Select(x => x.District).FirstOrDefault().ToLower().Contains(candParams.District)) &&
              
                (!string.IsNullOrEmpty(candParams.Email) ||
                  x.Email.ToLower() == candParams.Email.ToLower()) 
                )
          {
              if (candParams.IncludeEntityAddresses) AddInclude(x => x.EntityAddresses);
              if (candParams.IncludeAttachments) AddInclude(x => x.UserAttachments);
              if (candParams.IncludeUserPassorts) AddInclude(x => x.UserPassports);
              if (candParams.IncludeUserProfessions) AddInclude(x => x.UserProfessions);
              if (candParams.IncludeUserPhones) AddInclude(x => x.UserPhones);
              if (candParams.IncludeUserQualifications) AddInclude(x => x.UserQualifications);
              
              ApplyPaging(candParams.PageSize * (candParams.PageIndex - 1), candParams.PageSize);

              if (!string.IsNullOrEmpty(candParams.Sort)) {
                switch(candParams.Sort.ToLower()) {
                  case "nameasc":
                    AddOrderBy(x => x.FullName);
                    break;
                  case "namddesc":
                    AddOrderByDescending(x => x.FullName);
                    break;
                  /*
                  case "cityasc":
                    AddOrderBy(x => x.EntityAddresses.City);
                    break;
                  
                  case "citydesc":
                    AddOrderByDescending(x => x.City);
                    break;
                
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
              AddInclude(x => x.EntityAddresses);
              AddInclude(x => x.UserAttachments);
              AddInclude(x => x.UserPassports);
              AddInclude(x => x.UserProfessions);
              AddInclude(x => x.UserPhones);
              AddInclude(x => x.UserQualifications);
              
              AddOrderBy(x => x.ApplicationNo);
          }
          
          public CandidateSpecs(string appUserId) 
            : base(x => x.AppUserIdNotEnforced == appUserId)
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