using BarcodeGenerator.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
//using System.Web.Mvc;
using System.Diagnostics;
using System.Threading;
using System.Data.Entity;
using BarcodeGenerator.Service;
using System.Web.Http.Description;

namespace BarcodeGenerator.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class VoteAPIController : ApiController
    {
        UsersContext db = new UsersContext();
        Stopwatch stopwatch = new Stopwatch();
        UserAdmin ua = new UserAdmin();
        // GET api/voteapi


        [HttpPost]
        public async Task<HttpRequestResponse> GetVotingCommencement([FromBody] VoteModel post)
        {
            var response = await GetVotingCommencementAsync(post);

            return response;
        }

        private Task<HttpRequestResponse> GetVotingCommencementAsync([FromBody]VoteModel post)
        {
            try
            {

                var companyinfo = post.company;
                if (!string.IsNullOrEmpty(companyinfo))
                {
                    var companyExist = TimerControll.GetCompanyExist(companyinfo);
                    if(companyExist == false)
                    {
                        HttpRequestResponse httpresponse = new HttpRequestResponse
                        {
                            Code = "500",
                            Message = "Failed Request"
                        };
                    }

                    var UniqueAGMid = ua.RetrieveAGMUniqueID(companyinfo);
                    if (UniqueAGMid == -1)
                    {
                        HttpRequestResponse httpresponse = new HttpRequestResponse
                        {
                            Code = "500",
                            Message = "Failed Request"
                        };
                        return Task.FromResult<HttpRequestResponse>(httpresponse);
                    }
                        var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMid);
                    if (agmEvent == null)
                    {
                        HttpRequestResponse httpresponse = new HttpRequestResponse
                        {
                            Code = "500",
                            Message = "Failed Request"
                        };
                        return Task.FromResult<HttpRequestResponse>(httpresponse);
                    }
                            if(agmEvent.allChannels!=true || agmEvent.mobileChannel!=true)
                            {
                                HttpRequestResponse httpresponse = new HttpRequestResponse
                                {
                                    Code = "500",
                                    Message = "Channel not enabled for voting"
                                };
                                return Task.FromResult<HttpRequestResponse>(httpresponse);
                             }  
                            else
                            {
                                if (TimerControll.GetTimeStatus(companyinfo) == true)
                                {
                                    HttpRequestResponse httpresponse = new HttpRequestResponse
                                    {
                                        Code = "200",
                                        Message = "Voting has commenced"
                                    };
                                    return Task.FromResult<HttpRequestResponse>(httpresponse);

                                }
                                else
                                {
                                    HttpRequestResponse httpresponse = new HttpRequestResponse
                                    {
                                        Code = "406",
                                        Message = "Voting has not commenced"
                                    };

                                    return Task.FromResult<HttpRequestResponse>(httpresponse);
                                }

                            }
                    
                }
                else
                {
                    HttpRequestResponse httpresponse = new HttpRequestResponse
                    {
                        Code = "404",
                        Message = "Empty Request"
                    };

                    return Task.FromResult<HttpRequestResponse>(httpresponse);
                }

            }
            catch(Exception e)
            {
                HttpRequestResponse httpresponse = new HttpRequestResponse
                {
                    Code = "500",
                    Message = "Failed Request"
                };

                return Task.FromResult<HttpRequestResponse>(httpresponse);
            }
           

        }

        //// GET api/voteapi/5
        //public string Get(int id)
        //{
        //    return "value";
        //}
        [HttpPost]
        // POST api/voteapi
        public HttpResponseMessage Post([FromBody]VoteModel post)
        {
            if(!string.IsNullOrEmpty(post.company))
            {
                if (TimerControll.GetTimeStatus(post.company) == true)
                {
                    HttpRequestResponse httpresponse = new HttpRequestResponse
                    {
                        Code = "200",
                        Message = "Voting has commenced"
                    };

                    //return Request.CreateResponse(HttpStatusCode.OK, "Thank You");

                    var output = JsonConvert.SerializeObject(httpresponse);

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
                    };

                }
                else
                {
                    HttpRequestResponse httpresponse = new HttpRequestResponse
                    {
                        Code = "406",
                        Message = "Voting has not commenced"
                    };

                    var output = JsonConvert.SerializeObject(httpresponse);

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
                    };
                    //return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Voting has not commenced");
                }
            }
            else
            {
                HttpRequestResponse httpresponse = new HttpRequestResponse
                {
                    Code = "404",
                    Message = "Empty Request"
                };

                var output = JsonConvert.SerializeObject(httpresponse);

                return new HttpResponseMessage()
                {
                    Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
                };
                //return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Voting has not commenced");
            }

        }

        public async Task<HttpResponseMessage> CheckVotingCommencement(VoteModel post)
        {
            var response = await CheckVotingCommencementAsync(post);

            return response;
        }

        public Task<HttpResponseMessage> CheckVotingCommencementAsync(VoteModel post)
        {
            if (!string.IsNullOrEmpty(post.company))
            {
                if (TimerControll.GetTimeStatus(post.company) == true)
                {
                    HttpRequestResponse httpresponse = new HttpRequestResponse
                    {
                        Code = "200",
                        Message = "Voting has commenced"
                    };

                    //return Request.CreateResponse(HttpStatusCode.OK, "Thank You");

                    var output = JsonConvert.SerializeObject(httpresponse);

                    return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage()
                    {
                        Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
                    });

                }
                else
                {
                    HttpRequestResponse httpresponse = new HttpRequestResponse
                    {
                        Code = "406",
                        Message = "Voting has not commenced"
                    };

                    var output = JsonConvert.SerializeObject(httpresponse);

                    return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage()
                    {
                        Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
                    });
                    //return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Voting has not commenced");
                }
            }
            else
            {
                HttpRequestResponse httpresponse = new HttpRequestResponse
                {
                    Code = "404",
                    Message = "Empty Request"
                };

                var output = JsonConvert.SerializeObject(httpresponse);

                return Task.FromResult<HttpResponseMessage>(new HttpResponseMessage()
                {
                    Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json")
                });
                //return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Voting has not commenced");
            }
        }
        //// PUT api/voteapi/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/voteapi/5
        //public void Delete(int id)
        //{
        //}
    }
}
