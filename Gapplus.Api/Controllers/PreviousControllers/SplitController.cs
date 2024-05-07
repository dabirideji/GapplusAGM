// using BarcodeGenerator.Barcode;
// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;
// using BarcodeGenerator.Util;
// using Microsoft.AspNetCore.Http.HttpResults;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Configuration;
// using System.Data;
// using System.Data.SqlClient;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Threading.Tasks;


// namespace BarcodeGenerator.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]/[action]")]
//     public class SplitController : ControllerBase
//     {
//           UsersContext db ;
//         UserAdmin ua;
//        private readonly ITempDataManager _tempDataManager;
//        private readonly IViewBagManager _viewBagManager;
//         public SplitController(UsersContext _db,ITempDataManager tempDataManager,IViewBagManager viewBagManager)
//         {
//             db=_db;
//             _tempDataManager=tempDataManager;
//             _viewBagManager=viewBagManager;
//             ua=new UserAdmin(db);     
//         }


//         //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

//         //private static int RetrieveAGMUniqueID()
//         //{
//         //    UsersContext adb = new UsersContext();
//         //    var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

//         //    return AGMID;
//         //}

//         //private static int UniqueAGMId = RetrieveAGMUniqueID();

//         public int PageSize = 10;

//         public async Task<ActionResult> Index(int id)
//         {
//             var response = await IndexAsync(id);
//             return View(response);
//         }


//         private Task<SplitModel> IndexAsync(int id)
//         {
//             var shareholder = db.BarcodeStore.Find(id);
//             SplitModel split = new SplitModel();
//             split.Name = shareholder.Name;
//             split.Holding = shareholder.Holding.ToString();
//             split.ShareholderNumber = shareholder.ShareholderNum;
//             split.splitStatus = shareholder.split;
//             split.shareholder_Id = shareholder.Id;
//             split.splitvalue = shareholder.ShareholderNum;

//             return Task.FromResult<SplitModel>(split);
//         }


//         public async Task<ActionResult> SplitIndex(int id)
//         {
//             var response = await SplitIndexAsync(id);

//             return PartialView(response);
//         }


//         private Task<SplitModel> SplitIndexAsync(int id)
//         {
//             var shareholder = db.BarcodeStore.Find(id);

//             var companyinfo = ua.GetUserCompanyInfo();
//             string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//             Int64 maxvalue = 0;
//             SqlConnection conn =
//                     new SqlConnection(connStr);
//             string query = "SELECT MAX(ShareholderNum) FROM BarcodeModels Where Company ='" + companyinfo + "'";
//             //string query2 = "select * from BarcodeModels WHERE Name LIKE '%" + searchValue + "%' OR ShareholderNum LIKE '%" + searchValue + "%'";
//             conn.Open();
//             SqlCommand cmd = new SqlCommand(query, conn);
//             SqlDataReader read = cmd.ExecuteReader();

//             //Int64 maxvalue = 0;
//             while (read.Read())
//             {
//                 maxvalue = read.GetInt64(0);
//             }
//             read.Close();
//             conn.Close();
//             //var lastShareholderNumber = db.BarcodeStore.OrderByDescending(i=>i.Id).FirstOrDefault();
//             SplitModel split = new SplitModel();
//             split.Name = shareholder.Name;
//             split.Holding = shareholder.Holding.ToString();
//             split.ShareholderNumber = shareholder.ShareholderNum;
//             split.splitStatus = shareholder.split;
//             split.shareholder_Id = shareholder.Id;
//             split.ParentNumber = shareholder.ParentAccountNumber;
//             //split.splitvalue = shareholder.ShareholderNum;
//             split.splitvalue = maxvalue;

//             return Task.FromResult< SplitModel>(split);
//         }

//         public ActionResult SplitView(int id)
//         {

//             return View();
//         }



//         public async Task<JsonResult> GetShareholders(string query = null)
//         {

//             var response = await GetShareholdersAsync(query);
//             return Json(response, JsonRequestBehavior.AllowGet);

//         }


//         private Task<IEnumerable<BarcodeModelDto>> GetShareholdersAsync(string query)
//         {
//             var companyinfo = ua.GetUserCompanyInfo();
//             var shareholderQuery = db.BarcodeStore.Where(b => b.Company == companyinfo).ToList();
//             if (!String.IsNullOrWhiteSpace(query))
//                 shareholderQuery = shareholderQuery.Where(s => s.Name.Contains(query)).ToList();


//             var shareholderData = GetShareholdersResponse(shareholderQuery);

//             return Task.FromResult< IEnumerable<BarcodeModelDto>>(shareholderData);

//         }


