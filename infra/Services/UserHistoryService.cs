using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.Admin;
using core.Entities.Users;
using core.Interfaces;
using core.ParamsAndDtos;
using infra.Data;
using Microsoft.EntityFrameworkCore;
using core.Extensions;
using core.Params;
using core.Specifications;
using System.Collections.ObjectModel;
using core.Entities.Identity;
using core.Entities;
using AutoMapper;
using core.Entities.HR;

namespace infra.Services
{
     public class UserHistoryService : IUserHistoryService
     {
          private readonly ATSContext _context;
          private readonly IUnitOfWork _unitOfWork;
          private readonly ICommonServices _commonServices;
          private readonly IMapper _mapper;
          public IUserService UserService { get; }
          private readonly IUserService _userService;
          public UserHistoryService(ATSContext context, IUnitOfWork unitOfWork, ICommonServices commonServices,  IUserService userService, IMapper mapper)
          {
               _userService = userService;
               UserService = userService;
               _mapper = mapper;
               _commonServices = commonServices;
               _unitOfWork = unitOfWork;
               _context = context;
          }

           public async Task<UserHistory> AddUserContact(UserHistory userContact)
          {
               if (userContact.PersonId !=0) {
                    var user = await _context.Candidates.FindAsync(userContact.PersonId);
                    if (user == null) return null;
                    userContact.PersonName = user.FullName;
               }
               _unitOfWork.Repository<UserHistory>().Add(userContact);
               
               if (await _unitOfWork.Complete() > 0) return userContact;

               return null;
          }
          
          public async Task<bool> DeleteUserContact(UserHistory userContact)
          {
               var contact = await _context.UserHistories.FindAsync(userContact.Id);
               if (contact == null) throw new Exception("invalid object");

               _unitOfWork.Repository<UserHistory>().Delete(userContact);
               return await _unitOfWork.Complete() > 0;
          }

          public async Task<bool> DeleteUserContactById (int userContactId)
          {
               var contact = await _context.UserHistories.FindAsync(userContactId);
               if (contact == null) throw new Exception("invalid object");

               _unitOfWork.Repository<UserHistory>().Delete(contact);
               return await _unitOfWork.Complete() > 0;
          }

          public async Task<ICollection<ContactResult>> GetContactResults()
          {
               return await _context.ContactResults.OrderBy(x => x.Name).ToListAsync();
          }

          /*
          public async Task<UserHistoryDto> GetOrAddUserHistoryByNamePhone(string callerame, string mobileno) 
          {
               var ph = mobileno;
               if (ph.StartsWith("00")) ph=ph.Substring(3);
               if (ph.StartsWith("0")) ph=ph.Substring(2);
               if (ph.StartsWith("+91")) ph=ph.Substring(4);
               
               ph=mobileno.Trim();
               if (ph.Length < 10) throw new Exception("invalid phone no. " + mobileno + ", it should be 10 digits log");

               var history = await _context.UserHistories.Where(x=> x.PhoneNo == ph)
                    .Include(x => x.UserHistoryItems.OrderByDescending(x => x.DateOfContact))
                    .FirstOrDefaultAsync();
               if (history == null) {
                    var newhistory = new UserHistory{PhoneNo = ph, PartyName = callerame};
                    _unitOfWork.Repository<UserHistory>().Add(newhistory);
                    if (await _unitOfWork.Complete() == 0) throw new Exception("failed to write the user history to Database");
               }
               return _mapper.Map<UserHistory, UserHistoryDto>(history);
          }
          */
          public async Task<UserHistoryDto> GetHistoryFromHistoryId(int historyId)
          {
               var history = await _context.UserHistories.Where(x => x.Id == historyId).Include(x => x.UserHistoryItems).FirstOrDefaultAsync();
               return _mapper.Map<UserHistory, UserHistoryDto>(history);
          }

