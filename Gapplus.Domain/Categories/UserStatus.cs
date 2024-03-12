using System.Runtime.Serialization;

namespace Gapplus.Domain.Categories
{
    public enum UserStatus
    {
        [EnumMember(Value = "Active")]
        Active,
        [EnumMember(Value = "Disabled")]
        Disabled,

        [EnumMember(Value = "Restricted")]
        Restricted,

        [EnumMember(Value = "Suspended")]
        Suspended,

        [EnumMember(Value = "Deactivated")]
        Deactivated,

        [EnumMember(Value = "Deleted")]
        Deleted,
    }
}
