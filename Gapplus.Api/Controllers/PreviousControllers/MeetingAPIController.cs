using BarcodeGenerator.Barcode;
using BarcodeGenerator.Models;
using BarcodeGenerator.Models.ModelDTO;
using BarcodeGenerator.Service;
using BarcodeGenerator.Util;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BarcodeGenerator.Controllers
{
    public class MeetingAPIController : ApiController
    {
        UsersContext db = new UsersContext();
        UserAdmin ua = new UserAdmin();
        AGMRegistrationService _AGMService = new AGMRegistrationService();
        // GET api/<controller>

        ///<summary>
        /// Obtain QRCode 
        ///</summary>
        ///<returns>
        ///</returns>
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GenericAPIResponseDTO<string>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(GenericAPIResponseDTO<string>))]
        [SwaggerResponse(200, "Success", Type = typeof(GenericAPIResponseDTO<string>))]
        [SwaggerResponse(500, "Server Error", Type = typeof(GenericAPIResponseDTO<string>))]
        [HttpGet]
        public async Task<GenericAPIResponseDTO<string>> GetQRCode(string emailAddress)
        {
            var response = await GetQRCodeAsync(emailAddress);

            return response;
        }

        private Task<GenericAPIResponseDTO<string>> GetQRCodeAsync(string emailAddress)
        {
            try
            {
                var qrgenerator = new Qrcode();
                //byte[] qrcode;
                string qrcode;
                var shareholder = db.BarcodeStore.FirstOrDefault(s => s.emailAddress == emailAddress);
                if(shareholder==null)
                {
                    return Task.FromResult<GenericAPIResponseDTO<string>>(
                        new GenericAPIResponseDTO<string>
                        {
                            responseCode = "400",
                            responseMessage = "Meeting QRCode Unavailable for this account.",
                            responseData = ""
                        });
                }

                if (string.IsNullOrEmpty(shareholder.ImageUrl))
                {
                    qrcode = qrgenerator.GenerateMyQCCode(emailAddress, shareholder.ShareholderNum.ToString());
                    shareholder.ImageUrl = qrcode;
                    db.SaveChanges();
                }
                else
                {
                    qrcode = shareholder.ImageUrl;
                }
                // string ImageUrl = qrcode != null ? "data:image/jpg;base64," +
                //Convert.ToBase64String(qrcode) : "";
                string ImageUrl = !string.IsNullOrEmpty(qrcode) ? qrcode : "";
                return Task.FromResult<GenericAPIResponseDTO<string>>(
                    new GenericAPIResponseDTO<string>
                    {
                        responseCode = "200",
                        responseMessage = "Success",
                        responseData = ImageUrl
                    });

            }
            catch (Exception e)
            {
                return Task.FromResult<GenericAPIResponseDTO<string>>(
                    new GenericAPIResponseDTO<string>
                    {
                        responseCode = "500",
                        responseMessage = "Server Failure",
                        responseData = ""
                    });
            }

        }

        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(JoinVotingResponse))]      
        [SwaggerResponse(205, "Generic API Error", Type = typeof(JoinVotingResponse))]
        [SwaggerResponse(200, "Login Success", Type = typeof(JoinVotingResponse))]
        public async Task<JoinVotingResponse> JoinVotingOnMobile([FromBody] JoinVotingDTO post)
        {
            var response = await JoinVotingOnMobileAsync(post.Company, post.ShareholderNum);

            return response;

        }


        private Task<JoinVotingResponse> JoinVotingOnMobileAsync(string companyinfo, string ShareholderNum)
        {
            try
            {
                JoinVotingResponse ResponseMessage;

                //var votingStatus = TimerControll.GetTimeStatus(companyinfo);

                if (string.IsNullOrEmpty(ShareholderNum) && string.IsNullOrEmpty(companyinfo))
                {

                    //Return error code;
                    ResponseMessage = new JoinVotingResponse()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "LOGIN REQUEST FAILED. SHAREHOLDER NUMBER OR COMPANY INFO MAY BE INCORRECT",
                 
                    };
                    return Task.FromResult(ResponseMessage);
                }
                long shareholderNum;
                long shareholdernum;
                if (Int64.TryParse(ShareholderNum, out shareholderNum))
                {
                    shareholdernum = shareholderNum;
                }
                else
                {
                    //Return error code;
                    ResponseMessage = new JoinVotingResponse()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "LOGIN REQUEST FAILED. SHAREHOLDER NUMBER MAY BE INCORRECT",

                    };
                    return Task.FromResult(ResponseMessage);
                }
                //var shareholdernum = Int64.Parse(ShareholderNum);
                var Uniagmid = ua.RetrieveAGMUniqueID(companyinfo);
                if(Uniagmid == -1)
                {
                    ResponseMessage = new JoinVotingResponse()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "LOGIN REQUEST FAILED. COMPANY INFO MAY BE INCORRECT",

                    };
                    return Task.FromResult(ResponseMessage);
                }
                var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == Uniagmid);
                if (agmEvent.StopVoting == true)
                {
                    ResponseMessage = new JoinVotingResponse()
                    {
                        ResponseCode = "200",
                        ResponseMessage = "Success",
                        VotingEnded = agmEvent.StartVoting
                    };
                    return Task.FromResult<JoinVotingResponse>(ResponseMessage);
                }
                var Shareholder = db.Present.FirstOrDefault(p => p.ShareholderNum == shareholdernum && p.AGMID==Uniagmid);
                if (Shareholder == null)
                {
                    ResponseMessage = new JoinVotingResponse()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "LOGIN REQUEST FAILED. SHAREHOLDER NUMBER NOT REGISTERED",

                    };
                    return Task.FromResult(ResponseMessage);
                }
                var CheckOtherAccounts = db.Present.Where(p => p.emailAddress == Shareholder.emailAddress).ToList();
                bool isAnyAccountProxy = false;
                if (CheckOtherAccounts != null)
                {
                    isAnyAccountProxy = CheckOtherAccounts.Any(c => c.proxy == true);
                }

                //if (Shareholder == null)
                //{
                //    //Return error code;
                //    ResponseMessage = new JoinVotingResponse()
                //    {
                //        ResponseCode = "205",
                //        ResponseMessage = "LOGIN REQUEST FAILED. SHAREHOLDER NOT ADMINISTERED",                 
                //    };
                //    return Task.FromResult(ResponseMessage);
                //}

                //if (TimerControll.GetTimeStatus(companyinfo))
                //{
               
                if (agmEvent == null)
                {
                    //Return error code;
                    ResponseMessage = new JoinVotingResponse()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "LOGIN REQUEST FAILED. EVENT ID MISSING",
                       
                    };
                    return Task.FromResult(ResponseMessage);
                }
                else if (isAnyAccountProxy)
                {
                    //Shareholer not permitted to vote.
                    ResponseMessage = new JoinVotingResponse()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "LOGIN REQUEST FAILED. SHAREHOLDER MAY HAVE PARTICIPATED BY PROXY VOTES.",
                    
                    };
                    return Task.FromResult(ResponseMessage);
                }
                else if ((agmEvent.mobileChannel || agmEvent.allChannels))
                {
                    //Send Active Resolution to Client
                    var resolution = EventResolution(Uniagmid);
                    ResponseMessage = new JoinVotingResponse()
                    {
                        ResponseCode = "200",
                        ResponseMessage = "Success",
                        Resolution = resolution,
                        VotingStatus = agmEvent.StartVoting
                    };
                    return Task.FromResult<JoinVotingResponse>(ResponseMessage);
                }
                else if ((agmEvent.mobileChannel || agmEvent.allChannels))
                {
                    //Voting has not commenced.
                    ResponseMessage = new JoinVotingResponse()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "LOGIN REQUEST FAILED. VOTING HAS BEEN DISABLED ON THIS CHANNEL",

                    };
                    return Task.FromResult(ResponseMessage);
                }
                else
                {
                    //voting not permitted on this channel
                    ResponseMessage = new JoinVotingResponse()
                    {
                        ResponseCode = "205",
                        ResponseMessage = "LOGIN REQUEST FAILED. VOTING HAS BEEN DISABLED ON THIS CHANNEL",

                    };
                    return Task.FromResult(ResponseMessage);
                }


            }
            catch (Exception e)
            {
                JoinVotingResponse ResponseMessage = new JoinVotingResponse()
                {
                    ResponseCode = "205",
                    ResponseMessage = "LOGIN REQUEST FAILED. REQUEST CANNOT BE PROCESSED AT THE MOMENT",
                 
                };
                return Task.FromResult(ResponseMessage);
            }
        }


        private List<Question> EventResolution(int agmid)
        {
            var votemodel = new VoteModel();
            var resolution = db.Question.Where(r => r.AGMID == agmid).ToList();
            if (resolution != null)
            {
                return resolution;
            }
            else
            {
                return new List<Question>();
            }
            
        }
        ///<summary>
        /// This API lists active meetings
        ///</summary>
        ///<returns>
        ///</returns>
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GenericAPIListResponseDTO<AGMCompaniesResponse>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(GenericAPIListResponseDTO<AGMCompaniesResponse>))]
        [SwaggerResponse(200, "Success", Type = typeof(GenericAPIListResponseDTO<AGMCompaniesResponse>))]
        [SwaggerResponse(500, "Server Error", Type = typeof(GenericAPIListResponseDTO<AGMCompaniesResponse>))]
        [HttpGet]
        public async Task<GenericAPIListResponseDTO<AGMCompanies>> GetActiveAGMCompanies()
        {
            var response = await GetActiveAGMCompaniesAsync();

            return response;
        }


        private Task<GenericAPIListResponseDTO<AGMCompanies>> GetActiveAGMCompaniesAsync()
        {
            try
            {
                var companyNameList = db.Settings.Where(s => s.ArchiveStatus == false).Select(o => new AGMCompanies{ company = o.CompanyName, description = o.Description, agmid = o.AGMID, RegCode=o.RegCode, venue = o.Venue, dateTime = o.AgmDateTime, EnddateTime = o.AgmEndDateTime }).Distinct().OrderBy(k => k.company).ToList();
               
                return Task.FromResult<GenericAPIListResponseDTO<AGMCompanies>>(
                    new GenericAPIListResponseDTO<AGMCompanies> {
                        responseCode = "200",
                        responseMessage = "Success",
                        responseData = companyNameList
                    });

            }
            catch (Exception e)
            {
                return Task.FromResult<GenericAPIListResponseDTO<AGMCompanies>>(
                    new GenericAPIListResponseDTO<AGMCompanies>
                    {
                        responseCode = "500",
                        responseMessage = "Server Failure",
                        responseData = new List<AGMCompanies>()
                    });
            }

        }


        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AGMCompaniesResponse))]
        [SwaggerResponse(200, "", Type = typeof(AGMCompaniesResponse))]
        [SwaggerResponse(201, "No Active AGM Available", Type = typeof(AGMCompaniesResponse))]
        [SwaggerResponse(500, "Server Error", Type = typeof(AGMCompaniesResponse))]
        public async Task<AGMCompaniesResponse> GetActiveAGMCompany(string company)
        {
            var response = await _AGMService.GetActiveAGMCompanyAsync(company);

            return response;
        }



        ///<summary>
        /// Obtain logged on user's access url to an active meeting.
        ///</summary>
        ///<returns>
        ///</returns>
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GenericAPIResponseDTO<AGMAccesscodeResponse>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(GenericAPIResponseDTO<AGMAccesscodeResponse>))]
        [SwaggerResponse(200, "Success", Type = typeof(GenericAPIResponseDTO<AGMAccesscodeResponse>))]
        [SwaggerResponse(500, "Server Error", Type = typeof(GenericAPIResponseDTO<AGMAccesscodeResponse>))]
        [HttpPost]
        public async Task<GenericAPIResponseDTO<AGMAccesscodeResponse>> FetchLiveMeetingAccessUrl([FromBody] MeetingAccesscodeDTO post)
        {
            var response = await GetMeetingAccessCodeAsync(post);

            return response;
        }


        private Task<GenericAPIResponseDTO<AGMAccesscodeResponse>> GetMeetingAccessCodeAsync(MeetingAccesscodeDTO post)
        {
            try
            {
                GenericAPIResponseDTO<AGMAccesscodeResponse> response;
                if (!string.IsNullOrEmpty(post.company) && !string.IsNullOrEmpty(post.emailAddress))
                {

                    var shareholder = db.BarcodeStore.FirstOrDefault(s => s.Company.ToLower() == post.company.ToLower() && s.emailAddress.ToLower() == post.emailAddress.ToLower());
                    if(shareholder==null)
                    {
                        return Task.FromResult<GenericAPIResponseDTO<AGMAccesscodeResponse>>(new GenericAPIResponseDTO<AGMAccesscodeResponse>
                        {
                            responseCode = "400",
                            responseMessage = "Your account information cann't be validated for this meeting.",
                            responseData = new AGMAccesscodeResponse()
                        });
                    }
                    string AccessCode = shareholder.accesscode;
                    string Company= shareholder.Company;
                    string OnlineUrl = shareholder.OnlineEventUrl;
                    int AGMID = ua.RetrieveAGMUniqueID(post.company);
                    if(string.IsNullOrEmpty(AccessCode))
                    {
                        //generate Accesscode.
                        AccessCode = ua.GetAccessCode();
                    }
                    if (string.IsNullOrEmpty(OnlineUrl))
                    {
                        OnlineUrl = Utilities.GenerateAGMUrl(post.company, AGMID, post.emailAddress);

                    }
                    response = new GenericAPIResponseDTO<AGMAccesscodeResponse>
                    {
                        responseCode = "200",
                        responseMessage = "Success",
                        responseData = new AGMAccesscodeResponse
                        {
                            company = Company,
                            accesscode = AccessCode,
                            meetingurl = OnlineUrl
                        }

                    };
                    shareholder.OnlineEventUrl = OnlineUrl;
                    shareholder.accesscode = AccessCode;
                    db.SaveChanges();

                    return Task.FromResult<GenericAPIResponseDTO<AGMAccesscodeResponse>>(response);
                }

                return Task.FromResult<GenericAPIResponseDTO<AGMAccesscodeResponse>>(new GenericAPIResponseDTO<AGMAccesscodeResponse>{
                    responseCode = "400",
                    responseMessage = "Invalid meeting request, Please contact Coronation Registrars.",
                    responseData = new AGMAccesscodeResponse()
                });
            }
            catch (Exception e)
            {
                return Task.FromResult<GenericAPIResponseDTO<AGMAccesscodeResponse>>(new GenericAPIResponseDTO<AGMAccesscodeResponse>
                {
                    responseCode = "500",
                    responseMessage = "Server Failure",
                    responseData = new AGMAccesscodeResponse()
                });
            }

        }


        // GET api/<controller>/5
        //[HttpGet]
        //public GenericAPIResponseDTO<string> GetAGMID(string company)
        //{
        //    GenericAPIResponseDTO<string> response = new GenericAPIResponseDTO<string>();
        //    var UniqueAGMId = ua.RetrieveAGMUniqueID();
        //    if (UniqueAGMId != -1)
        //    {
        //        response.responseData = UniqueAGMId.ToString();
        //        response.responseCode = "200";
        //        response.responseMessage = "Success";
        //        return response;
        //    }

        //    //db.Settings.Where(s => s.CompanyName == company && s.ArchiveStatus == false).Select(o => o.AGMID).OrderByDescending(k => k).FirstOrDefault();
        //    response.responseData = "";
        //    response.responseCode = "404";
        //    response.responseMessage = "BadRequest";
        //    return response;
        //}

        //// POST api/<controller>
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}