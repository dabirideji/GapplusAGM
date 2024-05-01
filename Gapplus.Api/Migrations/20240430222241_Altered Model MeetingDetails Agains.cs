using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gapplus.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlteredModelMeetingDetailsAgains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AGMQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    ShareholderName = table.Column<string>(type: "TEXT", nullable: false),
                    shareholderquestion = table.Column<string>(type: "TEXT", nullable: false),
                    ShareholderNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    holding = table.Column<double>(type: "REAL", nullable: false),
                    PercentageHolding = table.Column<double>(type: "REAL", nullable: false),
                    emailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    phoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false),
                    MessageType = table.Column<string>(type: "TEXT", nullable: false),
                    ReplyToName = table.Column<string>(type: "TEXT", nullable: false),
                    ReplyToMessage = table.Column<string>(type: "TEXT", nullable: false),
                    ReplyToEmail = table.Column<string>(type: "TEXT", nullable: false),
                    IsFeedback = table.Column<bool>(type: "INTEGER", nullable: false),
                    datetime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AGMQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    idenity = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    ResponseCode = table.Column<string>(type: "TEXT", nullable: false),
                    ResponseMessage = table.Column<string>(type: "TEXT", nullable: false),
                    EventTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AGMTitle = table.Column<string>(type: "TEXT", nullable: false),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BarcodeStore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SN = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    Holding = table.Column<double>(type: "REAL", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    PercentageHolding = table.Column<double>(type: "REAL", nullable: false),
                    ShareholderNum = table.Column<long>(type: "INTEGER", nullable: false),
                    RegCode = table.Column<int>(type: "INTEGER", nullable: false),
                    BarcodeImage = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Barcode = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    OnlineEventUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Proxyupload = table.Column<string>(type: "TEXT", nullable: false),
                    Selected = table.Column<bool>(type: "INTEGER", nullable: false),
                    Consolidated = table.Column<bool>(type: "INTEGER", nullable: false),
                    ConsolidatedValue = table.Column<string>(type: "TEXT", nullable: false),
                    ConsolidatedParent = table.Column<string>(type: "TEXT", nullable: false),
                    Present = table.Column<bool>(type: "INTEGER", nullable: false),
                    PresentByProxy = table.Column<bool>(type: "INTEGER", nullable: false),
                    Preregistered = table.Column<bool>(type: "INTEGER", nullable: false),
                    split = table.Column<bool>(type: "INTEGER", nullable: false),
                    resolution = table.Column<bool>(type: "INTEGER", nullable: false),
                    combined = table.Column<bool>(type: "INTEGER", nullable: true),
                    TakePoll = table.Column<bool>(type: "INTEGER", nullable: false),
                    NotVerifiable = table.Column<bool>(type: "INTEGER", nullable: false),
                    AddedSplitAccount = table.Column<bool>(type: "INTEGER", nullable: true),
                    emailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    ParentAccountNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    Clikapad = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    password = table.Column<string>(type: "TEXT", nullable: true),
                    passwordToken = table.Column<string>(type: "TEXT", nullable: false),
                    accesscode = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: false),
                    UserLoginHistory = table.Column<bool>(type: "INTEGER", nullable: false),
                    Sessionid = table.Column<string>(type: "TEXT", nullable: false),
                    SessionVersion = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarcodeStore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyDescription = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyRegNo = table.Column<string>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyCreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompanyUpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Facilitators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SN = table.Column<string>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FacilitatorCompany = table.Column<string>(type: "TEXT", nullable: false),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false),
                    ResourceType = table.Column<string>(type: "TEXT", nullable: false),
                    BarcodeImage = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Barcode = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    OnlineEventUrl = table.Column<string>(type: "TEXT", nullable: false),
                    emailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    accesscode = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: false),
                    UserLoginHistory = table.Column<bool>(type: "INTEGER", nullable: false),
                    Sessionid = table.Column<string>(type: "TEXT", nullable: false),
                    SessionVersion = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilitators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacilitatorsArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SN = table.Column<string>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FacilitatorCompany = table.Column<string>(type: "TEXT", nullable: false),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false),
                    ResourceType = table.Column<string>(type: "TEXT", nullable: false),
                    BarcodeImage = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Barcode = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    OnlineEventUrl = table.Column<string>(type: "TEXT", nullable: false),
                    emailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    accesscode = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitatorsArchive", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeypadResults",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false),
                    voteReceived = table.Column<string>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    TimeReceived = table.Column<string>(type: "TEXT", nullable: false),
                    Keypad = table.Column<string>(type: "TEXT", nullable: false),
                    Keyvalue = table.Column<string>(type: "TEXT", nullable: false),
                    Valuechecked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeypadResults", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mailsettings",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServerName = table.Column<string>(type: "TEXT", nullable: false),
                    smtpHost = table.Column<string>(type: "TEXT", nullable: false),
                    smtpPort = table.Column<int>(type: "INTEGER", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    SentFrom = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mailsettings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MeetingRegistrations",
                columns: table => new
                {
                    ShareHolderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MeetingId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MeetingNumber = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingRegistrations", x => new { x.ShareHolderId, x.MeetingId });
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    from = table.Column<string>(type: "TEXT", nullable: false),
                    text = table.Column<string>(type: "TEXT", nullable: false),
                    sendAt = table.Column<string>(type: "TEXT", nullable: false),
                    flash = table.Column<bool>(type: "INTEGER", nullable: false),
                    transliteration = table.Column<string>(type: "TEXT", nullable: false),
                    intermediateReport = table.Column<bool>(type: "INTEGER", nullable: false),
                    notifyUrl = table.Column<string>(type: "TEXT", nullable: false),
                    notifyContentType = table.Column<string>(type: "TEXT", nullable: false),
                    callbackData = table.Column<string>(type: "TEXT", nullable: false),
                    validityPeriod = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Present",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Holding = table.Column<double>(type: "REAL", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    PermitPoll = table.Column<byte>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    admitSource = table.Column<string>(type: "TEXT", nullable: false),
                    PercentageHolding = table.Column<double>(type: "REAL", nullable: false),
                    ShareholderNum = table.Column<long>(type: "INTEGER", nullable: false),
                    newNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    ParentNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    TakePoll = table.Column<bool>(type: "INTEGER", nullable: false),
                    split = table.Column<bool>(type: "INTEGER", nullable: false),
                    present = table.Column<bool>(type: "INTEGER", nullable: false),
                    proxy = table.Column<bool>(type: "INTEGER", nullable: false),
                    preregistered = table.Column<bool>(type: "INTEGER", nullable: false),
                    emailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Clikapad = table.Column<string>(type: "TEXT", nullable: false),
                    GivenClikapad = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReturnedClikapad = table.Column<bool>(type: "INTEGER", nullable: false),
                    PresentTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Year = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<TimeSpan>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Present", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PresentArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Holding = table.Column<double>(type: "REAL", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    admitSource = table.Column<string>(type: "TEXT", nullable: true),
                    PermitPoll = table.Column<byte>(type: "INTEGER", nullable: false),
                    PercentageHolding = table.Column<double>(type: "REAL", nullable: false),
                    ShareholderNum = table.Column<long>(type: "INTEGER", nullable: false),
                    newNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    ParentNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    TakePoll = table.Column<bool>(type: "INTEGER", nullable: false),
                    split = table.Column<bool>(type: "INTEGER", nullable: false),
                    present = table.Column<bool>(type: "INTEGER", nullable: false),
                    proxy = table.Column<bool>(type: "INTEGER", nullable: false),
                    preregistered = table.Column<bool>(type: "INTEGER", nullable: false),
                    emailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Clikapad = table.Column<string>(type: "TEXT", nullable: false),
                    GivenClikapad = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReturnedClikapad = table.Column<bool>(type: "INTEGER", nullable: false),
                    PresentTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Year = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentArchive", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proxy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Holding = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    PercentageHolding = table.Column<string>(type: "TEXT", nullable: false),
                    ShareholderNum = table.Column<string>(type: "TEXT", nullable: false),
                    newNumber = table.Column<string>(type: "TEXT", nullable: false),
                    TakePoll = table.Column<bool>(type: "INTEGER", nullable: false),
                    split = table.Column<bool>(type: "INTEGER", nullable: false),
                    present = table.Column<bool>(type: "INTEGER", nullable: false),
                    proxy = table.Column<bool>(type: "INTEGER", nullable: false),
                    emailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PresentTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proxy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProxyList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    ShareholderNum = table.Column<long>(type: "INTEGER", nullable: false),
                    Validity = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProxyList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false),
                    question = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<string>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    voteType = table.Column<string>(type: "TEXT", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    questionStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    syncStatus = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false),
                    question = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<string>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    questionStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    syncStatus = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionArchive", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<string>(type: "TEXT", nullable: false),
                    Holding = table.Column<double>(type: "REAL", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    splitValue = table.Column<long>(type: "INTEGER", nullable: false),
                    ParentNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    PercentageHolding = table.Column<double>(type: "REAL", nullable: false),
                    ShareholderNum = table.Column<long>(type: "INTEGER", nullable: false),
                    phonenumber = table.Column<string>(type: "TEXT", nullable: false),
                    Clickapad = table.Column<string>(type: "TEXT", nullable: false),
                    VoteFor = table.Column<bool>(type: "INTEGER", nullable: true),
                    VoteAgainst = table.Column<bool>(type: "INTEGER", nullable: true),
                    VoteAbstain = table.Column<bool>(type: "INTEGER", nullable: true),
                    VoteVoid = table.Column<bool>(type: "INTEGER", nullable: true),
                    VoteStatus = table.Column<string>(type: "TEXT", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Present = table.Column<bool>(type: "INTEGER", nullable: false),
                    PresentByProxy = table.Column<bool>(type: "INTEGER", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultArchive", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Venue = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    AgmStart = table.Column<bool>(type: "INTEGER", nullable: false),
                    AgmEnd = table.Column<bool>(type: "INTEGER", nullable: false),
                    StopAdmittance = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartAdmittance = table.Column<bool>(type: "INTEGER", nullable: false),
                    StopVoting = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartVoting = table.Column<bool>(type: "INTEGER", nullable: false),
                    AdmittanceDateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    proxyChannel = table.Column<bool>(type: "INTEGER", nullable: false),
                    smsChannel = table.Column<bool>(type: "INTEGER", nullable: false),
                    webChannel = table.Column<bool>(type: "INTEGER", nullable: false),
                    mobileChannel = table.Column<bool>(type: "INTEGER", nullable: false),
                    ussdChannel = table.Column<bool>(type: "INTEGER", nullable: false),
                    allChannels = table.Column<bool>(type: "INTEGER", nullable: false),
                    AgmDateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AgmEndDateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    OnlineUrllink = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    SyncChoice = table.Column<string>(type: "TEXT", nullable: false),
                    AbstainBtnChoice = table.Column<bool>(type: "INTEGER", nullable: true),
                    MessagingChoice = table.Column<bool>(type: "INTEGER", nullable: false),
                    PreregisteredVotes = table.Column<bool>(type: "INTEGER", nullable: false),
                    ProxyVoteResult = table.Column<bool>(type: "INTEGER", nullable: false),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false),
                    RegCode = table.Column<int>(type: "INTEGER", nullable: false),
                    CountDownValue = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalRecordCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ShareHolding = table.Column<double>(type: "REAL", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    ArchiveStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    Year = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PrintOutTitle = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    When = table.Column<string>(type: "TEXT", nullable: false),
                    feebackEmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    feebackCCEmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    ImageSource = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false),
                    VoteForColorBg = table.Column<string>(type: "TEXT", nullable: false),
                    VoteAgainstColorBg = table.Column<string>(type: "TEXT", nullable: false),
                    VoteAbstaincolorBg = table.Column<string>(type: "TEXT", nullable: false),
                    VoteVoidColorBg = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SettingsArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Venue = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    AgmStart = table.Column<bool>(type: "INTEGER", nullable: false),
                    AgmEnd = table.Column<bool>(type: "INTEGER", nullable: false),
                    StopAdmittance = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartAdmittance = table.Column<bool>(type: "INTEGER", nullable: false),
                    AdmittanceDateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    proxyChannel = table.Column<bool>(type: "INTEGER", nullable: false),
                    smsChannel = table.Column<bool>(type: "INTEGER", nullable: false),
                    webChannel = table.Column<bool>(type: "INTEGER", nullable: false),
                    mobileChannel = table.Column<bool>(type: "INTEGER", nullable: false),
                    ussdChannel = table.Column<bool>(type: "INTEGER", nullable: false),
                    allChannels = table.Column<bool>(type: "INTEGER", nullable: false),
                    AgmDateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AgmEndDateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    OnlineUrllink = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    SyncChoice = table.Column<string>(type: "TEXT", nullable: false),
                    AbstainBtnChoice = table.Column<bool>(type: "INTEGER", nullable: true),
                    PreregisteredVotes = table.Column<bool>(type: "INTEGER", nullable: false),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false),
                    RegCode = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalRecordCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ShareHolding = table.Column<double>(type: "REAL", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    ArchiveStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    Year = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PrintOutTitle = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    When = table.Column<string>(type: "TEXT", nullable: false),
                    feebackEmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    feebackCCEmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    ImageSource = table.Column<string>(type: "TEXT", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false),
                    VoteForColorBg = table.Column<string>(type: "TEXT", nullable: false),
                    VoteAgainstColorBg = table.Column<string>(type: "TEXT", nullable: false),
                    VoteAbstaincolorBg = table.Column<string>(type: "TEXT", nullable: false),
                    VoteVoidColorBg = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsArchive", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShareHolderCompanies",
                columns: table => new
                {
                    ShareHolderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Holdings = table.Column<double>(type: "REAL", nullable: false),
                    PercentageHolding = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareHolderCompanies", x => new { x.ShareHolderId, x.CompanyId });
                });

            migrationBuilder.CreateTable(
                name: "ShareholderFeedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    phonenumber = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    When = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareholderFeedback", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShareHolders",
                columns: table => new
                {
                    ShareHolderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ShareHolderNum = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    emailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Interests = table.Column<string>(type: "TEXT", nullable: true),
                    ConsolidationStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Sessionid = table.Column<string>(type: "TEXT", nullable: true),
                    SessionVersion = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareHolders", x => x.ShareHolderId);
                });

            migrationBuilder.CreateTable(
                name: "UploadDatabase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SN = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Holding = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    PercentageHolding = table.Column<string>(type: "TEXT", nullable: false),
                    ShareholderNum = table.Column<long>(type: "INTEGER", nullable: false),
                    emailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadDatabase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false),
                    Identity = table.Column<string>(type: "TEXT", nullable: false),
                    EmailId = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyInfo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    MeetingId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MeetingDetailsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MeetingStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    MeetingCreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MeetingUpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.MeetingId);
                    table.ForeignKey(
                        name: "FK_Meetings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    to = table.Column<string>(type: "TEXT", nullable: false),
                    messageId = table.Column<int>(type: "INTEGER", nullable: false),
                    Messagesid = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Destinations_Messages_Messagesid",
                        column: x => x.Messagesid,
                        principalTable: "Messages",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Result",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AGMID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<string>(type: "TEXT", nullable: false),
                    Holding = table.Column<double>(type: "REAL", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    splitValue = table.Column<long>(type: "INTEGER", nullable: false),
                    ParentNumber = table.Column<long>(type: "INTEGER", nullable: false),
                    PercentageHolding = table.Column<double>(type: "REAL", nullable: false),
                    ShareholderNum = table.Column<long>(type: "INTEGER", nullable: false),
                    phonenumber = table.Column<string>(type: "TEXT", nullable: false),
                    Clickapad = table.Column<string>(type: "TEXT", nullable: false),
                    VoteChoice = table.Column<string>(type: "TEXT", nullable: false),
                    VoteFor = table.Column<bool>(type: "INTEGER", nullable: true),
                    VoteAgainst = table.Column<bool>(type: "INTEGER", nullable: true),
                    VoteAbstain = table.Column<bool>(type: "INTEGER", nullable: true),
                    VoteVoid = table.Column<bool>(type: "INTEGER", nullable: true),
                    VoteStatus = table.Column<string>(type: "TEXT", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Present = table.Column<bool>(type: "INTEGER", nullable: false),
                    PresentByProxy = table.Column<bool>(type: "INTEGER", nullable: false),
                    Pregistered = table.Column<bool>(type: "INTEGER", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Result_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SMSDeliveryLog",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    to = table.Column<string>(type: "TEXT", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    smsCount = table.Column<string>(type: "TEXT", nullable: false),
                    deliveryId = table.Column<string>(type: "TEXT", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSDeliveryLog", x => x.id);
                    table.ForeignKey(
                        name: "FK_SMSDeliveryLog_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SMSResult",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    messageId = table.Column<string>(type: "TEXT", nullable: false),
                    from = table.Column<string>(type: "TEXT", nullable: false),
                    to = table.Column<string>(type: "TEXT", nullable: false),
                    text = table.Column<string>(type: "TEXT", nullable: false),
                    cleanText = table.Column<string>(type: "TEXT", nullable: false),
                    keyword = table.Column<string>(type: "TEXT", nullable: false),
                    receivedAt = table.Column<string>(type: "TEXT", nullable: false),
                    smsCount = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSResult", x => x.id);
                    table.ForeignKey(
                        name: "FK_SMSResult_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MeetingDetails",
                columns: table => new
                {
                    MeetingDetailsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MeetingId = table.Column<Guid>(type: "TEXT", nullable: false),
                    uuid = table.Column<string>(type: "TEXT", nullable: false),
                    id = table.Column<long>(type: "INTEGER", nullable: false),
                    host_id = table.Column<string>(type: "TEXT", nullable: false),
                    host_email = table.Column<string>(type: "TEXT", nullable: false),
                    topic = table.Column<string>(type: "TEXT", nullable: false),
                    type = table.Column<int>(type: "INTEGER", nullable: false),
                    status = table.Column<string>(type: "TEXT", nullable: false),
                    start_time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    duration = table.Column<int>(type: "INTEGER", nullable: false),
                    timezone = table.Column<string>(type: "TEXT", nullable: false),
                    agenda = table.Column<string>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    start_url = table.Column<string>(type: "TEXT", nullable: false),
                    join_url = table.Column<string>(type: "TEXT", nullable: false),
                    password = table.Column<string>(type: "TEXT", nullable: false),
                    h323_password = table.Column<string>(type: "TEXT", nullable: false),
                    pstn_password = table.Column<string>(type: "TEXT", nullable: false),
                    encrypted_password = table.Column<string>(type: "TEXT", nullable: false),
                    MeetingSettingsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    pre_schedule = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingDetails", x => x.MeetingDetailsId);
                    table.ForeignKey(
                        name: "FK_MeetingDetails_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "MeetingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeetingSettings",
                columns: table => new
                {
                    MeetingSettingsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MeetingDetaildId = table.Column<Guid>(type: "TEXT", nullable: false),
                    host_video = table.Column<bool>(type: "INTEGER", nullable: false),
                    participant_video = table.Column<bool>(type: "INTEGER", nullable: false),
                    cn_meeting = table.Column<bool>(type: "INTEGER", nullable: false),
                    in_meeting = table.Column<bool>(type: "INTEGER", nullable: false),
                    join_before_host = table.Column<bool>(type: "INTEGER", nullable: false),
                    jbh_time = table.Column<int>(type: "INTEGER", nullable: false),
                    mute_upon_entry = table.Column<bool>(type: "INTEGER", nullable: false),
                    watermark = table.Column<bool>(type: "INTEGER", nullable: false),
                    use_pmi = table.Column<bool>(type: "INTEGER", nullable: false),
                    approval_type = table.Column<int>(type: "INTEGER", nullable: false),
                    audio = table.Column<string>(type: "TEXT", nullable: false),
                    auto_recording = table.Column<string>(type: "TEXT", nullable: false),
                    enforce_login = table.Column<bool>(type: "INTEGER", nullable: false),
                    enforce_login_domains = table.Column<string>(type: "TEXT", nullable: false),
                    alternative_hosts = table.Column<string>(type: "TEXT", nullable: false),
                    alternative_host_update_polls = table.Column<bool>(type: "INTEGER", nullable: false),
                    close_registration = table.Column<bool>(type: "INTEGER", nullable: false),
                    show_share_button = table.Column<bool>(type: "INTEGER", nullable: false),
                    allow_multiple_devices = table.Column<bool>(type: "INTEGER", nullable: false),
                    registrants_confirmation_email = table.Column<bool>(type: "INTEGER", nullable: false),
                    waiting_room = table.Column<bool>(type: "INTEGER", nullable: false),
                    request_permission_to_unmute_participants = table.Column<bool>(type: "INTEGER", nullable: false),
                    registrants_email_notification = table.Column<bool>(type: "INTEGER", nullable: false),
                    meeting_authentication = table.Column<bool>(type: "INTEGER", nullable: false),
                    encryption_type = table.Column<string>(type: "TEXT", nullable: false),
                    internal_meeting = table.Column<bool>(type: "INTEGER", nullable: false),
                    participant_focused_meeting = table.Column<bool>(type: "INTEGER", nullable: false),
                    push_change_to_calendar = table.Column<bool>(type: "INTEGER", nullable: false),
                    resources = table.Column<string>(type: "TEXT", nullable: false),
                    auto_start_meeting_summary = table.Column<bool>(type: "INTEGER", nullable: false),
                    alternative_hosts_email_notification = table.Column<bool>(type: "INTEGER", nullable: false),
                    show_join_info = table.Column<bool>(type: "INTEGER", nullable: false),
                    device_testing = table.Column<bool>(type: "INTEGER", nullable: false),
                    focus_mode = table.Column<bool>(type: "INTEGER", nullable: false),
                    meeting_invitees = table.Column<string>(type: "TEXT", nullable: false),
                    enable_dedicated_group_chat = table.Column<bool>(type: "INTEGER", nullable: false),
                    private_meeting = table.Column<bool>(type: "INTEGER", nullable: false),
                    email_notification = table.Column<bool>(type: "INTEGER", nullable: false),
                    host_save_video_order = table.Column<bool>(type: "INTEGER", nullable: false),
                    email_in_attendee_report = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingSettings", x => x.MeetingSettingsId);
                    table.ForeignKey(
                        name: "FK_MeetingSettings_MeetingDetails_MeetingDetaildId",
                        column: x => x.MeetingDetaildId,
                        principalTable: "MeetingDetails",
                        principalColumn: "MeetingDetailsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_Messagesid",
                table: "Destinations",
                column: "Messagesid");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingDetails_MeetingId",
                table: "MeetingDetails",
                column: "MeetingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeetingDetails_MeetingSettingsId",
                table: "MeetingDetails",
                column: "MeetingSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_CompanyId",
                table: "Meetings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingSettings_MeetingDetaildId",
                table: "MeetingSettings",
                column: "MeetingDetaildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Result_QuestionId",
                table: "Result",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SMSDeliveryLog_QuestionId",
                table: "SMSDeliveryLog",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_SMSResult_QuestionId",
                table: "SMSResult",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingDetails_MeetingSettings_MeetingSettingsId",
                table: "MeetingDetails",
                column: "MeetingSettingsId",
                principalTable: "MeetingSettings",
                principalColumn: "MeetingSettingsId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingDetails_MeetingSettings_MeetingSettingsId",
                table: "MeetingDetails");

            migrationBuilder.DropTable(
                name: "AGMQuestions");

            migrationBuilder.DropTable(
                name: "AppLogs");

            migrationBuilder.DropTable(
                name: "BarcodeStore");

            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "Facilitators");

            migrationBuilder.DropTable(
                name: "FacilitatorsArchive");

            migrationBuilder.DropTable(
                name: "KeypadResults");

            migrationBuilder.DropTable(
                name: "mailsettings");

            migrationBuilder.DropTable(
                name: "MeetingRegistrations");

            migrationBuilder.DropTable(
                name: "Present");

            migrationBuilder.DropTable(
                name: "PresentArchive");

            migrationBuilder.DropTable(
                name: "Proxy");

            migrationBuilder.DropTable(
                name: "ProxyList");

            migrationBuilder.DropTable(
                name: "QuestionArchive");

            migrationBuilder.DropTable(
                name: "Result");

            migrationBuilder.DropTable(
                name: "ResultArchive");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "SettingsArchive");

            migrationBuilder.DropTable(
                name: "ShareHolderCompanies");

            migrationBuilder.DropTable(
                name: "ShareholderFeedback");

            migrationBuilder.DropTable(
                name: "ShareHolders");

            migrationBuilder.DropTable(
                name: "SMSDeliveryLog");

            migrationBuilder.DropTable(
                name: "SMSResult");

            migrationBuilder.DropTable(
                name: "UploadDatabase");

            migrationBuilder.DropTable(
                name: "UserProfile");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "MeetingSettings");

            migrationBuilder.DropTable(
                name: "MeetingDetails");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
