// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;
// using Newtonsoft.Json;
// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Data.Entity;
// using System.Data.Entity.Infrastructure;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Net.Http.Headers;
// using System.Text;
// using System.Threading.Tasks;
// using System.Web;
// using System.Web.Mvc;

// namespace BarcodeGenerator.Controllers
// {
//     public class SMSManagerController : Controller
//     {
//         //
//         // GET: /SMSManager/

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

//         public ActionResult Index()
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var resolutions = db.Question.Where(q=>q.AGMID==UniqueAGMId).ToList();
//             return View(resolutions);
//         }


//         //public ActionResult SMSResolution()
//         //{
//         //    var resolutions = db.Question.ToList();          
//         //    foreach (var item in resolutions)
//         //    {
//         //        try
//         //        {
//         //          var DoesTextExist = db.Messages.SingleOrDefault(q => q.text == item.question);
//         //        if(DoesTextExist==null)
//         //        {
//         //            Messages message = new Messages();
//         //            message.text = item.question;
//         //            message.from = "InfoSMS";
//         //            message.flash = false;
//         //            message.notifyContentType = "application/json";
//         //            message.intermediateReport = true;
//         //            db.Messages.Add(message);
//         //        }
//         //        }catch
//         //        {
//         //            continue;
                
//         //        }

               
//         //    }
//         //    db.SaveChanges();
//         //    return RedirectToAction("Index");
//         //}

//         //
//         // GET: /SMSManager/Details/5

//         //public ActionResult AddContacts(int id)
//         //{
//         //    var presentShareholders = db.Present.Where(p=>p.PhoneNumber!=null).ToList();
//         //    var message = db.Messages.Find(id);
//         //    foreach (var item in presentShareholders)
//         //    {
//         //        var phonenumber = "234" + item.PhoneNumber;
//         //        var DoesDestinationExist = message.destinations.SingleOrDefault(s=>s.to==phonenumber);
//         //        if (DoesDestinationExist == null)
//         //        {
//         //            Destination destination = new Destination();
//         //            destination.to = phonenumber;
//         //            destination.messageId = message.id;
//         //            message.destinations.Add(destination);
//         //        }
//         //    }
//         //    db.SaveChanges();
//         //    return RedirectToAction("Index");
//         //}

//         public ActionResult  SendSingleSMS(int id)
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var resolution = db.Question.Find(id);
//             var destination = db.Present.Where(p=>p.AGMID==UniqueAGMId && p.PhoneNumber!=null).Select(s => s.PhoneNumber).ToArray();
//             singleSMS sms = new singleSMS
//             {
//                 from = "Gapplus",
//                 to = destination,
//                 text = resolution.question + ". Reply 1 in Favor or 2 for AGAINST or 3 for ABSTAIN to +2349060008064"
//             };

//             //var output = JsonConvert.SerializeObject(sms);
//             //HttpResponseMessage response;
//             //using (var client = new HttpClient())
//             //{
//             //    var byteArray = Encoding.ASCII.GetBytes("GapPlus:P@ssw0rd01");
//             //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
//             //    client.BaseAddress = new Uri("https://api.infobip.com/");
              
//             //    client.DefaultRequestHeaders.Accept.Clear();
//             //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
//             //   response = await client.PostAsJsonAsync(
//             //                 "sms/1/text/single", output);
//             //    response.EnsureSuccessStatusCode();
             
//             //}

//             return Json(sms, JsonRequestBehavior.AllowGet);
//         }

      
//         public String  LogSMSDelivery(int id, SMSDeliveryLog post)
//         {
//             var resolution = db.Question.Find(id);

//             SMSDeliveryLog delivery = new SMSDeliveryLog()
//             {
//                 to = post.to,
//                 status = post.status,
//                 description = post.description,
//                 smsCount = post.smsCount,
//                 deliveryId = post.deliveryId,
//                 date = DateTime.Now
//             };
//             resolution.SMSdeliveryLog.Add(delivery);
//             db.SaveChanges();

//             return "success" + " " + post.to;
//         }

//         public string SMSResult(int id, SMSResult post)
//         {
//             var resolution = db.Question.Find(id);
//             //Check if number is present   
//             //var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             if(UniqueAGMId == -1)
//             {
//                 return "AGM not active.";
//             }
//             var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
//             if (agmEvent != null)
//             {
//                 if (agmEvent.allChannels != true && agmEvent.smsChannel != true)
//                 {
//                     return "Channel not enabled for voting.";
//                 }
//             }

//             var abstainBtnchoice = true;

//             if (agmEvent != null && agmEvent.AbstainBtnChoice != null)
//             {
//                 abstainBtnchoice = (bool)agmEvent.AbstainBtnChoice;
//             }


//             if (abstainBtnchoice == false && (post.text.ToLower().Contains("3") || post.text.ToLower().Contains("abstain") || post.text.ToLower().Contains("absain") || post.text.ToLower().Contains("abstan")))
//             {
//                 return "ABSTAIN is not a choice.";
//             }

