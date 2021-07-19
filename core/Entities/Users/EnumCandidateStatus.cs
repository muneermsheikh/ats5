using System.Runtime.Serialization;

namespace core.Entities.Users
{
    public enum EnumCandidateStatus
    {
        [EnumMember(Value="Not Referred")]
        NotReferred, 
        [EnumMember(Value="Referred, awaiting selection")]
        Referred,
        [EnumMember(Value="Selected, deployment in process")]
        Selected,
        [EnumMember(Value="Traveled, currently overseas")]
        Traveled,
        [EnumMember(Value="Not Available")]
        NotAvailable
        
    }
}