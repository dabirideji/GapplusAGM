// using BarcodeGenerator.Models;
// using BarcodeGenerator.Models.ModelDTO;
// using BarcodeGenerator.Service;
// using Swashbuckle.Swagger.Annotations;
// using System;
// using System.Collections.Generic;
// using System.Data.Entity;
// using System.Data.Entity.Infrastructure;
// using System.Diagnostics;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Threading.Tasks;
// using System.Web.Http;

// namespace BarcodeGenerator.Controllers
// {
//     public class VotingAPIController : ApiController
//     {
//         UsersContext db = new UsersContext();
//         Stopwatch stopwatch = new Stopwatch();
//         UserAdmin ua = new UserAdmin();

//         private static string currentYear = DateTime.Now.Year.ToString();

//         // GET api/votingapi
//         ///<summary>
//         /// This API confirms if Abstain Button should be present during voting or not.
//         ///</summary>
//         ///<returns>
//         ///</returns>
//         [HttpGet]
//         [SwaggerResponse(HttpStatusCode.OK, Type = typeof(HttpRequestResponse))]
//         [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(HttpRequestResponse))]
//         [SwaggerResponse(200, "ABSTAIN Button choice is true", Type = typeof(HttpRequestResponse))]
//         [SwaggerResponse(201, "ABSTAIN button choice is false", Type = typeof(HttpRequestResponse))]
//         [SwaggerResponse(500, "Server Error", Type = typeof(VotingResponse))]
//         public async Task<HttpRequestResponse> AbstainButtonChoice(string company)
//         {
//             var response = await GetAbstainButtonChoiceAsync(company);

//             return response;
//         }

//         private Task<HttpRequestResponse> GetAbstainButtonChoiceAsync(string company)
//         {
//             try
//             {
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
//                 if (UniqueAGMId == -1)
//                 {
//                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                     {
//                         Code = "400",
//                         Message = "Bad Request"
//                     });
//                 }
//                 var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
//                 var abstainBtnchoice = true;

//                 if (agmEvent == null && agmEvent.AbstainBtnChoice == null)
//                 {
//                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                     {
//                         Code = "400",
//                         Message = "Bad Request"
//                     });
//                 }
//                 abstainBtnchoice = (bool)agmEvent.AbstainBtnChoice;
//                 if (abstainBtnchoice)
//                 {
//                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                     {
//                         Code = "200",
//                         Message = "ABSTAIN Button choice is true"
//                     });
//                 }
//                 return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                 {
//                     Code = "201",
//                     Message = "ABSTAIN button choice is false"
//                 });
//             }
//             catch(Exception e)
//             {
//                 return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                 {
//                     Code = "500",
//                     Message = "Server Error"
//                 });
//             }
           
//         }


//         [HttpPost]
//         [SwaggerResponse(HttpStatusCode.OK, Type = typeof(VotingResponse))]
//         [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(VotingResponse))]
//         [SwaggerResponse(404, "Channel not enabled for voting", Type = typeof(VotingResponse))]
//         [SwaggerResponse(200, "Voting has commenced", Type = typeof(VotingResponse))]
//         [SwaggerResponse(202, "Voting has Stopped for this event", Type = typeof(VotingResponse))]
//         [SwaggerResponse(203, "Account not present or account is proxy.", Type = typeof(VotingResponse))]
//         [SwaggerResponse(201, "Voting has not commenced", Type = typeof(VotingResponse))]
//         [SwaggerResponse(500, "Server Error", Type = typeof(VotingResponse))]
//         public async Task<VotingResponse> CheckVotingCommencement([FromBody]VotingStatusDTO post)
//         {
//             var response = await GetVotingCommencementAsync(post.Company, post.Email);

//             return response;
//         }

//         // GET api/votingapi
//         ///<summary>
//         /// Check if voting has commenced by passing the company name as parameter.
//         ///</summary>
//         ///<returns>
//         ///</returns>
//         //[HttpGet]
//         //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(VotingResponse))]
//         //[SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(VotingResponse))]
//         //[SwaggerResponse(404, "Channel not enabled for voting", Type = typeof(VotingResponse))]
//         //[SwaggerResponse(200, "Voting has commenced", Type = typeof(VotingResponse))]
//         //[SwaggerResponse(202, "Voting has Stopped for this event", Type = typeof(VotingResponse))]
//         //[SwaggerResponse(201, "Voting has not commenced", Type = typeof(VotingResponse))]
//         //[SwaggerResponse(500, "Server Error", Type = typeof(VotingResponse))]
//         //public async Task<VotingResponse> CheckVotingCommencement(string company)
//         //{
//         //    var response = await GetVotingCommencementAsync(company);

