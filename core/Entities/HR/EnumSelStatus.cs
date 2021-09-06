using System.Runtime.Serialization;

namespace core.Entities.HR
{
    public enum EnumSelStatus
    {
        
        [EnumMember(Value="Not Reviewed")]
        NotReviewed=100,
        
        [EnumMember(Value="Selected")]
        Selected=200,
        [EnumMember(Value="Rejected - Not Suitable")]
        RejectedNotSuitable=300,
        [EnumMember(Value="Rejected - Medically Unfit")]
        RejectedMedicallyUnfit=400,
        [EnumMember(Value="Rejected - Salary Expectation High")]
        RejectedHighSalaryExpectation=500,
        [EnumMember(Value="Rejected - No relevant exp")]
        RejectedNoRelevantExp=600,
        [EnumMember(Value="Rejected - Not qualified")]
        RejectedNotQualified=700,
        [EnumMember(Value="Rejected - Overage")]
        RejectedOverAge=800,
        [EnumMember(Value="Rejected - Not Available")]
        NotAvailable=900,
        [EnumMember(Value="Not Interested")]
        NotInterested=1000
    }
}