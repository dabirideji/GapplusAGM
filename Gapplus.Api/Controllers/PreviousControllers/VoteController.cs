using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using BarcodeGenerator.Util;
using Gapplus.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Controllers
{
    //  [Authorize]
    public class VoteController : ControllerBase
    {
        //
        // GET: /Vote/
        private readonly IViewBagManager _viewBagManager;

        public VoteController(UsersContext context, IViewBagManager viewBagManager)
        {
            _viewBagManager = viewBagManager;
            db = context;
            ua = new(db);
            _votingService = new(db);
        }
        UsersContext db;
        UserAdmin ua;
        VotingService _votingService;

        // private static string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static string connStr = DatabaseManager.GetConnectionString();
        SqlConnection conn =
                  new SqlConnection(connStr);

        //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

        //private static int RetrieveAGMUniqueID()
        //{
        //    UsersContext adb = new UsersContext();
        //    var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

        //    return AGMID;
        //}

        //private static int UniqueAGMId = RetrieveAGMUniqueID();

        private static string currentYear = DateTime.Now.Year.ToString();


        public int PageSize = 10;


        // public async Task<ActionResult> Index(int page = 1)
        // {
        //     var returnUrl = HttpContext.Request.Url.AbsolutePath;

        //     string returnvalue = "";
        //     if (HttpContext.Request.QueryString.Count > 0)
        //     {
        //         returnvalue = HttpContext.Request.QueryString["rel"].ToString();
        //     }
        //     ViewBag.value = returnvalue.Trim();
        //     var companyinfo = ua.GetUserCompanyInfo();
        //     if (String.IsNullOrEmpty(companyinfo))
        //     {
        //         return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
        //     }

        //     var response = await IndexAsync();



        //     return PartialView(response);
        // }



        [HttpGet("Index")]
        public async Task<IActionResult> Index(int page = 1)
        {
            string returnUrl = Request.Path.Value;

            string returnValue = "";
            if (Request.Query.ContainsKey("rel"))
            {
                returnValue = Request.Query["rel"];
            }
            // ViewBag.value = returnValue.Trim();
            _viewBagManager.SetValue("value", returnValue.Trim());
            var companyInfo = ua.GetUserCompanyInfo();
            if (String.IsNullOrEmpty(companyInfo))
            {
                // You might need to adjust how you handle redirects in .NET Core based on your application's setup
                return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnValue.Trim() });
            }

            var response = await IndexAsync();

            // return PartialView(response);
            return Ok(response);
        }



















        public Task<QuestionListViewModel> IndexAsync()
        {
            //var companyinfo = ua.GetUserCompanyInfo
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var currentYear = DateTime.Now.Year.ToString();
            var question = db.Question.Where(q => q.AGMID == UniqueAGMId).OrderBy(s => s.Id).ToList();
            SettingsModel agmEventSetting = null;
            if (UniqueAGMId != -1)
            {
                var UniqueAgmId = UniqueAGMId;
                agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAgmId);
                agmEventSetting.StartVoting = false;
                //db.Entry(agmEventSetting).State = EntityState.Modified;
                db.SaveChanges();
            }

            var abstainbtnchoice = true;
            var forBg = "green";
            var againstBg = "red";
            var abstainBg = "blue";
            var voidBg = "black";
            if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
            {
                abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
                if (!string.IsNullOrEmpty(agmEventSetting.VoteForColorBg))
                {
                    forBg = agmEventSetting.VoteForColorBg;
                }
                if (!string.IsNullOrEmpty(agmEventSetting.VoteAgainstColorBg))
                {
                    againstBg = agmEventSetting.VoteAgainstColorBg;
                }
                if (!string.IsNullOrEmpty(agmEventSetting.VoteAbstaincolorBg))
                {
                    abstainBg = agmEventSetting.VoteAbstaincolorBg;
                }
                if (!string.IsNullOrEmpty(agmEventSetting.VoteVoidColorBg))
                {
                    voidBg = agmEventSetting.VoteVoidColorBg;
                }
            }
            QuestionListViewModel displaymodel = new QuestionListViewModel
            {
                question = question,
                abstainBtnChoice = abstainbtnchoice,
                forBg = forBg,
                againstBg = againstBg,
                abstainBg = abstainBg,
                voidBg = voidBg,
                agmid = UniqueAGMId
                //PagingInfo = new PagingInfo
                //{
                //    CurrentPage = page,
                //    ItemsPerPage = PageSize,
                //    TotalItems = question.Count()
                //}
            };

            return Task.FromResult<QuestionListViewModel>(displaymodel);
        }



        public async Task<string> StartVote()
        {
            var user = User.Identity.Name;
            var companyinfo = ua.GetUserCompanyInfo(user);

            var response = await StartVoteAsync(companyinfo);

            return response;
        }


        //public async Task<HttpResponseMessage> StartVote()
        private async Task<string> StartVoteAsync(string companyinfo)
        {

            var Uniagmid = ua.RetrieveAGMUniqueID(companyinfo);


            if (!string.IsNullOrEmpty(companyinfo) && Uniagmid != -1)
            {
                //check if actve resolution has bn Synced.
                var resolution = EventResolution(Uniagmid);
                if (resolution != null)
                {
                    //var checkIfSyncAsBnDone = resolution.syncStatus;
                    //if(!checkIfSyncAsBnDone)
                    //{
                    var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == Uniagmid);



                    if (agmEvent != null)
                    {
                        Functions.ResolutionProgress(resolution.question, resolution.Id, companyinfo, agmEvent.allChannels, agmEvent.webChannel, agmEvent.mobileChannel);
                        TimerControll.ResetTime(companyinfo, agmEvent.AGMID);

                        TimerControll.setTime(companyinfo, agmEvent.AGMID, agmEvent.CountDownValue);
                        //agmEvent.StopAdmittance = true;
                        //if (agmEvent.allChannels || agmEvent.webChannel || agmEvent.mobileChannel)
                        //{


                        //}
                    }
                    //db.SaveChanges();
                    //BackEndTimer.SetTimer(companyinfo);
                    //ClikapadController ckp = new ClikapadController();
                    //ckp.GetClikapadResult(companyinfo);

                    return "Success";
                    //}
                    //return System.Threading.Tasks.Task.FromResult<string>("202");
                }
                //return System.Threading.Tasks.Task.FromResult<string>("Resolution has been Synchronized");


            }
            return "205";



        }


        private VoteModel EventResolution(int agmid)
        {
            var votemodel = new VoteModel();
            var resolution = db.Question.SingleOrDefault(r => r.AGMID == agmid && r.questionStatus == true);
            if (resolution != null)
            {
                votemodel.question = resolution.question;
                votemodel.Id = resolution.Id;
                votemodel.syncStatus = resolution.syncStatus;
            }
            return votemodel;
        }


        public async Task<string> StopVote()
        {
            var response = await StopVoteAsync();

            return response;
        }


        private Task<string> StopVoteAsync()
        {
            //var user = User.Identity.Name;
            var companyinfo = ua.GetUserCompanyInfo();

            if (!string.IsNullOrEmpty(companyinfo))
            {
                var Uniagmid = ua.RetrieveAGMUniqueID(companyinfo);
                TimerControll.stopTime(companyinfo, Uniagmid);
                Functions.StopTimer(companyinfo);
                //ClikapadController ckp = new ClikapadController();
                //BackEndTimer.StopTimer(companyinfo);
                return System.Threading.Tasks.Task.FromResult<string>("Success");
            }

            return System.Threading.Tasks.Task.FromResult<string>("Timer not stopped");
        }


        //public async Task<string> ResetTime()
        //{
        //    var response = await ResetTimeAsync();

        //    return response;
        //}

        public async Task<string> ResetTime(int id)
        {
            var response = await ResetTimeAsync(id);

            return response;
        }


        private async Task<string> ResetTimeAsync(int id)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            if (!string.IsNullOrEmpty(companyinfo))
            {
                var Uniagmid = ua.RetrieveAGMUniqueID(companyinfo);
                TimerControll.ResetTime(companyinfo, Uniagmid);

                var response = await _votingService.VoteReset(id);
                return response.Message;
            }
            return "Timer not reset";
        }




        public async Task<string> ClearTimer()
        {
            var response = await ClearTimerAsync();

            return response;
        }


        private Task<string> ClearTimerAsync()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            TimerControll.ClearTimerStore();
            //BackEndTimer.ClearTimerStore(companyinfo)

            return System.Threading.Tasks.Task.FromResult<string>("Success");
        }
        //
        // GET: /Vote/Details/5

        public ActionResult Collation(int id)
        {

            //var companyinfo = ua.GetUserCompanyInfo();
            var currentYear = DateTime.Now.Year.ToString();
            var result = db.Result.Where(r => r.QuestionId == id).ToList();
            SettingsModel agmEventSetting = null;
            if (result.Any())
            {
                var UniqueAgmId = result.First().AGMID;
                agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAgmId);
            }

            var abstainbtnchoice = true;
            if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
            {
                abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
            }
            VoteCollation collate = new VoteCollation
            {
                For = result.Where(y => y.VoteFor == true).Count().ToString(),
                Against = result.Where(n => n.VoteAgainst == true).Count().ToString(),
                Abstain = result.Where(n => n.VoteAbstain == true).Count().ToString(),
                VoteVoid = result.Where(n => n.VoteVoid == true).Count().ToString(),
                QuestionId = id,
                abstainBtnChoice = abstainbtnchoice

            };

            // return PartialView(collate);
            return Ok(collate);
        }

        // public async Task<ActionResult> VoteCollection()
        // {
        //     string returnvalue = "";
        //     if (HttpContext.Request.QueryString.Count > 0)
        //     {
        //         returnvalue = HttpContext.Request.QueryString["rel"].ToString();
        //     }
        //     ViewBag.value = returnvalue.Trim();
        //     var response = await VoteCollectionAsync();

        //     return PartialView(response);
        // }
        [HttpGet("VoteCollection")]
        public async Task<IActionResult> VoteCollection()
        {
            string returnValue = "";
            if (Request.Query.ContainsKey("rel"))
            {
                returnValue = Request.Query["rel"];
            }
            // ViewBag.value = returnValue.Trim();
            _viewBagManager.SetValue("value", returnValue.Trim());

            var response = await VoteCollectionAsync();

            // return PartialView(response);
            return Ok(response);
        }

        public Task<VoteCollectionModel> VoteCollectionAsync()
        {
            try
            {
                //var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID();

                VoteCollectionModel model = new VoteCollectionModel();
                var Present = db.Present.Where(r => r.AGMID == UniqueAGMId && r.present == true || r.AGMID == UniqueAGMId && r.proxy == true).ToList();
                var allPresent = Present.Count().ToString();
                model.allPresent = Present.Count();
                var Voted = db.Present.Where(r => r.AGMID == UniqueAGMId && r.TakePoll == true).ToList();
                var allVoted = Voted.Count().ToString();
                model.allVoted = Voted.Count();
                var votedcount = Convert.ToDouble(allVoted);
                var presentcount = Convert.ToDouble(allPresent);
                var percentageVote = (votedcount / presentcount) * 100;
                model.VotePercentage = percentageVote;

                return System.Threading.Tasks.Task.FromResult<VoteCollectionModel>(model);

            }
            catch (Exception e)
            {
                VoteCollectionModel model = new VoteCollectionModel();
                return System.Threading.Tasks.Task.FromResult<VoteCollectionModel>(model);
            }

        }

        public async Task<ActionResult> ElapsedRealTime()
        {
            var time = await ElapsedTimeAsync();

            // return PartialView(time);
            return Ok(time);
        }

        public async Task<ActionResult> ElapsedTime()
        {
            var time = await ElapsedTimeAsync();

            // return PartialView(time);
            return Ok(time);

        }

        public Task<TimeSpan> ElapsedTimeAsync()
        {
            var user = User.Identity.Name;
            string returnvalue = "";
            TimeSpan time;

            if (Request.Query.ContainsKey("rel"))
            {
                returnvalue = Request.Query["rel"];
            }
     
            // ViewBag.value = returnvalue.Trim();
            _viewBagManager.SetValue("value", returnvalue.Trim());
            var companyinfo = ua.GetUserCompanyInfo(user);
            if (!string.IsNullOrEmpty(companyinfo))
            {
                time = TimerControll.GetTime(companyinfo);
                return Task.FromResult<TimeSpan>(time);
            }
            time = new TimeSpan();
            return Task.FromResult<TimeSpan>(time);
        }


        public async Task<ActionResult> aElapsedTime(string id)
        {
            var time = await aElapsedTimeAsync(id);
            // return PartialView(time);
            return Ok(time);

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




        public ActionResult ElapsedTimeLarge()
        {
            var user = User.Identity.Name;
            string returnvalue = "";
            TimeSpan time;
            string returnValue = "";
            if (Request.Query.ContainsKey("rel"))
            {
                returnValue = Request.Query["rel"];
            }
            // ViewBag.value = returnvalue.Trim();
            _viewBagManager.SetValue("value", returnvalue.Trim());
            var companyinfo = ua.GetUserCompanyInfo(user);
            if (!string.IsNullOrEmpty(companyinfo))
            {
                time = TimerControll.GetTime(companyinfo);
                // return PartialView(time);
                return Ok(time);
            }
            time = new TimeSpan();
            // return PartialView(time);
            return Ok(time);




        }

        public ActionResult PollTime()
        {

       

            string returnvalue = "";

            if (Request.Query.ContainsKey("rel"))
            {
                returnvalue = Request.Query["rel"];
            }
     
                 _viewBagManager.SetValue("value", returnvalue.Trim());
                 

                //  return PartialView();
                 return Ok();
          
        }

        public ActionResult Details(int id, int page = 1)
        {
            try
            {
                //var companyinfo = ua.GetUserCompanyInfo();
                var result = db.Result.Where(r => r.QuestionId == id).ToArray();
                var question = db.Question.Find(id);
                SettingsModel agmEventSetting = null;
                if (question != null)
                {
                    agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == question.AGMID);
                }

                var abstainbtnchoice = true;
                if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
                {
                    abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
                }
                ResultListViewModel displaymodel = new ResultListViewModel
                {
                    //Result = result,
                    Question = question,
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = page,
                        ItemsPerPage = PageSize,
                        TotalItems = result.Count()
                    },
                    abstainBtnChoice = abstainbtnchoice

                };

                // return PartialView(displaymodel);
                return Ok(displaymodel);
            }
            catch
            {
                ResultListViewModel model = new ResultListViewModel();
                model.Result = null;
                model.PagingInfo = null;

                // return PartialView(model);
                return Ok(model);
            }

        }



        public ActionResult AjaxHandler(int id)
        {
            try
            {
                //Creating instance of DatabaseContext class  
                // using (UsersContext _context = new UsersContext())
                using (UsersContext _context = db)
                {
                   // already had coomment before  //var companyinfo = ua.GetUserCompanyInfo();


                    // var draw = Request.Form.GetValues("draw").FirstOrDefault();
                    // var start = Request.Form.GetValues("start").FirstOrDefault();
                    // var length = Request.Form.GetValues("length").FirstOrDefault();
                    // var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                    // var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                    // var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();


                    //Paging Size (10,20,50,100)    
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int recordsTotal = 0;
                    // Getting all Shareholder data 
                    //var result = db.Result.Where(r => r.QuestionId == id).ToArray();
                    var allresult = db.Result.Where(r => r.QuestionId == id).ToArray();
                    IEnumerable<Result> filteredresult;

                    //Sorting  
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        filteredresult = allresult.OrderBy(s => s.Id);
                    }
                    //Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        filteredresult = db.Result.Where(r => r.QuestionId == id).ToArray()
                                 .Where(c => c.Name.Contains(searchValue)

                                             ||
                                             c.ShareholderNum.ToString() == searchValue);
                    }
                    else
                    {
                        filteredresult = allresult;
                    }
                    //Paging   
                    var displayedresult = filteredresult
                             .Skip(skip)
                             .Take(pageSize);
                    //var result = displayedCompanies;
                    var shareholderresult = GetPresent(displayedresult);
                    //var shareholderData = from c in displayedCompanies
                    //             //select c;
                    //  select new[] { Convert.ToString(c.SN),Convert.ToString(c.Id), c.Name, c.Address, c.ShareholderNum, c.Holding, c.PercentageHolding,
                    //      c.PhoneNumber, c.emailAddress,Convert.ToString(c.Present),Convert.ToString(c.PresentByProxy) };   

                    //total number of rows count     
                    recordsTotal = allresult.Count();


                    //Returning Json Data    
                    // return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = shareholderresult }, JsonRequestBehavior.AllowGet);
                    return Ok(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = shareholderresult });
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private IEnumerable<Result> GetPresent(IEnumerable<Result> displayedpresent)
        {
            var shareholderresult = from c in displayedpresent
                                    select new Result()
                                    {

                                        Id = c.Id,
                                        Name = c.Name,
                                        Address = c.Address,
                                        Holding = c.Holding,
                                        PercentageHolding = c.PercentageHolding,
                                        ShareholderNum = c.ShareholderNum,
                                        VoteFor = c.VoteFor,
                                        VoteAgainst = c.VoteAgainst,
                                        VoteAbstain = c.VoteAbstain

                                    };

            return shareholderresult.ToList();

        }
        //
        // GET: /Vote/Create

        public ActionResult Create()
        {

            return Ok();
        }

        //
        // POST: /Vote/Create

        [HttpPost]
        public string Create(string model, string type)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID();
                // TODO: Add insert logic here
                Question question = new Question();
                question.question = model;
                question.Company = companyinfo;
                question.date = DateTime.Now;
                question.AGMID = UniqueAGMId;
                question.Year = DateTime.Now.Year.ToString();
                question.voteType = type;
                db.Question.Add(question);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return "success";
            }
            catch
            {
                return "failed";
            }
        }

        [HttpPost]
        public bool UpdateResolution(int id, string type)
        {
            try
            {
                var question = db.Question.Find(id);
                question.voteType = type;
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return true;
            }
            catch
            {
                return false;
            }
        }

        //[HttpPost]
        //public ActionResult Create(QuestionViewModel collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        Question question = new Question();
        //        question.question = collection.question;
        //        question.date = DateTime.Now;
        //        db.Question.Add(question);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /Vote/Edit/5

        public ActionResult Edit(int id)
        {
            var question = db.Question.Find(id);

            // return PartialView(question);
            return Ok(question);
        }

        //
        // POST: /Vote/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Question collection)
        {
            try
            {
                //collection.date = DateTime.Now;
                db.Entry(collection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                // return View();
                return StatusCode(500);
            }
        }

        [HttpPost]
        public ActionResult QuestionStatus(int id, QuestionStatus data)
        {
            //var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var question = db.Question.Find(id);

            string UpdatePresentShareholdersquery = "UPDATE PresentModels SET [TakePoll] = 0 Where AGMID='" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1";
            conn.Open();
            SqlCommand cmd = new SqlCommand(UpdatePresentShareholdersquery, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            //question.questionStatus = data.status;
            var questions = db.Question.Where(q => q.AGMID == UniqueAGMId).ToArray();
            var eventSetting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
            foreach (var q in questions)
            {
                if (q.Id == id)
                {
                    q.questionStatus = data.status;
                    eventSetting.StartVoting = data.status;
                }
                else { q.questionStatus = false; }
                db.Entry(q).State = EntityState.Modified;
            }
            db.SaveChanges();
            //var question = db.Question.Find(id);
            //question.questionStatus = data.status;
            //db.Entry(question).State = EntityState.Modified;
            //db.SaveChanges();
            //ViewBag.count = data.count;
            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;

            // return Json(question, JsonRequestBehavior.AllowGet);
            //return PartialView(question);
            return Ok(question);
        }
        //
        // GET: /Vote/Delete/5

        public ActionResult Delete(int id)
        {
            var question = db.Question.Find(id);
            // return PartialView(question);
            return Ok(question);
        }

        //
        // POST: /Vote/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, Question collection)
        {
            try
            {
                Question question = db.Question.Find(id);
                foreach (var item in question.result.ToList())
                {
                    db.Result.Remove(item);

                }
                foreach (var smsitem in question.SMSdeliveryLog.ToList())
                {
                    db.SMSDeliveryLog.Remove(smsitem);

                }

                foreach (var smsitem in question.SMSResult.ToList())
                {
                    question.SMSResult.Remove(smsitem);

                }

                db.Question.Remove(question);
                db.SaveChanges();
                return RedirectToAction("Index", "Vote");

            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Vote");
            }
        }


        public string DeleteResolution(int id)
        {
            try
            {
                Question question = db.Question.Find(id);
                foreach (var item in question.result.ToList())
                {
                    db.Result.Remove(item);

                }
                foreach (var smsitem in question.SMSdeliveryLog.ToList())
                {
                    db.SMSDeliveryLog.Remove(smsitem);

                }

                foreach (var smsitem in question.SMSResult.ToList())
                {
                    question.SMSResult.Remove(smsitem);

                }

                db.Question.Remove(question);
                db.SaveChanges();
                return "Success";

            }
            catch (Exception e)
            {
                return "Failed";
            }
        }
    }
}
