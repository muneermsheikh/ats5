using System.Collections.Generic;
using core.Entities.MasterEntities;

namespace core.Entities.Orders
{
    public class ContractReviewItem: BaseEntity
    {
        public ContractReviewItem()
        {
        }

        public ContractReviewItem(int orderItemId, int orderId, string categoryName, int quantity)
        {
            OrderId = orderId;
            OrderItemId = orderItemId;
            CategoryName = categoryName;
            Quantity = quantity;
        }
        public int ContractReviewId { get; set; }
        public int OrderId {get; set;}
        public int OrderItemId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public bool Ecnr { get; set; }
        public bool RequireAssess { get; set; }
        public string SourceFrom {get; set;}
        public int ReviewItemStatus {get; set;}=(int)EnumReviewItemStatus.NotReviewed;
        public ICollection<ReviewItem> ReviewItems {get; set;}     //copies data from ReviewItemData
        //public OrderItem OrderItem {get; set;}
    }
}