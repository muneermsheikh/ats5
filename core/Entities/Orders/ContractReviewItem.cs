using System.Collections.Generic;
using core.Entities.MasterEntities;

namespace core.Entities.Orders
{
    public class ContractReviewItem: BaseEntity
    {
        public ContractReviewItem()
        {
        }

        public ContractReviewItem(int orderItemId, string categoryName, int quantity)
        {
            OrderItemId = orderItemId;
            CategoryName = categoryName;
            Quantity = quantity;
        }

        public int OrderItemId { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public bool Ecnr { get; set; }
        public bool RequireAssess { get; set; }
        public string SourceFrom {get; set;}
        public EnumReviewItemStatus ReviewItemStatus {get; set;}=EnumReviewItemStatus.NotReviewed;
        public OrderItem OrderItem {get; set;}
    }
}