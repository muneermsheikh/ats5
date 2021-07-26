using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities.MasterEntities;
using core.Entities.Orders;
using core.ParamsAndDtos;
using core.Specifications;

namespace core.Interfaces
{
    public interface IContractReviewService
    {
        Task<bool> CreateContractReview(ContractReview cReview);
        Task<IReadOnlyList<ContractReviewItemDto>> GetContractReviewItemsAsync(ContractReviewSpecParams param);
        Task<IReadOnlyList<ContractReviewItemDto>> GetContractReviewItemsByOrderIdAsync(int orderid);
        Task<ContractReview> GetContractReview(int orderId);
        void EditContractReview (ContractReview contractReview);
        Task<bool> DeleteContractReview(int orderid);
        Task<bool> DeleteContractReviewItem(int orderitemid);

        //Task<ContractReview> GetContractReview(int orderId);
        //void EditContractReview (ContractReview review);
        void AddReviewStatus (string reviewStatusName);
        void AddReviewItemStatus (string reviewItemStatusName);
        Task<ICollection<ReviewStatus>> GetReviewStatus();
        Task<IReadOnlyList<ReviewItemStatus>> GetReviewItemStatus();
        Task<ContractReviewItemDto> GetContractReviewItemWithOrderDetails(int orderItemId);
    }
}