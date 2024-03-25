// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using System.Web;
// using System.Web.Mvc;

// namespace BarcodeGenerator.Controllers
// {
//     [Authorize]
//     public class DashboardController : Controller
//     {
//         UsersContext db = new UsersContext();
//         UserAdmin ua = new UserAdmin();
//         // GET: Dashboard
//         private static string currentYear = DateTime.Now.Year.ToString();

//         //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

//         //private static int RetrieveAGMUniqueID()
//         //{
//         //    UsersContext adb = new UsersContext();
//         //    var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

//         //    return AGMID;
//         //}

//         //private static int UniqueAGMId = RetrieveAGMUniqueID();

//         public async Task<ActionResult> Index()
//         {
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             var response = await IndexAsync();

//             return PartialView(response);
//         }

//         public Task<ReportRefreshModelView> IndexAsync()
//         {
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             ReportRefreshModelView model = new ReportRefreshModelView();

//             int TotalCount = 0;
//             Double TotalHolding = 0;
//             Double TotalPercentageHolding = 0;
//             Double presentholding = 0;
//             Double presentpercentHolding = 0;
//             Double proxyholding = 0;
//             Double proxypercentHolding = 0;
//             IEnumerable<PresentModel> present;
//             IEnumerable<PresentModel> proxy;
//             List<Question> questionlist = null;
//             int presentcount = 0;
//             int proxycount = 0;
//             var forBg = "green";
//             var againstBg = "red";
//             var abstainBg = "blue";
//             var voidBg = "black";
//             var abstainbtnchoice = true;
//             if (UniqueAGMId!=-1)
//             {
//                 var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

              

//                 if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
//                 {
//                     abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
//                     if (!string.IsNullOrEmpty(agmEventSetting.VoteForColorBg))
//                     {
//                         forBg = agmEventSetting.VoteForColorBg;
//                     }
//                     if (!string.IsNullOrEmpty(agmEventSetting.VoteAgainstColorBg))
//                     {
//                         againstBg = agmEventSetting.VoteAgainstColorBg;
//                     }
//                     if (!string.IsNullOrEmpty(agmEventSetting.VoteAbstaincolorBg))
//                     {
//                         abstainBg = agmEventSetting.VoteAbstaincolorBg;
//                     }
//                     if (!string.IsNullOrEmpty(agmEventSetting.VoteVoidColorBg))
//                     {
//                         voidBg = agmEventSetting.VoteVoidColorBg;
//                     }
//                 }

//                 present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.present == true && p.PermitPoll== 1).ToList();
//                 presentcount = present.Count();
//                 foreach (var item in present)
//                 {
//                     var value = item.Holding;
//                     var perHold = item.PercentageHolding;



//                     presentpercentHolding += perHold;
//                     presentholding += value;
//                 }
//                 proxy = db.Present.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).ToList();
//                 proxycount = proxy.Count();
                
//                 foreach (var item in proxy)
//                 {
//                     var value = item.Holding;
//                     var perHold = item.PercentageHolding;



//                     proxypercentHolding += perHold;
//                     proxyholding += value;
//                 }

//                 var question = db.Question.FirstOrDefault(q => q.AGMID == UniqueAGMId);
//                 if (question != null)
//                 {
//                     model.ResolutionName = question.question;
//                     ResolutionListModel rmodel = new ResolutionListModel
//                     {
//                         ResultFor = question.result.Where(r => r.VoteFor == true).ToList(),
//                         ResultAgainst = question.result.Where(r => r.VoteAgainst == true).ToList(),
//                         ResultAbstain = question.result.Where(r => r.VoteAbstain == true).ToList()
//                         //question = db.Question.Where(q => q.Company == companyinfo && q.Year == currentYear).ToList()
//                     };

//                     model.ResultForCount = rmodel.ResultFor.Count();
//                     model.ResultAgainstCount = rmodel.ResultAgainst.Count();
//                     model.ResultAbstainCount = rmodel.ResultAbstain.Count();
//                     var TotalPresent = db.Present.Where(p => p.AGMID == UniqueAGMId).Count();
//                     model.PercentageFor = ((double)rmodel.ResultFor.Count() / (double)TotalPresent) * 100;
//                     model.PercentageAgainst = ((double)rmodel.ResultAgainst.Count() / (double)TotalPresent) * 100;
//                     model.PercentageAbstain = ((double)rmodel.ResultAbstain.Count() / (double)TotalPresent) * 100;
//                     model.agmid = UniqueAGMId;
//                 }

//                 questionlist = db.Question.Where(q => q.AGMID == UniqueAGMId).ToList();
//             }
            
//             model.present = presentcount;
//             model.proxy = proxycount;

//             model.TotalPercentageHolding = String.Format("{0:0.####}", presentpercentHolding);
//             model.Holding = presentholding;


          

//             model.TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
//             model.ProxyHolding = proxyholding;

//             TotalCount = proxycount + presentcount;
//             model.TotalCount = TotalCount;
//             var percentagePresent = ((double)presentcount / (double)TotalCount) * 100;
//             var percentageProxy = ((double)proxycount / (double)TotalCount) * 100;
//             model.percentagePresent = String.Format("{0:0.####}", percentagePresent);
//             model.percentageProxy = String.Format("{0:0.####}", percentageProxy);
//             TotalHolding = presentholding + proxyholding;
//             model.TotalHolding = TotalHolding;

//             TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
//             model.PercentageTotalHolding = String.Format("{0:0.####}", TotalPercentageHolding);



