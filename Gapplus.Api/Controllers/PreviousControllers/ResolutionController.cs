// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;
// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Data.Entity;
// using System.Data.Entity.Infrastructure;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Threading.Tasks;
// using System.Web;
// using System.Web.Mvc;

// namespace BarcodeGenerator.Controllers
// {
//     public class ResolutionController : Controller
//     {
//         //
//         // GET: /Resolution/
//         UsersContext db = new UsersContext();
//         UserAdmin ua = new UserAdmin();
//         //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

//         //private static int RetrieveAGMUniqueID()
//         //{
//         //    UsersContext adb = new UsersContext();
//         //    var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

//         //    return AGMID;
//         //}

//         //private static int UniqueAGMId = RetrieveAGMUniqueID();

//         public int PageSize = 10;

//         public async Task<ActionResult> Index()
//         {
//             var response = await IndexAsync();

//             return PartialView(response);
//         }

//         public Task<QuestionListViewModel> IndexAsync()
//         {
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//             var currentYear = DateTime.Now.Year.ToString();
//             QuestionListViewModel model = new QuestionListViewModel
//             {
//                 question = db.Question.Where(q => q.Company == companyinfo && q.AGMID == UniqueAGMId).ToList(),
//                 abstainBtnChoice = abstainbtnchoice
//         };

//             return Task.FromResult<QuestionListViewModel>(model);
//         }

//         [HttpPost]
//         public async Task<ActionResult> Index(ResolutionPostModel post)
//         {
//             var response = await IndexPostAsync(post);

//             return PartialView(response);

//         }


  
//         private Task<ResolutionModel> IndexPostAsync(ResolutionPostModel post)
//         {
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//             var currentYear = DateTime.Now.Year.ToString();
//           ResolutionModel model = new ResolutionModel
//             {
//                 Name = post.name,
//                 NewNumber = post.newnumber,
//                 NewHolding = post.Holding,
//                 shareholdernum = post.number,
//                 splitvalue = post.splitvalue,
//                 ParentNumber = post.ParentNumber,
//                 abstainBtnChoice = abstainbtnchoice,
//                 Question = db.Question.Where(q=>q.AGMID == UniqueAGMId).ToList()
//             } ;     
            
//             return Task.FromResult<ResolutionModel>(model);
//         }

//         [HttpPost]
//         public async Task<ActionResult>splitpoll(PostResolution post)
//         {
//             var response = await splitpollAsync(post);

//             return RedirectToAction("SplitResultDetails", response);    
//         }

//         public Task<Result> splitpollAsync(PostResolution post)
//         {
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
           
//             //var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
//             //if (agmEvent != null)
//             //{
//             //    if (agmEvent.allChannels != true || agmEvent.proxyChannel != true)
//             //    {
//             //        return Task.FromResult<Result>(new Result());
//             //    }
//             //}
//             int id = post.Id;
//             var currentYear = DateTime.Now.Year.ToString();
//             var setting = db.Settings.SingleOrDefault(s=>s.AGMID == UniqueAGMId);
//             var abstainbtnchoice = true;
//             if (setting != null && setting.AbstainBtnChoice != null)
//             {
//                 abstainbtnchoice = (bool)setting.AbstainBtnChoice;
//             }

//             if (string.IsNullOrEmpty(post.RG))
//             {
//                 return Task.FromResult<Result>(new Result());
//             }

//             if (abstainbtnchoice != true && post.RG == "ABSTAIN")
//             {
//                 return Task.FromResult<Result>(new Result());
//             }

//             string response =  post.RG;    
//             var question = db.Question.Find(id);
//             var shareholder = db.BarcodeStore.FirstOrDefault(s =>s.Company==companyinfo && s.ShareholderNum == post.NewNumber);
//             //if (shareholder.resolution == true || shareholder.Present == true)
//             if ( shareholder.Present == true)
//             {
     
//                 return Task.FromResult<Result>(new Result());
//             }

//             shareholder.PresentByProxy = true;
//             shareholder.TakePoll = true;
//             shareholder.NotVerifiable = true;
//             shareholder.resolution = true;
//             string phonenumber = "";
//             if (!String.IsNullOrEmpty(shareholder.PhoneNumber))
//             {
//                 char[] arr = shareholder.PhoneNumber.Where(c => (char.IsLetterOrDigit(c))).ToArray();