          public async Task<UserHistoryDto> GetOrAddUserHistoryByParams(UserHistoryParams histParams)
          {
               //check if the object has empty elements
               
               var cv = new Candidate();
               var history = new UserHistory();
               string ph = histParams.MobileNo;

               if (histParams.Id > 0) {
                    history = await _context.UserHistories.Where(x => x.Id == histParams.Id)
                         .Include(x => x.UserHistoryItems).FirstOrDefaultAsync();
                    if(history !=null) return _mapper.Map<UserHistory, UserHistoryDto>(history);
               }

               var qry = _context.UserHistories.AsQueryable();
               if (histParams.ApplicationNo.HasValue) {
                    qry =  qry.Where(x =>x.ApplicationNo == histParams.ApplicationNo);
               } else {
                    if (!string.IsNullOrEmpty(ph)) qry = qry.Where(x => x.PhoneNo == ph);
                    if (!string.IsNullOrEmpty(histParams.EmailId)) qry = qry.Where(x => x.EmailId.ToLower() == histParams.EmailId.ToLower());
                    if(histParams.PersonId.HasValue) qry = qry.Where(x => x.PersonId==histParams.PersonId);
               }
               qry = qry.Include(x => x.UserHistoryItems);
               history = await qry.FirstOrDefaultAsync();

               if (history == null) {        //
                    //based upon persontype, get person details either from customer or candidate
                    //and create new history object
                    switch(histParams.PersonType.ToLower()) {
                         case "customer":
                         case  "vendor":
                         case "associate":
                              var qryC = _context.CustomerOfficials.Where(x => x.IsValid).AsQueryable();
                              if (histParams.PersonId.HasValue) qryC = qryC.Where(x => x.Id == histParams.PersonId);
                              if (!string.IsNullOrEmpty(histParams.EmailId)) qryC = qryC.Where(x => x.Email.ToLower() == histParams.EmailId.ToLower());
                              if (!string.IsNullOrEmpty(histParams.MobileNo)) qryC = qryC.Where(x => x.Mobile == ph);
                              var off = await qryC.Select(x => new {x.Id, x.OfficialName, x.Customer.CustomerName, x.Mobile, x.Customer.CustomerType}).FirstOrDefaultAsync();
                              if (off != null) {
                                   //history = new UserHistory(cust.Id, cust.OfficialName + "(" + custname + ")", null);
                                   history = new UserHistory{PersonId=off.Id, PersonName=off.OfficialName + "(" + off.CustomerName + ")", 
                                        PhoneNo = off.Mobile, PersonType = off.CustomerType};
                              }
                              break;
                         case "candidate":
                              if (!string.IsNullOrEmpty(histParams.MobileNo )) {
                                   cv = await _context.UserPhones.Where(x => x.MobileNo==ph).Select(x => x.Candidate).FirstOrDefaultAsync();
                              } else {
                                   var qryD = _context.Candidates.Include(x => x.UserPhones).AsQueryable();
                                   if (histParams.PersonId.HasValue) qryD = qryD.Where(x => x.Id == histParams.PersonId);
                                   if (!string.IsNullOrEmpty(histParams.EmailId )) qryD = qryD.Where(x => x.Email.ToLower() == histParams.EmailId.ToLower());
                                   cv = await qryD.FirstOrDefaultAsync();
                              }
                              
                              if (cv != null) {
                                   history = new UserHistory{PersonType=histParams.PersonType, PersonName = cv.FullName, PersonId = cv.Id,
                                        PhoneNo = cv.UserPhones.Where(x => x.IsMain).Select(x => x.MobileNo).FirstOrDefault()};
                              } 
                              break;
                         default:
                              break;
                    }
               
                    if (history !=null) {
                         _unitOfWork.Repository<UserHistory>().Add(history);
                         if (await _unitOfWork.Complete() == 0) return null;
                    } else if(histParams.CreateNewIfNull && cv==null && !string.IsNullOrEmpty(ph) && !string.IsNullOrEmpty(histParams.PersonName)) {
                         history = new UserHistory{PersonType=histParams.PersonType, PersonName = histParams.PersonName, PhoneNo = ph};
                    }
               }
               if (history == null) return null;
          
               var historyDto = _mapper.Map<UserHistory, UserHistoryDto>(history);

               if(historyDto.UserHistoryItems != null) {
                    foreach(var item in historyDto.UserHistoryItems) {
                         if (item.LoggedInUserId > 0) item.LoggedInUserName = await _commonServices.GetEmployeeNameFromEmployeeId(item.LoggedInUserId);
                         if (item.ContactResult > 0) item.ContactResultName = Enum.GetName(typeof(EnumContactResult), item.ContactResult);
                    }
               }
               
               return historyDto;
          }

