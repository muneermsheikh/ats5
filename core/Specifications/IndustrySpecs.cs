using System;
using System.Linq;
using core.Entities;
using core.Entities.Users;
using core.ParamsAndDtos;

namespace core.Specifications
{
    public class IndustrySpecs: BaseSpecification<Industry>
    {
        public IndustrySpecs(IndustryParams indParams)
            : base(x => 
                (string.IsNullOrEmpty(indParams.Search) || 
                  x.Name.ToLower().Contains(indParams.Search.ToLower())) 
                )
        {
            AddOrderBy(x => x.Name);
        }

        public IndustrySpecs(int id) 
            : base(x => x.Id == id)
        {
        }
  
    }
}