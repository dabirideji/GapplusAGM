using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using BarcodeGenerator.Util;
using Gapplus.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Controllers
{
    // [Authorize]
    public class AGMRealtimeReportController : ControllerBase
    {



public AGMRealtimeReportController(UsersContext context)
{
    db=context;
    ua=new UserAdmin(db);
}
        UsersContext db;
        UserAdmin ua;
        // GET: Dashboard
        private static string currentYear = DateTime.Now.Year.ToString();

        private static string connStr = DatabaseManager.GetConnectionString();
        SqlConnection conn =
                  new SqlConnection(connStr);
        // GET: AGMRealtimeReport

        public async Task<ActionResult> Index(int id)
        {
            var response = await IndexAsync(id);

            // return View(response);
            return Ok(response);
        }

        public async Task<ActionResult> RealTimeIndex(int id)
        {
            var response = await IndexAsync(id);

            // return View(response);
            return Ok(response);
        }

        public Task<RealtimeDto> IndexAsync(int id)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            RealtimeDto model = new RealtimeDto();

            int TotalCountFor = 0;
            int TotalCountAgainst = 0;
            int TotalCountAbstain = 0;
            int TotalCountVoid = 0;
            Double TotalHoldingFor = 0;
            Double TotalHoldingAgainst = 0;
            Double TotalHoldingAbstain = 0;
            Double TotalHoldingVoid = 0;


            if (UniqueAGMId != -1)
            {
                var abstainbtnchoice = true;
                var forBg = "green";
                var againstBg = "red";
                var abstainBg = "blue";
                var voidBg = "black";
                var eventSetting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
                if (eventSetting != null)
                {
                    if (!string.IsNullOrEmpty(eventSetting.VoteForColorBg))
                    {
                        forBg = eventSetting.VoteForColorBg;
                    }
                    if (!string.IsNullOrEmpty(eventSetting.VoteAgainstColorBg))
                    {
                        againstBg = eventSetting.VoteAgainstColorBg;
                    }
                    if (!string.IsNullOrEmpty(eventSetting.VoteAbstaincolorBg))
                    {
                        abstainBg = eventSetting.VoteAbstaincolorBg;
                    }
                    if (!string.IsNullOrEmpty(eventSetting.VoteVoidColorBg))
                    {
                        voidBg = eventSetting.VoteVoidColorBg;
                    }
                    if (eventSetting.AbstainBtnChoice != null)
                    {
                        abstainbtnchoice = (bool)eventSetting.AbstainBtnChoice;
                    }
                    model.allChannels = eventSetting.allChannels;
                    model.mobileChannel = eventSetting.mobileChannel;
                    model.smsChannel = eventSetting.smsChannel;
                    model.webChannel = eventSetting.webChannel;
                    model.agmid = eventSetting.AGMID;
                    model.abstainBtnChoice = abstainbtnchoice;
                    model.syncChoiceVoid = eventSetting.SyncChoice;
                    model.forBg = forBg;
                    model.againstBg = againstBg;
                    model.abstainBg = abstainBg;
                    model.voidBg = voidBg;
                    model.CompanyName = eventSetting.CompanyName;
                    model.Companylogo = eventSetting.Image != null ? "data:image/jpg;base64," +
 Convert.ToBase64String((byte[])eventSetting.Image) : "";
                }
                string UpdatePresentShareholdersquery = "UPDATE PresentModels SET [TakePoll] = 0 Where AGMID='" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1";
                conn.Open();
                SqlCommand cmdUpdate = new SqlCommand(UpdatePresentShareholdersquery, conn);
                cmdUpdate.ExecuteNonQuery();
                conn.Close();

                var questions = db.Question.Where(q => q.AGMID == UniqueAGMId).ToArray();
                var count = 0;
                foreach (var q in questions)
                {
                    if (q.Id == id)
                    {
                        q.questionStatus = true;
                        eventSetting.StartVoting = true;
                        model.ResolutinIndex = count;
                    }
                    else { q.questionStatus = false; }
                    db.Entry(q).State = EntityState.Modified;
                    count++;
                }
                db.SaveChanges();
                model.questions = questions.ToList();
                var question = db.Question.Find(id);
               
                if (question != null)
                {
                    model.Resolution = question.question;
                    model.voteType = question.voteType;

                    if (eventSetting.ProxyVoteResult)
                    {
                        string queryTotalCountFor = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND [VoteFor] = 1";
                        conn.Open();
                        SqlCommand cmdcountFor = new SqlCommand(queryTotalCountFor, conn);
                        //object o = cmd.ExecuteScalar();
                        if (!DBNull.Value.Equals(cmdcountFor.ExecuteScalar()))
                        {
                            TotalCountFor = Convert.ToInt32(cmdcountFor.ExecuteScalar());
                            model.ResultForCount = TotalCountFor;
                        }
                        conn.Close();
                        string queryTotalCountAgainst = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteAgainst = 1";
                        conn.Open();
                        SqlCommand cmdcountAgainst = new SqlCommand(queryTotalCountAgainst, conn);
                        //object o = cmd.ExecuteScalar();
                        if (!DBNull.Value.Equals(cmdcountAgainst.ExecuteScalar()))
                        {
                            TotalCountAgainst = Convert.ToInt32(cmdcountAgainst.ExecuteScalar());
                            model.ResultAgainstCount = TotalCountAgainst;
                        }
                        conn.Close();

                        if (abstainbtnchoice)
                        {
                            string queryTotalCountAbstain = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteAbstain = 1";
                            conn.Open();
                            SqlCommand cmdcountAbstain = new SqlCommand(queryTotalCountAbstain, conn);
                            //object o = cmd.ExecuteScalar();
                            if (!DBNull.Value.Equals(cmdcountAbstain.ExecuteScalar()))
                            {
                                TotalCountAbstain = Convert.ToInt32(cmdcountAbstain.ExecuteScalar());
                                model.ResultAbstainCount = TotalCountAbstain;
                            }
                            conn.Close();

                        }

                        var TotalPresent = db.Present.Where(p => p.AGMID == UniqueAGMId).Count();
                        model.PercentageFor = ((double)TotalCountFor / (double)TotalPresent) * 100;
                        model.PercentageAgainst = ((double)TotalCountAgainst / (double)TotalPresent) * 100;
                        if (abstainbtnchoice)
                        {
                            model.PercentageAbstain = ((double)TotalCountAbstain / (double)TotalPresent) * 100;
                        }


                        string queryTotalholdingFor = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND [VoteFor] = 1 AND QuestionId='" + question.Id + "'";
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(queryTotalholdingFor, conn);
                        //object o = cmd.ExecuteScalar();
                        if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                        {
                            TotalHoldingFor = Convert.ToDouble(cmd.ExecuteScalar());
                            model.TotalHoldingFor = Convert.ToDouble(TotalHoldingFor).ToString("0,0");
                        }
                        conn.Close();
                        string queryTotalholdingAgainst = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND VoteAgainst = 1 AND QuestionId='" + question.Id + "'";
                        conn.Open();
                        SqlCommand cmdagainst = new SqlCommand(queryTotalholdingAgainst, conn);
                        //object o = cmd.ExecuteScalar();
                        if (!DBNull.Value.Equals(cmdagainst.ExecuteScalar()))
                        {
                            TotalHoldingAgainst = Convert.ToDouble(cmdagainst.ExecuteScalar());
                            model.TotalHoldingAgainst = Convert.ToDouble(TotalHoldingAgainst).ToString("0,0");
                        }
                        conn.Close();

                        if (abstainbtnchoice)
                        {
                            string queryTotalholdingAbstain = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND VoteAbstain = 1 AND QuestionId='" + question.Id + "'";
                            conn.Open();
                            SqlCommand cmdabstain = new SqlCommand(queryTotalholdingAbstain, conn);
                            //object o = cmd.ExecuteScalar();
                            if (!DBNull.Value.Equals(cmdabstain.ExecuteScalar()))
                            {
                                TotalHoldingAbstain = Convert.ToDouble(cmdabstain.ExecuteScalar());
                                model.TotalHoldingAbstain = Convert.ToDouble(TotalHoldingAbstain).ToString("0,0");
                            }
                            conn.Close();
                        }
                    }



                }

            }

            return Task.FromResult<RealtimeDto>(model);
        }

        public void FetchAGMQuestionsAsync()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var questionList = db.AGMQuestions.Where(a => a.Company == companyinfo);
        }
        //private Task<IEnumerable<Question>> IndexAsync()
        //{
        //    var response = await ResolutionIndexAsync();

        //    return Task.FromResult<IEnumerable<Question>>(resolutions);
        //}

        public async Task<ActionResult> ResolutionIndex()
        {
            var response = await ResolutionIndexAsync();

            // return PartialView(response);
            return Ok(response);
            //return Json(response, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> CustomRealtimeIndex()
        {
            var response = await CustomResolutionIndexAsync();

            //return PartialView(response);
            // return Json(response, JsonRequestBehavior.AllowGet);
            return Ok(response);
        }


        public Task<RealtimeDto> CustomResolutionIndexAsync()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            RealtimeDto model = new RealtimeDto();

            int TotalCountFor = 0;
            int TotalCountAgainst = 0;
            int TotalCountAbstain = 0;
            int TotalCountVoid = 0;
            Double TotalHoldingFor = 0;
            Double TotalHoldingAgainst = 0;
            Double TotalHoldingAbstain = 0;
            Double TotalHoldingVoid = 0;


            if (UniqueAGMId != -1)
            {
                var abstainbtnchoice = true;
                var eventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

                var forBg = "green";
                var againstBg = "red";
                var abstainBg = "blue";
                var voidBg = "black";
                if (eventSetting != null)
                {
                    if (!string.IsNullOrEmpty(eventSetting.VoteForColorBg))
                    {
                        forBg = eventSetting.VoteForColorBg;
                    }
                    if (!string.IsNullOrEmpty(eventSetting.VoteAgainstColorBg))
                    {
                        againstBg = eventSetting.VoteAgainstColorBg;
                    }
                    if (!string.IsNullOrEmpty(eventSetting.VoteAbstaincolorBg))
                    {
                        abstainBg = eventSetting.VoteAbstaincolorBg;
                    }
                    if (!string.IsNullOrEmpty(eventSetting.VoteVoidColorBg))
                    {
                        voidBg = eventSetting.VoteVoidColorBg;
                    }
                    if (eventSetting.AbstainBtnChoice != null)
                    {
                        abstainbtnchoice = (bool)eventSetting.AbstainBtnChoice;
                    }
                    model.allChannels = eventSetting.allChannels;
                    model.mobileChannel = eventSetting.mobileChannel;
                    model.smsChannel = eventSetting.smsChannel;
                    model.webChannel = eventSetting.webChannel;
                    model.agmid = eventSetting.AGMID;
                    model.abstainBtnChoice = abstainbtnchoice;
                    model.syncChoiceVoid = eventSetting.SyncChoice;
                    model.forBg = forBg;
                    model.againstBg = againstBg;
                    model.abstainBg = abstainBg;
                    model.voidBg = voidBg;
                    model.CompanyName = eventSetting.CompanyName;
                    model.Companylogo = eventSetting.Image != null ? "data:image/jpg;base64," +
                    Convert.ToBase64String((byte[])eventSetting.Image) : "";
                }

                var question = db.Question.SingleOrDefault(q => q.AGMID == UniqueAGMId && q.questionStatus == true);
                if (question != null)
                {
                    model.Resolution = question.question;
                    model.ResolutionId = question.Id;
                    model.voteType = question.voteType;

                    //Display Proxy votes after general votes....
                    //if (!eventSetting.ProxyVoteResult)
                    //{
                     
                        //if (!TimerControll.GetTimeStatus(eventSetting.CompanyName))
                        //{
                            string queryTotalCountFor = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND [VoteFor] = 1";
                            conn.Open();
                            SqlCommand cmdcountFor = new SqlCommand(queryTotalCountFor, conn);
                            //object o = cmd.ExecuteScalar();
                            if (!DBNull.Value.Equals(cmdcountFor.ExecuteScalar()))
                            {
                                TotalCountFor = Convert.ToInt32(cmdcountFor.ExecuteScalar());
                                model.ResultForCount = TotalCountFor;
                            }
                            conn.Close();
                            string queryTotalCountAgainst = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteAgainst = 1";
                            conn.Open();
                            SqlCommand cmdcountAgainst = new SqlCommand(queryTotalCountAgainst, conn);
                            //object o = cmd.ExecuteScalar();
                            if (!DBNull.Value.Equals(cmdcountAgainst.ExecuteScalar()))
                            {
                                TotalCountAgainst = Convert.ToInt32(cmdcountAgainst.ExecuteScalar());
                                model.ResultAgainstCount = TotalCountAgainst;
                            }
                            conn.Close();

                            if (abstainbtnchoice)
                            {
                                string queryTotalCountAbstain = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteAbstain = 1";
                                conn.Open();
                                SqlCommand cmdcountAbstain = new SqlCommand(queryTotalCountAbstain, conn);
                                //object o = cmd.ExecuteScalar();
                                if (!DBNull.Value.Equals(cmdcountAbstain.ExecuteScalar()))
                                {
                                    TotalCountAbstain = Convert.ToInt32(cmdcountAbstain.ExecuteScalar());
                                    model.ResultAbstainCount = TotalCountAbstain;
                                }
                                conn.Close();

                            }
                            //string queryTotalCountVoid = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteVoid = 1";
                            //conn.Open();
                            //SqlCommand cmdcountVoid = new SqlCommand(queryTotalCountVoid, conn);
                            ////object o = cmd.ExecuteScalar();
                            //if (!DBNull.Value.Equals(cmdcountVoid.ExecuteScalar()))
                            //{
                            //    TotalCountVoid = Convert.ToInt32(cmdcountVoid.ExecuteScalar());
                            //    model.ResultVoidCount = TotalCountVoid;
                            //}
                            //conn.Close();

                            //var TotalPresent = db.Present.Where(p => p.AGMID == UniqueAGMId).Count();


                            //model.PercentageVoid = ((double)TotalCountVoid / (double)TotalPresent) * 100;

                            string queryTotalholdingFor = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND [VoteFor] = 1 AND QuestionId='" + question.Id + "'";
                            conn.Open();
                            SqlCommand cmd = new SqlCommand(queryTotalholdingFor, conn);
                            //object o = cmd.ExecuteScalar();
                            if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                            {
                                TotalHoldingFor = Convert.ToDouble(cmd.ExecuteScalar());
                                model.TotalHoldingFor = TotalHoldingFor > 0 ? TotalHoldingFor.ToString("0,0") : "0";
                            }
                            conn.Close();
                            string queryTotalholdingAgainst = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND VoteAgainst = 1 AND QuestionId='" + question.Id + "'";
                            conn.Open();
                            SqlCommand cmdagainst = new SqlCommand(queryTotalholdingAgainst, conn);
                            //object o = cmd.ExecuteScalar();
                            if (!DBNull.Value.Equals(cmdagainst.ExecuteScalar()))
                            {
                                TotalHoldingAgainst = Convert.ToDouble(cmdagainst.ExecuteScalar());
                                model.TotalHoldingAgainst = TotalHoldingAgainst > 0 ? TotalHoldingAgainst.ToString("0,0") : "0";
                            }
                            conn.Close();

                            if (abstainbtnchoice)
                            {
                                string queryTotalholdingAbstain = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND VoteAbstain = 1 AND QuestionId='" + question.Id + "'";
                                conn.Open();
                                SqlCommand cmdabstain = new SqlCommand(queryTotalholdingAbstain, conn);
                                //object o = cmd.ExecuteScalar();
                                if (!DBNull.Value.Equals(cmdabstain.ExecuteScalar()))
                                {
                                    TotalHoldingAbstain = Convert.ToDouble(cmdabstain.ExecuteScalar());
                                    model.TotalHoldingAbstain = TotalHoldingAbstain > 0 ? TotalHoldingAbstain.ToString("0,0") : "0";
                                }
                                conn.Close();
                            }
                        //}
                        //else
                        //{
                        //    string queryTotalCountFor = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND [VoteFor] = 1 AND Source != 'Proxy'";
                        //    conn.Open();
                        //    SqlCommand cmdcountFor = new SqlCommand(queryTotalCountFor, conn);
                        //    //object o = cmd.ExecuteScalar();
                        //    if (!DBNull.Value.Equals(cmdcountFor.ExecuteScalar()))
                        //    {
                        //        TotalCountFor = Convert.ToInt32(cmdcountFor.ExecuteScalar());
                        //        model.ResultForCount = TotalCountFor;
                        //    }
                        //    conn.Close();
                        //    string queryTotalCountAgainst = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteAgainst = 1 AND Source != 'Proxy'";
                        //    conn.Open();
                        //    SqlCommand cmdcountAgainst = new SqlCommand(queryTotalCountAgainst, conn);
                        //    //object o = cmd.ExecuteScalar();
                        //    if (!DBNull.Value.Equals(cmdcountAgainst.ExecuteScalar()))
                        //    {
                        //        TotalCountAgainst = Convert.ToInt32(cmdcountAgainst.ExecuteScalar());
                        //        model.ResultAgainstCount = TotalCountAgainst;
                        //    }
                        //    conn.Close();

                        //    if (abstainbtnchoice)
                        //    {
                        //        string queryTotalCountAbstain = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteAbstain = 1 AND Source != 'Proxy'";
                        //        conn.Open();
                        //        SqlCommand cmdcountAbstain = new SqlCommand(queryTotalCountAbstain, conn);
                        //        //object o = cmd.ExecuteScalar();
                        //        if (!DBNull.Value.Equals(cmdcountAbstain.ExecuteScalar()))
                        //        {
                        //            TotalCountAbstain = Convert.ToInt32(cmdcountAbstain.ExecuteScalar());
                        //            model.ResultAbstainCount = TotalCountAbstain;
                        //        }
                        //        conn.Close();

                        //    }
                        //    //string queryTotalCountVoid = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteVoid = 1";
                        //    //conn.Open();
                        //    //SqlCommand cmdcountVoid = new SqlCommand(queryTotalCountVoid, conn);
                        //    ////object o = cmd.ExecuteScalar();
                        //    //if (!DBNull.Value.Equals(cmdcountVoid.ExecuteScalar()))
                        //    //{
                        //    //    TotalCountVoid = Convert.ToInt32(cmdcountVoid.ExecuteScalar());
                        //    //    model.ResultVoidCount = TotalCountVoid;
                        //    //}
                        //    //conn.Close();

                        //    //var TotalPresent = db.Present.Where(p => p.AGMID == UniqueAGMId).Count();


                        //    //model.PercentageVoid = ((double)TotalCountVoid / (double)TotalPresent) * 100;

                        //    string queryTotalholdingFor = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND [VoteFor] = 1 AND QuestionId='" + question.Id + "' AND Source != 'Proxy'";
                        //    conn.Open();
                        //    SqlCommand cmd = new SqlCommand(queryTotalholdingFor, conn);
                        //    //object o = cmd.ExecuteScalar();
                        //    if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                        //    {
                        //        TotalHoldingFor = Convert.ToDouble(cmd.ExecuteScalar());
                        //        model.TotalHoldingFor = TotalHoldingFor > 0 ? TotalHoldingFor.ToString("0,0") : "0";
                        //    }
                        //    conn.Close();
                        //    string queryTotalholdingAgainst = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND VoteAgainst = 1 AND QuestionId='" + question.Id + "' AND Source != 'Proxy'";
                        //    conn.Open();
                        //    SqlCommand cmdagainst = new SqlCommand(queryTotalholdingAgainst, conn);
                        //    //object o = cmd.ExecuteScalar();
                        //    if (!DBNull.Value.Equals(cmdagainst.ExecuteScalar()))
                        //    {
                        //        TotalHoldingAgainst = Convert.ToDouble(cmdagainst.ExecuteScalar());
                        //        model.TotalHoldingAgainst = TotalHoldingAgainst > 0 ? TotalHoldingAgainst.ToString("0,0") : "0";
                        //    }
                        //    conn.Close();

                        //    if (abstainbtnchoice)
                        //    {
                        //        string queryTotalholdingAbstain = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND VoteAbstain = 1 AND QuestionId='" + question.Id + "' AND Source != 'Proxy'";
                        //        conn.Open();
                        //        SqlCommand cmdabstain = new SqlCommand(queryTotalholdingAbstain, conn);
                        //        //object o = cmd.ExecuteScalar();
                        //        if (!DBNull.Value.Equals(cmdabstain.ExecuteScalar()))
                        //        {
                        //            TotalHoldingAbstain = Convert.ToDouble(cmdabstain.ExecuteScalar());
                        //            model.TotalHoldingAbstain = TotalHoldingAbstain > 0 ? TotalHoldingAbstain.ToString("0,0") : "0";
                        //        }
                        //        conn.Close();
                        //    }
                        //}
                    //}
                    //else
                    //{
                    //    //Display Proxy votes before general votes....

                    //    string queryTotalCountFor = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND [VoteFor] = 1";
                    //    conn.Open();
                    //    SqlCommand cmdcountFor = new SqlCommand(queryTotalCountFor, conn);
                    //    //object o = cmd.ExecuteScalar();
                    //    if (!DBNull.Value.Equals(cmdcountFor.ExecuteScalar()))
                    //    {
                    //        TotalCountFor = Convert.ToInt32(cmdcountFor.ExecuteScalar());
                    //        model.ResultForCount = TotalCountFor;
                    //    }
                    //    conn.Close();
                    //    string queryTotalCountAgainst = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteAgainst = 1";
                    //    conn.Open();
                    //    SqlCommand cmdcountAgainst = new SqlCommand(queryTotalCountAgainst, conn);
                    //    //object o = cmd.ExecuteScalar();
                    //    if (!DBNull.Value.Equals(cmdcountAgainst.ExecuteScalar()))
                    //    {
                    //        TotalCountAgainst = Convert.ToInt32(cmdcountAgainst.ExecuteScalar());
                    //        model.ResultAgainstCount = TotalCountAgainst;
                    //    }
                    //    conn.Close();

                    //    if (abstainbtnchoice)
                    //    {
                    //        string queryTotalCountAbstain = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteAbstain = 1";
                    //        conn.Open();
                    //        SqlCommand cmdcountAbstain = new SqlCommand(queryTotalCountAbstain, conn);
                    //        //object o = cmd.ExecuteScalar();
                    //        if (!DBNull.Value.Equals(cmdcountAbstain.ExecuteScalar()))
                    //        {
                    //            TotalCountAbstain = Convert.ToInt32(cmdcountAbstain.ExecuteScalar());
                    //            model.ResultAbstainCount = TotalCountAbstain;
                    //        }
                    //        conn.Close();

                    //    }
                    //    //string queryTotalCountVoid = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteVoid = 1";
                    //    //conn.Open();
                    //    //SqlCommand cmdcountVoid = new SqlCommand(queryTotalCountVoid, conn);
                    //    ////object o = cmd.ExecuteScalar();
                    //    //if (!DBNull.Value.Equals(cmdcountVoid.ExecuteScalar()))
                    //    //{
                    //    //    TotalCountVoid = Convert.ToInt32(cmdcountVoid.ExecuteScalar());
                    //    //    model.ResultVoidCount = TotalCountVoid;
                    //    //}
                    //    //conn.Close();

                    //    //var TotalPresent = db.Present.Where(p => p.AGMID == UniqueAGMId).Count();


                    //    //model.PercentageVoid = ((double)TotalCountVoid / (double)TotalPresent) * 100;

                    //    string queryTotalholdingFor = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND [VoteFor] = 1 AND QuestionId='" + question.Id + "'";
                    //    conn.Open();
                    //    SqlCommand cmd = new SqlCommand(queryTotalholdingFor, conn);
                    //    //object o = cmd.ExecuteScalar();
                    //    if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                    //    {
                    //        TotalHoldingFor = Convert.ToDouble(cmd.ExecuteScalar());
                    //        model.TotalHoldingFor = TotalHoldingFor > 0 ? TotalHoldingFor.ToString("0,0") : "0";
                    //    }
                    //    conn.Close();
                    //    string queryTotalholdingAgainst = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND VoteAgainst = 1 AND QuestionId='" + question.Id + "'";
                    //    conn.Open();
                    //    SqlCommand cmdagainst = new SqlCommand(queryTotalholdingAgainst, conn);
                    //    //object o = cmd.ExecuteScalar();
                    //    if (!DBNull.Value.Equals(cmdagainst.ExecuteScalar()))
                    //    {
                    //        TotalHoldingAgainst = Convert.ToDouble(cmdagainst.ExecuteScalar());
                    //        model.TotalHoldingAgainst = TotalHoldingAgainst > 0 ? TotalHoldingAgainst.ToString("0,0") : "0";
                    //    }
                    //    conn.Close();

                    //    if (abstainbtnchoice)
                    //    {
                    //        string queryTotalholdingAbstain = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND VoteAbstain = 1 AND QuestionId='" + question.Id + "'";
                    //        conn.Open();
                    //        SqlCommand cmdabstain = new SqlCommand(queryTotalholdingAbstain, conn);
                    //        //object o = cmd.ExecuteScalar();
                    //        if (!DBNull.Value.Equals(cmdabstain.ExecuteScalar()))
                    //        {
                    //            TotalHoldingAbstain = Convert.ToDouble(cmdabstain.ExecuteScalar());
                    //            model.TotalHoldingAbstain = TotalHoldingAbstain > 0 ? TotalHoldingAbstain.ToString("0,0") : "0";
                    //        }
                    //        conn.Close();
                    //    }
                    //}

                   


                   



                    int TotalPresent = TotalCountFor + TotalCountAgainst;
                    if (abstainbtnchoice)
                    {
                        TotalPresent = TotalCountFor + TotalCountAgainst + TotalCountAbstain;
                    }
                    else
                    {
                        TotalPresent = TotalCountFor + TotalCountAgainst;
                    }

                    if (model.voteType != "Poll")
                    {
                        model.PercentageFor = ((double)TotalCountFor / (double)TotalPresent) * 100;
                        model.PercentageAgainst = ((double)TotalCountAgainst / (double)TotalPresent) * 100;
                        model.PercentageForValue = model.PercentageFor > 0 ? Math.Round(model.PercentageFor, 2).ToString() + "%" : "0.00%";
                        model.PercentageAgainstValue = model.PercentageAgainst > 0 ? Math.Round(model.PercentageAgainst, 2).ToString() + "%" : "0.00%";
                        if (abstainbtnchoice)
                        {
                            model.PercentageAbstain = ((double)TotalCountAbstain / (double)TotalPresent) * 100;
                            model.PercentageAbstainValue = model.PercentageAbstain > 0 ? Math.Round(model.PercentageAbstain, 2).ToString() + "%" : "0.00%";
                        }
                    }
                    double TotalHolding;
                    if (abstainbtnchoice)
                    {
                        model.TotalCount = TotalPresent;
                        TotalHolding = TotalHoldingAgainst + TotalHoldingFor + TotalHoldingAbstain;
                        model.TotalHolding = TotalHolding > 0 ? TotalHolding.ToString("0,0") : "0";
                        var TotalPercentage = model.PercentageFor + model.PercentageAgainst + model.PercentageAbstain;
                        model.TotalPercentage = TotalPercentage;
                        model.TotalPercentageValue = Math.Round(TotalPercentage, 2).ToString() + "%";
                    }
                    else
                    {
                        model.TotalCount = TotalPresent;
                        TotalHolding = TotalHoldingAgainst + TotalHoldingFor;
                        model.TotalHolding = TotalHolding > 0 ? TotalHolding.ToString("0,0") : "0";
                        var TotalPercentage = model.PercentageFor + model.PercentageAgainst;
                        model.TotalPercentage = TotalPercentage;
                        model.TotalPercentageValue = Math.Round(TotalPercentage, 2).ToString() + "%";
                    }

                    if (model.voteType == "Poll")
                    {
                        model.PercentageFor = (TotalHoldingFor / TotalHolding) * 100;
                        model.PercentageAgainst = (TotalHoldingAgainst / TotalHolding) * 100;
                        model.PercentageForValue = model.PercentageFor > 0 ? Math.Round(model.PercentageFor, 2).ToString() + "%" : "0.00%";
                        model.PercentageAgainstValue = model.PercentageAgainst > 0 ? Math.Round(model.PercentageAgainst, 2).ToString() + "%" : "0.00%";
                        if (abstainbtnchoice)
                        {
                            model.PercentageAbstain = (TotalHoldingAbstain / TotalHolding) * 100;
                            model.PercentageAbstainValue = model.PercentageAbstain > 0 ? Math.Round(model.PercentageAbstain, 2).ToString() + "%" : "0.00%";
                        }
                    }

                    //string queryTotalholdingVoid = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND VoteVoid = 1 AND QuestionId='" + question.Id + "'";
                    //conn.Open();
                    //SqlCommand cmdVoid = new SqlCommand(queryTotalholdingVoid, conn);
                    ////object o = cmd.ExecuteScalar();
                    //if (!DBNull.Value.Equals(cmdVoid.ExecuteScalar()))
                    //{
                    //    TotalHoldingVoid = Convert.ToDouble(cmdVoid.ExecuteScalar());
                    //    model.TotalHoldingVoid = Convert.ToDouble(TotalHoldingVoid).ToString("0,0");
                    //}
                    //conn.Close();
                }

            }

            return Task.FromResult<RealtimeDto>(model);
        }

        public Task<RealtimeDto> ResolutionIndexAsync()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            RealtimeDto model = new RealtimeDto();

            int TotalCountFor = 0;
            int TotalCountAgainst = 0;
            int TotalCountAbstain = 0;
            int TotalCountVoid = 0;
            Double TotalHoldingFor = 0;
            Double TotalHoldingAgainst = 0;
            Double TotalHoldingAbstain = 0;
            Double TotalHoldingVoid = 0;


            if (UniqueAGMId != -1)
            {
                var abstainbtnchoice = true;
                var eventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

                var forBg = "green";
                var againstBg = "red";
                var abstainBg = "blue";
                var voidBg = "black";
                if (eventSetting != null)
                {
                    if (!string.IsNullOrEmpty(eventSetting.VoteForColorBg))
                    {
                        forBg = eventSetting.VoteForColorBg;
                    }
                    if (!string.IsNullOrEmpty(eventSetting.VoteAgainstColorBg))
                    {
                        againstBg = eventSetting.VoteAgainstColorBg;
                    }
                    if (!string.IsNullOrEmpty(eventSetting.VoteAbstaincolorBg))
                    {
                        abstainBg = eventSetting.VoteAbstaincolorBg;
                    }
                    if (!string.IsNullOrEmpty(eventSetting.VoteVoidColorBg))
                    {
                        voidBg = eventSetting.VoteVoidColorBg;
                    }
                    if (eventSetting.AbstainBtnChoice != null)
                    {
                        abstainbtnchoice = (bool)eventSetting.AbstainBtnChoice;
                    }
                    model.allChannels = eventSetting.allChannels;
                    model.mobileChannel = eventSetting.mobileChannel;
                    model.smsChannel = eventSetting.smsChannel;
                    model.webChannel = eventSetting.webChannel;
                    model.agmid = eventSetting.AGMID;
                    model.abstainBtnChoice = abstainbtnchoice;
                    model.syncChoiceVoid = eventSetting.SyncChoice;
                    model.forBg = forBg;
                    model.againstBg = againstBg;
                    model.abstainBg = abstainBg;
                    model.voidBg = voidBg;
                    model.CompanyName = eventSetting.CompanyName;
                    model.Companylogo = eventSetting.Image != null ? "data:image/jpg;base64," +
Convert.ToBase64String((byte[])eventSetting.Image) : "";
                }

                var question = db.Question.SingleOrDefault(q => q.AGMID == UniqueAGMId && q.questionStatus == true);
                if (question != null)
                {
                    model.Resolution = question.question;
                    model.voteType = question.voteType;
                    string queryTotalCountFor = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='"+question.Id+"' AND [VoteFor] = 1";
                    conn.Open();
                    SqlCommand cmdcountFor = new SqlCommand(queryTotalCountFor, conn);
                    //object o = cmd.ExecuteScalar();
                    if (!DBNull.Value.Equals(cmdcountFor.ExecuteScalar()))
                    {
                        TotalCountFor = Convert.ToInt32(cmdcountFor.ExecuteScalar());
                        model.ResultForCount = TotalCountFor;
                    }
                    conn.Close();
                    string queryTotalCountAgainst = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteAgainst = 1";
                    conn.Open();
                    SqlCommand cmdcountAgainst = new SqlCommand(queryTotalCountAgainst, conn);
                    //object o = cmd.ExecuteScalar();
                    if (!DBNull.Value.Equals(cmdcountAgainst.ExecuteScalar()))
                    {
                        TotalCountAgainst = Convert.ToInt32(cmdcountAgainst.ExecuteScalar());
                        model.ResultAgainstCount = TotalCountAgainst;
                    }
                    conn.Close();

                    if(abstainbtnchoice)
                    {
                        string queryTotalCountAbstain = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteAbstain = 1";
                        conn.Open();
                        SqlCommand cmdcountAbstain = new SqlCommand(queryTotalCountAbstain, conn);
                        //object o = cmd.ExecuteScalar();
                        if (!DBNull.Value.Equals(cmdcountAbstain.ExecuteScalar()))
                        {
                            TotalCountAbstain = Convert.ToInt32(cmdcountAbstain.ExecuteScalar());
                            model.ResultAbstainCount = TotalCountAbstain;
                        }
                        conn.Close();

                    }
                    //string queryTotalCountVoid = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + question.Id + "' AND VoteVoid = 1";
                    //conn.Open();
                    //SqlCommand cmdcountVoid = new SqlCommand(queryTotalCountVoid, conn);
                    ////object o = cmd.ExecuteScalar();
                    //if (!DBNull.Value.Equals(cmdcountVoid.ExecuteScalar()))
                    //{
                    //    TotalCountVoid = Convert.ToInt32(cmdcountVoid.ExecuteScalar());
                    //    model.ResultVoidCount = TotalCountVoid;
                    //}
                    //conn.Close();

                    var TotalPresent = db.Present.Where(p => p.AGMID == UniqueAGMId).Count();
                    if (model.voteType != "Poll")
                    {
                        model.PercentageFor = ((double)TotalCountFor / (double)TotalPresent) * 100;
                        model.PercentageAgainst = ((double)TotalCountAgainst / (double)TotalPresent) * 100;
                        model.PercentageForValue = model.PercentageFor > 0 ? Math.Round(model.PercentageFor, 2).ToString() + "%" : "0.00%";
                        model.PercentageAgainstValue = model.PercentageAgainst > 0 ? Math.Round(model.PercentageAgainst, 2).ToString() + "%" : "0.00%";
                        if (abstainbtnchoice)
                        {
                            model.PercentageAbstain = ((double)TotalCountAbstain / (double)TotalPresent) * 100;
                            model.PercentageAbstainValue = model.PercentageAbstain > 0 ? Math.Round(model.PercentageAbstain, 2).ToString() + "%" : "0.00%";
                        }
                    }

                    //model.PercentageVoid = ((double)TotalCountVoid / (double)TotalPresent) * 100;

                    string queryTotalholdingFor = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND [VoteFor] = 1 AND QuestionId='" + question.Id + "'";
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(queryTotalholdingFor, conn);
                    //object o = cmd.ExecuteScalar();
                    if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                    {
                        TotalHoldingFor = Convert.ToDouble(cmd.ExecuteScalar());
                        model.TotalHoldingFor = TotalHoldingFor > 0 ? TotalHoldingFor.ToString("0,0"): "0";
                    }
                    conn.Close();
                    string queryTotalholdingAgainst = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND VoteAgainst = 1 AND QuestionId='" + question.Id + "'";
                    conn.Open();
                    SqlCommand cmdagainst = new SqlCommand(queryTotalholdingAgainst, conn);
                    //object o = cmd.ExecuteScalar();
                    if (!DBNull.Value.Equals(cmdagainst.ExecuteScalar()))
                    {
                        TotalHoldingAgainst = Convert.ToDouble(cmdagainst.ExecuteScalar());
                        model.TotalHoldingAgainst = TotalHoldingAgainst > 0 ? TotalHoldingAgainst.ToString("0,0"): "0";
                    }
                    conn.Close();

                    if (abstainbtnchoice)
                    {
                        string queryTotalholdingAbstain = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND VoteAbstain = 1 AND QuestionId='" + question.Id + "'";
                        conn.Open();
                        SqlCommand cmdabstain = new SqlCommand(queryTotalholdingAbstain, conn);
                        //object o = cmd.ExecuteScalar();
                        if (!DBNull.Value.Equals(cmdabstain.ExecuteScalar()))
                        {
                            TotalHoldingAbstain = Convert.ToDouble(cmdabstain.ExecuteScalar());
                            model.TotalHoldingAbstain = TotalHoldingAbstain > 0 ? TotalHoldingAbstain.ToString("0,0"): "0";
                        }
                        conn.Close();
                    }
                    double TotalHolding;

                    if (abstainbtnchoice)
                    {
                        model.TotalCount = TotalCountAgainst + TotalCountFor+ TotalCountAbstain;
                        TotalHolding = TotalHoldingAgainst + TotalHoldingFor + TotalHoldingAbstain;
                        model.TotalHolding = TotalHolding > 0 ? TotalHolding.ToString("0,0"): "0";
                        var TotalPercentage = model.PercentageFor + model.PercentageAgainst + model.PercentageAbstain;
                        model.TotalPercentage = TotalPercentage;
                        model.TotalPercentageValue = Math.Round(TotalPercentage, 2).ToString() + "%";
                    }
                    else
                    {
                        model.TotalCount = TotalCountAgainst + TotalCountFor;
                         TotalHolding = TotalHoldingAgainst + TotalHoldingFor;
                        model.TotalHolding = TotalHolding > 0 ? TotalHolding.ToString("0,0"): "0";
                        var TotalPercentage = model.PercentageFor + model.PercentageAgainst;
                        model.TotalPercentage = TotalPercentage;
                        model.TotalPercentageValue = Math.Round(TotalPercentage, 2).ToString() + "%";
                    }


                    if (model.voteType != "Poll")
                    {
                        model.PercentageFor = (TotalHoldingFor / TotalHolding) * 100;
                        model.PercentageAgainst = (TotalHoldingAgainst/TotalHolding) * 100;
                        model.PercentageForValue = model.PercentageFor > 0 ? Math.Round(model.PercentageFor, 2).ToString() + "%" : "0.00%";
                        model.PercentageAgainstValue = model.PercentageAgainst > 0 ? Math.Round(model.PercentageAgainst, 2).ToString() + "%" : "0.00%";
                        if (abstainbtnchoice)
                        {
                            model.PercentageAbstain = ((double)TotalHoldingAbstain / (double)TotalHolding) * 100;
                            model.PercentageAbstainValue = model.PercentageAbstain > 0 ? Math.Round(model.PercentageAbstain, 2).ToString() + "%" : "0.00%";
                        }
                    }
                    //string queryTotalholdingVoid = "SELECT SUM(Holding) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND VoteVoid = 1 AND QuestionId='" + question.Id + "'";
                    //conn.Open();
                    //SqlCommand cmdVoid = new SqlCommand(queryTotalholdingVoid, conn);
                    ////object o = cmd.ExecuteScalar();
                    //if (!DBNull.Value.Equals(cmdVoid.ExecuteScalar()))
                    //{
                    //    TotalHoldingVoid = Convert.ToDouble(cmdVoid.ExecuteScalar());
                    //    model.TotalHoldingVoid = Convert.ToDouble(TotalHoldingVoid).ToString("0,0");
                    //}
                    //conn.Close();
                }

            }

         return Task.FromResult<RealtimeDto>(model);
        }



      

        public async Task<ActionResult> ResultionResult([FromServices] IViewBagManager _viewBagManager)
        {
            ReportViewModelDto response = new ReportViewModelDto();
            var syncstatus = "";
            VoteController votecontroller = new VoteController(db,_viewBagManager);
            var stopresponse = await votecontroller.StopVote();
            var companyinfo = ua.GetUserCompanyInfo();
            //ViewBag.Message = syncstatus;
            Functions.RealtimeProgress(companyinfo, stopresponse);
            syncstatus = await ConfirmSyncVotesAsync();
            Functions.RealtimeProgress(companyinfo, syncstatus);
            response = await ResolutionResultAsync();

            //if (stopresponse == "Success")
            //{
            //    syncstatus = await ConfirmSyncVotesAsync();
            //}
            //else
            //{
            //    var companyinfo = ua.GetUserCompanyInfo();
            //    //ViewBag.Message = syncstatus;
            //    Functions.RealtimeProgress(companyinfo, stopresponse);
            //}
            
            //if(syncstatus == "Success" || )
            //{
            //    response = await ResolutionResultAsync();
            //}
            //else
            //{
            //    var companyinfo = ua.GetUserCompanyInfo();
            //    //ViewBag.Message = syncstatus;
            //    Functions.RealtimeProgress(companyinfo, syncstatus);
            //}

            // return PartialView(response);
            return Ok(response);
        }



        public Task<ReportViewModelDto> ResolutionResultAsync()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            ReportViewModelDto model = new ReportViewModelDto();
            Double holding = 0;
            Double percentHolding = 0;
            Double holdingAgainst = 0;
            Double percentHoldingAgainst = 0;
            Double holdingAbstain = 0;
            Double percentHoldingAbstain = 0;
            Double holdingVoid = 0;
            Double percentHoldingVoid = 0;
            Double TotalHolding = 0;
            Double TotalPercentHolding = 0;
            int TotalCountFor = 0;
            int TotalCountAgainst = 0;
            int TotalCountAbstain = 0;
            int TotalCountVoid = 0;


            if (UniqueAGMId != -1)
            {
                var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId); 

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
                model.abstainBtnChoice = abstainbtnchoice;
                model.AGMID = UniqueAGMId;
                model.forBg = forBg;
                model.againstBg = againstBg;
                model.abstainBg = abstainBg;
                model.voidBg = voidBg;
                var resolution = db.Question.SingleOrDefault(q => q.questionStatus == true && q.AGMID == UniqueAGMId);
                model.Id = resolution.Id;
                model.resolutionName = resolution.question;
   

                string queryTotalCountFor = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + resolution.Id + "' AND [VoteFor] = 1";
                conn.Open();
                SqlCommand cmdcountFor = new SqlCommand(queryTotalCountFor, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmdcountFor.ExecuteScalar()))
                {
                    TotalCountFor = Convert.ToInt32(cmdcountFor.ExecuteScalar());
                    model.ResultForCount = TotalCountFor;
                }
                conn.Close();
                string queryTotalCountAgainst = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + resolution.Id + "' AND VoteAgainst = 1";
                conn.Open();
                SqlCommand cmdcountAgainst = new SqlCommand(queryTotalCountAgainst, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmdcountAgainst.ExecuteScalar()))
                {
                    TotalCountAgainst = Convert.ToInt32(cmdcountAgainst.ExecuteScalar());
                    model.ResultAbstainCount = TotalCountAgainst;
                }
                conn.Close();

                bool? abstainBtnChoice =true;
                if(agmEventSetting!=null && agmEventSetting.AbstainBtnChoice!=null)
                {
                    abstainBtnChoice = agmEventSetting.AbstainBtnChoice;
                }
                if(abstainBtnChoice == true)
                {
                    string queryTotalCountAbstain = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + resolution.Id + "' AND VoteAbstain = 1";
                    conn.Open();
                    SqlCommand cmdcountAbstain = new SqlCommand(queryTotalCountAbstain, conn);
                    //object o = cmd.ExecuteScalar();
                    if (!DBNull.Value.Equals(cmdcountAbstain.ExecuteScalar()))
                    {
                        TotalCountAbstain = Convert.ToInt32(cmdcountAbstain.ExecuteScalar());
                        //model.ResultAbstainCount = TotalCountAbstain;
                    }
                    conn.Close();
                }

                string queryTotalCountVoid = "SELECT COUNT(*) FROM Results WHERE AGMID = '" + UniqueAGMId + "' AND QuestionId='" + resolution.Id + "' AND VoteVoid = 1";
                conn.Open();
                SqlCommand cmdcountVoid = new SqlCommand(queryTotalCountVoid, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmdcountVoid.ExecuteScalar()))
                {
                    TotalCountVoid = Convert.ToInt32(cmdcountVoid.ExecuteScalar());
                    //model.ResultAbstainCount = TotalCountAbstain;
                }
                conn.Close();
                //model.ResultFor = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.For == "FOR").ToList();
                //model.ResultAgainst = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.Against == "AGAINST").ToList();
                //model.ResultAbstain = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.Abstain == "ABSTAIN").ToList();
                //};
                int resultCount = 0;
                if (abstainbtnchoice)
                {
                    resultCount = resolution.result.Where(r => r.AGMID == UniqueAGMId).Count();
                }
                else
                {
                    resultCount = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAbstain==false).Count();
                }
               
                model.ResultForCount = TotalCountFor;
                model.ResultAgainstCount = TotalCountAgainst;
                model.ResultAbstainCount = TotalCountAbstain;
                model.ResultVoidCount = TotalCountVoid;
                model.TotalCount = resultCount;
                model.SyncChoice = agmEventSetting.SyncChoice;

                //SqlConnection conn =
                //new SqlConnection(connStr);
                //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                string query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND [VoteFor] = 1";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    holding = Convert.ToDouble(cmd.ExecuteScalar());
                    model.ResultForHolding = Convert.ToDouble(holding).ToString("0,0");
                }
                conn.Close();
                string query1 = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + resolution.Id + "' AND [VoteFor] = 1";
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query1, conn);
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                    model.ResultForPercentHolding = percentHolding.ToString();
                }
                conn.Close();

                model.ResultForHolding = holding.ToString();
                model.ResultForPercentHolding = percentHolding.ToString();

                //            SqlConnection conn =
                //new SqlConnection(connStr);
                //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                string query3 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteAgainst = 1";
                conn.Open();
                SqlCommand cmd3 = new SqlCommand(query3, conn);

                if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                {
                    holdingAgainst = Convert.ToDouble(cmd3.ExecuteScalar());
                    model.ResultForHoldingAgainst = Convert.ToDouble(holdingAgainst).ToString("0,0");
                }
                conn.Close();
                string query4 = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + resolution.Id + "' AND VoteAgainst = 1";
                conn.Open();
                SqlCommand cmd4 = new SqlCommand(query4, conn);
                if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                {
                    percentHoldingAgainst = Convert.ToDouble(cmd4.ExecuteScalar());
                }
                conn.Close();

                model.ResultForHoldingAgainst = Convert.ToDouble(holdingAgainst).ToString("0,0");
                model.ResultForPercentHoldingAgainst = percentHoldingAgainst.ToString();

                //bool? abstainBtnChoice = true;
                if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
                {
                    abstainBtnChoice = agmEventSetting.AbstainBtnChoice;
                }
                if (abstainBtnChoice == true)
                {

                    string query5 = "SELECT SUM(Holding) FROM Results WHERE   QuestionId='" + resolution.Id + "' AND VoteAbstain = 1";
                    conn.Open();
                    SqlCommand cmd5 = new SqlCommand(query5, conn);

                    if (!DBNull.Value.Equals(cmd5.ExecuteScalar()))
                    {
                        holdingAbstain = Convert.ToDouble(cmd5.ExecuteScalar());
                    }
                    conn.Close();
                    string query6 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteAbstain = 1";
                    conn.Open();
                    SqlCommand cmd6 = new SqlCommand(query6, conn);
                    if (!DBNull.Value.Equals(cmd6.ExecuteScalar()))
                    {
                        percentHoldingAgainst = Convert.ToDouble(cmd6.ExecuteScalar());
                    }
                    conn.Close();

                    model.ResultForHoldingAbstain = Convert.ToDouble(holdingAbstain).ToString("0,0");
                    model.ResultForPercentHoldingAbstain = percentHoldingAbstain.ToString();
                }


                string query8 = "SELECT SUM(Holding) FROM Results WHERE   QuestionId='" + resolution.Id + "' AND VoteVoid = 1";
                conn.Open();
                SqlCommand cmd8 = new SqlCommand(query8, conn);

                if (!DBNull.Value.Equals(cmd8.ExecuteScalar()))
                {
                    holdingVoid = Convert.ToDouble(cmd8.ExecuteScalar());
                }
                conn.Close();
                string query9 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteVoid = 1";
                conn.Open();
                SqlCommand cmd9 = new SqlCommand(query9, conn);
                if (!DBNull.Value.Equals(cmd9.ExecuteScalar()))
                {
                    percentHoldingAgainst = Convert.ToDouble(cmd9.ExecuteScalar());
                }
                conn.Close();

                model.ResultForHoldingVoid = Convert.ToDouble(holdingVoid).ToString("0,0");
                model.ResultForPercentHoldingVoid = percentHoldingVoid.ToString();

                if (abstainbtnchoice)
                {
                    string query7 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "'";
                    conn.Open();
                    SqlCommand cmd7 = new SqlCommand(query7, conn);
                    if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
                    {
                        TotalHolding = Convert.ToDouble(cmd7.ExecuteScalar());
                    }
                    conn.Close();
                }
                else
                {
                    string query7 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteAbstain != 1";
                    conn.Open();
                    SqlCommand cmd7 = new SqlCommand(query7, conn);
                    if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
                    {
                        TotalHolding = Convert.ToDouble(cmd7.ExecuteScalar());
                    }
                    conn.Close();
                }
              

                model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0");;
                model.TotalPercentHolding = TotalPercentHolding.ToString();

                //calcuate percentage holding per resolution..
                var pFor = (holding / TotalHolding) * 100;
                model.PercentageResultFor = pFor.ToString();
                var pAgainst = (holdingAgainst / TotalHolding) * 100;
                model.PercentageResultAgainst = pAgainst.ToString();
                double pAbstain = 0;
                if (abstainbtnchoice)
                {
                    pAbstain = (holdingAbstain / TotalHolding) * 100;
                    //bool? abstainBtnChoice = true;

                    model.PercentageResultAbstain = pAbstain.ToString();
                }
                var pVoid = (holdingVoid / TotalHolding) * 100;
                model.PercentageResultVoid = pVoid.ToString();


                var sumAllPercentages = pFor + pAgainst + pAbstain + pVoid;

                model.TotalPercentageHolding = sumAllPercentages.ToString();
            }


            return Task.FromResult<ReportViewModelDto>(model);
        }


        private Task<string> ConfirmSyncVotesAsync()
        {
            //var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var AgmEventSetting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
            var currentYear = DateTime.Now.Year.ToString();
            var currentResolution = db.Question.FirstOrDefault(q => q.AGMID == UniqueAGMId && q.questionStatus == true);

            var yetToVote = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.TakePoll == false).ToList();

            //Check Synced Votes

            var CheckSync = db.Result.Any(v => v.AGMID == UniqueAGMId && v.VoteStatus == "Synced" && v.QuestionId == currentResolution.Id);
            if (CheckSync)
            {
                //Unsync votes
                var syncVoters = db.Result.Where(r => r.QuestionId == currentResolution.Id && r.VoteStatus == "Synced").ToList();
                               
                if (syncVoters.Any())
                {
                    db.Result.RemoveRange(syncVoters);
                    
                }
                db.SaveChanges();
            }
            //var resolution = db.Question.Find(id);
            //var result = db.Result.Where(r=>r.QuestionId==id).ToList();

            var votechoice = "FOR";
            if (AgmEventSetting != null && AgmEventSetting.SyncChoice != null)
            {
                votechoice = AgmEventSetting.SyncChoice.Trim();
            }

            var abstainBtnchoice = true;

            if (AgmEventSetting != null && AgmEventSetting.AbstainBtnChoice != null)
            {
                abstainBtnchoice = (bool)AgmEventSetting.AbstainBtnChoice;
            }
            int i = 1;
            foreach (var item in yetToVote)
            {

                var checkresult = db.Result.Any(r => r.ShareholderNum == item.ShareholderNum && r.QuestionId == currentResolution.Id);
                if (!checkresult)
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
                    result.QuestionId = currentResolution.Id;
                    result.VoteStatus = "Synced";
                    if (votechoice == "FOR")
                    {
                        result.VoteFor = true;
                        result.VoteAgainst = false;
                        result.VoteAbstain = false;
                        result.VoteVoid = false;
                    }
                    else if (votechoice == "AGAINST")
                    {
                        result.VoteAgainst = true;
                        result.VoteFor = false;
                        result.VoteAbstain = false;
                        result.VoteVoid = false;
                    }
                    else if (votechoice == "ABSTAIN")
                    {
                       if(abstainBtnchoice)
                        {
                            result.VoteAbstain = true;
                            result.VoteFor = false;
                            result.VoteAgainst = false;
                            result.VoteVoid = false;
                        }

                    }
                    else if (votechoice == "VOID")
                    {
                        if (abstainBtnchoice)
                        {
                            result.VoteAbstain = false;
                            result.VoteFor = false;
                            result.VoteAgainst = false;
                            result.VoteVoid = true;
                        }

                    }

                    db.Result.Add(result);
                    //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
                    //Functions.SendProgress("Process in progress...", i, yetToVote.Count());
                    i++;
                    //resolution.syncStatus = true;
                    //db.Entry(resolution).State = EntityState.Modified;

                    //item.TakePoll = true;
                    //db.Entry(item).State = EntityState.Modified;

                }
                //else
                //{
                //    checkresult.date = DateTime.Now;
                //    //checkresult.Yes = "Yes";
                //    db.Entry(checkresult).State = EntityState.Modified;
                //    //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
                //    //Functions.SendProgress("Process in progress...", i, yetToVote.Count());
                //    i++;

                //}

            }

            try
            {
                currentResolution.syncStatus = true;
                db.Entry(currentResolution).State = EntityState.Modified;
                db.SaveChanges();
                return Task.FromResult<string>("Success");
            }
            catch (DbUpdateConcurrencyException e)
            {
                return Task.FromResult<string>("Something went Wrong! " + e.StackTrace);
            }

        }





        //private Task<string> ConfirmSyncVotesAsync()
        //{
        //    //var companyinfo = ua.GetUserCompanyInfo();
        //    var UniqueAGMId = ua.RetrieveAGMUniqueID();
        //    var AgmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
        //    var currentYear = DateTime.Now.Year.ToString();
        //    var currentResolution = db.Question.SingleOrDefault(q => q.AGMID == UniqueAGMId && q.questionStatus == true);

        //    var yetToVote = db.Present.Where(p => p.AGMID == UniqueAGMId && p.TakePoll == false).ToList();

        //    //Check Synced Votes
        //    if(!db.Result.Any())
        //    {
        //        return Task.FromResult<string>("No votes");
        //    }
        //    //var CheckSync = db.Result.Any(v => v.AGMID == UniqueAGMId && v.VoteStatus == "Synced" && v.QuestionId == currentResolution.Id);
        //    //if (CheckSync)
        //    //{
        //    //    return Task.FromResult<string>("Synced");
        //    //}
        //    //int i = 1;
        //    foreach (var item in yetToVote)
        //    {
        //        var votechoice = "FOR";
        //        if(AgmEventSetting!=null && AgmEventSetting.SyncChoice!=null)
        //        {
        //            votechoice = AgmEventSetting.SyncChoice;
        //        }

        //            var checkresult = db.Result.Any(r =>r.AGMID==UniqueAGMId && r.Identity == item.ShareholderNum && r.QuestionId == currentResolution.Id);
        //            if (!checkresult)
        //            {
        //                if(votechoice =="FOR")
        //            {
        //                string query2 = "Insert into Results(Name,Address,Company,Year,AGMID,date,Holding,Identity,phonenumber,PercentageHolding,Present,QuestionId,VoteStatus,For) Values('" +
        //                item.Name + "','" + item.Address + "','" + item.Company + "','" + currentYear + "','" + item.AGMID + "','" + DateTime.Now + "','" + item.Holding + "','" + item.ShareholderNum + "','" + item.PhoneNumber + "','" + item.PercentageHolding + "','" + 1 + "','" + currentResolution.Id + "','" + 1 + "','" + votechoice + "')";
        //                conn.Open();
        //                SqlCommand cmd = new SqlCommand(query2, conn);
        //                cmd.ExecuteNonQuery();
        //                conn.Close();
        //            }
        //                else if(votechoice == "AGAINST")
        //            {
        //                string query2 = "Insert into Results(Name,Address,Company,Year,AGMID,date,Holding,Identity,phonenumber,PercentageHolding,Present,QuestionId,VoteStatus,Against) Values('" +
        //                item.Name + "','" + item.Address + "','" + item.Company + "','" + currentYear + "','" + item.AGMID + "','" + DateTime.Now + "','" + item.Holding + "','" + item.ShareholderNum + "','" + item.PhoneNumber + "','" + item.PercentageHolding + "','" + 1 + "','" + currentResolution.Id + "','" + 1 + "','" + votechoice + "')";
        //                conn.Open();
        //                SqlCommand cmd = new SqlCommand(query2, conn);
        //                cmd.ExecuteNonQuery();
        //                conn.Close();

        //            }
        //                else if(votechoice == "ABSTAIN")
        //            {
        //                string query2 = "Insert into Results(Name,Address,Company,Year,AGMID,date,Holding,Identity,phonenumber,PercentageHolding,Present,QuestionId,VoteStatus,Abstain) Values('" +
        //                item.Name + "','" + item.Address + "','" + item.Company + "','" + currentYear + "','" + item.AGMID + "','" + DateTime.Now + "','" + item.Holding + "','" + item.ShareholderNum + "','" + item.PhoneNumber + "','" + item.PercentageHolding + "','" + 1 + "','" + currentResolution.Id + "','" + 1 + "','" + votechoice + "')";
        //                conn.Open();
        //                SqlCommand cmd = new SqlCommand(query2, conn);
        //                cmd.ExecuteNonQuery();
        //                conn.Close();
        //            }


        //                //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
        //                //Functions.SendProgress("Process in progress...", i, yetToVote.Count);
        //            }           
        //            //else
        //            //{
        //            //    checkresult.date = DateTime.Now;
        //            //    //checkresult.Yes = "Yes";
        //            //    db.Entry(checkresult).State = EntityState.Modified;
        //            //    //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
        //            //    //Functions.SendProgress("Process in progress...", i, yetToVote.Count());

        //            //}

        //          }


        //    try
        //    {
        //        db.SaveChanges();
        //        return Task.FromResult<string>("Success");
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return Task.FromResult<string>("Something went Wrong!");
        //    }

        //}


    }
}