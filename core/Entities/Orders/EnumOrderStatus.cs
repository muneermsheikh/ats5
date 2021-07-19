using System.Runtime.Serialization;

namespace core.Entities.Orders
{
    public enum EnumOrderStatus
    {
        [EnumMember(Value="Awaiting Review")]
        AwaitingReview, 
        [EnumMember(Value="Reviewed and Accepted")]
        ReviewedAndAccepted,
        [EnumMember(Value="Reviewed and declined")]
        ReviewedAndDeclined,
        [EnumMember(Value="Assigned to HR")]
        AssignedToHR,
        [EnumMember(Value="Concluded")]
        Concluded
        
    }
}