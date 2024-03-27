using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AGMVotingController : ControllerBase
    {
        WebVotingService _webVotingService;

        public AGMVotingController(UsersContext context)
        {
             _webVotingService = new WebVotingService(context);
        }
        // GET: AGMEvent
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = await _webVotingService.IndexAsync();

            // return View(model);
            return Ok(model);
        }


        [HttpPost]
        public async Task<ActionResult> AuthenticateWebVoting(accredationDto model)
        {
            var response = await _webVotingService.AccreditationConfirmationPostAsync(model);

            // return Json(response, JsonRequestBehavior.AllowGet);
            return Ok(response);
        }


        [HttpGet]
        public async Task<ActionResult> Vote(string query)
        {

            var response = await _webVotingService.AccreditationConfirmationAsync(query);
            // return View(response);
            return Ok(response);
        }


        [HttpPost]
        public async Task<ActionResult> TakeResolution(PostResolution post)
        {

            var response = await _webVotingService.WebVotingAsync(post);

            // return Json(response, JsonRequestBehavior.AllowGet);
            return Ok(response);
        }

    }
}