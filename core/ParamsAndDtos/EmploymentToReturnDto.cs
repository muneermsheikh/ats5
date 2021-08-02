using System;

namespace core.ParamsAndDtos
{
    public class EmploymentToReturnDto
    {
        //public int CVRefId { get; set; }
        //public int SelectionDecisionId {get; set;}
        //public DateTime SelectedOn { get; set; }
        public int Charges {get; set;}
        public int ContractPeriodInMonths { get; set; }
        public string SalaryCurrency { get; set; }
        public int Salary { get; set; }
        public bool HousingProvidedFree { get; set; }
        public int HousingAllowance { get; set; }
        public bool FoodProvidedFree { get; set; }
        public int FoodAllowance { get; set; }
        public bool TransportProvidedFree { get; set; }
        public int TransportAllowance { get; set; }
        public int OtherAllowance { get; set; }
        public int LeavePerYearInDays { get; set; }
        public int LeaveAirfareEntitlementAfterMonths {get; set;}
        public DateTime OfferAcceptedOn { get; set; }
        public string OfferAttachmentUrl { get; set; }
        public string OfferAcceptanceUrl { get; set; }

    }
}