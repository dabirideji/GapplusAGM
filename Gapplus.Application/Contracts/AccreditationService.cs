// using System;
// using System.Collections.Generic;
// using System.Data.SqlClient;
// using System.Diagnostics;
// using System.Linq;
// using System.Threading.Tasks;
// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;
// using Gapplus.Application.Helpers;
// using Microsoft.AspNetCore.Http;

// namespace Gapplus.Application.Contracts
// {
 
//     public class AccreditationService
//     {
//        private readonly UsersContext db;

//         public AccreditationService(UsersContext usersContext)
//         {
//             db=usersContext;
//              ua = new UserAdmin(db);
//              agmM = new AGMManager(db);
//              _messages = new MessageService(db);
            
//         }
//         Stopwatch stopwatch = new Stopwatch();
//         UserAdmin ua;
//         AGMManager agmM;
//         MessageService _messages;

//         private static string currentYear = DateTime.Now.Year.ToString();

//         // private static string connStr = DatabaseManager.GetConnectionString();
//         private static string connStr = DatabaseManager.GetConnectionString();
//         SqlConnection conn =
//                   new SqlConnection(connStr);

//         Dictionary<string, string> resourceTypes = new Dictionary<string, string>(){
//                                         {"Shareholder", "Shareholder"},
//                                         {"Non-Shareholder", "Facilitator"}
//         };


//         // GET: Accreditation
//         //[Authorize]
//         // public async Task<ActionResult> Index()
//         // {
//         //     var response = await IndexAsync();
//         //     ViewBag.portal = "Welcome to AGMLive, your one-stop hybrid shareholder meeting solution";
//         //     string changePortalText = "";
//         //     changePortalText = TempData["Portal"] as string;
//         //     if(!string.IsNullOrEmpty(changePortalText)||!string.IsNullOrWhiteSpace(changePortalText))
//         //     {
//         //         ViewBag.portal = changePortalText;
//         //     } 

//         //     return View(response);
//         // }

//         public Task<AccreditationResponse> IndexAsync()
//         {
//             try
//             {
//                 AccreditationResponse model = new AccreditationResponse
//                 {
//                     companies = db.Settings.Where(s => s.ArchiveStatus == false).Select(o => new AGMCompanies { company = o.CompanyName, description = o.Description, agmid = o.AGMID, venue = o.Venue, dateTime = o.AgmDateTime }).Distinct().OrderBy(k => k.company).ToList(),
//                     ResourceTypes = resourceTypes
//             };


//                 return Task.FromResult<AccreditationResponse>(model);

//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<AccreditationResponse>(new AccreditationResponse());
//             }

//         }

//         // public ActionResult AccreditationIndex(string companyinfo)
//         // {
//         //     var model = new accredationDto
//         //     {
//         //         company = companyinfo,
//         //         accesscode = ""
//         //     };

//         //     return PartialView(model);
//         // }


//         // public ActionResult AGMStageLogin(string companyinfo)
//         // {
//         //     ViewBag.Portal = "You have a current session on another browser.";
//         //     //var model = new accredationDto
//         //     //{
//         //     //    company = companyinfo,
//         //     //    accesscode = ""
//         //     //};

//         //     return View();
//         // }

//         // public ActionResult TermsCondition()
//         // {

//         //     return View();
//         // }



//         // public async Task<ActionResult> JoinOngoingVotingResolution(string company, string shareholdernum)
//         // {
//         //     var response = await JoinOngoingVotingResolutionAsync(company, shareholdernum);

//         //     return Json(response, JsonRequestBehavior.AllowGet);
//         // }


//         private Task<VotingResponseMessage> JoinOngoingVotingResolutionAsync(string companyinfo, string ShareholderNum)
//         {
//             try
//             {
//                 VotingResponseMessage ResponseMessage;

//                 //var votingStatus = TimerControll.GetTimeStatus(companyinfo);

//                 if (string.IsNullOrEmpty(ShareholderNum) && string.IsNullOrEmpty(companyinfo))
//                 {

//                     //Return error code;
//                     ResponseMessage = new VotingResponseMessage()
//                     {
//                         ResponseCode = "205",
//                         ResponseMessage = "REQUEST CANNOT BE PROCESSED AT THE MOMENT",
//                         Status = TimerControll.GetTimeStatus(companyinfo)
//                 };
//                     return Task.FromResult<VotingResponseMessage>(ResponseMessage);
//                 }
//                 var shareholdernum = Int64.Parse(ShareholderNum);
//                 var Uniagmid = ua.RetrieveAGMUniqueID(companyinfo);
//                 var Shareholder = db.Present.FirstOrDefault(p => p.ShareholderNum == shareholdernum);
//                 var CheckOtherAccounts = db.Present.Where(p => p.emailAddress == Shareholder.emailAddress).ToList();
//                 bool isAnyAccountProxy = false;
//                 if (CheckOtherAccounts!=null)
//                 {
//                      isAnyAccountProxy = CheckOtherAccounts.Any(c => c.proxy == true);
//                 }
                
//                 if (Shareholder == null)
//                 {
//                     //Return error code;
//                     ResponseMessage = new VotingResponseMessage()
//                     {
//                         ResponseCode = "205",
//                         ResponseMessage = "REQUEST CANNOT BE PROCESSED AT THE MOMENT",
//                         Status = TimerControll.GetTimeStatus(companyinfo)
//                     };
//                     return Task.FromResult<VotingResponseMessage>(ResponseMessage);
//                 }

//                 //if (TimerControll.GetTimeStatus(companyinfo))
//                 //{
//                     var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == Uniagmid);
//                     if (agmEvent == null)
//                     {
//                         //Return error code;
//                         ResponseMessage = new VotingResponseMessage()
//                         {
//                             ResponseCode = "205",
//                             ResponseMessage = "REQUEST CANNOT BE PROCESSED AT THE MOMENT",
//                             Status = TimerControll.GetTimeStatus(companyinfo)
//                         };
//                         return Task.FromResult<VotingResponseMessage>(ResponseMessage);
//                     }

//                     //if (Shareholder.PermitPoll == 0 && !isAnyAccountProxy && !TimerControll.GetTimeStatus(companyinfo))
//                     //{
//                     //    //Shareholder not permitted to vote.
//                     //    ResponseMessage = new VotingResponseMessage()
//                     //    {
//                     //        ResponseCode = "205",
//                     //        ResponseMessage = "RESOLUTION WILL NOT BE DISPLAYED TO YOU BECAUSE THE SYSTEM MAY HAVE ADMITTED YOU AFTER VOTING COMMENCED.",
//                     //        Status = TimerControll.GetTimeStatus(companyinfo)


//                     //    };
//                     //    return Task.FromResult<VotingResponseMessage>(ResponseMessage);
//                     //}
//                     //else if(Shareholder.PermitPoll == 0 && !isAnyAccountProxy && TimerControll.GetTimeStatus(companyinfo))
//                     //{
//                     ////Shareholder not permitted to vote.
//                     //ResponseMessage = new VotingResponseMessage()
//                     //{
//                     //    ResponseCode = "205",
//                     //    ResponseMessage = "RESOLUTION IS NOT DISPLAYED TO YOU BECAUSE THE SYSTEM MAY HAVE ADMITTED YOU AFTER VOTING COMMENCED.",
//                     //    Status = TimerControll.GetTimeStatus(companyinfo)
//                     //};
//                     //return Task.FromResult<VotingResponseMessage>(ResponseMessage);
//                     //}
//                     else if (isAnyAccountProxy && !TimerControll.GetTimeStatus(companyinfo))
//                     {
//                         //Shareholer not permitted to vote.
//                         ResponseMessage = new VotingResponseMessage()
//                         {
//                             ResponseCode = "205",
//                             ResponseMessage = "RESOLUTION WILL NOT BE DISPLAYED TO YOU BECAUSE SHAREHOLDER ALREADY VOTED BY PROXY, PROXY VOTES CANNOT BE CHANGED. THANK YOU YOUR PARTICIPATION.",
//                              Status = TimerControll.GetTimeStatus(companyinfo)
//                         };
//                         return Task.FromResult<VotingResponseMessage>(ResponseMessage);
//                     }
//                     else if(isAnyAccountProxy && TimerControll.GetTimeStatus(companyinfo))
//                     {
//                     //Shareholer not permitted to vote.
//                     ResponseMessage = new VotingResponseMessage()
//                     {
//                         ResponseCode = "205",
//                         ResponseMessage = "RESOLUTION IS NOT DISPLAYED TO YOU BECAUSE SHAREHOLDER ALREADY VOTED BY PROXY, PROXY VOTES CANNOT BE CHANGED. THANK YOU YOUR PARTICIPATION.",
//                         Status = TimerControll.GetTimeStatus(companyinfo)
//                     };
//                     return Task.FromResult<VotingResponseMessage>(ResponseMessage);
//                     }
//                     else if ((agmEvent.allChannels || agmEvent.webChannel) && TimerControll.GetTimeStatus(companyinfo))
//                     {
//                         //Send Active Resolution to Client
//                         var resolution = EventResolution(Uniagmid);
//                         ResponseMessage = new VotingResponseMessage()
//                         {
//                             ResponseCode = "200",
//                             ResponseMessage = resolution.question,
//                             ActiveResolution = resolution.question,
//                             ActiveResolutinId = resolution.Id,
//                             Company = companyinfo,
//                             Status = TimerControll.GetTimeStatus(companyinfo)
//                         };
//                         return Task.FromResult<VotingResponseMessage>(ResponseMessage);
//                     }
//                     else if ((agmEvent.allChannels || agmEvent.webChannel) && !TimerControll.GetTimeStatus(companyinfo))
//                     {
//                         //Voting has not commenced.
//                         ResponseMessage = new VotingResponseMessage()
//                         {
//                             ResponseCode = "205",
//                             ResponseMessage = "PLEASE WAIT FOR VOTING TO COMMENCE.",
//                             Status = TimerControll.GetTimeStatus(companyinfo)
//                         };
//                         return Task.FromResult<VotingResponseMessage>(ResponseMessage);
//                     }
//                     else
//                     {
//                         //voting not permitted on this channel
//                         ResponseMessage = new VotingResponseMessage()
//                         {
//                             ResponseCode = "205",
//                             ResponseMessage = "VOTING HAS BEEN DISABLED ON THIS CHANNEL BY THE FACILITATOR.",
//                             Status = TimerControll.GetTimeStatus(companyinfo)
//                         };
//                         return Task.FromResult<VotingResponseMessage>(ResponseMessage);
//                     }


//             }
//             catch (Exception e)
//             {
//                 VotingResponseMessage ResponseMessage = new VotingResponseMessage()
//                 {
//                     ResponseCode = "205",
//                     ResponseMessage = "REQUEST CANNOT BE PROCESSED AT THE MOMENT",
//                     Status = TimerControll.GetTimeStatus(companyinfo)
//                 };
//                 return Task.FromResult<VotingResponseMessage>(ResponseMessage);
//             }



//         }


//         private VoteModel EventResolution(int agmid)
//         {
//             var votemodel = new VoteModel();
//             var resolution = db.Question.SingleOrDefault(r => r.AGMID == agmid && r.questionStatus == true);
//             if (resolution != null)
//             {
//                 votemodel.question = resolution.question;
//                 votemodel.Id = resolution.Id;
//             }
//             return votemodel;
//         }

//         // public async Task<ActionResult> aElapsedTime(string id)
//         // {
//         //     var time = await aElapsedTimeAsync(id);
//         //     return PartialView(time);
//         // }


//         public Task<TimeSpan> aElapsedTimeAsync(string id)
//         {
//             TimeSpan time;

//             if (!string.IsNullOrEmpty(id))
//             {
//                 time = TimerControll.GetTime(id);
//                 return Task.FromResult<TimeSpan>(time);
//             }
//             time = new TimeSpan();
//             return Task.FromResult<TimeSpan>(time);
//         }



//         // public async Task<ActionResult> aElapsedTimeEventLoading(string id)
//         // {
//         //     var time = await aElapsedTimeEventLoadingAsync(id);

//         //     return PartialView(time);
//         // }




//         public Task<TimeSpan>aElapsedTimeEventLoadingAsync(string company)
//         {
//             TimeSpan time = new TimeSpan();
//             var currentTime = DateTime.Now;
//             if (!string.IsNullOrEmpty(company))
//             {
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
//                 if (UniqueAGMId != -1)
//                 {
//                     var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
//                     if(AgmEvent.AgmDateTime!=null)
//                     {
//                         if(AgmEvent.AgmDateTime== currentTime)
//                         {
//                             time = new TimeSpan();
//                         }
//                         else if(AgmEvent.AgmDateTime > currentTime)
//                         {
//                             time = (TimeSpan)(AgmEvent.AgmDateTime - currentTime);
//                         }
//                         else
//                         {
//                             time = new TimeSpan();
//                         }
                            
//                     }
                    
//                 }
                
//                 return Task.FromResult<TimeSpan>(time);
//             }
            
//             return Task.FromResult<TimeSpan>(time);
//         }


        
//         // public async Task<ActionResult> GetShareholderConsolidatedAccounts(string email,string company)
//         // {
           
//         //     var response = await GetShareholderConsolidatedAccountsAsync(email, company);

//         //      return PartialView(response);
//         // }

//         private Task<List<BarcodeModel>> GetShareholderConsolidatedAccountsAsync(string email, string company)
//         {

//             var accounts = db.BarcodeStore.Where(b => b.Company == company && b.emailAddress == email && b.Consolidated==true).ToList();

