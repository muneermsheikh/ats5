using System;
using System.Linq;
using core.Entities;
using core.Entities.HR;
using core.Entities.Orders;
using core.Entities.Users;
using core.ParamsAndDtos;

namespace core.Specifications
{
    public class SelectionDecisionSpecs: BaseSpecification<SelectionDecision>
    {
        public SelectionDecisionSpecs(SelDecisionSpecParams specParams)
            : base(x => 
                (!specParams.OrderId.HasValue || x.OrderId == specParams.OrderId) &&
                (!specParams.OrderNo.HasValue || x.OrderNo == specParams.OrderNo) &&
                (!specParams.OrderItemId.HasValue || x.OrderItemId == specParams.OrderItemId) &&
                (!specParams.CategoryId.HasValue || x.CategoryId == specParams.CategoryId) &&
                (string.IsNullOrEmpty(specParams.CategoryName) || x.CategoryName == specParams.CategoryName) &&
                (!specParams.CVRefId.HasValue || x.CVRefId == specParams.CVRefId) &&
                (!specParams.CandidateId.HasValue || x.CandidateId == specParams.CandidateId) &&
                (!specParams.ApplicationNo.HasValue ||  x.ApplicationNo == specParams.ApplicationNo)
                )
        {
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
            AddOrderBy(x => x.OrderId);
            AddOrderBy(x => x.OrderItemId);
        }

         public SelectionDecisionSpecs(int orderItemId) 
            : base(x => x.OrderItemId == orderItemId)
        {
        }

        public SelectionDecisionSpecs(int orderid, int dummy) 
            : base(x => x.OrderId == orderid)
        {
            AddOrderBy(x => x.OrderItemId);
        }

        public SelectionDecisionSpecs(string candidateId) 
            : base(x => x.CandidateId == Convert.ToInt32(candidateId))
        {
        }
        
        public SelectionDecisionSpecs(string applicationNo, string dummy) 
            : base(x => x.ApplicationNo == Convert.ToInt32(applicationNo))
        {
        }
        
      }
}