//         //    return response;
//         //}

//         private Task<VotingResponse> GetVotingCommencementAsync(string company, string email)
//         {
//             try
//             {
//                 VotingResponse httpresponse;
//                 var companyinfo = company;
//                 if (string.IsNullOrEmpty(companyinfo)||string.IsNullOrEmpty(email))
//                 {
//                     httpresponse = new VotingResponse
//                     {
//                         Code = "400",
//                         Message = "Meeting Request is Unavailable"
//                     };
//                     return Task.FromResult<VotingResponse>(httpresponse);
//                 }

//                 //var companyExist = TimerControll.GetCompanyExist(companyinfo);
//                 //if (companyExist == false)
//                 //{
//                 //    httpresponse = new VotingResponse
//                 //    {
//                 //        Code = "404",
//                 //        Message = "Company not listed for Voting"
//                 //    };
//                 //    return Task.FromResult<VotingResponse>(httpresponse);
//                 //}

//                 var UniqueAGMid = ua.RetrieveAGMUniqueID(companyinfo);
//                 if (UniqueAGMid == -1)
//                 {
//                     httpresponse = new VotingResponse
//                     {
//                         Code = "400",
//                         Message = "Meeting Request is Unavailable"
//                     };
//                     return Task.FromResult<VotingResponse>(httpresponse);
//                 }
//                 var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMid);
//                 if (agmEvent == null)
//                 {
//                     httpresponse = new VotingResponse
//                     {
//                         Code = "400",
//                         Message = "Meeting Request is Unavailable"
//                     };
//                     return Task.FromResult<VotingResponse>(httpresponse);
//                 }

//                 var CheckShareholder = db.Present.Any(p => p.emailAddress == email && p.AGMID == UniqueAGMid);
//                 if (!CheckShareholder)
//                 {
//                     httpresponse = new VotingResponse
//                     {
//                         Code = "203",
//                         Message = "Your account is not registered for this event."
//                     };
//                     return Task.FromResult<VotingResponse>(httpresponse);
//                 }

//                 var ShareholderForProxy = db.Present.Any(p => p.emailAddress == email && p.AGMID == UniqueAGMid && p.proxy==true);
//                 if (ShareholderForProxy)
//                 {
//                     httpresponse = new VotingResponse
//                     {
//                         Code = "203",
//                         Message = "Your account has been registered as Proxy. You will not be able to participate in live Voting."
//                     };
//                     return Task.FromResult<VotingResponse>(httpresponse);
//                 }


//                 if (!agmEvent.allChannels || !agmEvent.mobileChannel )
//                 {
//                     httpresponse = new VotingResponse
//                     {
//                         Code = "404",
//                         Message = "Mobile Channel not enabled for voting"
//                     };
//                     return Task.FromResult<VotingResponse>(httpresponse);
//                 }
//                 else
//                 {
//                     //if (TimerControll.GetTimeStatus(companyinfo) == true)
//                     //{
//                     //    httpresponse = new VotingResponse
//                     //    {
//                     //        Code = "200",
//                     //        Message = "Voting has commenced"
//                     //    };
//                     //    return Task.FromResult<VotingResponse>(httpresponse);

//                     //}
//                     if (agmEvent.StartVoting == true)
//                     {
//                         httpresponse = new VotingResponse
//                         {
//                             Code = "200",
//                             Message = "Voting has commenced"
//                         };
//                         return Task.FromResult<VotingResponse>(httpresponse);

//                     }
//                     else if(agmEvent.StopVoting == true)
//                     {
//                         httpresponse = new VotingResponse
//                         {
//                             Code = "202",
//                             Message = "Voting has Stopped for this Event"
//                         };
//                         return Task.FromResult<VotingResponse>(httpresponse);
//                     }
//                     else
//                     {
//                         httpresponse = new VotingResponse
//                         {
//                             Code = "201",
//                             Message = "Voting has not commenced"
//                         };

//                         return Task.FromResult<VotingResponse>(httpresponse);
//                     }

//                 }


//             }
//             catch (Exception e)
//             {
//                 VotingResponse httpresponse = new VotingResponse
//                 {
//                     Code = "500",
//                     Message = "Service request failed at this time."
//                 };

//                 return Task.FromResult<VotingResponse>(httpresponse);
//             }
//         }



