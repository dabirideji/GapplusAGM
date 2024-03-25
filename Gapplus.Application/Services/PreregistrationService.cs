using BarcodeGenerator.Models;
using BarcodeGenerator.Models.ModelDTO;
using Gapplus.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Service
{
    public class PreregistrationService
    {


        public PreregistrationService(UsersContext _db)
        {
            db =_db;
            ua = new UserAdmin(db);
            agmM = new AGMManager(db);
            presentService = new PresentService(db);
            resolutionService = new ResolutionService(db);
        }



        UsersContext db;
        Stopwatch stopwatch = new Stopwatch();
        UserAdmin ua;
        AGMManager agmM;
        PresentService presentService ;
        ResolutionService resolutionService ;
        EmailService _emailService = new EmailService();
        private static string currentYear = DateTime.Now.Year.ToString();
        // private static string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static string connStr = DatabaseManager.GetConnectionString();

        private SqlConnection connn =
                new SqlConnection(connStr);

        public async Task<GenericResponseDto<string>> CheckForShareholderInDb(string company, string email)
        {
            try
            {
                var checkForEmail = await ua.GetShareHolderAccount(company, email);
                if (checkForEmail.Status)
                {
                    //Generate Toke, Save Token and send Token to email
                    var generateToken = await ua.GetPregistrationToken();
                    if (!string.IsNullOrEmpty(generateToken))
                    {
                        //Save Token to Db
                        var SavedToken = await SaveTokenToDb(company, email, generateToken);
                        if (SavedToken.Status)
                        {
                            //Send Token to Shareholder Email.
                            var shareholderRecord = db.BarcodeStore.FirstOrDefault(s => s.Company.ToLower() == company.ToLower() && s.emailAddress.ToLower() == email.ToLower());
                            var shareholder = new BarcodeModelDto
                            {
                                Name = shareholderRecord.Name,
                                Token = shareholderRecord.passwordToken,
                                Company = shareholderRecord.Company,
                                emailAddress = shareholderRecord.emailAddress
                            };
                            var sentEmailResponse = await _emailService.SendEmailToShareholderEmailAddress(shareholder);

                            return sentEmailResponse;
                        }
                        else
                        {
                            return SavedToken;
                        }
                    }
                    else
                    {
                        return new GenericResponseDto<string>
                        {
                            Code = 205,
                            Status = false,
                            Message = "We couldn't sent generate Token. Please try again."
                        };
                    }

                }
                else if(checkForEmail.Code==201)
                {
                    return new GenericResponseDto<string>
                    {
                        Code = 201,
                        Status = false,
                        Message = checkForEmail.Message,
                        EmailRegistraionUrl = "~/Pregistration"
                    };
                }
                else
                {
                    return new GenericResponseDto<string>
                    {
                        Code = checkForEmail.Code,
                        Status = false,
                        Message = checkForEmail.Message,
                    
                    };
                }
            }
            catch (Exception e)
            {
                return new GenericResponseDto<string>
                {
                    Code = 500,
                    Status = false,
                    Message = "We couldn't process this request at the moment. Please try again later."
                };
            }
        }



        public async Task<APIMessageLog> PreregistrationLogin(string company, string email, string Token)
        {
            var response = await CheckForShareholderTokenInDb(company, email, Token);

            return response;
        }


        public async Task<GenericResponseDto<string>> SavePreregistrationResult(List<PreregistrationResultDto> dto)
        {
            var email = dto.FirstOrDefault().Email;
            var company = dto.FirstOrDefault().Company;
            if (!string.IsNullOrEmpty(email)&& !string.IsNullOrEmpty(company))
            {
                var shareholderAccount = await ua.GetShareHolderAccount(company, email);

                if (shareholderAccount.Status)
                {
                    foreach (var item in dto)
                    {
                        await resolutionService.TaskResolution(item.ResolutionId, item.ResultChoice, company, shareholderAccount.Data.ShareholderNum);
                    }
                    var presentResponse = await presentService.MarkPregisteredShareholderAsync(company, email);
                    if (presentResponse.Status)
                    {
                        //Set result in result database

                    
                        return new GenericResponseDto<string>
                        {
                            Status = true,
                            Message = "success"
                        };
                    }
                }
                //Mark shareholder as present


            }
            return new GenericResponseDto<string>
            {
                Status = false,
                Message = "Preregistraton process couldn't be completed. Please try again Later."
            };
        }

        public Task<GenericResponseDto<string>> MarkAsPreregistered(string company, string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(company) && !string.IsNullOrEmpty(email))
                {
                    //Check for shareholder Email in db
                    var ShareholderRecords = db.BarcodeStore.Where(u => u.Company.ToLower() == company.ToLower() && u.emailAddress.ToLower() == email.ToLower());
                    var PresentRecord = db.Present.FirstOrDefault(u => u.Company.ToLower() == company.ToLower() && u.emailAddress.ToLower() == email.ToLower());
                    if (ShareholderRecords!=null)
                    {
                        foreach(var record in ShareholderRecords)
                        {
                            record.Preregistered = true;
                            
                        }
                        PresentRecord.preregistered = true;
                        db.SaveChanges();
                        return Task.FromResult(new GenericResponseDto<string>
                        {
                            Status = true,
                            Message = "success"
                        });
                    }
                    else
                    {
                        return Task.FromResult(new GenericResponseDto<string>
                        {
                            Code = 1,
                            Status = false,
                            Message = "Email you entered is not registered."
                        });
                    }
                }
                return Task.FromResult(new GenericResponseDto<string>
                {
                    Status = false,
                    Message = "Either Company or Email Information provided is empty."
                });

            }
            catch (Exception e)
            {
                return Task.FromResult(new GenericResponseDto<string>
                {
                    Code = 500,
                    Status = false,
                    Message = "We couldn't process this request at the moment. Please try again later."
                });
            }

        }


        public async Task<APIMessageLog> CheckForShareholderTokenInDb(string company, string email, string Token)
        {
            int agmid;
            APIMessageLog MessageLog;
            var abstainbtnchoice = true;
            AppLog log;
            try
            {
               
  
                if (!string.IsNullOrEmpty(company) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(Token))
                {
                    var forBg = "green";
                    var againstBg = "red";
                    var abstainBg = "blue";
                    var voidBg = "black";
                    //VoteModel query = (VoteModel)decryptedtext;
                    var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
                    if(UniqueAGMId == -1)
                    {
                        MessageLog = new APIMessageLog()
                        {
                            Status = "Error",
                            ResponseCode = "205",
                            ResponseMessage = "AGM event not active at the moment.",
                            EventTime = DateTime.Now
                        };
                        log = new AppLog();
                        log.Status = MessageLog.Status;
                        log.ResponseCode = MessageLog.ResponseCode;
                        log.ResponseMessage = MessageLog.ResponseMessage;
                        log.EventTime = MessageLog.EventTime;
                        db.AppLogs.Add(log);
                        db.SaveChanges();
                        return MessageLog;
                    }
                    var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                    if (AgmEvent == null)
                    {
                        MessageLog = new APIMessageLog()
                        {
                            Status = "Error",
                            ResponseCode = "205",
                            ResponseMessage = "AGM event not active at the moment.",
                            EventTime = DateTime.Now
                        };
                        log = new AppLog();
                        log.Status = MessageLog.Status;
                        log.ResponseCode = MessageLog.ResponseCode;
                        log.ResponseMessage = MessageLog.ResponseMessage;
                        log.EventTime = MessageLog.EventTime;
                        db.AppLogs.Add(log);
                        db.SaveChanges();
                        return MessageLog;

                    }
                        if (!string.IsNullOrEmpty(AgmEvent.VoteForColorBg))
                        {
                            forBg = AgmEvent.VoteForColorBg;
                        }
                        if (!string.IsNullOrEmpty(AgmEvent.VoteAgainstColorBg))
                        {
                            againstBg = AgmEvent.VoteAgainstColorBg;
                        }
                        if (!string.IsNullOrEmpty(AgmEvent.VoteAbstaincolorBg))
                        {
                            abstainBg = AgmEvent.VoteAbstaincolorBg;
                        }
                        if (!string.IsNullOrEmpty(AgmEvent.VoteVoidColorBg))
                        {
                            voidBg = AgmEvent.VoteVoidColorBg;
                        }
                        if (AgmEvent.AbstainBtnChoice != null)
                        {
                            abstainbtnchoice = (bool)AgmEvent.AbstainBtnChoice;
                        }
                        company = AgmEvent.CompanyName;

                        //Check for shareholder Email in db
                        var CheckTokenRecord = db.BarcodeStore.FirstOrDefault(u => u.Company.ToLower() == company.ToLower() && u.emailAddress.ToLower() == email.ToLower() && u.passwordToken == Token.Trim());
                    if (CheckTokenRecord == null)
                    {
                        MessageLog = new APIMessageLog()
                        {
                            Status = "Error",
                            ResponseCode = "205",
                            ResponseMessage = "Token information can not be authenticated.",
                            EventTime = DateTime.Now
                        };
                        log = new AppLog();
                        log.Status = MessageLog.Status;
                        log.ResponseCode = MessageLog.ResponseCode;
                        log.ResponseMessage = MessageLog.ResponseMessage;
                        log.EventTime = MessageLog.EventTime;
                        db.AppLogs.Add(log);
                        db.SaveChanges();
                        return MessageLog;
                    }
                            BarcodeModel cshareholderRecord;
                            var accountResponse = await ua.GetShareHolderAccount(company, email);
                            if (!accountResponse.Status)
                            {
                                MessageLog = new APIMessageLog()
                                {
                                    Status = "Error",
                                    ResponseCode = accountResponse.Code.ToString(),
                                    ResponseMessage = accountResponse.Message,
                                    EventTime = DateTime.Now
                                };
                                log = new AppLog();
                                log.Status = MessageLog.Status;
                                log.ResponseCode = MessageLog.ResponseCode;
                                log.ResponseMessage = MessageLog.ResponseMessage;
                                log.EventTime = MessageLog.EventTime;
                                db.AppLogs.Add(log);
                                db.SaveChanges();

                                return MessageLog;
                            }
                            cshareholderRecord = accountResponse.Data;
                            //if (accountResponse.ListData.Count != 0)
                            //{
                            //    var checkIfAnyAccountIsProxy = accountResponse.ListData.Any(s => s.PresentByProxy == true);
                            //    if (checkIfAnyAccountIsProxy)
                            //    {
                            //        MessageLog = new APIMessageLog()
                            //        {
                            //            Status = "Error",
                            //            ResponseCode = "205",
                            //            ResponseMessage = "This account cann't pre-register since it has voted by proxy.",
                            //            EventTime = DateTime.Now
                            //        };
                            //        log = new AppLog();
                            //        log.Status = MessageLog.Status;
                            //        log.ResponseCode = MessageLog.ResponseCode;
                            //        log.ResponseMessage = MessageLog.ResponseMessage;
                            //        log.EventTime = MessageLog.EventTime;
                            //        db.AppLogs.Add(log);
                            //        db.SaveChanges();
                            //        return MessageLog;
                            //    }
                            //    if(accountResponse.ListData.Count > 1)
                            //    {
                            //        var checkifConsolidate = accountResponse.ListData.Any(c => c.Consolidated == true);
                            //        if (!checkifConsolidate)
                            //        {
                            //            cshareholderRecord = agmM.ConsolidateRequest(company, accountResponse.ListData);
                            //        }
                            //        else
                            //        {
                            //            var consolidatedvalue = accountResponse.ListData.Where(s => s.Consolidated == true).Select(s => s.ConsolidatedValue).FirstOrDefault();

                            //            cshareholderRecord = agmM.GetConsolidatedAccount(company, consolidatedvalue);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        cshareholderRecord = accountResponse.ListData.FirstOrDefault();
                            //    }


                            var resolutions = from question in db.Question
                                              where question.AGMID == UniqueAGMId
                                              select question;
                            MessageLog = new APIMessageLog()
                            {
                                Status = "Successful Login",
                                ResponseCode = "200",
                                ResponseMessage = "",
                                EventTime = DateTime.Now,
                                Rsolutions = resolutions.ToList(),
                                AGMID = UniqueAGMId,
                                abstainBtnChoice = abstainbtnchoice,
                                AGMTitle = AgmEvent.Description,
                                PreregisterationShareholder = cshareholderRecord,
                                forBg = forBg,
                                againstBg = againstBg,
                                abstainBg = abstainBg,
                                voidBg = voidBg,
                                Companylogo = AgmEvent.Image != null ? "data:image/jpg;base64," +
                             Convert.ToBase64String((byte[])AgmEvent.Image) : "",
                                //VotingResult = db.Result.Where(r => r.ShareholderNum == cshareholderRecord.ShareholderNum && r.AGMID == UniqueAGMId).ToList()
                            };
                            log = new AppLog();
                            log.Status = MessageLog.Status;
                            log.ResponseCode = MessageLog.ResponseCode;
                            log.ResponseMessage = MessageLog.ResponseMessage;
                            log.EventTime = MessageLog.EventTime;
                            db.AppLogs.Add(log);
                            db.SaveChanges();
                            return MessageLog;
                        
                    
   
                    }
                    else
                    {
                        MessageLog = new APIMessageLog()
                        {
                            Status = "Error",
                            ResponseCode = "205",
                            ResponseMessage = "Token information may not be correct.",
                            EventTime = DateTime.Now
                        };
                        log = new AppLog();
                        log.Status = MessageLog.Status;
                        log.ResponseCode = MessageLog.ResponseCode;
                        log.ResponseMessage = MessageLog.ResponseMessage;
                        log.EventTime = MessageLog.EventTime;
                        db.AppLogs.Add(log);
                        db.SaveChanges();
                        return MessageLog;
                    }
                //}
                //MessageLog = new APIMessageLog()
                //{
                //    Status = "Error",
                //    ResponseCode = "205",
                //    ResponseMessage = "Token information may not be correct.",
                //    EventTime = DateTime.Now
                //};
                //log = new AppLog();
                //log.Status = MessageLog.Status;
                //log.ResponseCode = MessageLog.ResponseCode;
                //log.ResponseMessage = MessageLog.ResponseMessage;
                //log.EventTime = MessageLog.EventTime;
                //db.AppLogs.Add(log);
                //db.SaveChanges();
                //return MessageLog;

            }
            catch (Exception e)
            {
                MessageLog = new APIMessageLog()
                {
                    Status = "Error",
                    ResponseCode = "205",
                    ResponseMessage = "The system is unable to process the request at the moment. Please try again later.",
                    EventTime = DateTime.Now
                };
                log = new AppLog();
                log.Status = MessageLog.Status;
                log.ResponseCode = MessageLog.ResponseCode;
                log.ResponseMessage = MessageLog.ResponseMessage;
                log.EventTime = MessageLog.EventTime;
                db.AppLogs.Add(log);
                db.SaveChanges();
                return MessageLog;
            }

        }

        public Task<GenericResponseDto<string>> SaveTokenToDb(string company, string email, string Token)
        {
            try
            {
                if(!string.IsNullOrEmpty(company) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(Token))
                {
                    
                    string query3 = "UPDATE BarcodeModels SET passwordToken = '"+Token+"' Where Company='" + company + "' and emailAddress='"+email+"'";
                    connn.Open();
                    SqlCommand cmd3 = new SqlCommand(query3, connn);
                    cmd3.CommandTimeout = 180;
                    cmd3.ExecuteNonQuery();
                    connn.Close();
                    return Task.FromResult( new GenericResponseDto<string>
                    {
                        Status = true,
                        Message = "success"
                    });
                }
                return Task.FromResult(new GenericResponseDto<string>
                {
                    Status = false,
                    Message = "The system couldn't update token request."
                });

            }
            catch(Exception ex)
            {
                return Task.FromResult(new GenericResponseDto<string>
                {
                    Code = 500,
                    Status = false,
                    Message = "We couldn't process this request at the moment. please try again later."
                });
            }
        }
    }
}