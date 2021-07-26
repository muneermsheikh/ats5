using System.Runtime.Serialization;

namespace core.Entities.Admin
{
    public enum EnumCustomerStatus
    {
        [EnumMember(Value="Active")]
        Active,
        [EnumMember(Value="Closed")]
        Closed,
        [EnumMember(Value="Blacklisted")]
        Blacklisted
    }
}