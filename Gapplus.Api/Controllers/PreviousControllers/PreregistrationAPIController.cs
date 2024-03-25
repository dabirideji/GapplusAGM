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
// using Swashbuckle.Swagger.Annotations;
// using BarcodeGenerator.Models.ModelDTO;

// namespace BarcodeGenerator.Controllers
// {
//     [ApiExplorerSettings(IgnoreApi = true)]
//     public class PreregistrationAPIController : ApiController
//     {
//         AGMRegistrationService _AGMService = new AGMRegistrationService();
//         PreregistrationService _preregistrationService = new PreregistrationService();
//         UsersContext db = new UsersContext();
//         //UserAdmin ua = new UserAdmin();

//         // GET api/<controller>
//         //public IEnumerable<string> Get()
//         //{
//         //    return new string[] { "value1", "value2" };
//         //}

//         // GET api/<controller>/5



//         [HttpGet]
//         [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AGMCompaniesResponse))]
//         [SwaggerResponse(200, "", Type = typeof(AGMCompaniesResponse))]
//         [SwaggerResponse(500, "Server Error", Type = typeof(AGMCompaniesResponse))]
//         public async Task<List<AGMCompanies>> GetActiveAGMCompanies()
//         {

//             var response = await GetActiveAGMCompaniesAsync();

//             return response;
//         }


//         [HttpGet]
//         [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AGMCompaniesResponse))]
//         [SwaggerResponse(200, "", Type = typeof(AGMCompaniesResponse))]
//         [SwaggerResponse(201, "No Active AGM Available", Type = typeof(AGMCompaniesResponse))]
//         [SwaggerResponse(500, "Server Error", Type = typeof(AGMCompaniesResponse))]
//         public async Task<AGMCompaniesResponse> GetActiveAGMCompany(string company)
//         {
//             var response = await _AGMService.GetActiveAGMCompanyAsync(company);

//             return response;
//         }


//         private Task<List<AGMCompanies>> GetActiveAGMCompaniesAsync()
//         {
//             try
//             {
//                 var companyNameList = db.Settings.Where(s => s.ArchiveStatus == false).Select(o => new AGMCompanies { company = o.CompanyName, description = o.Description, agmid = o.AGMID, venue = o.Venue, dateTime = o.AgmDateTime, EnddateTime = o.AgmEndDateTime }).Distinct().OrderBy(k => k.company).ToList();

//                 return Task.FromResult<List<AGMCompanies>>(companyNameList);

//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<List<AGMCompanies>>(new List<AGMCompanies>());
//             }

//         }


//         [HttpPost]
//         public async Task<AGMAccesscodeResponse> GetShareholderAccessCode([FromBody] PreregistrationDto post)
//         {
//             var response = await _AGMService.GetShareholderAccessCodeAsync(post);

//             return response;
//         }

//         [HttpPost]
//         public async Task<GenericResponseDto<string>> ConfirmShareholderCompanyAndEmail([FromBody] PreregistrationRegisterModel dto)
//         {
//             var response = await _preregistrationService.CheckForShareholderInDb(dto.Company,dto.Email);

//             return response;
//         }

//         [HttpPost]
//         public async Task<APIMessageLog> ConfirmShareholderToken([FromBody] PreregistrationRegisterModel dto)
//         {
//             var response = await _preregistrationService.PreregistrationLogin(dto.Company, dto.Email,dto.Token);

//             return response;
//         }



//         [HttpPost]
//         public async Task<GenericResponseDto<string>> ConfirmPreregistrationResults([FromBody] List<PreregistrationResultDto> dto)
//         {
//             var response = await _preregistrationService.SavePreregistrationResult(dto);

//             return response;
//         }


//         //private Task<AGMAccesscodeResponse> GetShareholderAccessCodeAsync(PreregistrationDto post)
//         //{
//         //    try
//         //    {
//         //        if (!string.IsNullOrEmpty(post.Company) && !string.IsNullOrEmpty(post.emailAddress))
//         //        {
//         //            var shareholder = db.BarcodeStore.FirstOrDefault(s => s.Company.ToLower() == post.Company.ToLower() && s.emailAddress.ToLower() == post.emailAddress.ToLower());

//         //              var acccescodeResponse =    new AGMAccesscodeResponse { company = shareholder.Company, accesscode = shareholder.accesscode };

