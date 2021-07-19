using System;
using System.Collections.Generic;


namespace core.Entities.Admin
{
     public class Employee: BaseEntity
    {
          public Employee()
          {
          }

          public Employee(Person person, ICollection<EmployeeQualification> qualifications, 
            DateTime dOJ, string department, ICollection<EmployeeHRSkill> hrSkills, 
            ICollection<EmployeeOtherSkill> otherSkills, string add, string city)
          {
               Person = person;
               Qualifications = qualifications;
               DOJ = dOJ;
               Department = department;
               HrSkills = hrSkills;
               OtherSkills = otherSkills;
               Add = add;
               City=city;
          }

        public string AppUserId { get; set; }
        public Person Person {get; set;}
        public DateTime DOJ {get; set;}
        public string Department { get; set; }
        public DateTime? LastWorkingDay {get; set;}
        public EnumEmployeeStatus Status { get; set; } = EnumEmployeeStatus.Employed;
        public string Remarks { get; set; }        
        public ICollection<EmployeeQualification> Qualifications {get; set;}
        public ICollection<EmployeeHRSkill> HrSkills {get; set;}
        public ICollection<EmployeeOtherSkill> OtherSkills{get; set;}
        public string Add { get; set; }
        public string City { get; set; }
    }
}