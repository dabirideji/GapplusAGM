// using BarcodeGenerator.Models;
// using Newtonsoft.Json;
// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Data.Entity.Infrastructure;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Threading.Tasks;
// using System.Web.Http;
// //using System.Web.Mvc;
// using System.Diagnostics;
// using System.Threading;
// using System.Data.Entity;
// using BarcodeGenerator.Service;
// using System.Web.Http.Description;

// namespace BarcodeGenerator.Controllers
// {
//     [ApiExplorerSettings(IgnoreApi = true)]
//     public class VoteCollectionAPIController : ApiController
//     {

//         UsersContext db = new UsersContext();
//         Stopwatch stopwatch = new Stopwatch();
//         UserAdmin ua = new UserAdmin();

//         //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

//         //private int RetrieveAGMUniqueID(string companyinfo)
//         //{
//         //    var AGMUniqueID = db.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

//         //    return AGMUniqueID;
//         //}
//         private static string currentYear = DateTime.Now.Year.ToString();

//         HttpRequestResponse successresponse = new HttpRequestResponse
//         {
//             Code = "200",
//             Message = "Success"
//         };

//         HttpRequestResponse timeoutresponse = new HttpRequestResponse
//         {
//             Code = "406",
//             Message = "Voting clock has stopped"
//         };
//         HttpRequestResponse resolutiontimeresponse = new HttpRequestResponse
//         {
//             Code = "400",
//             Message = "Wait for voting to commence on this resolution"
//         };
//         HttpRequestResponse voterresponse = new HttpRequestResponse
//         {
//             Code = "505",
//             Message = "Voter not marked present at AGM"
//         };

//         HttpRequestResponse Errorresponse = new HttpRequestResponse
//         {
//             Code = "500",
//             Message = "Error"
//         };

//         [HttpGet]
//         public HttpResponseMessage Get()
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             var question = db.Question.ToList();
//             var questionlist = from c in question
//                                select new Resolutions()
//                                {
//                                    Id = c.Id,
//                                    question = c.question,
//                                    date = c.date,
//                                    questionStatus = c.questionStatus
//                                };

//             //var questionlist = question.ToList();
//             //dynamic collectionWrapper = new
//             //{
//             //    resolutions = questionlist,
               
//             //};
//             var output = JsonConvert.SerializeObject(questionlist);

//             return new HttpResponseMessage()
//             {
//                 Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
//             };
//         }

//         // GET api/votecollectionapi/5

//         [HttpPost]
//         public async Task<HttpRequestResponse> GetResolutionRequest([FromBody] VoteModel post)
//         {
//             var response = await GetResolutionRequestAsync(post);

//             return response;
//         }



//         private Task<HttpRequestResponse> GetResolutionRequestAsync([FromBody]VoteModel post)
//         {
//             try
//             {
//                 if (!string.IsNullOrEmpty(post.company))
//                 {
//                     var UniqueAGMId = ua.RetrieveAGMUniqueID(post.company);
//                     if(UniqueAGMId != -1)
//                     {
//                         var question = db.Question.Where(q => q.Company == post.company && q.AGMID == UniqueAGMId).ToList();
//                         var questionlist = from c in question
//                                            select new Resolutions()
//                                            {
//                                                Id = c.Id,
//                                                question = c.question,
//                                                date = c.date,
//                                                questionStatus = c.questionStatus
//                                            };

//                         return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                         {
//                             Code = "200",
//                             Message = "Success",
//                             Content = questionlist
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
//                     return Task.FromResult<HttpRequestResponse>( new HttpRequestResponse
//                     {
//                         Code = "404",
//                         Message = "Empty Request"
//                     });
//                 }
//             }
//             catch(Exception e)
//             {
//                 return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                 {
//                     Code = "500",
//                     Message = "Failed Request"
//                 });
//             }
          
           
//         }

//         // POST api/votecollectionapi
//         [HttpPost]
//         public async Task<HttpRequestResponse> VotingResponse([FromBody] VoteModel post)
//         {
//             var response = await VotingResponseAsync(post);

//             return response;
//         }