//                 shareholder.PhoneNumber = new string(arr);
//                 if (shareholder.PhoneNumber.StartsWith("234"))
//                 {
//                     phonenumber = shareholder.PhoneNumber;
//                 }
//                 else if (shareholder.PhoneNumber.StartsWith("0"))
//                 {

//                     double number;
//                     if (double.TryParse(shareholder.PhoneNumber, out number))
//                     {
//                         number = double.Parse(shareholder.PhoneNumber);
//                         shareholder.PhoneNumber = "234" + number.ToString();
//                     }
//                     else
//                     {
//                         char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                         var phonenum = shareholder.PhoneNumber.Split(delimiterChars);
//                         //string phonenumberresult = string.Concat(phonenum);
//                         if (double.TryParse(phonenum[0], out number))
//                         {
//                             number = double.Parse(phonenum[0]);
//                             shareholder.PhoneNumber = "234" + number.ToString();
//                         }

//                     }

//                 }

//             }
//             ////var newNumber = Int64.Parse(post.NewNumber);
//             var proxyitem = db.Present.Where(p =>p.AGMID == UniqueAGMId && p.newNumber == post.NewNumber);
//             if (!proxyitem.Any())
//             {
//                 PresentModel proxy = new PresentModel();
//                 proxy.Name = shareholder.Name;
//                 proxy.Address = shareholder.Address;
//                 proxy.admitSource = "Proxy";
//                 proxy.PermitPoll = 1;
//                 proxy.ShareholderNum = post.NewNumber;
//                 proxy.Year = currentYear;
//                 proxy.AGMID = UniqueAGMId;
//                 proxy.Company = shareholder.Company;
//                 //proxy.emailAddress = shareholder.emailAddress;
//                 //proxy.PhoneNumber = shareholder.PhoneNumber;
//                 proxy.PhoneNumber = phonenumber;
//                 if (!String.IsNullOrEmpty(shareholder.emailAddress))
//                 {
//                     proxy.emailAddress = string.Format("proxy{0}", shareholder.emailAddress);
//                 }
//                 proxy.newNumber = post.NewNumber;
//                 if (post.ParentNumber == 0)
//                 {
//                     proxy.ParentNumber = shareholder.ParentAccountNumber;
//                 }
//                 else
//                 {
//                     proxy.ParentNumber = post.ParentNumber;
//                 }

//                 proxy.Holding = double.Parse(post.NewHolding);
//                 proxy.split = true;

//                 if (setting != null)
//                 {
//                     var totalHolding = setting.ShareHolding;
//                     var nHolding = Double.Parse(post.NewHolding);
//                     Double TotalShareholding = totalHolding;
//                     var newPHolding = (nHolding / TotalShareholding) * 100;
//                     proxy.PercentageHolding = newPHolding;
//                 }
//                 //proxy.PercentageHolding = post..PercentageHolding;
//                 proxy.proxy = true;
//                 proxy.present = false;
//                 proxy.TakePoll = true;
//                 proxy.PresentTime = DateTime.Now;
//                 proxy.Timestamp = DateTime.Now.TimeOfDay;
//                 db.Present.Add(proxy);
//         }
//         var checkresult = db.Result.FirstOrDefault(r => r.ShareholderNum == post.NewNumber && r.QuestionId == question.Id);
//                 if (checkresult != null)
//                 {
//                     checkresult.date = DateTime.Now;
//                 checkresult.Timestamp = DateTime.Now.TimeOfDay;
//                 checkresult.Holding = double.Parse(post.NewHolding);
//                     checkresult.VoteStatus = "Voted";
//                 checkresult.Source = "Proxy";
//                     if (response == "FOR")
//                     {
//                         checkresult.VoteFor = true;
//                         checkresult.VoteAgainst= false;
//                         checkresult.VoteAbstain = false;
//                     checkresult.VoteVoid = false;
//                 }
//                     else if (response == "AGAINST")
//                     {
//                         checkresult.VoteAgainst = true;
//                         checkresult.VoteFor= false;
//                         checkresult.VoteAbstain = false;
//                     checkresult.VoteVoid = false;
//                 }
//                     else if (response == "ABSTAIN")
//                     {
//                         checkresult.VoteAbstain = true;
//                         checkresult.VoteFor = false;
//                         checkresult.VoteAgainst = false;
//                     checkresult.VoteVoid = false;
//                 }
//                     db.Entry(checkresult).State = EntityState.Modified;
//                     try
//                     {
//                         db.SaveChanges();
//                         return Task.FromResult<Result>(checkresult);
//                     }
//                     catch (DbUpdateConcurrencyException)
//                     {


