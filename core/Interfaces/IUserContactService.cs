using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.Admin;
using core.Params;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IUserContactService
    {
        
        Task<UserContact> AddUserContact(UserContact userContactedItem);
        Task<UserContact> EditUserContact(UserContact userContactedItem);
        Task<bool> DeleteUserContact(UserContact userContact);
        Task<bool> DeleteUserContactById(int userContactId);

        Task<Pagination<UserContactDto>> GetUserContactsFromParams (UserContactSpecParams userContactSpecParams);
        
        /*
        Task<ICollection<UserContactDto>> GetUserContacts (int candidateId);
        Task<ICollection<UserContactDto>> GetUserContactsOfAnOrder(int orderId);
        Task<ICollection<UserContactDto>> GetUserContactsForADate(DateTime dt);
        Task<ICollection<UserContactDto>> GetUserContactsOfAnOrderItem(int orderItemId);
        Task<ICollection<UserContactDto>> GetUserContactsForOrderItemForAContactStatus(int orderItemId, EnumContactResult enumContactResult);
        Task<ICollection<UserContactDto>> GetUserContactsOfACandidateForOrderItem(int candidateId, int orderItemId);
        Task<ICollection<UserContactDto>> GetUserContactsOfAnOrderOnADate(int orderId, DateTime dt);
        */
        
        

    }
}
