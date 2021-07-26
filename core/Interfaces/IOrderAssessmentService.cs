using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.MasterEntities;
using core.Entities.Orders;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IOrderAssessmentService
    {
        //stddASsessment
        Task<bool> AddStddQs(ICollection<AssessmentQBank> Qs);
        Task<bool> EditStddQs(ICollection<AssessmentQBank> qs);
        Task<bool> DeleteStddQ(AssessmentQBank Q);
        Task<OrderItemAssessment> CopyStddQToOrderAssessmentItem(int orderitemid);
        Task<IReadOnlyList<AssessmentQBank>> GetAssessmentQsFromBankBySubject (AssessmentStddQsParams qsParams);
        
        //orderAssessment
        Task<OrderItemAssessment> GetOrderAssessmentItemQs (int OrderItemId);
        //Task<OrderAssessment> GetOrderAssessmentAsync (int orderId);
        
        Task<bool> EditOrderAssessmentItem(OrderItemAssessment assessmentItem);
        Task<bool> DeleteAssessmentItemQ(int orderitemid);
    }
}