using System;
using System.Linq;
using core.Entities;

namespace core.Specifications
{
    public class CustomersWithFiltersForCountSpecs: BaseSpecification<Customer>
    {
        public CustomersWithFiltersForCountSpecs(CustomerSpecParams custParams)
            : base(x => 
                (string.IsNullOrEmpty(custParams.Search) || 
                    x.CustomerName.ToLower().Contains(custParams.Search.ToLower())) &&
                (!custParams.IndustryId.HasValue || 
                    x.CustomerIndustries.Select(x => x.IndustryId)
                    .Contains(Convert.ToInt32(custParams.IndustryId))))
        {
            
        }
    }
}