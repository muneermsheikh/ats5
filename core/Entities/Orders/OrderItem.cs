using System;
using System.Collections.Generic;
using core.Entities.Admin;
using core.Entities.HR;

namespace core.Entities.Orders
{
    public class OrderItem: BaseEntity
    {
          public OrderItem()
          {
          }
          public OrderItem(int srNo, int categoryId, string categoryName, int industryId, string industryName, 
            string sourceFrom, int quantity, int minCVs, int maxCVs, bool ecnr, bool requireAssess, DateTime completeBefore, 
            int charges)
          {
               SrNo = srNo;
               CategoryId = categoryId;
               CategoryName = categoryName;
               IndustryId = industryId;
               IndustryName = industryName;
               SourceFrom = sourceFrom;
               Quantity = quantity;
               MinCVs = minCVs;
               MaxCVs = maxCVs;
               Ecnr = ecnr;
               RequireAssess = requireAssess;
               CompleteBefore = completeBefore;
               Charges = charges;
          }

          public OrderItem(int orderId, int srNo, int categoryId, string categoryName, int industryId, string industryName, 
            string sourceFrom, int quantity, int minCVs, int maxCVs, bool ecnr, bool requireAssess, DateTime completeBefore, 
            int charges)
          {
               OrderId = orderId;
               SrNo = srNo;
               CategoryId = categoryId;
               CategoryName = categoryName;
               IndustryId = industryId;
               IndustryName = industryName;
               SourceFrom = sourceFrom;
               Quantity = quantity;
               MinCVs = minCVs;
               MaxCVs = maxCVs;
               Ecnr = ecnr;
               RequireAssess = requireAssess;
               CompleteBefore = completeBefore;
               Charges = charges;
          }

        public int OrderId { get; set; }
        public int SrNo { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int IndustryId { get; set; }
        public string IndustryName { get; set; }
        public string SourceFrom { get; set; }
        public int Quantity { get; set; }
        public int MinCVs { get; set; }
        public int MaxCVs { get; set; }
        public bool Ecnr { get; set; }=false;
        public bool RequireAssess { get; set; }=false;
        public DateTime CompleteBefore { get; set; }
        public int? HrExecId { get; set; }
        public string HRExecName { get; set; }
        public int? HrSupId { get; set; }
        public string HrSupName { get; set; }
        public int? HrmId { get; set; }
        public string HrmName { get; set; }
        public int? AssignedId { get; set; }
        public string AssignedToName { get; set; }
        public int Charges { get; set; }
        public int FeeFromClientINR {get; set;}
        public EnumOrderItemStatus Status { get; set; }=EnumOrderItemStatus.AwaitingReview;
        public EnumReviewItemStatus ReviewItemStatusId { get; set; }=EnumReviewItemStatus.NotReviewed;
        public virtual Employee Assigned { get; set; }
        public Category Category { get; set; }
        public JobDescription JobDescription { get; set; }
        public Remuneration Remuneration { get; set; }
        public ContractReviewItem ContractReviewItem { get; set; }
        //public Order Order { get; set; }
        public ICollection<CVRef> CVRefs { get; set; }

    }
}