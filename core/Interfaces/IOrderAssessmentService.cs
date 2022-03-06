using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.EmailandSMS;
using core.Entities.HR;
using core.Entities.MasterEntities;
using core.Entities.Orders;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IOrderAssessmentService
    {
        //stddASsessment
        Task<OrderItemAssessment> CopyStddQToOrderAssessmentItem(int orderitemid);
        Task<IReadOnlyList<AssessmentQBank>> GetAssessmentQsFromBankBySubject (AssessmentStddQsParams qsParams);
        
        //orderAssessment
        Task<OrderItemAssessment> GetOrderAssessmentItemQs (int OrderItemId);
        //Task<OrderAssessment> GetOrderAssessmentAsync (int orderId);
        Task<bool> EditOrderAssessmentItem(OrderItemAssessment assessmentItem);
        Task<bool> DeleteAssessmentItemQ(int orderitemid);

        
    }
}