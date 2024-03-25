using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gapplus.Api.Migrations
{
    /// <inheritdoc />
    public partial class RollbackandRemigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AGMQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShareholderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shareholderquestion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShareholderNumber = table.Column<long>(type: "bigint", nullable: false),
                    holding = table.Column<double>(type: "float", nullable: false),
                    PercentageHolding = table.Column<double>(type: "float", nullable: false),
                    emailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AGMID = table.Column<int>(type: "int", nullable: false),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReplyToName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReplyToMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReplyToEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFeedback = table.Column<bool>(type: "bit", nullable: false),
                    datetime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AGMQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idenity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AGMTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AGMID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BarcodeStore",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SN = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Holding = table.Column<double>(type: "float", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PercentageHolding = table.Column<double>(type: "float", nullable: false),
                    ShareholderNum = table.Column<long>(type: "bigint", nullable: false),
                    RegCode = table.Column<int>(type: "int", nullable: false),
                    BarcodeImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OnlineEventUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proxyupload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Selected = table.Column<bool>(type: "bit", nullable: false),
                    Consolidated = table.Column<bool>(type: "bit", nullable: false),
                    ConsolidatedValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsolidatedParent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Present = table.Column<bool>(type: "bit", nullable: false),
                    PresentByProxy = table.Column<bool>(type: "bit", nullable: false),
                    Preregistered = table.Column<bool>(type: "bit", nullable: false),
                    split = table.Column<bool>(type: "bit", nullable: false),
                    resolution = table.Column<bool>(type: "bit", nullable: false),
                    combined = table.Column<bool>(type: "bit", nullable: true),
                    TakePoll = table.Column<bool>(type: "bit", nullable: false),
                    NotVerifiable = table.Column<bool>(type: "bit", nullable: false),
                    AddedSplitAccount = table.Column<bool>(type: "bit", nullable: true),
                    emailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentAccountNumber = table.Column<long>(type: "bigint", nullable: false),
                    Clikapad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    passwordToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accesscode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserLoginHistory = table.Column<bool>(type: "bit", nullable: false),
                    Sessionid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionVersion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BarcodeStore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyStatus = table.Column<int>(type: "int", nullable: false),
                    CompanyAddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "Facilitators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacilitatorCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AGMID = table.Column<int>(type: "int", nullable: false),
                    ResourceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BarcodeImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OnlineEventUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    emailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accesscode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserLoginHistory = table.Column<bool>(type: "bit", nullable: false),
                    Sessionid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionVersion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilitators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FacilitatorsArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacilitatorCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AGMID = table.Column<int>(type: "int", nullable: false),
                    ResourceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BarcodeImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OnlineEventUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    emailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accesscode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitatorsArchive", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeypadResults",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AGMID = table.Column<int>(type: "int", nullable: false),
                    voteReceived = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeReceived = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Keypad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Keyvalue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valuechecked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeypadResults", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mailsettings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    smtpHost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    smtpPort = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentFrom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mailsettings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    from = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sendAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    flash = table.Column<bool>(type: "bit", nullable: false),
                    transliteration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    intermediateReport = table.Column<bool>(type: "bit", nullable: false),
                    notifyUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    notifyContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    callbackData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    validityPeriod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Present",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AGMID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Holding = table.Column<double>(type: "float", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermitPoll = table.Column<byte>(type: "tinyint", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    admitSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PercentageHolding = table.Column<double>(type: "float", nullable: false),
                    ShareholderNum = table.Column<long>(type: "bigint", nullable: false),
                    newNumber = table.Column<long>(type: "bigint", nullable: false),
                    ParentNumber = table.Column<long>(type: "bigint", nullable: false),
                    TakePoll = table.Column<bool>(type: "bit", nullable: false),
                    split = table.Column<bool>(type: "bit", nullable: false),
                    present = table.Column<bool>(type: "bit", nullable: false),
                    proxy = table.Column<bool>(type: "bit", nullable: false),
                    preregistered = table.Column<bool>(type: "bit", nullable: false),
                    emailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clikapad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GivenClikapad = table.Column<bool>(type: "bit", nullable: false),
                    ReturnedClikapad = table.Column<bool>(type: "bit", nullable: false),
                    PresentTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Present", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PresentArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AGMID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Holding = table.Column<double>(type: "float", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    admitSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PermitPoll = table.Column<byte>(type: "tinyint", nullable: false),
                    PercentageHolding = table.Column<double>(type: "float", nullable: false),
                    ShareholderNum = table.Column<long>(type: "bigint", nullable: false),
                    newNumber = table.Column<long>(type: "bigint", nullable: false),
                    ParentNumber = table.Column<long>(type: "bigint", nullable: false),
                    TakePoll = table.Column<bool>(type: "bit", nullable: false),
                    split = table.Column<bool>(type: "bit", nullable: false),
                    present = table.Column<bool>(type: "bit", nullable: false),
                    proxy = table.Column<bool>(type: "bit", nullable: false),
                    preregistered = table.Column<bool>(type: "bit", nullable: false),
                    emailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clikapad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GivenClikapad = table.Column<bool>(type: "bit", nullable: false),
                    ReturnedClikapad = table.Column<bool>(type: "bit", nullable: false),
                    PresentTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentArchive", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proxy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Holding = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PercentageHolding = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShareholderNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    newNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TakePoll = table.Column<bool>(type: "bit", nullable: false),
                    split = table.Column<bool>(type: "bit", nullable: false),
                    present = table.Column<bool>(type: "bit", nullable: false),
                    proxy = table.Column<bool>(type: "bit", nullable: false),
                    emailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PresentTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Timestamp = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proxy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProxyList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShareholderNum = table.Column<long>(type: "bigint", nullable: false),
                    Validity = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProxyList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AGMID = table.Column<int>(type: "int", nullable: false),
                    question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    voteType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    questionStatus = table.Column<bool>(type: "bit", nullable: false),
                    syncStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AGMID = table.Column<int>(type: "int", nullable: false),
                    question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    questionStatus = table.Column<bool>(type: "bit", nullable: false),
                    syncStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionArchive", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AGMID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Holding = table.Column<double>(type: "float", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    splitValue = table.Column<long>(type: "bigint", nullable: false),
                    ParentNumber = table.Column<long>(type: "bigint", nullable: false),
                    PercentageHolding = table.Column<double>(type: "float", nullable: false),
                    ShareholderNum = table.Column<long>(type: "bigint", nullable: false),
                    phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clickapad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteFor = table.Column<bool>(type: "bit", nullable: true),
                    VoteAgainst = table.Column<bool>(type: "bit", nullable: true),
                    VoteAbstain = table.Column<bool>(type: "bit", nullable: true),
                    VoteVoid = table.Column<bool>(type: "bit", nullable: true),
                    VoteStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<TimeSpan>(type: "time", nullable: false),
                    Present = table.Column<bool>(type: "bit", nullable: false),
                    PresentByProxy = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultArchive", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgmStart = table.Column<bool>(type: "bit", nullable: false),
                    AgmEnd = table.Column<bool>(type: "bit", nullable: false),
                    StopAdmittance = table.Column<bool>(type: "bit", nullable: false),
                    StartAdmittance = table.Column<bool>(type: "bit", nullable: false),
                    StopVoting = table.Column<bool>(type: "bit", nullable: false),
                    StartVoting = table.Column<bool>(type: "bit", nullable: false),
                    AdmittanceDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    proxyChannel = table.Column<bool>(type: "bit", nullable: false),
                    smsChannel = table.Column<bool>(type: "bit", nullable: false),
                    webChannel = table.Column<bool>(type: "bit", nullable: false),
                    mobileChannel = table.Column<bool>(type: "bit", nullable: false),
                    ussdChannel = table.Column<bool>(type: "bit", nullable: false),
                    allChannels = table.Column<bool>(type: "bit", nullable: false),
                    AgmDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AgmEndDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OnlineUrllink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SyncChoice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbstainBtnChoice = table.Column<bool>(type: "bit", nullable: true),
                    MessagingChoice = table.Column<bool>(type: "bit", nullable: false),
                    PreregisteredVotes = table.Column<bool>(type: "bit", nullable: false),
                    ProxyVoteResult = table.Column<bool>(type: "bit", nullable: false),
                    AGMID = table.Column<int>(type: "int", nullable: false),
                    RegCode = table.Column<int>(type: "int", nullable: false),
                    CountDownValue = table.Column<int>(type: "int", nullable: false),
                    TotalRecordCount = table.Column<int>(type: "int", nullable: false),
                    ShareHolding = table.Column<double>(type: "float", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArchiveStatus = table.Column<bool>(type: "bit", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrintOutTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    When = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    feebackEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    feebackCCEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    VoteForColorBg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteAgainstColorBg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteAbstaincolorBg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteVoidColorBg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SettingsArchive",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgmStart = table.Column<bool>(type: "bit", nullable: false),
                    AgmEnd = table.Column<bool>(type: "bit", nullable: false),
                    StopAdmittance = table.Column<bool>(type: "bit", nullable: false),
                    StartAdmittance = table.Column<bool>(type: "bit", nullable: false),
                    AdmittanceDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    proxyChannel = table.Column<bool>(type: "bit", nullable: false),
                    smsChannel = table.Column<bool>(type: "bit", nullable: false),
                    webChannel = table.Column<bool>(type: "bit", nullable: false),
                    mobileChannel = table.Column<bool>(type: "bit", nullable: false),
                    ussdChannel = table.Column<bool>(type: "bit", nullable: false),
                    allChannels = table.Column<bool>(type: "bit", nullable: false),
                    AgmDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AgmEndDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OnlineUrllink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SyncChoice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AbstainBtnChoice = table.Column<bool>(type: "bit", nullable: true),
                    PreregisteredVotes = table.Column<bool>(type: "bit", nullable: false),
                    AGMID = table.Column<int>(type: "int", nullable: false),
                    RegCode = table.Column<int>(type: "int", nullable: false),
                    TotalRecordCount = table.Column<int>(type: "int", nullable: false),
                    ShareHolding = table.Column<double>(type: "float", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArchiveStatus = table.Column<bool>(type: "bit", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrintOutTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    When = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    feebackEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    feebackCCEmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    VoteForColorBg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteAgainstColorBg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteAbstaincolorBg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteVoidColorBg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingsArchive", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShareholderFeedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    When = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareholderFeedback", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadDatabase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Holding = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PercentageHolding = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShareholderNum = table.Column<long>(type: "bigint", nullable: false),
                    emailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadDatabase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Identity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyInfo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    messageId = table.Column<int>(type: "int", nullable: false),
                    Messagesid = table.Column<int>(type: "int", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AGMID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Holding = table.Column<double>(type: "float", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    splitValue = table.Column<long>(type: "bigint", nullable: false),
                    ParentNumber = table.Column<long>(type: "bigint", nullable: false),
                    PercentageHolding = table.Column<double>(type: "float", nullable: false),
                    ShareholderNum = table.Column<long>(type: "bigint", nullable: false),
                    phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clickapad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteChoice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteFor = table.Column<bool>(type: "bit", nullable: true),
                    VoteAgainst = table.Column<bool>(type: "bit", nullable: true),
                    VoteAbstain = table.Column<bool>(type: "bit", nullable: true),
                    VoteVoid = table.Column<bool>(type: "bit", nullable: true),
                    VoteStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Timestamp = table.Column<TimeSpan>(type: "time", nullable: false),
                    Present = table.Column<bool>(type: "bit", nullable: false),
                    PresentByProxy = table.Column<bool>(type: "bit", nullable: false),
                    Pregistered = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
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
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    smsCount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    deliveryId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: true)
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
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    messageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    from = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    to = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cleanText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    keyword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    receivedAt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    smsCount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_Messagesid",
                table: "Destinations",
                column: "Messagesid");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AGMQuestions");

            migrationBuilder.DropTable(
                name: "AppLogs");

            migrationBuilder.DropTable(
                name: "BarcodeStore");

            migrationBuilder.DropTable(
                name: "Companies");

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
                name: "ShareholderFeedback");

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
        }
    }
}
