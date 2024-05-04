using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace Gapplus.Domain.Models.Base;

   [Keyless]
    public class MeetingSettings
    {
    public bool host_video { get; set; }
    public bool participant_video { get; set; }
    public bool cn_meeting { get; set; }
    public bool in_meeting { get; set; }
    public bool join_before_host { get; set; }
    public int jbh_time { get; set; }
    public bool mute_upon_entry { get; set; }
    public bool watermark { get; set; }
    public bool use_pmi { get; set; }
    public int approval_type { get; set; }
    public string audio { get; set; }
    public string auto_recording { get; set; }
    public bool enforce_login { get; set; }
    public string enforce_login_domains { get; set; }
    public string alternative_hosts { get; set; }
    public bool alternative_host_update_polls { get; set; }
    public bool close_registration { get; set; }
    public bool show_share_button { get; set; }
    public bool allow_multiple_devices { get; set; }
    public bool registrants_confirmation_email { get; set; }
    public bool waiting_room { get; set; }
    public bool request_permission_to_unmute_participants { get; set; }
    public bool registrants_email_notification { get; set; }
    public bool meeting_authentication { get; set; }
    public string encryption_type { get; set; }
    // public bool approved_or_denied_countries_or_regions { get; set; }
    // public BreakoutRoom breakout_room { get; set; }
    // public ContinuousMeetingChat continuous_meeting_chat { get; set; }
    public bool internal_meeting { get; set; }
    public bool participant_focused_meeting { get; set; }
    public bool push_change_to_calendar { get; set; }
    // Change from List<object> to appropriate type
    public ICollection<string> resources { get; set; }
    public bool auto_start_meeting_summary { get; set; }
    public bool alternative_hosts_email_notification { get; set; }
    public bool show_join_info { get; set; }
    public bool device_testing { get; set; }
    public bool focus_mode { get; set; }
    // Change from List<object> to appropriate type
    // public SignLanguageInterpretation sign_language_interpretation { get; set; }
    public ICollection<string> meeting_invitees { get; set; }
    public bool enable_dedicated_group_chat { get; set; }
    public bool private_meeting { get; set; }
    public bool email_notification { get; set; }
    public bool host_save_video_order { get; set; }
    public bool email_in_attendee_report { get; set; }
}

public class ApprovedOrDeniedCountriesOrRegions
{
    public bool enable { get; set; }
}

public class BreakoutRoom
{
    public bool enable { get; set; }
}

public class ContinuousMeetingChat
{
    public bool enable { get; set; }
    public bool auto_add_invited_external_users { get; set; }
}

public class SignLanguageInterpretation
{
    public bool enable { get; set; }
}

    public class MeetingDetails{
    public string uuid { get; set; }
    public long id { get; set; }
    public string host_id { get; set; }
    public string host_email { get; set; }
    public string topic { get; set; }
    public int type { get; set; }
    public string status { get; set; }
    public DateTime start_time { get; set; }
    public int duration { get; set; }
    public string timezone { get; set; }
    public string agenda { get; set; }
    public DateTime created_at { get; set; }
    public string start_url { get; set; }
    public string join_url { get; set; }
    public string password { get; set; }
    public string h323_password { get; set; }
    public string pstn_password { get; set; }
    public string encrypted_password { get; set; }
    [NotMapped]
    public MeetingSettings MeetingSettings { get; set; }
    public bool pre_schedule { get; set; }
}






















































