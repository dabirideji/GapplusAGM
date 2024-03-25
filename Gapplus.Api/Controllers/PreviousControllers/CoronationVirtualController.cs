// using BarcodeGenerator.Models;
// using BarcodeGenerator.Models.ModelDTO;
// using BarcodeGenerator.Service;
// using BarcodeGenerator.Util;
// using System;
// using System.Collections.Generic;
// using System.Configuration;
// using System.Data.SqlClient;
// using System.Linq;
// using System.Threading.Tasks;
// using System.Web;
// using System.Web.Mvc;

// namespace BarcodeGenerator.Controllers
// {
//     public class CoronationVirtualController : Controller
//     {
//         UsersContext db = new UsersContext();
//         PreregistrationService _preregistration = new PreregistrationService();
//         UserAdmin ua = new UserAdmin();
//         AGMManager agmmanager = new AGMManager();
//         WebSessionManager _websessionManager = new WebSessionManager();
//         ResolutionService _resolutionService = new ResolutionService();
//         AuthenticationService authService = new AuthenticationService();

//         MessageService _messages = new MessageService();

//         private static string currentYear = DateTime.Now.Year.ToString();

//         private static string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//         SqlConnection conn =
//                   new SqlConnection(connStr);

//         Dictionary<string, string> resourceTypes = new Dictionary<string, string>(){
//                                         {"Shareholder", "Shareholder"},
//                                         {"Non-Shareholder", "Facilitator"}
//         };
//         // GET: CoronationVirtual
//         public ActionResult Index()
//         {
//             return View();
//         }

//         public async Task<ActionResult> TokenComfirmation(RequestViewModel dto)
//         {
//             var accountResponse = await ua.GetShareHolderAccount(dto.company, dto.email);
//             //var MessageLog = new APIMessageLog()
//             //{
//             //    Status = accountResponse.Status.ToString(),
//             //    ResponseCode = accountResponse.Code.ToString(),
//             //    ResponseMessage = accountResponse.Message,
//             //    EventTime = DateTime.Now
//             //};
//             //var log = new AppLog();
//             //log.Status = MessageLog.Status;
//             //log.ResponseCode = MessageLog.ResponseCode;
//             //log.ResponseMessage = MessageLog.ResponseMessage;
//             //log.EventTime = MessageLog.EventTime;
//             //db.AppLogs.Add(log);
//             //db.SaveChanges();
//             //if (MessageLog.ResponseCode == "200")
//             //{
//             //    MessageLog.PreregisterationShareholder = accountResponse.Data;
//             //    Session["Authorize"] = MessageLog;
//             //}


//             return PartialView(accountResponse);
//         }

//         [HttpPost]
//         public async Task<ActionResult> ConfirmShareholderToken(PreregistrationRegisterModel dto)
//         {
//             var sessionid = System.Web.HttpContext.Current.Session.SessionID;
//             var response = await authService.CoronationVirtualTokenAuthentication(dto.Company, dto.Email, dto.Token);

//             if (response.ResponseCode == "200")
//             {

//                 Session["Authorize"] = response;
//             }


//             return Json(response, JsonRequestBehavior.AllowGet);
//         }


//         public async Task<ActionResult> CoronationVirtual()
//         {
//             var sessionid = System.Web.HttpContext.Current.Session.SessionID;

//             var model = (APIMessageLog)Session["Authorize"];
//             var abstainbtnchoice = true;


//             if (model != null && (model.ResponseCode == "" || model.ResponseCode == "200"))
//             {

//                 if (model.ResourceType == "Facilitator")
//                 {
//                     if (model.facilitator != null)
//                     {
//                         var checkLogin = WebSessionManager.CheckFacilitatorLoginHistory(model.facilitator.Company, model.facilitator.emailAddress, sessionid);
//                         if (checkLogin)
//                         {
//                             //Create new Session login for user
//                             var oldsessionversion = await WebSessionManager.CreateFacilitatorLoginHistory(model.facilitator.Company, model.facilitator.emailAddress, sessionid);

//                             //Send a refresh call to the browser
//                             Functions.LogoutPreviousPages(model.facilitator.Company, oldsessionversion);
//                         }

//                         //Check for the right session
//                         var sessionresponse = await WebSessionManager.CheckFacilitatorLoginHistoryAsync(model.facilitator.Company, model.facilitator.emailAddress, sessionid);

//                         if (!sessionresponse)
//                         {
//                             model.ResponseMessage = "A new session on another browser has logged you out.";
//                             TempData["Portal"] = model.ResponseMessage;
//                             return RedirectToAction("Index");
//                         }
//                         var UniqueAGMId = ua.RetrieveAGMUniqueID(model.facilitator.Company);
//                         if (UniqueAGMId != -1)
//                         {
//                             var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
//                             ViewBag.facilitatoremail = model.facilitator.emailAddress;
//                             ViewBag.AGMTitle = model.AGMTitle.ToUpper();
//                             ViewBag.resourcetype = model.ResourceType;
//                             ViewBag.startadmittance = false;
//                             model.EventStatus = AgmEvent.AgmStart;
//                             model.EventUrl = AgmEvent.OnlineUrllink;
//                             model.UserPollStatus = 0;
//                             model.UserProxyStatus = false;
//                             model.AGMID = UniqueAGMId;
//                             model.MessagingChoice = AgmEvent.MessagingChoice;
//                             model.allChannel = AgmEvent.allChannels;
//                             model.webChannel = AgmEvent.webChannel;
//                             model.mobileChannel = AgmEvent.mobileChannel;
//                             model.Messages = await _messages.GetAllQuestions(UniqueAGMId);
//                             if (model.ResponseCode == "200")
//                             {
//                                 var oldsessionversion = await WebSessionManager.CreateFacilitatorLoginHistory(model.facilitator.Company, model.facilitator.emailAddress, sessionid);

