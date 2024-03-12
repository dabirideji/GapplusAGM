using BarcodeGenerator.Models;
using BarcodeGenerator.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Service
{
    public class AuthenticationService
    {
        private UsersContext db;

        public AuthenticationService(UsersContext _db)
        {
            db = _db;
            ua = new UserAdmin(db);
            agmM = new AGMManager(db);
            _messages = new MessageService(db);
            _presentservice = new PresentService(db);
        }
        Stopwatch stopwatch = new Stopwatch();
        UserAdmin ua;
        AGMManager agmM;
        MessageService _messages;
        PresentService _presentservice;

        private static string currentYear = DateTime.Now.Year.ToString();

        private static string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection conn =
                  new SqlConnection(connStr);

        Dictionary<string, string> resourceTypes = new Dictionary<string, string>(){
                                        {"Shareholder", "Shareholder"},
                                        {"Non-Shareholder", "Facilitator"}
        };




        public async Task<APIMessageLog> CoronationVirtualTokenAuthentication(string company, string email, string Token)
        {
            int agmid;
            APIMessageLog MessageLog;
            var abstainbtnchoice = true;
            AppLog log;
            try
            {


                if (!string.IsNullOrEmpty(company) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(Token))
                {
                    var forBg = "green";
                    var againstBg = "red";
                    var abstainBg = "blue";
                    var voidBg = "black";
                    //VoteModel query = (VoteModel)decryptedtext;
                    var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
                    if (UniqueAGMId == -1)
                    {
                        MessageLog = new APIMessageLog()
                        {
                            Status = "Error",
                            ResponseCode = "205",
                            ResponseMessage = "AGM event not active at the moment.",
                            EventTime = DateTime.Now
                        };
                        log = new AppLog();
                        log.Status = MessageLog.Status;
                        log.ResponseCode = MessageLog.ResponseCode;
                        log.ResponseMessage = MessageLog.ResponseMessage;
                        log.EventTime = MessageLog.EventTime;
                        db.AppLogs.Add(log);
                        db.SaveChanges();
                        return MessageLog;
                    }
                    var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                    if (AgmEvent == null)
                    {
                        MessageLog = new APIMessageLog()
                        {
                            Status = "Error",
                            ResponseCode = "205",
                            ResponseMessage = "AGM event not active at the moment.",
                            EventTime = DateTime.Now
                        };
                        log = new AppLog();
                        log.Status = MessageLog.Status;
                        log.ResponseCode = MessageLog.ResponseCode;
                        log.ResponseMessage = MessageLog.ResponseMessage;
                        log.EventTime = MessageLog.EventTime;
                        db.AppLogs.Add(log);
                        db.SaveChanges();
                        return MessageLog;

                    }
                    if (!string.IsNullOrEmpty(AgmEvent.VoteForColorBg))
                    {
                        forBg = AgmEvent.VoteForColorBg;
                    }
                    if (!string.IsNullOrEmpty(AgmEvent.VoteAgainstColorBg))
                    {
                        againstBg = AgmEvent.VoteAgainstColorBg;
                    }
                    if (!string.IsNullOrEmpty(AgmEvent.VoteAbstaincolorBg))
                    {
                        abstainBg = AgmEvent.VoteAbstaincolorBg;
                    }
                    if (!string.IsNullOrEmpty(AgmEvent.VoteVoidColorBg))
                    {
                        voidBg = AgmEvent.VoteVoidColorBg;
                    }
                    if (AgmEvent.AbstainBtnChoice != null)
                    {
                        abstainbtnchoice = (bool)AgmEvent.AbstainBtnChoice;
                    }
                    company = AgmEvent.CompanyName;

                    //Check for shareholder Email in db
                    var CheckTokenRecord = db.BarcodeStore.FirstOrDefault(u => u.Company.ToLower() == company.ToLower() && u.emailAddress.ToLower() == email.ToLower() && u.passwordToken == Token.Trim());
                    if (CheckTokenRecord == null)
                    {
                        MessageLog = new APIMessageLog()
                        {
                            Status = "Error",
                            ResponseCode = "205",
                            ResponseMessage = "Token information can not be authenticated.",
                            EventTime = DateTime.Now
                        };
                        log = new AppLog();
                        log.Status = MessageLog.Status;
                        log.ResponseCode = MessageLog.ResponseCode;
                        log.ResponseMessage = MessageLog.ResponseMessage;
                        log.EventTime = MessageLog.EventTime;
                        db.AppLogs.Add(log);
                        db.SaveChanges();
                        return MessageLog;
                    }
                    BarcodeModel cshareholderRecord;
                    PresentModel shareholder;
                    var accountResponse = await ua.GetShareHolderAccount(company, email);
                    var presentResponse = await _presentservice.MarkShareholderVirtuallyPresentAsync(company, email);
                    if (!accountResponse.Status)
                    {
                        MessageLog = new APIMessageLog()
                        {
                            Status = "Error",
                            ResponseCode = accountResponse.Code.ToString(),
                            ResponseMessage = accountResponse.Message,
                            EventTime = DateTime.Now
                        };
                        log = new AppLog();
                        log.Status = MessageLog.Status;
                        log.ResponseCode = MessageLog.ResponseCode;
                        log.ResponseMessage = MessageLog.ResponseMessage;
                        log.EventTime = MessageLog.EventTime;
                        db.AppLogs.Add(log);
                        db.SaveChanges();

                        return MessageLog;
                    }

                    if (!presentResponse.Status)
                    {
                        MessageLog = new APIMessageLog()
                        {
                            Status = "Error",
                            ResponseCode = presentResponse.Code.ToString(),
                            ResponseMessage = presentResponse.Message,
                            EventTime = DateTime.Now
                        };
                        log = new AppLog();
                        log.Status = MessageLog.Status;
                        log.ResponseCode = MessageLog.ResponseCode;
                        log.ResponseMessage = MessageLog.ResponseMessage;
                        log.EventTime = MessageLog.EventTime;
                        db.AppLogs.Add(log);
                        db.SaveChanges();

                        return MessageLog;
                    }
                    cshareholderRecord = accountResponse.Data;

                    shareholder = presentResponse.Data;


                    var resolutions = from question in db.Question
                                      where question.AGMID == UniqueAGMId
                                      select question;

                    MessageLog = new APIMessageLog()
                    {
                        Status = "successfully Accredited",
                        ResponseCode = "200",
                        ResponseMessage = "",
                        EventTime = DateTime.Now,
                        EventUrl = AgmEvent.OnlineUrllink,
                        EventStatus = AgmEvent.AgmStart,
                        shareholder = shareholder,
                        UserProxyStatus = cshareholderRecord.PresentByProxy,
                        AGMTitle = AgmEvent.Description,
                        MessagingChoice = AgmEvent.MessagingChoice,
                        allChannel = AgmEvent.allChannels,
                        webChannel = AgmEvent.webChannel,
                        mobileChannel = AgmEvent.mobileChannel,
                        AGMID = UniqueAGMId,
                        Messages = await _messages.GetAllQuestions(UniqueAGMId)
                    };
                    //MessageLog = new APIMessageLog()
                    //{
                    //    Status = "Successful Login",
                    //    ResponseCode = "200",
                    //    ResponseMessage = "",
                    //    EventTime = DateTime.Now,
                    //    Rsolutions = resolutions.ToList(),
                    //    AGMID = UniqueAGMId,
                    //    abstainBtnChoice = abstainbtnchoice,
                    //    AGMTitle = AgmEvent.Description,
                    //    PreregisterationShareholder = cshareholderRecord,
                    //    shareholder = shareholder,
                    //    forBg = forBg,
                    //    againstBg = againstBg,
                    //    abstainBg = abstainBg,
                    //    voidBg = voidBg,
                    //    Companylogo = AgmEvent.Image != null ? "data:image/jpg;base64," +
                    // Convert.ToBase64String((byte[])AgmEvent.Image) : "",
                    //    //VotingResult = db.Result.Where(r => r.ShareholderNum == cshareholderRecord.ShareholderNum && r.AGMID == UniqueAGMId).ToList()
                    //};
                    log = new AppLog();
                    log.Status = MessageLog.Status;
                    log.ResponseCode = MessageLog.ResponseCode;
                    log.ResponseMessage = MessageLog.ResponseMessage;
                    log.EventTime = MessageLog.EventTime;
                    db.AppLogs.Add(log);
                    db.SaveChanges();
                    return MessageLog;



                }
                else
                {
                    MessageLog = new APIMessageLog()
                    {
                        Status = "Error",
                        ResponseCode = "205",
                        ResponseMessage = "Token information may not be correct.",
                        EventTime = DateTime.Now
                    };
                    log = new AppLog();
                    log.Status = MessageLog.Status;
                    log.ResponseCode = MessageLog.ResponseCode;
                    log.ResponseMessage = MessageLog.ResponseMessage;
                    log.EventTime = MessageLog.EventTime;
                    db.AppLogs.Add(log);
                    db.SaveChanges();
                    return MessageLog;
                }

            }
            catch (Exception e)
            {
                MessageLog = new APIMessageLog()
                {
                    Status = "Error",
                    ResponseCode = "205",
                    ResponseMessage = "The system is unable to process the request at the moment. Please try again later.",
                    EventTime = DateTime.Now
                };
                log = new AppLog();
                log.Status = MessageLog.Status;
                log.ResponseCode = MessageLog.ResponseCode;
                log.ResponseMessage = MessageLog.ResponseMessage;
                log.EventTime = MessageLog.EventTime;
                db.AppLogs.Add(log);
                db.SaveChanges();
                return MessageLog;
            }
        }
    }
}