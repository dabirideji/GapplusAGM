using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BarcodeGenerator.Controllers
{
    // [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClikapadAPIController : ControllerBase
    {

        UsersContext db;
        Stopwatch stopwatch ;
        UserAdmin ua;

        public ClikapadAPIController(UsersContext context)
        {
            db=context;   
            stopwatch = new Stopwatch();
            ua = new UserAdmin(db);
        }
        public static List<KeypadResults> keypadResults = new List<KeypadResults>();
        // GET api/<controller>
        [HttpGet]
        public string GetAGMID(int id)
        {
            var companyAGMID = db.Settings.SingleOrDefault(s => s.AGMID == id);
            if(companyAGMID!=null)
            {
                return companyAGMID.CompanyName;
            }

            return "";

        }

        // GET api/<controller>/5

        private string Get(int id)
        {
            return "value";
        }


        [HttpGet]
        public async Task<bool> ChecIfClikapadExist(string id)
        {
            var response = await ua.CheckIfClikapadIsAssigned(id);

            return response;
        }

        private Task<bool> CheckIfClikapadIsAssigned(string clikapad)
        {
            bool status;
            var pad = clikapad;
            if(string.IsNullOrEmpty(pad) || pad == "0")
            {
                status = false;
                return Task.FromResult(status);
            }

            status = db.BarcodeStore.Any(p => p.Clikapad.Trim() == pad.Trim()) || db.Present.Any(p => p.Clikapad.Trim() == pad.Trim());
            return Task.FromResult(status);
        }


        // POST api/<controller>
        [HttpPost]
        public async Task<bool> VotingResponse([FromBody] ClikapadReceiveDto post)
        {
            var UniqueAGMId = int.Parse(post.pagmid.Trim());
            KeypadResults resultvalue = new KeypadResults
            {
                AGMID = UniqueAGMId,
                voteReceived = post.pVotes,
                Company = post.pcompany,
                TimeReceived = post.ptime,
                Keypad = post.pKeypad,
                Keyvalue = post.pKeyvalue,
                Valuechecked = false
            };
            db.KeypadResults.Add(resultvalue);
            db.SaveChanges();
            var response = await VotingResponseAsync(post.pcompany);

            return response;
        }

        // POST api/votecollectionapi
        private Task<bool> VotingResponseAsync(string company)
        {
            try
            {
                //var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
                if (UniqueAGMId == -1)
                {
                    return Task.FromResult<bool>(false);
                }
                var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                //if (agmEvent != null)
                //{
                //    if (agmEvent.allChannels != true || agmEvent.webChannel != true)
                //    {
                //        return Task.FromResult<bool>(false);
                //    }
                //}

                var abstainBtnchoice = true;
                var currentYear = DateTime.Now.Year.ToString();
                if (agmEvent != null && agmEvent.AbstainBtnChoice != null)
                {
                    abstainBtnchoice = (bool)agmEvent.AbstainBtnChoice;
                }



                if (TimerControll.GetTimeStatus(company))
                {
                    var clikapadResults = db.KeypadResults.Where(r => r.AGMID == UniqueAGMId && r.Valuechecked == false);
                    var question = db.Question.SingleOrDefault(q => q.AGMID == UniqueAGMId && q.questionStatus == true);
                    foreach (var resultvalue in clikapadResults)
                    {
                        var keypad = resultvalue.Keypad.Trim();
                        if (abstainBtnchoice == false && resultvalue.Keyvalue.Contains("3"))
                        {
                            continue;
                        }
                        using (UsersContext qdb = db)
                        {
                            var voter = qdb.Present.SingleOrDefault(q => q.AGMID == UniqueAGMId && q.Clikapad == keypad);
                            if (voter != null && question != null)
                            {


                                var checkresult = qdb.Result.SingleOrDefault(r => r.AGMID == UniqueAGMId && r.ShareholderNum == voter.ShareholderNum && r.QuestionId == question.Id);
                                //if true - edit value
                                if (checkresult != null)
                                {
                                    checkresult.date = DateTime.Now;
                                    checkresult.Timestamp = DateTime.Now.TimeOfDay;
                                    checkresult.VoteStatus = "Voted";
                                    checkresult.Source = "Pad";
                                    if (resultvalue.Keyvalue.Contains("1"))
                                    {
                                        checkresult.VoteFor = true;
                                        checkresult.VoteAgainst = false;
                                        checkresult.VoteAbstain = false;
                                        checkresult.VoteVoid = false;
                                    }
                                    else if (resultvalue.Keyvalue.Contains("2"))
                                    {
                                        checkresult.VoteAgainst = true;
                                        checkresult.VoteFor = false;
                                        checkresult.VoteAbstain = false;
                                        checkresult.VoteVoid = false;
                                    }
                                    else if (resultvalue.Keyvalue.Contains("3"))
                                    {
                                        checkresult.VoteAbstain = true;
                                        checkresult.VoteFor = false;
                                        checkresult.VoteAgainst = false;
                                        checkresult.VoteVoid = false;
                                    }
                                    qdb.Entry(checkresult).State = EntityState.Modified;
                                    try
                                    {
                                        qdb.SaveChanges();

                                    }
                                    catch (Exception e)
                                    {
                                        continue;
                                    }


                                }
                                else
                                {
                                    Result result = new Result();
                                    result.Name = voter.Name;
                                    result.Company = voter.Company;
                                    result.ShareholderNum = voter.ShareholderNum;
                                    result.Address = voter.Address;
                                    result.Holding = voter.Holding;
                                    result.EmailAddress = voter.emailAddress;
                                    result.phonenumber = voter.PhoneNumber;
                                    result.Clickapad = voter.Clikapad;
                                    result.Year = currentYear;
                                    result.PercentageHolding = voter.PercentageHolding;
                                    result.QuestionId = question.Id;
                                    result.AGMID = UniqueAGMId;
                                    result.date = DateTime.Now;
                                    result.Timestamp = DateTime.Now.TimeOfDay;
                                    result.VoteStatus = "Voted";
                                    result.Source = "Pad";
                                    result.Present = true;
                                    if (resultvalue.Keyvalue.Contains("1"))
                                    {
                                        result.VoteFor = true;
                                        result.VoteAgainst = false;
                                        result.VoteAbstain = false;
                                        result.VoteVoid = false;

                                    }
                                    else if (resultvalue.Keyvalue.Contains("2"))
                                    {
                                        result.VoteAgainst = true;
                                        result.VoteFor = false;
                                        result.VoteAbstain = false;
                                        result.VoteVoid = false;
                                    }
                                    else if (resultvalue.Keyvalue.Contains("3"))
                                    {
                                        result.VoteAbstain = true;
                                        result.VoteFor = false;
                                        result.VoteAgainst = false;
                                        result.VoteVoid = false;
                                    }
                                    qdb.Result.Add(result);
                                }

                                voter.TakePoll = true;
                                qdb.Entry(voter).State = EntityState.Modified;
                                try
                                {
                                    qdb.SaveChanges();

                                }
                                catch (Exception e)
                                {
                                    continue;
                                }
                            }

                        }
                        resultvalue.Valuechecked = true;
                        db.Entry(resultvalue).State = EntityState.Modified;
                    }
                    db.SaveChanges();

                    //if (!TimerControll.GetTimeStatus(company))
                    //    break;
                }
                //return System.Threading.Tasks.Task.FromResult<bool>(true);
                return Task.FromResult<bool>(true);
            }
            catch (Exception e)
            {
                return Task.FromResult<bool>(false);
            }
        }




        //// POST api/<controller>
        //[HttpPost]
        //public async Task<string> VotingResponse([FromBody]ClikapadReceiveDto post)
        //{

        //    var response = await VotingResponseAsync(post);

        //    return response;
        //}

        //// POST api/votecollectionapi
        //private Task<string> VotingResponseAsync(ClikapadReceiveDto post)
        //{
        //    try
        //    {
        //        var company = "";
        //        if(string.IsNullOrEmpty(post.pagmid) || string.IsNullOrEmpty(post.pKeypad))
        //        {
        //            return Task.FromResult<string>("Invalid");
        //        }
        //        var UniqueAGMId = int.Parse(post.pagmid.Trim());

        //        var agmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

        //        var abstainBtnchoice = true;

        //        if (agmEvent != null && agmEvent.AbstainBtnChoice != null)
        //        {
        //            abstainBtnchoice = (bool)agmEvent.AbstainBtnChoice;
        //        }

        //        if (agmEvent==null || string.IsNullOrEmpty(agmEvent.CompanyName))
        //        {
        //            return Task.FromResult<string>("Invalid");
        //        }
        //        company = agmEvent.CompanyName.Trim();

        //        //if (agmEvent != null)
        //        //{
        //        //    if (agmEvent.allChannels != true || agmEvent.webChannel != true)
        //        //    {
        //        //        return Task.FromResult<string>("Channel not enabled for voting.");
        //        //    }
        //        //}
        //        if (TimerControll.GetTimeStatus(company))
        //        {
        //            //var clikapadResults = db.KeypadResults.Where(r => r.AGMID == UniqueAGMId && r.Valuechecked == false);
        //            var question = db.Question.SingleOrDefault(q => q.AGMID == UniqueAGMId && q.questionStatus == true);
        //            if (question == null)
        //            {
        //                return Task.FromResult<string>("Failed");
        //            }

        //            if (abstainBtnchoice==false && post.pKeyvalue.Contains("3"))
        //            {
        //                return Task.FromResult<string>("Failed");
        //            }
        //            //    var keypad = resultvalue.Keypad.Trim();
        //            using (UsersContext qdb = new UsersContext())
        //            {
        //                var checkvoters = db.Present.Where(p => p.AGMID == UniqueAGMId && p.Clikapad.Trim() == post.pKeypad.Trim());
        //                if (checkvoters != null && checkvoters.Count() == 1)
        //                {
        //                    var voter = checkvoters.First();
        //                    var checkresult = qdb.Result.SingleOrDefault(r => r.ShareholderNum == voter.ShareholderNum && r.QuestionId == question.Id);
        //                    //if true - edit value
        //                    if (checkresult != null)
        //                    {
        //                        checkresult.date = DateTime.Now;
        //                        checkresult.Timestamp = DateTime.Now.TimeOfDay;
        //                        checkresult.VoteStatus = "Voted";
        //                        checkresult.Source = "Pad";
        //                        if (post.pKeyvalue.Contains("1"))
        //                        {
        //                            checkresult.VoteFor = true;
        //                            checkresult.VoteAgainst = false;
        //                            checkresult.VoteAbstain = false;
        //                        }
        //                        else if (post.pKeyvalue.Contains("2"))
        //                        {
        //                            checkresult.VoteAgainst = true;
        //                            checkresult.VoteFor = false;
        //                            checkresult.VoteAbstain = false;
        //                        }
        //                        else if (post.pKeyvalue.Contains("3"))
        //                        {
        //                                checkresult.VoteAbstain = true;
        //                                checkresult.VoteFor = false;
        //                                checkresult.VoteAgainst = false;
        //                        }
        //                        qdb.Entry(checkresult).State = EntityState.Modified;
        //                        try
        //                        {
        //                            qdb.SaveChanges();

        //                        }
        //                        catch (DbUpdateConcurrencyException e)
        //                        {

        //                        }


        //                    }
        //                    else
        //                    {
        //                        Result result = new Result();
        //                        result.Name = voter.Name;
        //                        result.ShareholderNum = voter.ShareholderNum;
        //                        result.Address = voter.Address;
        //                        result.Holding = voter.Holding;
        //                        result.phonenumber = voter.PhoneNumber;
        //                        result.Clickapad = voter.Clikapad;
        //                        result.PercentageHolding = voter.PercentageHolding;
        //                        result.QuestionId = question.Id;
        //                        result.AGMID = UniqueAGMId;
        //                        result.date = DateTime.Now;
        //                        result.Timestamp = DateTime.Now.TimeOfDay;
        //                        result.VoteStatus = "Voted";
        //                        result.Source = "Pad";
        //                        result.Present = true;
        //                        if (post.pKeyvalue.Contains("1"))
        //                        {
        //                            result.VoteFor = true;
        //                            result.VoteAgainst = false;
        //                            result.VoteAbstain = false;

        //                        }
        //                        else if (post.pKeyvalue.Contains("2"))
        //                        {
        //                            result.VoteAgainst = true;
        //                            result.VoteFor = false;
        //                            result.VoteAbstain = false;
        //                        }
        //                        else if (post.pKeyvalue.Contains("3"))
        //                        {
        //                                result.VoteAbstain = true;
        //                                result.VoteFor = false;
        //                                result.VoteAgainst = false;
        //                        }
        //                        qdb.Result.Add(result);
        //                        voter.TakePoll = true;
        //                        qdb.Entry(voter).State = EntityState.Modified;
        //                        try
        //                        {
        //                            qdb.SaveChanges();

        //                        }
        //                        catch (DbUpdateConcurrencyException e)
        //                        {

        //                        }
        //                    }

        //                    return Task.FromResult<string>("success");
        //                }
        //                else if (checkvoters != null && checkvoters.Count() > 1)
        //                {
        //                    foreach (var item in checkvoters)
        //                    {
        //                        var voter = item;
        //                        var checkresult = qdb.Result.SingleOrDefault(r => r.ShareholderNum == voter.ShareholderNum && r.QuestionId == question.Id);
        //                        //if true - edit value
        //                        if (checkresult != null)
        //                        {
        //                            checkresult.date = DateTime.Now;
        //                            checkresult.Timestamp = DateTime.Now.TimeOfDay;
        //                            checkresult.VoteStatus = "Voted";
        //                            checkresult.Source = "Pad";
        //                            if (post.pKeyvalue.Contains("1"))
        //                            {
        //                                checkresult.VoteFor = true;
        //                                checkresult.VoteAgainst = false;
        //                                checkresult.VoteAbstain = false;
        //                            }
        //                            else if (post.pKeyvalue.Contains("2"))
        //                            {
        //                                checkresult.VoteAgainst = true;
        //                                checkresult.VoteFor = false;
        //                                checkresult.VoteAbstain = false;
        //                            }
        //                            else if (post.pKeyvalue.Contains("3"))
        //                            {
        //                                    checkresult.VoteAbstain = true;
        //                                    checkresult.VoteFor = false;
        //                                    checkresult.VoteAgainst = false;
        //                            }
        //                            qdb.Entry(checkresult).State = EntityState.Modified;
        //                            try
        //                            {
        //                                qdb.SaveChanges();

        //                            }
        //                            catch (DbUpdateConcurrencyException e)
        //                            {

        //                            }


        //                        }
        //                        else
        //                        {
        //                            Result result = new Result();
        //                            result.Name = voter.Name;
        //                            result.ShareholderNum = voter.ShareholderNum;
        //                            result.Address = voter.Address;
        //                            result.Holding = voter.Holding;
        //                            result.phonenumber = voter.PhoneNumber;
        //                            result.Clickapad = voter.Clikapad;
        //                            result.PercentageHolding = voter.PercentageHolding;
        //                            result.QuestionId = question.Id;
        //                            result.AGMID = UniqueAGMId;
        //                            result.date = DateTime.Now;
        //                            result.Timestamp = DateTime.Now.TimeOfDay;
        //                            result.VoteStatus = "Voted";
        //                            result.Source = "Pad";
        //                            result.Present = true;
        //                            if (post.pKeyvalue.Contains("1"))
        //                            {
        //                                result.VoteFor = true;
        //                                result.VoteAgainst = false;
        //                                result.VoteAbstain = false;

        //                            }
        //                            else if (post.pKeyvalue.Contains("2"))
        //                            {
        //                                result.VoteAgainst = true;
        //                                result.VoteFor = false;
        //                                result.VoteAbstain = false;
        //                            }
        //                            else if (post.pKeyvalue.Contains("3"))
        //                            {
        //                                    result.VoteAbstain = true;
        //                                    result.VoteFor = false;
        //                                    result.VoteAgainst = false;
        //                            }
        //                            qdb.Result.Add(result);
        //                            voter.TakePoll = true;
        //                            qdb.Entry(voter).State = EntityState.Modified;
        //                            try
        //                            {
        //                                qdb.SaveChanges();

        //                            }
        //                            catch (DbUpdateConcurrencyException e)
        //                            {

        //                            }
        //                        }

        //                    }
        //                    KeypadResults resultvalue = new KeypadResults
        //                    {
        //                        AGMID = UniqueAGMId,
        //                        voteReceived = post.pVotes,
        //                        Company = post.pcompany,
        //                        TimeReceived = post.ptime,
        //                        Keypad = post.pKeypad,
        //                        Keyvalue = post.pKeyvalue,
        //                        Valuechecked = true
        //                    };
        //                    db.KeypadResults.Add(resultvalue);
        //                    db.SaveChanges();
        //                    return Task.FromResult<string>("success");
        //                }
        //                else
        //                {
        //                    return Task.FromResult<string>("Failed");
        //                }
        //            }
        //        }
        //        return Task.FromResult<string>("Failed");
        //    }
        //    catch (Exception e)
        //    {
        //        return Task.FromResult<string>("Failed");
        //    }
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