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

namespace infra.Services
{
     public class UserContactService : IUserContactService
     {
          private readonly ATSContext _context;
          private readonly IUnitOfWork _unitOfWork;
          public UserContactService(ATSContext context, IUnitOfWork unitOfWork)
          {
               _unitOfWork = unitOfWork;
               _context = context;
          }

          //from params not working
          public async Task<Pagination<UserContactDto>> GetUserContactsFromParams(UserContactSpecParams userParams)
          {
               var spec = new UserContactSpecs(userParams);
               var countSpec = new UserContactForCountSpecs(userParams);
               
               var contacts = await _unitOfWork.Repository<UserContact>().ListAsync(spec);
               var totalItems = await _unitOfWork.Repository<UserContact>().CountAsync(countSpec);
               
               var collection = new Collection<UserContact>();

               //collection = new ObservableCollection<UserContact>(collection.ToList().Distinct());
               
               var qry = from dto in contacts 
                    join c in _context.Candidates on dto.CandidateId equals c.Id
                    join item in _context.OrderItems on dto.OrderItemId equals item.Id
                    join o in _context.Orders on item.OrderId equals o.Id
                    join cat in _context.Categories on item.CategoryId equals cat.Id
                    join e in _context.Employees on dto.LoggedInUserId equals e.Id
                    select new UserContactDto(dto.Id,
                         c.FirstName + " " + c.FamilyName, c.ApplicationNo, item.Id, o.OrderNo + "-" + item.SrNo, cat.Name,
                         dto.Subject, dto.DateOfContact, e.KnownAs, dto.UserPhoneNoContacted,
                         EnumExtensions.GetEnumDisplayName(dto.enumContactResult), 
                         dto.GistOfDiscussions, dto.NextReminderOn);
                    
               var data = await Task.FromResult(qry.ToList());

               //var data = await ConvertUserContactToUserContactDto(contacts).ToList();

               var ret = new Pagination<UserContactDto>(userParams.PageIndex, userParams.PageSize, totalItems, data);
               return ret;
          }

          public async Task<UserContact> AddUserContact(UserContact userContact)
          {
               if (userContact.OrderItemId != 0)
               {
                    userContact.OrderId = await _context.OrderItems.Where(x => x.Id == userContact.OrderItemId).Select(x => x.OrderId).FirstOrDefaultAsync();
               }
               _unitOfWork.Repository<UserContact>().Add(userContact);
               
               if (await _context.Candidates.FindAsync(userContact.CandidateId) == null) throw new Exception("Invalid candidate Id");

               //update usePhones is number is wrong number
               switch (userContact.enumContactResult)
               {
                    case EnumContactResult.WrongNumber:
                         var ph = await _context.UserPhones.Where(x => x.CandidateId == userContact.CandidateId 
                              && x.MobileNo == userContact.UserPhoneNoContacted && x.IsValid == true).FirstOrDefaultAsync();
                         if (ph != null) {
                              ph.IsValid = false;
                              ph.Remarks = "set to Invalid on " + DateTime.Now + " by " + userContact.LoggedInUserId;
                              _unitOfWork.Repository<UserPhone>().Update(ph);
                         }
                         break;
                    //case EnumContactResult.MedicallyUnfit:
                    //case EnumContactResult.NotInterested:
                         
                    //case EnumContactResult.NotInterestedUnKnownReasons:
                    default:
                         break;

               }

               if (await _unitOfWork.Complete() > 0) return userContact;

               return null;
          }
          
          public async Task<UserContact> EditUserContact(UserContact userContact)
          {
               var contact = await _context.Candidates.FindAsync(userContact.CandidateId);
               if (contact==null) throw new Exception("Invalid candidate Id");
               var usr = await _context.UserContacts.FindAsync(userContact.Id);
               if (usr==null) throw new Exception("failed to retrieve user Contact object from the object to edit");
               if (usr.OrderItemId != userContact.OrderItemId)
               {
                    if (userContact.OrderItemId != 0) {
                    userContact.OrderId = await _context.OrderItems.Where(x => x.Id == userContact.OrderItemId).Select(x => x.OrderId).FirstOrDefaultAsync();
                    }
               }

               if (usr.CandidateId != userContact.CandidateId) {
                    //UserContact.CandidateId is part of key and cannot be modified.  It must be deleted and object reinserted
                    _unitOfWork.Repository<UserContact>().Delete(usr);
                    if (await _unitOfWork.Complete() == 0) throw new Exception("Failed to delete the UserContact before inserting new record");
                    usr = new UserContact(userContact.CandidateId, userContact.DateOfContact, 
                         userContact.UserPhoneNoContacted, userContact.LoggedInUserId, 
                         userContact.enumContactResult, userContact.GistOfDiscussions, 
                         userContact.NextReminderOn, userContact.OrderItemId, userContact.Subject,
                         userContact.LoggedInUserId);
                    _unitOfWork.Repository<UserContact>().Add(usr);
               } else {
                    _context.Entry(usr).CurrentValues.SetValues(userContact);
                    _context.Entry(usr).State = EntityState.Modified;
               }
               switch (userContact.enumContactResult)
               {
                    case EnumContactResult.WrongNumber:
                         var ph = await _context.UserPhones.Where(x => x.CandidateId == userContact.CandidateId 
                              && x.MobileNo == userContact.UserPhoneNoContacted && x.IsValid == true).FirstOrDefaultAsync();
                         if (ph != null) {
                              ph.IsValid = false;
                              ph.Remarks = "set to Invalid on " + DateTime.Now + " by " + userContact.LoggedInUserId;
                              _unitOfWork.Repository<UserPhone>().Update(ph);
                         }
                         break;
                    //case EnumContactResult.MedicallyUnfit:
                    //case EnumContactResult.NotInterested:
                         
                    //case EnumContactResult.NotInterestedUnKnownReasons:
                    default:
                         break;
               }
               
               if (await _context.SaveChangesAsync() > 0) return usr;
               return null;
          }
     