//         ///<summary>
//         /// By passing the company parameter, this API provides a list of AGM Resolutions.
//         ///</summary>
//         ///<returns>
//         ///</returns>
//         [HttpGet]
//         [SwaggerResponse(HttpStatusCode.OK, Type = typeof(HttpRequestResponse))]
//         [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(HttpRequestResponse))]
//         [SwaggerResponse(200, "Success", Type = typeof(HttpRequestResponse))]
//         [SwaggerResponse(203, "Voting has ended.", Type = typeof(HttpRequestResponse))]
//         [SwaggerResponse(500, "Server Error", Type = typeof(HttpRequestResponse))]
//         public async Task<HttpRequestResponse> GetResolutionRequest(string company)
//             {
//                 var response = await GetResolutionRequestAsync(company);

//                 return response;
//             }


//             private Task<HttpRequestResponse> GetResolutionRequestAsync(string company)
//             {
//                 try
//                 {
//                 if (string.IsNullOrEmpty(company))
//                 {
//                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                     {
//                         Code = "400",
//                         Message = "Resolution unavailable for this Request"
//                     });
//                 }
//                     var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
//                     if (UniqueAGMId == -1)
//                     {
//                         return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                         {
//                             Code = "400",
//                             Message = "Resolution unavailable for this Request"
//                         });
//                     }
//                 var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
//                 if (agmEvent.StopVoting == true)
//                 {
//                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                     {
//                         Code = "203",
//                         Message = "Voting has ended."
//                     });
//                 }
//                 var question = db.Question.Where(q => q.Company == company && q.AGMID == UniqueAGMId).ToList();
//                             var questionlist = from c in question
//                                                select new Resolutions()
//                                                {
//                                                    Id = c.Id,
//                                                    question = c.question,
//                                                    date = c.date,
//                                                    questionStatus = c.questionStatus
//                                                };

//                             return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                             {
//                                 Code = "200",
//                                 Message = "Success",
//                                 Content = questionlist
//                             });

//                 }
//                 catch (Exception e)
//                 {
//                     return Task.FromResult<HttpRequestResponse>(new HttpRequestResponse
//                     {
//                         Code = "500",
//                         Message = "Resolution request failed at this time."
//                     });
//                 }


//             }


//         [HttpPost]
//         [SwaggerResponse(HttpStatusCode.OK, Type = typeof(VotingResponse))]
//         [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(VotingResponse))]
//         [SwaggerResponse(400, "Request is denied", Type = typeof(VotingResponse))]
//         [SwaggerResponse(404, "Channel not enabled for voting", Type = typeof(VotingResponse))]
//         [SwaggerResponse(200, "Resolution is active", Type = typeof(VotingResponse))]
//         [SwaggerResponse(203, "Voting has ended for this event", Type = typeof(VotingResponse))]
//         [SwaggerResponse(201, "Wait for voting to commence on this resolution", Type = typeof(VotingResponse))]
//         [SwaggerResponse(500, "Server Error", Type = typeof(VotingResponse))]
//         public async Task<VotingResponse> CheckResolutionStatus([FromBody] ResolutionStatusDTO post)
//         {
//             var response = await CheckResolutionStatusAsync(post);

//             return response;
//         }

//         private Task<VotingResponse> CheckResolutionStatusAsync(ResolutionStatusDTO post)
//         {
//             try
//             {
//                 if (!ModelState.IsValid)
//                 {
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "400",
//                         Message = "Request is denied"
//                     });
//                 }

//                 if (string.IsNullOrEmpty(post.company) || string.IsNullOrWhiteSpace(post.company))
//                 {
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "400",
//                         Message = "Request is denied"

//                     });
//                 }
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID(post.company);
//                 if (UniqueAGMId == -1)
//                 {
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "400",
//                         Message = "Request is denied"
//                     });
//                 }

//                 var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

//                 if (agmEvent == null)
//                 {
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "400",
//                         Message = "Request is denied"
//                     });
//                 }

//                 if (agmEvent.StopVoting == true)
//                 {
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "203",
//                         Message = "Voting has ended for this event."
//                     });
//                 }

//                 if (agmEvent.allChannels != true && agmEvent.mobileChannel != true)
//                 {
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "400",
//                         Message = "Channel not Enabled for voting."
//                     });
//                 }
             
//                 var question = db.Question.Find(post.resolutionid);
//                 if (question != null && !question.questionStatus)
//                 {
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "201",
//                         Message = "Wait for voting to commence on this resolution"
//                     });
//                 }
//                 else
//                 {

//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "200",
//                         Message = "Success"
//                     });

//                 }

