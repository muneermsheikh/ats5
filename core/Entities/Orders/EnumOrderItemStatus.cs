using System.Runtime.Serialization;

namespace core.Entities.Orders
{
    public enum EnumOrderItemStatus
    {
        [EnumMember(Value="Awaiting Review")]
        AwaitingReview,
        [EnumMember(Value="Accepted")]
        ReviewedAndAccepted,
        [EnumMember(Value="decline - salary not feasible")]
        DeclinedSalaryNotFeasible,
        [EnumMember(Value="Visa availability uncertain")]
        DeclinedVisaAvailabilityUncertain,
        [EnumMember(Value="Negative Historical Status")]
        DeclinedHistoricalStatusNegative,
        [EnumMember(Value="Background report negative")]
        DeclinedBackgroundReportNegative,
        [EnumMember(Value="Concluded")]
        Concluded,
        [EnumMember(Value="Canceled")]
        Canceled
        
    }
}