//             return Task.FromResult<List<BarcodeModel>>(accounts);
//         }
//         // public async Task<ActionResult> Logout()
//         // {
//         //     var response = await LogoutAsync();

//         //     TempData["Portal"] = "Thank you for attending AGM";
//         //     return RedirectToAction("Index");
//         // }

//         // private Task<List<AGMCompanies>> LogoutAsync()
//         // {
//         //     try
//         //     {
//         //         var companyNameList = db.Settings.Where(s => s.ArchiveStatus == false).Select(o => new AGMCompanies { company = o.CompanyName, description = o.Description, agmid = o.AGMID, venue = o.Venue, dateTime = o.AgmDateTime }).Distinct().OrderBy(k => k.company).ToList();
//         //       var session =   Session["Authorize"] as APIMessageLog;
//         //         if (session.ResourceType != "Facilitator")
//         //         {
//         //             WebSessionManager.LogoutUserLoginHistory(session.shareholder.Company, session.shareholder.emailAddress);
//         //             Session.Clear();
//         //             return Task.FromResult<List<AGMCompanies>>(companyNameList);
//         //         }
//         //         else
//         //         {
//         //             WebSessionManager.LogoutFacilitatorLoginHistory(session.facilitator.Company, session.facilitator.emailAddress);
//         //             Session.Clear();
//         //             return Task.FromResult<List<AGMCompanies>>(companyNameList);
//         //         }

//         //     }
//         //     catch (Exception e)
//         //     {
//         //         return Task.FromResult<List<AGMCompanies>>(new List<AGMCompanies>());
//         //     }
//         // }
        

// //         private Task<List<AGMCompanies>> LogoutAsync()
// // {
// //     try
// //     {
// //         var companyNameList = db.Settings
// //             .Where(s => s.ArchiveStatus == false)
// //             .Select(o => new AGMCompanies 
// //             { 
// //                 company = o.CompanyName, 
// //                 description = o.Description, 
// //                 agmid = o.AGMID, 
// //                 venue = o.Venue, 
// //                 dateTime = o.AgmDateTime 
// //             })
// //             .Distinct()
// //             .OrderBy(k => k.company)
// //             .ToList();
        
    
// //         var session = HttpContext.Session.GetObjectFromJson<APIMessageLog>("Authorize");

// //         if (session.ResourceType != "Facilitator")
// //         {
// //             WebSessionManager.LogoutUserLoginHistory(session.shareholder.Company, session.shareholder.emailAddress);
// //         }
// //         else
// //         {
// //             WebSessionManager.LogoutFacilitatorLoginHistory(session.facilitator.Company, session.facilitator.emailAddress);
// //         }

// //         HttpContext.Session.Clear();
        
// //         return Task.FromResult<List<AGMCompanies>>(companyNameList);
// //     }
// //     catch (Exception e)
// //     {
// //         // Log the exception
// //         return Task.FromResult<List<AGMCompanies>>(new List<AGMCompanies>());
// //     }
// // }


//         private Task<string> EventResolutionAsync(string company)
//         {
//             string question = "";
//             var resolution = db.Question.SingleOrDefault(r =>r.Company.ToLower() == company.ToLower() && r.questionStatus == true);
//             if(resolution!=null)
//             {
//                 question = resolution.question;
//             }
//             return Task.FromResult<string>(question);
//         }


//         public async Task<ActionResult> AGMEvent()
//         {

//             var sessionid = System.Web.HttpContext.Current.Session.SessionID;
            
//             var model = (APIMessageLog)Session["Authorize"];
//             var abstainbtnchoice = true;


//             if (model!=null && (model.ResponseCode == "" || model.ResponseCode=="200" || model.ResponseCode == "206"))
//             {
               
//                 if (model.ResourceType=="Facilitator")
//                 {
//                     if (model.facilitator != null)
//                     {
//                         if(model.ResponseCode == "206")
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
//                         if (model.ResponseCode == "206")
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
//                             if(AgmEvent!=null)
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
//                 if(model!=null)
//                 {
//                     TempData["Portal"] = model.ResponseMessage;
//                 }
                
//                 //TempData["failedresponse"] = model;
//                 return RedirectToAction("Index");
//             }
            
//         }


//         public async Task<ActionResult> LoginConfirmation(bool status)
//         {
//             //bool statuss = bool.Parse(status);
          
//             bool response = false;
//             var model = Session["Authorize"] as APIMessageLog;
//             if(model!=null && model.ResourceType != "Facilitator")
//             {
//                 if(model.shareholder !=null)
//                 {
//                      response = await LoginConfirmationAsync(model.shareholder.Company, model.shareholder.emailAddress,status,model.ResourceType);

//                 }            
//             }
//             else
//             {
//                 if (model.facilitator != null)
//                 {
//                     response = await LoginConfirmationAsync(model.facilitator.Company, model.facilitator.emailAddress, status, model.ResourceType);
//                 }

//             }
//             return Json(response, JsonRequestBehavior.AllowGet);


//         }


//         private Task<bool> LoginConfirmationAsync(string companyinfo,string email, bool status, string resourceType)
//         {
//             bool value = false;
//             if(resourceType!="Facilitator")
//             {
//                 value = WebSessionManager.ShareholderLoginHistoryConfirmation(companyinfo, email, status);
//             }
//             else
//             {
//                  value = WebSessionManager.FacilitatorLoginHistoryConfirmation(companyinfo, email, status);
//             }

//             return Task.FromResult<bool>(value);
//         }





//         public async Task<ActionResult> EventLoadingPage(string id)
//         {
//             var response = await EventLoadingPageAsync(id);
//             return View(response);
//         }

//         public Task<EventLoadingDto> EventLoadingPageAsync(string company)
//         {
//             var currentTime = DateTime.Now;
//             TimeSpan agmCountdown = new TimeSpan();
//             SettingsModel AgmEvent;
//             var eventLoadingmodel = new EventLoadingDto();
//             if (!string.IsNullOrEmpty(company))
//             {
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
//                 if (UniqueAGMId != -1)
//                 {
//                     AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
//                     if(AgmEvent!=null && AgmEvent.AgmDateTime!=null)
//                     {
//                         agmCountdown = (TimeSpan)(AgmEvent.AgmDateTime - currentTime);
//                         eventLoadingmodel = new EventLoadingDto()
//                         {
//                             timespan = agmCountdown,
//                             company = company,
//                             AGMDescription = AgmEvent.Description,
//                             MeetingDate = (DateTime)AgmEvent.AgmDateTime,
//                             MeetingVenue = AgmEvent.Venue
//                         };
//                     }
                    
//                 }
//             }           
//             return Task.FromResult<EventLoadingDto>(eventLoadingmodel);
//         }



//         [HttpPost]
//         public async Task<ActionResult> AccreditationConfirmation(accredationDto post)
//         {
//             var sessionid = System.Web.HttpContext.Current.Session.SessionID;

//             var response = await AccreditationConfirmationPostAsync(post, sessionid);

//             Session["Authorize"] = response;

//             return Json(response, JsonRequestBehavior.AllowGet);
//         }




//         private async Task<APIMessageLog> AccreditationConfirmationPostAsync(accredationDto model, string sessionid)
//         {
            
//             try
//             {

//                 Session.Clear();
//                 Application_BeginRequest();
//                 APIMessageLog MessageLog;
//                 AppLog log;
//                 if(!string.IsNullOrEmpty(model.accesscode) && model.accesscode.Substring(0,1)=="S")
//                 {
//                     if (!string.IsNullOrEmpty(model.company) && !string.IsNullOrEmpty(model.accesscode))
//                     {
//                         var UniqueAGMId = ua.RetrieveAGMUniqueID(model.company);
//                         var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
//                         var company = AgmEvent.CompanyName;


//                         var shareholderRecord = db.BarcodeStore.SingleOrDefault(u => u.Company.ToLower() == model.company.ToLower() && u.accesscode.ToLower() == model.accesscode.ToLower());
//                         if (shareholderRecord == null)
//                         {

//                             MessageLog = new APIMessageLog()
//                             {
//                                 Status = "Failed Accreditation",
//                                 ResponseCode = "205",
//                                 ResponseMessage = "Failed Accreditation Attempt - You may have entered a wrong Accesscode",
//                                 EventTime = DateTime.Now
//                             };
//                             log = new AppLog();
//                             log.Status = MessageLog.Status;
//                             log.ResponseCode = MessageLog.ResponseCode;
//                             log.ResponseMessage = MessageLog.ResponseMessage;
//                             log.EventTime = MessageLog.EventTime;
//                             db.AppLogs.Add(log);
//                             db.SaveChanges();
//                             return MessageLog;
//                         }
//                         else
//                         {
//                             if (shareholderRecord.Company != null && shareholderRecord.emailAddress != null)
//                             {

//                                 if (UniqueAGMId != -1)
//                                 {

//                                     if (AgmEvent != null && AgmEvent.AgmEnd == true)
//                                     {
//                                         MessageLog = new APIMessageLog()
//                                         {
//                                             Status = "Failed Accreditation",
//                                             ResponseCode = "205",
//                                             ResponseMessage = "Failed Accreditation Attempt - AGM may have ended and accesscode expired",
//                                             EventTime = DateTime.Now
//                                         };

//                                         log = new AppLog();
//                                         log.Status = MessageLog.Status;
//                                         log.ResponseCode = MessageLog.ResponseCode;
//                                         log.ResponseMessage = MessageLog.ResponseMessage;
//                                         log.EventTime = MessageLog.EventTime;
//                                         db.AppLogs.Add(log);
//                                         db.SaveChanges();
//                                         return MessageLog;
//                                     }
//                                     var shareholderRecords = db.BarcodeStore.Where(u => u.Company.ToLower() == shareholderRecord.Company.ToLower() && u.emailAddress.ToLower() == shareholderRecord.emailAddress.ToLower()).ToList();
//                                     if (shareholderRecords.Any())
//                                     {
//                                         if (shareholderRecords.Count() == 1)
//                                         {
//                                             var record = shareholderRecords.First();
                                          
//                                             var shareholder = db.Present.Any(u => u.AGMID == UniqueAGMId && u.ShareholderNum == record.ShareholderNum);

//                                                 record.Present = true;
//                                                 PresentModel present = new PresentModel();
//                                                 present.Name = record.Name;
//                                                 present.Company = record.Company;
//                                                 present.Address = record.Address;
//                                                 present.admitSource = "Web";
//                                                 present.ShareholderNum = record.ShareholderNum;
//                                                 present.Holding = record.Holding;
//                                                 present.AGMID = UniqueAGMId;
//                                                 present.PercentageHolding = record.PercentageHolding;
//                                                 present.present = true;
//                                                 present.proxy = false;
//                                             present.Year = currentYear;
//                                             present.PresentTime = DateTime.Now;
//                                                 present.Timestamp = DateTime.Now.TimeOfDay;
//                                                 present.emailAddress = record.emailAddress;
//                                             if (AgmEvent.StopAdmittance == true)
//                                             {
//                                                 present.PermitPoll = 0;
//                                             }
//                                             else if (AgmEvent.allChannels || AgmEvent.webChannel)
//                                             {
//                                                 present.PermitPoll = 1;
//                                             }
//                                             else
//                                             {
//                                                 present.PermitPoll = 0;
//                                             }
//                                             if (!String.IsNullOrEmpty(record.PhoneNumber))
//                                                 {
//                                                     if (record.PhoneNumber.StartsWith("234"))
//                                                     {
//                                                         present.PhoneNumber = record.PhoneNumber;
//                                                     }
//                                                     else if (record.PhoneNumber.StartsWith("0"))
//                                                     {
//                                                         double number;
//                                                         if (double.TryParse(record.PhoneNumber, out number))
//                                                         {
//                                                             number = double.Parse(record.PhoneNumber);
//                                                             present.PhoneNumber = "234" + number.ToString();
//                                                         }
//                                                         else
//                                                         {
//                                                             char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                                                             var phonenum = record.PhoneNumber.Split(delimiterChars);
//                                                             //string phonenumberresult = string.Concat(phonenum);
//                                                             if (double.TryParse(phonenum[0], out number))
//                                                             {
//                                                                 number = double.Parse(phonenum[0]);
//                                                                 present.PhoneNumber = "234" + number.ToString();
//                                                             }

//                                                         }

//                                                 }

//                                                 }
//                                                 present.Clikapad = record.Clikapad;

//                                             if (record.PresentByProxy != true && !shareholder && AgmEvent.StartAdmittance)
//                                             {
//                                                 db.Present.Add(present);
//                                                 record.Date = DateTime.Today.ToString();
//                                                 db.Entry(record).State = EntityState.Modified;
//                                             }

//                                             var checkLogin = WebSessionManager.CheckUserLoginHistory(record.Company, record.emailAddress, sessionid);
//                                             if (checkLogin)
//                                             {
//                                                 MessageLog = new APIMessageLog()
//                                                 {
//                                                     Status = "Login attempt Alert",
//                                                     ResponseCode = "206",
//                                                     ResponseMessage = "You may have logged in on another browser, previous login will expire. Click Continue.",
//                                                     EventTime = DateTime.Now,
//                                                     EventUrl = AgmEvent.OnlineUrllink,
//                                                     EventStatus = AgmEvent.AgmStart,
//                                                     sessionVersion = record.SessionVersion,
//                                                     UserProxyStatus = record.PresentByProxy,
//                                                     shareholder = present,
//                                                     AGMTitle = AgmEvent.Description,
//                                                     MessagingChoice = AgmEvent.MessagingChoice,
//                                                     allChannel = AgmEvent.allChannels,
//                                                     webChannel = AgmEvent.webChannel,
//                                                     mobileChannel = AgmEvent.mobileChannel,
//                                                     AGMID = UniqueAGMId,
//                                                     Messages = await _messages.GetAllQuestions(UniqueAGMId)

