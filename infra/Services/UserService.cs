using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities;
using core.Entities.Admin;
using core.Entities.EmailandSMS;
using core.Entities.Identity;
using core.Entities.Users;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class UserService : IUserService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ATSContext _context;
          private readonly UserManager<AppUser> _userManager;
          private readonly ITaskService _taskService;
          private readonly IComposeMessages _composeMessages;
          //private readonly TokenService _tokenService;
          public UserService(IUnitOfWork unitOfWork, ATSContext context, ITaskService taskService,
               UserManager<AppUser> usermanager, IComposeMessages composeMessages
               //, TokenService tokenService
               )
          {
               //_tokenService = tokenService;
               _userManager = usermanager;
               _context = context;
               _unitOfWork = unitOfWork;
               _taskService = taskService;
               _composeMessages = composeMessages;
          }

          public async Task<Candidate> CreateCandidateAsync(RegisterDto registerDto)
          {
               var cand = new Candidate(registerDto.Address.Gender, registerDto.AppUserId, registerDto.AppUserIdNotEnforced,
                    await _context.Candidates.MaxAsync(x => x.ApplicationNo) + 1,
                    registerDto.Address.FirstName, registerDto.Address.SecondName,
                    registerDto.Address.FamilyName, registerDto.DisplayName, registerDto.Address.DOB,
                    registerDto.PlaceOfBirth, registerDto.AadharNo, registerDto.Email, registerDto.Introduction,
                    registerDto.Interests, registerDto.NotificationDesired, registerDto.UserQualifications, registerDto.UserProfessions,
                    registerDto.UserPassports, null);
               
               cand.Created = DateTime.UtcNow;
               cand.LastActive = DateTime.UtcNow;
               cand.CompanyId = registerDto.CompanyId;
               cand.AppUserId = registerDto.AppUserId;

               //PP No is unique in the db - include only those passports that do not already exist in the database
               var lstPP = new List<UserPassport>();
               foreach (var pp in cand.UserPassports)
               {
                    var existingPP = _context.UserPassports.Where(c => c.PassportNo == pp.PassportNo).FirstOrDefaultAsync();
                    if (existingPP == null)
                    {
                         lstPP.Add(pp);
                    }
               }
               cand.UserPassports = lstPP.Count() > 0 ? lstPP : null;
               
               if (registerDto.UserPhones != null)
               {
                    var lstP = new List<UserPhone>();
                    foreach (var p in registerDto.UserPhones)
                    {
                         var existingP = _context.UserPhones.Where(c => c.PhoneNo == p.PhoneNo).FirstOrDefaultAsync();
                         if (existingP == null)
                         {
                              lstP.Add(p);
                         }
                    }
                    cand.UserPhones = lstP.Count() > 0 ? lstP : null;
               }

               if (registerDto.UserProfessions != null)
               {
                    var qry = (from p in registerDto.UserProfessions
                               group p by new {p.CategoryId, p.IndustryId} into g
                               where g.Count() > 1
                               select g.Key);
                    cand.UserProfessions = qry.Count() == 0 && qry != null ? registerDto.UserProfessions : null;
               }

               if (registerDto.UserQualifications != null)
               {
                    var qry = (from p in cand.UserQualifications
                               group p by p.QualificationId into g
                               where g.Count() > 1
                               select g.Key);

                    cand.UserQualifications = qry.Count() == 0 && qry != null ? registerDto.UserQualifications : null;
               }

               
               _unitOfWork.Repository<Candidate>().Add(cand);

               if (registerDto.NotificationDesired) {
                    var paramsDto = new CandidateMessageParamDto{Candidate = cand, DirectlySendMessage = false};
                    await _composeMessages.AckToCandidateByEmail(paramsDto);
                    await _composeMessages.AckToCandidateBySMS(paramsDto);
               }
               
               var result = await _unitOfWork.Complete();

               if (result <= 0) return null;

               return cand;
          }

          public async Task<Candidate> GetCandidateByIdAsync(int id)
          {
               return await _unitOfWork.Repository<Candidate>().GetByIdAsync(id);
          }

          public async Task<Candidate> GetCandidateBySpecsIdentityIdAsync(string appUserId)
          {
               var spec = new CandidateSpecs(1);
               return await _unitOfWork.Repository<Candidate>().GetEntityWithSpec(spec);
          }

          public async Task<Candidate> GetCandidateBySpecsUserIdAsync(int userId)
          {
               var spec = new CandidateSpecs(userId);
               return await _unitOfWork.Repository<Candidate>().GetEntityWithSpec(spec);
          }
     //employees
          public async Task<Employee> CreateEmployeeAsync(RegisterDto registerDto)
          {
               /*
               //CHECK IF PHONES ALREADY TAKEN
               var ph = await _context.UserPhones.Where(x => 
                    registerDto.UserPhones.Select(x => x.PhoneNo).ToList().Contains(x.PhoneNo)).ToListAsync();
               if(ph!=null & ph.Count() >0) {
                    return null;
               }
               */

               var person = new Person(registerDto.Address.Gender, registerDto.Address.FirstName, registerDto.Address.SecondName,
                    registerDto.Address.FamilyName, registerDto.DisplayName, registerDto.Address.DOB,
                    registerDto.PlaceOfBirth, registerDto.AadharNo, registerDto.PpNo, registerDto.Nationality);
               
               var qs = new List<EmployeeQualification>();
               if(registerDto.UserQualifications !=null && registerDto.UserQualifications.Count > 0)
               {
                    foreach(var q in registerDto.UserQualifications)
                    {
                         qs.Add(new EmployeeQualification(q.QualificationId, q.IsMain));
                    }
               }

               var phs = new List<UserPhone>();
               if(registerDto.UserPhones != null && registerDto.UserPhones.Count() > 0) {
                    foreach(var p in registerDto.UserPhones)
                    {
                         phs.Add(new UserPhone(p.PhoneNo, p.MobileNo, p.IsMain));
                    }
               }
               phs = phs.Count() > 0 ? phs : null;

               var hrskills = new List<EmployeeHRSkill>();
               if(registerDto.HrSkills!=null && registerDto.HrSkills.Count > 0) {
                    foreach(var h in registerDto.HrSkills) {
                         hrskills.Add(new EmployeeHRSkill(h.CategoryId,h.IndustryId, h.SkillLevel));
                    }
               }
               hrskills = hrskills.Count() > 0 ? hrskills : null;

               var otherskills = new List<EmployeeOtherSkill>();
               if(registerDto.OtherSkills != null && registerDto.OtherSkills.Count() > 0) {
                    foreach(var o in registerDto.OtherSkills) {
                         otherskills.Add(new EmployeeOtherSkill(o.SkillDataId, o.SkillLevel, o.IsMain));
                    }
               }
               otherskills = otherskills.Count() > 0 ? otherskills: null;

               var emp = new Employee(registerDto.Address.Gender, registerDto.Address.FirstName,
                    registerDto.Address.SecondName, registerDto.Address.FamilyName, registerDto.DisplayName,
                    registerDto.Address.DOB, registerDto.AadharNo, qs, registerDto.DOJ,
                    registerDto.Department, hrskills, registerDto.Password, otherskills, registerDto.Address.Add, registerDto.Address.City);

               _unitOfWork.Repository<Employee>().Add(emp);

               var result = await _unitOfWork.Complete();

               if (result <= 0) return null;

               return emp;
          }

          public async Task<CustomerOfficial> CreateCustomerOfficialAsync(RegisterDto registerDto)
          {
               
               var off = new CustomerOfficial(registerDto.AppUserId, registerDto.Address.Gender, registerDto.Position, 
                    registerDto.Address.FirstName + " " + registerDto.Address.SecondName + " " + registerDto.Address.FamilyName, registerDto.Position, 
                    registerDto.UserPhones.Where(x => !string.IsNullOrEmpty(x.PhoneNo)).Select(x => x.PhoneNo).FirstOrDefault(),  
                    registerDto.UserPhones.Where(x => x.IsMain && !string.IsNullOrEmpty(x.MobileNo)).Select(x => x.MobileNo).FirstOrDefault(),
                    registerDto.Email, "", true);
                    
               _unitOfWork.Repository<CustomerOfficial>().Add(off);

               var result = await _unitOfWork.Complete();

               if (result <= 0) return null;

               return off;
          }


          public async Task<AppUser> CreateUserAsync(RegisterDto dto)
          {
               //for customer and vendor official, customer Id is mandatory
               if (dto.UserType.ToLower() =="employee" && string.IsNullOrEmpty(dto.AadharNo)) {
                    
                    throw new Exception("Exception Code 400 - for employees, Aadhar number is mandatory");
                    //return null;
               }

               if (dto.UserType.ToLower()=="official" && (int)dto.CompanyId==0) {
                    throw new Exception ("Error 400 - For officials, customer Id is essential");
                    //return null;
               }
               
               if (dto.UserPhones !=null && dto.UserPhones.Count() > 0) {
                    foreach(var ph in dto.UserPhones) {
                         if (ph.PhoneNo == "" && ph.MobileNo == "") return null;     // BadRequest(new ApiResponse(400, "either phone no or mobile no must be mentioned"));
                    }
               }

               var objPP = new UserPassport();

               if(string.IsNullOrEmpty(dto.PpNo)) {
                    objPP = null;
               } else {
                    objPP = new UserPassport(dto.PpNo, dto.Nationality, dto.PPValidity);
               }

               var user = new AppUser
               {
                    UserType = dto.UserType,
                    DisplayName = dto.DisplayName,
                    Address = dto.Address,
                    //UserPassport = objPP,

                    Email = dto.Email,
                    UserName = dto.Email
               };

               var result = await _userManager.CreateAsync(user, dto.Password);

               if (!result.Succeeded) throw new Exception("Exception Code 400 - bad request - failed to create the identity user");

               var userDtoToReturn = new UserDto
               {
                    DisplayName = user.DisplayName,
                    //Token = _tokenService.CreateToken(user),
                    Email = user.Email
               };

               var userAdded = await _userManager.FindByEmailAsync(dto.Email);
               return userAdded;
          }

          public async Task<bool> CheckEmailExistsAsync(string email)
          {
               return await _userManager.FindByEmailAsync(email) != null;
          }


     }
}