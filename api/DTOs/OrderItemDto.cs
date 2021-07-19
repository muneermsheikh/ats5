using System;
using System.Collections.Generic;
using core.Entities.Orders;

namespace api.DTOs
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int SrNo { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public int MinCVs { get; set; }
        public bool Ecnr { get; set; }
        public bool RequireAssess { get; set; }
        public DateTime CompleteBefore { get; set; }
        public int? HrExecId { get; set; }
        public int? HrSupId { get; set; }
        public int? HrmId { get; set; }
        public int? AssignedId { get; set; }
        public int Charges { get; set; }
        public int FeeFromClientINR {get; set;}
        public string Status { get; set; }
        public JobDescription JobDescription { get; set; }
        public Remuneration Remuneration { get; set; }
        public ContractReviewItem ContractReviewItem { get; set; }
    }
}