//                     return Task.FromResult<Result>(checkresult);
//                     }
//                 }
//                 else 
//                 {
//                     //var setting = db.Settings.ToArray();
//                     if (setting == null )
//                     {
//                         TempData["Message"] = "Please value for Total Holding";
//                     return Task.FromResult<Result>(new Result());
//                 }
//                     var totalHolding = setting.ShareHolding;
//                     Double TotalShareholding = totalHolding;
//                     var newPHold = Double.Parse(post.NewHolding);
//                     var pHold = (newPHold / TotalShareholding) * 100;

//                     var newPercentageHolding = pHold;
//                     Result result = new Result();
//                     result.ShareholderNum = post.NewNumber;
//                 if (post.ParentNumber == 0)
//                 {
//                     result.ParentNumber = shareholder.ParentAccountNumber;
//                 }
//                 else
//                 {
//                     result.ParentNumber = post.ParentNumber;
//                 }
//                 //result.ParentNumber = post.shareholdernum;
//                 result.EmailAddress = shareholder.emailAddress;
//                     result.Holding = double.Parse(post.NewHolding);
//                     result.Name = post.Name;
//                 result.Company = shareholder.Company;
//                 result.Year = DateTime.Now.Year.ToString();
//                 result.AGMID = UniqueAGMId;
//                     result.Address = shareholder.Address;
//                     result.PercentageHolding = newPercentageHolding; 
//                     result.QuestionId = question.Id;
//                     result.VoteStatus = "Voted";
//                 result.Source = "Proxy";
//                     result.date = DateTime.Now;
//                 result.Timestamp = DateTime.Now.TimeOfDay;
//                     result.PresentByProxy = true;

//                     if (response == "FOR")
//                     {
//                         result.VoteFor = true;
//                         result.VoteAgainst = false;
//                         result.VoteAbstain = false;
//                         result.VoteVoid = false;

//                 }
//                     else if (response == "AGAINST")
//                     {
//                         result.VoteAgainst = true;
//                         result.VoteFor = false;
//                         result.VoteAbstain = false;
//                         result.VoteVoid = false;
//                      }
//                     else if (response == "ABSTAIN")
//                     {
//                         result.VoteAbstain = true;
//                         result.VoteFor = false;
//                         result.VoteAgainst = false;
//                     result.VoteVoid = false;
//                 }
//                     question.result.Add(result);

//                     try
//                     {
//                         db.SaveChanges();
//                         return Task.FromResult<Result>(result);

//                     }
//                     catch (DbUpdateConcurrencyException)
//                     {

//                     return Task.FromResult<Result>(new Result());

//                     }

//                 }
//         }
//         //
//         // GET: /Resolution/Details/5
//         [HttpPost]
//         public async Task<ActionResult> poll(PostResolution post)
//         {
//             var response = await pollAsync(post);

//             return RedirectToAction("ResultDetailsIndex", response);
//         }



//         private Task<Result> pollAsync(PostResolution post)
//         {
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//             var setting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
           
//             if(string.IsNullOrEmpty(post.RG))
//             {
//                 return Task.FromResult<Result>(new Result());
//             }

//             if (abstainbtnchoice != true && post.RG=="ABSTAIN")
//             {
//                 return Task.FromResult<Result>(new Result());
//             }

//             var currentYear = DateTime.Now.Year.ToString();
//             int id = post.Id;
//             string response = post.RG;
//             var question = db.Question.Find(id);
//             var shareholder = db.BarcodeStore.FirstOrDefault(s => s.Company==companyinfo && s.ShareholderNum == post.shareholdernum);
//             //if (shareholder.split == true || shareholder.Present == true)
//             if (shareholder.Present == true)
//             {                

//                 return Task.FromResult<Result>(new Result());
//             }

//                 shareholder.PresentByProxy = true;
//                 shareholder.TakePoll = true;
//             shareholder.NotVerifiable = true;
//                  shareholder.resolution = true;
//             string phonenumber = "";
//             if (!String.IsNullOrEmpty(shareholder.PhoneNumber))
//             {
//                 char[] arr = shareholder.PhoneNumber.Where(c => (char.IsLetterOrDigit(c))).ToArray();

