// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using System.Web;
// using Microsoft.AspNetCore.Mvc;

// namespace BarcodeGenerator.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]/[action]")]
//     //   [Authorize]
//     public class HomeController : ControllerBase
//     {
//         UsersContext db = new UsersContext();
//         //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

//         //private int RetrieveAGMUniqueID()
//         //{
//         //    var AGMUniqueID = db.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

//         //    return AGMUniqueID;
//         //}

//         public async Task<ActionResult> Index()
//         {
//             var response = await IndexAsync();

//             return View(response);

//         }

//         private Task<UserProfile> IndexAsync()
//         {
//             var user = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
//             return Task.FromResult<UserProfile>(user);
//         }


//         public ActionResult Success()
//         {
//             ViewBag.Message = "Barcode Information has been sent to your email";

//             return View();
//         }

//         public ActionResult Failure()
//         {
//             ViewBag.Message = "Barcode Information Couldn't be sent to email";

//             return View();
//         }

//         public ActionResult Wrong()
//         {

//             ViewBag.Message = "The number entered doesn't exist. Please retry with the correct number.";
//             return View();

//         }

// [HttpGet("{id}")]
//         public ActionResult ResolutionChart(int id)
//         {

//             ViewBag.presentcount = db.Present.Count();

//             ViewBag.shareholders = db.BarcodeStore.Count();
//             var question = db.Question.ToList();
//             // return PartialView(question);
//             return Ok();

//         }
//     }
// }
