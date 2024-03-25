// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Data.Entity;
// using System.Data.Entity.Infrastructure;
// using System.Diagnostics;
// using System.Linq;
// using System.Threading.Tasks;
// using System.Web;
// using System.Web.Mvc;
// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;

// namespace BarcodeGenerator.Controllers
// {
//     public class ClikapadController : Controller
//     {
//         UsersContext db = new UsersContext();
//         Stopwatch stopwatch = new Stopwatch();
//         UserAdmin ua = new UserAdmin();
//         //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

//         //private int RetrieveAGMUniqueID()
//         //{
//         //    UsersContext adb = new UsersContext();
//         //    var companyinfo = ua.GetUserCompanyInfo();
//         //    var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

//         //    return AGMID;
//         //}
//         public async Task<bool> GetClikapadResult(string company)
//         {

//             var response = await GetClikapadResultsAsync(company);

//             return response;
//         }


//         public Task<bool> GetClikapadResultsAsync(string company)
//         {
//             try
//             {
//                 //var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
//                 if(UniqueAGMId == -1)
//                 {
//                     return Task.FromResult<bool>(false);
//                 }
//                 var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
//                 //if (agmEvent != null)
//                 //{
//                 //    if (agmEvent.allChannels != true || agmEvent.webChannel != true)
//                 //    {
//                 //        return Task.FromResult<bool>(false);
//                 //    }
//                 //}

//                 var abstainBtnchoice = true;

//                 if (agmEvent != null && agmEvent.AbstainBtnChoice != null)
//                 {
//                     abstainBtnchoice = (bool)agmEvent.AbstainBtnChoice;
//                 }



//                 if (TimerControll.GetTimeStatus(company))
//                 {
//                     var clikapadResults = db.KeypadResults.Where(r => r.AGMID == UniqueAGMId && r.Valuechecked == false);
//                     var question = db.Question.SingleOrDefault(q => q.AGMID == UniqueAGMId && q.questionStatus == true);
//                     foreach (var resultvalue in clikapadResults)
//                     {
//                         var keypad = resultvalue.Keypad.Trim();
//                         if (abstainBtnchoice == false && resultvalue.Keyvalue.Contains("3"))
//                         {
//                             continue;
//                         }
//                         using (UsersContext qdb = new UsersContext())
//                         {
//                             var voter = qdb.Present.SingleOrDefault(q => q.AGMID == UniqueAGMId && q.Clikapad == keypad);
//                             if (voter != null && question != null)
//                             {


//                                 var checkresult = qdb.Result.SingleOrDefault(r => r.AGMID == UniqueAGMId && r.ShareholderNum == voter.ShareholderNum && r.QuestionId == question.Id);
//                                 //if true - edit value
//                                 if (checkresult != null)
//                                 {
//                                     checkresult.date = DateTime.Now;
//                                     checkresult.Timestamp = DateTime.Now.TimeOfDay;
//                                     checkresult.VoteStatus = "Voted";
//                                     checkresult.Source = "Pad";
//                                     if (resultvalue.Keyvalue.Contains("1"))
//                                     {
//                                         checkresult.VoteFor = true;
//                                         checkresult.VoteAgainst = false;
//                                         checkresult.VoteAbstain = false;
//                                         checkresult.VoteVoid = false;
//                                     }
//                                     else if (resultvalue.Keyvalue.Contains("2"))
//                                     {
//                                         checkresult.VoteAgainst = true;
//                                         checkresult.VoteFor = false;
//                                         checkresult.VoteAbstain = false;
//                                         checkresult.VoteVoid = false;
//                                     }
//                                     else if (resultvalue.Keyvalue.Contains("3"))
//                                     {
//                                         checkresult.VoteAbstain = true;
//                                         checkresult.VoteFor = false;
//                                         checkresult.VoteAgainst = false;
//                                         checkresult.VoteVoid = false;
//                                     }
//                                     qdb.Entry(checkresult).State = EntityState.Modified;
//                                     try
//                                     {
//                                         qdb.SaveChanges();

//                                     }
//                                     catch (DbUpdateConcurrencyException e)
//                                     {

//                                     }