//                                             };

//                                                 log = new AppLog();
//                                                 log.Status = MessageLog.Status;
//                                                 log.ResponseCode = MessageLog.ResponseCode;
//                                                 log.ResponseMessage = MessageLog.ResponseMessage;
//                                                 log.EventTime = MessageLog.EventTime;
//                                                 db.AppLogs.Add(log);
//                                                 db.SaveChanges();
//                                                 return MessageLog;
//                                             }

//                                             MessageLog = new APIMessageLog()
//                                                 {
//                                                     Status = "successfully Accredited",
//                                                     ResponseCode = "200",
//                                                     ResponseMessage = "Successful Accreditation. Email Address" + " " + record.emailAddress + " " + "AccessCode:" + " " + model.accesscode,
//                                                     EventTime = DateTime.Now,
//                                                     EventUrl = AgmEvent.OnlineUrllink,
//                                                     EventStatus = AgmEvent.AgmStart,
//                                                     sessionVersion = record.SessionVersion,
//                                                     UserProxyStatus = record.PresentByProxy,
//                                                     shareholder = present,
//                                                     AGMTitle = AgmEvent.Description,
//                                                     MessagingChoice = AgmEvent.MessagingChoice,
//                                                 allChannel = AgmEvent.allChannels,
//                                                 webChannel = AgmEvent.webChannel,
//                                                 mobileChannel = AgmEvent.mobileChannel,
//                                                 AGMID = UniqueAGMId,
//                                                     Messages = await _messages.GetAllQuestions(UniqueAGMId)
//                                     };
//                                                 log = new AppLog();
//                                                 log.Status = MessageLog.Status;
//                                                 log.ResponseCode = MessageLog.ResponseCode;
//                                                 log.ResponseMessage = MessageLog.ResponseMessage;
//                                                 log.EventTime = MessageLog.EventTime;
//                                                 db.AppLogs.Add(log);
//                                                 db.SaveChanges();
//                                                 return MessageLog;

//                                         }
//                                         else if (shareholderRecords.Count() > 1)
//                                         {
//                                             var checkIfAnyAccountIsProxy = shareholderRecords.Any(s => s.PresentByProxy == true);
//                                             //var srecord = shareholderRecords.First();
//                                             BarcodeModel crecord;
//                                             var checkifConsolidate = shareholderRecords.Any(c => c.Consolidated == true);
//                                             if (!checkifConsolidate)
//                                             {
//                                                 crecord = agmM.ConsolidateRequest(company, shareholderRecords);
//                                             }
//                                             else
//                                             {
//                                                 var consolidatedvalue = shareholderRecords.Where(s => s.Consolidated == true).Select(s => s.ConsolidatedValue).FirstOrDefault();

//                                                 crecord = agmM.GetConsolidatedAccount(company,consolidatedvalue);
//                                             }
//                                             double TotalHolding = 0;
//                                             double TotalPercentageHolding = 0;

//                                             var srecord = db.BarcodeStore.Find(crecord.Id);
//                                             //foreach (var sr in shareholderRecords)
//                                             //{
//                                             var shareholder = db.Present.Any(u => u.AGMID == UniqueAGMId && u.ShareholderNum == srecord.ShareholderNum);

//                                             //string queryHolding = "SELECT SUM(Holding) FROM BarcodeModels WHERE Company ='" + model.company + "' AND emailAddress = '" + srecord.emailAddress + "'";
//                                             //conn.Open();
//                                             //SqlCommand cmd = new SqlCommand(queryHolding, conn);
//                                             ////object o = cmd.ExecuteScalar();
//                                             //if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                                             //{
//                                             //    TotalHolding = Convert.ToDouble(cmd.ExecuteScalar());

//                                             //}
//                                             //conn.Close();

//                                             //string queryPercentage = "SELECT SUM(PercentageHolding) FROM BarcodeModels WHERE Company ='" + model.company + "' AND emailAddress = '" + srecord.emailAddress + "'";
//                                             //conn.Open();
//                                             //SqlCommand pcmd = new SqlCommand(queryPercentage, conn);
//                                             ////object o = cmd.ExecuteScalar();
//                                             //if (!DBNull.Value.Equals(pcmd.ExecuteScalar()))
//                                             //{
//                                             //    TotalPercentageHolding = Convert.ToDouble(pcmd.ExecuteScalar());

//                                             //}
//                                             //conn.Close();

//                                             srecord.Present = true;
//                                             PresentModel present = new PresentModel();
//                                             present.Name = srecord.Name;
//                                             present.Company = srecord.Company;
//                                             present.Address = srecord.Address;
//                                             present.admitSource = "Web";
//                                             present.ShareholderNum = srecord.ShareholderNum;
//                                             present.Holding = srecord.Holding;
//                                             present.AGMID = UniqueAGMId;
//                                             present.PercentageHolding = srecord.PercentageHolding;
//                                             present.present = true;
//                                             present.proxy = false;
//                                             present.Year = currentYear;
//                                             present.PresentTime = DateTime.Now;
//                                             present.Timestamp = DateTime.Now.TimeOfDay;
//                                             present.emailAddress = srecord.emailAddress;
//                                             if (AgmEvent.StopAdmittance || checkIfAnyAccountIsProxy)
//                                             {
//                                                 present.PermitPoll = 0;
//                                             }
//                                             else if (AgmEvent.allChannels || AgmEvent.webChannel)
//                                             {
//                                                 present.PermitPoll = 1;
//                                             }

//                                             else
//                                             {
//                                                 present.PermitPoll = 0;
//                                             }
//                                             if (!String.IsNullOrEmpty(srecord.PhoneNumber))
//                                             {
//                                                 if (srecord.PhoneNumber.StartsWith("234"))
//                                                 {
//                                                     present.PhoneNumber = srecord.PhoneNumber;
//                                                 }
//                                                 else if (srecord.PhoneNumber.StartsWith("0"))
//                                                 {
//                                                     double number;
//                                                     if (double.TryParse(srecord.PhoneNumber, out number))
//                                                     {
//                                                         number = double.Parse(srecord.PhoneNumber);
//                                                         present.PhoneNumber = "234" + number.ToString();
//                                                     }
//                                                     else
//                                                     {
//                                                         char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                                                         var phonenum = srecord.PhoneNumber.Split(delimiterChars);
//                                                         //string phonenumberresult = string.Concat(phonenum);
//                                                         if (double.TryParse(phonenum[0], out number))
//                                                         {
//                                                             number = double.Parse(phonenum[0]);
//                                                             present.PhoneNumber = "234" + number.ToString();
//                                                         }

//                                                     }


//                                                 }

//                                             }
//                                             present.Clikapad = srecord.Clikapad;
//                                             if (!shareholder && AgmEvent.StartAdmittance)
//                                             {
//                                                 db.Present.Add(present);
//                                             srecord.Date = DateTime.Today.ToString();
//                                             db.Entry(srecord).State = EntityState.Modified;
//                                              }

//                                             var checkLogin = WebSessionManager.CheckUserLoginHistory(srecord.Company, srecord.emailAddress, sessionid);
//                                             if (checkLogin)
//                                             {
//                                                 MessageLog = new APIMessageLog()
//                                                 {
//                                                     Status = "Login Alert",
//                                                     ResponseCode = "206",
//                                                     ResponseMessage = "You may have logged in on another browser. The previous login will expire. Click Continue.",
//                                                     EventTime = DateTime.Now,
//                                                     EventUrl = AgmEvent.OnlineUrllink,
//                                                     EventStatus = AgmEvent.AgmStart,
//                                                     sessionVersion = srecord.SessionVersion,
//                                                     UserProxyStatus = checkIfAnyAccountIsProxy,
//                                                     shareholder = present,
//                                                     consolidateAccountMessage = "Your accounts were consolidated into one.",
//                                                     AGMTitle = AgmEvent.Description,
//                                                     MessagingChoice = AgmEvent.MessagingChoice,
//                                                     allChannel = AgmEvent.allChannels,
//                                                     webChannel = AgmEvent.webChannel,
//                                                     mobileChannel = AgmEvent.mobileChannel,
//                                                     AGMID = UniqueAGMId,
//                                                     Messages = await _messages.GetAllQuestions(UniqueAGMId)
//                                                 };

//                                                 log = new AppLog();
//                                                 log.Status = MessageLog.Status;
//                                                 log.ResponseCode = MessageLog.ResponseCode;
//                                                 log.ResponseMessage = MessageLog.ResponseMessage;
//                                                 log.EventTime = MessageLog.EventTime;
//                                                 db.AppLogs.Add(log);
//                                                 db.SaveChanges();
//                                                 Functions.PresentCount(UniqueAGMId, true);
//                                                 return MessageLog;
//                                             }

//                                             MessageLog = new APIMessageLog()
//                                             {
//                                                 Status = "successfully Accredited",
//                                                 ResponseCode = "200",
//                                                 ResponseMessage = "Successful Accreditation. Email Address" + " " + srecord.emailAddress + " " + "AccessCode:" + " " + model.accesscode,
//                                                 EventTime = DateTime.Now,
//                                                 EventUrl = AgmEvent.OnlineUrllink,
//                                                 EventStatus = AgmEvent.AgmStart,
//                                                 sessionVersion = srecord.SessionVersion,
//                                                 UserProxyStatus = checkIfAnyAccountIsProxy,
//                                                 shareholder = present,
//                                                 consolidateAccountMessage = "Your accounts were consolidated into one.",
//                                                 AGMTitle = AgmEvent.Description,
//                                                 MessagingChoice = AgmEvent.MessagingChoice,
//                                                 allChannel = AgmEvent.allChannels,
//                                                 webChannel = AgmEvent.webChannel,
//                                                 mobileChannel = AgmEvent.mobileChannel,
//                                                 AGMID = UniqueAGMId,
//                                                 Messages = await _messages.GetAllQuestions(UniqueAGMId)
//                                             };
//                                             log = new AppLog();
//                                             log.Status = MessageLog.Status;
//                                             log.ResponseCode = MessageLog.ResponseCode;
//                                             log.ResponseMessage = MessageLog.ResponseMessage;
//                                             log.EventTime = MessageLog.EventTime;
//                                             db.AppLogs.Add(log);
//                                             db.SaveChanges();
//                                             Functions.PresentCount(UniqueAGMId, true);
//                                             return MessageLog;

//                                         }

//                                     }
//                                     else
//                                     {
//                                         MessageLog = new APIMessageLog()
//                                         {
//                                             Status = "Failed",
//                                             ResponseCode = "205",
//                                             ResponseMessage = "Failed e-accreditation Attempt - The Company's AGM may not be listed or incorrect login credential. Email:" + " " + shareholderRecord.emailAddress + " " + "AccessCode:" + " " + model.accesscode,
//                                             EventTime = DateTime.Now
//                                         };
//                                         log = new AppLog();
//                                         log.Status = MessageLog.Status;
//                                         log.ResponseCode = MessageLog.ResponseCode;
//                                         log.ResponseMessage = MessageLog.ResponseMessage;
//                                         log.EventTime = MessageLog.EventTime;
//                                         db.AppLogs.Add(log);
//                                         db.SaveChanges();

//                                         return MessageLog;

//                                     }

//                                 }
//                                 MessageLog = new APIMessageLog()
//                                 {
//                                     Status = "Failed Accreditation",
//                                     ResponseCode = "205",
//                                     ResponseMessage = "Failed Accreditation Attempt - AGM not Active at the moment",
//                                     EventTime = DateTime.Now
//                                 };
//                                 log = new AppLog();
//                                 log.Status = MessageLog.Status;
//                                 log.ResponseCode = MessageLog.ResponseCode;
//                                 log.ResponseMessage = MessageLog.ResponseMessage;
//                                 log.EventTime = MessageLog.EventTime;
//                                 db.AppLogs.Add(log);
//                                 db.SaveChanges();
//                                 return MessageLog;
//                             }
//                             MessageLog = new APIMessageLog()
//                             {
//                                 Status = "Failed Accreditation",
//                                 ResponseCode = "205",
//                                 ResponseMessage = "Failed Accreditation Attempt - Null Entry for Company information or Accesscode",
//                                 EventTime = DateTime.Now
//                             };
//                             log = new AppLog();
//                             log.Status = MessageLog.Status;
//                             log.ResponseCode = MessageLog.ResponseCode;
//                             log.ResponseMessage = MessageLog.ResponseMessage;
//                             log.EventTime = MessageLog.EventTime;
//                             db.AppLogs.Add(log);
//                             db.SaveChanges();
//                             return MessageLog;

//                         }
//                     }
//                     MessageLog = new APIMessageLog()
//                     {
//                         Status = "Failed Accreditation",
//                         ResponseCode = "205",
//                         ResponseMessage = "Failed Accreditation Attempt - Null Entry for Company information or Accesscode",
//                         EventTime = DateTime.Now
//                     };
//                     log = new AppLog();
//                     log.Status = MessageLog.Status;
//                     log.ResponseCode = MessageLog.ResponseCode;
//                     log.ResponseMessage = MessageLog.ResponseMessage;
//                     log.EventTime = MessageLog.EventTime;
//                     db.AppLogs.Add(log);
//                     db.SaveChanges();
//                     return MessageLog;

