using System.Linq;
using System.Threading.Tasks;
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
          public EmployeeService(ATSContext context, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
          {
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

               if (await _context.SaveChangesAsync() > 0) {
                    var appuser = new AppUser
                    {
                            UserType = "employee", Gender = emp.Gender, DisplayName = emp.KnownAs,
                            Email = emp.Email, UserName = emp.Email,
                            PhoneNumber = emp.UserPhones.Select(x => x.MobileNo).FirstOrDefault(),
                            Address = new Address
                            {
                                AddressType = "R", Gender = emp.Gender, FirstName = emp.FirstName + " " + emp.SecondName + " " + emp.FamilyName,
                                Add = emp.Add, State = emp.Address2, City = emp.City, Country = emp.Country
                            }
                    };

                    await _userManager.UpdateAsync(appuser);

                    return true;
               } else {
                   return false;
               }
          }

          public async Task<Employee> AddNewEmployee(Employee employee)
          {
               var appuser = new AppUser
               {
                    UserType = "employee", Gender = employee.Gender, DisplayName = employee.KnownAs,
                    Email = employee.Email, UserName = employee.Email,
                    PhoneNumber = employee.UserPhones.Select(x => x.MobileNo).FirstOrDefault(),
                    Address = new Address
                    {
                         AddressType = "R", Gender = employee.Gender, FirstName = employee.FirstName + " " + employee.SecondName + " " + employee.FamilyName,
                         Add = employee.Add, State = employee.Address2, City = employee.City, Country = employee.Country
                    }
               };

               var added = await _userManager.CreateAsync(appuser, employee.Password);
               if (added.Succeeded)
               {
                    var user = await _userManager.FindByEmailAsync(employee.Email);
                    employee.AppUserId = user.Id;
               }

               _unitOfWork.Repository<Employee>().Add(employee);

               if (await _unitOfWork.Complete() > 0)
               {
                    return await _context.Employees
                        .Where(x => x.FirstName == employee.FirstName && x.FamilyName == employee.FamilyName &&
                            x.City == employee.City).FirstOrDefaultAsync();
               }
               else
               {
                    return null;
               }
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
     }
}