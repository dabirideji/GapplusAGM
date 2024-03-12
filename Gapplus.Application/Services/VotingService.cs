using BarcodeGenerator.Models;
using BarcodeGenerator.Models.ModelDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Service
{
    public class VotingService
    {

     private readonly UsersContext db;

        public VotingService(UsersContext _db)
        {
            db = _db;
        }


        public Task<GenericResponseDto<string>> VoteReset(int resolutionId)
        {
            try
            {
                if (resolutionId != 0)
                {
                    List<Result> results = db.Result.Where(r => r.QuestionId == resolutionId).ToList();
                    if (results != null)
                    {
                        db.Result.RemoveRange(results);
                    }


                    db.SaveChanges();

                    return Task.FromResult(new GenericResponseDto<string>
                    {
                        Status = true,
                        Message = "success"
                    });
                }

                return Task.FromResult(new GenericResponseDto<string>
                {
                    Status = false,
                    Message = "Results couldn't be cleared."
                });
            }
            catch (Exception e)
            {
                return Task.FromResult(new GenericResponseDto<string>
                {
                    Status = false,
                    Code=500,
                    Message = "Results couldn't be cleared."
                });

            }
        }

    }
}