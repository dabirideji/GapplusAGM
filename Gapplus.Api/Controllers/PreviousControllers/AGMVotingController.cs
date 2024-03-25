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
//     public class AGMVotingController : Controller
//     {
//         WebVotingService _webVotingService = new WebVotingService();
//         // GET: AGMEvent
//         public async Task<ActionResult> Index()
//         {
//             var model = await _webVotingService.IndexAsync();

//             return View(model);
//         }


//         [HttpPost]
//         public async Task<ActionResult> AuthenticateWebVoting(accredationDto model)
//         {
//             var response = await _webVotingService.AccreditationConfirmationPostAsync(model);

//             return Json(response, JsonRequestBehavior.AllowGet);
//         }


//         public async Task<ActionResult> Vote(string query)
//         {

//             var response = await _webVotingService.AccreditationConfirmationAsync(query);
//             return View(response);
//         }


//         [HttpPost]
//         public async Task<ActionResult> TakeResolution(PostResolution post)
//         {

//             var response = await _webVotingService.WebVotingAsync(post);

//             return Json(response, JsonRequestBehavior.AllowGet);
//         }

//     }
// }