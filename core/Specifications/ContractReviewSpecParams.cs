namespace core.Specifications
{
    public class ContractReviewSpecParams: CommonSpecParams
    {
        public int? OrderItemId { get; set; }
        public int? OrderId { get; set; }
        public int? CustomerId {get; set;}
    }
}