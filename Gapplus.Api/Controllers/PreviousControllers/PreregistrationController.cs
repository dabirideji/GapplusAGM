using BarcodeGenerator.Models;
using BarcodeGenerator.Models.ModelDTO;
using BarcodeGenerator.Service;
using BarcodeGenerator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BarcodeGenerator.Controllers
{
    public class PreregistrationController : Controller
    {
        UsersContext db = new UsersContext();
        PreregistrationService _preregistration = new PreregistrationService();
        UserAdmin ua = new UserAdmin();
        AGMManager agmmanager = new AGMManager();
        WebSessionManager _websessionManager = new WebSessionManager();
        ResolutionService _resolutionService = new ResolutionService();
        //MessageService _messages = new MessageService();

        // GET: Preregistration
        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        public async Task<ActionResult> TokenComfirmation(RequestViewModel dto)
        {
            var accountResponse = await ua.GetShareHolderAccount(dto.company, dto.email);
            var MessageLog = new APIMessageLog()
            {
                Status = accountResponse.Status.ToString(),
                ResponseCode = accountResponse.Code.ToString(),
                ResponseMessage = accountResponse.Message,
                EventTime = DateTime.Now
            };
            var log = new AppLog();
            log.Status = MessageLog.Status;
            log.ResponseCode = MessageLog.ResponseCode;
            log.ResponseMessage = MessageLog.ResponseMessage;
            log.EventTime = MessageLog.EventTime;
            db.AppLogs.Add(log);
            db.SaveChanges();
            if (MessageLog.ResponseCode == "200")
            {
                MessageLog.PreregisterationShareholder = accountResponse.Data;
                Session["Authorize"] = MessageLog;
            }


            return PartialView(accountResponse);
        }

        [HttpPost]
        public async Task<ActionResult>  ConfirmShareholderToken(PreregistrationRegisterModel dto)
        {
            var response = await _preregistration.PreregistrationLogin(dto.Company, dto.Email, dto.Token);

            if (response.ResponseCode == "200")
            {
                
                Session["Authorize"] = response;
            }


            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Preregistration()
        {

            var sessionid = System.Web.HttpContext.Current.Session.SessionID;

            var model = (APIMessageLog)Session["Authorize"];
            var abstainbtnchoice = true;


            if (model != null && (model.ResponseCode == "" || model.ResponseCode == "200" || model.ResponseCode == "206"))
            {

                if (model.PreregisterationShareholder != null)
                {
                    if (model.ResponseCode == "206")
                    {
                        //Create new Session login for user
                        var oldsessionversion = await WebSessionManager.CreateUserLoginHistory(model.PreregisterationShareholder.Company, model.PreregisterationShareholder.emailAddress, sessionid);
                        //model.sessionVersion = sessionversion;
                        //Send a refresh call to the browser
                        Functions.LogoutPreviousPages(model.PreregisterationShareholder.Company, oldsessionversion);
                    }
                    var sessionresponse = await WebSessionManager.CheckUserLoginHistoryAsync(model.PreregisterationShareholder.Company, model.PreregisterationShareholder.emailAddress, sessionid);

                    if (!sessionresponse)
                    {
                        model.ResponseMessage = "A new session on another browser has logged you out.";
                        TempData["Portal"] = model.ResponseMessage;
                        return RedirectToAction("Index");
                    }
                    var UniqueAGMId = ua.RetrieveAGMUniqueID(model.PreregisterationShareholder.Company);
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
                        ViewBag.shareholderemail = model.PreregisterationShareholder.emailAddress;
                        ViewBag.AGMTitle = AgmEvent.Title.ToUpper();
                        //ViewBag.resourcetype = model.ResourceType;

                        model.abstainBtnChoice = abstainbtnchoice;
                        model.MessagingChoice = AgmEvent.MessagingChoice;
                        ViewBag.abstainBtnchoice = abstainbtnchoice;
                        //ViewBag.startadmittance = db.Present.Any(p => p.AGMID == UniqueAGMId && p.ShareholderNum == model.shareholder.ShareholderNum);
                        if (model.ResponseCode == "200")
                        {
                            var oldsessionversion = await WebSessionManager.CreateUserLoginHistory(model.PreregisterationShareholder.Company, model.PreregisterationShareholder.emailAddress, sessionid);

                        }
                        model.UserPollStatus = await WebSessionManager.GetShareholderAttendanceStatus(UniqueAGMId, model.PreregisterationShareholder.emailAddress);
                        model.UserLoginStatus = await WebSessionManager.GetShareholderLoginStatus(model.PreregisterationShareholder.Company, model.PreregisterationShareholder.emailAddress);
                        model.sessionVersion = await WebSessionManager.GetShareholderSessionVersion(model.PreregisterationShareholder.Company, model.PreregisterationShareholder.emailAddress);
                        //ViewBag.userProxyStatus = model.UserProxyStatus;
                        //ViewBag.pollStatus = model.UserPollStatus;
                        model.AGMID = UniqueAGMId;
                        //model.Messages = await _messages.GetAllQuestions(UniqueAGMId);
                        model.Rsolutions = await _resolutionService.GetResolutions(UniqueAGMId);
                        model.Preregistered = await _websessionManager.GetShareholderRegistrationStatus(model.PreregisterationShareholder.Company, model.PreregisterationShareholder.emailAddress);
                        model.ResponseCode = "";
                        model.forBg = forBg;
                        model.againstBg = againstBg;
                        model.abstainBg = abstainBg;
                        model.voidBg = voidBg;
                        model.Companylogo = AgmEvent.Image != null ? "data:image/jpg;base64," +
                         Convert.ToBase64String((byte[])AgmEvent.Image) : "";

                    }

                }
                return View(model);
            }
            else
            {
                if (model != null)
                {
                    TempData["Portal"] = model.ResponseMessage;
                }

                //TempData["failedresponse"] = model;
                return RedirectToAction("Index");
            }
            //return View();

        }

        //public async Task<ActionResult> PreregistrationResolutionUpdate(PreregistrationResultDto dto)
        //{
        //    var response = await _preregistration.SavePreregistrationResult(dto);

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}
    }
}