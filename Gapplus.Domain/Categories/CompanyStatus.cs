using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Gapplus.Domain.Categories
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CompanyStatus
    {
        [EnumMember(Value = "Active")]
        Active,

        [EnumMember(Value = "Restricted")]
        Restricted,

        [EnumMember(Value = "Suspended")]
        Suspended,

        [EnumMember(Value = "Locked")]
        Locked,

        [EnumMember(Value = "FilledUp")]
        FilledUp,
        [EnumMember(Value = "Disabled")]
        Disabled
    }
}
