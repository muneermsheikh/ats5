using System.Runtime.Serialization;

namespace core.Entities.HR
{
    public enum EnumCVRefStatus
    {
        [EnumMember(Value="Referred")]
        Referred,
        [EnumMember(Value="Selected")]
        Selected,
        [EnumMember(Value="Rejected - Not Suitable")]
        RejectedNotSuitable,
        [EnumMember(Value="Rejected - Medically Unfit")]
        RejectedMedicallyUnfit,
        [EnumMember(Value="Rejected - Salary Expectation High")]
        RejectedHighSalaryExpectation,
        [EnumMember(Value="Rejected - No relevant exp")]
        RejectedNoRelevantExp,
        [EnumMember(Value="Rejected - Not qualified")]
        RejectedNotQualified,
        [EnumMember(Value="Rejected - Overage")]
        RejectedOverAge,
        [EnumMember(Value="Rejected - Not Available")]
        NotAvailable,
        [EnumMember(Value="Not Interested")]
        NotInterested
    }
}