          public async Task<bool> DeleteUserContact(UserContact userContact)
          {
               var contact = await _context.UserContacts.FindAsync(userContact.Id);
               if (contact == null) throw new Exception("invalid object");

               //_context.Entry(contact).CurrentValues.SetValues(contact);
               //_context.Entry(contact).State = EntityState.Deleted;

               _unitOfWork.Repository<UserContact>().Delete(userContact);
               return await _unitOfWork.Complete() > 0;
          }

          public async Task<bool> DeleteUserContactById (int userContactId)
          {
               var contact = await _context.UserContacts.FindAsync(userContactId);
               if (contact == null) throw new Exception("invalid object");

               _unitOfWork.Repository<UserContact>().Delete(contact);
               return await _unitOfWork.Complete() > 0;
          }
     
     
     /*
          public async Task<ICollection<UserContactDto>> GetUserContacts(int candidateId)
          {
               var qry = await _context.UserContacts.Where(x => x.CandidateId == candidateId)
                    //.Include(x => x.UserContactedItems)
                    .ToListAsync();
               return await ConvertUserContactToUserContactDto(qry);
          }
     
          public async Task<ICollection<UserContactDto>> GetUserContactsForADate(DateTime dt)
          {
               var qry = await _context.UserContacts
                    .Where(x => dt.Date == x.DateOfContact.Date).ToListAsync();
               return await ConvertUserContactToUserContactDto(qry);
          }

          public async Task<ICollection<UserContactDto>> GetUserContactsOfAnOrder(int orderId)
          {
               var orderitemids = await _context.OrderItems
                    .Where(x => x.OrderId == orderId).Select(x => x.Id).ToListAsync();
               var temp = await _context.UserContacts.Where(x => orderitemids.Contains(x.OrderItemId))
                    .OrderBy(x => x.DateOfContact).ToListAsync();
               return await ConvertUserContactToUserContactDto(temp);
          }

          public async Task<ICollection<UserContactDto>> GetUserContactsOfAnOrderOnADate(int orderId, DateTime dt)
          {
               var orderitemids = await _context.OrderItems.Where(x => x.OrderId == orderId).Select(x => x.Id).ToListAsync();

               var temp = await _context.UserContacts
                    .Where(x => orderitemids.Contains(x.OrderItemId) && x.DateOfContact.Date == dt.Date)
                    .OrderBy(x => x.DateOfContact)
                    .ToListAsync();
               return await ConvertUserContactToUserContactDto(temp);
          }

          public async Task<ICollection<UserContactDto>> GetUserContactsForOrderItemForAContactStatus(int orderItemId, EnumContactResult enumContactResult)
          {
               var temp = await _context.UserContacts
                    .Where(x => x.OrderItemId == orderItemId && x.enumContactResult == enumContactResult)
                    .OrderByDescending(x => x.DateOfContact)
                    .ToListAsync();
               return await ConvertUserContactToUserContactDto(temp);
          }
          
          public async Task<ICollection<UserContactDto>> GetUserContactsOfACandidateForOrderItem(int candidateId, int orderItemId)
          {
               var temp = await _context.UserContacts
                    .Where(x => x.CandidateId == candidateId && x.OrderItemId == orderItemId)
                    .OrderByDescending(x => x.DateOfContact)
                    .ToListAsync();
               
               return await ConvertUserContactToUserContactDto(temp);
          }

          public async Task<ICollection<UserContactDto>> GetUserContactsOfAnOrderItem(int orderItemId)
          {
               var temp = await _context.UserContacts
                    .Where(x => x.OrderItemId == orderItemId)
                    .OrderByDescending(x => x.DateOfContact)
                    .ToListAsync();
               
               return await ConvertUserContactToUserContactDto(temp);
          }


     */
          private async Task<ICollection<UserContactDto>> ConvertUserContactToUserContactDto(ICollection<UserContact> UserContacts)
          {
               //var dtos = new List<UserContactDto>();
               var qry = from dto in UserContacts 
                    join c in _context.Candidates on dto.CandidateId equals c.Id
                    join item in _context.OrderItems on dto.OrderItemId equals item.Id
                    join o in _context.Orders on item.OrderId equals o.Id
                    join cat in _context.Categories on item.CategoryId equals cat.Id
                    join e in _context.Employees on dto.LoggedInUserId equals e.Id
                    select new UserContactDto(dto.Id,
                         c.FullName, c.ApplicationNo, item.Id, o.OrderNo + "-" + item.SrNo, cat.Name,
                         dto.Subject, dto.DateOfContact, e.KnownAs, dto.UserPhoneNoContacted,
                         EnumExtensions.GetEnumDisplayName(dto.enumContactResult), 
                         dto.GistOfDiscussions, dto.NextReminderOn);
                    
                    
               var result = await Task.FromResult(qry.ToList());

               return result;
          }

        
     }
}