//                 shareholder.PhoneNumber = new string(arr);
//                 if (shareholder.PhoneNumber.StartsWith("234"))
//                 {
//                     phonenumber = shareholder.PhoneNumber;
//                 }
//                 else if (shareholder.PhoneNumber.StartsWith("0"))
//                 {

//                     double number;
//                     if (double.TryParse(shareholder.PhoneNumber, out number))
//                     {
//                         number = double.Parse(shareholder.PhoneNumber);
//                         shareholder.PhoneNumber = "234" + number.ToString();
//                     }
//                     else
//                     {
//                         char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                         var phonenum = shareholder.PhoneNumber.Split(delimiterChars);
//                         //string phonenumberresult = string.Concat(phonenum);
//                         if (double.TryParse(phonenum[0], out number))
//                         {
//                             number = double.Parse(phonenum[0]);
//                             shareholder.PhoneNumber = "234" + number.ToString();
//                         }

//                     }

//                 }

//             }
//             var proxy = db.Present.Where(p =>p.AGMID== UniqueAGMId && p.ShareholderNum == post.shareholdernum );
//                  if (!proxy.Any())
//                  {
//                      PresentModel model = new PresentModel();
//                      model.Name = shareholder.Name;
//                      model.Address = shareholder.Address;
//                 model.Company = shareholder.Company;
//                 model.admitSource = "Proxy";
//                 model.PermitPoll = 1;
//                 model.Year = currentYear;
//                 model.AGMID = UniqueAGMId;
//                      model.ShareholderNum = shareholder.ShareholderNum;
//                      model.Holding = shareholder.Holding;
//                 model.PhoneNumber = phonenumber;
//                 if (!String.IsNullOrEmpty(shareholder.emailAddress))
//                 {
//                     model.emailAddress = string.Format("proxy{0}", shareholder.emailAddress);
//                 }
//                 //model.emailAddress = shareholder.emailAddress;
//                 //model.PhoneNumber = model.PhoneNumber;
//                 model.ParentNumber = shareholder.ParentAccountNumber;
//                 model.PercentageHolding = shareholder.PercentageHolding;
//                      model.proxy = true;
//                      model.present = false;
//                      model.TakePoll = true;
//                 model.PresentTime = DateTime.Now;
//                 model.Timestamp = DateTime.Now.TimeOfDay;
//                 db.Present.Add(model);
//                 shareholder.emailAddress = string.Format("proxy{0}", shareholder.emailAddress);
//             }
//                 //id = question.Id;
//                 var checkresult = db.Result.FirstOrDefault(r =>r.ShareholderNum == post.shareholdernum && r.QuestionId == id);
//                 if (checkresult != null)
//                 {
//                     checkresult.date = DateTime.Now;
//                 checkresult.Timestamp = DateTime.Now.TimeOfDay;
//                 //checkresult.Holding = float.Parse(post.NewHolding);
//                 checkresult.Holding = shareholder.Holding;
//                 checkresult.VoteStatus = "Voted";
//                 checkresult.Source = "Proxy";
//                     if (response == "FOR")
//                     {
//                         checkresult.VoteFor = true;
//                         checkresult.VoteAgainst = false;
//                         checkresult.VoteAbstain= false;
//                     checkresult.VoteVoid = false;
//                 }
//                     else if (response == "AGAINST")
//                     {
//                         checkresult.VoteAgainst = true;
//                         checkresult.VoteFor = false;
//                         checkresult.VoteAbstain = false;
//                     checkresult.VoteVoid = false;
//                 }
//                     else if (response == "ABSTAIN")
//                     {
//                         checkresult.VoteAbstain = true;
//                         checkresult.VoteFor = false;
//                         checkresult.VoteAgainst = false;
//                         checkresult.VoteVoid = false;
//                 }
//                     db.Entry(checkresult).State = EntityState.Modified;
//                     try
//                     {
//                         db.SaveChanges();
//                     return Task.FromResult<Result>(checkresult);
                    
//                     }
//                     catch (DbUpdateConcurrencyException)
//                     {
//                         TempData["Message"] = "Error Adding your poll Information";
                    
