using System;
using System.Linq;
using core.Entities;
using core.Entities.Orders;
using core.Entities.Users;
using core.ParamsAndDtos;

namespace core.Specifications
{
    public class ContractReviewSpecs: BaseSpecification<ContractReviewItem>
    {
        public ContractReviewSpecs(ContractReviewSpecParams cParams)
            : base(x => 
                (!cParams.OrderId.HasValue || 
                  x.OrderId == cParams.OrderId) &&
                (!cParams.OrderItemId.HasValue ||
                    x.OrderItemId == cParams.OrderItemId)
                )
        {
            AddOrderBy(x => x.OrderItemId);
        }

        public ContractReviewSpecs(int orderItemId) 
            : base(x => x.OrderItemId == orderItemId)
        {
        }
        public ContractReviewSpecs(int orderid, int dummy) 
            : base(x => x.OrderId == orderid)
        {
            AddOrderBy(x => x.OrderItemId);
        }
        
      }
}