//                                 }
//                                 else
//                                 {
//                                     Result result = new Result();
//                                     result.Name = voter.Name;
//                                     result.ShareholderNum = voter.ShareholderNum;
//                                     result.Address = voter.Address;
//                                     result.Holding = voter.Holding;
//                                     result.phonenumber = voter.PhoneNumber;
//                                     result.Clickapad = voter.Clikapad;
//                                     result.PercentageHolding = voter.PercentageHolding;
//                                     result.QuestionId = question.Id;
//                                     result.AGMID = UniqueAGMId;
//                                     result.date = DateTime.Now;
//                                     result.Timestamp = DateTime.Now.TimeOfDay;
//                                     result.VoteStatus = "Voted";
//                                     result.Source = "Pad";
//                                     result.Present = true;
//                                     if (resultvalue.Keyvalue.Contains("1"))
//                                     {
//                                         result.VoteFor = true;
//                                         result.VoteAgainst = false;
//                                         result.VoteAbstain = false;
//                                         result.VoteVoid = false;

//                                     }
//                                     else if (resultvalue.Keyvalue.Contains("2"))
//                                     {
//                                         result.VoteAgainst = true;
//                                         result.VoteFor = false;
//                                         result.VoteAbstain = false;
//                                         result.VoteVoid = false;
//                                     }
//                                     else if (resultvalue.Keyvalue.Contains("3"))
//                                     {
//                                         result.VoteAbstain = true;
//                                         result.VoteFor = false;
//                                         result.VoteAgainst = false;
//                                         result.VoteVoid = false;
//                                     }
//                                     qdb.Result.Add(result);
//                                 }

//                                 voter.TakePoll = true;
//                                 qdb.Entry(voter).State = EntityState.Modified;
//                                 try
//                                 {
//                                     qdb.SaveChanges();

//                                 }
//                                 catch (DbUpdateConcurrencyException e)
//                                 {
//                                     break;
//                                 }
//                             }

//                         }
//                         resultvalue.Valuechecked = true;
//                         db.Entry(resultvalue).State = EntityState.Modified;
//                     }
//                     db.SaveChanges();

//                     //if (!TimerControll.GetTimeStatus(company))
//                     //    break;
//                 }
//                 //return System.Threading.Tasks.Task.FromResult<bool>(true);
//                 return Task.FromResult<bool>(true);
//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<bool>(false);
//             }
//         }



//         //public Task<bool> GetClikapadResultsAsync(string company)
//         //{ 
//         //   try
//         //    {
//         //        //var companyinfo = ua.GetUserCompanyInfo();
//         //        var UniqueAGMId = ua.RetrieveAGMUniqueID(company);

//         //        while(TimerControll.GetTimeStatus(company))
//         //        {
//         //            var clikapadResults = db.KeypadResults.Where(r =>r.AGMID == UniqueAGMId && r.Valuechecked == false);
//         //            var question = db.Question.SingleOrDefault(q =>q.AGMID == UniqueAGMId && q.questionStatus == true);
//         //            if(question==null)
//         //            {
//         //                break;
//         //            }
//         //            foreach (var resultvalue in clikapadResults)
//         //            {
//         //                var keypad = resultvalue.Keypad.Trim();
//         //                using (UsersContext qdb = new UsersContext())
//         //                {
//         //                    var voter = qdb.Present.SingleOrDefault(q =>q.AGMID == UniqueAGMId && q.Clikapad == keypad);
//         //                    if (voter != null && question != null)
//         //                    {
//         //                        break;
//         //                    }
//         //                        var checkresult = qdb.Result.SingleOrDefault(r =>r.AGMID == UniqueAGMId && r.Identity == voter.ShareholderNum && r.QuestionId == question.Id);
//         //                        //if true - edit value
//         //                        if (checkresult != null)
//         //                        {
//         //                            checkresult.date = DateTime.Now;
//         //                            checkresult.Timestamp = DateTime.Now.TimeOfDay;
//         //                            checkresult.VoteStatus = "Voted";
//         //                            checkresult.Source = "Pad";
//         //                            if (resultvalue.Keyvalue.Contains("1"))
//         //                            {
//         //                                checkresult.For = "FOR";
//         //                                checkresult.Against = null;
//         //                                checkresult.Abstain = null;
//         //                            }
//         //                            else if (resultvalue.Keyvalue.Contains("2"))
//         //                            {
//         //                                checkresult.Against = "AGAINST";
//         //                                checkresult.For = null;
//         //                                checkresult.Abstain = null;
//         //                            }
//         //                            else if (resultvalue.Keyvalue.Contains("3"))
//         //                            {
//         //                                checkresult.Abstain = "ABSTAIN";
//         //                                checkresult.For = null;
//         //                                checkresult.Against = null;
//         //                            }
//         //                            qdb.Entry(checkresult).State = EntityState.Modified;
//         //                            try
//         //                            {
//         //                                qdb.SaveChanges();

