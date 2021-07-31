using System;
using System.Linq;
using core.Entities;
using core.Entities.Admin;
using core.Entities.Users;
using core.Params;
using core.ParamsAndDtos;

namespace core.Specifications
{
    public class EmployeeSpecs: BaseSpecification<Employee>
    {
        public EmployeeSpecs(EmployeeSpecParams empParams)
            : base(x => 
                (string.IsNullOrEmpty(empParams.Search) || 
                  (x.FirstName.ToLower().ToLower()
                  + x.FamilyName.ToLower()).Contains(empParams.Search.ToLower())) &&
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
            if(empParams.IncludeHRSkills) AddInclude(x => x.HrSkills);
            if(empParams.IncludeOtherSkills) AddInclude(x => x.OtherSkills);
            if(empParams.IncludePhones) AddInclude(x => x.UserPhones);
            if (empParams.IncludeQualifications) AddInclude(x => x.Qualifications);

            ApplyPaging(empParams.PageIndex * (empParams.PageSize - 1), empParams.PageSize);

            if (!string.IsNullOrEmpty(empParams.Sort))
            {
                switch (empParams.Sort)
                {
                    case "CityAsc": AddOrderBy(x => x.City); break;
                    case "CityDesc": AddOrderByDescending(x => x.City); break;
                    case "DepartmentAsc": AddOrderBy(x => x.Department); break;
                    case "DepartmentDesc": AddOrderByDescending(x => x.Department); break;
                    case "StatusAsc": AddOrderBy(x => x.Status); break;
                    case "StatusDesc": AddOrderByDescending(x => x.Status); break;
                    case "IndustryAsc": AddOrderBy(x => x.HrSkills.Select(x => x.IndustryId)); break;
                    case "IndustryDesc": AddOrderByDescending(x => x.HrSkills.Select(x => x.IndustryId)); break;
                    default: break;
                }   
                AddOrderBy(x => x.FirstName);
            }
        }

        public EmployeeSpecs(int id) 
            : base(x => x.Id == id)
        {
        }
  
    }
}