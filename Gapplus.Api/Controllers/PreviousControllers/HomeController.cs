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
    //   [Authorize]
    public class HomeController : ControllerBase
    {
        UsersContext db;
        private readonly IViewBagManager _viewBagManager;

        public HomeController(UsersContext _db,IViewBagManager viewBagManager){
            db=_db;
            _viewBagManager=viewBagManager;
        }
        //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

        //private int RetrieveAGMUniqueID()
        //{
        //    var AGMUniqueID = db.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

        //    return AGMUniqueID;
        //}

[HttpGet]
        public async Task<ActionResult> Index()
        {
            var response = await IndexAsync();

            return Ok(response);

        }

        private Task<UserProfile> IndexAsync()
        {
            var user = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
            return Task.FromResult<UserProfile>(user);
        }


        private void Success()
        {
            // ViewBag.Message = "Barcode Information has been sent to your email";
            _viewBagManager.SetValue("Message",  "Barcode Information has been sent to your email");
        }

        private void Failure()
        {
            // ViewBag.Message = "Barcode Information Couldn't be sent to email";
            _viewBagManager.SetValue("Message", "Barcode Information Couldn't be sent to email");
        }

        private void Wrong()
        {

            // ViewBag.Message = "The number entered doesn't exist. Please retry with the correct number.";
            _viewBagManager.SetValue("Message","The number entered doesn't exist. Please retry with the correct number.");


        }

[HttpGet("{id}")]
        public ActionResult ResolutionChart(int id)
        {

            // ViewBag.presentcount = db.Present.Count();
            _viewBagManager.SetValue("presentCount",db.Present.Count());

            // ViewBag.shareholders = db.BarcodeStore.Count();
            _viewBagManager.SetValue("shareholders",db.BarcodeStore.Count());
            var question = db.Question.ToList();
            // return PartialView(question);
            return Ok();

        }
    }
}
