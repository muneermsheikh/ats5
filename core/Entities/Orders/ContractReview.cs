using System;
using System.Collections.Generic;
using core.Entities.MasterEntities;

namespace core.Entities.Orders
{
    public class ContractReview: BaseEntity
    {
        public ContractReview()
        {
        }

        public ContractReview(int enquiryId, int reviewedBy, int reviewStatusId)
        {
            OrderId = enquiryId;
            ReviewedBy = reviewedBy;
            ReviewedOn = DateTime.Now;
            ReviewStatusId = reviewStatusId;
        }

        public ContractReview(int enquiryId, int enquiryNo, DateTime enquiryDate, 
            int customerId, string customerName)
        {
            OrderId = enquiryId;
            OrderNo = enquiryNo;
            OrderDate = enquiryDate;
            CustomerId = customerId;
            CustomerName = customerName;
        }

            public ContractReview(int enquiryId, int enquiryNo, DateTime enquiryDate, 
            int customerId, string customerName, ICollection<ContractReviewItem> reviewItems, int statusId)
        {
            OrderId = enquiryId;
            OrderNo = enquiryNo;
            OrderDate = enquiryDate;
            CustomerId = customerId;
            CustomerName = customerName;
            ContractReviewItems=reviewItems;
            ReviewStatusId = statusId;
        }

        public int OrderId { get; set; }
        public int OrderNo {get; set; }
        public DateTime OrderDate {get; set;}
        public int CustomerId {get; set;}
        public string CustomerName {get; set;}
        public int? ReviewedBy { get; set; }
        public DateTime? ReviewedOn { get; set; } = DateTime.Now;
        public ICollection<ContractReviewItem> ContractReviewItems {get; set; }
        public int ReviewStatusId { get; set; } = 0;    //not reviewed
        public ReviewStatus ReviewStatus {get; set;}
    }
}