using System;
using System.Linq;
using core.Entities;
using core.Entities.HR;
using core.Entities.Orders;
using core.Entities.Users;
using core.ParamsAndDtos;

namespace core.Specifications
{
    public class SelectionDecisionForCountSpecs: BaseSpecification<SelectionDecision>
    {
        public SelectionDecisionForCountSpecs(SelDecisionSpecParams specParams)
            : base(x => 
                (!specParams.OrderId.HasValue || x.OrderId == specParams.OrderId) &&
                (!specParams.OrderNo.HasValue || x.OrderNo == specParams.OrderNo) &&
                (!specParams.OrderItemId.HasValue || x.OrderItemId == specParams.OrderItemId) &&
                (!specParams.CategoryId.HasValue || x.CategoryId == specParams.CategoryId) &&
                (string.IsNullOrEmpty(specParams.CategoryName) || x.CategoryName == specParams.CategoryName) &&
                (!specParams.CVRefId.HasValue || x.CVRefId == specParams.CVRefId) &&
                (!specParams.CandidateId.HasValue || x.CandidateId == specParams.CandidateId) &&
                (!specParams.ApplicationNo.HasValue ||  x.ApplicationNo == specParams.ApplicationNo) &&
                (specParams.CVRefIds.Length==0 || specParams.CVRefIds.Contains(x.CVRefId))
                )
        {
        }

        public SelectionDecisionForCountSpecs(int orderItemId) 
            : base(x => x.OrderItemId == orderItemId)
        {
        }

        public SelectionDecisionForCountSpecs(int orderid, int dummy) 
            : base(x => x.OrderId == orderid)
        {
        }

        public SelectionDecisionForCountSpecs(string candidateId) 
            : base(x => x.CandidateId == Convert.ToInt32(candidateId))
        {
        }
        
        public SelectionDecisionForCountSpecs(string applicationNo, string dummy) 
            : base(x => x.ApplicationNo == Convert.ToInt32(applicationNo))
        {
        }
        
      }
}