//                 }
//                 else
//                 {
//                     if (!string.IsNullOrEmpty(model.company) && !string.IsNullOrEmpty(model.accesscode))
//                     {
//                         var UniqueAGMId = ua.RetrieveAGMUniqueID(model.company);
//                         var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

//                         if (UniqueAGMId != -1)
//                         {
//                             var FacilitatorRecord = db.Facilitators.SingleOrDefault(u => u.AGMID == UniqueAGMId && u.accesscode.ToLower() == model.accesscode.ToLower());
//                             if (FacilitatorRecord == null)
//                             {

//                                 MessageLog = new APIMessageLog()
//                                 {
//                                     Status = "Failed Accreditation",
//                                     ResponseCode = "205",
//                                     ResponseMessage = "Failed Accreditation Attempt - You may have entered a wrong Accesscode",
//                                     EventTime = DateTime.Now
//                                 };
//                                 log = new AppLog();
//                                 log.Status = MessageLog.Status;
//                                 log.ResponseCode = MessageLog.ResponseCode;
//                                 log.ResponseMessage = MessageLog.ResponseMessage;
//                                 log.EventTime = MessageLog.EventTime;
//                                 db.AppLogs.Add(log);
//                                 db.SaveChanges();
//                                 return MessageLog;
//                             }
//                             else
//                             {

//                                     if (AgmEvent != null && AgmEvent.AgmEnd == true)
//                                     {
//                                         MessageLog = new APIMessageLog()
//                                         {
//                                             Status = "Failed Accreditation",
//                                             ResponseCode = "205",
//                                             ResponseMessage = "Failed Accreditation Attempt - AGM may have ended and accesscode expired",
//                                             EventTime = DateTime.Now
//                                         };

//                                         log = new AppLog();
//                                         log.Status = MessageLog.Status;
//                                         log.ResponseCode = MessageLog.ResponseCode;
//                                         log.ResponseMessage = MessageLog.ResponseMessage;
//                                         log.EventTime = MessageLog.EventTime;
//                                         db.AppLogs.Add(log);
//                                         db.SaveChanges();
//                                         return MessageLog;
//                                     }
//                                     else
//                                     {
//                                     var checkLogin = WebSessionManager.CheckFacilitatorLoginHistory(FacilitatorRecord.Company, FacilitatorRecord.emailAddress, sessionid);
//                                     if(checkLogin)
//                                     {
//                                         MessageLog = new APIMessageLog()
//                                         {
//                                             Status = "Login Attempt Alert",
//                                             ResponseCode = "206",
//                                             ResponseMessage = "You have a session on another browser. That session will be logged out. click Continue.",
//                                             EventTime = DateTime.Now,
//                                             EventUrl = AgmEvent.OnlineUrllink,
//                                             EventStatus = AgmEvent.AgmStart,
//                                             ResourceType = "Facilitator",
//                                             facilitator = FacilitatorRecord,
//                                             sessionVersion = FacilitatorRecord.SessionVersion,
//                                             AGMTitle = AgmEvent.Description,
//                                             MessagingChoice = AgmEvent.MessagingChoice,
//                                             allChannel = AgmEvent.allChannels,
//                                             webChannel = AgmEvent.webChannel,
//                                             mobileChannel = AgmEvent.mobileChannel,
//                                             AGMID = UniqueAGMId
//                                         };
//                                         log = new AppLog();
//                                         log.Status = MessageLog.Status;
//                                         log.ResponseCode = MessageLog.ResponseCode;
//                                         log.ResponseMessage = MessageLog.ResponseMessage;
//                                         log.EventTime = MessageLog.EventTime;
//                                         db.AppLogs.Add(log);
//                                         db.SaveChanges();
//                                         return MessageLog;
//                                     }

//                                         MessageLog = new APIMessageLog()
//                                         {
//                                             Status = "successfully Accredited",
//                                             ResponseCode = "200",
//                                             ResponseMessage = "Successful Accreditation. Email Address" + " " + FacilitatorRecord.emailAddress + " " + "AccessCode:" + " " + model.accesscode,
//                                             EventTime = DateTime.Now,
//                                             EventUrl = AgmEvent.OnlineUrllink,
//                                             EventStatus = AgmEvent.AgmStart,
//                                             ResourceType = "Facilitator",
//                                             facilitator = FacilitatorRecord,
//                                             AGMTitle = AgmEvent.Description,
//                                             MessagingChoice = AgmEvent.MessagingChoice,
//                                             allChannel = AgmEvent.allChannels,
//                                             webChannel = AgmEvent.webChannel,
//                                             mobileChannel = AgmEvent.mobileChannel,
//                                             AGMID = UniqueAGMId
//                                         };
//                                         log = new AppLog();
//                                         log.Status = MessageLog.Status;
//                                         log.ResponseCode = MessageLog.ResponseCode;
//                                         log.ResponseMessage = MessageLog.ResponseMessage;
//                                         log.EventTime = MessageLog.EventTime;
//                                         db.AppLogs.Add(log);
//                                         db.SaveChanges();
//                                         return MessageLog;
//                                     }

//                                 }
//                             }
//                             MessageLog = new APIMessageLog()
//                             {
//                                 Status = "Failed Accreditation",
//                                 ResponseCode = "205",
//                                 ResponseMessage = "Failed Accreditation Attempt - AGM not active for selected company.",
//                                 EventTime = DateTime.Now
//                             };
//                             log = new AppLog();
//                             log.Status = MessageLog.Status;
//                             log.ResponseCode = MessageLog.ResponseCode;
//                             log.ResponseMessage = MessageLog.ResponseMessage;
//                             log.EventTime = MessageLog.EventTime;
//                             db.AppLogs.Add(log);
//                             db.SaveChanges();
//                             return MessageLog;

//                         }
                    
//                     MessageLog = new APIMessageLog()
//                     {
//                         Status = "Failed Accreditation",
//                         ResponseCode = "205",
//                         ResponseMessage = "Failed Accreditation Attempt - Null Entry for Company information or Accesscode",
//                         EventTime = DateTime.Now
//                     };
//                     log = new AppLog();
//                     log.Status = MessageLog.Status;
//                     log.ResponseCode = MessageLog.ResponseCode;
//                     log.ResponseMessage = MessageLog.ResponseMessage;
//                     log.EventTime = MessageLog.EventTime;
//                     db.AppLogs.Add(log);
//                     db.SaveChanges();
//                     return MessageLog;

//                 }


//             }
//             catch(Exception e)
//             {
//                 var MessageLog = new APIMessageLog()
//                 {
//                     Status = "Error",
//                     ResponseCode = "500",
//                     ResponseMessage = "Something went wrong while processing this request from" + " " + "Please contact admin",
//                     EventTime = DateTime.Now
//                 };
//                 AppLog log = new AppLog();
//                 log.Status = MessageLog.Status;
//                 log.ResponseCode = MessageLog.ResponseCode;
//                 log.ResponseMessage = MessageLog.ResponseMessage;
//                 log.EventTime = MessageLog.EventTime;
//                 db.AppLogs.Add(log);
//                 db.SaveChanges();
//                 return MessageLog;
//             }

//         }

//         [HttpPost]
//         public async Task<ActionResult> AGMQuestion(string question, string shareholdernum, string company)
//         {
//             var response = await AGMQuestionAsync(question, shareholdernum, company);

//             return Json(response, JsonRequestBehavior.AllowGet);
//         }


//         public Task<APIMessageLog> AGMQuestionAsync(string question, string shareholdernum, string company)
//         {
//             try
//             {
//                 APIMessageLog MessageLog = new APIMessageLog();
//                 //var loginShareholder = (APIMessageLog)Session["Authorize"];
            
//                 if (string.IsNullOrWhiteSpace(question) || string.IsNullOrWhiteSpace(shareholdernum))
//                 {
//                     MessageLog = new APIMessageLog
//                     {
//                         ResponseCode = "205",
//                         ResponseMessage = "No question was typed"
//                     };
//                     return Task.FromResult<APIMessageLog>(MessageLog);
//                 }

//                 long shareholderNum;
//                 if(Int64.TryParse(shareholdernum, out shareholderNum))
//                 {
//                     var loginShareholder = db.BarcodeStore.FirstOrDefault(b => b.Company.ToUpper() == company.ToUpper() && b.ShareholderNum == shareholderNum);
//                     var agmid = ua.RetrieveAGMUniqueID(company);
//                     AGMQuestion agmQ = new AGMQuestion
//                     {

//                         ShareholderName = loginShareholder.Name,
//                         shareholderquestion = question.Trim(),
//                         Company = loginShareholder.Company,
//                         datetime = DateTime.Now,
//                         ShareholderNumber = loginShareholder.ShareholderNum,
//                         emailAddress = loginShareholder.emailAddress,
//                         holding = loginShareholder.Holding,
//                         PercentageHolding = loginShareholder.PercentageHolding,
//                         phoneNumber = loginShareholder.PhoneNumber,
//                         AGMID = agmid
                        
//                     };
//                     db.AGMQuestions.Add(agmQ);
//                     db.SaveChanges();
//                     EmailManager em = new EmailManager();
//                     var response = em.SendEmail(agmQ);
//                     MessageLog = new APIMessageLog
//                     {
//                         ResponseCode = "200",
//                         ResponseMessage = "Success",
//                     };
//                     return Task.FromResult<APIMessageLog>(MessageLog);
//                 }
//                 else
//                 {
//                     MessageLog = new APIMessageLog
//                     {
//                         ResponseCode = "205",
//                         ResponseMessage = "Request Error.",
//                     };
//                     return Task.FromResult<APIMessageLog>(MessageLog);
//                 }   
             

//             }
//             catch (Exception e)
//             {
//                 var messagelog = new APIMessageLog()
//                 {
//                     ResponseCode = "205",
//                     ResponseMessage = e.Message
//                 };
//                 return Task.FromResult<APIMessageLog>(messagelog);
//             }
            
//         }

//         //public string DangoteEncryptIndex()
//         //{
//         //    var query = string.Format("{0}|{1}", "johnfemy@yahoo.com",2);
//         //    var encryptedtext = query.Encrypt();
//         //    return encryptedtext;
//         //}

//         //public string AccessEncryptIndex()
//         //{
//         //    var query = string.Format("{0}|{1}|{2}","johnfemy@yahoo.com",5);
//         //    var encryptedtext = query.Encrypt();
//         //    return encryptedtext;
//         //}
//         protected void Application_BeginRequest()
//         {
//             Response.Cache.SetCacheability(HttpCacheability.NoCache);
//             Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
//             Response.Cache.SetNoStore();
//         }


//         public async Task<ActionResult> AccreditationConfirmation(string query)
//         {
//             if (string.IsNullOrEmpty(query))
//             {
//                 return RedirectToAction("Index");
//             }
//             var sessionid = System.Web.HttpContext.Current.Session.SessionID;
//             var model = await AccreditationConfirmationAsync(query, sessionid);
//             var abstainbtnchoice = true;

//             Session["Authorize"] = model;
//             if (model.ResponseCode == "206")
//             {
//                 return RedirectToAction("AGMStageLogin");
//             }
//             else if(model.ResponseCode == "200")
//             {
//                 var forBg = "green";
//                 var againstBg = "red";
//                 var abstainBg = "blue";
//                 var voidBg = "black";
//                 var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == model.AGMID);
//                 if (AgmEvent != null)
//                 {
//                     model.EventStatus = AgmEvent.AgmStart;

//                     model.EventUrl = AgmEvent.OnlineUrllink;
//                     if (!string.IsNullOrEmpty(AgmEvent.VoteForColorBg))
//                     {
//                         forBg = AgmEvent.VoteForColorBg;
//                     }
//                     if (!string.IsNullOrEmpty(AgmEvent.VoteAgainstColorBg))
//                     {
//                         againstBg = AgmEvent.VoteAgainstColorBg;
//                     }
//                     if (!string.IsNullOrEmpty(AgmEvent.VoteAbstaincolorBg))
//                     {
//                         abstainBg = AgmEvent.VoteAbstaincolorBg;
//                     }
//                     if (!string.IsNullOrEmpty(AgmEvent.VoteVoidColorBg))
//                     {
//                         voidBg = AgmEvent.VoteVoidColorBg;
//                     }
//                     if (AgmEvent.AbstainBtnChoice != null)
//                     {
//                         abstainbtnchoice = (bool)AgmEvent.AbstainBtnChoice;
//                     }
//                 }
//                 ViewBag.shareholderemail = model.shareholder.emailAddress;
//                 ViewBag.AGMTitle = model.AGMTitle.ToUpper();
//                 ViewBag.resourcetype = model.ResourceType;

//                 model.abstainBtnChoice = abstainbtnchoice;
//                 model.MessagingChoice = AgmEvent.MessagingChoice;
//                 model.allChannel = AgmEvent.allChannels;
//                 model.webChannel = AgmEvent.webChannel;
//                 model.mobileChannel = AgmEvent.mobileChannel;
//                 ViewBag.abstainBtnchoice = abstainbtnchoice;

//                 ViewBag.startadmittance = db.Present.Any(p => p.AGMID == model.AGMID && p.ShareholderNum == model.shareholder.ShareholderNum);
//                 if (model.ResponseCode == "200")
//                 {
//                     var oldsessionversion = await WebSessionManager.CreateUserLoginHistory(model.shareholder.Company, model.shareholder.emailAddress, sessionid);

