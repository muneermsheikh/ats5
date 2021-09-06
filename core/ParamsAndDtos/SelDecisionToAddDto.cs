using System;

namespace core.ParamsAndDtos
{
    public class SelDecisionToAddDto
    {
        public int CVRefId { get; set; }
        public DateTime DecisionDate { get; set; }
        public int SelectionStatusId { get; set; }
        public string Remarks { get; set; }
        
        public int Charges { get; set; }
        public string SalaryCurrency { get; set; }
        public int Salary { get; set; }
        public int ContractPeriodInMonths { get; set; }
        public bool HousingProvidedFree { get; set; }
        public int HousingAllowance { get; set; }
        public bool FoodProvidedFree { get; set; }
        public int FoodAllowance { get; set; }
        public bool TransportProvidedFree { get; set; }
        public int TransportAllowance { get; set; }
        public int OtherAllowance { get; set; }
        public int LeavePerYearInDays { get; set; }
        public int LeaveAirfareEntitlementAfterMonths {get; set;}
    }
}