using System;
using System.Linq;
using core.Entities;
using core.Entities.HR;
using core.Entities.Users;
using core.Params;
using core.ParamsAndDtos;

namespace core.Specifications
{
    public class CVRefSpecs: BaseSpecification<CVRef>
    {
        public CVRefSpecs(CVRefSpecParams specParams)
            : base(x => 
                (!specParams.OrderId.HasValue || x.OrderId == specParams.OrderId) &&
                (!specParams.OrderNo.HasValue || x.OrderNo == specParams.OrderNo) &&
                (!specParams.OrderItemId.HasValue || x.OrderItemId == specParams.OrderItemId) &&
                (!specParams.CategoryId.HasValue || x.CategoryId == specParams.CategoryId) &&
                (string.IsNullOrEmpty(specParams.CategoryName) || x.CategoryName == specParams.CategoryName) &&
                (!specParams.Id.HasValue || x.Id == specParams.Id) &&
                (!specParams.CandidateId.HasValue || x.CandidateId == specParams.CandidateId) &&
                (!specParams.ApplicationNo.HasValue ||  x.ApplicationNo == specParams.ApplicationNo) &&
                (specParams.Ids.Count() == 0 || specParams.Ids.Contains(x.Id))
            )
        {
            if(specParams.IncludeDeployments) AddInclude(x => x.Deploys);
            
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
            AddOrderBy(x => x.ReferredOn);
        }
  
    }
}