[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MeetingStatus
{
    [EnumMember(Value = "Waiting")]
    Waiting,
    [EnumMember(Value = "Started")]

    Started,
    [EnumMember(Value = "Finished")]

    Finished,
    [EnumMember(Value = "Cancelled")]

    Cancelled,
    [EnumMember(Value = "Ongoing")]

    Ongoing,
    [EnumMember(Value = "Upcoming")]

    Upcoming
}








public class Meeting
{
    [Key]
    public Guid MeetingId { get; set; } = Guid.NewGuid();

    [ForeignKey("Company")]
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    public string MeetingDetails { get; set; }
    public MeetingStatus MeetingStatus { get; set; }=MeetingStatus.Waiting;
    public DateTime MeetingCreatedAt { get; set; }
    public DateTime MeetingUpdatedAt { get; set; }
}


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ConsolidationStatus
{
    [EnumMember(Value = "Consolidated")]
    Consolidated,

    [EnumMember(Value = "Not Consolidated")]
    NotConsolidated,

    [EnumMember(Value = "Required Consolidation")]
    RequiredConsolidation,

    [EnumMember(Value = "Pending")]
    Pending
}
public class ShareHolder
{
    public Guid ShareHolderId { get; set; } = Guid.NewGuid();
    public int ShareHolderNum { get; set; } = 0;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string emailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ImageUrl { get; set; }
    public string Name
    {
        get => $"{FirstName} {LastName}";
        set
        {
            // Optionally, you can parse the value to extract first and last names
            var names = value.Split(' ');
            if (names.Length >= 2)
            {
                FirstName = names[0];
                LastName = string.Join(" ", names.Skip(1));
            }
            else
            {
                // Handle cases where the full name is not properly formatted
                FirstName = value;
                LastName = string.Empty;
            }
        }
    }
    public List<int>? Interests { get; set; }
    public ConsolidationStatus ConsolidationStatus { get; set; } = ConsolidationStatus.NotConsolidated;
    public string Password { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDisabled { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.MinValue;
    public string? Sessionid { get; set; }
    public string? SessionVersion { get; set; }

    //public string Proxyupload { get; set; }
    // [DefaultValue(false)]
    // public bool Present { get; set; } = false;
    // [DefaultValue(false)]
    // public bool PresentByProxy { get; set; } = false;
    // [DefaultValue(false)]
    // public bool Preregistered { get; set; } = false;
    // [DefaultValue(false)]
    // public bool split { get; set; } = false;
    // [DefaultValue(false)]
    // public bool resolution { get; set; } = false;
    // [DefaultValue(false)]
    // public bool? combined { get; set; } = false;
    // [DefaultValue(false)]
    // public bool TakePoll { get; set; } = false;
    // [DefaultValue(false)]
    // public bool NotVerifiable { get; set; } = false;
    // [DefaultValue(false)]
    // public bool? AddedSplitAccount { get; set; } = false;
    // [DefaultValue(0)]
    // public Int64 ParentAccountNumber { get; set; } = 0;
    // public string? password { get; set; }
    // public string passwordToken { get; set; }
    // [DefaultValue(false)]
    // public bool UserLoginHistory { get; set; } = false;
    // public string accesscode { get; set; }
    // public string? Clikapad { get; set; }
    // public string? ConsolidatedValue { get; set; }// public string? ConsolidatedParent { get; set; }

}






[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CompanyStatus
{
    [EnumMember(Value = "Active")]
    Active,

    [EnumMember(Value = "Inactive")]
    Inactive,

    [EnumMember(Value = "Suspended")]
    Suspended,

    [EnumMember(Value = "Closed")]
    Closed
}
public class Company
{
    [Key]
    public Guid CompanyId { get; set; } = Guid.NewGuid();

    public String CompanyName { get; set; }
    public String CompanyDescription { get; set; }
    public String CompanyImageUrl { get; set; }
    public String CompanyRegNo { get; set; }


    public List<String>? Tags { get; set; }
    public CompanyStatus CompanyStatus { get; set; } = CompanyStatus.Active;
    public DateTime CompanyCreatedAt { get; set; }
    public DateTime CompanyUpdatedAt { get; set; }
}





[PrimaryKey("ShareHolderId", "CompanyId")]
public class ShareHolderCompanyRelationShip
{

    public Guid ShareHolderId { get; set; }
    [ForeignKey("Company")]
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    public double Holdings { get; set; }
    public double PercentageHolding { get; set; } = 0;
}


[PrimaryKey("ShareHolderId", "MeetingId")]
public class MeetingRegistration
{

    public Guid ShareHolderId { get; set; }
    public Guid MeetingId { get; set; }
    public long MeetingNumber { get; set; }
}
