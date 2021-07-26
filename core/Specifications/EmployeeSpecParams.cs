using System;
using core.Entities.Admin;

namespace core.Specifications
{
    public class EmployeeSpecParams: CommonSpecParams
    {
        public int? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string FamilyName {get; set;}
        public string Department {get; set;}
        public DateTime? DOJ {get; set;}
        public EnumEmployeeStatus? Status {get; set;}
        public int? IndustryId {get; set;}
        public int? CategoryId {get; set;}
        public int? SkillLevel {get; set;}
        public int? SkillDataId {get; set;}
        public int? OtherSkillLevel {get; set;}

        public string City {get; set;}
        public string PhoneNo {get; set;}
    }
}