//                 }
//                 model.UserPollStatus = await WebSessionManager.GetShareholderAttendanceStatus(model.AGMID, model.shareholder.emailAddress);
//                 model.UserLoginStatus = await WebSessionManager.GetShareholderLoginStatus(model.shareholder.Company, model.shareholder.emailAddress);
//                 model.sessionVersion = await WebSessionManager.GetShareholderSessionVersion(model.shareholder.Company, model.shareholder.emailAddress);
//                 ViewBag.userProxyStatus = model.UserProxyStatus;
//                 ViewBag.pollStatus = model.UserPollStatus;
//                 model.Messages = await _messages.GetAllQuestions(model.AGMID);
//                 model.ResponseCode = "";
//                 model.forBg = forBg;
//                 model.againstBg = againstBg;
//                 model.abstainBg = abstainBg;
//                 model.voidBg = voidBg;
//                 model.Companylogo = AgmEvent.Image != null ? "data:image/jpg;base64," +
//                  Convert.ToBase64String((byte[])AgmEvent.Image) : "";
//                 //}

//                 return View(model);
//             }
//             else
//             {
//                 return RedirectToAction("Index");
//             }
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID(model.shareholder.Company);
//             //if (UniqueAGMId != -1)
//             //{
            
//             //return RedirectToAction("AGMEvent","Accreditation");
//         }


//         public async Task<APIMessageLog> AccreditationConfirmationAsync(string encryptedText, string sessionid)
//         {
//             //var query = new VoteModel();
//             string companyinfo;
//             string emailAddress ="";
//             int agmid;
//             APIMessageLog MessageLog;
//             AppLog log;

//             try
//                 {

//                 Session.Clear();
//                 Application_BeginRequest();

//                 if (!string.IsNullOrEmpty(encryptedText))
//                 {
//                     var replacencryptedString = encryptedText.Replace(" ", "+");
//                     var decryptedtext = replacencryptedString.Decrypt();
//                     var querystring = decryptedtext.Split('|');
//                     emailAddress = querystring[0].Trim();
//                     //companyinfo = querystring[1].Trim();
//                     agmid = int.Parse(querystring[1].Trim());

//                     //VoteModel query = (VoteModel)decryptedtext;


//                     //companyinfo = query.company;
//                     //emailAddress = query.identity;
//                     //agmid = query.agmid;

//                     var agmstatus = db.Settings.FirstOrDefault(s => s.AGMID == agmid);
                    
//                     if (agmstatus != null)
//                     {

//                         if ( agmstatus.AgmEnd == true)
//                         {
//                             MessageLog = new APIMessageLog()
//                             {
//                                 Status = "Failed Accreditation",
//                                 ResponseCode = "205",
//                                 ResponseMessage = "Failed Accreditation Attempt - AGM may have ended and accesscode expired",
//                                 EventTime = DateTime.Now
//                             };
//                             log = new AppLog();
//                             log.Status = MessageLog.Status;
//                             log.ResponseCode = MessageLog.ResponseCode;
//                             log.ResponseMessage = MessageLog.ResponseMessage;
//                             log.EventTime = MessageLog.EventTime;
//                             db.AppLogs.Add(log);
//                             db.SaveChanges();
//                             return MessageLog;
//                         }
//                         if (agmstatus.ArchiveStatus)
//                         {
//                             var MessageLog5 = new APIMessageLog()
//                             {
//                                 Status = "Failed Accreditation",
//                                 ResponseCode = "205",
//                                 ResponseMessage = "Failed Accreditation Attempt - Link may have been closed or expired. Email:" + " " + emailAddress,
//                                 EventTime = DateTime.Now
//                             };
//                             log = new AppLog();
//                             log.Status = MessageLog5.Status;
//                             log.ResponseCode = MessageLog5.ResponseCode;
//                             log.ResponseMessage = MessageLog5.ResponseMessage;
//                             log.EventTime = MessageLog5.EventTime;
//                             db.AppLogs.Add(log);
//                             db.SaveChanges();
//                             return MessageLog5;
//                         }


//                         if (!string.IsNullOrEmpty(agmstatus.CompanyName) && !string.IsNullOrEmpty(emailAddress))
//                         {

//                             companyinfo = agmstatus.CompanyName;
//                             var UniqueAGMId = agmid;
//                             if (UniqueAGMId != -1)
//                             {
//                                 var shareholderRecords = db.BarcodeStore.Where(u => u.Company.ToLower() == companyinfo.ToLower() && u.emailAddress.ToLower() == emailAddress.ToLower()).ToList();
//                                 if (shareholderRecords.Any())
//                                 {
//                                     if (shareholderRecords.Count() == 1)
//                                     {
//                                         var record = shareholderRecords.First();

//                                         var shareholder = db.Present.Any(u => u.AGMID == UniqueAGMId && u.ShareholderNum == record.ShareholderNum);

//                                             record.Present = true;
//                                             PresentModel present = new PresentModel();
//                                             present.Name = record.Name;
//                                             present.Company = record.Company;
//                                             present.Address = record.Address;
//                                             present.admitSource = "Web";
//                                             present.ShareholderNum = record.ShareholderNum;
//                                             present.Holding = record.Holding;
//                                             present.AGMID = UniqueAGMId;
//                                             present.PercentageHolding = record.PercentageHolding;
//                                             present.present = true;
//                                         present.Year = currentYear;
//                                         present.proxy = false;
//                                             present.PresentTime = DateTime.Now;
//                                             present.Timestamp = DateTime.Now.TimeOfDay;
//                                             present.emailAddress = record.emailAddress;
//                                           if (agmstatus.StopAdmittance == true)
//                                             {
//                                                 present.PermitPoll = 0;
//                                             }
//                                              else if (agmstatus.allChannels || agmstatus.webChannel)
//                                             {
//                                                 present.PermitPoll = 1;
//                                             }

//                                             else
//                                             {
//                                                 present.PermitPoll = 0;
//                                             }
//                                             if (!String.IsNullOrEmpty(record.PhoneNumber))
//                                             {
//                                                 if (record.PhoneNumber.StartsWith("234"))
//                                                 {
//                                                     present.PhoneNumber = record.PhoneNumber;
//                                                 }
//                                                 else if (record.PhoneNumber.StartsWith("0"))
//                                                 {
//                                                     double number;
//                                                     if (double.TryParse(record.PhoneNumber, out number))
//                                                     {
//                                                         number = double.Parse(record.PhoneNumber);
//                                                         present.PhoneNumber = "234" + number.ToString();
//                                                     }
//                                                     else
//                                                     {
//                                                         char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                                                         var phonenum = record.PhoneNumber.Split(delimiterChars);
//                                                         //string phonenumberresult = string.Concat(phonenum);
//                                                         if (double.TryParse(phonenum[0], out number))
//                                                         {
//                                                             number = double.Parse(phonenum[0]);
//                                                             present.PhoneNumber = "234" + number.ToString();
//                                                         }


//                                                 }

//                                             }

//                                             }
//                                             present.Clikapad = record.Clikapad;
//                                         if (record.PresentByProxy != true && !shareholder && agmstatus.StartAdmittance)
//                                         {
//                                             db.Present.Add(present);                     
//                                         }
//                                         record.Date = DateTime.Today.ToString();
//                                         db.Entry(record).State = EntityState.Modified;

//                                         var checkLogin = WebSessionManager.CheckUserLoginHistory(record.Company, record.emailAddress, sessionid);
//                                             if (checkLogin)
//                                             {

//                                                 MessageLog = new APIMessageLog()
//                                                 {
//                                                     Status = "Failed Login",
//                                                     ResponseCode = "206",
//                                                     ResponseMessage = "Failed Login Attempt - you may have logged in on another device",
//                                                     EventTime = DateTime.Now,
//                                                     EventUrl = agmstatus.OnlineUrllink,
//                                                     EventStatus = agmstatus.AgmStart,
//                                                     sessionVersion = record.SessionVersion,
//                                                     UserProxyStatus = record.PresentByProxy,
//                                                     shareholder = present,
//                                                     AGMTitle = agmstatus.Description,
//                                                     MessagingChoice = agmstatus.MessagingChoice,
//                                                     allChannel = agmstatus.allChannels,
//                                                     webChannel = agmstatus.webChannel,
//                                                     mobileChannel = agmstatus.mobileChannel,
//                                                     AGMID = UniqueAGMId,
//                                                     Messages = await _messages.GetAllQuestions(UniqueAGMId)
//                                                 };

//                                                 log = new AppLog();
//                                                 log.Status = MessageLog.Status;
//                                                 log.ResponseCode = MessageLog.ResponseCode;
//                                                 log.ResponseMessage = MessageLog.ResponseMessage;
//                                                 log.EventTime = MessageLog.EventTime;
//                                                 db.AppLogs.Add(log);
//                                                 db.SaveChanges();
//                                             Functions.PresentCount(UniqueAGMId, true);
//                                             return MessageLog;
//                                             }
//                                             MessageLog = new APIMessageLog()
//                                             {
//                                                 Status = "successfully Accredited",
//                                                 ResponseCode = "200",
//                                                 ResponseMessage = "Successful Accreditation. Email Address" + " " + emailAddress,
//                                                 EventTime = DateTime.Now,
//                                                 EventUrl = agmstatus.OnlineUrllink,
//                                                 EventStatus = agmstatus.AgmStart,
//                                                 shareholder = present,
//                                                 UserProxyStatus = record.PresentByProxy,
//                                                 AGMTitle = agmstatus.Description,
//                                                 MessagingChoice = agmstatus.MessagingChoice,
//                                                 allChannel = agmstatus.allChannels,
//                                                 webChannel = agmstatus.webChannel,
//                                                 mobileChannel = agmstatus.mobileChannel,
//                                                 AGMID = UniqueAGMId,
//                                                 Messages = await _messages.GetAllQuestions(UniqueAGMId)
//                                             };
//                                             log = new AppLog();
//                                             log.Status = MessageLog.Status;
//                                             log.ResponseCode = MessageLog.ResponseCode;
//                                             log.ResponseMessage = MessageLog.ResponseMessage;
//                                             log.EventTime = MessageLog.EventTime;
//                                             db.AppLogs.Add(log);
//                                             db.SaveChanges();
//                                         Functions.PresentCount(UniqueAGMId, true);
//                                         return MessageLog;

//                                     }
//                                     else if (shareholderRecords.Count() > 1)
//                                     {
//                                         double TotalHolding = 0;
//                                         double TotalPercentageHolding = 0;
//                                         BarcodeModel crecord;

//                                         var checkIfAnyAccountIsProxy = shareholderRecords.Any(s => s.PresentByProxy == true);
//                                         var checkifConsolidate = shareholderRecords.Any(c => c.Consolidated == true);
//                                         if(!checkifConsolidate)
//                                         {
//                                             crecord = agmM.ConsolidateRequest(companyinfo, shareholderRecords);
//                                         }
//                                         else
//                                         {
//                                             var consolidatedvalue = shareholderRecords.Where(s => s.Consolidated == true).Select(s => s.ConsolidatedValue).FirstOrDefault();

//                                             crecord = agmM.GetConsolidatedAccount(companyinfo, consolidatedvalue);
//                                         }


//                                         var srecord = db.BarcodeStore.Find(crecord.Id);
//                                         var shareholder = db.Present.Any(u => u.AGMID == UniqueAGMId && u.ShareholderNum == srecord.ShareholderNum);


//                                         srecord.Present = true;
//                                         PresentModel present = new PresentModel();
//                                         present.Name = srecord.Name;
//                                         present.Company = srecord.Company;
//                                         present.Address = srecord.Address;
//                                         present.admitSource = "Web";
//                                         present.ShareholderNum = srecord.ShareholderNum;
//                                         present.Holding = srecord.Holding;
//                                         present.AGMID = UniqueAGMId;
//                                         present.PercentageHolding = srecord.PercentageHolding;
//                                         present.present = true;
//                                         present.proxy = false;
//                                         present.Year = currentYear;
//                                         present.PresentTime = DateTime.Now;
//                                         present.Timestamp = DateTime.Now.TimeOfDay;
//                                         present.emailAddress = srecord.emailAddress;

//                                         if (agmstatus.StopAdmittance == true || checkIfAnyAccountIsProxy)
//                                         {
//                                             present.PermitPoll = 0;
//                                         }
//                                         else if (agmstatus.allChannels || agmstatus.webChannel)
//                                         {
//                                             present.PermitPoll = 1;
//                                         }
//                                         else
//                                         {
//                                             present.PermitPoll = 0;
//                                         }

//                                         if (!String.IsNullOrEmpty(srecord.PhoneNumber))
//                                         {
//                                             if (srecord.PhoneNumber.StartsWith("234"))
//                                             {
//                                                 present.PhoneNumber = srecord.PhoneNumber;
//                                             }
//                                             else if (srecord.PhoneNumber.StartsWith("0"))
//                                             {
//                                                 double number;
//                                                 if (double.TryParse(srecord.PhoneNumber, out number))
//                                                 {
//                                                     number = double.Parse(srecord.PhoneNumber);
//                                                     present.PhoneNumber = "234" + number.ToString();
//                                                 }
//                                                 else
//                                                 {
//                                                     char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                                                     var phonenum = srecord.PhoneNumber.Split(delimiterChars);
//                                                     //string phonenumberresult = string.Concat(phonenum);
//                                                     if (double.TryParse(phonenum[0], out number))
//                                                     {
//                                                         number = double.Parse(phonenum[0]);
//                                                         present.PhoneNumber = "234" + number.ToString();
//                                                     }

