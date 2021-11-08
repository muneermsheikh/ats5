using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using core.Entities.Admin;
using core.Entities.Identity;
using core.Entities.Users;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class EmployeeService : IEmployeeService
     {
          private readonly ATSContext _context;
          private readonly IUnitOfWork _unitOfWork;
          private readonly UserManager<AppUser> _userManager;
          private readonly IMapper _mapper;
          public EmployeeService(ATSContext context, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper)
          {
               _mapper = mapper;
               _userManager = userManager;
               _unitOfWork = unitOfWork;
               _context = context;
          }

          public async Task<bool> DeleteEmployee(Employee employee)
          {
               _unitOfWork.Repository<Employee>().Delete(employee);
               return await _unitOfWork.Complete() > 0;
          }

          public async Task<bool> EditEmployee(Employee emp)
          {
               //thanks to @slauma of stackoverflow
               var existingEmp = _context.Employees
                   .Where(p => p.Id == emp.Id)
                   .AsNoTracking()
                   .SingleOrDefault();

               if (existingEmp == null) return false;

               //ignore any changes to AppUserId that the client might ahve made
               emp.AppUserId = existingEmp.AppUserId;      //this cannot be changed by the client

               _context.Entry(existingEmp).CurrentValues.SetValues(emp);   //saves only the parent, not children

               //Delete children that exist in existing record, but not in the new model order
               foreach (var existingPh in existingEmp.UserPhones.ToList())
               {
                    if (!emp.UserPhones.Any(c => c.Id == existingPh.Id && c.Id != default(int)))
                    {
                         _context.EmployeePhones.Remove(existingPh);
                         _context.Entry(existingPh).State = EntityState.Deleted;
                    }
               }

               foreach (var existingQ in existingEmp.Qualifications.ToList())
               {
                    if (!emp.Qualifications.Any(c => c.Id == existingQ.Id && c.Id != default(int)))
                    {
                         _context.EmployeeQualifications.Remove(existingQ);
                         _context.Entry(existingQ).State = EntityState.Deleted;
                    }
               }

               foreach (var existingSk in existingEmp.HrSkills.ToList())
               {
                    if (!emp.HrSkills.Any(c => c.Id == existingSk.Id && c.Id != default(int)))
                    {
                         _context.EmployeeHRSkills.Remove(existingSk);
                         _context.Entry(existingSk).State = EntityState.Deleted;
                    }
               }

               foreach (var existingOSk in existingEmp.OtherSkills.ToList())
               {
                    if (!emp.OtherSkills.Any(c => c.Id == existingOSk.Id && c.Id != default(int)))
                    {
                         _context.EmployeeOtherSkills.Remove(existingOSk);
                         _context.Entry(existingOSk).State = EntityState.Deleted;
                    }
               }

               //children that are not deleted, are either updated or new ones to be added
               foreach (var item in emp.UserPhones)
               {
                    var existingPh = existingEmp.UserPhones.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();

                    if (existingPh != null)       // record exists, update it
                    {
                         _context.Entry(existingPh).CurrentValues.SetValues(item);
                         _context.Entry(existingPh).State = EntityState.Modified;
                    }
                    else            //record does not exist, insert a new record
                    {
                         var newPh = new EmployeePhone(item.PhoneNo, item.MobileNo, item.IsMain);
                         existingEmp.UserPhones.Add(newPh);
                         _context.Entry(newPh).State = EntityState.Added;
                    }
               }

               foreach (var item in emp.Qualifications)
               {
                    var existingQ = existingEmp.Qualifications.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();

                    if (existingQ != null)       // record exists, update it
                    {
                         _context.Entry(existingQ).CurrentValues.SetValues(item);
                         _context.Entry(existingQ).State = EntityState.Modified;
                    }
                    else            //record does not exist, insert a new record
                    {
                         var newQ = new EmployeeQualification(item.QualificationId, item.IsMain);
                         existingEmp.Qualifications.Add(newQ);
                         _context.Entry(newQ).State = EntityState.Added;
                    }
               }

               foreach (var item in emp.HrSkills)
               {
                    var existingSk = existingEmp.HrSkills.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();

                    if (existingSk != null)       // record exists, update it
                    {
                         _context.Entry(existingSk).CurrentValues.SetValues(item);
                         _context.Entry(existingSk).State = EntityState.Modified;
                    }
                    else            //record does not exist, insert a new record
                    {
                         var newSk = new EmployeeHRSkill(item.CategoryId, item.IndustryId, item.SkillLevel);
                         existingEmp.HrSkills.Add(newSk);
                         _context.Entry(newSk).State = EntityState.Added;
                    }
               }

               foreach (var item in emp.OtherSkills)
               {
                    var existingOSk = existingEmp.OtherSkills.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();

                    if (existingOSk != null)       // record exists, update it
                    {
                         _context.Entry(existingOSk).CurrentValues.SetValues(item);
                         _context.Entry(existingOSk).State = EntityState.Modified;
                    }
                    else            //record does not exist, insert a new record
                    {
                         var newOSk = new EmployeeOtherSkill(item.SkillDataId, item.SkillLevel, item.IsMain);
                         existingEmp.OtherSkills.Add(newOSk);
                         _context.Entry(newOSk).State = EntityState.Added;
                    }
               }

               _context.Entry(existingEmp).State = EntityState.Modified;

               if (await _context.SaveChangesAsync() > 0)
               {
                    var appuser = new AppUser
                    {
                         UserType = "employee",
                         Gender = emp.Gender,
                         DisplayName = emp.KnownAs,
                         Email = emp.Email,
                         UserName = emp.Email,
                         PhoneNumber = emp.UserPhones.Select(x => x.MobileNo).FirstOrDefault(),
                         Address = new Address
                         {
                              AddressType = "R",
                              Gender = emp.Gender,
                              FirstName = emp.FirstName + " " + emp.SecondName + " " + emp.FamilyName,
                              Add = emp.Add,
                              State = emp.Address2,
                              City = emp.City,
                              Country = emp.Country
                         }
                    };

                    await _userManager.UpdateAsync(appuser);

                    return true;
               }
               else
               {
                    return false;
               }
          }

          public async Task<ICollection<Employee>> AddNewEmployees(ICollection<EmployeeToAddDto> employees)
          {

               var emps = new List<Employee>();
               var empsDto = new List<EmployeeToAddDto>();

               foreach (var employee in employees)
               {
                    var email = employee.Email;
                    if (string.IsNullOrEmpty(email)) continue;
                    if (await _userManager.FindByEmailAsync(email) != null)
                    {
                         //return BadRequest(new ApiValidationErrorResponse { Errors = new[] { "Email address " + email + " is in use" } });
                         continue;
                    }

                    if (string.IsNullOrEmpty(employee.Password))
                    {
                         //return BadRequest(new ApiResponse(400, "Password essential"));
                         continue;
                    }

                    var user = new AppUser
                    {
                         UserType = "Employee",
                         DisplayName = employee.KnownAs,
                         Address = new Address
                         {
                              AddressType = "R",
                              Gender = employee.Gender ?? "M",
                              FirstName = employee.FirstName,
                              FamilyName = employee.FamilyName,
                              Add = employee.Add + ", " + employee.Address + ", " + employee.City + " " + employee.Pin
                         },
                         Email = employee.Email,
                         UserName = employee.Email
                         //, Token = _tokenService.CreateToken
                    };
                    var result = await _userManager.CreateAsync(user, employee.Password);
                    //employee.AppUserId = user.Id;
                    employee.appUser = user;

                    var qualifications = new List<EmployeeQualification>();
                    var hrskills = new List<EmployeeHRSkill>();
                    var otherSkills = new List<EmployeeOtherSkill>();
                    var emp = new Employee(employee.Gender ?? "M", employee.FirstName, employee.SecondName, employee.FamilyName,
                         employee.KnownAs, employee.DOB, employee.AadharNo, qualifications, employee.DOJ, employee.Department, 
                         hrskills, otherSkills, employee.Add, employee.City, employee.Position ?? "Not Available");
                    //var emp = _mapper.Map<EmployeeToAddDto, Employee>(employee);
                    emp.AppUserId = user.Id;
                    emp.Email = employee.Email;
                    
                    _unitOfWork.Repository<Employee>().Add(emp);
                    emps.Add(emp);

               }

               if (emps.Count == 0) return null;
               
               if (await _unitOfWork.Complete() == 0) return null;

               return emps;               
          }

          public async Task<Pagination<Employee>> GetEmployeePaginated(EmployeeSpecParams empParams)
          {
               var spec = new EmployeeSpecs(empParams);
               var countSpec = new EmployeeForCountSpecs(empParams);

               var totalItems = await _unitOfWork.Repository<Employee>().CountAsync(countSpec);
               var emps = await _unitOfWork.Repository<Employee>().ListAsync(spec);

               //var data = _mapper.Map<IReadOnlyList<EmployeeToReturnDto>>(emps);

               return new Pagination<Employee>(empParams.PageIndex, empParams.PageSize, totalItems, emps);
          }

          public async Task<EmployeeDto> GetEmployeeFromIdAsync(int employeeId)
          {
               var emp = await _context.Employees.Where(x => x.Id == employeeId)
                    .Select(x => new
                    {
                         x.FirstName,
                         x.SecondName,
                         x.FamilyName,
                         x.KnownAs,
                         x.Position,
                         x.Email,
                         x.AppUserId,
                         x.UserPhones
                    }).FirstOrDefaultAsync();
               if (emp == null) return null;
               var empAppUser = await _userManager.FindByIdAsync(emp.AppUserId.ToString());
               var empusername = empAppUser == null ? "" : empAppUser.Email;
               return new EmployeeDto
               {
                    EmployeeName = emp.FirstName + " " + emp.SecondName + " " + emp.FamilyName,
                    KnownAs = emp.KnownAs,
                    Position = emp.Position,
                    OfficialPhoneNo = emp.UserPhones?.Where(x => x.IsMain && x.IsOfficial && x.IsValid).Select(x => x.PhoneNo).FirstOrDefault(),
                    OfficialMobileNo = emp.UserPhones?.Where(x => x.IsMain && x.IsValid && x.IsOfficial).Select(x => x.MobileNo).FirstOrDefault(),
                    OfficialEmailAddress = emp.Email,
                    AppUserId = emp.AppUserId,
                    UserName = empusername
               };
          }

          public async Task<int> GetEmployeeIdFromAppUserIdAsync(int appUserId)
          {
               return await _context.Employees.Where(x => x.AppUserId == appUserId).Select(x => x.Id).FirstOrDefaultAsync();
          }

          public async Task<EmployeeDto> GetEmployeeBriefAsyncFromAppUserId(int appUserId)
          {
               var emp = await _context.Employees.Where(x => x.AppUserId == appUserId)
                    .Select(x => new { x.Id, x.FirstName, x.SecondName, x.FamilyName, x.KnownAs, x.Position, x.Email })
                    .FirstOrDefaultAsync();
               if (emp != null)
               {
                    return new EmployeeDto
                    {
                         EmployeeId = emp.Id,
                         EmployeeName = emp.FirstName + " " + emp.SecondName + " " + emp.FamilyName,
                         Position = emp.Position,
                         KnownAs = emp.KnownAs,
                         Email = emp.Email,
                         AppUserId = appUserId
                    };
               }
               else
               {
                    return null;
               }
          }

          public async Task<EmployeeDto> GetEmployeeBriefAsyncFromEmployeeId(int id)
          {
               var emp = await _context.Employees.Where(x => x.Id == id)
                    .Select(x => new { x.AppUserId, x.KnownAs, x.FirstName, x.SecondName, x.FamilyName, x.Position, x.Email })
                    .FirstOrDefaultAsync();
               if (emp != null)
               {
                    return new EmployeeDto
                    {
                         EmployeeId = id,
                         EmployeeName = emp.FirstName + " " + emp.SecondName + " " + emp.FamilyName,
                         KnownAs = emp.KnownAs,
                         Position = emp.Position,
                         Email = emp.Email,
                         AppUserId = emp.AppUserId
                    };
               }
               else
               {
                    return null;
               }
          }

          public async Task<string> GetEmployeeNameFromEmployeeId(int id)
          {
               return await _context.Employees.Where(x => x.Id == id).Select(x => x.FirstName + " " + x.FamilyName).FirstOrDefaultAsync();
          }

     }
}