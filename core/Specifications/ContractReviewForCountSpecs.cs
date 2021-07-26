using System;
using System.Linq;
using core.Entities;
using core.Entities.Orders;
using core.Entities.Users;
using core.ParamsAndDtos;

namespace core.Specifications
{
    public class ContractReviewForCountSpecs: BaseSpecification<ContractReviewItem>
    {
        public ContractReviewForCountSpecs(ContractReviewSpecParams cParams)
            : base(x => 
                (!cParams.OrderId.HasValue || 
                  x.OrderId == cParams.OrderId) &&
                (!cParams.OrderItemId.HasValue ||
                    x.OrderItemId == cParams.OrderItemId)
                )
        {
        }

        public ContractReviewForCountSpecs(int orderItemId) 
            : base(x => x.OrderItemId == orderItemId)
        {
        }

        public ContractReviewForCountSpecs(int orderid, int dummy) 
            : base(x => x.OrderId == orderid)
        {
        }
        
      }
}