//                                                 }

//                                             }

//                                         }
//                                         present.Clikapad = srecord.Clikapad;
//                                         if (!shareholder && agmstatus.StartAdmittance)
//                                         {
//                                             db.Present.Add(present);
//                                         srecord.Date = DateTime.Today.ToString();
//                                         db.Entry(srecord).State = EntityState.Modified;
//                                         }
//                                         var checkLogin = WebSessionManager.CheckUserLoginHistory(srecord.Company, srecord.emailAddress, sessionid);
//                                         if (checkLogin)
//                                         {
//                                             MessageLog = new APIMessageLog()
//                                             {
//                                                 Status = "Failed Login",
//                                                 ResponseCode = "206",
//                                                 ResponseMessage = "Failed Login Attempt - you may have logged in on another device",
//                                                 EventTime = DateTime.Now,
//                                                 EventUrl = agmstatus.OnlineUrllink,
//                                                 EventStatus = agmstatus.AgmStart,
//                                                 sessionVersion = srecord.SessionVersion,
//                                                 shareholder = present,
//                                                 UserProxyStatus = checkIfAnyAccountIsProxy,
//                                                 consolidateAccountMessage = "Your accounts were consolidated into one.",
//                                                 AGMTitle = agmstatus.Description,
//                                                 MessagingChoice = agmstatus.MessagingChoice,
//                                                 allChannel = agmstatus.allChannels,
//                                                 webChannel = agmstatus.webChannel,
//                                                 mobileChannel = agmstatus.mobileChannel,
//                                                 AGMID = UniqueAGMId,
//                                                 Messages = await _messages.GetAllQuestions(UniqueAGMId)
//                                             };

//                                             log = new AppLog();
//                                             log.Status = MessageLog.Status;
//                                             log.ResponseCode = MessageLog.ResponseCode;
//                                             log.ResponseMessage = MessageLog.ResponseMessage;
//                                             log.EventTime = MessageLog.EventTime;
//                                             db.AppLogs.Add(log);
//                                             db.SaveChanges();
//                                             Functions.PresentCount(UniqueAGMId, true);
//                                             return MessageLog;
//                                         }


//                                         MessageLog = new APIMessageLog()
//                                             {
//                                                 Status = "successfully Accredited",
//                                                 ResponseCode = "200",
//                                                 ResponseMessage = "Successful Accreditation. Email Address" + " " + emailAddress,
//                                                 EventTime = DateTime.Now,
//                                                 EventUrl = agmstatus.OnlineUrllink,
//                                                 EventStatus = agmstatus.AgmStart,
//                                                 sessionVersion = srecord.SessionVersion,
//                                             shareholder = present,
//                                             UserProxyStatus = checkIfAnyAccountIsProxy,
//                                             consolidateAccountMessage = "Your accounts were consolidated into one.",
//                                                 AGMTitle = agmstatus.Description,
//                                             MessagingChoice = agmstatus.MessagingChoice,
//                                             allChannel = agmstatus.allChannels,
//                                             webChannel = agmstatus.webChannel,
//                                             mobileChannel = agmstatus.mobileChannel,
//                                             AGMID = UniqueAGMId,
//                                             Messages = await _messages.GetAllQuestions(UniqueAGMId)
//                                         };
//                                             log = new AppLog();
//                                             log.Status = MessageLog.Status;
//                                             log.ResponseCode = MessageLog.ResponseCode;
//                                             log.ResponseMessage = MessageLog.ResponseMessage;
//                                             log.EventTime = MessageLog.EventTime;
//                                             db.AppLogs.Add(log);
//                                             db.SaveChanges();
//                                         Functions.PresentCount(UniqueAGMId, true);
//                                         return MessageLog;

//                                     }
  
//                                 }
//                                 else
//                                 {
//                                     MessageLog = new APIMessageLog()
//                                     {
//                                         Status = "Failed",
//                                         ResponseCode = "205",
//                                         ResponseMessage = "Failed e-accreditation Attempt - The Company's AGM may not be listed or incorrect login credential. Email:" + " " + emailAddress,
//                                         EventTime = DateTime.Now
//                                     };
//                                     log = new AppLog();
//                                     log.Status = MessageLog.Status;
//                                     log.ResponseCode = MessageLog.ResponseCode;
//                                     log.ResponseMessage = MessageLog.ResponseMessage;
//                                     log.EventTime = MessageLog.EventTime;
//                                     db.AppLogs.Add(log);
//                                     db.SaveChanges();

//                                     return MessageLog;

//                                 }
//                             }
//                             else
//                             {
//                                 MessageLog = new APIMessageLog()
//                                 {
//                                     Status = "Failed Accreditation",
//                                     ResponseCode = "205",
//                                     ResponseMessage = "Failed Accreditation Attempt - AGM not available for company selected. Email:" + " " + emailAddress,
//                                     EventTime = DateTime.Now
//                                 };
//                                 log = new AppLog();
//                                 log.Status = MessageLog.Status;
//                                 log.ResponseCode = MessageLog.ResponseCode;
//                                 log.ResponseMessage = MessageLog.ResponseMessage;
//                                 log.EventTime = MessageLog.EventTime;
//                                 db.AppLogs.Add(log);
//                                 db.SaveChanges();

//                                 return MessageLog;
//                             }

//                         }
//                         else
//                         {
//                             MessageLog = new APIMessageLog()
//                             {
//                                 Status = "Failed Accreditation",
//                                 ResponseCode = "205",
//                                 ResponseMessage = "Failed Accreditation Attempt - Empty parameter supplied. Email:" + " " + emailAddress,
//                                 EventTime = DateTime.Now
//                             };
//                             log = new AppLog();
//                             log.Status = MessageLog.Status;
//                             log.ResponseCode = MessageLog.ResponseCode;
//                             log.ResponseMessage = MessageLog.ResponseMessage;
//                             log.EventTime = MessageLog.EventTime;
//                             db.AppLogs.Add(log);
//                             db.SaveChanges();
//                             return MessageLog;
//                         }

//                     }
//                     else
//                     {
//                         MessageLog = new APIMessageLog()
//                         {
//                             Status = "Failed Accreditation",
//                             ResponseCode = "205",
//                             ResponseMessage = "Failed Accreditation Attempt - AGM access to this url may have expired",
//                             EventTime = DateTime.Now
//                         };
//                         log = new AppLog();
//                         log.Status = MessageLog.Status;
//                         log.ResponseCode = MessageLog.ResponseCode;
//                         log.ResponseMessage = MessageLog.ResponseMessage;
//                         log.EventTime = MessageLog.EventTime;
//                         db.AppLogs.Add(log);
//                         db.SaveChanges();

//                         return MessageLog;
//                     }
//                 }
//                 MessageLog = new APIMessageLog()
//                 {
//                     Status = "Failed Accreditation",
//                     ResponseCode = "205",
//                     ResponseMessage = "Failed Accreditation Attempt - Information received cannot be proccessed" + " " + "Please go through pre-registration portal.",
//                     EventTime = DateTime.Now
//                 };
//                 log = new AppLog();
//                 log.Status = MessageLog.Status;
//                 log.ResponseCode = MessageLog.ResponseCode;
//                 log.ResponseMessage = MessageLog.ResponseMessage;
//                 log.EventTime = MessageLog.EventTime;
//                 db.AppLogs.Add(log);
//                 db.SaveChanges();

//                 return MessageLog;
//             }
//            catch (Exception e)
//            {
//                     var MessageLog7 = new APIMessageLog()
//                     {
//                         Status = "Error",
//                         ResponseCode = "500",
//                         ResponseMessage = "System cannot process your reqeuest at this time " + emailAddress + " " + "Please contact admin",
//                         EventTime = DateTime.Now
//                     };
//                 log = new AppLog();
//                 log.Status = MessageLog7.Status;
//                 log.ResponseCode = MessageLog7.ResponseCode;
//                 log.ResponseMessage = MessageLog7.ResponseMessage;
//                 log.EventTime = MessageLog7.EventTime;
//                 db.AppLogs.Add(log);
//                 db.SaveChanges();
//                     return MessageLog7;

//                 }
//             }

//         //private BarcodeModel GetConsolidatedAccount(string company , string consolidatedvalue)
//         //{
//         //    var shareholdernum = Int64.Parse(consolidatedvalue);
//         //    var shareholder = db.BarcodeStore.FirstOrDefault(s =>s.Company == company &&  s.ShareholderNum == shareholdernum);

//         //    return shareholder;
//         //}



//         //private BarcodeModel ConsolidateRequest(string companyinfo, List<BarcodeModel> consolidees)
//         //{
//         //    try
//         //    {
//         //        //var companyinfo = ua.GetUserCompanyInfo();
//         //        //var jvalues = JsonConvert.DeserializeObject(val);
//         //        //JArray a = JArray.Parse(val);

//         //        //Jvar IdsJson = Json.Parse(jvalues);
//         //        //var value = jvalues.Split(',');
//         //        string connStr = DatabaseManager.GetConnectionString();
//         //        Int64 maxvalue = 0;
//         //        SqlConnection conn =
//         //                new SqlConnection(connStr);
//         //        string query = "SELECT MAX(ShareholderNum) FROM BarcodeModels WHERE Company='" + companyinfo + "'";
//         //        //string query2 = "select * from BarcodeModels WHERE Name LIKE '%" + searchValue + "%' OR ShareholderNum LIKE '%" + searchValue + "%'";
//         //        conn.Open();
//         //        SqlCommand cmd = new SqlCommand(query, conn);
//         //        SqlDataReader read = cmd.ExecuteReader();

//         //        //Int64 maxvalue = 0;
//         //        while (read.Read())
//         //        {
//         //            maxvalue = read.GetInt64(0);
//         //        }
//         //        read.Close();
//         //        conn.Close();

//         //        var setting = db.Settings.ToArray();
//         //        //var ConsolidatedAccounts = db.BarcodeStore.Where(b => b.Company == companyinfo && b.Selected == true);
//         //        var ConsolidatedAccounts = consolidees;
//         //        List<BarcodeModel> ConsolidatedList = new List<BarcodeModel>();
//         //        ConsolidatedList.Clear();
//         //        //var consolidatedIds = mod;
//         //        //
//         //        foreach (var item in ConsolidatedAccounts)
//         //        {
//         //            var id = item.Id;
//         //            var consolidatedItem = db.BarcodeStore.Find(id);
//         //            if (consolidatedItem != null)
//         //            {

//         //                consolidatedItem.Consolidated = true;
//         //                consolidatedItem.ConsolidatedValue = (maxvalue + 1).ToString();
//         //                consolidatedItem.Selected = false;
//         //                consolidatedItem.NotVerifiable = true;
//         //                //consolidatedItem.PresentByProxy = true;
//         //                //consolidatedItem.AddedSplitAccount = false;
//         //                consolidatedItem.split = false;

//         //                ConsolidatedList.Add(consolidatedItem);

//         //            }
//         //        }
//         //        //foreach (var consolidatedAcount in ConsolidatedAccounts)
//         //        //{
//         //        //    var id = consolidatedAcount.Id;

//         //        //}         

//         //        BarcodeModel consolidatedAccount = new BarcodeModel();
//         //        foreach (var item in ConsolidatedList)
//         //        {
//         //            consolidatedAccount.Holding += item.Holding;
//         //            consolidatedAccount.PercentageHolding += item.PercentageHolding;
//         //        }
//         //        consolidatedAccount.Name = ConsolidatedList[0].Name;
//         //        consolidatedAccount.Address = ConsolidatedList[0].Address;
//         //        consolidatedAccount.emailAddress = ConsolidatedList[0].emailAddress;
//         //        consolidatedAccount.Address = ConsolidatedList[0].Address;
//         //        consolidatedAccount.ConsolidatedValue = (maxvalue + 1).ToString();
//         //        consolidatedAccount.ShareholderNum = (maxvalue + 1);
//         //        consolidatedAccount.ConsolidatedParent = "true";
//         //        consolidatedAccount.Company = ConsolidatedList[0].Company;
//         //        //consolidatedAccount.PresentByProxy = false;
//         //        consolidatedAccount.PhoneNumber = ConsolidatedList[0].PhoneNumber;

//         //        db.BarcodeStore.Add(consolidatedAccount);


//         //        db.SaveChanges();

//         //        return consolidatedAccount;
//         //    }
//         //    catch (Exception e)
//         //    {
//         //        return new BarcodeModel();
//         //    }
//         //}


//         public async Task<ActionResult> AutoAdmittance(string company, string shareholderNum)
//         {
//             var response = await AutoAdmittanceAsync(company, shareholderNum);

//             return Json(response, JsonRequestBehavior.AllowGet);
//         }





        
//         private Task<string> AutoAdmittanceAsync(string company,string shareholderNum)
//         {
//             if(!string.IsNullOrEmpty(company) && !string.IsNullOrEmpty(shareholderNum))
//             {
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
//                 if(UniqueAGMId== -1)
//                 {
//                     return Task.FromResult<string>("Failed");
//                 }
//                 var agmstatus = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
//                 if(agmstatus == null)
//                 {
//                     return Task.FromResult<string>("Failed");
//                 }
//                 var sHolderNum = Int64.Parse(shareholderNum);
//                 var sholder = db.BarcodeStore.FirstOrDefault(b => b.Company == company && b.ShareholderNum == sHolderNum);
//                 var shareholderRecords = db.BarcodeStore.Where(u => u.Company.ToLower() == company.ToLower() && u.emailAddress.ToLower() == sholder.emailAddress.ToLower()).ToList();
//                 if (shareholderRecords.Any())
//                 {
//                     if (shareholderRecords.Count() == 1)
//                     {
//                         var record = shareholderRecords.First();

