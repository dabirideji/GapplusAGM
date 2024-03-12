using BarcodeGenerator.Models;
using BarcodeGenerator.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace BarcodeGenerator.Service
{
    public class MessageService
    {
        private readonly UsersContext _db;

        public MessageService(UsersContext db)
        {
            _db = db;
        }

        public Task<List<AGMQuestion>> GetAllQuestions(int AGMId)
        {
            var questions = (from q in _db.AGMQuestions
                             where q.AGMID == AGMId && q.IsFeedback==false
                             select q).OrderBy(c=>c.Id).ToList();
            return Task.FromResult(questions);
        }

        public Task<APIMessageLog> CreateQuestion(AGMQuestionDto dto)
        {
            APIMessageLog MessageLog = new APIMessageLog();
            try
            {
                AGMQuestion model = new AGMQuestion
                {
                    datetime = DateTime.Now,
                    AGMID = dto.AGMID,
                    Company = dto.Company,
                    emailAddress = dto.emailAddress,
                    holding = dto.holding,
                    PercentageHolding = dto.PercentageHolding,
                    phoneNumber = dto.phoneNumber,
                    shareholderquestion = dto.shareholderquestion,
                    ShareholderName = dto.ShareholderName,
                    ShareholderNumber = dto.ShareholderNumber,
                    MessageType = dto.MessageType,
                    ReplyToEmail = dto.ReplyToEmail,
                    ReplyToMessage = dto.ReplyToMessage,
                    ReplyToName = dto.ReplyToName
                };
                _db.AGMQuestions.Add(model);
                _db.SaveChanges();
                AGMQuestionDto message  = new AGMQuestionDto
                {
                    ShareholderName = model.ShareholderName,
                    datetimeString = model.datetime.ToShortTimeString(),
                    shareholderquestion = model.shareholderquestion,
                    Firstletter = model.ShareholderName.Substring(0, 1),
                    MessageType = model.MessageType,
                    emailAddress = model.emailAddress,
                    ShareholderNumber = model.ShareholderNumber,
                    Id = model.Id,
                    ReplyToEmail = model.ReplyToEmail,
                    ReplyToMessage = model.ReplyToMessage,
                    ReplyToName = model.ReplyToName
                };
              
                Functions.LoadNewMessages(dto.AGMID, message);
                MessageLog = new APIMessageLog
                {
                    ResponseCode = "200",
                    ResponseMessage = "Success",
                };
                return Task.FromResult(MessageLog);
            }
            catch(Exception e)
            {
                MessageLog = new APIMessageLog
                {
                    ResponseCode = "500",
                    ResponseMessage = "Server Error",
                };
                return Task.FromResult(MessageLog);
            }
           
                
        }

        public async Task<APIMessageLog> SendToCustomerCareAsync(AGMQuestionDto dto)
        {
            APIMessageLog MessageLog = new APIMessageLog();
            try
            {
             
                AGMQuestion model = new AGMQuestion
                {
                    datetime = DateTime.Now,
                    AGMID = dto.AGMID,
                    Company = dto.Company,
                    emailAddress = dto.emailAddress,
                    holding = dto.holding,
                    PercentageHolding = dto.PercentageHolding,
                    phoneNumber = dto.phoneNumber,
                    shareholderquestion = dto.shareholderquestion,
                    ShareholderName = dto.ShareholderName,
                    ShareholderNumber = dto.ShareholderNumber,
                    MessageType = dto.MessageType,
                    IsFeedback = true
                };
                _db.AGMQuestions.Add(model);
                _db.SaveChanges();
                    EmailService em = new EmailService();
                    var response = await em.SendEmailToCustomerService(model);
                    MessageLog = new APIMessageLog
                    {
                        ResponseCode = "200",
                        ResponseMessage = "Success",
                    };
                    return MessageLog;



            }
            catch (Exception e)
            {
                var messagelog = new APIMessageLog()
                {
                    ResponseCode = "205",
                    ResponseMessage = e.Message
                };
                return messagelog;
            }

        }

        public Task<AGMQuestion> UpdateQuestion(AGMQuestionDto dto)
        {
            AGMQuestion model = _db.AGMQuestions.Find(dto.Id);

            model.shareholderquestion = dto.shareholderquestion;

            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();

            return Task.FromResult(model);

        }


        public Task<AGMQuestion> GetQuestionById(int id)
        {
            var question = _db.AGMQuestions.Find(id);

            if(question!=null)
                return Task.FromResult(question);

            return Task.FromResult(new AGMQuestion());
        }

        public Task<bool> RemoveQuestion(int id)
        {
            var question = _db.AGMQuestions.Find(id);
            if (question != null)
            {
                _db.AGMQuestions.Remove(question);
                return Task.FromResult(true);

            }
            return Task.FromResult(false);

        }
    }
}