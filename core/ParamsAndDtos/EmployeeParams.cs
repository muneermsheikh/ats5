
using System;
using core.Entities.Admin;

namespace core.ParamsAndDtos
{
    public class EmployeeParams: ParamPages
    {
        public int? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string FamilyName {get; set;}
        public string Department {get; set;}
        public DateTime? DOJ {get; set;}
        public EnumEmployeeStatus Status {get; set;}
        public EmployeeHRSkill HRSkill {get; set;}
        public EmployeeOtherSkill OtherSkill {get; set;}
        public string City {get; set;}
        public string PhoneNo {get; set;}
        
    }
}