//                      return Task.FromResult<Result>(new Result());
//                     }
//                 }
//                 else
//                 {
//                     Result result = new Result();
//                     result.ShareholderNum = post.shareholdernum;
//                 if (post.ParentNumber == 0)
//                 {
//                     result.ParentNumber = shareholder.ShareholderNum;
//                 }
//                 else
//                 {
//                     result.ParentNumber = post.ParentNumber;
//                 }
//                 result.phonenumber = shareholder.PhoneNumber;
//                 result.Company = shareholder.Company;
//                 result.Year = DateTime.Now.Year.ToString();
//                 result.AGMID = UniqueAGMId;
//                 //result.Holding = double.Parse(post.NewHolding);
//                 result.Holding = shareholder.Holding;
//                 result.Name = post.Name;
//                     result.splitValue = post.splitvalue;
//                     result.Address = shareholder.Address;
//                     result.PercentageHolding = shareholder.PercentageHolding;
//                     result.QuestionId = question.Id;
//                     result.VoteStatus = "Voted";
//                     result.date = DateTime.Now;
//                 result.Timestamp = DateTime.Now.TimeOfDay;
//                 result.PresentByProxy = true;
//                 result.Source = "Proxy";

//                     if (response == "FOR")
//                     {
//                         result.VoteFor = true;
//                         result.VoteAgainst = false;
//                         result.VoteAbstain = false;
//                         result.VoteVoid = false;

//                 }
//                     else if (response == "AGAINST")
//                     {
//                         result.VoteAgainst = true;
//                         result.VoteFor = false;
//                         result.VoteAbstain = false;
//                         result.VoteVoid = false;
//                 }
//                     else if (response == "ABSTAIN")
//                     {
//                         result.VoteAbstain = true;
//                         result.VoteFor = false;
//                         result.VoteAgainst = false;
//                         result.VoteVoid = false;
//                 }
//                     question.result.Add(result);

//                     try
//                     {
//                         db.SaveChanges();
//                         return Task.FromResult<Result>(result);

//                     }
//                     catch (DbUpdateConcurrencyException e)
//                     {

//                         TempData["Message"] = "Error Adding your poll Information";

//                      return Task.FromResult<Result>(new Result());
//                     }
//             }
 
//         }

//         public async Task<ActionResult> Details(int id, int page = 1)
//         {
//             var response = await DetailsAsync(id, page);

//             return PartialView(response);

//         }

//             public Task<ResultListViewModel> DetailsAsync(int id, int page = 1)
//             {
//             try
//             {
//                 //var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//                 var result = db.Result.Where(r =>r.QuestionId == id).ToArray();
//                 var question = db.Question.Find(id);
//                 ResultListViewModel displaymodel = new ResultListViewModel
//                 {
//                     Result = result,
//                     Question = question,
//                     PagingInfo = new PagingInfo
//                     {
//                         CurrentPage = page,
//                         ItemsPerPage = PageSize,
//                         TotalItems = result.Count()
//                     },
//                     abstainBtnChoice = abstainbtnchoice
//                 };

//                 return Task.FromResult<ResultListViewModel>(displaymodel);
//             }
//             catch
//             {

//                 return Task.FromResult<ResultListViewModel>(new ResultListViewModel());
//             }

//         }


//         public async Task<ActionResult> ResultDetailsIndex(Result model)
//         {
//             var response = await ResultDetailsIndexAsync(model);

//             return PartialView(response);
//         }

//         public Task<ResultListViewModel> ResultDetailsIndexAsync(Result model)
//         {
//             try
//             {
//                 //var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//                 var result = db.Result.Where(r => r.QuestionId == model.QuestionId && r.ShareholderNum == model.ShareholderNum).ToArray();
//                 var question = db.Question.Find(model.QuestionId);
//                 ResultListViewModel displaymodel = new ResultListViewModel
//                 {
//                     Result = result,
//                     Question = question,
//                     abstainBtnChoice = abstainbtnchoice

//                 };

//                 return Task.FromResult< ResultListViewModel>(displaymodel);
//             }
//             catch
//             {

//                 return Task.FromResult<ResultListViewModel>(new ResultListViewModel());
//             }

//         }
//         //
//         // GET: /Resolution/Create

//         public async Task<ActionResult> ResultDetails(Result model)
//         {
//             var response = await ResultDetailsAsync(model);

//             return PartialView(response);
//         }