//         //                            }
//         //                            catch (DbUpdateConcurrencyException e)
//         //                            {

//         //                            }

//         //                        }
//         //                        else
//         //                        {
//         //                            Result result = new Result();
//         //                            result.Name = voter.Name;
//         //                            result.Identity = voter.ShareholderNum;
//         //                            result.Address = voter.Address;
//         //                            result.Holding = voter.Holding;
//         //                            result.phonenumber = voter.PhoneNumber;
//         //                            result.Clickapad = voter.Clikapad;
//         //                            result.PercentageHolding = voter.PercentageHolding;
//         //                            result.QuestionId = question.Id;
//         //                            result.AGMID = UniqueAGMId;
//         //                            result.date = DateTime.Now;
//         //                            result.Timestamp = DateTime.Now.TimeOfDay;
//         //                            result.VoteStatus = "Voted";
//         //                            result.Source = "Pad";
//         //                            result.Present = true;
//         //                            if (resultvalue.Keyvalue.Contains("1"))
//         //                            {
//         //                                result.For = "FOR";

//         //                            }
//         //                            else if (resultvalue.Keyvalue.Contains("2"))
//         //                            {
//         //                                result.Against = "AGAINST";
//         //                            }
//         //                            else if (resultvalue.Keyvalue.Contains("3"))
//         //                            {
//         //                                result.Abstain = "ABSTAIN";
//         //                            }
//         //                            qdb.Result.Add(result);
//         //                        }

//         //                        voter.TakePoll = true;
//         //                        qdb.Entry(voter).State = EntityState.Modified;
//         //                        try
//         //                        {
//         //                            qdb.SaveChanges();

//         //                        }
//         //                        catch (DbUpdateConcurrencyException e)
//         //                        {
//         //                            break;
//         //                        }


//         //                }
//         //                resultvalue.Valuechecked = true;
//         //                db.Entry(resultvalue).State = EntityState.Modified;
//         //            }
//         //            db.SaveChanges();

//         //            //if (!TimerControll.GetTimeStatus(company))
//         //            //    break;
//         //        }
//         //        //return System.Threading.Tasks.Task.FromResult<bool>(true);
//         //        return Task.FromResult<bool>(true);
//         //    }
//         //    catch (Exception e)
//         //    {
//         //        return Task.FromResult<bool>(false);
//         //    }
//         //}



//         public void ClearKeyPadResults(string company)
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID(company);

//             var results = db.KeypadResults.Where(r=>r.Company.ToLower()== company.ToLower());
//             foreach (var item in results)
//             {
//                 db.KeypadResults.Remove(item);
//             }
//             db.SaveChanges();
//         }
//         //        var voter = db.Present.SingleOrDefault(q => q.emailAddress == post.identity && q.present == true);

//         //        if (voter != null)
//         //        {
//         //            var question = db.Question.Find(post.Id);
//         //            if (question != null && question.questionStatus == true)
//         //            {
//         //                if (TimerControll.GetTimeStatus() == true)
//         //                {
                           
//         //                }

//         //                //Voting clock has stopped
//         //                var timeoutput = JsonConvert.SerializeObject(timeoutresponse);

//         //                //return new HttpResponseMessage()
//         //                //{
//         //                //    Content = new StringContent(timeoutput, System.Text.Encoding.UTF8, "application/json")
//         //                //};

//         //            }

//         //            //Wait for voting to commence on this resolution
//         //            var resolutiontimeoutput = JsonConvert.SerializeObject(resolutiontimeresponse);

//         //            //return new HttpResponseMessage()
//         //            //{
//         //            //    Content = new StringContent(resolutiontimeoutput, System.Text.Encoding.UTF8, "application/json")
//         //            //};

//         //        }

//         //        //Voter not marked present at AGM
//         //        var voteroutput = JsonConvert.SerializeObject(voterresponse);

//         //        //return new HttpResponseMessage()
//         //        //{
//         //        //    Content = new StringContent(voteroutput, System.Text.Encoding.UTF8, "application/json")
//         //        //};


//         //    }
//         //    catch
//         //    {
//         //        var error = JsonConvert.SerializeObject(Errorresponse);

//         //        //return new HttpResponseMessage()
//         //        //{
//         //        //    Content = new StringContent(error, System.Text.Encoding.UTF8, "application/json")
//         //        //};
//         //    }

//         //}

//     }
// }