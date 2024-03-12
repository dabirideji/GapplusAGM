using BarcodeGenerator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Service
{
    public class ResolutionService
    {
        private readonly UsersContext db;
        UserAdmin ua;

        public ResolutionService(UsersContext _db)
        {
            db = _db;
            ua = new UserAdmin(db);
        }

        public Task<List<Question>> GetResolutions(int agmid)
        {
            try
            {
                var resolutions = db.Question.Where(q => q.AGMID == agmid).ToList();
                return Task.FromResult(resolutions);
            }
            catch (Exception e)
            {
                return Task.FromResult(new List<Question>());
            }
        }




        public Task<ResolutionsResponseDTO> TaskResolution(int resolutionId, string resolutionChoice, string company, long shareholderNum)
        {
            int id = resolutionId;
            string response = resolutionChoice;
            var question = db.Question.Find(id);
            var UniqueAGMId = ua.RetrieveAGMUniqueID(company);

            var currentYear = DateTime.Now.Year.ToString();
            var shareholder = db.Present.FirstOrDefault(s => s.AGMID == UniqueAGMId && s.ShareholderNum == shareholderNum);
            if (shareholder == null || shareholder.present != true)
            {
                var model = new ResolutionsResponseDTO
                {
                    Code = "205",
                    Message = "Invalid Vote",
                    Choice = response
                };

                return Task.FromResult<ResolutionsResponseDTO>(model);
            }
            shareholder.TakePoll = true;


            var checkresult = db.Result.FirstOrDefault(r => r.ShareholderNum == shareholderNum && r.QuestionId == id);
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
                        Choice = response,
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
                        Choice = response
                    };

                    return Task.FromResult<ResolutionsResponseDTO>(model);
                }
            }
            else
            {
                Result result = new Result();
                result.ShareholderNum = shareholderNum;
                result.Company = shareholder.Company;
                result.Year = DateTime.Now.Year.ToString();
                result.AGMID = UniqueAGMId;
                result.phonenumber = shareholder.PhoneNumber;
                result.Holding = shareholder.Holding;
                result.Name = shareholder.Name;
                result.Address = shareholder.Address;
                result.PercentageHolding = shareholder.PercentageHolding;
                result.QuestionId = question.Id;
                result.date = DateTime.Now;
                result.Timestamp = DateTime.Now.TimeOfDay;
                result.Pregistered = true;
                result.VoteStatus = "Voted";
                result.Source = "Preregistered";

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
                        Choice = response,
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
                        Choice = response,
                        questionid = question.Id
                    };

                    return Task.FromResult<ResolutionsResponseDTO>(model);
                }
            }
        }


    }
}