//         public Task<ResultListViewModel> ResultDetailsAsync(Result model)
//         {
//             try
//             {
//                 //var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//                 var result = db.Result.Where(r => r.QuestionId == model.QuestionId && r.Name==model.Name).ToArray();
//                 var question = db.Question.Find(model.QuestionId);
//                 ResultListViewModel displaymodel = new ResultListViewModel
//                 {
//                     Result = result,
//                     Question = question,
//                     abstainBtnChoice = abstainbtnchoice

//                 };

//                 return Task.FromResult<ResultListViewModel>(displaymodel);
//             }
//             catch
//             {
//                 return Task.FromResult<ResultListViewModel>(new ResultListViewModel());
//             }

//         }


//         public async Task<ActionResult> PresentResultDetails(Result model)
//         {
//             var response = await PresentResultDetailsAsync(model);

//             return PartialView(response);
//         }

//             public Task<ResultListViewModel> PresentResultDetailsAsync(Result model)
//         {
//             try
//             {
//                 //var companyinfo = ua.GetUserCompanyInfo();
//                 var result = db.Result.Where(r =>  r.QuestionId == model.QuestionId && r.Name == model.Name).ToArray();
//                 var question = db.Question.Find(model.QuestionId);
//                 ResultListViewModel displaymodel = new ResultListViewModel
//                 {
//                     Result = result,
//                     Question = question

//                 };

//                 return Task.FromResult<ResultListViewModel>(displaymodel);
//             }
//             catch
//             {
//                 return Task.FromResult<ResultListViewModel>(new ResultListViewModel());
//             }

//         }

//         public async Task<ActionResult> SplitResultDetails(Result model)
//         {
//             var response = await SplitResultDetailsAsync(model);

//             return PartialView(response);
//         }

//         private Task<ResultListViewModel> SplitResultDetailsAsync(Result model)
//         {
//             try
//             {
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//                 var result = db.Result.Where(r => r.QuestionId == model.QuestionId && r.Name == model.Name).ToArray();
//                 var question = db.Question.Find(model.QuestionId);
//                 ResultListViewModel displaymodel = new ResultListViewModel
//                 {
//                     Result = result,
//                     Question = question,
//                     abstainBtnChoice = abstainbtnchoice

//                 };

//                 return Task.FromResult<ResultListViewModel>(displaymodel);
//             }
//             catch
//             {
//                 return Task.FromResult<ResultListViewModel>(new ResultListViewModel());
//             }

//         }
//         public ActionResult Create()
//         {
//             return View();
//         }

//         //
//         // POST: /Resolution/Create

//         [HttpPost]
//         public ActionResult Create(FormCollection collection)
//         {
//             try
//             {
//                 // TODO: Add insert logic here

//                 return RedirectToAction("Index");
//             }
//             catch
//             {
//                 return View();
//             }
//         }

//         public async Task<ActionResult> RefreshVoteIndex(int id)
//         {
//             var response = await RefreshVoteIndexAsync(id);

//             return PartialView(response);
//         }


//         public Task<ResolutionModel> RefreshVoteIndexAsync(int id)
//         {
//             var shareholdernum = id;
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             var currentYear = DateTime.Now.Year.ToString();
//             var shareholder = db.Present.FirstOrDefault(s =>s.AGMID == UniqueAGMId && s.ShareholderNum == shareholdernum);
//             ResolutionModel model = new ResolutionModel
//             {
//                 Name = shareholder.Name,
//                 shareholdernum = shareholder.ShareholderNum,
//                 NewHolding = shareholder.Holding.ToString(),
//                 resolutionstatus = shareholder.TakePoll,
//                 splitvalue = shareholder.ShareholderNum,
//                 shareholder_Id = shareholder.Id,

//                 Question = db.Question.Where(q=>q.AGMID == UniqueAGMId).ToList()



//             };

//             return Task.FromResult< ResolutionModel>(model);
//         }


//         public async Task<ActionResult> VoteIndex(int id)
//         {
//             var response = await VoteIndexAsync(id);

//             return View(response);
//         }


//         public Task<ResolutionModel> VoteIndexAsync(int id)
//         {
//             var shareholder = db.Present.Find(id);
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//             var currentYear = DateTime.Now.Year.ToString();
//             ResolutionModel model = new ResolutionModel
//             {
//                 Name = shareholder.Name,
//                 shareholdernum = shareholder.ShareholderNum,
//                 NewHolding = shareholder.Holding.ToString(),
//                 resolutionstatus = shareholder.TakePoll,
//                 splitvalue = shareholder.ShareholderNum,
//                 shareholder_Id = shareholder.Id,
//                 abstainBtnChoice = abstainbtnchoice,

