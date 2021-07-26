using System;
using System.Linq;
using core.Entities;
using core.Entities.Users;
using core.ParamsAndDtos;

namespace core.Specifications
{
    public class CategorySpecs: BaseSpecification<Category>
    {
        public CategorySpecs(CategoryParams catParams)
            : base(x => 
                (string.IsNullOrEmpty(catParams.Search) || 
                  x.Name.ToLower().Contains(catParams.Search.ToLower())) 
                )
        {
            AddOrderBy(x => x.Name);
        }

        public CategorySpecs(int id) 
            : base(x => x.Id == id)
        {
        }
  
    }
}