//             // POST api/votecollectionapi
//         private Task<HttpRequestResponse> VotingResponseAsync(VoteModel post)
//         {
//                 try
//                 {


//                 if(!ModelState.IsValid)
//                 {
//                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                     {
//                         Code = "403",
//                         Message = "Invalid Request"
//                     });
//                 }

//                 if(!string.IsNullOrEmpty(post.company)&& !string.IsNullOrWhiteSpace(post.company))
//                 {
//                     var UniqueAGMId = ua.RetrieveAGMUniqueID(post.company);
//                     if (UniqueAGMId != -1)
//                     {
//                         var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

//                         if(agmEvent!=null)
//                         {
//                             if(agmEvent.allChannels!=true && agmEvent.mobileChannel != true )
//                             {
//                                 return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                                 {
//                                     Code = "403",
//                                     Message = "Channel not Enabled for voting."
//                                 });
//                             }
//                         }

//                         var abstainBtnchoice = true;

//                         if (agmEvent != null && agmEvent.AbstainBtnChoice != null)
//                         {
//                             abstainBtnchoice = (bool)agmEvent.AbstainBtnChoice;
//                         }

//                         if (abstainBtnchoice == false && post.response == "ABSTAIN")
//                         {
//                             return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                             {
//                                 Code = "403",
//                                 Message = "ABSTAIN is not a choice."
//                             });
//                         }

//                         if (TimerControll.GetTimeStatus(post.company) == true)
//                         {
//                             var question = db.Question.Find(post.Id);
//                             if (question != null && question.questionStatus == true)
//                             {
//                                 //var voter = db.Present.SingleOrDefault(q => q.emailAddress == post.identity && q.present == true);                         
//                                 var voters = db.Present.Where(q => q.AGMID == UniqueAGMId && q.emailAddress == post.identity && q.present == true).ToList();

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
//                                         checkresult.Source = "Mobile";
//                                         if (post.response == "FOR")
//                                         {
//                                             checkresult.VoteFor = true;
//                                             checkresult.VoteAgainst = false;
//                                             checkresult.VoteAbstain = false;
//                                             checkresult.VoteVoid = false;
//                                         }
//                                         else if (post.response == "AGAINST")
//                                         {
//                                             checkresult.VoteAgainst = true;
//                                             checkresult.VoteFor = false;
//                                             checkresult.VoteAbstain = false;
//                                             checkresult.VoteVoid = false;
//                                         }
//                                         else if (post.response == "ABSTAIN")
//                                         {
//                                             checkresult.VoteAbstain = true;
//                                             checkresult.VoteFor = false;
//                                             checkresult.VoteAgainst = false;
//                                             checkresult.VoteVoid = false;
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
//                                         result.Source = "Mobile";
//                                         result.Present = true;
//                                         if (post.response == "FOR")
//                                         {
//                                             result.VoteFor = true;
//                                             result.VoteAgainst = false;
//                                             result.VoteAbstain = false;
//                                             result.VoteVoid = false;
//                                         }
//                                         else if (post.response == "AGAINST")
//                                         {
//                                             result.VoteAgainst = true;
//                                             result.VoteFor= false;
//                                             result.VoteAbstain = false;
//                                             result.VoteVoid = false;
//                                         }
//                                         else if (post.response == "ABSTAIN")
//                                         {
//                                             result.VoteAbstain = true;
//                                             result.VoteFor = false;
//                                             result.VoteAgainst = false;
//                                             result.VoteVoid = false;
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
//                                         Message = "Success"
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
//                                         var checkresult = db.Result.FirstOrDefault(r => r.ShareholderNum == voter.ShareholderNum && r.QuestionId == post.Id);
//                                         //if true - edit value
//                                         if (checkresult != null)
//                                         {
//                                             checkresult.date = DateTime.Now;
//                                             checkresult.Timestamp = DateTime.Now.TimeOfDay;
//                                             checkresult.VoteStatus = "Voted";
//                                             checkresult.Source = "Mobile";
//                                             if (post.response == "FOR")
//                                             {
//                                                 checkresult.VoteFor = true;
//                                                 checkresult.VoteAgainst = false;
//                                                 checkresult.VoteAbstain = false;
//                                                 checkresult.VoteVoid = false;
//                                             }
//                                             else if (post.response == "AGAINST")
//                                             {
//                                                 checkresult.VoteAgainst = true;
//                                                 checkresult.VoteFor = false;
//                                                 checkresult.VoteAbstain = false;
//                                                 checkresult.VoteVoid = false;
//                                             }
//                                             else if (post.response == "ABSTAIN")
//                                             {
//                                                 checkresult.VoteAbstain = true;
//                                                 checkresult.VoteFor = false;
//                                                 checkresult.VoteAgainst = false;
//                                                 checkresult.VoteVoid = false;
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
//                                             result.Source = "Mobile";
//                                             result.Present = true;
//                                             if (post.response == "FOR")
//                                             {
//                                                 result.VoteFor = true;
//                                                 result.VoteAgainst = false;
//                                                 result.VoteAbstain = false;
//                                                 result.VoteVoid = false;
//                                             }
//                                             else if (post.response == "AGAINST")
//                                             {
//                                                 result.VoteAgainst = true;
//                                                 result.VoteFor = false;
//                                                 result.VoteAbstain = false;
//                                                 result.VoteVoid = false;
//                                             }
//                                             else if (post.response == "ABSTAIN")
//                                             {
//                                                 result.VoteAbstain = true;
//                                                 result.VoteFor = false;
//                                                 result.VoteAgainst = false;
//                                                 result.VoteVoid = false;
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
//                                         Message = "Success"
//                                     });
//                                     //var sucoutput = JsonConvert.SerializeObject(successresponse);

