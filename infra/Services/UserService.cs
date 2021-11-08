using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using core.Entities;
using core.Entities.Admin;
using core.Entities.EmailandSMS;
using core.Entities.Identity;
using core.Entities.Users;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace infra.Services
{
     public class UserService : IUserService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ATSContext _context;
          private readonly UserManager<AppUser> _userManager;
          private readonly ITaskService _taskService;
          private readonly IComposeMessages _composeMessages;
          private readonly IConfiguration _config;
          //private readonly TokenService _tokenService;
          public UserService(IUnitOfWork unitOfWork, ATSContext context, ITaskService taskService, IConfiguration config,
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
               _config = config;
          }

          public async Task<Candidate> CreateCandidateAsync(RegisterDto registerDto)
          {
               var NextAppNo = await _context.Candidates.MaxAsync(x => x.ApplicationNo);
               NextAppNo = NextAppNo == 0 ? 10001 : NextAppNo+1;

               var cand = new Candidate(registerDto.Address.Gender, registerDto.AppUserId, registerDto.AppUserIdNotEnforced,
                    NextAppNo  ,     //await _context.Candidates.MaxAsync(x => x.ApplicationNo) ?? 1000 + 1,
                    registerDto.Address.FirstName, registerDto.Address.SecondName,
                    registerDto.Address.FamilyName, registerDto.DisplayName, registerDto.Address.DOB,
                    registerDto.PlaceOfBirth, registerDto.AadharNo, registerDto.Email, registerDto.Introduction,
                    registerDto.Interests, registerDto.NotificationDesired, registerDto.UserQualifications, registerDto.UserProfessions,
                    registerDto.UserPassports, null);
               
               cand.Created = DateTime.UtcNow;
               cand.LastActive = DateTime.UtcNow;
               cand.CompanyId = registerDto.CompanyId;
               cand.AppUserId = registerDto.AppUserId;
               
               if (registerDto.EntityAddresses != null)
               {
                    var lstAdd = new List<EntityAddress>();
                    foreach(var add in cand.EntityAddresses)
                    {
                         lstAdd.Add(add);          
                    }
                    cand.EntityAddresses = lstAdd;               
               }
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
                         var existingP = _context.UserPhones.Where(c => c.MobileNo == p.MobileNo).FirstOrDefaultAsync();
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

               //upload file attachments
               

               return cand;
          }

/*          public async Task<Candidate> GetCandidateByIdAsync(int id)
          {
               return await _unitOfWork.Repository<Candidate>().GetByIdAsync(id);
          }

          public async Task<Candidate> GetCandidateBySpecsIdentityIdAsync(int appUserId)
          {
               var spec = new CandidateSpecs(1);
               return await _unitOfWork.Repository<Candidate>().GetEntityWithSpec(spec);
          }

          public async Task<Candidate> GetCandidateBySpecsUserIdAsync(int userId)
          {
               var spec = new CandidateSpecs(userId);
               return await _unitOfWork.Repository<Candidate>().GetEntityWithSpec(spec);
          }
*/
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
                         phs.Add(new UserPhone(p.MobileNo, p.IsMain));
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
                    registerDto.Address.DOB, registerDto.AadharNo, qs, registerDto.DOJ, registerDto.Department, hrskills, 
                    otherskills, registerDto.Address.Add, registerDto.Address.City, registerDto.Position);

               _unitOfWork.Repository<Employee>().Add(emp);

               var result = await _unitOfWork.Complete();

               if (result <= 0) return null;

               return emp;
          }

          public async Task<CustomerOfficial> CreateCustomerOfficialAsync(RegisterDto registerDto)
          {
               
               var off = new CustomerOfficial(registerDto.AppUserId, registerDto.Address.Gender, registerDto.Position, 
                    registerDto.Address.FirstName + " " + registerDto.Address.SecondName + " " + registerDto.Address.FamilyName, registerDto.Position, 
                    registerDto.UserPhones.Where(x => !string.IsNullOrEmpty(x.MobileNo)).Select(x => x.MobileNo).FirstOrDefault(),  
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
                         if (ph.MobileNo == "") return null;     // BadRequest(new ApiResponse(400, "either phone no or mobile no must be mentioned"));
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

               //var userAdded = await _userManager.FindByEmailAsync(dto.Email);
               return user;
          }

          public async Task<bool> CheckEmailExistsAsync(string email)
          {
               return await _userManager.FindByEmailAsync(email) != null;
          }

          public async Task<ICollection<UserProfession>> EditUserProfessions(UserAndProfessions userProfessions)
          {
               var candidateId = userProfessions.CandidateId;

               var selectedProfessions = new List<UserProfession>();
               foreach(var item in userProfessions.CandidateProfessions)
               {
                    if (await _unitOfWork.Repository<Category>().GetByIdAsync(item.CategoryId) != null)  
                         // check if the profession exists in DB before adding to selectedProfessions
                         selectedProfessions.Add(new UserProfession(
                         candidateId, item.CategoryId, item.IndustryId, item.IsMain));
               } 

               if (selectedProfessions.Count() == 0 ) 
                    throw new Exception ("none of the professions of the Candidate exist on record");
                    
               //get the candidate Professions as exist in DB.  Add to the DB those professions that do not
               //existing in this DB set (selectedProfessions.ExceptWhatExist in Db)
               var existingUserProfessions = await _unitOfWork.Repository<UserProfession>()
                    .ListAsync(new UserProfessionsSpecs(candidateId));
                         //
               var selectedCategoryIds = userProfessions.CandidateProfessions.Select(x => x.CategoryId).ToArray();
               var existingUserCategoryIds = existingUserProfessions.Select(x => x.CategoryId).ToList();
               foreach(var p in selectedProfessions.Where(x => !existingUserCategoryIds.Contains(x.CategoryId)).ToList())
               {
                    _unitOfWork.Repository<UserProfession>().Add(new UserProfession(candidateId, p.CategoryId, p.IndustryId, p.IsMain));
               }
               
               //remove professions
               //int ct = 0;
               //var selectedIds = selectedProfessions.Select(x => x.CategoryId).ToList();
               foreach(var p in existingUserProfessions.Where(x => !selectedProfessions.Select(x => x.CategoryId).ToList().Contains(x.CategoryId)).ToList())
               {
                    _unitOfWork.Repository<UserProfession>().Delete(p);
                    //ct++;
               }
               
               if(await _unitOfWork.Complete() == 0) throw new Exception("Failed to edit the user professions, Or the records did not need to change");

               return selectedProfessions;
          }

          public async Task<Pagination<Candidate>> GetCandidates(CandidateSpecParams candidateParams)
          {
               var specsParams = new CandidateSpecs(candidateParams);
               var countParams = new CandidateForCountSpecs(candidateParams);

               var totalItems = await _unitOfWork.Repository<Candidate>().CountAsync(countParams);
               var candidateList = await _unitOfWork.Repository<Candidate>().ListAsync(specsParams);

               return new Pagination<Candidate>(candidateParams.PageIndex, candidateParams.PageSize, totalItems, candidateList);
          }

          public async Task<Candidate> UpdateCandidateAsync(Candidate model )
          {
               var existingObject = await _context.Candidates.Where(x => x.Id == model.Id)
                    .Include(x => x.EntityAddresses).Include(x => x.UserPhones)
                    .Include(x => x.UserQualifications).Include(x => x.UserProfessions)
                    .Include(x => x.UserExperiences).Include(x => x.UserAttachments)
               .FirstOrDefaultAsync();

               if (existingObject == null) return null;
               //update top level entity, i.e. candidate, without related obj

               _context.Entry(existingObject).CurrentValues.SetValues(model);

               //start updating related entities
               //start with deleting records from DB whicha are not present in the model
               foreach(var existingItem in existingObject.EntityAddresses.ToList())
               {
                    if (!model.EntityAddresses.Any(c => c.Id == existingItem.Id && c.Id != default(int)))
                    {
                         _context.EntityAddresses.Remove(existingItem);
                         _context.Entry(existingItem).State = EntityState.Deleted;
                    }
               }
               //records that are present in DB AND model are the ones to be updated;
               if (model.EntityAddresses != null)
               {
                    foreach(var item in model.EntityAddresses)
                    {
                         var existingItem = existingObject.EntityAddresses.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();
                         if (existingItem != null)     //record present in DB, therefore update DB record with values from the model
                         {
                              _context.Entry(existingItem).CurrentValues.SetValues(item);
                              _context.Entry(existingItem).State = EntityState.Modified;
                         } else         //insert new record
                         {
                              var newObj = new EntityAddress(item.AddressType, item.Add, item.StreetAdd,
                                   item.City, item.Pin, item.State, item.District, item.Country, item.IsMain);
                              existingObject.EntityAddresses.Add(newObj);
                              _context.Entry(newObj).State = EntityState.Added;
                         }
                    }
               }

               //UserPhones
               if (model.UserPhones != null)
               {
                    foreach(var existingItem in existingObject.UserPhones.ToList())
                    {
                         if (!model.UserPhones.Any(c => c.Id == existingItem.Id && c.Id != default(int)))
                         {
                              _context.UserPhones.Remove(existingItem);
                              _context.Entry(existingItem).State = EntityState.Deleted;
                         }
                    }
                    foreach(var item in model.UserPhones)
                    {
                         var existingItem = existingObject.UserPhones.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();
                         if (existingItem != null)     //record present in DB, therefore update DB record with values from the model
                         {
                              _context.Entry(existingItem).CurrentValues.SetValues(item);
                              _context.Entry(existingItem).State = EntityState.Modified;
                         } else         //insert new record
                         {
                              var newObj = new UserPhone(existingObject.Id, item.MobileNo, item.IsMain);
                              existingObject.UserPhones.Add(newObj);
                              _context.Entry(newObj).State = EntityState.Added;
                         }
                    }
               }

               //UserQualifications
               if(model.UserQualifications != null)
               {
                    foreach(var existingItem in existingObject.UserQualifications.ToList())
                    {
                         if (!model.UserQualifications.Any(c => c.Id == existingItem.Id && c.Id != default(int)))
                         {
                              _context.UserQualifications.Remove(existingItem);
                              _context.Entry(existingItem).State = EntityState.Deleted;
                         }
                    }
                    foreach(var item in model.UserQualifications)
                    {
                         var existingItem = existingObject.UserQualifications.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();
                         if (existingItem != null)     //record present in DB, therefore update DB record with values from the model
                         {
                              _context.Entry(existingItem).CurrentValues.SetValues(item);
                              _context.Entry(existingItem).State = EntityState.Modified;
                         } else         //insert new record
                         {
                              var newObj = new UserQualification(existingObject.Id, item.QualificationId, item.IsMain);
                              existingObject.UserQualifications.Add(newObj);
                              _context.Entry(newObj).State = EntityState.Added;
                         }
                    }
               }

               
               //UserProfessions
               if(model.UserProfessions != null)
               {
                    foreach(var existingItem in existingObject.UserProfessions.ToList())
                    {
                         if (!model.UserProfessions.Any(c => c.Id == existingItem.Id && c.Id != default(int)))
                         {
                              _context.UserProfessions.Remove(existingItem);
                              _context.Entry(existingItem).State = EntityState.Deleted;
                         }
                    }

                    foreach(var item in model.UserProfessions)
                    {
                         var existingItem = existingObject.UserProfessions.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();
                         if (existingItem != null)     //record present in DB, therefore update DB record with values from the model
                         {
                              _context.Entry(existingItem).CurrentValues.SetValues(item);
                              _context.Entry(existingItem).State = EntityState.Modified;
                         } else         //insert new record
                         {
                              var newObj = new UserProfession(existingObject.Id, item.CategoryId, item.IndustryId, item.IsMain);
                              existingObject.UserProfessions.Add(newObj);
                              _context.Entry(newObj).State = EntityState.Added;
                         }
                    }
               }

               //UserPassports
               if (model.UserPassports != null)
               {
                    foreach(var existingItem in existingObject.UserPassports.ToList())
                    {
                         if (!model.UserPassports.Any(c => c.Id == existingItem.Id && c.Id != default(int)))
                         {
                              _context.UserPassports.Remove(existingItem);
                              _context.Entry(existingItem).State = EntityState.Deleted;
                         }
                    }

                    foreach(var item in model.UserPassports)
                    {
                         var existingItem = existingObject.UserPassports.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();
                         if (existingItem != null)     //record present in DB, therefore update DB record with values from the model
                         {
                              _context.Entry(existingItem).CurrentValues.SetValues(item);
                              _context.Entry(existingItem).State = EntityState.Modified;
                         } else         //insert new record
                         {
                              var newObj = new UserPassport(existingObject.Id, item.PassportNo, item.Nationality, item.Validity);
                              existingObject.UserPassports.Add(newObj);
                              _context.Entry(newObj).State = EntityState.Added;
                         }
                    }
               }

               //UserAExperiences
               if (model.UserExperiences != null)
               {
                    foreach(var existingItem in existingObject.UserExperiences.ToList())
                    {
                         if (!model.UserExperiences.Any(c => c.Id == existingItem.Id && c.Id != default(int)))
                         {
                              _context.UserExps.Remove(existingItem);
                              _context.Entry(existingItem).State = EntityState.Deleted;
                         }
                    }
                    foreach(var item in model.UserExperiences)
                    {
                         var existingItem = existingObject.UserExperiences.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();
                         if (existingItem != null)     //record present in DB, therefore update DB record with values from the model
                         {
                              _context.Entry(existingItem).CurrentValues.SetValues(item);
                              _context.Entry(existingItem).State = EntityState.Modified;
                         } else         //insert new record
                         {
                              var nextSrNo = await _context.UserExps.Where(x => x.CandidateId == existingObject.Id).MaxAsync(x => x.SrNo);
                              ++nextSrNo;
                              var newObj = new UserExp(existingObject.Id, nextSrNo, item.PositionId, item.Employer, 
                                   item.Position, item.SalaryCurrency, (int)item.MonthlySalaryDrawn, item.WorkedFrom, 
                                   item.WorkedUpto);
                              existingObject.UserExperiences.Add(newObj);
                              _context.Entry(newObj).State = EntityState.Added;
                         }
                    }
               }
          
               _context.Entry(existingObject).State = EntityState.Modified;

               if (await _context.SaveChangesAsync() > 0) return existingObject;

               return null;
          }



          
     }
}