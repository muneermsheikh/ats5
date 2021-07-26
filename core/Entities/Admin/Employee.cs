using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using core.Entities.Users;

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

          public Employee(string appUserId, Person person, string aadharNo, string add, string city, DateTime doj, string dept,   
               ICollection<UserPhone> userphones,  ICollection<EmployeeQualification> qualifications,  
               ICollection<EmployeeHRSkill> employeeHRSkills, ICollection<EmployeeOtherSkill> empOtherSkills)
          {
               AppUserId = appUserId;
               AadharNo = aadharNo;
               Person = person;
               Qualifications = qualifications;
               HrSkills = employeeHRSkills;
               OtherSkills = empOtherSkills;
               Add = add;
               City = city;
               UserPhones = userphones;
               DOJ = doj;
               Department = dept;
          }

        public string AppUserId { get; set; }
        public Person Person {get; set;}
        public DateTime DOJ {get; set;}
        public string Department { get; set; }
        public DateTime? LastWorkingDay {get; set;}
        public EnumEmployeeStatus Status { get; set; } = EnumEmployeeStatus.Employed;
        public string Remarks { get; set; }        
        [Required, MaxLength(12), MinLength(12)]
        public string AadharNo { get; set; }
        public DateTime Created {get; set;}
        public DateTime LastActive {get; set;}
        public ICollection<EmployeeQualification> Qualifications {get; set;}
        public ICollection<EmployeeHRSkill> HrSkills {get; set;}
        public ICollection<EmployeeOtherSkill> OtherSkills{get; set;}
        public ICollection<UserPhone> UserPhones {get; set;}
        public string Add { get; set; }
        public string City { get; set; }
    }
}