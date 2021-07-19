using System.Runtime.Serialization;

namespace core.Entities.HR
{
    public enum EnumCandidateStatus
    {
         [EnumMember(Value="Not Referred")]
         NotReferred,
          [EnumMember(Value="Referred")]
         Referred,
          [EnumMember(Value="Selected")]
         Selected,
          [EnumMember(Value="Not available")]
         NotAvailable,
          [EnumMember(Value="Not Interested")]
         NotInterested
    }
}