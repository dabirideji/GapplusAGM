using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BarcodeGenerator.Models;
using BarcodeGenerator.Models.ModelDTO;
using BarcodeGenerator.Service;
using BarcodeGenerator.Util;
using Gapplus.Application.Helpers;
using Microsoft.AspNetCore.Http;

namespace Gapplus.Application.Contracts
{
    public class CoronationVirtualService
    {
        UsersContext db;
        PreregistrationService _preregistration;
        UserAdmin ua;
        AGMManager agmmanager;
        WebSessionManager _websessionManager;
        ResolutionService _resolutionService;
        AuthenticationService authService;

        MessageService _messages;

        public CoronationVirtualService(UsersContext _db)
        {
            db = _db;

            ua = new UserAdmin(db);
            agmmanager = new AGMManager(db);
            _websessionManager = new WebSessionManager(db);
            _preregistration = new PreregistrationService(db);
            _resolutionService = new ResolutionService(db);
            authService = new AuthenticationService(db);
            _messages = new MessageService(db);
        }




        private static string currentYear = DateTime.Now.Year.ToString();

        private static string connStr = DatabaseManager.GetConnectionString();
        SqlConnection conn =
                  new SqlConnection(connStr);

        Dictionary<string, string> resourceTypes = new Dictionary<string, string>(){
                                        {"Shareholder", "Shareholder"},
                                        {"Non-Shareholder", "Facilitator"}
        };
        // GET: CoronationVirtual


        
        public async Task<APIMessageLog> ConfirmShareholderToken(PreregistrationRegisterModel dto)
        {
            // var sessionid = System.Web.HttpContext.Current.Session.SessionID;
            var sessionid = SessionManager.GetSessionData<String>("SessionId");
        if(string.IsNullOrEmpty(sessionid)){
            throw new UnauthorizedAccessException("SESSION ID NOT SET OR INVALID || INVALID SESSION");
        }
            var response = await authService.CoronationVirtualTokenAuthentication(dto.Company, dto.Email, dto.Token);

            if (response.ResponseCode == "200")
            {

                // Session["Authorize"] = response;
                SessionManager.SetSessionData("Authorize",response);
            }
                return response;


            // return Json(response, JsonRequestBehavior.AllowGet);
        }