//             model.resolutions = questionlist != null ? questionlist : new List<Question>();
//             var setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
//             if (setting != null)
//             {
//                 model.logo = setting.Image != null ? "data:image/jpg;base64," +
//                            Convert.ToBase64String((byte[])setting.Image) : "";
//                 model.Company = setting.CompanyName;
//                 model.AGMTitle = setting.Title;
//             }
//             model.abstainBtnChoice = abstainbtnchoice;
//             model.forBg = forBg;
//             model.againstBg = againstBg;
//             model.abstainBg = abstainBg;
//             model.voidBg = voidBg;
//             return Task.FromResult<ReportRefreshModelView>(model);
//         }




//         public async Task<ActionResult> ResolutionChart(int id)
//         {
//             var response = await ResolutionChartAsync(id);

//             return PartialView(response);
//         }

//         public Task<ResolutionListModel> ResolutionChartAsync(int id)
//         {
//             Double holding = 0;
//             Double percentHolding = 0;
//             Double holdingAgainst = 0;
//             Double percentHoldingAgainst = 0;
//             Double holdingAbstain = 0;
//             Double percentHoldingAbstain = 0;
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             //Double TotalHolding = 0;
//             //Double TotalPercentHolding = 0;

//                 var question = db.Question.Find(id);
//                 ViewBag.ResolutionName = question.question;
//             if (UniqueAGMId != -1)
//             {
//                 var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

//                 var abstainbtnchoice = true;
//                 var forBg = "green";
//                 var againstBg = "red";
//                 var abstainBg = "blue";
//                 var voidBg = "black";
//                 if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
//                 {
//                     abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
//                     if (!string.IsNullOrEmpty(agmEventSetting.VoteForColorBg))
//                     {
//                         forBg = agmEventSetting.VoteForColorBg;
//                     }
//                     if (!string.IsNullOrEmpty(agmEventSetting.VoteAgainstColorBg))
//                     {
//                         againstBg = agmEventSetting.VoteAgainstColorBg;
//                     }
//                     if (!string.IsNullOrEmpty(agmEventSetting.VoteAbstaincolorBg))
//                     {
//                         abstainBg = agmEventSetting.VoteAbstaincolorBg;
//                     }
//                     if (!string.IsNullOrEmpty(agmEventSetting.VoteVoidColorBg))
//                     {
//                         voidBg = agmEventSetting.VoteVoidColorBg;
//                     }
//                 }


//                 ResolutionListModel model = new ResolutionListModel
//                 {
//                     ResultFor = question.result.Where(r => r.VoteFor == true).ToList(),
//                     ResultAgainst = question.result.Where(r => r.VoteAgainst == true).ToList(),
//                     ResultAbstain = question.result.Where(r => r.VoteAbstain == true).ToList(),

//                     questionList = db.Question.Where(q => q.AGMID == UniqueAGMId).ToList(),
//                      abstainBtnChoice = abstainbtnchoice,
//                     forBg = forBg,
//                     againstBg = againstBg,
//                     abstainBg = abstainBg,
//                     voidBg = voidBg

//             };

//                 ViewBag.ResultForCount = model.ResultFor.Count();
//                 ViewBag.ResultAgainstCount = model.ResultAgainst.Count();
//                 ViewBag.ResultAbstainCount = model.ResultAbstain.Count();


//                 //Percentage voters 

//                 var TotalPresent = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll== 1).Count();
//                 ViewBag.PercentageFor = ((double)model.ResultFor.Count() / (double)TotalPresent) * 100;
//                 ViewBag.PercentageAgainst = ((double)model.ResultAgainst.Count() / (double)TotalPresent) * 100;
//                 ViewBag.PercentageAbstain = ((double)model.ResultAbstain.Count() / (double)TotalPresent) * 100;

//                 return Task.FromResult<ResolutionListModel>(model);
//             }
//             ResolutionListModel model1 = new ResolutionListModel
//             {
//                 ResultFor = new List<Result>(),
//                 ResultAgainst = new List<Result>(),
//                 ResultAbstain = new List<Result>(),

//                 questionList = new List<Question>()
//             };
//             return Task.FromResult<ResolutionListModel>(model1 );
//         }



//         public async Task<ActionResult> VotingBoard()
//         {
//             var model = await VotingBoardAsync();

//             return PartialView(model);
//         }

//         public Task<ResolutionListModel> VotingBoardAsync()
//         {
//             ResolutionListModel model = new ResolutionListModel();
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             if (UniqueAGMId != -1)
//             {
//                 var question = db.Question.FirstOrDefault(q => q.AGMID == UniqueAGMId && q.questionStatus == true);
//                 if (question != null)
//                 {
//                     //ViewBag.ResolutionName = question.question;

//                     model.question = question;
//                     model.ResultFor = question.result.Where(r => r.VoteFor == true).ToList();
//                     model.ResultAgainst = question.result.Where(r => r.VoteAgainst == true).ToList();
//                     model.ResultAbstain = question.result.Where(r => r.VoteAbstain == true).ToList();
//                     model.questionList = db.Question.Where(q => q.AGMID == UniqueAGMId).ToList();
//                     model.TotalCount = question.result.Count();

//                     return System.Threading.Tasks.Task.FromResult<ResolutionListModel>(model);
//                 }
//                 else
//                 {
//                     return System.Threading.Tasks.Task.FromResult<ResolutionListModel>(new ResolutionListModel());
//                 }
//             }
//             return System.Threading.Tasks.Task.FromResult<ResolutionListModel>(new ResolutionListModel());

//         }

//         public async Task<ActionResult> PresentCount()
//         {
//             var response = await PresentCountAsync();
//             ViewBag.response = response;
//             return PartialView();
//         }

//         private Task<int> PresentCountAsync()
//         {
//             int presentCount=0;
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             if(UniqueAGMId!=-1)
//             {
//                 presentCount = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1).Count();
//             }

           

//             return System.Threading.Tasks.Task.FromResult<int>(presentCount);
//         }
//     }
// }