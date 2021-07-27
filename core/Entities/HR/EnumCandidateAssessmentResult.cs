using System.Runtime.Serialization;

namespace core.Entities.HR
{
    public enum EnumCandidateAssessmentResult
    {
        [EnumMember(Value="Grade A+ - Excellent - 80%+")]
        Excellent,
        [EnumMember(Value="Grade A - Very Good - 70%+")]
        VeryGood,
        [EnumMember(Value="Grade B - Good - 60%+")]
        Good,
        [EnumMember(Value="Grade C - Poor - 50%+")]
        Poor,
        [EnumMember(Value="Grade D - Very Poor - 49%-")]
        VeryPoor
    }
}