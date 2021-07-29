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
        public SelectionDecisionForCountSpecs(SelDecisionParams cParams)
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