//         private IEnumerable<BarcodeModelDto> GetShareholdersResponse(IEnumerable<BarcodeModel> shareholders)
//         {
//             var shareholderData = from c in shareholders
//                                   select new BarcodeModelDto()
//                                   {
//                                       SN = c.SN,
//                                       Id = c.Id,
//                                       Name = c.Name,
//                                       Address = c.Address,
//                                       ShareholderNum = c.ShareholderNum,
//                                       Holding = c.Holding.ToString(),
//                                       PercentageHolding = c.PercentageHolding.ToString(),
//                                       Company = c.Company,
//                                       PhoneNumber = c.PhoneNumber,
//                                       emailAddress = c.emailAddress,
//                                   };

//             return shareholderData;

//         }

//         public async Task<ActionResult> RefreshIndex(int id)
//         {
//             var response = await RefreshIndexAsync(id);

//             return PartialView(response);
//         }

//         public Task<SplitModel> RefreshIndexAsync(int id)
//         {
//             var shareholdernum = id;
//             var companyinfo = ua.GetUserCompanyInfo();
//             var shareholder = db.BarcodeStore.SingleOrDefault(b=>b.Company == companyinfo && b.ShareholderNum == shareholdernum);
//             SplitModel split = new SplitModel();
//             split.Name = shareholder.Name;
//             split.Holding = shareholder.Holding.ToString();
//             split.ShareholderNumber = shareholder.ShareholderNum;
//             split.splitStatus = shareholder.split;
//             split.shareholder_Id = shareholder.Id;
//             split.splitvalue = shareholder.ShareholderNum;
//             split.ParentNumber = shareholder.ParentAccountNumber;

//             return Task.FromResult<SplitModel>(split);
//         }
//         //
//         // GET: /Split/Details/5

//         public async Task<ActionResult> Details(int id, int page = 1)
//         {

//             var response = await DetailsAync(id, page);
//                 return PartialView(response);

//         }


//         public Task<ResultListViewModel> DetailsAync(int id, int page)
//         {
//             try
//             {
//                 //var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//                 var result = db.Result.Where(r => r.QuestionId == id).ToArray();
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

//         [HttpPost]
//         public async Task<string> CommitSplit(ResolutionPostModel post)
//         {
//             var response = await CommitSplitAsync(post);

//             return response;
//         }


//         public Task<string> CommitSplitAsync(ResolutionPostModel post)
//         {
//             try
//             {

//                 var companyinfo = ua.GetUserCompanyInfo();
//                 var setting = db.Settings.Where(s=>s.CompanyName==companyinfo).ToArray();

//             var shareholder = db.BarcodeStore.SingleOrDefault(s =>s.Company==companyinfo && s.ShareholderNum == post.splitvalue);
//             if (shareholder.resolution == true || shareholder.Present == true)
//             {
//                 //TempData["Message"] = "Full Resolution has been taken already with this account, remove resolution to complete Split";
//                 return Task.FromResult<string>("Already marked present");
//             }

//             shareholder.split = true;
//             shareholder.NotVerifiable = true;

//                 db.Entry(shareholder).State = EntityState.Modified;
//                 BarcodeModel bmodel = new BarcodeModel();
//                 bmodel.Name = shareholder.Name;
//                 bmodel.Address = shareholder.Address;
//                 bmodel.Company = shareholder.Company;
//                 bmodel.ShareholderNum = post.newnumber;
//                 bmodel.emailAddress = shareholder.emailAddress;
//                 bmodel.PhoneNumber = shareholder.PhoneNumber;
//                 bmodel.AddedSplitAccount = true;
//                 if (post.ParentNumber == 0)
//                 {
//                     bmodel.ParentAccountNumber = shareholder.ShareholderNum;
//                 }
//                 else
//                 {
//                     bmodel.ParentAccountNumber = post.ParentNumber;
//                 }
//                 bmodel.Holding = double.Parse(post.Holding);
//                 //bmodel.split = true;

//                 if (setting.Length > 0 && setting[0].ShareHolding !=0)
//                 {
//                     var totalHolding = setting[0].ShareHolding;
//                     var nHolding = Double.Parse(post.Holding);
//                     Double TotalShareholding = totalHolding;
//                     var newPHolding = (nHolding / TotalShareholding) * 100;

//                     bmodel.PercentageHolding = newPHolding;
//                 }

//                 db.BarcodeStore.Add(bmodel);

//                 db.SaveChanges();
//                 return Task.FromResult<string>("Success");

//             }
//             catch(Exception e)
//             {
//                 return Task.FromResult<string>("Failed");
//             }

           
//         }

//         public async Task<ActionResult> ResultDelete(DeleteReturnModel model)
//         {
//             var response = await ResultDeleteAsync(model);

//             return PartialView(response);
//         }


