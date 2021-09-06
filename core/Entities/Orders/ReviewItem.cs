namespace core.Entities.Orders
{
    public class ReviewItem: BaseEntity
    {
          public ReviewItem()
          {
          }

          public int OrderItemId { get; set; }
        public int ContractReviewItemId { get; set; }
        public int SrNo { get; set; }
        public string ReviewParameter { get; set; }
        public bool Response { get; set; }
        public bool IsMandatoryTrue { get; set; }=false;
        public string Remarks { get; set; }

    }
}