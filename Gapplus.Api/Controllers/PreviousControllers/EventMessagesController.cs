using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace BarcodeGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EventMessagesController : ControllerBase
    {
        MessageService _messages;
        EmailService _emailService;

        UsersContext db;
        UserAdmin ua;
        AGMManager agmM;
        private readonly IViewBagManager _viewBagManager;
        // GET: EventMessages
        public EventMessagesController(UsersContext context, IViewBagManager viewBagManager)
        {
            db = context;
            _messages = new MessageService(db);
            _emailService = new EmailService();
            ua = new UserAdmin(db);
            agmM = new AGMManager(db);
            _viewBagManager = viewBagManager;

        }


        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var response = await IndexAsync();

            // var returnUrl = HttpContext.Request.Url.AbsolutePath;
            var returnUrl = HttpContext.Request.Path; ;

            string returnvalue = "";

            if (HttpContext.Request.Query.ContainsKey("rel"))
            {
                returnvalue = HttpContext.Request.Query["rel"].ToString();
            }



            // ViewBag.value = returnvalue.Trim();
            _viewBagManager.SetValue("value", returnvalue.Trim());



            if (String.IsNullOrEmpty(response.CompanyInfo))
            {
                // return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
                return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
            }

            // return PartialView(response);
            return Ok(response);

        }


        private async Task<MessageModelIndexDto> IndexAsync()
        {
            var user = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            SettingsModel setting = new SettingsModel();

            var AGMTitle = "";
            if (UniqueAGMId != -1)
            {

                setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
                if (setting != null && !String.IsNullOrEmpty(setting.Title))
                {
                    AGMTitle = setting.Title.ToUpper();


                }

            }


            MessageModelIndexDto dto = new MessageModelIndexDto
            {
                User = user,
                Settings = setting,
                AGMTitle = AGMTitle,
                CompanyInfo = companyinfo,
                AGMID = UniqueAGMId,
                Messages = await _messages.GetAllQuestions(UniqueAGMId),

            };
            return dto;
        }

        [HttpGet]
        public async Task<ActionResult> CreateAGMQuestion(AGMQuestionDto dto)
        {
            var response = await _messages.CreateQuestion(dto);

            // return Json( response, JsonRequestBehavior.AllowGet);
            return Ok(response);
        }


        [HttpGet]
        public async Task<ActionResult> SendFeedbackToCustomerService(AGMQuestionDto dto)
        {
            var response = await _messages.SendToCustomerCareAsync(dto);

            // return Json(response, JsonRequestBehavior.AllowGet);
            return Ok(response);

        }

    }
}