//         public Task<ResultListViewModel> ResultDeleteAsync(DeleteReturnModel model)
//         {
//             try
//             {
//                 //var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//                 var currentYear = DateTime.Now.Year.ToString();
//                 var question = db.Question.SingleOrDefault(r=> r.AGMID == UniqueAGMId && r.question==model.Question);
//                 var result = db.Result.Where(r =>r.QuestionId == question.Id && r.Name == model.name).ToArray();
                
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

//         [HttpPost]
//         public async Task<ActionResult> ResultDetails(PostValue model)
//         {
//             var response = await ResultDetailsAsync(model);

//             return PartialView(response);
//         }


//         private Task<ResultListViewModel> ResultDetailsAsync(PostValue model)
//         {
//             try
//             {
//                 //var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//                 var value = Int64.Parse(model.value);
//                 Question question = new Question();
//                 IEnumerable<Result> result = new List<Result>();
//                 if (model.Id != 0)
//                 {
//                      result = db.Result.Where(r => r.QuestionId == model.Id && r.ShareholderNum == value).ToArray();
//                     question = db.Question.Find(model.Id);
//                 }

                
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



//         [HttpPost]
//         public async Task<ActionResult> SplitResultDetails(PostValue model)
//         {
//             var response = await SplitResultDetailsAsync(model);

//             return PartialView(response);
//         }


//         private Task<ResultListViewModel> SplitResultDetailsAsync(PostValue model)
//         {
//             try
//             {
//                 //var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//                 var value = Int64.Parse(model.value);
//                 var result = db.Result.Where(r => r.QuestionId == model.Id && r.ParentNumber == value).ToArray();
//                 var question = db.Question.Find(model.Id);
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

//                 return Task.FromResult< ResultListViewModel>(new ResultListViewModel());
//             }

//         }



//         [HttpPost]
//         public async Task<ActionResult> PresentDetails(PostValue model)
//         {
//             var response = await PresentDetailsAsync(model);

//             return PartialView(response);
//         }


//         public Task<ResultListViewModel> PresentDetailsAsync(PostValue model)
//         {
//             try
//             {
//                 //var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//                 var value = int.Parse(model.value);
//                 var result = db.Result.Where(r =>r.QuestionId == model.Id && r.ShareholderNum == value).ToArray();
//                 var question = db.Question.Find(model.Id);
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
//             };

//         }


//         [HttpPost]
//         public async Task<ActionResult> SplitResult(int id, SplitResultPostModel post)
//         {
//             var response = await SplitResultAsync(id, post);

//             return PartialView(response);
//         }



//         private Task<SplitResultModel> SplitResultAsync(int id, SplitResultPostModel post)
//         {
//             try
//             {
//                 //var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//                 var shareholder = db.BarcodeStore.Find(id);
//                 var currentYear = DateTime.Now.Year.ToString();
//                 //IEnumerable<Result> result = new List<Result>();

//                 //if(shareholder.ParentAccountNumber!=0)
//                 //{
//                     var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.ParentNumber == shareholder.ShareholderNum).ToArray();
//                 //}
                
                
//                 var question = db.Question.Where(q=>q.AGMID == UniqueAGMId);
//                 SplitResultModel model = new SplitResultModel
//                 {
//                     Result = result,
//                     question = question,
//                     abstainBtnChoice = abstainbtnchoice

//                 };

//                 return Task.FromResult<SplitResultModel>(model);
//             }
//             catch
//             {

//                 return Task.FromResult<SplitResultModel>(new SplitResultModel());
//             }

//         }


//         //
//         // GET: /Split/Create


//         [HttpPost]
//         public async Task<string> SplitStatus(int id, SplitDeletePostModel post)
//         {
//             var response = await SplitStatusAsync(id, post);
//             return response;

//         }



//         private Task<string> SplitStatusAsync (int id, SplitDeletePostModel post)
//         {
//             try
//             {
//                 var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var shareholder = db.BarcodeStore.Find(id);
//                 var currentYear = DateTime.Now.Year.ToString();
//             var result = db.Result.Where(r=>r.AGMID== UniqueAGMId && r.ParentNumber == post.shareholdernum && r.Name==post.name || r.Company == companyinfo && r.ShareholderNum==post.shareholdernum && r.Name == post.name).ToArray();
//             //var proxy = db.Present.Where(p=>p.ShareholderNum==post.splitvalue);
//                 var proxy = db.Present.Where(p =>p.AGMID == UniqueAGMId && p.ParentNumber== post.shareholdernum || p.Company == companyinfo && p.AGMID == UniqueAGMId && p.ShareholderNum == post.shareholdernum);
//                 var splitbarcodemodel = db.BarcodeStore.Where(p => p.Company == companyinfo && p.ParentAccountNumber == post.shareholdernum && p.AddedSplitAccount == true);

