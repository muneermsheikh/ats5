using System;
using System.Runtime.Serialization;

namespace core.Entities.Orders
{
    public enum EnumReviewItemStatus
    {
        [EnumMember(Value="Not Reviewed")]
        NotReviewed, 
        [EnumMember(Value="Accepted")]
        Accepted,
        [EnumMember(Value="Declined - Salary Not Feasible")]
        SalaryNotFeasible,
        [EnumMember(Value="Declined - Visa availability uncertain")]
        VisaAvailabilityUncertain,
        [EnumMember(Value="Negative Background report on customer")]
        NegativeBackgroundReport

    }
}