//         //            return Task.FromResult<AGMAccesscodeResponse>(acccescodeResponse);
//         //        }

//         //        return Task.FromResult<AGMAccesscodeResponse>(new AGMAccesscodeResponse());
//         //    }
//         //    catch (Exception e)
//         //    {
//         //        return Task.FromResult<AGMAccesscodeResponse>(new AGMAccesscodeResponse());
//         //    }

//         //}

//         //[HttpGet]
//         //public List<AGMCompaniesResponse> GetAGMCompanies()
//         //{

//         //    var companyNameList = db.Settings.Where(s => s.ArchiveStatus == false).Select(o =>new AGMCompaniesResponse {company = o.CompanyName, description = o.Description, AGMID = o.AGMID }).Distinct().OrderBy(k => k.company).ToList();

//         //    return companyNameList;
//         //}

//         [HttpGet]
//         public async Task<int> GetPreregistrationAGMID(string company)
//         {
//             var companyAGMID = await _AGMService.GetPreregistrationAGMID(company);
//             //var companyAGMID= db.Settings.Where(s =>s.CompanyName==company && s.ArchiveStatus == false).Select(o => o.AGMID).OrderByDescending(k => k).FirstOrDefault();

//             return companyAGMID;
//         }



//         //public HttpRequestResponse GetVotingResponse(string id)
//         //{

//         //    return new HttpRequestResponse();
//         //}

//         public async Task<PreRegistrationResponse> GetPreregistrationComfirmation(VoteModel query)
//         {
//             return await _AGMService.GetPreregistrationComfirmationAsync(query);
//         }

//             //public PreRegistrationResponse GetPreregistrationComfirmation(VoteModel query)
//             //{
//             //    try
//             //    {
//             //        string companyinfo = query.company;
//             //        string emailAddress = query.identity;
//             //        int agmid = query.agmid;

//             //        var agmstatus = db.Settings.FirstOrDefault(s => s.AGMID == agmid);
//             //        if (agmstatus.ArchiveStatus)
//             //        {
//             //            var MessageLog5 = new APIMessageLog()
//             //            {
//             //                Status = "Failed Accreditation",
//             //                ResponseCode = "205",
//             //                ResponseMessage = "Failed Accreditation Attempt - Empty Company parameter or Email provided. Email:" + " " + emailAddress,
//             //                EventTime = DateTime.Now
//             //            };
//             //            db.APIMessageLogs.Add(MessageLog5);
//             //            db.SaveChanges();

//             //        }

//             //        if (!string.IsNullOrEmpty(companyinfo) && !string.IsNullOrEmpty(emailAddress))
//             //        {
//             //            var UniqueAGMId = ua.RetrieveAGMUniqueID(companyinfo);
//             //            if (UniqueAGMId != -1)
//             //            {
//             //                var shareholderRecords = db.BarcodeStore.Where(u => u.Company.ToLower() == companyinfo.ToLower() && u.emailAddress.ToLower() == emailAddress.ToLower());
//             //                if (shareholderRecords != null)
//             //                {
//             //                    foreach (var sr in shareholderRecords)
//             //                    {
//             //                        var shareholder = db.Present.FirstOrDefault(u => u.AGMID == UniqueAGMId && u.ShareholderNum == sr.ShareholderNum);

//             //                        if (sr.PresentByProxy != true && shareholder == null)
//             //                        {
//             //                            sr.Present = true;
//             //                            PresentModel present = new PresentModel();
//             //                            present.Name = sr.Name;
//             //                            present.Address = sr.Address;
//             //                            present.ShareholderNum = sr.ShareholderNum;
//             //                            present.Holding = sr.Holding;
//             //                            present.AGMID = UniqueAGMId;
//             //                            present.PercentageHolding = sr.PercentageHolding;
//             //                            present.present = true;
//             //                            present.proxy = false;
//             //                            present.PresentTime = DateTime.Now;
//             //                            present.Timestamp = DateTime.Now.TimeOfDay;
//             //                            present.emailAddress = sr.emailAddress;
//             //                            if (!String.IsNullOrEmpty(sr.PhoneNumber))
//             //                            {
//             //                                if (sr.PhoneNumber.StartsWith("234"))
//             //                                {
//             //                                    present.PhoneNumber = sr.PhoneNumber;
//             //                                }
//             //                                else if (sr.PhoneNumber.StartsWith("0"))
//             //                                {
//             //                                    var number = double.Parse(sr.PhoneNumber);
//             //                                    present.PhoneNumber = "234" + number.ToString();