//                 if(splitbarcodemodel.Count()>0)
//                 {
//                     foreach(var item in splitbarcodemodel)
//                     {
//                         if (item.Consolidated == true)
//                         {
//                             return Task.FromResult<string>("A member of the split ("+" "+ item.ShareholderNum +" "+") has been Consolidated!") ;
//                         }

//                     }
//                 }

//                 if (post.status == false && shareholder.resolution == false || shareholder.Present == false)
//             {
//                 shareholder.split = false;
//                     shareholder.PresentByProxy = false;
//                     shareholder.resolution = false;
//                     shareholder.TakePoll = false;
//                 shareholder.ParentAccountNumber = 0;
//                     shareholder.NotVerifiable = false;
//                     //if (shareholder.emailAddress != null && shareholder.emailAddress.StartsWith("proxy"))
//                     //{
//                     //    shareholder.emailAddress = shareholder.emailAddress.Substring(5);
//                     //}
//                     for (int i=0;i<result.Length;i++)
//                 {
//                     db.Result.Remove(result[i]);
//                 }
//                 foreach(var item in proxy)
//                 {
//                 db.Present.Remove(item);
//                 }

//                     foreach (var item in splitbarcodemodel)
//                     {
//                         db.BarcodeStore.Remove(item);
//                     }
//                 }
//             else if (post.status == false && shareholder.resolution == true)
//             {
//                 shareholder.PresentByProxy = true;
                
 

//             }
//             else if (post.status == true && shareholder.resolution == true||shareholder.Present==true)
//             {
//                 TempData["Message"] = "Account may have been checked Proxy!";
//                 return Task.FromResult<string>("Account may have been checked Proxy!");

//             }
//             else 
//             {
//                 shareholder.split = false;
//             }
            
//             db.Entry(shareholder).State = EntityState.Modified;
//             db.SaveChanges();
//                 Functions.PresentCount(UniqueAGMId, true);
//                 HttpResponseMessage response = new HttpResponseMessage();
//             response.StatusCode = HttpStatusCode.OK;

//             return Task.FromResult<string>("Success!");
//             }
//             catch(Exception e)
//             {
//                 TempData["Message"] = "Couldn't Clear All Resolution data";
//                 return Task.FromResult<string>("Something went wrong! Couldn't Clear All Resolution data. Contact Admin");
//             }
//         }

//         //
//         // POST: /Split/Create

//         [HttpPost]
//         public ActionResult Create(IFormFile collection)
//         {
//             try
//             {
//                 // TODO: Add insert logic here

//                 return RedirectToAction("Index");
//             }
//             catch
//             {
//                 return Ok();
//             }
//         }


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
//                 return Ok();
//             }
//         }

//         //
//         // GET: /Split/Delete/5

//         [HttpPost]
//         public async Task<ActionResult> Delete(DeletePostValue model)
//         {
//             var response = await DeleteAsync(model);

//             return RedirectToAction("ResultDelete", response);
//         }


//         private Task<DeleteReturnModel> DeleteAsync(DeletePostValue model)
//         {
//             try
//             {
//                 var result = db.Result.Find(model.id);
//                 db.Result.Remove(result);
//                 db.SaveChanges();

//                 DeleteReturnModel returnmodel = new DeleteReturnModel();
//                 returnmodel.name = model.name;
//                 returnmodel.Question = model.Question;
//                 //HttpResponseMessage response = new HttpResponseMessage();
//                 //response.StatusCode = HttpStatusCode.OK;
//                 return Task.FromResult<DeleteReturnModel>(returnmodel);
//             }
//             catch(Exception e)
//             {
//                 return Task.FromResult<DeleteReturnModel>(new DeleteReturnModel());
//             }
//         }

//         //
//         // POST: /Split/Delete/5

//         public async Task<HttpResponseMessage> DeleteAll(ResultPost post)
//         {
//             var response = await DeleteAllAsync(post);

//             return response;
//         }

//         public Task<HttpResponseMessage> DeleteAllAsync(ResultPost post)
//         {
//             try
//             {
//                 // TODO: Add delete logic here
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var holding = double.Parse(post.Holding);
//                 var result = db.Result.Where(r=>r.AGMID == UniqueAGMId && r.ShareholderNum==post.Identity && r.Holding == holding).ToArray();

//                 for (int i = 0; i < result.Length;i++ )
//                 {
//                     db.Result.Remove(result[i]);
//                 }
//                 db.SaveChanges();
//                 HttpResponseMessage response = new HttpResponseMessage();
//                 response.StatusCode = HttpStatusCode.OK;
//                 return Task.FromResult<HttpResponseMessage>(response);
//             }
//             catch
//             {
//                 HttpResponseMessage response = new HttpResponseMessage();
//                 response.StatusCode = HttpStatusCode.BadRequest;
//                 return Task.FromResult<HttpResponseMessage>(response);
//             }
//         }
//     }
// }