          public async Task<bool> EditContactHistory(UserHistory model, AppUser appuser)
          {
               if (string.IsNullOrEmpty(model.PersonName) &&
                    model.PersonId == 0 ) return false;
               if (model.Id==0) return false;

               var existingHistory = await _context.UserHistories
                    .Where(x => x.Id == model.Id)
                    .Include(x => x.UserHistoryItems)
                    .AsNoTracking()
                    .SingleOrDefaultAsync();
               
               if (existingHistory == null) return false;
               int loggedInUserId=9;
               if (appuser != null) loggedInUserId = await _context.Employees.Where(x => x.Email == appuser.Email).Select(x => x.Id).FirstOrDefaultAsync();
               _context.Entry(existingHistory).CurrentValues.SetValues(model);

               foreach(var existingHistoryItem in existingHistory.UserHistoryItems.ToList())
               {
                    if (!model.UserHistoryItems.Any(c => c.Id == existingHistoryItem.Id && 
                         c.Id != default(int)))
                    {
                         _context.UserHistoryItems.Remove(existingHistoryItem);
                         _context.Entry(existingHistoryItem).State = EntityState.Deleted;
                    }
               }

               if(model.UserHistoryItems != null) {
                    foreach(var modelHistoryItem in model.UserHistoryItems)
                    {
                         var existingModelItem = existingHistory.UserHistoryItems
                              .Where(c => c.Id == modelHistoryItem.Id && c.Id != default(int)).SingleOrDefault();
                         if (existingModelItem != null)
                         {
                              _context.Entry(existingModelItem).CurrentValues.SetValues(modelHistoryItem);
                              _context.Entry(existingModelItem).State = EntityState.Modified;
                         } else {
                              var newHistoryItem = new UserHistoryItem(model.Id, modelHistoryItem.PhoneNo, 
                                   modelHistoryItem.DateOfContact.Year < 2000 ? DateTime.Now : modelHistoryItem.DateOfContact, 
                                   loggedInUserId, modelHistoryItem.Subject, 
                                   modelHistoryItem.CategoryRef, modelHistoryItem.ContactResult, modelHistoryItem.GistOfDiscussions);
                                   
                              existingHistory.UserHistoryItems.Add(newHistoryItem);
                              _context.Entry(newHistoryItem).State = EntityState.Added;
                         }
                    }
               }

               _context.Entry(existingHistory).State = EntityState.Modified;

               return await _context.SaveChangesAsync() > 0;

          }
     
          public async Task<CandidateBriefDto> GetCandidateBriefByParams(CandidateSpecParams SpecParams)
          {
               var cand = new Candidate();

               if(SpecParams.ApplicationNoFrom != 0) {
                    var c = await _context.Candidates.Where(x => x.ApplicationNo == SpecParams.ApplicationNoFrom)
                         .Select(x => new CandidateBriefDto(x.Id, x.Gender, x.ApplicationNo, x.AadharNo, x.FullName,
                              x.City, x.ReferredBy, ""))
                         .FirstOrDefaultAsync();
                    return c;
               } else if (SpecParams.CandidateId != 0) {
                    var c = await _context.Candidates.Where(x => x.Id == SpecParams.CandidateId)
                         .Select(x => new CandidateBriefDto(x.Id, x.Gender, x.ApplicationNo, x.AadharNo, x.FullName,
                              x.City, x.ReferredBy, ""))
                         .FirstOrDefaultAsync();
                    return c;
               } 
               return null;
          }

          public async Task<UserHistoryDto> GetHistoryByCandidateId(int candidateid)
          {
               var history = await _context.UserHistories.Where(x => x.PersonId == candidateid && x.PersonType.ToLower() == "candidate")
                    .Include(x => x.UserHistoryItems)
                    .FirstOrDefaultAsync();

               if (history == null) {
                    var cv = await _context.Candidates.Where(x => x.Id == candidateid)
                         .Select(x => new {id=x.Id, name=x.FullName, appno=x.ApplicationNo, emailid=x.Email, ph=x.UserPhones.Where(x => x.IsMain).Select(x => x.MobileNo).FirstOrDefault()})
                         .FirstOrDefaultAsync();
                    if (cv == null) return null;
                    history = new UserHistory{ApplicationNo=cv.appno, PersonId=cv.id, PersonName=cv.name, PersonType="candidate", PhoneNo=cv.ph, EmailId=cv.emailid};
                    _unitOfWork.Repository<UserHistory>().Add(history);
                    if (await _unitOfWork.Complete() == 0) return null;
               }
               
               var historyDto = _mapper.Map<UserHistory, UserHistoryDto>(history);

               if(historyDto.UserHistoryItems != null) {
                    foreach(var item in historyDto.UserHistoryItems) {
                         if (item.LoggedInUserId > 0) item.LoggedInUserName = await _commonServices.GetEmployeeNameFromEmployeeId(item.LoggedInUserId);
                         if (item.ContactResult > 0) item.ContactResultName = Enum.GetName(typeof(EnumContactResult), item.ContactResult);
                    }
               }

               return historyDto;
          }
     }
}