//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<VotingResponse>(new VotingResponse
//                 {
//                     Code = "500",
//                     Message = "Request Failed at this time."
//                 });
//             }
//         }



//         ///<summary>
//         /// This API takes voter's response payload. Payload parameter "response": "FOR"||"AGAINST"||"ABSTAIN"
//         ///</summary>
//         ///<returns>
//         ///</returns>
//         [HttpPost]
//         [SwaggerResponse(HttpStatusCode.OK, Type = typeof(VotingResponse))]
//         [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(VotingResponse))]
//         [SwaggerResponse(200, "Success", Type = typeof(VotingResponse))]
//         [SwaggerResponse(203, "Voting has ended for this event", Type = typeof(VotingResponse))]
//         [SwaggerResponse(404, "Generic Message", Type = typeof(VotingResponse))]
//         [SwaggerResponse(500, "Server Error", Type = typeof(VotingResponse))]        
//         public async Task<VotingResponse> VotingResponse([FromBody]VoteModelDTO post)
//         {
//             var response = await VotingResponseAsync(post);

//             return response;
//         }



//         private Task<VotingResponse> VotingResponseAsync(VoteModelDTO post)
//         {
//             try
//             {
//                 if (!ModelState.IsValid)
//                 {
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "400",
//                         Message = "Voting Request is denied"
//                     });
//                 }

//                 if (string.IsNullOrEmpty(post.company) || string.IsNullOrWhiteSpace(post.company))
//                 {
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "400",
//                         Message = "Voting Request is denied"

//                     });
//                 }
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID(post.company);
//                 if (UniqueAGMId == -1)
//                 {
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "400",
//                         Message = "Voting Request is denied"
//                     });
//                 }

//                         var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

//                 if (agmEvent == null)
//                 {
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "400",
//                         Message = "Voting Request is denied"
//                     });
//                 }

//                 if (agmEvent.StopVoting == true)
//                 {
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "203",
//                         Message = "Voting has ended for this event."
//                     });
//                 }
//                 if (agmEvent.allChannels != true && agmEvent.mobileChannel != true)
//                             {
//                                 return Task.FromResult<VotingResponse>(new VotingResponse
//                                 {
//                                     Code = "404",
//                                     Message = "Channel not Enabled for voting."
//                                 });
//                             }


//                         var abstainBtnchoice = true;

//                         if (agmEvent != null && agmEvent.AbstainBtnChoice != null)
//                         {
//                             abstainBtnchoice = (bool)agmEvent.AbstainBtnChoice;
//                         }

//                         if (abstainBtnchoice == false && post.response == "ABSTAIN")
//                         {
//                             return Task.FromResult<VotingResponse>(new VotingResponse
//                             {
//                                 Code = "404",
//                                 Message = "ABSTAIN is not a choice."
//                             });
//                         }

//                 if (!TimerControll.GetTimeStatus(post.company))
//                 {
//                     //Voting clock has stopped
//                     return Task.FromResult<VotingResponse>(new VotingResponse
//                     {
//                         Code = "404",
//                         Message = "Voting clock has stopped"

//                     });
//                 }

//                     var question = db.Question.Find(post.resolutionid);
//                     if (question != null)
//                     {
//                         if (!question.questionStatus)
//                         {
//                             return Task.FromResult<VotingResponse>(new VotingResponse
//                             {
//                                 Code = "404",
//                                 Message = "Wait for voting to commence on this resolution"
//                             });
//                         }

//                     }
//                                 //var voter = db.Present.SingleOrDefault(q => q.emailAddress == post.identity && q.present == true);                         
//                                 var voters = db.Present.Where(q => q.AGMID == UniqueAGMId && q.emailAddress == post.emailAddress && q.present == true).ToList();

//                                 if (voters.Count() == 1)
//                                 {
//                                     var votee = voters.FirstOrDefault();

//                                     var voter = db.Present.Find(votee.Id);
//                                     var checkresult = db.Result.FirstOrDefault(r => r.ShareholderNum == voter.ShareholderNum && r.QuestionId == post.resolutionid);
//                                     //if true - edit value
//                                     if (checkresult != null)
//                                     {
//                                         checkresult.date = DateTime.Now;
//                                         checkresult.Timestamp = DateTime.Now.TimeOfDay;
//                                         checkresult.VoteStatus = "Voted";
//                                         checkresult.Source = "Mobile";
//                                         if (post.response.ToUpper() == "FOR")
//                                         {
//                                             checkresult.VoteFor = true;
//                                             checkresult.VoteAgainst = false;
//                                             checkresult.VoteAbstain = false;
//                                             checkresult.VoteVoid = false;
//                                         }
//                                         else if (post.response.ToUpper() == "AGAINST")
//                                         {
//                                             checkresult.VoteAgainst = true;
//                                             checkresult.VoteFor = false;
//                                             checkresult.VoteAbstain = false;
//                                             checkresult.VoteVoid = false;
//                                         }
//                                         else if (post.response.ToUpper() == "ABSTAIN")
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

