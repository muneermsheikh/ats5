using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.Admin;
using core.Entities.HR;
using core.Entities.Identity;
using core.Params;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IUserHistoryService
    {
        Task<UserHistory> AddUserContact(UserHistory userHistory);
        Task<bool> DeleteUserContact(UserHistory userHistory);
        Task<bool> DeleteUserContactById(int userContactId);
        Task<ICollection<ContactResult>> GetContactResults();
        Task<bool> EditContactHistory(UserHistory model, AppUser appuser);
        Task<UserHistoryDto> GetHistoryByCandidateId(int candidateid);
        Task<UserHistoryDto> GetOrAddUserHistoryByParams(UserHistoryParams historyParams);
        //Task<UserHistoryDto> GetOrAddUserHistoryByNamePhone(string callerame, string mobileno);
        Task<UserHistoryDto> GetHistoryFromHistoryId(int historyId);
    }
}
