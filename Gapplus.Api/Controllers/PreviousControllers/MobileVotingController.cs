using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using BarcodeGenerator.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BarcodeGenerator.Controllers
{
    public class MobileVotingController : Controller
    {
        UsersContext db = new UsersContext();
        Stopwatch stopwatch = new Stopwatch();
        UserAdmin ua = new UserAdmin();
        AGMManager agmM = new AGMManager();
        WebVotingService _webVotingService = new WebVotingService();

        private static string currentYear = DateTime.Now.Year.ToString();

        private static string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        SqlConnection conn =
                  new SqlConnection(connStr);

        Dictionary<string, string> resourceTypes = new Dictionary<string, string>(){
                                        {"Shareholder", "Shareholder"},
                                        {"Non-Shareholder", "Facilitator"}
        };


        // GET: Accreditation
        //[Authorize]
        public async Task<ActionResult> Index()
        {
            var response = await IndexAsync();
            ViewBag.portal = "Welcome to AGMLive, your one-stop hybrid shareholder meeting solution";
            string changePortalText = "";
            changePortalText = TempData["Portal"] as string;
            if (!string.IsNullOrEmpty(changePortalText) || !string.IsNullOrWhiteSpace(changePortalText))
            {
                ViewBag.portal = changePortalText;
            }

            return View(response);
        }

        private Task<AccreditationResponse> IndexAsync()
        {
            try
            {
                AccreditationResponse model = new AccreditationResponse
                {
                    companies = db.Settings.Where(s => s.ArchiveStatus == false).Select(o => new AGMCompanies { company = o.CompanyName, description = o.Description, agmid = o.AGMID, venue = o.Venue, dateTime = o.AgmDateTime }).Distinct().OrderBy(k => k.company).ToList(),
                    ResourceTypes = resourceTypes
                };


                return Task.FromResult<AccreditationResponse>(model);

            }
            catch (Exception e)
            {
                return Task.FromResult<AccreditationResponse>(new AccreditationResponse());
            }

        }

        public ActionResult AccreditationIndex(string companyinfo)
        {
            var model = new accredationDto
            {
                company = companyinfo,
                accesscode = ""
            };

            return PartialView(model);
        }


        public ActionResult AGMStageLogin(string companyinfo)
        {
            ViewBag.Portal = "You have a current session on another browser.";
            //var model = new accredationDto
            //{
            //    company = companyinfo,
            //    accesscode = ""
            //};

            return View();
        }

        public ActionResult TermsCondition()
        {

            return View();
        }



        public async Task<ActionResult> JoinOngoingVotingResolution(string company, string shareholdernum)
        {
            var response = await JoinOngoingVotingResolutionAsync(company, shareholdernum);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        private Task<VotingResponseMessage> JoinOngoingVotingResolutionAsync(string companyinfo, string ShareholderNum)
        {
            try
            {
                VotingResponseMessage ResponseMessage;

                //var votingStatus = TimerControll.GetTimeStatus(companyinfo);

                if (string.IsNullOrEmpty(ShareholderNum) && string.IsNullOrEmpty(companyinfo))
                {

                    //Return error code;
                    ResponseMessage = new VotingResponseMessage()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "REQUEST CANNOT BE PROCESSED AT THE MOMENT",
                        Status = TimerControll.GetTimeStatus(companyinfo)
                    };
                    return Task.FromResult<VotingResponseMessage>(ResponseMessage);
                }
                var shareholdernum = Int64.Parse(ShareholderNum);
                var Uniagmid = ua.RetrieveAGMUniqueID(companyinfo);
                var Shareholder = db.Present.FirstOrDefault(p => p.ShareholderNum == shareholdernum);
                var CheckOtherAccounts = db.Present.Where(p => p.ShareholderNum == Shareholder.ShareholderNum).ToList();
                bool isAnyAccountProxy = false;
                if (CheckOtherAccounts != null)
                {
                    isAnyAccountProxy = CheckOtherAccounts.Any(c => c.proxy == true);
                }

                if (Shareholder == null)
                {
                    //Return error code;
                    ResponseMessage = new VotingResponseMessage()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "REQUEST CANNOT BE PROCESSED AT THE MOMENT",
                        Status = TimerControll.GetTimeStatus(companyinfo)
                    };
                    return Task.FromResult<VotingResponseMessage>(ResponseMessage);
                }

                //if (TimerControll.GetTimeStatus(companyinfo))
                //{
                var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == Uniagmid);
                if (agmEvent == null)
                {
                    //Return error code;
                    ResponseMessage = new VotingResponseMessage()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "REQUEST CANNOT BE PROCESSED AT THE MOMENT",
                        Status = TimerControll.GetTimeStatus(companyinfo)
                    };
                    return Task.FromResult<VotingResponseMessage>(ResponseMessage);
                }

                //if (Shareholder.PermitPoll == 0 && !isAnyAccountProxy && !TimerControll.GetTimeStatus(companyinfo))
                //{
                //    //Shareholder not permitted to vote.
                //    ResponseMessage = new VotingResponseMessage()
                //    {
                //        ResponseCode = "205",
                //        ResponseMessage = "RESOLUTION WILL NOT BE DISPLAYED TO YOU BECAUSE THE SYSTEM MAY HAVE ADMITTED YOU AFTER VOTING COMMENCED.",
                //        Status = TimerControll.GetTimeStatus(companyinfo)


                //    };
                //    return Task.FromResult<VotingResponseMessage>(ResponseMessage);
                //}
                //else if(Shareholder.PermitPoll == 0 && !isAnyAccountProxy && TimerControll.GetTimeStatus(companyinfo))
                //{
                ////Shareholder not permitted to vote.
                //ResponseMessage = new VotingResponseMessage()
                //{
                //    ResponseCode = "205",
                //    ResponseMessage = "RESOLUTION IS NOT DISPLAYED TO YOU BECAUSE THE SYSTEM MAY HAVE ADMITTED YOU AFTER VOTING COMMENCED.",
                //    Status = TimerControll.GetTimeStatus(companyinfo)
                //};
                //return Task.FromResult<VotingResponseMessage>(ResponseMessage);
                //}
                else if (isAnyAccountProxy && !TimerControll.GetTimeStatus(companyinfo))
                {
                    //Shareholer not permitted to vote.
                    ResponseMessage = new VotingResponseMessage()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "RESOLUTION WILL NOT BE DISPLAYED TO YOU BECAUSE SHAREHOLDER ALREADY VOTED BY PROXY, PROXY VOTES CANNOT BE CHANGED. THANK YOU YOUR PARTICIPATION.",
                        Status = TimerControll.GetTimeStatus(companyinfo)
                    };
                    return Task.FromResult<VotingResponseMessage>(ResponseMessage);
                }
                else if (isAnyAccountProxy && TimerControll.GetTimeStatus(companyinfo))
                {
                    //Shareholer not permitted to vote.
                    ResponseMessage = new VotingResponseMessage()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "RESOLUTION IS NOT DISPLAYED TO YOU BECAUSE SHAREHOLDER ALREADY VOTED BY PROXY, PROXY VOTES CANNOT BE CHANGED. THANK YOU YOUR PARTICIPATION.",
                        Status = TimerControll.GetTimeStatus(companyinfo)
                    };
                    return Task.FromResult<VotingResponseMessage>(ResponseMessage);
                }
                else if ((agmEvent.mobileChannel||agmEvent.allChannels  ) && TimerControll.GetTimeStatus(companyinfo))
                {
                    //Send Active Resolution to Client
                    var resolution = EventResolution(Uniagmid);
                    ResponseMessage = new VotingResponseMessage()
                    {
                        ResponseCode = "200",
                        ResponseMessage = resolution.question,
                        ActiveResolution = resolution.question,
                        ActiveResolutinId = resolution.Id,
                        Company = companyinfo,
                        Status = TimerControll.GetTimeStatus(companyinfo)
                    };
                    return Task.FromResult<VotingResponseMessage>(ResponseMessage);
                }
                else if ((agmEvent.mobileChannel||agmEvent.allChannels) && !TimerControll.GetTimeStatus(companyinfo))
                {
                    //Voting has not commenced.
                    ResponseMessage = new VotingResponseMessage()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "PLEASE WAIT FOR VOTING TO COMMENCE.",
                        Status = TimerControll.GetTimeStatus(companyinfo)
                    };
                    return Task.FromResult<VotingResponseMessage>(ResponseMessage);
                }
                else
                {
                    //voting not permitted on this channel
                    ResponseMessage = new VotingResponseMessage()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "VOTING HAS BEEN DISABLED ON THIS CHANNEL BY THE FACILITATOR.",
                        Status = TimerControll.GetTimeStatus(companyinfo)
                    };
                    return Task.FromResult<VotingResponseMessage>(ResponseMessage);
                }


            }
            catch (Exception e)
            {
                VotingResponseMessage ResponseMessage = new VotingResponseMessage()
                {
                    ResponseCode = "205",
                    ResponseMessage = "REQUEST CANNOT BE PROCESSED AT THE MOMENT",
                    Status = TimerControll.GetTimeStatus(companyinfo)
                };
                return Task.FromResult<VotingResponseMessage>(ResponseMessage);
            }



        }


        private VoteModel EventResolution(int agmid)
        {
            var votemodel = new VoteModel();
            var resolution = db.Question.SingleOrDefault(r => r.AGMID == agmid && r.questionStatus == true);
            if (resolution != null)
            {
                votemodel.question = resolution.question;
                votemodel.Id = resolution.Id;
            }
            return votemodel;
        }

        public async Task<ActionResult> aElapsedTime(string id)
        {
            var time = await aElapsedTimeAsync(id);
            return PartialView(time);
        }


        public Task<TimeSpan> aElapsedTimeAsync(string id)
        {
            TimeSpan time;

            if (!string.IsNullOrEmpty(id))
            {
                time = TimerControll.GetTime(id);
                return Task.FromResult<TimeSpan>(time);
            }
            time = new TimeSpan();
            return Task.FromResult<TimeSpan>(time);
        }



        public async Task<ActionResult> aElapsedTimeEventLoading(string id)
        {
            var time = await aElapsedTimeEventLoadingAsync(id);
            return PartialView(time);
        }

        public Task<TimeSpan> aElapsedTimeEventLoadingAsync(string company)
        {
            TimeSpan time = new TimeSpan();
            var currentTime = DateTime.Now;
            if (!string.IsNullOrEmpty(company))
            {
                var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
                if (UniqueAGMId != -1)
                {
                    var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                    if (AgmEvent.AgmDateTime != null)
                    {
                        if (AgmEvent.AgmDateTime == currentTime)
                        {
                            time = new TimeSpan();
                        }
                        else if (AgmEvent.AgmDateTime > currentTime)
                        {
                            time = (TimeSpan)(AgmEvent.AgmDateTime - currentTime);
                        }
                        else
                        {
                            time = new TimeSpan();
                        }

                    }

                }

                return Task.FromResult<TimeSpan>(time);
            }

            return Task.FromResult<TimeSpan>(time);
        }


        public async Task<ActionResult> Logout()
        {
            var response = await LogoutAsync();

            TempData["Portal"] = "Thank you for attending AGM";
            return RedirectToAction("Index");
        }

        private Task<List<AGMCompanies>> LogoutAsync()
        {
            try
            {
                var companyNameList = db.Settings.Where(s => s.ArchiveStatus == false).Select(o => new AGMCompanies { company = o.CompanyName, description = o.Description, agmid = o.AGMID, venue = o.Venue, dateTime = o.AgmDateTime }).Distinct().OrderBy(k => k.company).ToList();
                var session = Session["Authorize"] as APIMessageLog;
                //if (session.ResourceType != "Facilitator")
                //{
                    MobileWebVotingSessionManager.LogoutUserLoginHistory(session.shareholder.Company, session.shareholder.ShareholderNum);
                    Session.Clear();
                    return Task.FromResult<List<AGMCompanies>>(companyNameList);
                //}
                //else
                //{
                //    MobileWebVotingSessionManager.LogoutFacilitatorLoginHistory(session.facilitator.Company, session.facilitator.ShareholderNum);
                //    Session.Clear();
                //    return Task.FromResult<List<AGMCompanies>>(companyNameList);
                //}

            }
            catch (Exception e)
            {
                return Task.FromResult<List<AGMCompanies>>(new List<AGMCompanies>());
            }
        }


        private Task<string> EventResolutionAsync(string company)
        {
            string question = "";
            var resolution = db.Question.SingleOrDefault(r => r.Company.ToLower() == company.ToLower() && r.questionStatus == true);
            if (resolution != null)
            {
                question = resolution.question;
            }
            return Task.FromResult<string>(question);
        }


        public async Task<ActionResult> Vote()
        {
            var sessionid = System.Web.HttpContext.Current.Session.SessionID;
            var model = (APIMessageLog)Session["Authorize"];
            var abstainbtnchoice = true;


            if (model != null && (model.ResponseCode == "" || model.ResponseCode == "200" || model.ResponseCode == "206"))
            {

                    if (model.shareholder != null)
                    {
                        if (model.ResponseCode == "206")
                        {
                            //Create new Session login for user
                            var oldsessionversion = await MobileWebVotingSessionManager.CreateUserLoginHistory(model.shareholder.Company, model.shareholder.ShareholderNum, sessionid);
                        //model.sessionVersion = sessionversion;
                        //Send a refresh call to the browser
                        Functions.LogoutPreviousPages(model.shareholder.Company, oldsessionversion);


                    }
                    //var sessionresponse = await MobileWebVotingSessionManager.CheckUserLoginHistoryAsync(model.shareholder.Company, model.shareholder.ShareholderNum, sessionid);

                    //if (sessionresponse)
                    //{
                    //    model.ResponseMessage = "A new session on another browser has logged you out.";
                    //    TempData["Portal"] = model.ResponseMessage;
                    //    return RedirectToAction("Index");
                    //}
                    var UniqueAGMId = ua.RetrieveAGMUniqueID(model.shareholder.Company);
                        if (UniqueAGMId != -1)
                        {
                            var forBg = "green";
                            var againstBg = "red";
                            var abstainBg = "blue";
                            var voidBg = "black";
                            var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                            if (AgmEvent != null)
                            {
                                model.EventStatus = AgmEvent.AgmStart;
                            model.allChannel = AgmEvent.allChannels;
                            model.webChannel = AgmEvent.webChannel;
                            model.mobileChannel = AgmEvent.mobileChannel;
                                model.EventUrl = AgmEvent.OnlineUrllink;
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
                            }
                            ViewBag.shareholderemail = model.shareholder.ShareholderNum;
                            ViewBag.AGMTitle = model.AGMTitle.ToUpper();
                            ViewBag.resourcetype = model.ResourceType;

                            model.abstainBtnChoice = abstainbtnchoice;
                            ViewBag.abstainBtnchoice = abstainbtnchoice;
                            ViewBag.startadmittance = db.Present.Any(p => p.AGMID == UniqueAGMId && p.ShareholderNum == model.shareholder.ShareholderNum);
                            if (model.ResponseCode == "200")
                            {
                                var oldsessionversion = await MobileWebVotingSessionManager.CreateUserLoginHistory(model.shareholder.Company, model.shareholder.ShareholderNum, sessionid);

                            }
                            model.UserPollStatus = await MobileWebVotingSessionManager.GetShareholderAttendanceStatus(UniqueAGMId, model.shareholder.ShareholderNum);
                            model.UserLoginStatus = await MobileWebVotingSessionManager.GetShareholderLoginStatus(model.shareholder.Company, model.shareholder.ShareholderNum);
                            model.sessionVersion = await MobileWebVotingSessionManager.GetShareholderSessionVersionAsync(model.shareholder.Company, model.shareholder.ShareholderNum);
                            ViewBag.userProxyStatus = model.UserProxyStatus;
                            ViewBag.pollStatus = model.UserPollStatus;
                            model.ResponseCode = "";
                            model.forBg = forBg;
                            model.againstBg = againstBg;
                            model.abstainBg = abstainBg;
                            model.voidBg = voidBg;
                            model.Companylogo = AgmEvent.Image != null ? "data:image/jpg;base64," +
                             Convert.ToBase64String((byte[])AgmEvent.Image) : "";

                        }
                    return View(model);
                }               

            }

            if (model != null)
            {
                TempData["Portal"] = model.ResponseMessage;
            }

            //TempData["failedresponse"] = model;
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> LoginConfirmation(bool status)
        {
            //bool statuss = bool.Parse(status);

            bool response = false;
            var model = Session["Authorize"] as APIMessageLog;
            if (model != null && model.shareholder != null)
            {

                    response = await LoginConfirmationAsync(model.shareholder.Company, model.shareholder.ShareholderNum, status);

            }
            return Json(response, JsonRequestBehavior.AllowGet);


        }


        private Task<bool> LoginConfirmationAsync(string companyinfo, long shareholderNum, bool status)
        {
            bool value = false;

                value = MobileWebVotingSessionManager.ShareholderLoginHistoryConfirmation(companyinfo, shareholderNum, status);

            return Task.FromResult<bool>(value);
        }





        public async Task<ActionResult> EventLoadingPage(string id)
        {
            var response = await EventLoadingPageAsync(id);
            return View(response);
        }

        public Task<EventLoadingDto> EventLoadingPageAsync(string company)
        {
            var currentTime = DateTime.Now;
            TimeSpan agmCountdown = new TimeSpan();
            SettingsModel AgmEvent;
            var eventLoadingmodel = new EventLoadingDto();
            if (!string.IsNullOrEmpty(company))
            {
                var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
                if (UniqueAGMId != -1)
                {
                    AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                    if (AgmEvent != null && AgmEvent.AgmDateTime != null)
                    {
                        agmCountdown = (TimeSpan)(AgmEvent.AgmDateTime - currentTime);
                        eventLoadingmodel = new EventLoadingDto()
                        {
                            timespan = agmCountdown,
                            company = company,
                            AGMDescription = AgmEvent.Description,
                            MeetingDate = (DateTime)AgmEvent.AgmDateTime,
                            MeetingVenue = AgmEvent.Venue

                        };
                    }

                }
            }

            return Task.FromResult<EventLoadingDto>(eventLoadingmodel);
        }



        //[HttpPost]
        //public async Task<ActionResult> AccreditationConfirmation(accredationDto post)
        //{
        //    var sessionid = System.Web.HttpContext.Current.Session.SessionID;

        //    var response = await AccreditationConfirmationPostAsync(post, sessionid);

        //    Session["Authorize"] = response;

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}




        //private Task<APIMessageLog> AccreditationConfirmationPostAsync(accredationDto model, string sessionid)
        //{

        //    try
        //    {

        //        Session.Clear();
        //        Application_BeginRequest();
        //        APIMessageLog MessageLog;
        //        AppLog log;
        //        if (model.agmid > 0 && model.shareholderNum > 0)
        //        {
        //            var UniqueAGMId = model.agmid;
        //            var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
        //            var company = AgmEvent.CompanyName;

        //            var shareholderRecord = db.Present.SingleOrDefault(u => u.AGMID == UniqueAGMId && u.ShareholderNum == model.shareholderNum && u.present == true);
        //            if (shareholderRecord == null)
        //            {
        //                    MessageLog = new APIMessageLog()
        //                    {
        //                        Status = "Failed Accreditation",
        //                        ResponseCode = "205",
        //                        ResponseMessage = "Failed Accreditation Attempt - You may have entered a wrong Accesscode",
        //                        EventTime = DateTime.Now
        //                    };
        //                    log = new AppLog();
        //                    log.Status = MessageLog.Status;
        //                    log.ResponseCode = MessageLog.ResponseCode;
        //                    log.ResponseMessage = MessageLog.ResponseMessage;
        //                    log.EventTime = MessageLog.EventTime;
        //                    db.AppLogs.Add(log);
        //                    db.SaveChanges();
        //                    return Task.FromResult<APIMessageLog>(MessageLog);
        //            }


        //            if (UniqueAGMId != -1)
        //            {
        //                MessageLog = new APIMessageLog()
        //                {
        //                    Status = "Failed",
        //                    ResponseCode = "205",
        //                    ResponseMessage = "Failed e-accreditation Attempt - The Company's AGM may not be listed or incorrect login credential. Email:" + " " + shareholderRecord.ShareholderNum + " " + "AccessCode:" + " " + model.accesscode,
        //                    EventTime = DateTime.Now
        //                };
        //                log = new AppLog();
        //                log.Status = MessageLog.Status;
        //                log.ResponseCode = MessageLog.ResponseCode;
        //                log.ResponseMessage = MessageLog.ResponseMessage;
        //                log.EventTime = MessageLog.EventTime;
        //                db.AppLogs.Add(log);
        //                db.SaveChanges();

        //                return Task.FromResult<APIMessageLog>(MessageLog);
        //            }

        //                            if (AgmEvent != null && AgmEvent.AgmEnd == true)
        //                            {
        //                                MessageLog = new APIMessageLog()
        //                                {
        //                                    Status = "Failed Accreditation",
        //                                    ResponseCode = "205",
        //                                    ResponseMessage = "Failed Accreditation Attempt - AGM may have ended and accesscode expired",
        //                                    EventTime = DateTime.Now
        //                                };

        //                                log = new AppLog();
        //                                log.Status = MessageLog.Status;
        //                                log.ResponseCode = MessageLog.ResponseCode;
        //                                log.ResponseMessage = MessageLog.ResponseMessage;
        //                                log.EventTime = MessageLog.EventTime;
        //                                db.AppLogs.Add(log);
        //                                db.SaveChanges();
        //                                return Task.FromResult<APIMessageLog>(MessageLog);
        //                            }
                                   

        //                                    var checkLogin = MobileWebVotingSessionManager.CheckUserLoginHistory(shareholderRecord.Company, shareholderRecord.ShareholderNum, sessionid);
        //                                    if (!checkLogin)
        //                                    {
        //                                        MessageLog = new APIMessageLog()
        //                                        {
        //                                            Status = "Login attempt Alert",
        //                                            ResponseCode = "206",
        //                                            ResponseMessage = "You may have logged in on another browser, previous login will expire. Click Continue.",
        //                                            EventTime = DateTime.Now,
        //                                            EventUrl = AgmEvent.OnlineUrllink,
        //                                            EventStatus = AgmEvent.AgmStart,
        //                                            sessionVersion = MobileWebVotingSessionManager.GetShareholderSessionVersion(shareholderRecord.Company, shareholderRecord.ShareholderNum),
        //                                            //UserProxyStatus = shareholderRecord.PresentByProxy,
        //                                            shareholder = shareholderRecord,
        //                                            AGMTitle = AgmEvent.Description
        //                                        };

        //                                        log = new AppLog();
        //                                        log.Status = MessageLog.Status;
        //                                        log.ResponseCode = MessageLog.ResponseCode;
        //                                        log.ResponseMessage = MessageLog.ResponseMessage;
        //                                        log.EventTime = MessageLog.EventTime;
        //                                        db.AppLogs.Add(log);
        //                                        db.SaveChanges();
        //                                        return Task.FromResult<APIMessageLog>(MessageLog);
        //                                    }

        //                                    MessageLog = new APIMessageLog()
        //                                    {
        //                                        Status = "successfully Accredited",
        //                                        ResponseCode = "200",
        //                                        ResponseMessage = "Successful Accreditation. Email Address" + " " + shareholderRecord.ShareholderNum + " " + "AccessCode:" + " " + model.accesscode,
        //                                        EventTime = DateTime.Now,
        //                                        EventUrl = AgmEvent.OnlineUrllink,
        //                                        EventStatus = AgmEvent.AgmStart,
        //                                        sessionVersion = MobileWebVotingSessionManager.GetShareholderSessionVersion(shareholderRecord.Company, shareholderRecord.ShareholderNum),
        //                                        //UserProxyStatus = shareholderRecord.PresentByProxy,
        //                                        shareholder = shareholderRecord,
        //                                        AGMTitle = AgmEvent.Description
        //                                    };
        //                                    log = new AppLog();
        //                                    log.Status = MessageLog.Status;
        //                                    log.ResponseCode = MessageLog.ResponseCode;
        //                                    log.ResponseMessage = MessageLog.ResponseMessage;
        //                                    log.EventTime = MessageLog.EventTime;
        //                                    db.AppLogs.Add(log);
        //                                    db.SaveChanges();
        //                                    return Task.FromResult<APIMessageLog>(MessageLog);

        //        }
        //        MessageLog = new APIMessageLog()
        //        {
        //            Status = "Failed Accreditation",
        //            ResponseCode = "205",
        //            ResponseMessage = "Failed Accreditation Attempt - Null Entry for Company information or Accesscode",
        //            EventTime = DateTime.Now
        //        };
        //        log = new AppLog();
        //        log.Status = MessageLog.Status;
        //        log.ResponseCode = MessageLog.ResponseCode;
        //        log.ResponseMessage = MessageLog.ResponseMessage;
        //        log.EventTime = MessageLog.EventTime;
        //        db.AppLogs.Add(log);
        //        db.SaveChanges();
        //        return Task.FromResult<APIMessageLog>(MessageLog);

        //    }
        //    catch (Exception e)
        //    {
        //        var MessageLog = new APIMessageLog()
        //        {
        //            Status = "Error",
        //            ResponseCode = "500",
        //            ResponseMessage = "Something went wrong while processing this request from" + " " + "Please contact admin",
        //            EventTime = DateTime.Now
        //        };
        //        AppLog log = new AppLog();
        //        log.Status = MessageLog.Status;
        //        log.ResponseCode = MessageLog.ResponseCode;
        //        log.ResponseMessage = MessageLog.ResponseMessage;
        //        log.EventTime = MessageLog.EventTime;
        //        db.AppLogs.Add(log);
        //        db.SaveChanges();
        //        return Task.FromResult<APIMessageLog>(MessageLog);
        //    }

        //}

        [HttpPost]
        public async Task<ActionResult> AGMQuestion(string question, string shareholdernum, string company)
        {
            var response = await AGMQuestionAsync(question, shareholdernum, company);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public Task<APIMessageLog> AGMQuestionAsync(string question, string shareholdernum, string company)
        {
            try
            {
                APIMessageLog MessageLog = new APIMessageLog();
                //var loginShareholder = (APIMessageLog)Session["Authorize"];

                if (string.IsNullOrWhiteSpace(question) || string.IsNullOrWhiteSpace(shareholdernum))
                {
                    MessageLog = new APIMessageLog
                    {
                        ResponseCode = "205",
                        ResponseMessage = "No question was typed"
                    };
                    return Task.FromResult<APIMessageLog>(MessageLog);
                }

                long shareholderNum;
                if (Int64.TryParse(shareholdernum, out shareholderNum))
                {
                    var loginShareholder = db.BarcodeStore.FirstOrDefault(b => b.Company.ToUpper() == company.ToUpper() && b.ShareholderNum == shareholderNum);
                    var agmid = ua.RetrieveAGMUniqueID(company);
                    AGMQuestion agmQ = new AGMQuestion
                    {

                        ShareholderName = loginShareholder.Name,
                        shareholderquestion = question.Trim(),
                        Company = loginShareholder.Company,
                        datetime = DateTime.Now,
                        ShareholderNumber = loginShareholder.ShareholderNum,
                        emailAddress = loginShareholder.emailAddress,
                        holding = loginShareholder.Holding,
                        PercentageHolding = loginShareholder.PercentageHolding,
                        phoneNumber = loginShareholder.PhoneNumber,
                        AGMID = agmid

                    };
                    db.AGMQuestions.Add(agmQ);
                    db.SaveChanges();
                    EmailManager em = new EmailManager();
                    var response = em.SendEmail(agmQ);
                    MessageLog = new APIMessageLog
                    {
                        ResponseCode = "200",
                        ResponseMessage = "Success",
                    };
                    return Task.FromResult<APIMessageLog>(MessageLog);
                }
                else
                {
                    MessageLog = new APIMessageLog
                    {
                        ResponseCode = "205",
                        ResponseMessage = "Request Error.",
                    };
                    return Task.FromResult<APIMessageLog>(MessageLog);
                }


            }
            catch (Exception e)
            {
                var messagelog = new APIMessageLog()
                {
                    ResponseCode = "205",
                    ResponseMessage = e.Message
                };
                return Task.FromResult<APIMessageLog>(messagelog);
            }

        }

        //public string DangoteEncryptIndex()
        //{
        //    var query = string.Format("{0}|{1}", "johnfemy@yahoo.com",2);
        //    var encryptedtext = query.Encrypt();
        //    return encryptedtext;
        //}

        //public string AccessEncryptIndex()
        //{
        //    var query = string.Format("{0}|{1}|{2}","johnfemy@yahoo.com",5);
        //    var encryptedtext = query.Encrypt();
        //    return encryptedtext;
        //}
        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }


        [HttpPost]
        public async Task<ActionResult> AuthenticateWebVoting(accredationDto model)
        {
            var response = await _webVotingService.AccreditationConfirmationPostAsync(model);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> Voting(string query)
        {
            var sessionid = System.Web.HttpContext.Current.Session.SessionID;
            var response = await VotingAsync(query, sessionid);

            Session["Authorize"] = response;
            if (response.ResponseCode == "206")
            {
                return RedirectToAction("AGMStageLogin");
            }

            return RedirectToAction("Vote");
        }

        public Task<APIMessageLog> VotingAsync(string encryptedText, string sessionid)
        {
            //var query = new VoteModel();
            string companyinfo;
            int ShareholderNum;
            int agmid;
            APIMessageLog MessageLog;
            AppLog log;

            try
            {

                Session.Clear();
                Application_BeginRequest();

                if (!string.IsNullOrEmpty(encryptedText))
                {
                    var replacencryptedString = encryptedText.Replace(" ", "+");
                    var decryptedtext = replacencryptedString.Decrypt();
                    var querystring = decryptedtext.Split('|');
                    ShareholderNum = int.Parse(querystring[0].Trim());
                    //companyinfo = querystring[1].Trim();
                    agmid = int.Parse(querystring[1].Trim());

                    //VoteModel query = (VoteModel)decryptedtext;


                    //companyinfo = query.company;
                    //ShareholderNum = query.identity;
                    //agmid = query.agmid;

                    var agmstatus = db.Settings.FirstOrDefault(s => s.AGMID == agmid);

                    if (agmstatus != null)
                    {

                        if (agmstatus.AgmEnd == true)
                        {
                            MessageLog = new APIMessageLog()
                            {
                                Status = "Failed Voting Login Attempt",
                                ResponseCode = "205",
                                ResponseMessage = "Failed Voting Login Attempt- AGM may have ended and accesscode expired",
                                EventTime = DateTime.Now
                            };
                            log = new AppLog();
                            log.Status = MessageLog.Status;
                            log.ResponseCode = MessageLog.ResponseCode;
                            log.ResponseMessage = MessageLog.ResponseMessage;
                            log.EventTime = MessageLog.EventTime;
                            db.AppLogs.Add(log);
                            db.SaveChanges();
                            return Task.FromResult<APIMessageLog>(MessageLog);
                        }

                        if (agmid == -1)
                        {
                            MessageLog = new APIMessageLog()
                            {
                                Status = "Failed",
                                ResponseCode = "205",
                                ResponseMessage = "Failed Voting Login Attempt - The Company's AGM may not be listed or incorrect login credential",
                                EventTime = DateTime.Now
                            };
                            log = new AppLog();
                            log.Status = MessageLog.Status;
                            log.ResponseCode = MessageLog.ResponseCode;
                            log.ResponseMessage = MessageLog.ResponseMessage;
                            log.EventTime = MessageLog.EventTime;
                            db.AppLogs.Add(log);
                            db.SaveChanges();

                            return Task.FromResult<APIMessageLog>(MessageLog);
                        }

                        if (agmstatus != null && agmstatus.AgmEnd == true)
                        {
                            MessageLog = new APIMessageLog()
                            {
                                Status = "Failed Voting Login Attempt",
                                ResponseCode = "205",
                                ResponseMessage = "Failed Voting Login Attempt- AGM may have ended and accesscode expired",
                                EventTime = DateTime.Now
                            };

                            log = new AppLog();
                            log.Status = MessageLog.Status;
                            log.ResponseCode = MessageLog.ResponseCode;
                            log.ResponseMessage = MessageLog.ResponseMessage;
                            log.EventTime = MessageLog.EventTime;
                            db.AppLogs.Add(log);
                            db.SaveChanges();
                            return Task.FromResult<APIMessageLog>(MessageLog);
                        }

                        var shareholderRecord = db.Present.SingleOrDefault(u => u.AGMID == agmid && u.ShareholderNum == ShareholderNum && u.present == true);
                        if (shareholderRecord == null)
                        {
                            MessageLog = new APIMessageLog()
                            {
                                Status = "Failed Voting Login Attempt",
                                ResponseCode = "205",
                                ResponseMessage = "Failed Voting Login Attempt - You may have entered a wrong Accesscode",
                                EventTime = DateTime.Now
                            };
                            log = new AppLog();
                            log.Status = MessageLog.Status;
                            log.ResponseCode = MessageLog.ResponseCode;
                            log.ResponseMessage = MessageLog.ResponseMessage;
                            log.EventTime = MessageLog.EventTime;
                            db.AppLogs.Add(log);
                            db.SaveChanges();
                            return Task.FromResult<APIMessageLog>(MessageLog);
                        }



                        var checkLogin = MobileWebVotingSessionManager.CheckUserLoginHistory(shareholderRecord.Company, shareholderRecord.ShareholderNum, sessionid);
                        if (checkLogin)
                        {
                            MessageLog = new APIMessageLog()
                            {
                                Status = "Login attempt Alert",
                                ResponseCode = "206",
                                ResponseMessage = "You may have logged in on another browser, previous login will expire. Click Continue.",
                                EventTime = DateTime.Now,
                                EventUrl = agmstatus.OnlineUrllink,
                                EventStatus = agmstatus.AgmStart,
                                sessionVersion = MobileWebVotingSessionManager.GetShareholderSessionVersion(shareholderRecord.Company, shareholderRecord.ShareholderNum),
                                //UserProxyStatus = shareholderRecord.PresentByProxy,
                                shareholder = shareholderRecord,
                                AGMTitle = agmstatus.Description
                            };

                            log = new AppLog();
                            log.Status = MessageLog.Status;
                            log.ResponseCode = MessageLog.ResponseCode;
                            log.ResponseMessage = MessageLog.ResponseMessage;
                            log.EventTime = MessageLog.EventTime;
                            db.AppLogs.Add(log);
                            db.SaveChanges();
                            return Task.FromResult<APIMessageLog>(MessageLog);
                        }

                        MessageLog = new APIMessageLog()
                        {
                            Status = "successfully login for voting",
                            ResponseCode = "200",
                            ResponseMessage = "Successful Voting login.",
                            EventTime = DateTime.Now,
                            EventUrl = agmstatus.OnlineUrllink,
                            EventStatus = agmstatus.AgmStart,
                            sessionVersion = MobileWebVotingSessionManager.GetShareholderSessionVersion(shareholderRecord.Company, shareholderRecord.ShareholderNum),
                            //UserProxyStatus = shareholderRecord.PresentByProxy,
                            shareholder = shareholderRecord,
                            AGMTitle = agmstatus.Description
                        };
                        log = new AppLog();
                        log.Status = MessageLog.Status;
                        log.ResponseCode = MessageLog.ResponseCode;
                        log.ResponseMessage = MessageLog.ResponseMessage;
                        log.EventTime = MessageLog.EventTime;
                        db.AppLogs.Add(log);
                        db.SaveChanges();
                        return Task.FromResult<APIMessageLog>(MessageLog);

                    }


                }
                MessageLog = new APIMessageLog()
                {
                    Status = "Failed Voting Login Attempt",
                    ResponseCode = "205",
                    ResponseMessage = "Failed Voting Login Attempt - Null Entry for Company information or Accesscode",
                    EventTime = DateTime.Now
                };
                log = new AppLog();
                log.Status = MessageLog.Status;
                log.ResponseCode = MessageLog.ResponseCode;
                log.ResponseMessage = MessageLog.ResponseMessage;
                log.EventTime = MessageLog.EventTime;
                db.AppLogs.Add(log);
                db.SaveChanges();
                return Task.FromResult<APIMessageLog>(MessageLog);
            }
            catch (Exception e)
            {
                 MessageLog = new APIMessageLog()
                {
                    Status = "Error",
                    ResponseCode = "500",
                    ResponseMessage = "Something went wrong while processing this request from" + " " + "Please contact admin",
                    EventTime = DateTime.Now,
                    
                    
                };
                log = new AppLog();
                log.Status = MessageLog.Status;
                log.ResponseCode = MessageLog.ResponseCode;
                log.ResponseMessage = MessageLog.ResponseMessage;
                log.EventTime = MessageLog.EventTime;
                db.AppLogs.Add(log);
                db.SaveChanges();
                return Task.FromResult<APIMessageLog>(MessageLog);
            }
        }



            public async Task<ActionResult> AutoAdmittance(string company, string shareholderNum, string accesscode)
        {
            var response = await _webVotingService.AutoAdmittanceAsync(company, shareholderNum,accesscode);

            return Json(response, JsonRequestBehavior.AllowGet);
        }



      

        [HttpPost]
        public async Task<JsonResult> VotingResponse(VoteModel post)
        {
            var response = await VotingResponseAsync(post);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        private Task<HttpRequestResponse> VotingResponseAsync(VoteModel post)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
                //    {
                //        Code = "403",
                //        Message = "Invalid Request"
                //    });
                //}
                if (!string.IsNullOrEmpty(post.company) && !string.IsNullOrWhiteSpace(post.company))
                {
                    var UniqueAGMId = ua.RetrieveAGMUniqueID(post.company);

                    if (UniqueAGMId != -1)
                    {
                        var abstainBtnchoice = true;
                        var AgmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                        if (AgmEventSetting != null && AgmEventSetting.AbstainBtnChoice != null)
                        {
                            abstainBtnchoice = (bool)AgmEventSetting.AbstainBtnChoice;
                        }


                        if (abstainBtnchoice == false && post.response == "ABSTAIN")
                        {
                            return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
                            {
                                Code = "505",
                                Message = "ABSTAIN is not a choice."
                            });
                        }

                        if (TimerControll.GetTimeStatus(post.company) == true)
                        {
                            var question = db.Question.SingleOrDefault(q => q.AGMID == UniqueAGMId && q.questionStatus == true);
                            if (question != null)
                            {
                                //var voter = db.Present.SingleOrDefault(q => q.emailAddress == post.identity && q.present == true);                         
                                var voter = db.Present.FirstOrDefault(q => q.AGMID == UniqueAGMId && q.ShareholderNum == post.shareholderNum && q.PermitPoll == 1);

                                if(voter==null)
                                {
                                    return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
                                    {
                                        Code = "505",
                                        Message = "SHAREHOLDER MAY HAVE VOTED BY PROXY OR NOT PERMITTED TO VOTE BY THE FACILITATOR. THANK YOU."
                                    });

                                }
                                    //var voter = db.Present.Find(votee.Id);
                                    var checkresult = db.Result.FirstOrDefault(r => r.ShareholderNum == voter.ShareholderNum && r.QuestionId == post.Id);
                                    //if true - edit value
                                    if (checkresult != null)
                                    {
                                        checkresult.date = DateTime.Now;
                                        checkresult.Timestamp = DateTime.Now.TimeOfDay;
                                        checkresult.VoteStatus = "Voted";
                                        checkresult.Source = "Web";
                                        if (post.response == "FOR")
                                        {
                                            checkresult.VoteFor = true;
                                            checkresult.VoteAgainst = false;
                                            checkresult.VoteAbstain = false;
                                        }
                                        else if (post.response == "AGAINST")
                                        {
                                            checkresult.VoteAgainst = true;
                                            checkresult.VoteFor = false;
                                            checkresult.VoteAbstain = false;
                                        }
                                        else if (post.response == "ABSTAIN")
                                        {

                                            checkresult.VoteAbstain = true;
                                            checkresult.VoteFor = false;
                                            checkresult.VoteAgainst = false;
                                        }
                                        db.Entry(checkresult).State = EntityState.Modified;

                                        try
                                        {
                                            db.SaveChanges();

                                        }
                                        catch (DbUpdateConcurrencyException e)
                                        {

                                        }

                                        return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
                                        {
                                            Code = "200",
                                            Message = "Success"
                                        });

                                        //var sucessoutput = JsonConvert.SerializeObject(successresponse);

                                        //return new HttpResponseMessage()
                                        //{
                                        //    Content = new StringContent(sucessoutput, System.Text.Encoding.UTF8, "application/json")
                                        //};

                                    }
                                    else
                                    {
                                        Result result = new Result();
                                        result.Name = voter.Name;
                                        result.ShareholderNum = voter.ShareholderNum;
                                        result.Address = voter.Address;
                                        //result.ParentNumber = voter.ParentNumber;
                                        result.Holding = voter.Holding;
                                        result.Company = post.company.Trim();
                                        result.Year = currentYear;
                                        result.AGMID = ua.RetrieveAGMUniqueID(post.company);
                                        result.phonenumber = voter.PhoneNumber;
                                        result.PercentageHolding = voter.PercentageHolding;
                                        result.QuestionId = post.Id;
                                        result.date = DateTime.Now;
                                        result.Timestamp = DateTime.Now.TimeOfDay;
                                        result.VoteStatus = "Voted";
                                        result.Source = "Web";
                                        result.Present = true;
                                        if (post.response == "FOR")
                                        {
                                            result.VoteFor = true;
                                            result.VoteAgainst = false;
                                            result.VoteAbstain = false;

                                        }
                                        else if (post.response == "AGAINST")
                                        {
                                            result.VoteAgainst = true;
                                            result.VoteFor = false;
                                            result.VoteAbstain = false;
                                        }
                                        else if (post.response == "ABSTAIN")
                                        {
                                            result.VoteAbstain = true;
                                            result.VoteFor = false;
                                            result.VoteAgainst = false;
                                        }
                                        db.Result.Add(result);
                                    }

                                    voter.TakePoll = true;
                                    db.Entry(voter).State = EntityState.Modified;

                                    try
                                    {
                                        db.SaveChanges();

                                    }
                                    catch (DbUpdateConcurrencyException e)
                                    {

                                    }

                                    return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
                                    {
                                        Code = "200",
                                        Message = "SUCCESS"
                                    });
                                    //Voting Success
                                    //var sucoutput = JsonConvert.SerializeObject(successresponse);

                                    //return new HttpResponseMessage()
                                    //{
                                    //    Content = new StringContent(sucoutput, System.Text.Encoding.UTF8, "application/json")
                                    //};                             

                                //Voter not marked present at AGM


                            }
                            //Wait for voting to commence on this resolution

                            return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
                            {
                                Code = "400",
                                Message = "WAIT FOR VOTING TO COMMENCE ON THIS RESOLUTION"
                            });

                        }
                        //Voting clock has stopped

                        return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
                        {
                            Code = "406",
                            Message = "VOTING CLOCK HAS STOPPED"

                        });
                    }

                    return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
                    {
                        Code = "500",
                        Message = "Failed Request"
                    });

                }
                else
                {
                    return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
                    {
                        Code = "404",
                        Message = "Empty Request"

                    });

                }


            }
            catch (Exception e)
            {
                //var log = new AppLog();
                //log.Status = "Error";
                //log.ResponseCode = e.Message;
                //log.ResponseMessage = e.StackTrace.ToString();
                //log.EventTime = DateTime.Now;
                //db.AppLogs.Add(log);
                //db.SaveChanges();
                return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
                {
                    Code = "500",
                    Message = "Failed Request"
                });
            }

        }


    }
}