//             var voters = db.Present.Where(q =>q.AGMID == UniqueAGMId && q.PhoneNumber == post.from && q.present==true).ToArray();
//           if (voters.Count() != 0 && resolution.questionStatus == true)
//           {
//               foreach(var voter in voters)
//               {
//                     //if yes, check if he has voted.
//                var checkresults = db.Result.Where(r => r.AGMID == UniqueAGMId && r.phonenumber == post.from && r.QuestionId == id).ToArray();
//                     //if true - edit value
//                     if (checkresults.Count() != 0)
//                     {
//                         foreach (var checkresult in checkresults)
//                         {
//                             checkresult.date = DateTime.Now;
//                             checkresult.VoteStatus = "Voted";
//                             checkresult.Source = "SMS";
//                             if (post.text.ToLower().Contains("1") || post.text.ToLower().Contains("for") || post.text.ToLower().Contains("form") || post.text.ToLower().Contains("fore"))
//                             {
//                                 checkresult.VoteFor = true;
//                                 checkresult.VoteAgainst = false;
//                                 checkresult.VoteAbstain = false;
//                                 checkresult.VoteVoid = false;
//                             }
//                             else if (post.text.ToLower().Contains("2") || post.text.ToLower().Contains("against") || post.text.ToLower().Contains("againt") || post.text.ToLower().Contains("again"))
//                             {
//                                 checkresult.VoteAgainst = true;
//                                 checkresult.VoteFor = false;
//                                 checkresult.VoteAbstain = false;
//                                 checkresult.VoteVoid = false;
//                             }
//                             else if (post.text.ToLower().Contains("3") || post.text.ToLower().Contains("abstain") || post.text.ToLower().Contains("absain") || post.text.ToLower().Contains("abstan"))
//                             {
//                                 checkresult.VoteAbstain = true;
//                                 checkresult.VoteFor = false;
//                                 checkresult.VoteAgainst = false;
//                                 checkresult.VoteVoid = false;
//                             }
//                             db.Entry(checkresult).State = EntityState.Modified;
//                         }
//                         SMSResult smsresult = new SMSResult()
//                         {
//                             to = post.to,
//                             messageId = post.messageId,
//                             keyword = post.keyword,
//                             smsCount = post.smsCount,
//                             receivedAt = post.receivedAt,
//                             from = post.from,
//                             text = post.text,
//                             cleanText = post.cleanText
//                         };
//                         resolution.SMSResult.Add(smsresult);

//                         try
//                         {
//                             db.SaveChanges();
//                         }
//                         catch (DbUpdateConcurrencyException)
//                         {
//                             continue;
//                         }


//                     }
//                     else if (checkresults.Count() == 0)
//                     {
//                         Result result = new Result
//                         {
//                             Name = voter.Name,
//                             ShareholderNum = voter.ShareholderNum,
//                             phonenumber = voter.PhoneNumber,
//                             AGMID = UniqueAGMId,
//                         Address = voter.Address,
//                         Holding = voter.Holding,
//                         PercentageHolding = voter.PercentageHolding,
//                         QuestionId = id,
//                         date = DateTime.Now,
//                         Present = true,
//                         VoteStatus = "Voted",
//                             Source = "SMS"
//                     };
//                         if (post.text.ToLower().Contains("1") || post.text.ToLower().Contains("for") || post.text.ToLower().Contains("form") || post.text.ToLower().Contains("fore"))
//                         {
//                             result.VoteFor = true;
//                             result.VoteAgainst = false;
//                             result.VoteAbstain = false;
//                             result.VoteVoid = false;
//                         }
//                         else if (post.text.ToLower().Contains("2") || post.text.ToLower().Contains("against") || post.text.ToLower().Contains("againt") || post.text.ToLower().Contains("again"))
//                         {
//                             result.VoteAgainst = true;
//                             result.VoteFor = false;
//                             result.VoteAbstain = false;
//                             result.VoteVoid = false;
//                         }
//                         else if (post.text.ToLower().Contains("3") || post.text.ToLower().Contains("abstain") || post.text.ToLower().Contains("absain") || post.text.ToLower().Contains("abstan"))
//                         {
//                             result.VoteAbstain = true;
//                             result.VoteFor = false;
//                             result.VoteAgainst = false;
//                             result.VoteVoid = false;
//                         }
//                         db.Result.Add(result);
//                         voter.TakePoll = true;
//                         SMSResult newsmsresult = new SMSResult()
//                         {
//                             to = post.to,
//                             messageId = post.messageId,
//                             keyword = post.keyword,
//                             smsCount = post.smsCount,
//                             receivedAt = post.receivedAt,
//                             from = post.from,
//                             text = post.text,
//                             cleanText = post.cleanText
//                         };
//                         resolution.SMSResult.Add(newsmsresult);
//                         db.Entry(voter).State = EntityState.Modified;
//                         try
//                         {
//                             db.SaveChanges();
//                         }
//                         catch (DbUpdateConcurrencyException)
//                         {
//                             continue;
//                         }

                      
//                     }
                    
//               }
//              return "Success!";
//           }
//           return "Voter is not marked present";
//         }
        
//         //public ActionResult SendSMS()
//         //{
//         //    var allmessages = db.Messages.ToList();
//         //    sendSMS sms = new sendSMS 
//         //    {
//         //        bulkId = "BULK-ID-123-xyz",
//         //        messages = allmessages
               
//         //    };
//         //    return View();
//         //}

//         public ActionResult Details(int id)
//         {
//             return View();
//         }

//         //
//         // GET: /SMSManager/Create

//         public ActionResult Create()
//         {

//             return View();
//         }

//         //
//         // POST: /SMSManager/Create

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

//         //
//         // GET: /SMSManager/Edit/5

//         public ActionResult Edit(int id)
//         {
//             return View();
//         }

//         //
//         // POST: /SMSManager/Edit/5

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
//         // GET: /SMSManager/Delete/5

//         public ActionResult Delete(int id)
//         {
//             return View();
//         }

//         //
//         // POST: /SMSManager/Delete/5

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
