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

          public OrderItem(int orderId, int srNo, int categoryId, int industryId, string sourceFrom, 
            int quantity, int minCVs, int maxCVs, bool ecnr, bool requireAssess, DateTime completeBefore, 
            int charges)
          {
               OrderId = orderId;
               SrNo = srNo;
               CategoryId = categoryId;
               IndustryId = industryId;
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
        public int IndustryId { get; set; }
        public string SourceFrom { get; set; }
        public int Quantity { get; set; }
        public int MinCVs { get; set; }
        public int MaxCVs { get; set; }
        public bool Ecnr { get; set; }=false;
        public bool RequireAssess { get; set; }=false;
        public DateTime CompleteBefore { get; set; }
        public int? HrExecId { get; set; }
        public int? HrSupId { get; set; }
        public int? HrmId { get; set; }
        public int? AssignedId { get; set; }
        public int Charges { get; set; }
        public int FeeFromClientINR {get; set;}
        public EnumOrderItemStatus Status { get; set; }=EnumOrderItemStatus.AwaitingReview;
        public int ReviewItemStatusId { get; set; }
        public Employee Assigned { get; set; }
        public Category Category { get; set; }
        public JobDescription JobDescription { get; set; }
        public Remuneration Remuneration { get; set; }
        public ContractReviewItem ContractReviewItem { get; set; }
        //public Order Order { get; set; }
        public ICollection<CVRef> CVRefs { get; set; }

    }
}