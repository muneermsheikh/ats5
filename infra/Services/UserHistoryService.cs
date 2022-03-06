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

          public async Task<UserHistoryDto> GetUserHistoryData(UserHistorySpecParams userParams)
          {
               var spec = new UserContactSpecs(userParams);
               var userhistory = await _unitOfWork.Repository<UserHistory>().GetEntityWithSpec(spec);
               if (userhistory == null) return null;
               return _mapper.Map<UserHistory, UserHistoryDto>(userhistory);
          }

          public async Task<UserHistory> AddUserContact(UserHistory userContact)
          {
               if (userContact.CandidateId !=0) {
                    var user = await _context.Candidates.FindAsync(userContact.CandidateId);
                    if (user == null) return null;
                    userContact.PartyName = user.FullName;
                    userContact.ApplicationNo = user.ApplicationNo;
                    userContact.AadharNo = user.AadharNo;
               } else if (userContact.CustomerOfficialId !=0) {
                    var user = await (from o in _context.CustomerOfficials where o.Id == userContact.CustomerOfficialId
                         join c in _context.Customers on o.CustomerId equals c.Id 
                         select new {o.OfficialName, c.Id}).FirstOrDefaultAsync();
                    userContact.PartyName = user.OfficialName;
                    //userContact.CustomerId = user.Id;
               }
               _unitOfWork.Repository<UserHistory>().Add(userContact);
               
               if (await _unitOfWork.Complete() > 0) return userContact;

               return null;
          }
          
          public async Task<bool> DeleteUserContact(UserHistory userContact)
          {
               var contact = await _context.UserHistories.FindAsync(userContact.Id);
               if (contact == null) throw new Exception("invalid object");

               //_context.Entry(contact).CurrentValues.SetValues(contact);
               //_context.Entry(contact).State = EntityState.Deleted;

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


          public async Task<UserHistoryDto> GetOrAddUserHistoryByParams(UserHistorySpecParams specParams)
          {
               var history = new UserHistory();
               if (specParams.CandidateId.HasValue) {
                    history = await _context.UserHistories.Where(x=> x.CandidateId == (int)specParams.CandidateId)
                         .Include(x => x.UserHistoryItems.OrderByDescending(x => x.DateOfContact))
                         .FirstOrDefaultAsync();
               } else if (specParams.ApplicationNo.HasValue) {
                    history = await _context.UserHistories.Where(x => x.ApplicationNo == (int)specParams.ApplicationNo)
                         .Include(x => x.UserHistoryItems.OrderByDescending(x => x.DateOfContact))
                         .FirstOrDefaultAsync();
               };
               //var spec = new UserContactSpecs(specParams);
               //var history = await _unitOfWork.Repository<UserHistory>().GetEntityWithSpec(spec);

               if (history == null) {
                    if(specParams.CandidateId.HasValue) {
                         var cand = await _userService.GetCandidateBriefById((int)specParams.CandidateId);
                         if (cand == null) return null;
                         history = new UserHistory(cand.Id, cand.AadharNo, cand.FullName, cand.ApplicationNo, null);
                         _unitOfWork.Repository<UserHistory>().Add(history);
                         if(await _unitOfWork.Complete() == 0) return null;
                    }
               }
               if (history == null) return null;
               var historyDto = new UserHistoryDto();
               historyDto = _mapper.Map<UserHistory, UserHistoryDto>(history);

               foreach(var item in historyDto.UserHistoryItems) {
                    if (item.LoggedInUserId > 0) item.LoggedInUserName = await _commonServices.GetEmployeeNameFromEmployeeId(item.LoggedInUserId);
                    if (item.ContactResult > 0) item.ContactResultName = Enum.GetName(typeof(EnumContactResult), item.ContactResult);
               }
               return historyDto;
          }

 
          public async Task<bool> EditContactHistory(UserHistory model, AppUser appuser)
          {
               if (string.IsNullOrEmpty(model.AadharNo) &&
                    model.ApplicationNo == 0 ) return false;
               if (string.IsNullOrEmpty(model.PartyName)) return false;

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
     }
}