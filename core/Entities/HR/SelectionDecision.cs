using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using core.Entities.Orders;

namespace core.Entities.HR
{
    public class SelectionDecision: BaseEntity
    {
          public SelectionDecision()
          {
          }

        public SelectionDecision(int cVRefId, int orderItemId, int categoryId, string categoryName, int orderId, 
              int orderNo, string customerName, int applicationNo, int candidateId, string candidateName, DateTime decisionDate, 
              int selectionStatusId, string remarks)
           {
               CVRefId = cVRefId;
               OrderItemId = orderItemId;
               CategoryId = categoryId;
               CategoryName = categoryName;
               OrderId = orderId;
               OrderNo = orderNo;
               CustomerName = customerName;
               ApplicationNo = applicationNo;
               CandidateId = candidateId;
               CandidateName = candidateName;
               DecisionDate = decisionDate;
               SelectionStatusId = selectionStatusId;
               Remarks = remarks;
          }

          public SelectionDecision(int cVRefId, int orderItemId, int categoryId, string categoryName, int orderId, 
              int orderNo, string customerName, int applicationNo, int candidateId, string candidateName, DateTime decisionDate, 
              int selectionStatusId, string remarks, int charges,  string salaryCurrency, int salary, 
              int contractPeriodInMonths, bool housingProvidedFree, int housingAllowance, bool foodProvidedFree, 
              int foodAllowance, bool transportProvidedFree, int transportAllowance, int otherAllowance,
              int leavePerYearInDays,int leaveAirfareEntitlementAfterMonths)
 
          {
               CVRefId = cVRefId;
               OrderItemId = orderItemId;
               CategoryId = categoryId;
               CategoryName = categoryName;
               OrderId = orderId;
               OrderNo = orderNo;
               CustomerName = customerName;
               ApplicationNo = applicationNo;
               CandidateId = candidateId;
               CandidateName = candidateName;
               DecisionDate = decisionDate;
               SelectionStatusId = selectionStatusId;
               //Employment = employment;
               Remarks = remarks;
               //SelectedOn= selectedOn;
               Charges = charges;
               SalaryCurrency = salaryCurrency;
               Salary = salary;
              ContractPeriodInMonths= contractPeriodInMonths;
              HousingProvidedFree = housingProvidedFree;
              HousingAllowance = housingAllowance;
              FoodProvidedFree = foodProvidedFree;
              FoodAllowance = foodAllowance;
              TransportProvidedFree = transportProvidedFree;
              TransportAllowance = transportAllowance;
              OtherAllowance = otherAllowance;
              LeavePerYearInDays = leavePerYearInDays;
              LeaveAirfareEntitlementAfterMonths = leaveAirfareEntitlementAfterMonths;
          }

          public int CVRefId { get; set; }
          //public int? EmploymentId {get; set;}
          public int OrderItemId { get; set; }
          public int CategoryId { get; set; }
          public string CategoryName {get; set;}
          public int OrderId { get; set; }
          public int OrderNo {get; set;}
          public string CustomerName {get; set;}
          public int ApplicationNo {get; set;}
          public int CandidateId {get; set;}
          public string CandidateName {get; set;}
          public DateTime DecisionDate {get; set;}
          public int SelectionStatusId { get; set; }
          [ForeignKey("CVRefId")]
          public virtual CVRef CVRef {get; set;}
        //[ForeignKey("EmploymentId")]
        //public Employment Employment {get; set;}
          public DateTime SelectedOn { get; set; }
          public int Charges {get; set;}
          public string SalaryCurrency { get; set; }
          public int Salary { get; set; }
          [Range(1,36)]
          public int ContractPeriodInMonths { get; set; }=24;
          public bool HousingProvidedFree { get; set; }
          public int HousingAllowance { get; set; }
          public bool FoodProvidedFree { get; set; }
          public int FoodAllowance { get; set; }
          public bool TransportProvidedFree { get; set; }
          public int TransportAllowance { get; set; }
          public int OtherAllowance { get; set; }
          public int LeavePerYearInDays { get; set; }
          public int LeaveAirfareEntitlementAfterMonths {get; set;}
          public string Remarks {get; set;}
    }
}