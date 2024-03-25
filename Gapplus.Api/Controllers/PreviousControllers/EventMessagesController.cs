// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using System.Web;
// using System.Web.Mvc;

// namespace BarcodeGenerator.Controllers
// {
//     public class EventMessagesController : Controller
//     {
//         MessageService _messages = new MessageService();
//         EmailService _emailService = new EmailService();

//         UsersContext db = new UsersContext();
//         UserAdmin ua = new UserAdmin();
//         AGMManager agmM = new AGMManager();
//         // GET: EventMessages
//         public async Task<ActionResult> Index()
//         {
//             var response = await IndexAsync();

//             var returnUrl = HttpContext.Request.Url.AbsolutePath;

//             string returnvalue = "";

//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();

//             if (String.IsNullOrEmpty(response.CompanyInfo))
//             {
//                 return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
//             }

//             return PartialView(response);

//         }


//         public async Task<MessageModelIndexDto> IndexAsync()
//         {
//             var user = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             SettingsModel setting = new SettingsModel();

//             var AGMTitle = "";
//             if (UniqueAGMId != -1)
//             {

//                 setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
//                 if (setting != null && !String.IsNullOrEmpty(setting.Title))
//                 {
//                     AGMTitle = setting.Title.ToUpper();
   

//                 }

//             }


//             MessageModelIndexDto dto = new MessageModelIndexDto
//             {
//                 User = user,
//                 Settings = setting,
//                 AGMTitle = AGMTitle,
//                 CompanyInfo = companyinfo,
//                 AGMID = UniqueAGMId,
//                 Messages = await _messages.GetAllQuestions(UniqueAGMId),

//             };
//             return dto;
//         }

//         public async Task<ActionResult> CreateAGMQuestion(AGMQuestionDto dto)
//         {
//             var response = await _messages.CreateQuestion(dto);

//             return Json( response, JsonRequestBehavior.AllowGet);
//         }


//         public async Task<ActionResult> SendFeedbackToCustomerService(AGMQuestionDto dto)
//         {
//             var response = await _messages.SendToCustomerCareAsync(dto);

//             return Json(response, JsonRequestBehavior.AllowGet);
//         }

//     }
// }