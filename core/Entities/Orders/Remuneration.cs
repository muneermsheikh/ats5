using System.ComponentModel.DataAnnotations;

namespace core.Entities.Orders
{
    public class Remuneration
    {
          public Remuneration()
          {
          }

          public Remuneration(string salaryCurrency, int salaryMin, int salaryMax, 
            int contractPeriodInMonths, bool housingProvidedFree, int housingAllowance, 
            bool foodProvidedFree, int foodAllowance, bool transportProvidedFree, 
            int transportAllowance, int otherAllowance, int leavePerYearInDays, 
            int leaveAirfareEntitlementAfterMonths)
          {
               SalaryCurrency = salaryCurrency;
               SalaryMin = salaryMin;
               SalaryMax = salaryMax;
               ContractPeriodInMonths = contractPeriodInMonths;
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

        [Required]
        [MinLength(3), MaxLength(3)]
        public string SalaryCurrency { get; set; }
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
        
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
        public int LeaveAirfareEntitlementAfterMonths { get; set; }
        
    }
}