//             //                                }

//             //                            }
//             //                            present.Clikapad = sr.Clikapad;
//             //                            db.Present.Add(present);
//             //                            sr.Date = DateTime.Today.ToString();
//             //                            db.Entry(sr).State = EntityState.Modified;

//             //                            //var sucessoutput = JsonConvert.SerializeObject(successresponse);
//             //                            var MessageLog = new APIMessageLog()
//             //                            {
//             //                                Status = "successfully Accredited",
//             //                                ResponseCode = "200",
//             //                                ResponseMessage = sr.ShareholderNum + " " + "Successful Accreditation. Email Address" + " " + emailAddress,
//             //                                EventTime = DateTime.Now
//             //                            };
//             //                            db.APIMessageLogs.Add(MessageLog);
//             //                            //db.SaveChanges();
//             //                        }
//             //                        else
//             //                        {
//             //                            var MessageLog2 = new APIMessageLog()
//             //                            {
//             //                                Status = "Already Accredited",
//             //                                ResponseCode = "202",
//             //                                ResponseMessage = sr.ShareholderNum + " " + "Successful Accreditation. Email:" + " " + emailAddress,
//             //                                EventTime = DateTime.Now
//             //                            };
//             //                            db.APIMessageLogs.Add(MessageLog2);
//             //                        }


//             //                    }
//             //                }
//             //                else
//             //                {
//             //                    var MessageLog3 = new APIMessageLog()
//             //                    {
//             //                        Status = "Failed",
//             //                        ResponseCode = "205",
//             //                        ResponseMessage = "Failed e-accreditation Attempt - The Company's AGM may not be enlisted or incorrect email address. Email:" + " " + query.identity,
//             //                        EventTime = DateTime.Now
//             //                    };
//             //                    db.APIMessageLogs.Add(MessageLog3);

//             //                }
//             //            }
//             //            else
//             //            {
//             //                var MessageLog4 = new APIMessageLog()
//             //                {
//             //                    Status = "Failed Accreditation",
//             //                    ResponseCode = "205",
//             //                    ResponseMessage = "Failed Accreditation Attempt - AGM not available for company. Email:" + " " + emailAddress,
//             //                    EventTime = DateTime.Now
//             //                };
//             //                db.APIMessageLogs.Add(MessageLog4);
//             //            }

//             //        }
//             //        else
//             //        {
//             //            var MessageLog5 = new APIMessageLog()
//             //            {
//             //                Status = "Failed Accreditation",
//             //                ResponseCode = "205",
//             //                ResponseMessage = "Failed Accreditation Attempt - Empty Company parameter or Email provided. Email:" + " " + emailAddress,
//             //                EventTime = DateTime.Now
//             //            };
//             //            db.APIMessageLogs.Add(MessageLog5);
//             //        }

//             //        db.SaveChanges();

//             //        return new PreRegistrationResponse()
//             //        {
//             //            Status = "Success",
//             //            ResponseCode = "200",
//             //            ResponseMessage = "Post received and processed"

//             //        };

//             //    }
//             //     catch (Exception e)
//             //     {
//             //        var MessageLog7 = new APIMessageLog()
//             //        {
//             //            Status = "Error",
//             //            ResponseCode = "500",
//             //            ResponseMessage = "Something went wrong while processing this request from " + query.identity + " " + "Error Message: " + e.Message + " " + "Please contact admin",
//             //            EventTime = DateTime.Now
//             //        };
//             //            db.APIMessageLogs.Add(MessageLog7);
//             //            db.SaveChanges();
//             //            return new PreRegistrationResponse()
//             //            {
//             //                Status = "Error",
//             //                ResponseCode = "500",
//             //                ResponseMessage = "Something went wrong while processing this request from " + query.identity + " " + "Error Message: " + e.Message + " " + "Please contact admin",
//             //            };
//             //     }

//             //}

//             [HttpPost]
//         public async Task<PreRegistrationResponse> GetPreregistrationRegister([FromBody] PreregistrationDto[] post)
//         {
//             return await _AGMService.GetPreregistrationRegister(post);

//         }