//                 Question = db.Question.Where(q => q.AGMID == UniqueAGMId).ToList()

//             };

//             return Task.FromResult< ResolutionModel>(model);
//         }


//         public async Task<ActionResult> PresentVoteIndex(int id)
//         {
//             var response = await PresentVoteIndexAsync(id);

//             return PartialView(response);
//         }

//         public Task<ResolutionModel> PresentVoteIndexAsync(int id)
//         {
//             var shareholder = db.Present.Find(id);
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//             var currentYear = DateTime.Now.Year.ToString();
//             ResolutionModel model = new ResolutionModel
//             {
//                 Name = shareholder.Name,
//                 shareholdernum = shareholder.ShareholderNum,
//                 NewHolding = shareholder.Holding.ToString(),
//                 resolutionstatus = shareholder.TakePoll,
//                 splitvalue = shareholder.ShareholderNum,
//                 shareholder_Id = shareholder.Id,
//                 abstainBtnChoice = abstainbtnchoice,
//                 Question = db.Question.Where(q =>q.AGMID == UniqueAGMId).ToList()



//             };

//             return Task.FromResult<ResolutionModel>(model);
//         }

//         [HttpPost]
//         public async Task<ActionResult> PresentVoteIndex(PostResolution post)
//         {
//             var response = await PresentVoteIndexAsync(post);

//             return RedirectToAction("PresentResultDetails", response);
//         }


//         private Task<Result> PresentVoteIndexAsync(PostResolution post)
//         {
//             int id = post.Id;
//             string response = post.RG;
//             var question = db.Question.Find(id);
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             var currentYear = DateTime.Now.Year.ToString();
//             var shareholder = db.Present.FirstOrDefault(s => s.AGMID == UniqueAGMId && s.ShareholderNum == post.shareholdernum);
//             if (shareholder == null || shareholder.present != true )
//             {
//                 //TempData["Message"] = "Accout has been Splitted Already, Unsplit to complete Resolution";
//                 HttpResponseMessage message = new HttpResponseMessage();
//                 message.StatusCode = HttpStatusCode.BadRequest;

//                 return Task.FromResult<Result>(new Result());
//             }

//                 shareholder.proxy = false;
//                 shareholder.TakePoll = true;
//                 shareholder.present = true;
//                 shareholder.split = false;
               

//             var checkresult = db.Result.FirstOrDefault(r =>r.ShareholderNum == post.shareholdernum && r.QuestionId == id);
//             if (checkresult != null)
//             {
//                 checkresult.date = DateTime.Now;
//                 checkresult.Timestamp = DateTime.Now.TimeOfDay;
//                 checkresult.Holding = double.Parse(post.NewHolding);
//                 checkresult.VoteStatus = "Voted";
//                 checkresult.Source = "Web";
//                 if (response == "FOR")
//                 {
//                     checkresult.VoteFor = true;
//                     checkresult.VoteAgainst = false;
//                     checkresult.VoteAbstain = false;
//                     checkresult.VoteVoid = false;
//                 }
//                 else if (response == "AGAINST")
//                 {
//                     checkresult.VoteAgainst = true;
//                     checkresult.VoteFor = false;
//                     checkresult.VoteAbstain = false;
//                     checkresult.VoteVoid = false;
//                 }
//                 else if (response == "ABSTAIN")
//                 {
//                     checkresult.VoteAbstain = true;
//                     checkresult.VoteFor = false;
//                     checkresult.VoteAgainst = false;
//                     checkresult.VoteVoid = false;
//                 }
//                 db.Entry(checkresult).State = EntityState.Modified;
//                 db.Entry(shareholder).State = EntityState.Modified;
//                 try
//                 {
//                     db.SaveChanges();
//                     return Task.FromResult<Result>(checkresult);
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     return Task.FromResult<Result>(new Result());
//                 }
//             }
//             else
//             {
//                 Result result = new Result();
//                 result.ShareholderNum = post.shareholdernum;
//                 result.Company = shareholder.Company;
//                 result.Year = DateTime.Now.Year.ToString();
//                 result.AGMID = UniqueAGMId;
//                 result.phonenumber = shareholder.PhoneNumber;
//                 result.Holding = double.Parse(post.NewHolding);
//                 result.Name = post.Name;
//                 result.splitValue = post.splitvalue;
//                 result.Address = shareholder.Address;
//                 result.PercentageHolding = shareholder.PercentageHolding;
//                 result.QuestionId = question.Id;
//                 result.date = DateTime.Now;
//                 result.Timestamp = DateTime.Now.TimeOfDay;
//                 result.Present = true;
//                 result.VoteStatus = "Voted";
//                 result.Source = "Web";