//                                         return Task.FromResult<VotingResponse>(new VotingResponse
//                                         {
//                                             Code = "200",
//                                             Message = "Success"
//                                         });

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
//                                         result.QuestionId = post.resolutionid;
//                                         result.date = DateTime.Now;
//                                         result.Timestamp = DateTime.Now.TimeOfDay;
//                                         result.VoteStatus = "Voted";
//                                         result.Source = "Mobile";
//                                         result.Present = true;
//                                         if (post.response.ToUpper() == "FOR")
//                                         {
//                                             result.VoteFor = true;
//                                             result.VoteAgainst = false;
//                                             result.VoteAbstain = false;
//                                             result.VoteVoid = false;
//                                         }
//                                         else if (post.response.ToUpper() == "AGAINST")
//                                         {
//                                             result.VoteAgainst = true;
//                                             result.VoteFor = false;
//                                             result.VoteAbstain = false;
//                                             result.VoteVoid = false;
//                                         }
//                                         else if (post.response.ToUpper() == "ABSTAIN")
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

//                                     return Task.FromResult<VotingResponse>(new VotingResponse
//                                     {
//                                         Code = "200",
//                                         Message = "Success"
//                                     });

//                                 }
//                                 else if (voters.Count() > 1)
//                                 {
//                                     foreach (var votee in voters)
//                                     {
//                                         var voter = db.Present.Find(votee.Id);
//                                         var checkresult = db.Result.FirstOrDefault(r => r.ShareholderNum == voter.ShareholderNum && r.QuestionId == post.resolutionid);
//                                         //if true - edit value
//                                         if (checkresult != null)
//                                         {
//                                             checkresult.date = DateTime.Now;
//                                             checkresult.Timestamp = DateTime.Now.TimeOfDay;
//                                             checkresult.VoteStatus = "Voted";
//                                             checkresult.Source = "Mobile";
//                                             if (post.response.ToUpper() == "FOR")
//                                             {
//                                                 checkresult.VoteFor = true;
//                                                 checkresult.VoteAgainst = false;
//                                                 checkresult.VoteAbstain = false;
//                                                 checkresult.VoteVoid = false;
//                                             }
//                                             else if (post.response.ToUpper() == "AGAINST")
//                                             {
//                                                 checkresult.VoteAgainst = true;
//                                                 checkresult.VoteFor = false;
//                                                 checkresult.VoteAbstain = false;
//                                                 checkresult.VoteVoid = false;
//                                             }
//                                             else if (post.response.ToUpper() == "ABSTAIN")
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
//                                             result.QuestionId = post.resolutionid;
//                                             result.date = DateTime.Now;
//                                             result.Timestamp = DateTime.Now.TimeOfDay;
//                                             result.VoteStatus = "Voted";
//                                             result.Source = "Mobile";
//                                             result.Present = true;
//                                             if (post.response.ToUpper() == "FOR")
//                                             {
//                                                 result.VoteFor = true;
//                                                 result.VoteAgainst = false;
//                                                 result.VoteAbstain = false;
//                                                 result.VoteVoid = false;
//                                             }
//                                             else if (post.response.ToUpper() == "AGAINST")
//                                             {
//                                                 result.VoteAgainst = true;
//                                                 result.VoteFor = false;
//                                                 result.VoteAbstain = false;
//                                                 result.VoteVoid = false;
//                                             }
//                                             else if (post.response.ToUpper() == "ABSTAIN")
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

//                                     return Task.FromResult<VotingResponse>(new VotingResponse
//                                     {
//                                         Code = "200",
//                                         Message = "Success"
//                                     });

//                                 }

//                                 //Voter not marked present at AGM
//                                 return Task.FromResult<VotingResponse>(new VotingResponse
//                                 {
//                                     Code = "404",
//                                     Message = "Voter not marked present at AGM"
//                                 });


//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<VotingResponse>(new VotingResponse
//                 {
//                     Code = "500",
//                     Message = "Voting Request Failed at this time."
//                 });
//             }

//         }

//     }
// }