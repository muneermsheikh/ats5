using System;
using System.Linq;
using core.Entities;
using core.Entities.Users;
using core.ParamsAndDtos;

namespace core.Specifications
{
    public class IndustryForCountSpecs: BaseSpecification<Industry>
    {
        public IndustryForCountSpecs(IndustryParams indParams)
            : base(x => 
                (string.IsNullOrEmpty(indParams.Search) || 
                  x.Name.ToLower().Contains(indParams.Search.ToLower())) 
                )
        {
        }

        public IndustryForCountSpecs(int id) 
            : base(x => x.Id == id)
        {
        }
  
    }
}