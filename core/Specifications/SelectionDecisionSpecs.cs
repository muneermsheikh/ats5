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
        public SelectionDecisionSpecs(SelDecisionParams cParams)
            : base(x => 
                (!cParams.OrderId.HasValue || x.OrderId == cParams.OrderId) &&
                (!cParams.OrderNo.HasValue || x.OrderNo == cParams.OrderNo) &&
                (!cParams.OrderItemId.HasValue || x.OrderItemId == cParams.OrderItemId) &&
                (!cParams.CategoryId.HasValue || x.CategoryId == cParams.CategoryId) &&
                (string.IsNullOrEmpty(cParams.CategoryName) || x.CategoryName == cParams.CategoryName) &&
                (!cParams.CVRefId.HasValue || x.CVRefId == cParams.CVRefId) &&
                (!cParams.CandidateId.HasValue || x.CandidateId == cParams.CandidateId) &&
                (!cParams.ApplicationNo.HasValue ||  x.ApplicationNo == cParams.ApplicationNo) 
                )
        {
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