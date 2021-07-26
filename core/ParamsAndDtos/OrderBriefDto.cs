using System;
using System.Collections.Generic;

namespace core.ParamsAndDtos
{
    public class OrderBriefDto
    {
        public OrderBriefDto(int id, int contractReviewId, int orderNo, DateTime orderDate, int customerId, string customerCustomerName, 
            DateTime completeBy, int reviewStatusId, string reviewStatus)
        {
            Id = id;
            ContractReviewId = contractReviewId;
            OrderNo = orderNo;
            OrderDate = orderDate;
            CustomerCustomerName = customerCustomerName;
            CompleteBy = completeBy;
            CustomerId = customerId;
            ReviewStatusId = reviewStatusId;
            ReviewStatus = reviewStatus;
        }

        public int Id { get; set; }
        public int ContractReviewId { get; set; }
        public int OrderNo { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCustomerName { get; set; }
        public DateTime CompleteBy { get; set; }
        public int ReviewStatusId { get; set; }
        public string ReviewStatus { get; set; }
        public int ReviewedBy { get; set; }
        public DateTime ReviewedOn { get; set; }
        public ICollection<ContractReviewItemDto> OrderReviewItems {get; set;}

    }
}