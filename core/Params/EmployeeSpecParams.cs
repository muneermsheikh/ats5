using System;
using core.Entities.Admin;

namespace core.Params
{
    public class EmployeeSpecParams: ParamPages
    {
          public EmployeeSpecParams()
          {
          }

        public int? CVRefid {get; set;}
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
        public bool IncludeQualifications { get; set; }
        public bool IncludeHRSkills { get; set; }
        public bool IncludeOtherSkills {get; set;}
        public bool IncludePhones {get; set;}
    }
}