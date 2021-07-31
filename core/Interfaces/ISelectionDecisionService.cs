using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.HR;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface ISelectionDecisionService
    {
         Task<IReadOnlyList<SelectionDecision>> RegisterSelections(ICollection<SelectionDecisionToRegisterDto> selectionDecisions);
         Task<bool> EditSelection(SelectionDecision selectionDecision);
         Task<bool> DeleteSelection(SelectionDecision selectionDecision);
         
    }
}