//                 if (response == "FOR")
//                 {
//                     result.VoteFor = true;
//                     result.VoteAgainst = false;
//                     result.VoteAbstain = false;
//                     result.VoteVoid = false;

//                 }
//                 else if (response == "AGAINST")
//                 {
//                     result.VoteAgainst = true;
//                     result.VoteFor = false;
//                     result.VoteAbstain = false;
//                     result.VoteVoid = false;
//                 }
//                 else if (response == "ABSTAIN")
//                 {
//                     result.VoteAbstain = true;
//                     result.VoteFor = false;
//                     result.VoteAgainst = false;
//                     result.VoteVoid = false;
//                 }
//                 question.result.Add(result);
//                 db.Entry(shareholder).State = EntityState.Modified;

//                 try
//                 {
//                     db.SaveChanges();
//                     HttpResponseMessage message = new HttpResponseMessage();
//                     message.StatusCode = HttpStatusCode.Accepted;
//                     return Task.FromResult<Result>(result);

//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {

//                     TempData["Message"] = "Error Adding your poll Information";

//                     HttpResponseMessage message = new HttpResponseMessage();
//                     message.StatusCode = HttpStatusCode.BadRequest;
//                     return Task.FromResult<Result>(new Result());
//                 }
//             }

//         }

//         [HttpPost]
//         public async Task<HttpResponseMessage> Votestatus(int id, SplitDeletePostModel post)
//         {
//             var response = await VotestatusAsync(id, post);

//             return response;

//         }


//         private Task<HttpResponseMessage> VotestatusAsync(int id, SplitDeletePostModel post)
//         {
//             try
//             {
//                 //var shareholder = db.BarcodeStore.Find(id);
//                 var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var currentYear = DateTime.Now.Year.ToString();
//                 var presentmodel = db.Present.SingleOrDefault(r => r.AGMID == UniqueAGMId && r.ShareholderNum == post.splitvalue);
//                 var result = db.Result.Where(r =>r.AGMID == UniqueAGMId &&  r.ShareholderNum == post.splitvalue && r.Name == post.name).ToArray();

//                 if (post.status == false &&  presentmodel.present == true)
//                 {

//                     presentmodel.TakePoll = false;
                  
//                     for (int i = 0; i < result.Length; i++)
//                     {
//                         db.Result.Remove(result[i]);
//                     }
//                     db.Entry(presentmodel).State = EntityState.Modified;
//                     db.SaveChanges();

                   
//                 }
//                 HttpResponseMessage response = new HttpResponseMessage();
//                 response.StatusCode = HttpStatusCode.OK;

//                 return Task.FromResult<HttpResponseMessage>(response);

//             }
//             catch (Exception e)
//             {
//                 HttpResponseMessage response = new HttpResponseMessage();
//                 response.StatusCode = HttpStatusCode.Forbidden;
//                 return Task.FromResult<HttpResponseMessage>(response);
//             }
//         }

//         //
//         // GET: /Resolution/Edit/5

//         public ActionResult Edit(int id)
//         {
//             return View();
//         }

//         //
//         // POST: /Resolution/Edit/5

//         [HttpPost]
//         public ActionResult Edit(int id, FormCollection collection)
//         {
//             try
//             {
//                 // TODO: Add update logic here

//                 return RedirectToAction("Index");
//             }
//             catch
//             {
//                 return View();
//             }
//         }

//         //
//         // GET: /Resolution/Delete/5

//         public ActionResult Delete(int id)
//         {
//             return View();
//         }

//         //
//         // POST: /Resolution/Delete/5

//         [HttpPost]
//         public ActionResult Delete(int id, FormCollection collection)
//         {
//             try
//             {
//                 // TODO: Add delete logic here

//                 return RedirectToAction("Index");
//             }
//             catch
//             {
//                 return View();
//             }
//         }
//     }
// }