//                                     //return new HttpResponseMessage()
//                                     //{
//                                     //    Content = new StringContent(sucoutput, System.Text.Encoding.UTF8, "application/json")
//                                     //};

//                                 }

//                                 //Voter not marked present at AGM
//                                 return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                                 {
//                                     Code = "505",
//                                     Message = "Voter not marked present at AGM"
//                                 });
//                                 //var voteroutput = JsonConvert.SerializeObject(voterresponse);

//                                 //return new HttpResponseMessage()
//                                 //{
//                                 //    Content = new StringContent(voteroutput, System.Text.Encoding.UTF8, "application/json")
//                                 //};
//                             }
//                             //Wait for voting to commence on this resolution

//                             return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                             {
//                                 Code = "400",
//                                 Message = "Wait for voting to commence on this resolution"
//                             });
//                             //var resolutiontimeoutput = JsonConvert.SerializeObject(resolutiontimeresponse);

//                             //return new HttpResponseMessage()
//                             //{
//                             //    Content = new StringContent(resolutiontimeoutput, System.Text.Encoding.UTF8, "application/json")
//                             //};
//                         }
//                         //Voting clock has stopped

//                         return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                         {
//                             Code = "406",
//                             Message = "Voting clock has stopped"

//                         });
//                     }

//                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                     {
//                         Code = "500",
//                         Message = "Failed Request"
//                     });
//                     //var timeoutput = JsonConvert.SerializeObject(timeoutresponse);

//                     //return new HttpResponseMessage()
//                     //{
//                     //    Content = new StringContent(timeoutput, System.Text.Encoding.UTF8, "application/json")
//                     //};
//                 }
//                 else
//                 {
//                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                     {
//                         Code = "404",
//                         Message = "Empty Request"
                        
//                     });

//                     //var output = JsonConvert.SerializeObject(httpresponse);

//                     //return new HttpResponseMessage()
//                     //{
//                     //    Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
//                     //};
//                 }
               

//             }
//             catch(Exception e)
//             {
//                 return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                 {
//                     Code = "500",
//                     Message = "Failed Request"
//                 });
//                 //var error = JsonConvert.SerializeObject(Errorresponse);

//                 //return new HttpResponseMessage()
//                 //{
//                 //    Content = new StringContent(error, System.Text.Encoding.UTF8, "application/json")
//                 //};
//             }

//         }





//         //// PUT api/votecollectionapi/5
//         //public void Put(int id, [FromBody]string value)
//         //{
//         //}

//         //// DELETE api/votecollectionapi/5
//         //public void Delete(int id)
//         //{
//         //}
//     }
// }
