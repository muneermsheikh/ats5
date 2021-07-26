using System;
using System.Linq;
using core.Entities;
using core.Entities.Admin;
using core.Entities.Users;
using core.ParamsAndDtos;

namespace core.Specifications
{
    public class EmployeeForCountSpecs: BaseSpecification<Employee>
    {
        public EmployeeForCountSpecs(EmployeeSpecParams empParams)
            : base(x => 
                (string.IsNullOrEmpty(empParams.Search) || 
                  (x.Person.FirstName.ToLower().ToLower()
                  + x.Person.FamilyName.ToLower()).Contains(empParams.Search.ToLower())) &&
                (string.IsNullOrEmpty(empParams.City) ||
                    x.City.ToLower() == empParams.City) &&
                (!empParams.EmployeeId.HasValue ||
                    x.Id == empParams.EmployeeId) &&
                (string.IsNullOrEmpty(empParams.Department) ||
                    x.Department.ToLower() == empParams.Department.ToLower()) &&
                (!empParams.Status.HasValue ||
                    x.Status == empParams.Status) &&
                (!empParams.IndustryId.HasValue || 
                    x.HrSkills.Select(s => s.IndustryId).ToList().Contains((int)empParams.IndustryId)) &&
                (!empParams.CategoryId.HasValue || 
                    x.HrSkills.Select(s => s.CategoryId).ToList().Contains((int)empParams.CategoryId)) &&
                (!empParams.SkillDataId.HasValue || 
                    x.OtherSkills.Select(s => s.SkillDataId).ToList().Contains((int)empParams.SkillDataId)) &&
                (!empParams.OtherSkillLevel.HasValue || 
                    x.OtherSkills.Select(s => s.SkillLevel).ToList().Contains((int)empParams.OtherSkillLevel)) &&
                (string.IsNullOrEmpty(empParams.PhoneNo) || 
                    x.UserPhones.Select(x => x.PhoneNo).ToList().Contains(empParams.PhoneNo) ||
                    x.UserPhones.Select(x => x.MobileNo).ToList().Contains(empParams.PhoneNo))
                )
        {
        }

        public EmployeeForCountSpecs(int id) 
            : base(x => x.Id == id)
        {
        }
  
    }
}