//             // POST api/<controller>
//         //    [HttpPost]
//         //public PreRegistrationResponse GetPreregistrationRegister([FromBody]PreregistrationDto[] post)
//         //{
//         //    try
//         //    {
//         //        AppLog log;
//         //        foreach (PreregistrationDto v in post)
//         //        {
//         //            Int64 shareholdernum = 0;
//         //            string companyinfo = v.Company;
//         //            if (!string.IsNullOrEmpty(companyinfo) && v.ShareholderNum != null)
//         //            {
//         //                shareholdernum = Int64.Parse(v.ShareholderNum);
//         //                var UniqueAGMId = ua.RetrieveAGMUniqueID(companyinfo);
//         //                if (UniqueAGMId != -1)
//         //                {
//         //                    var barcode = db.BarcodeStore.FirstOrDefault(u => u.Company.ToLower() == companyinfo.ToLower() && u.ShareholderNum == shareholdernum);
//         //                    if (barcode != null)
//         //                    {

//         //                        var shareholder = db.Present.FirstOrDefault(u => u.AGMID == UniqueAGMId && u.ShareholderNum == barcode.ShareholderNum);

//         //                        if (barcode.PresentByProxy != true && shareholder == null)
//         //                        {
//         //                            barcode.Present = true;
//         //                            PresentModel present = new PresentModel();
//         //                            present.Name = barcode.Name;
//         //                            present.Address = barcode.Address;
//         //                            present.ShareholderNum = barcode.ShareholderNum;
//         //                            present.Holding = barcode.Holding;
//         //                            present.AGMID = UniqueAGMId;
//         //                            present.PercentageHolding = barcode.PercentageHolding;
//         //                            present.present = true;
//         //                            present.proxy = false;
//         //                            present.PresentTime = DateTime.Now;
//         //                            present.Timestamp = DateTime.Now.TimeOfDay;
//         //                            present.emailAddress = v.emailAddress;
//         //                            if (!String.IsNullOrEmpty(v.PhoneNumber))
//         //                            {
//         //                                if (barcode.PhoneNumber.StartsWith("234"))
//         //                                {
//         //                                    present.PhoneNumber = barcode.PhoneNumber;
//         //                                }
//         //                                else if (barcode.PhoneNumber.StartsWith("0"))
//         //                                {
//         //                                    var number = double.Parse(barcode.PhoneNumber);
//         //                                    present.PhoneNumber = "234" + number.ToString();

//         //                                }

//         //                            }
//         //                            present.Clikapad = barcode.Clikapad;
//         //                            db.Present.Add(present);
//         //                            barcode.Date = DateTime.Today.ToString();
//         //                            db.Entry(barcode).State = EntityState.Modified;

//         //                            //var sucessoutput = JsonConvert.SerializeObject(successresponse);
//         //                            var MessageLog = new APIMessageLog()
//         //                            {
//         //                                Status = "successfully Accredited",
//         //                                ResponseCode = "200",
//         //                                ResponseMessage = v.ShareholderNum + " " + "Successful Accreditation. Email Address" + " " + v.emailAddress,
//         //                                EventTime = DateTime.Now
//         //                            };
//         //                            log = new AppLog();
//         //                            log.Status = MessageLog.Status;
//         //                            log.ResponseCode = MessageLog.ResponseCode;
//         //                            log.ResponseMessage = MessageLog.ResponseMessage;
//         //                            log.EventTime = MessageLog.EventTime;
//         //                            db.AppLogs.Add(log);
//         //                            //db.SaveChanges();
//         //                        }
//         //                        else
//         //                        {
//         //                            var MessageLog = new APIMessageLog()
//         //                            {
//         //                                Status = "Already Accredited",
//         //                                ResponseCode = "202",
//         //                                ResponseMessage = v.ShareholderNum + " " + "Successful Accreditation. Email:" + " " + v.emailAddress,
//         //                                EventTime = DateTime.Now
//         //                            };
//         //                            log = new AppLog();
//         //                            log.Status = MessageLog.Status;
//         //                            log.ResponseCode = MessageLog.ResponseCode;
//         //                            log.ResponseMessage = MessageLog.ResponseMessage;
//         //                            log.EventTime = MessageLog.EventTime;
//         //                            db.AppLogs.Add(log);
//         //                        }


