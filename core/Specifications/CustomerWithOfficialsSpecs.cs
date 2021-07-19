using System;
using System.Linq;
using System.Linq.Expressions;
using core.Entities;

namespace core.Specifications
{
     public class CustomerWithOfficialsSpecs : BaseSpecification<Customer>
     {
          public CustomerWithOfficialsSpecs(CustomerSpecParams custParams)
            : base(x => 
                (string.IsNullOrEmpty(custParams.Search) || 
                  x.CustomerName.ToLower().Contains(custParams.Search.ToLower())) &&
                (!custParams.IndustryId.HasValue || 
                  x.CustomerIndustries.Select(x => x.IndustryId).Contains(Convert.ToInt32(custParams.IndustryId))))
          {
              AddInclude(x => x.CustomerOfficials);
              AddInclude(x => x.CustomerIndustries);
              AddOrderBy(x => x.CustomerName);
              ApplyPaging(custParams.PageSize * (custParams.PageIndex - 1), custParams.PageSize);

              if (!string.IsNullOrEmpty(custParams.Sort)) {
                switch(custParams.Sort.ToLower()) {
                  case "nameasc":
                    AddOrderBy(x => x.CustomerName);
                    break;
                  case "namddesc":
                    AddOrderByDescending(x => x.CustomerName);
                    break;
                  
                  case "typeasc":
                    AddOrderBy(x => x.CustomerType);
                    break;
                  
                  case "typedesc":
                    AddOrderByDescending(x => x.CustomerType);
                    break;
                  
                  case "cityasc":
                    AddOrderBy(x => x.City);
                    break;
                  
                  case "citydesc":
                    AddOrderByDescending(x => x.City);
                    break;

                  default: AddOrderBy(x => x.CustomerName);
                    break;
                }
              }
          }

          public CustomerWithOfficialsSpecs(int id) 
            : base(x => x.Id == id)
          {
              AddInclude(x => x.CustomerOfficials);
              AddInclude(x => x.CustomerIndustries);
          }
     }
}