//                         var shareholder = db.Present.Any(u => u.AGMID == UniqueAGMId && u.ShareholderNum == record.ShareholderNum);

//                         record.Present = true;
//                         PresentModel present = new PresentModel();
//                         present.Name = record.Name;
//                         present.Company = record.Company;
//                         present.Address = record.Address;
//                         present.admitSource = "Web";
//                         present.ShareholderNum = record.ShareholderNum;
//                         present.Holding = record.Holding;
//                         present.AGMID = UniqueAGMId;
//                         present.PercentageHolding = record.PercentageHolding;
//                         present.present = true;
//                         present.Year = currentYear;
//                         present.proxy = false;
//                         present.PresentTime = DateTime.Now;
//                         present.Timestamp = DateTime.Now.TimeOfDay;
//                         present.emailAddress = record.emailAddress;
//                         if (agmstatus.StopAdmittance == true)
//                         {
//                             present.PermitPoll = 0;
//                         }
//                         else if (agmstatus.allChannels || agmstatus.webChannel)
//                         {
//                             present.PermitPoll = 1;
//                         }

//                         else
//                         {
//                             present.PermitPoll = 0;
//                         }
//                         if (!String.IsNullOrEmpty(record.PhoneNumber))
//                         {
//                             if (record.PhoneNumber.StartsWith("234"))
//                             {
//                                 present.PhoneNumber = record.PhoneNumber;
//                             }
//                             else if (record.PhoneNumber.StartsWith("0"))
//                             {
//                                 double number;
//                                 if (double.TryParse(record.PhoneNumber, out number))
//                                 {
//                                     number = double.Parse(record.PhoneNumber);
//                                     present.PhoneNumber = "234" + number.ToString();
//                                 }
//                                 else
//                                 {
//                                     char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                                     var phonenum = record.PhoneNumber.Split(delimiterChars);
//                                     //string phonenumberresult = string.Concat(phonenum);
//                                     if (double.TryParse(phonenum[0], out number))
//                                     {
//                                         number = double.Parse(phonenum[0]);
//                                         present.PhoneNumber = "234" + number.ToString();
//                                     }

//                                 }

//                             }

//                         }
//                         present.Clikapad = record.Clikapad;
//                         if (record.PresentByProxy != true && !shareholder && agmstatus.StartAdmittance)
//                         {
//                             db.Present.Add(present);
//                             record.Date = DateTime.Today.ToString();
//                             db.Entry(record).State = EntityState.Modified;
//                         }
                      
//                         db.SaveChanges();
//                         Functions.PresentCount(UniqueAGMId, true);
//                         return Task.FromResult<string>("success");

//                     }
//                     else if (shareholderRecords.Count() > 1)
//                     {
//                         BarcodeModel crecord;

//                         var checkIfAnyAccountIsProxy = shareholderRecords.Any(s => s.PresentByProxy == true);
//                         var checkifConsolidate = shareholderRecords.Any(c => c.Consolidated == true);
//                         if (!checkifConsolidate)
//                         {
//                             crecord = agmM.ConsolidateRequest(company, shareholderRecords);
//                         }
//                         else
//                         {
//                             var consolidatedvalue = shareholderRecords.Where(s => s.Consolidated == true).Select(s => s.ConsolidatedValue).FirstOrDefault();

//                             crecord = agmM.GetConsolidatedAccount(company, consolidatedvalue);
//                         }

//                         var srecord = db.BarcodeStore.Find(crecord.Id);
//                         var shareholder = db.Present.Any(u => u.AGMID == UniqueAGMId && u.ShareholderNum == srecord.ShareholderNum);

//                         srecord.Present = true;
//                         PresentModel present = new PresentModel();
//                         present.Name = srecord.Name;
//                         present.Company = srecord.Company;
//                         present.Address = srecord.Address;
//                         present.admitSource = "Web";
//                         present.ShareholderNum = srecord.ShareholderNum;
//                         present.Holding = srecord.Holding;
//                         present.AGMID = UniqueAGMId;
//                         present.PercentageHolding = srecord.PercentageHolding;
//                         present.present = true;
//                         present.proxy = false;
//                         present.Year = currentYear;
//                         present.PresentTime = DateTime.Now;
//                         present.Timestamp = DateTime.Now.TimeOfDay;
//                         present.emailAddress = srecord.emailAddress;

//                         if (checkIfAnyAccountIsProxy)
//                         {
//                             present.PermitPoll = 0;
//                         }
//                         else if (agmstatus.allChannels || agmstatus.webChannel)
//                         {
//                             present.PermitPoll = 1;
//                         }
//                         else
//                         {
//                             present.PermitPoll = 0;
//                         }

//                         if (!String.IsNullOrEmpty(srecord.PhoneNumber))
//                         {
//                             if (srecord.PhoneNumber.StartsWith("234"))
//                             {
//                                 present.PhoneNumber = srecord.PhoneNumber;
//                             }
//                             else if (srecord.PhoneNumber.StartsWith("0"))
//                             {
//                                 double number;
//                                 if (double.TryParse(srecord.PhoneNumber, out number))
//                                 {
//                                     number = double.Parse(srecord.PhoneNumber);
//                                     present.PhoneNumber = "234" + number.ToString();
//                                 }
//                                 else
//                                 {
//                                     char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                                     var phonenum = srecord.PhoneNumber.Split(delimiterChars);
//                                     //string phonenumberresult = string.Concat(phonenum);
//                                     if (double.TryParse(phonenum[0], out number))
//                                     {
//                                         number = double.Parse(phonenum[0]);
//                                         present.PhoneNumber = "234" + number.ToString();
//                                     }

//                                 }

//                             }

//                         }
//                         present.Clikapad = srecord.Clikapad;
//                         if (!shareholder && agmstatus.StartAdmittance)
//                         {
//                             db.Present.Add(present);
//                             srecord.Date = DateTime.Today.ToString();
//                             db.Entry(srecord).State = EntityState.Modified;
//                         }                 
//                         db.SaveChanges();
//                         Functions.PresentCount(UniqueAGMId, true);
//                         return Task.FromResult<string>("success");

//                     }

//                 }
//                 return Task.FromResult<string>("failed");
//             }
//             return Task.FromResult<string>("failed");
//         }




//         public async Task<ActionResult> ExternalAdmission(string query)
//         {
//            var sessionid = System.Web.HttpContext.Current.Session.SessionID;
//             var response = await ExternalAdmissionAsync(query, sessionid);

//             Session["Authorize"] = response as APIMessageLog;

//             if (response.ResponseCode == "206")
//             {
//                 return RedirectToAction("AGMStageLogin");
//             }
//             return RedirectToAction("AGMEvent");
//         }


//         public Task<APIMessageLog> ExternalAdmissionAsync(string encryptedText, string sessionid)
//         {
//             //var query = new VoteModel();
//             string companyinfo;
//             string emailAddress = "";
//             int agmid;
//             APIMessageLog MessageLog;
//             AppLog log;

//             try
//             {

//                 Session.Clear();
//                 Application_BeginRequest();

//                 if (!string.IsNullOrEmpty(encryptedText))
//                 {
//                     var replacencryptedString = encryptedText.Replace(" ", "+");
//                     var decryptedtext = replacencryptedString.Decrypt();
//                     var querystring = decryptedtext.Split('|');
//                     emailAddress = querystring[0].Trim();
//                     //companyinfo = querystring[1].Trim();
//                     agmid = int.Parse(querystring[1].Trim());

//                     var agmstatus = db.Settings.FirstOrDefault(s => s.AGMID == agmid);

//                     if (agmstatus != null)
//                     {

//                         if (agmstatus.AgmEnd == true)
//                         {
//                             MessageLog = new APIMessageLog()
//                             {
//                                 Status = "Failed Accreditation",
//                                 ResponseCode = "205",
//                                 ResponseMessage = "Failed Accreditation Attempt - AGM may have ended and accesscode expired",
//                                 EventTime = DateTime.Now
//                             };
//                             log = new AppLog();
//                             log.Status = MessageLog.Status;
//                             log.ResponseCode = MessageLog.ResponseCode;
//                             log.ResponseMessage = MessageLog.ResponseMessage;
//                             log.EventTime = MessageLog.EventTime;
//                             db.AppLogs.Add(log);
//                             db.SaveChanges();
//                             return Task.FromResult<APIMessageLog>(MessageLog);
//                         }
//                         if (agmstatus.ArchiveStatus)
//                         {
//                             var MessageLog5 = new APIMessageLog()
//                             {
//                                 Status = "Failed Accreditation",
//                                 ResponseCode = "205",
//                                 ResponseMessage = "Failed Accreditation Attempt - Link may have been closed or expired. Email:" + " " + emailAddress,
//                                 EventTime = DateTime.Now
//                             };
//                             log = new AppLog();
//                             log.Status = MessageLog5.Status;
//                             log.ResponseCode = MessageLog5.ResponseCode;
//                             log.ResponseMessage = MessageLog5.ResponseMessage;
//                             log.EventTime = MessageLog5.EventTime;
//                             db.AppLogs.Add(log);
//                             db.SaveChanges();
//                             return Task.FromResult<APIMessageLog>(MessageLog5);
//                         }

//                         companyinfo = agmstatus.CompanyName;
//                         var UniqueAGMId = agmid;
//                         if (UniqueAGMId != -1 && !string.IsNullOrEmpty(emailAddress))
//                         {
//                             var FacilitatorRecord = db.Facilitators.FirstOrDefault(u => u.AGMID == UniqueAGMId && u.emailAddress.ToLower() == emailAddress.ToLower());

//                             if (FacilitatorRecord != null)
//                             {
//                                 var checkpreviousLogin = WebSessionManager.CheckFacilitatorLoginHistory(FacilitatorRecord.Company, FacilitatorRecord.emailAddress, sessionid);
//                                 if(checkpreviousLogin)
//                                 {
//                                     MessageLog = new APIMessageLog()
//                                     {
//                                         Status = "Login Attempt Alert",
//                                         ResponseCode = "206",
//                                         ResponseMessage = "You have a session on another browser. That session will be logged out. click Continue.",
//                                         EventTime = DateTime.Now,
//                                         EventUrl = agmstatus.OnlineUrllink,
//                                         EventStatus = agmstatus.AgmStart,
//                                         ResourceType = "Facilitator",
//                                         facilitator = FacilitatorRecord,
//                                         sessionVersion = FacilitatorRecord.SessionVersion,
//                                         AGMTitle = agmstatus.Description,
//                                         MessagingChoice = agmstatus.MessagingChoice,
//                                         allChannel = agmstatus.allChannels,
//                                         webChannel = agmstatus.webChannel,
//                                         mobileChannel = agmstatus.mobileChannel,
//                                         AGMID = UniqueAGMId
//                                     };
//                                     log = new AppLog();
//                                     log.Status = MessageLog.Status;
//                                     log.ResponseCode = MessageLog.ResponseCode;
//                                     log.ResponseMessage = MessageLog.ResponseMessage;
//                                     log.EventTime = MessageLog.EventTime;
//                                     db.AppLogs.Add(log);
//                                     db.SaveChanges();
//                                     return Task.FromResult<APIMessageLog>(MessageLog);
//                                 }
//                                 MessageLog = new APIMessageLog()
//                                 {
//                                     Status = "successfully Accredited",
//                                     ResponseCode = "200",
//                                     ResponseMessage = "Successful Accreditation. Email Address" + " " + emailAddress,
//                                     EventTime = DateTime.Now,
//                                     EventUrl = agmstatus.OnlineUrllink,
//                                     EventStatus = agmstatus.AgmStart,
//                                     ResourceType = "Facilitator",
//                                     facilitator = FacilitatorRecord,
//                                     AGMTitle = agmstatus.Description,
//                                     MessagingChoice = agmstatus.MessagingChoice,
//                                     allChannel = agmstatus.allChannels,
//                                     webChannel = agmstatus.webChannel,
//                                     mobileChannel = agmstatus.mobileChannel,
//                                     AGMID = UniqueAGMId
//                                 };
//                                 log = new AppLog();
//                                 log.Status = MessageLog.Status;
//                                 log.ResponseCode = MessageLog.ResponseCode;
//                                 log.ResponseMessage = MessageLog.ResponseMessage;
//                                 log.EventTime = MessageLog.EventTime;
//                                 db.AppLogs.Add(log);
//                                 db.SaveChanges();
//                                 return Task.FromResult<APIMessageLog>(MessageLog);
//                             }
//                             else
//                             {
//                                 MessageLog = new APIMessageLog()
//                                 {
//                                     Status = "Failed Accreditation",
//                                     ResponseCode = "205",
//                                     ResponseMessage = "Failed Accreditation Attempt - Invalid url parameter entry.",
//                                     EventTime = DateTime.Now
//                                 };
//                                 log = new AppLog();
//                                 log.Status = MessageLog.Status;
//                                 log.ResponseCode = MessageLog.ResponseCode;
//                                 log.ResponseMessage = MessageLog.ResponseMessage;
//                                 log.EventTime = MessageLog.EventTime;
//                                 db.AppLogs.Add(log);
//                                 db.SaveChanges();