        public async Task<object> CoronationVirtual()
        {
            // var sessionid = System.Web.HttpContext.Current.Session.SessionID;
            // var model = (APIMessageLog)Session["Authorize"];


            string sessionid =(string) SessionManager.GetSessionData<string>("SessionId");

            var model = (APIMessageLog)SessionManager.GetSessionData<APIMessageLog>("Authorize");
            var abstainbtnchoice = true;


            if (model != null && (model.ResponseCode == "" || model.ResponseCode == "200"))
            {

                if (model.ResourceType == "Facilitator")
                {
                    if (model.facilitator != null)
                    {
                        var checkLogin = WebSessionManager.CheckFacilitatorLoginHistory(model.facilitator.Company, model.facilitator.emailAddress, sessionid);
                        if (checkLogin)
                        {
                            //Create new Session login for user
                            var oldsessionversion = await WebSessionManager.CreateFacilitatorLoginHistory(model.facilitator.Company, model.facilitator.emailAddress, sessionid);

                            //Send a refresh call to the browser
                            Functions.LogoutPreviousPages(model.facilitator.Company, oldsessionversion);
                        }

                        //Check for the right session
                        var sessionresponse = await WebSessionManager.CheckFacilitatorLoginHistoryAsync(model.facilitator.Company, model.facilitator.emailAddress, sessionid);

                        if (!sessionresponse)
                        {
                            model.ResponseMessage = "A new session on another browser has logged you out.";
                            // TempData["Portal"] = model.ResponseMessage;
                            // return RedirectToAction("Index");
                            return model;
                        }
                        var UniqueAGMId = ua.RetrieveAGMUniqueID(model.facilitator.Company);
                        if (UniqueAGMId != -1)
                        {
                            var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                            // ViewBag.facilitatoremail = model.facilitator.emailAddress;
                            // ViewBag.AGMTitle = model.AGMTitle.ToUpper();
                            // ViewBag.resourcetype = model.ResourceType;
                            // ViewBag.startadmittance = false;
                            model.EventStatus = AgmEvent.AgmStart;
                            model.EventUrl = AgmEvent.OnlineUrllink;
                            model.UserPollStatus = 0;
                            model.UserProxyStatus = false;
                            model.AGMID = UniqueAGMId;
                            model.MessagingChoice = AgmEvent.MessagingChoice;
                            model.allChannel = AgmEvent.allChannels;
                            model.webChannel = AgmEvent.webChannel;
                            model.mobileChannel = AgmEvent.mobileChannel;
                            model.Messages = await _messages.GetAllQuestions(UniqueAGMId);
                            if (model.ResponseCode == "200")
                            {
                                var oldsessionversion = await WebSessionManager.CreateFacilitatorLoginHistory(model.facilitator.Company, model.facilitator.emailAddress, sessionid);

                            }
                            model.ResponseCode = "";
                            model.UserLoginStatus = await WebSessionManager.GetFacilatorLoginStatus(model.facilitator.Company, model.facilitator.emailAddress);
                            model.sessionVersion = await WebSessionManager.GetFacilatorSessionVersion(model.facilitator.Company, model.facilitator.emailAddress);
                            model.Companylogo = AgmEvent.Image != null ? "data:image/jpg;base64," +
                                       Convert.ToBase64String((byte[])AgmEvent.Image) : "";
                            //if (AgmEvent.AbstainBtnChoice != null)
                            //{
                            //    abstainbtnchoice = (bool)AgmEvent.AbstainBtnChoice;
                            //}
                            //model.abstainBtnChoice = abstainbtnchoice;
                            //ViewBag.abstainBtnchoice = abstainbtnchoice;
                        }

                    }
                }
                else
                {
                    if (model.shareholder != null)
                    {

                        var checkLogin = WebSessionManager.CheckUserLoginHistory(model.shareholder.Company, model.shareholder.emailAddress, sessionid);
                        if (checkLogin)
                        {

                            //Create new Session login for user
                            var oldsessionversion = await WebSessionManager.CreateUserLoginHistory(model.shareholder.Company, model.shareholder.emailAddress, sessionid);
                            //model.sessionVersion = sessionversion;
                            //Send a refresh call to the browser
                            Functions.LogoutPreviousPages(model.shareholder.Company, oldsessionversion);
                        }
                        var sessionresponse = await WebSessionManager.CheckUserLoginHistoryAsync(model.shareholder.Company, model.shareholder.emailAddress, sessionid);

                        if (!sessionresponse)
                        {
                            model.ResponseMessage = "A new session on another browser has logged you out.";
                            // TempData["Portal"] = model.ResponseMessage;
                            // return RedirectToAction("Index");
                            return model;
                        }
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
                            // ViewBag.shareholderemail = model.shareholder.emailAddress;
                            // ViewBag.AGMTitle = model.AGMTitle.ToUpper();
                            // ViewBag.resourcetype = model.ResourceType;

                            model.abstainBtnChoice = abstainbtnchoice;
                            model.MessagingChoice = AgmEvent.MessagingChoice;
                            // ViewBag.abstainBtnchoice = abstainbtnchoice;
                            // ViewBag.startadmittance = db.Present.Any(p => p.AGMID == UniqueAGMId && p.ShareholderNum == model.shareholder.ShareholderNum);
                            if (model.ResponseCode == "200")
                            {
                                var oldsessionversion = await WebSessionManager.CreateUserLoginHistory(model.shareholder.Company, model.shareholder.emailAddress, sessionid);

                            }
                            model.UserPollStatus = await WebSessionManager.GetShareholderAttendanceStatus(UniqueAGMId, model.shareholder.emailAddress);
                            model.UserLoginStatus = await WebSessionManager.GetShareholderLoginStatus(model.shareholder.Company, model.shareholder.emailAddress);
                            model.sessionVersion = await WebSessionManager.GetShareholderSessionVersion(model.shareholder.Company, model.shareholder.emailAddress);
                            // ViewBag.userProxyStatus = model.UserProxyStatus;
                            // ViewBag.pollStatus = model.UserPollStatus;
                            model.AGMID = UniqueAGMId;
                            model.Messages = await _messages.GetAllQuestions(UniqueAGMId);
                            model.ResponseCode = "";
                            model.forBg = forBg;
                            model.againstBg = againstBg;
                            model.abstainBg = abstainBg;
                            model.voidBg = voidBg;
                            model.Companylogo = AgmEvent.Image != null ? "data:image/jpg;base64," +
                             Convert.ToBase64String((byte[])AgmEvent.Image) : "";

                        }

                    }
                }


                // return View(model);
                return model;
            }
            else
            {
                // if (model != null)
                // {
                //     TempData["Portal"] = model.ResponseMessage;
                // }

                //TempData["failedresponse"] = model;
                // return RedirectToAction("Index");
                return model;
            }

        }
    }


}