//         //                        //return new PreRegistrationResponse()
//         //                        //{
//         //                        //    Status = "Already Accredited",
//         //                        //    ResponseCode = "02",
//         //                        //    ResponseMessage = "Successful Accreditation"
//         //                        //};
//         //                    }
//         //                    else
//         //                    {
//         //                        var MessageLog = new APIMessageLog()
//         //                        {
//         //                            Status = "Failed",
//         //                            ResponseCode = "205",
//         //                            ResponseMessage = v.ShareholderNum + " " + "Failed Accreditation Attempt - The Company's AGM may not be enlisted or Shareholder number incorrect. Email:" + " " +v.emailAddress,
//         //                            EventTime = DateTime.Now
//         //                        };
//         //                        log = new AppLog();
//         //                        log.Status = MessageLog.Status;
//         //                        log.ResponseCode = MessageLog.ResponseCode;
//         //                        log.ResponseMessage = MessageLog.ResponseMessage;
//         //                        log.EventTime = MessageLog.EventTime;
//         //                        db.AppLogs.Add(log);

//         //                    }
                          
//         //                    //return new PreRegistrationResponse()
//         //                    //{
//         //                    //    Status = "Failed",
//         //                    //    ResponseCode = "05",
//         //                    //    ResponseMessage = "Failed Accreditation - The Company's AGM may not be enlisted or Shareholder number incorrect"
//         //                    //};
//         //                }
//         //                else
//         //                {
//         //                    var MessageLog = new APIMessageLog()
//         //                    {
//         //                        Status = "Failed Accreditation",
//         //                        ResponseCode = "205",
//         //                        ResponseMessage = v.ShareholderNum + " " + "Failed Accreditation Attempt - AGM not available for company Email:" + " " + v.emailAddress,
//         //                        EventTime = DateTime.Now
//         //                    };
//         //                    log = new AppLog();
//         //                    log.Status = MessageLog.Status;
//         //                    log.ResponseCode = MessageLog.ResponseCode;
//         //                    log.ResponseMessage = MessageLog.ResponseMessage;
//         //                    log.EventTime = MessageLog.EventTime;
//         //                    db.AppLogs.Add(log);
//         //                }

//         //                //return new PreRegistrationResponse()
//         //                //{
//         //                //    Status = "Failed Accreditation",
//         //                //    ResponseCode = "05",
//         //                //    ResponseMessage = "Failed Accreditation - AGM not available for company "
//         //                //};

//         //            }
//         //            else
//         //            {
//         //                var MessageLog = new APIMessageLog()
//         //                {
//         //                    Status = "Failed Accreditation",
//         //                    ResponseCode = "205",
//         //                    ResponseMessage = "Failed Accreditation Attempt - Empty Company parameter or Shareholder Number. Email:"+" " +v.emailAddress,
//         //                    EventTime = DateTime.Now
//         //                };
//         //                log = new AppLog();
//         //                log.Status = MessageLog.Status;
//         //                log.ResponseCode = MessageLog.ResponseCode;
//         //                log.ResponseMessage = MessageLog.ResponseMessage;
//         //                log.EventTime = MessageLog.EventTime;
//         //                db.AppLogs.Add(log);
//         //            }
//         //        }
//         //        db.SaveChanges();

//         //        return new PreRegistrationResponse()
//         //        {
//         //            Status = "Success",
//         //            ResponseCode = "200",
//         //            ResponseMessage = "Post received and processed"
                    
//         //        };
//         //    }
//         //    catch(Exception e)
//         //    {
//         //        var MessageLog = new APIMessageLog()
//         //        {
//         //            Status = "Error",
//         //            ResponseCode = "500",
//         //            ResponseMessage = "Something went wrong while processing this request from "+post[0].emailAddress+" " + "Error Message: " + e.Message + " " + "Please contact admin",
//         //            EventTime = DateTime.Now
//         //        };
//         //        var log = new AppLog();
//         //        log.Status = MessageLog.Status;
//         //        log.ResponseCode = MessageLog.ResponseCode;
//         //        log.ResponseMessage = MessageLog.ResponseMessage;
//         //        log.EventTime = MessageLog.EventTime;
//         //        db.AppLogs.Add(log);
//         //        db.SaveChanges();
//         //        return new PreRegistrationResponse()
//         //        {
//         //            Status = "Error",
//         //            ResponseCode = "500",
//         //            ResponseMessage = "Something went wrong while processing this request from " + post[0].emailAddress + " " + "Error Message: " + e.Message+ " " +"Please contact admin",
//         //        };
//         //    }
            
//         //}


//     }
// }