//                                 return Task.FromResult<APIMessageLog>(MessageLog);
//                             }

//                         }
//                         else
//                         {
//                             MessageLog = new APIMessageLog()
//                             {
//                                 Status = "Failed Accreditation",
//                                 ResponseCode = "205",
//                                 ResponseMessage = "Failed Accreditation Attempt - Invalid url parameter entry.",
//                                 EventTime = DateTime.Now
//                             };
//                             log = new AppLog();
//                             log.Status = MessageLog.Status;
//                             log.ResponseCode = MessageLog.ResponseCode;
//                             log.ResponseMessage = MessageLog.ResponseMessage;
//                             log.EventTime = MessageLog.EventTime;
//                             db.AppLogs.Add(log);
//                             db.SaveChanges();

//                             return Task.FromResult<APIMessageLog>(MessageLog);
//                         }
//                     }
//                     MessageLog = new APIMessageLog()
//                     {
//                         Status = "Failed Accreditation",
//                         ResponseCode = "205",
//                         ResponseMessage = "Failed Accreditation Attempt - Information received cannot be proccessed" + " " + "Please go through pre-registration portal.",
//                         EventTime = DateTime.Now
//                     };
//                     log = new AppLog();
//                     log.Status = MessageLog.Status;
//                     log.ResponseCode = MessageLog.ResponseCode;
//                     log.ResponseMessage = MessageLog.ResponseMessage;
//                     log.EventTime = MessageLog.EventTime;
//                     db.AppLogs.Add(log);
//                     db.SaveChanges();

//                     return Task.FromResult<APIMessageLog>(MessageLog);
//                 }
//                 MessageLog = new APIMessageLog()
//                 {
//                     Status = "Failed Accreditation",
//                     ResponseCode = "205",
//                     ResponseMessage = "Failed Accreditation Attempt - Information received cannot be proccessed" + " " + "Please go through pre-registration portal.",
//                     EventTime = DateTime.Now
//                 };
//                 log = new AppLog();
//                 log.Status = MessageLog.Status;
//                 log.ResponseCode = MessageLog.ResponseCode;
//                 log.ResponseMessage = MessageLog.ResponseMessage;
//                 log.EventTime = MessageLog.EventTime;
//                 db.AppLogs.Add(log);
//                 db.SaveChanges();

//                 return Task.FromResult<APIMessageLog>(MessageLog);
//             }
//             catch (Exception e)
//             {
//                 var MessageLog7 = new APIMessageLog()
//                 {
//                     Status = "Error",
//                     ResponseCode = "500",
//                     ResponseMessage = "System cannot process your reqeuest at this time " + emailAddress + " " + "Error Message: " + e.Message + " " + "Please contact admin",
//                     EventTime = DateTime.Now
//                 };
//                 log = new AppLog();
//                 log.Status = MessageLog7.Status;
//                 log.ResponseCode = MessageLog7.ResponseCode;
//                 log.ResponseMessage = MessageLog7.ResponseMessage;
//                 log.EventTime = MessageLog7.EventTime;
//                 db.AppLogs.Add(log);
//                 db.SaveChanges();
//                 return Task.FromResult<APIMessageLog>(MessageLog7);

//             }
//         }


//         [HttpPost]
//         public async Task<JsonResult> VotingResponse(VoteModel post)
//         {
//             var response = await VotingResponseAsync(post);

//             return  Json(response, JsonRequestBehavior.AllowGet);
//         }


//         private Task<HttpRequestResponse> VotingResponseAsync(VoteModel post)
//         {
//             try
//             {
//                 if (!ModelState.IsValid)
//                 {
//                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                     {
//                         Code = "403",
//                         Message = "Invalid Request"
//                     });
//                 }
//                 if (!string.IsNullOrEmpty(post.company) && !string.IsNullOrWhiteSpace(post.company))
//                 {
//                     var UniqueAGMId = ua.RetrieveAGMUniqueID(post.company);

//                     if (UniqueAGMId != -1)
//                     {
//                         var abstainBtnchoice = true;
//                         var AgmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
//                         if (AgmEventSetting != null && AgmEventSetting.AbstainBtnChoice != null)
//                         {
//                             abstainBtnchoice = (bool)AgmEventSetting.AbstainBtnChoice;
//                         }


//                         if (abstainBtnchoice == false && post.response == "ABSTAIN")
//                         {
//                             return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                             {
//                                 Code = "505",
//                                 Message = "ABSTAIN is not a choice."
//                             });
//                         }

//                         if (TimerControll.GetTimeStatus(post.company) == true)
//                         {
//                             var question = db.Question.SingleOrDefault(q=>q.AGMID==UniqueAGMId && q.questionStatus==true);
//                             if (question != null)
//                             {
//                                 //var voter = db.Present.SingleOrDefault(q => q.emailAddress == post.identity && q.present == true);                         
//                                 var voters = db.Present.Where(q => q.AGMID == UniqueAGMId && q.emailAddress == post.identity && q.PermitPoll == 1).ToList();

//                                 if (voters.Count() == 1)
//                                 {
//                                     var votee = voters.First();

//                                     var voter = db.Present.Find(votee.Id);
//                                     var checkresult = db.Result.FirstOrDefault(r => r.ShareholderNum == voter.ShareholderNum && r.QuestionId == post.Id);
//                                     //if true - edit value
//                                     if (checkresult != null)
//                                     {
//                                         checkresult.date = DateTime.Now;
//                                         checkresult.Timestamp = DateTime.Now.TimeOfDay;
//                                         checkresult.VoteStatus = "Voted";
//                                         checkresult.Source = "Web";
//                                         if (post.response == "FOR")
//                                         {
//                                             checkresult.VoteFor = true;
//                                             checkresult.VoteAgainst = false;
//                                             checkresult.VoteAbstain = false;
//                                         }
//                                         else if (post.response == "AGAINST")
//                                         {
//                                             checkresult.VoteAgainst = true;
//                                             checkresult.VoteFor = false;
//                                             checkresult.VoteAbstain = false;
//                                         }
//                                         else if (post.response == "ABSTAIN")
//                                         {

//                                                 checkresult.VoteAbstain = true;
//                                                 checkresult.VoteFor = false;
//                                                 checkresult.VoteAgainst = false;
//                                         }
//                                         db.Entry(checkresult).State = EntityState.Modified;

//                                         try
//                                         {
//                                             db.SaveChanges();

//                                         }
//                                         catch (DbUpdateConcurrencyException e)
//                                         {

//                                         }

//                                         return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                                         {
//                                             Code = "200",
//                                             Message = "Success"
//                                         });

//                                         //var sucessoutput = JsonConvert.SerializeObject(successresponse);

//                                         //return new HttpResponseMessage()
//                                         //{
//                                         //    Content = new StringContent(sucessoutput, System.Text.Encoding.UTF8, "application/json")
//                                         //};

//                                     }
//                                     else
//                                     {
//                                         Result result = new Result();
//                                         result.Name = voter.Name;
//                                         result.ShareholderNum = voter.ShareholderNum;
//                                         result.Address = voter.Address;
//                                         //result.ParentNumber = voter.ParentNumber;
//                                         result.Holding = voter.Holding;
//                                         result.Company = post.company.Trim();
//                                         result.Year = currentYear;
//                                         result.AGMID = ua.RetrieveAGMUniqueID(post.company);
//                                         result.phonenumber = voter.PhoneNumber;
//                                         result.PercentageHolding = voter.PercentageHolding;
//                                         result.QuestionId = post.Id;
//                                         result.date = DateTime.Now;
//                                         result.Timestamp = DateTime.Now.TimeOfDay;
//                                         result.VoteStatus = "Voted";
//                                         result.Source = "Web";
//                                         result.Present = true;
//                                         if (post.response == "FOR")
//                                         {
//                                             result.VoteFor = true;
//                                             result.VoteAgainst = false;
//                                             result.VoteAbstain = false;

//                                         }
//                                         else if (post.response == "AGAINST")
//                                         {
//                                             result.VoteAgainst = true;
//                                             result.VoteFor = false;
//                                             result.VoteAbstain = false;
//                                         }
//                                         else if (post.response == "ABSTAIN")
//                                         {
//                                                 result.VoteAbstain = true;
//                                                 result.VoteFor = false;
//                                                 result.VoteAgainst = false;
//                                         }
//                                         db.Result.Add(result);
//                                     }

//                                     voter.TakePoll = true;
//                                     db.Entry(voter).State = EntityState.Modified;

//                                     try
//                                     {
//                                         db.SaveChanges();

//                                     }
//                                     catch (DbUpdateConcurrencyException e)
//                                     {

//                                     }

//                                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                                     {
//                                         Code = "200",
//                                         Message = "SUCCESS"
//                                     });
//                                     //Voting Success
//                                     //var sucoutput = JsonConvert.SerializeObject(successresponse);

//                                     //return new HttpResponseMessage()
//                                     //{
//                                     //    Content = new StringContent(sucoutput, System.Text.Encoding.UTF8, "application/json")
//                                     //};

//                                 }
//                                 else if (voters.Count() > 1)
//                                 {
//                                     foreach (var votee in voters)
//                                     {
//                                         var voter = db.Present.Find(votee.Id);
//                                         var checkresult = db.Result.FirstOrDefault(r => r.ShareholderNum== voter.ShareholderNum && r.QuestionId == post.Id);
//                                         //if true - edit value
//                                         if (checkresult != null)
//                                         {
//                                             checkresult.date = DateTime.Now;
//                                             checkresult.Timestamp = DateTime.Now.TimeOfDay;
//                                             checkresult.VoteStatus = "Voted";
//                                             checkresult.Source = "Web";
//                                             if (post.response == "FOR")
//                                             {
//                                                 checkresult.VoteFor = true;
//                                                 checkresult.VoteAgainst = false;
//                                                 checkresult.VoteAbstain = false;
//                                             }
//                                             else if (post.response == "AGAINST")
//                                             {
//                                                 checkresult.VoteAgainst = true;
//                                                 checkresult.VoteFor = false;
//                                                 checkresult.VoteAbstain = false;
//                                             }
//                                             else if (post.response == "ABSTAIN")
//                                             {
//                                                     checkresult.VoteAbstain = true;
//                                                     checkresult.VoteFor = false;
//                                                     checkresult.VoteAgainst = false;

//                                             }
//                                             db.Entry(checkresult).State = EntityState.Modified;

//                                             //db.SaveChanges();

//                                         }
//                                         else
//                                         {
//                                             Result result = new Result();
//                                             result.Name = voter.Name;
//                                             result.ShareholderNum = voter.ShareholderNum;
//                                             //result.ParentNumber = voter.ParentNumber;
//                                             result.Address = voter.Address;
//                                             result.Company = post.company.Trim();
//                                             result.Year = currentYear;
//                                             result.AGMID = ua.RetrieveAGMUniqueID(post.company);
//                                             result.Holding = voter.Holding;
//                                             result.phonenumber = voter.PhoneNumber;
//                                             result.PercentageHolding = voter.PercentageHolding;
//                                             result.QuestionId = post.Id;
//                                             result.date = DateTime.Now;
//                                             result.Timestamp = DateTime.Now.TimeOfDay;
//                                             result.VoteStatus = "Voted";
//                                             result.Source = "Web";
//                                             result.Present = true;
//                                             if (post.response == "FOR")
//                                             {
//                                                 result.VoteFor = true;
//                                                 result.VoteAgainst = false;
//                                                 result.VoteAbstain = false;

//                                             }
//                                             else if (post.response == "AGAINST")
//                                             {
//                                                 result.VoteAgainst = true;
//                                                 result.VoteFor = false;
//                                                 result.VoteAbstain = false;
//                                             }
//                                             else if (post.response == "ABSTAIN")
//                                             {
//                                                     result.VoteAbstain = true;
//                                                     result.VoteFor = false;
//                                                     result.VoteAgainst = false;
//                                             }
//                                             db.Result.Add(result);
//                                         }

//                                         voter.TakePoll = true;
//                                         db.Entry(voter).State = EntityState.Modified;
//                                     }

//                                     try
//                                     {
//                                         db.SaveChanges();

//                                     }
//                                     catch (DbUpdateConcurrencyException e)
//                                     {

//                                     }
//                                     // Voting Success

//                                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                                     {
//                                         Code = "200",
//                                         Message = "SUCCESS"
//                                     });
       

//                                 }

//                                 //Voter not marked present at AGM
//                                 return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                                 {
//                                     Code = "505",
//                                     Message = "SHAREHOLDER MAY HAVE VOTED BY PROXY OR NOT PERMITTED TO VOTE BY THE FACILITATOR. THANK YOU."
//                                 });

//                             }
//                             //Wait for voting to commence on this resolution

//                             return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                             {
//                                 Code = "400",
//                                 Message = "WAIT FOR VOTING TO COMMENCE ON THIS RESOLUTION"
//                             });

//                         }
//                         //Voting clock has stopped

//                         return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                         {
//                             Code = "406",
//                             Message = "VOTING CLOCK HAS STOPPED"

//                         });
//                     }

//                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                     {
//                         Code = "500",
//                         Message = "Failed Request"
//                     });

//                 }
//                 else
//                 {
//                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                     {
//                         Code = "404",
//                         Message = "Empty Request"

//                     });

//                 }


//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                 {
//                     Code = "500",
//                     Message = "Failed Request"
//                 });
//             }

//         }



        
//     }


// }