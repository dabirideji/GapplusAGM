using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using BarcodeGenerator.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BarcodeGenerator.Controllers
{
    public class ResultController : Controller
    {
        //
        // GET: /Result/

        UsersContext db = new UsersContext();
        UserAdmin ua = new UserAdmin();

        private static string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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

        public ActionResult Index()
        {
            return View();
        }

        //



        // GET: /Result/Details/5



        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Result/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Result/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Result/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Result/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public async Task<ActionResult> SyncVotes(int id)
        {
            var response = await SyncVotesAsync(id);
            return PartialView(response);
        }


        private Task<Question> SyncVotesAsync(int id)
        {
            ViewBag.syncOption = new SelectList(new[] { "FOR", "AGAINST", "ABSTAIN","VOID"});
            var question = db.Question.Find(id);
            return Task.FromResult<Question>(question);
        }

        [HttpPost, ActionName("SyncVotes")]
        [ValidateAntiForgeryToken]
        public async Task<string> ConfirmSyncVotes(PostValue form)
        {
            var response = await ConfirmSyncVotesAsync(form);

            return response;
        }


        private Task<string> ConfirmSyncVotesAsync(PostValue form)
        {

            if(!ModelState.IsValid)
            {
                return Task.FromResult<string>("Empty");
            }
            //var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var currentYear = DateTime.Now.Year.ToString();


            var yetToVote = db.Present.Where(p =>p.AGMID == UniqueAGMId &&  p.TakePoll == false && p.PermitPoll==1).ToList();
            if(yetToVote==null)
            {
                return Task.FromResult<string>("Empty");
            }
            //Check Synced Votes

            //var CheckSync = db.Result.Any(v => v.AGMID == UniqueAGMId && v.VoteStatus == "Synced" && v.QuestionId==form.Id);
            //if (CheckSync)
            //{
            //    return Task.FromResult<string>("Votes May have been synced. Unsync and retry.");
            //}
            var resolution = db.Question.Find(form.Id);
            if(resolution==null)
            {
                return Task.FromResult<string>("Empty");
            }
            //var result = db.Result.Where(r=>r.QuestionId==id).ToList();

            //if (resolution.syncStatus != true)
            //{
            int i = 1;
                foreach (var item in yetToVote)
                {
                    
                    var checkresult = db.Result.Any(r =>r.ShareholderNum == item.ShareholderNum && r.QuestionId == form.Id);
                    if(!checkresult)
                    {
                    
                    Result result = new Result();
                    result.Name = item.Name;
                    result.Address = item.Address;
                    result.Company = item.Company;
                    result.Year = currentYear;
                    result.AGMID = UniqueAGMId;
                    result.date = DateTime.Now;
                    result.Holding = item.Holding;
                    result.ShareholderNum = item.ShareholderNum;
                    result.phonenumber = item.PhoneNumber;
                    result.PercentageHolding = item.PercentageHolding;
                    result.Present = true;
                    result.QuestionId = form.Id;
                    result.VoteStatus = "Synced";
                    if(form.value == "FOR")
                    { result.VoteFor = true;
                        result.VoteAgainst = false;
                        result.VoteAbstain = false;
                        result.VoteVoid = false;
                    }
                    else if (form.value == "AGAINST")
                    {
                        result.VoteAgainst = true;
                        result.VoteFor = false;
                        result.VoteAbstain = false;
                        result.VoteVoid = false;
                    }
                    else if (form.value == "ABSTAIN")
                    {
                        result.VoteAbstain = true;
                        result.VoteAgainst = false;
                        result.VoteAgainst = false;
                        result.VoteVoid = false;
                    }
                    else if (form.value == "VOID")
                    {
                        result.VoteAbstain = false;
                        result.VoteAgainst = false;
                        result.VoteAgainst = false;
                        result.VoteVoid = true;
                    }

                    db.Result.Add(result);
                    //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
                    Functions.SendProgress("Process in progress...", i, yetToVote.Count());
                    i++;
                 //resolution.syncStatus = true;
                //db.Entry(resolution).State = EntityState.Modified;
              
                    //item.TakePoll = true;
                    //db.Entry(item).State = EntityState.Modified;
                  
                    }
                    //else
                    //{
                    //      checkresult.date = DateTime.Now;
                    //        //checkresult.Yes = "Yes";
                    //        db.Entry(checkresult).State = EntityState.Modified;
                    //        //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
                    //        Functions.SendProgress("Process in progress...", i, yetToVote.Count());
                    //        i++;
                       
                    //}

                }

                 try
                 {
                resolution.syncStatus = true;
                db.Entry(resolution).State = EntityState.Modified;
                db.SaveChanges();
                    return Task.FromResult<string>("Success");
                 }
                 catch (DbUpdateConcurrencyException e)
                 {
                return Task.FromResult<string>("Something went Wrong! " +e.StackTrace);
                 }
                
            }


        public async Task<ActionResult> UnSyncVotes(int id)
        {
            var response = await UnSyncVotesAsync(id);

            return PartialView(response);
        }

        private Task<Question> UnSyncVotesAsync(int id)
        {
            var question = db.Question.Find(id);
            return Task.FromResult<Question>(question);
        }


        [HttpPost, ActionName("UnSyncVotes")]
        [ValidateAntiForgeryToken]
        public async Task<string> ConfirmUnSyncVotes(int id)
        {
            var response = await ConfirmUnSyncVotesAsync(id);

            return response;
        }


        private Task<string> ConfirmUnSyncVotesAsync(int id)
        {
            //var companyinfo = ua.GetUserCompanyInfo();
            var currentYear = DateTime.Now.Year.ToString();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            //var yetToVote = db.Present.Where(p => p.AGMID== UniqueAGMId &&  p.TakePoll == false && p.PermitPoll==1).ToList();


            //check Sync status
            //var CheckSync = db.Result.Any(v =>v.QuestionId==id && v.VoteStatus == "Synced");
            //if (!CheckSync)
            //{
            //    return Task.FromResult<string>("Votes may have been Unsynced. Synchronize.");
            //}

            var resolution = db.Question.Find(id);


            var syncVoters = db.Result.Where(r => r.QuestionId == id && r.VoteStatus == "Synced").ToList();
          
            if(resolution==null)
            {
                return Task.FromResult<string>("Empty");
            }
            //var result = db.Result.Where(r=>r.QuestionId==id).ToList();
            if (syncVoters.Any())
            {
                db.Result.RemoveRange(syncVoters);
            }

            string UpdatePresentShareholdersquery = "UPDATE PresentModels SET [TakePoll] = 0 Where AGMID='" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1";
            conn.Open();
            SqlCommand cmd = new SqlCommand(UpdatePresentShareholdersquery, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            //if (resolution.syncStatus != true)
            //{
            //int i = 1;
            //foreach (var item in syncVoters)
            //{

            //    var checkresult = db.Result.FirstOrDefault(r =>r.ShareholderNum == item.ShareholderNum && r.QuestionId == id && r.VoteStatus== "Synced");
            //    if (checkresult != null)
            //    {
            //        db.Result.Remove(checkresult);
            //    }

            //    //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
            //    Functions.SendProgress("Process in progress...", i, syncVoters.Count());
            //    i++;
            //}

            try
            {
                resolution.syncStatus = false;
                db.Entry(resolution).State = EntityState.Modified;
                db.SaveChanges();
                return Task.FromResult<string>("Success");
            }
            catch (DbUpdateConcurrencyException e)
            {
                return Task.FromResult<string>("Something went Wrong! " + e.StackTrace);
            }

        }

        //public ActionResult ResetVoters()
        //{
        //   // var question = db.Question.Find(id);
        //    return PartialView();
        //}

        //[HttpPost]
        //public string ConfirmResetVoters()
        //{
        //    var PresentToVote = db.Present.ToList();
        //    //var resolution = db.Question.Find(id);
        //    //var result = db.Result.Where(r=>r.QuestionId==id).ToList();

        //    //if (resolution.syncStatus != true)
        //    //{
        //    int i = 1;
        //    foreach (var item in PresentToVote)
        //    {

        //        item.TakePoll = false;

        //        //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
        //        Functions.SendProgress("Process in progress...", i, PresentToVote.Count());
        //        i++;
        //    }

        //    try
        //    {
        //        db.SaveChanges();
        //        return "Success";
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return "Something went Wrong!";
        //    }

        //}

        public ActionResult Delete()
        {
            //var question = db.Question.Find(id);
            return PartialView();
        }
        //
        // POST: /Result/Delete/5

        [HttpPost]
        public async Task<ActionResult> Delete(FormCollection collection)
        {
            var response = await DeleteAsync(collection);

            return RedirectToAction("Index", "Vote");
        }


        private Task<string> DeleteAsync(FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                //var resultDelete = db.Result.Where(d=>d.QuestionId==id).ToArray();
                //var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID();
                var currentYear = DateTime.Now.Year.ToString();
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                SqlConnection conn =
                        new SqlConnection(connStr);
                //var resultDelete = db.Result.ToArray();
                //for (int i = 0; i < resultDelete.Length; i++)
                //{

                //    db.Result.Remove(resultDelete[i]);

                //}
                    string query1 = "TRUNCATE TABLE Results";
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.ExecuteNonQuery();
                    conn.Close();

                //var resolution = db.Question.Find(id);
                var resolution = db.Question.Where(q=>q.AGMID==UniqueAGMId).ToArray();
                for (int j = 0; j < resolution.Length; j++)
                {

                    resolution[j].syncStatus = false;
                    db.Entry(resolution[j]).State = EntityState.Modified;
                }
                
                var present = db.Present.Where(p=> p.AGMID==UniqueAGMId && p.TakePoll==true).ToArray();
                for (int k = 0; k < present.Length; k++)
                {

                    present[k].TakePoll = false;
                    db.Entry(present[k]).State = EntityState.Modified;

                }
                ViewBag.Message = "Cleared Results Successfully";
                db.SaveChanges();
                return Task.FromResult<string>("Cleared Results Successfully");
            }
            catch
            {
                ViewBag.Message = "Couldn't Delete Results";
                return Task.FromResult<string>("Couldn't Delete Results");
            }
            //ViewBag.Message = "Bad Request";
            //return RedirectToAction("Index","Vote");
        }


        public async Task<string> TruncateResult()
        {
            var response = await TruncateResultAsync();

            return response;
        }


        private Task<string> TruncateResultAsync()
        {
            try
            {
                // TODO: Add delete logic here
                //var resultDelete = db.Result.Where(d=>d.QuestionId==id).ToArray();
                //var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID();
                var currentYear = DateTime.Now.Year.ToString();
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                SqlConnection conn =
                        new SqlConnection(connStr);
                //var resultDelete = db.Result.ToArray();
                //for (int i = 0; i < resultDelete.Length; i++)
                //{

                //    db.Result.Remove(resultDelete[i]);

                //}
                string query1 = "TRUNCATE TABLE Results";
                conn.Open();
                SqlCommand cmd1 = new SqlCommand(query1, conn);
                cmd1.ExecuteNonQuery();
                conn.Close();

                //var resolution = db.Question.Find(id);
                var resolution = db.Question.Where(q => q.AGMID == UniqueAGMId).ToArray();
                for (int j = 0; j < resolution.Length; j++)
                {

                    resolution[j].syncStatus = false;
                    db.Entry(resolution[j]).State = EntityState.Modified;
                }

                var present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.TakePoll == true).ToArray();
                for (int k = 0; k < present.Length; k++)
                {

                    present[k].TakePoll = false;
                    db.Entry(present[k]).State = EntityState.Modified;

                }
                ViewBag.Message = "Cleared Results Successfully";
                db.SaveChanges();
                return Task.FromResult<string>("Success");
            }
            catch
            {
                ViewBag.Message = "Couldn't Delete Results";
                return Task.FromResult<string>("Failed");
            }

        }

    
    }
}
