using BarcodeGenerator.Models;
using BarcodeGenerator.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Service
{
    public class WebVotingService
    {
        public WebVotingService(UsersContext _db)
        {
            db = _db; ua = new UserAdmin(db); agmM = new AGMManager(db);
        }

        UsersContext db;
        Stopwatch stopwatch = new Stopwatch();
        UserAdmin ua;
        AGMManager agmM;
        private static string currentYear = DateTime.Now.Year.ToString();

        public Task<AccreditationResponse> IndexAsync()
        {
            try
            {
                AccreditationResponse model = new AccreditationResponse
                {
                    companies = db.Settings.Where(s => s.ArchiveStatus == false).Select(o => new AGMCompanies { company = o.CompanyName, description = o.Description, agmid = o.AGMID, venue = o.Venue, dateTime = o.AgmDateTime }).Distinct().OrderBy(k => k.company).ToList(),

                };


                return Task.FromResult<AccreditationResponse>(model);

            }
            catch (Exception e)
            {
                return Task.FromResult<AccreditationResponse>(new AccreditationResponse());
            }

        }

        public async Task<APIMessageLog> AccreditationConfirmationPostAsync(accredationDto model)
        {

            try
            {
                //Session.Clear();
                //Application_BeginRequest();
                APIMessageLog MessageLog;
                AppLog log;
                //if (!string.IsNullOrEmpty(model.accesscode))
                //{

                if (model.agmid > 0 && model.shareholderNum > 0 && !string.IsNullOrEmpty(model.accesscode))
                {
                    var UniqueAGMId = model.agmid;
                    var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                    var company = AgmEvent.CompanyName;


                    //check if shareholder is preset
                    var shareholderRecord = db.Present.SingleOrDefault(u => u.AGMID == UniqueAGMId && u.ShareholderNum == model.shareholderNum && u.present == true);
                    if (shareholderRecord == null)
                    {
                        var response = await AutoAdmittanceAsync(company, model.shareholderNum.ToString(), model.accesscode.Trim());
                        if (response == "success")
                        {
                            MessageLog = new APIMessageLog()
                            {
                                Status = "Success",
                                ResponseCode = "200",
                                ResponseMessage = "Successfull login",
                                EventTime = DateTime.Now,
                                requestQuery = GenerateWebVotingRequest(UniqueAGMId, model.shareholderNum)
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
                                Status = "Failed Voting Login Attempt",
                                ResponseCode = "205",
                                ResponseMessage = "Failed Voting Login Attempt - You may have entered a wrong Shareholder number or accesscode.",
                                EventTime = DateTime.Now,

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
                    else
                    {

                        MessageLog = new APIMessageLog()
                        {
                            Status = "Success",
                            ResponseCode = "200",
                            ResponseMessage = "Successfull login",
                            EventTime = DateTime.Now,
                            requestQuery = GenerateWebVotingRequest(UniqueAGMId, model.shareholderNum)
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

                MessageLog = new APIMessageLog()
                {
                    Status = "Failed Voting Login Attempt",
                    ResponseCode = "205",
                    ResponseMessage = "Failed Voting Login Attempt - Company information, accesscode or Shareholder Number may be incorrect.",
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
            catch (Exception e)
            {
                var MessageLog = new APIMessageLog()
                {
                    Status = "Error",
                    ResponseCode = "500",
                    ResponseMessage = "Something went wrong while processing this request from" + " " + "Please contact admin",
                    EventTime = DateTime.Now
                };
                AppLog log = new AppLog();
                log.Status = MessageLog.Status;
                log.ResponseCode = MessageLog.ResponseCode;
                log.ResponseMessage = MessageLog.ResponseMessage;
                log.EventTime = MessageLog.EventTime;
                db.AppLogs.Add(log);
                db.SaveChanges();
                return MessageLog;
            }

        }


        public Task<APIMessageLog> AccreditationConfirmationAsync(string encryptedText)
        {
            //var query = new VoteModel();
            //string companyinfo;
            string shareholderNum = "";
            int agmid;
            APIMessageLog MessageLog;
            var abstainbtnchoice = true;
            AppLog log;

            try
            {

                //Session.Clear();
                //Application_BeginRequest();

                if (!string.IsNullOrEmpty(encryptedText))
                {
                    var replacencryptedString = encryptedText.Replace(" ", "+");
                    var decryptedtext = replacencryptedString.Decrypt();
                    var querystring = decryptedtext.Split('|');
                    int shareholdNum = int.Parse(querystring[0].Trim());
                    //companyinfo = querystring[1].Trim();
                    agmid = int.Parse(querystring[1].Trim());

                    var forBg = "green";
                    var againstBg = "red";
                    var abstainBg = "blue";
                    var voidBg = "black";
                    //VoteModel query = (VoteModel)decryptedtext;
                    var UniqueAGMId = agmid;
                    string company = "";
                    var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                    if (AgmEvent != null)
                    {

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
                    }


                    //companyinfo = query.company;
                    //emailAddress = query.identity;
                    //agmid = query.agmid;

                    var shareholder = db.Present.FirstOrDefault(u => u.AGMID == agmid && u.ShareholderNum == shareholdNum && u.present == true);
                    if (shareholder == null)
                    {
                        MessageLog = new APIMessageLog()
                        {
                            Status = "Error",
                            ResponseCode = "205",
                            ResponseMessage = "Profile Problem",
                            EventTime = DateTime.Now
                        };
                        log = new AppLog();
                        log.Status = MessageLog.Status;
                        log.ResponseCode = MessageLog.ResponseCode;
                        log.ResponseMessage = MessageLog.ResponseMessage;
                        log.EventTime = MessageLog.EventTime;
                        db.AppLogs.Add(log);
                        db.SaveChanges();
                        return Task.FromResult<APIMessageLog>(MessageLog);
                    }

                    var resolutions = from question in db.Question
                                      where question.AGMID == agmid
                                      select question;
                    MessageLog = new APIMessageLog()
                    {
                        Status = "Successful Login",
                        ResponseCode = "200",
                        ResponseMessage = "",
                        EventTime = DateTime.Now,
                        Rsolutions = resolutions.ToList(),
                        AGMID = agmid,
                        abstainBtnChoice = abstainbtnchoice,
                        AGMTitle = AgmEvent.Description,
                        shareholder = shareholder,
                        forBg = forBg,
                        againstBg = againstBg,
                        abstainBg = abstainBg,
                        voidBg = voidBg,
                        Companylogo = AgmEvent.Image != null ? "data:image/jpg;base64," +
                     Convert.ToBase64String((byte[])AgmEvent.Image) : "",
                        VotingResult = db.Result.Where(r => r.ShareholderNum == shareholder.ShareholderNum && r.AGMID == agmid).ToList()
                    };
                    log = new AppLog();
                    log.Status = MessageLog.Status;
                    log.ResponseCode = MessageLog.ResponseCode;
                    log.ResponseMessage = MessageLog.ResponseMessage;
                    log.EventTime = MessageLog.EventTime;
                    db.AppLogs.Add(log);
                    db.SaveChanges();
                    return Task.FromResult<APIMessageLog>(MessageLog);

                }

                MessageLog = new APIMessageLog()
                {
                    Status = "Error",
                    ResponseCode = "205",
                    ResponseMessage = "Profile Problem ",
                    EventTime = DateTime.Now
                };
                log = new AppLog();
                log.Status = MessageLog.Status;
                log.ResponseCode = MessageLog.ResponseCode;
                log.ResponseMessage = MessageLog.ResponseMessage;
                log.EventTime = MessageLog.EventTime;
                db.AppLogs.Add(log);
                db.SaveChanges();
                return Task.FromResult<APIMessageLog>(MessageLog);
            }
            catch (Exception e)
            {
                var MessageLog7 = new APIMessageLog()
                {
                    Status = "Error",
                    ResponseCode = "500",
                    ResponseMessage = "System cannot process your reqeuest at this time " + shareholderNum + " " + "Please contact admin",
                    EventTime = DateTime.Now
                };
                log = new AppLog();
                log.Status = MessageLog7.Status;
                log.ResponseCode = MessageLog7.ResponseCode;
                log.ResponseMessage = MessageLog7.ResponseMessage;
                log.EventTime = MessageLog7.EventTime;
                db.AppLogs.Add(log);
                db.SaveChanges();
                return Task.FromResult<APIMessageLog>(MessageLog7);

            }
        }

        public Task<ResolutionsResponseDTO> WebVotingAsync(PostResolution post)
        {
            int id = post.Id;
            string response = post.RG;
            var question = db.Question.Find(id);
            var UniqueAGMId = post.agmid;

            var currentYear = DateTime.Now.Year.ToString();
            var shareholder = db.Present.FirstOrDefault(s => s.AGMID == UniqueAGMId && s.ShareholderNum == post.shareholdernum);
            if (shareholder == null || shareholder.present != true)
            {
                var model = new ResolutionsResponseDTO
                {
                    Code = "205",
                    Message = "Invalid Vote",
                    Choice = post.RG
                };

                return Task.FromResult<ResolutionsResponseDTO>(model);
            }
            shareholder.TakePoll = true;

            var checkresult = db.Result.FirstOrDefault(r => r.ShareholderNum == post.shareholdernum && r.QuestionId == id);
            if (checkresult != null)
            {
                checkresult.date = DateTime.Now;
                checkresult.Timestamp = DateTime.Now.TimeOfDay;
                checkresult.Holding = shareholder.Holding;
                checkresult.VoteStatus = "Voted";
                checkresult.Source = "Web";
                if (response == "FOR")
                {
                    checkresult.VoteFor = true;
                    checkresult.VoteAgainst = false;
                    checkresult.VoteAbstain = false;
                    checkresult.VoteVoid = false;
                }
                else if (response == "AGAINST")
                {
                    checkresult.VoteAgainst = true;
                    checkresult.VoteFor = false;
                    checkresult.VoteAbstain = false;
                    checkresult.VoteVoid = false;
                }
                else if (response == "ABSTAIN")
                {
                    checkresult.VoteAbstain = true;
                    checkresult.VoteFor = false;
                    checkresult.VoteAgainst = false;
                    checkresult.VoteVoid = false;
                }
                db.Entry(checkresult).State = EntityState.Modified;
                db.Entry(shareholder).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    var model = new ResolutionsResponseDTO
                    {
                        Code = "200",
                        Message = "Success",
                        Choice = post.RG,
                        questionid = question.Id
                    };

                    return Task.FromResult<ResolutionsResponseDTO>(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    db.SaveChanges();
                    var model = new ResolutionsResponseDTO
                    {
                        Code = "500",
                        Message = "Vote not counted",
                        Choice = post.RG
                    };

                    return Task.FromResult<ResolutionsResponseDTO>(model);
                }
            }
            else
            {
                Result result = new Result();
                result.ShareholderNum = post.shareholdernum;
                result.Company = shareholder.Company;
                result.Year = DateTime.Now.Year.ToString();
                result.AGMID = UniqueAGMId;
                result.phonenumber = shareholder.PhoneNumber;
                result.Holding = shareholder.Holding;
                result.Name = post.Name;
                result.splitValue = post.splitvalue;
                result.Address = shareholder.Address;
                result.PercentageHolding = shareholder.PercentageHolding;
                result.QuestionId = question.Id;
                result.date = DateTime.Now;
                result.Timestamp = DateTime.Now.TimeOfDay;
                result.Present = true;
                result.VoteStatus = "Voted";
                result.Source = "Web";

                if (response == "FOR")
                {
                    result.VoteFor = true;
                    result.VoteAgainst = false;
                    result.VoteAbstain = false;
                    result.VoteVoid = false;

                }
                else if (response == "AGAINST")
                {
                    result.VoteAgainst = true;
                    result.VoteFor = false;
                    result.VoteAbstain = false;
                    result.VoteVoid = false;
                }
                else if (response == "ABSTAIN")
                {
                    result.VoteAbstain = true;
                    result.VoteFor = false;
                    result.VoteAgainst = false;
                    result.VoteVoid = false;
                }
                question.result.Add(result);
                db.Entry(shareholder).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                    var model = new ResolutionsResponseDTO
                    {
                        Code = "200",
                        Message = "Success",
                        Choice = post.RG,
                        questionid = question.Id
                    };

                    return Task.FromResult<ResolutionsResponseDTO>(model);

                }
                catch (DbUpdateConcurrencyException)
                {

                    var model = new ResolutionsResponseDTO
                    {
                        Code = "500",
                        Message = "Vote not counted",
                        Choice = post.RG,
                        questionid = question.Id
                    };

                    return Task.FromResult<ResolutionsResponseDTO>(model);
                }
            }

        }


        public Task<string> AutoAdmittanceAsync(string company, string shareholderNum, string accesscode)
        {
            if (!string.IsNullOrEmpty(company) && !string.IsNullOrEmpty(shareholderNum) && !string.IsNullOrEmpty(accesscode))
            {
                var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
                if (UniqueAGMId == -1)
                {
                    return Task.FromResult<string>("Failed");
                }
                var agmstatus = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
                if (agmstatus == null)
                {
                    return Task.FromResult<string>("Failed");
                }
                var sHolderNum = Int64.Parse(shareholderNum);
                //var sholder = db.BarcodeStore.FirstOrDefault(b => b.Company == company && b.ShareholderNum == sHolderNum && b.accesscode==accesscode);
                var shareholdershareholderRecords = db.BarcodeStore.Where(u => u.Company.ToLower() == company.ToLower() && u.ShareholderNum == sHolderNum && u.accesscode == accesscode).ToList();
                if (shareholdershareholderRecords.Any())
                {
                    if (shareholdershareholderRecords.Count() == 1)
                    {
                        var shareholderRecord = shareholdershareholderRecords.First();

                        var shareholder = db.Present.Any(u => u.AGMID == UniqueAGMId && u.ShareholderNum == shareholderRecord.ShareholderNum);

                        shareholderRecord.Present = true;
                        PresentModel present = new PresentModel();
                        present.Name = shareholderRecord.Name;
                        present.Company = shareholderRecord.Company;
                        present.Address = shareholderRecord.Address;
                        present.admitSource = "Web";
                        present.ShareholderNum = shareholderRecord.ShareholderNum;
                        present.Holding = shareholderRecord.Holding;
                        present.AGMID = UniqueAGMId;
                        present.PercentageHolding = shareholderRecord.PercentageHolding;
                        present.present = true;
                        present.Year = currentYear;
                        present.proxy = false;
                        present.PresentTime = DateTime.Now;
                        present.Timestamp = DateTime.Now.TimeOfDay;
                        present.ShareholderNum = shareholderRecord.ShareholderNum;
                        //if (agmstatus.StopAdmittance == true)
                        //{
                        //    present.PermitPoll = 0;
                        //}
                        if (agmstatus.mobileChannel || agmstatus.allChannels)
                        {
                            present.PermitPoll = 1;
                        }
                        else
                        {
                            present.PermitPoll = 0;
                        }
                        if (!String.IsNullOrEmpty(shareholderRecord.PhoneNumber))
                        {
                            if (shareholderRecord.PhoneNumber.StartsWith("234"))
                            {
                                present.PhoneNumber = shareholderRecord.PhoneNumber;
                            }
                            else if (shareholderRecord.PhoneNumber.StartsWith("0"))
                            {
                                double number;
                                if (double.TryParse(shareholderRecord.PhoneNumber, out number))
                                {
                                    number = double.Parse(shareholderRecord.PhoneNumber);
                                    present.PhoneNumber = "234" + number.ToString();
                                }
                                else
                                {
                                    char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
                                    var phonenum = shareholderRecord.PhoneNumber.Split(delimiterChars);
                                    //string phonenumberresult = string.Concat(phonenum);
                                    if (double.TryParse(phonenum[0], out number))
                                    {
                                        number = double.Parse(phonenum[0]);
                                        present.PhoneNumber = "234" + number.ToString();
                                    }

                                }

                            }

                        }
                        present.Clikapad = shareholderRecord.Clikapad;
                        if (shareholderRecord.PresentByProxy != true && !shareholder && agmstatus.StartAdmittance)
                        {
                            db.Present.Add(present);
                            shareholderRecord.Date = DateTime.Today.ToString();
                            db.Entry(shareholderRecord).State = EntityState.Modified;
                        }

                        db.SaveChanges();
                        Functions.PresentCount(UniqueAGMId, true);
                        return Task.FromResult<string>("success");

                    }
                    else if (shareholdershareholderRecords.Count() > 1)
                    {
                        BarcodeModel cshareholderRecord;

                        var checkIfAnyAccountIsProxy = shareholdershareholderRecords.Any(s => s.PresentByProxy == true);
                        var checkifConsolidate = shareholdershareholderRecords.Any(c => c.Consolidated == true);
                        if (!checkifConsolidate)
                        {
                            cshareholderRecord = agmM.ConsolidateRequest(company, shareholdershareholderRecords);
                        }
                        else
                        {
                            var consolidatedvalue = shareholdershareholderRecords.Where(s => s.Consolidated == true).Select(s => s.ConsolidatedValue).FirstOrDefault();

                            cshareholderRecord = agmM.GetConsolidatedAccount(company, consolidatedvalue);
                        }

                        var sshareholderRecord = db.BarcodeStore.Find(cshareholderRecord.Id);
                        var shareholder = db.Present.Any(u => u.AGMID == UniqueAGMId && u.ShareholderNum == sshareholderRecord.ShareholderNum);

                        sshareholderRecord.Present = true;
                        PresentModel present = new PresentModel();
                        present.Name = sshareholderRecord.Name;
                        present.Company = sshareholderRecord.Company;
                        present.Address = sshareholderRecord.Address;
                        present.admitSource = "Web";
                        present.ShareholderNum = sshareholderRecord.ShareholderNum;
                        present.Holding = sshareholderRecord.Holding;
                        present.AGMID = UniqueAGMId;
                        present.PercentageHolding = sshareholderRecord.PercentageHolding;
                        present.present = true;
                        present.proxy = false;
                        present.Year = currentYear;
                        present.PresentTime = DateTime.Now;
                        present.Timestamp = DateTime.Now.TimeOfDay;
                        present.ShareholderNum = sshareholderRecord.ShareholderNum;

                        if (checkIfAnyAccountIsProxy)
                        {
                            present.PermitPoll = 0;
                        }
                        else if (agmstatus.mobileChannel || agmstatus.allChannels)
                        {
                            present.PermitPoll = 1;
                        }
                        else
                        {
                            present.PermitPoll = 0;
                        }

                        if (!String.IsNullOrEmpty(sshareholderRecord.PhoneNumber))
                        {
                            if (sshareholderRecord.PhoneNumber.StartsWith("234"))
                            {
                                present.PhoneNumber = sshareholderRecord.PhoneNumber;
                            }
                            else if (sshareholderRecord.PhoneNumber.StartsWith("0"))
                            {
                                double number;
                                if (double.TryParse(sshareholderRecord.PhoneNumber, out number))
                                {
                                    number = double.Parse(sshareholderRecord.PhoneNumber);
                                    present.PhoneNumber = "234" + number.ToString();
                                }
                                else
                                {
                                    char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
                                    var phonenum = sshareholderRecord.PhoneNumber.Split(delimiterChars);
                                    //string phonenumberresult = string.Concat(phonenum);
                                    if (double.TryParse(phonenum[0], out number))
                                    {
                                        number = double.Parse(phonenum[0]);
                                        present.PhoneNumber = "234" + number.ToString();
                                    }

                                }

                            }

                        }
                        present.Clikapad = sshareholderRecord.Clikapad;
                        if (!shareholder && agmstatus.StartAdmittance)
                        {
                            db.Present.Add(present);
                            sshareholderRecord.Date = DateTime.Today.ToString();
                            db.Entry(sshareholderRecord).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                        Functions.PresentCount(UniqueAGMId, true);
                        return Task.FromResult<string>("success");

                    }



                }
                return Task.FromResult<string>("failed");
            }
            return Task.FromResult<string>("failed");
        }


        private string GenerateWebVotingRequest(int AGMID, int shareholderNum)
        {
            string request = "";

            var query = string.Format("{0}|{1}", shareholderNum, AGMID);
            var encryptedtext = query.Encrypt();
            request = $"?query={encryptedtext}";
            return request;

        }
    }
}