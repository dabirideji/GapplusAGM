using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Gapplus.Application.DTO.ZoomMeeting
{
  public class ZoomMeetingRequest
{
    public string topic { get; set; }
    public int type { get; set; }
    public string start_time { get; set; }
    public int duration { get; set; }
    public string timezone { get; set; }
    public string agenda { get; set; }
    public ZoomMeetingSettings settings { get; set; }
}

public class JoinMeetingRequest
{
    public string MeetingNumber { get; set; } // Add MeetingNumber property
    public string Role { get; set; } // Add Role property
}


public class CreateMeetingDto
{
    public string Topic { get; set; }
    public int Type { get; set; }
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public string Agenda { get; set; }
    public MeetingSettings Settings { get; set; }
}

// public class SimpleCreateMeetingDto
// {
//     public string Topic { get; set; }
//     public int? StartTimeInMinutes { get; set; }
//     public int Duration { get; set; }
//     public string Agenda { get; set; }
// }

public class SimpleCreateMeetingDto
{
    public string Topic { get; set; }
    public int? StartTimeInMinutes { get; set; }
    public int Duration { get; set; }
    public string Agenda { get; set; }
    public string Password { get; set; } // New property for meeting password
}


public class ZoomMeetingsResponseDto
{
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public string NextPageToken { get; set; }
    public List<ZoomMeetingDto> Meetings { get; set; }
}

public class PaginationParams
{
    public int PageSize { get; set; } = 30;
    public int PageNumber { get; set; } = 1;
}

public class ZoomMeetingDto
{
    [JsonProperty("uuid")]
    public string Uuid { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("host_id")]
    public string HostId { get; set; }

    [JsonProperty("topic")]
    public string Topic { get; set; }

    [JsonProperty("type")]
    public int Type { get; set; }

    [JsonProperty("start_time")]
    public DateTime? StartTime { get; set; }

    [JsonProperty("duration")]
    public int Duration { get; set; }

    [JsonProperty("timezone")]
    public string TimeZone { get; set; }

    [JsonProperty("agenda")]
    public string Agenda { get; set; }

    [JsonProperty("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonProperty("join_url")]
    public string JoinUrl { get; set; }
}

// public class ZoomMeetingDto
// {
//     public string Uuid { get; set; }
//     public long Id { get; set; }
//     public string HostId { get; set; }
//     public string Topic { get; set; }
//     public int Type { get; set; }
//     public DateTime StartTime { get; set; }
//     public int Duration { get; set; }
//     public string TimeZone { get; set; }
//     public string Agenda { get; set; }
//     public DateTime CreatedAt { get; set; }
//     public string JoinUrl { get; set; }
// }

public class UpdateZoomMeetingDto
{
    public string? Topic { get; set; }
    public int? StartTimeInMinutes { get; set; } // Minutes from now for start time
    public int? Duration { get; set; }
    public string? Agenda { get; set; }
}


// Define a class to represent the payload structure
public class ZoomSignaturePayload
{
    public string MeetingNumber { get; set; }
    public int Role { get; set; }
}

public class MeetingSettingsDto
{
    public bool HostVideo { get; set; }
    public bool ParticipantVideo { get; set; }
    public bool JoinBeforeHost { get; set; }
    public bool MuteUponEntry { get; set; }
    public bool Watermark { get; set; }
    public bool UsePmi { get; set; }
    public int ApprovalType { get; set; }
    public string Audio { get; set; }
    public string AutoRecording { get; set; }
}
public class MeetingSettings
{
    public bool? HostVideo { get; set; }
    public bool? ParticipantVideo { get; set; }
    public bool? JoinBeforeHost { get; set; }
    public bool? MuteUponEntry { get; set; }
    public bool? WaitingRoom { get; set; }
    public string[] GlobalDialInCountries { get; set; }
    public bool? RegistrantsEmailNotification { get; set; }
    public bool? RegistrantsConfirmationEmail { get; set; }
    public int? ApprovalType { get; set; }
    public string[] ApprovedOrDeniedCountriesOrRegions { get; set; }
    public bool? AlternativeHostsEmailNotification { get; set; }
    public bool? CloseRegistration { get; set; }
    public bool? UsePmi { get; set; }
    public bool? ShowShareButton { get; set; }
    public bool? AllowMultipleDevices { get; set; }
    public string[] AlternativeHosts { get; set; }
    public bool? AutoRecording { get; set; }
    public string Audio { get; set; }
    public string[] ApprovedList { get; set; }
    public string[] DeniedList { get; set; }
    public string Method { get; set; }
    public string AuthenticationOption { get; set; }
    public string[] AuthenticationDomains { get; set; }
    public string[] AuthenticationException { get; set; }
    public string AudioConferenceInfo { get; set; }
    public bool? EnforceLogin { get; set; }
    public string EnforceLoginDomains { get; set; }
    public bool? FocusMode { get; set; }
    public int? JbhTime { get; set; }
    public bool? LanguageInterpretationEnable { get; set; }
    public Interpreter[] Interpreters { get; set; }
    public bool? SignLanguageInterpretationEnable { get; set; }
    public SignLanguageInterpreter[] SignLanguageInterpreters { get; set; }
    public bool? MeetingAuthentication { get; set; }
    public bool? PrivateMeeting { get; set; }
    public int? RegistrationType { get; set; }
    public bool? Watermark { get; set; }
    public bool? InternalMeeting { get; set; }
    public bool? ParticipantFocusedMeeting { get; set; }
    public bool? PushChangeToCalendar { get; set; }
    public Resource[] Resources { get; set; }
    public bool? ContinuousMeetingChatEnable { get; set; }
    public bool? AutoAddInvitedExternalUsers { get; set; }
    public string ChannelId { get; set; }
    public bool? AutoStartMeetingSummary { get; set; }
    public bool? AutoStartAiCompanionQuestions { get; set; }
    public TrackingField[] TrackingFields { get; set; }
}

public class Interpreter
{
    public string Email { get; set; }
    public string Languages { get; set; }
}

public class SignLanguageInterpreter
{
    public string Email { get; set; }
    public string SignLanguage { get; set; }
}

public class Resource
{
    public string ResourceType { get; set; }
    public string ResourceId { get; set; }
    public string PermissionLevel { get; set; }
}

public class TrackingField
{
    public string Field { get; set; }
    public string Value { get; set; }
    public bool? Visible { get; set; }
}

public class ZoomMeetingSettings
{
    public bool host_video { get; set; }
    public bool participant_video { get; set; }
    public bool join_before_host { get; set; }
    public bool mute_upon_entry { get; set; }
    public bool watermark { get; set; }
    public bool use_pmi { get; set; }
    public int approval_type { get; set; }
    public string audio { get; set; }
    public string auto_recording { get; set; }
}

}