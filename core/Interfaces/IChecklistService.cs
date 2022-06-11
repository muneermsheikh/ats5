using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.HR;
using core.Entities.Identity;
using core.Entities.MasterEntities;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IChecklistService
    {
        //masterdata
        Task<ChecklistHRData> AddChecklistHRParameter(string checklistParameter);
        Task<bool> DeleteChecklistHRDataAsync(ChecklistHRData checklistHRData);
        Task<bool> EditChecklistHRDataAsync(ChecklistHRData checklistHRData);
        Task<IReadOnlyList<ChecklistHRData>> GetChecklistHRDataListAsync();
        

        //checklistHR
        Task<ChecklistHR> AddNewChecklistHR(int candidateId, int orderItemId, int LoggedInEmployeeId);
        Task<List<string>> EditChecklistHR(ChecklistHRDto model, LoggedInUserDto loggedInUserDto);
        Task<ChecklistHRDto> GetChecklistHR(int candidateId, int orderItemId, LoggedInUserDto loggedInUserDto);
        Task<bool> DeleteChecklistHR(ChecklistHRDto checklistHR, LoggedInUserDto loggedInDto);
        Task<int> GetChecklistHRId(int candidateid, int orderitemid);

    }
}