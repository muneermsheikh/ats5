using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.EmailandSMS;
using core.Entities.HR;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface ISelectionDecisionService
    {
         Task<IReadOnlyList<EmailMessage>> RegisterSelections(ICollection<SelDecisionToAddDto> selDto, int loggedInEmpId);
         Task<bool> EditSelection(SelectionDecision selectionDecision);
         Task<bool> DeleteSelection(SelectionDecision selectionDecision);
         Task<Pagination<SelectionDecision>> GetSelectionDecisions (SelDecisionSpecParams specParams);
         Task<IReadOnlyList<SelectionsPendingDto>> GetPendingSelections();
         
    }
}