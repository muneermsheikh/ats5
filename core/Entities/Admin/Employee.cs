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

          public Employee(string gender, string firstName, string secondName, string familyName, 
               string knownAs, DateTime dob, string aadharNo, ICollection<EmployeeQualification> qualifications, 
               DateTime dOJ, string department, ICollection<EmployeeHRSkill> hrSkills, string password,
               ICollection<EmployeeOtherSkill> otherSkills, string add, string city)
          {
               Gender=gender; FirstName = firstName; SecondName=secondName; FamilyName=familyName;
               KnownAs=knownAs; DOB= dob; AadharNo=aadharNo; Qualifications = qualifications;
               DOJ = dOJ; Department = department; HrSkills = hrSkills; OtherSkills = otherSkills;
               Add = add; City=city; Password = password;
          }

          public Employee(string gender, string firstName, string secondName, string familyName, 
               string knownAs, DateTime dob, string aadharNo, string appUserId, string add, 
               string city, DateTime doj, string dept, string password, ICollection<EmployeePhone> userphones,  
               ICollection<EmployeeQualification> qualifications,  
               ICollection<EmployeeHRSkill> employeeHRSkills, ICollection<EmployeeOtherSkill> empOtherSkills)
          {
               Gender=gender; FirstName = firstName; SecondName=secondName; FamilyName=familyName;
               KnownAs=knownAs; DOB= dob; AppUserId = appUserId; AadharNo = aadharNo; Qualifications = qualifications;
               HrSkills = employeeHRSkills; OtherSkills = empOtherSkills; Add = add; City = city;
               UserPhones = userphones; DOJ = doj; Department = dept; Password = password;
          }

        public string AppUserId { get; set; }
        public string Gender { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FamilyName { get; set; }
        [Required]         
        public string KnownAs { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        public string PlaceOfBirth { get; set; }
        public string AadharNo { get; set; }
        public string Nationality {get; set;}
        [EmailAddress]
        public string Email {get; set;}
        public DateTime DOJ {get; set;}
        public string Department { get; set; }
        public DateTime? LastWorkingDay {get; set;}
        public EnumEmployeeStatus Status { get; set; } = EnumEmployeeStatus.Employed;
        public string Remarks { get; set; }        
        public DateTime Created {get; set;}=DateTime.Now;
        public DateTime? LastActive {get; set;}
        public string Password {get; set;}
        public ICollection<EmployeeQualification> Qualifications {get; set;}
        public ICollection<EmployeeHRSkill> HrSkills {get; set;}
        public ICollection<EmployeeOtherSkill> OtherSkills{get; set;}
        public ICollection<EmployeePhone> UserPhones {get; set;}
        public string Add { get; set; }
        public string Address2 {get; set;}
        public string City { get; set; }
        public string Country {get; set;}
    }
}