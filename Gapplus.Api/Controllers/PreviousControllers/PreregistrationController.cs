using BarcodeGenerator.Models;
using BarcodeGenerator.Models.ModelDTO;
using BarcodeGenerator.Service;
using BarcodeGenerator.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarcodeGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PreregistrationController : ControllerBase
    {
        UsersContext db;
        private readonly ITempDataManager _tempDataManager;
        private readonly IViewBagManager _viewBagManager;
        PreregistrationService _preregistration;
        UserAdmin ua;
        AGMManager agmmanager;
        WebSessionManager _websessionManager;
        ResolutionService _resolutionService;



        public PreregistrationController(UsersContext _db,ITempDataManager tempDataManager,IViewBagManager viewBagManager)
        {
            db=_db;
            _tempDataManager = tempDataManager;
            _viewBagManager = viewBagManager;
            _preregistration = new PreregistrationService(db);
            ua = new UserAdmin(db);
            agmmanager = new AGMManager(db);
            _websessionManager = new WebSessionManager(db);
            _resolutionService = new ResolutionService(db);

        }
        //MessageService _messages = new MessageService();



        [HttpPost]
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
                SessionManager.SetSessionData("Authorize",MessageLog);
                // Session["Authorize"] = MessageLog;
            }
            return Ok(accountResponse);
        }

        [HttpPost]
        public async Task<ActionResult>  ConfirmShareholderToken(PreregistrationRegisterModel dto)
        {
            var response = await _preregistration.PreregistrationLogin(dto.Company, dto.Email, dto.Token);

            if (response.ResponseCode == "200")
            {
                // Session["Authorize"] = response;
                SessionManager.SetSessionData("Authorize",response);
            }
            return Ok(response);
        }

[HttpGet]
        public async Task<ActionResult> Preregistration()
        {
            ///
///     need corrections here pls
///     /// 



            // var sessionid = System.Web.HttpContext.Current.Session.SessionID;
            var sessionid =SessionManager.GetSessionData<string>("SessionID");
                   // var model = (APIMessageLog)Session["Authorize"];
            var model =SessionManager.GetSessionData<APIMessageLog>("Authorize");
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

                        // TempData["Portal"] = model.ResponseMessage;
                        _tempDataManager.SetTempData("Portal",model.ResponseMessage);
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
                        // ViewBag.shareholderemail = model.PreregisterationShareholder.emailAddress;
                        // ViewBag.AGMTitle = AgmEvent.Title.ToUpper();
              
                        //ViewBag.resourcetype = model.ResourceType;

                        model.abstainBtnChoice = abstainbtnchoice;
                        model.MessagingChoice = AgmEvent.MessagingChoice;
                        // ViewBag.abstainBtnchoice = abstainbtnchoice;
                        _viewBagManager.SetValue("abstainBtnchoice",abstainbtnchoice);
                                  _viewBagManager.SetValue("shareholderemail", model.PreregisterationShareholder.emailAddress);
                        _viewBagManager.SetValue("AGMTitle",AgmEvent.Title.ToUpper());
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
                return Ok(model);
            }
            else
            {
                if (model != null)
                {
                        _tempDataManager.SetTempData("Portal",model.ResponseMessage);

                    // TempData["Portal"] = model.ResponseMessage;
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