//                             }
//                             model.ResponseCode = "";
//                             model.UserLoginStatus = await WebSessionManager.GetFacilatorLoginStatus(model.facilitator.Company, model.facilitator.emailAddress);
//                             model.sessionVersion = await WebSessionManager.GetFacilatorSessionVersion(model.facilitator.Company, model.facilitator.emailAddress);
//                             model.Companylogo = AgmEvent.Image != null ? "data:image/jpg;base64," +
//                                        Convert.ToBase64String((byte[])AgmEvent.Image) : "";
//                             //if (AgmEvent.AbstainBtnChoice != null)
//                             //{
//                             //    abstainbtnchoice = (bool)AgmEvent.AbstainBtnChoice;
//                             //}
//                             //model.abstainBtnChoice = abstainbtnchoice;
//                             //ViewBag.abstainBtnchoice = abstainbtnchoice;
//                         }

//                     }
//                 }
//                 else
//                 {
//                     if (model.shareholder != null)
//                     {

//                         var checkLogin = WebSessionManager.CheckUserLoginHistory(model.shareholder.Company, model.shareholder.emailAddress, sessionid);
//                         if (checkLogin)
//                         {

//                             //Create new Session login for user
//                             var oldsessionversion = await WebSessionManager.CreateUserLoginHistory(model.shareholder.Company, model.shareholder.emailAddress, sessionid);
//                             //model.sessionVersion = sessionversion;
//                             //Send a refresh call to the browser
//                             Functions.LogoutPreviousPages(model.shareholder.Company, oldsessionversion);
//                         }
//                         var sessionresponse = await WebSessionManager.CheckUserLoginHistoryAsync(model.shareholder.Company, model.shareholder.emailAddress, sessionid);

//                         if (!sessionresponse)
//                         {
//                             model.ResponseMessage = "A new session on another browser has logged you out.";
//                             TempData["Portal"] = model.ResponseMessage;
//                             return RedirectToAction("Index");
//                         }
//                         var UniqueAGMId = ua.RetrieveAGMUniqueID(model.shareholder.Company);
//                         if (UniqueAGMId != -1)
//                         {
//                             var forBg = "green";
//                             var againstBg = "red";
//                             var abstainBg = "blue";
//                             var voidBg = "black";
//                             var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
//                             if (AgmEvent != null)
//                             {
//                                 model.EventStatus = AgmEvent.AgmStart;

//                                 model.EventUrl = AgmEvent.OnlineUrllink;
//                                 if (!string.IsNullOrEmpty(AgmEvent.VoteForColorBg))
//                                 {
//                                     forBg = AgmEvent.VoteForColorBg;
//                                 }
//                                 if (!string.IsNullOrEmpty(AgmEvent.VoteAgainstColorBg))
//                                 {
//                                     againstBg = AgmEvent.VoteAgainstColorBg;
//                                 }
//                                 if (!string.IsNullOrEmpty(AgmEvent.VoteAbstaincolorBg))
//                                 {
//                                     abstainBg = AgmEvent.VoteAbstaincolorBg;
//                                 }
//                                 if (!string.IsNullOrEmpty(AgmEvent.VoteVoidColorBg))
//                                 {
//                                     voidBg = AgmEvent.VoteVoidColorBg;
//                                 }
//                                 if (AgmEvent.AbstainBtnChoice != null)
//                                 {
//                                     abstainbtnchoice = (bool)AgmEvent.AbstainBtnChoice;
//                                 }
//                             }
//                             ViewBag.shareholderemail = model.shareholder.emailAddress;
//                             ViewBag.AGMTitle = model.AGMTitle.ToUpper();
//                             ViewBag.resourcetype = model.ResourceType;

//                             model.abstainBtnChoice = abstainbtnchoice;
//                             model.MessagingChoice = AgmEvent.MessagingChoice;
//                             ViewBag.abstainBtnchoice = abstainbtnchoice;
//                             ViewBag.startadmittance = db.Present.Any(p => p.AGMID == UniqueAGMId && p.ShareholderNum == model.shareholder.ShareholderNum);
//                             if (model.ResponseCode == "200")
//                             {
//                                 var oldsessionversion = await WebSessionManager.CreateUserLoginHistory(model.shareholder.Company, model.shareholder.emailAddress, sessionid);

//                             }
//                             model.UserPollStatus = await WebSessionManager.GetShareholderAttendanceStatus(UniqueAGMId, model.shareholder.emailAddress);
//                             model.UserLoginStatus = await WebSessionManager.GetShareholderLoginStatus(model.shareholder.Company, model.shareholder.emailAddress);
//                             model.sessionVersion = await WebSessionManager.GetShareholderSessionVersion(model.shareholder.Company, model.shareholder.emailAddress);
//                             ViewBag.userProxyStatus = model.UserProxyStatus;
//                             ViewBag.pollStatus = model.UserPollStatus;
//                             model.AGMID = UniqueAGMId;
//                             model.Messages = await _messages.GetAllQuestions(UniqueAGMId);
//                             model.ResponseCode = "";
//                             model.forBg = forBg;
//                             model.againstBg = againstBg;
//                             model.abstainBg = abstainBg;
//                             model.voidBg = voidBg;
//                             model.Companylogo = AgmEvent.Image != null ? "data:image/jpg;base64," +
//                              Convert.ToBase64String((byte[])AgmEvent.Image) : "";

//                         }

//                     }
//                 }


//                 return View(model);
//             }
//             else
//             {
//                 if (model != null)
//                 {
//                     TempData["Portal"] = model.ResponseMessage;
//                 }

//                 //TempData["failedresponse"] = model;
//                 return RedirectToAction("Index");
//             }

//         }
//     }
// }