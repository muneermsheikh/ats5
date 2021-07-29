using System;
using System.Linq;
using core.Entities;
using core.Entities.HR;
using core.Entities.Orders;
using core.Entities.Users;
using core.ParamsAndDtos;

namespace core.Specifications
{
    public class SelectionsPendingSpecs: BaseSpecification<CVRef>
    {
        public SelectionsPendingSpecs(SelectionsPendingParams cParams)
            : base(x => 
                (!cParams.OrderId.HasValue || x.OrderId == cParams.OrderId) &&
                (!cParams.OrderNo.HasValue || x.OrderNo == cParams.OrderNo) &&
                (!cParams.OrderItemId.HasValue || x.OrderItemId == cParams.OrderId) &&
                (!cParams.CategoryId.HasValue || x.CategoryId == cParams.CategoryId) &&
                (string.IsNullOrEmpty(cParams.CategoryName) || x.CategoryName == cParams.CategoryName) &&
                (!cParams.CVRefId.HasValue || x.Id == cParams.CVRefId) &&
                (!cParams.CandidateId.HasValue || x.CandidateId == cParams.CandidateId) &&
                (!cParams.ApplicationNo.HasValue ||  x.ApplicationNo == cParams.ApplicationNo)  
                )
        {
            AddOrderBy(x => x.OrderId);
            AddOrderBy(x => x.OrderItemId);
        }

        public SelectionsPendingSpecs(int orderItemId) 
            : base(x => x.OrderItemId == orderItemId)
        {
        }

        public SelectionsPendingSpecs(int orderid, int dummy) 
            : base(x => x.OrderId == orderid)
        {
            AddOrderBy(x => x.OrderItemId);
        }

        public SelectionsPendingSpecs(string candidateId) 
            : base(x => x.CandidateId == Convert.ToInt32(candidateId))
        {
        }
        
        public SelectionsPendingSpecs(string applicationNo, string dummy) 
            : base(x => x.ApplicationNo == Convert.ToInt32(applicationNo))
        {
        }
        
      }
}