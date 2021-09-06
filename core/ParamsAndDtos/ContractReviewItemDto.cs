using System;
using System.Collections.Generic;
using core.Entities.Orders;

namespace core.ParamsAndDtos
{
    public class ContractReviewItemDto
    {
          public ContractReviewItemDto()
          {
          }

          public ContractReviewItemDto(int id, int contractReviewId, int orderId, int orderItemId,  int srNo, string professionName,  
            string sourceFrom, int quantity, bool ecnr, bool requireAssess, DateTime completeBefore, 
            ICollection<ReviewItem> reviewIems, int reviewItemStatusId)
        {
            Id = id;
            ContractReviewId = contractReviewId;
            OrderId = orderId;
            OrderItemId = orderItemId;
            SrNo = srNo;
            ProfessionName = professionName;
            Quantity = quantity;
            Ecnr = ecnr;
            RequireAssess = requireAssess;
            CompleteBefore = completeBefore;
            ReviewItems = reviewIems;
            /* 
            TechnicallyFeasible = technicallyFeasible;
            CommerciallyFeasible = commerciallyFeasible;
            LogisticallyFeasible = logisticallyFeasible;
            VisaAvailable = visaAvailable;
            DocumentationWillBeAvailable = documentationWillBeAvailable;
            HistoricalStatusAvailable = historicalStatusAvailable;
            SalaryOfferedFeasible = salaryOfferedFeasible;
            ServiceChargesInINR = serviceChargesInINR;
            FeeFromClientCurrency = feeFromClientCurrency;
            FeeFromClient = feeFromClient;
            ReviewItemStatusId = reviewItemStatusId;
            */
        }
 
        public int Id { get; set; }
        public int SrNo { get; set; }
        public int ContractReviewId {get; set;}
        public int OrderId { get; set; }
        public int OrderNo { get; set; }
        public int OrderItemId { get; set; }
        public string ProfessionName {get; set;}
        public string SourceFrom { get; set; }
        public bool RequireAssess { get; set; }
        public bool Ecnr {get; set;}
        public int Quantity {get; set; }
        public DateTime CompleteBefore { get; set; }
        public ICollection<ReviewItem> ReviewItems {get; set;}
        /*
        public bool? TechnicallyFeasible { get; set; }
        public bool? CommerciallyFeasible { get; set; }
        public bool? LogisticallyFeasible { get; set; }
        public bool? VisaAvailable { get; set; }
        public bool? DocumentationWillBeAvailable { get; set; }
        public bool? HistoricalStatusAvailable { get; set; }
        public bool? SalaryOfferedFeasible { get; set; }
        public string ServiceChargesInINR { get; set; }
        public string FeeFromClientCurrency { get; set;}
        public int? FeeFromClient { get; set; }
        public int ReviewItemStatusId { get; set; }
        public Remuneration Remuneration {get; set;}
        */
    }
}