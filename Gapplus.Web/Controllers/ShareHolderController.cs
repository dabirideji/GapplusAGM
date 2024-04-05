using BarcodeGenerator.Models;
using Gapplus.Application.Response;
using Gapplus.Web.DTO.ShareHolder;
using Gapplus.Web.Models;
using Gapplus.Web.RefitContracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Refit;
using System.Text;
using System.Text.Json.Serialization;

namespace Gapplus.Web.Controllers
{
    public class ShareHolderController : Controller
    {
        private readonly IShareHolderContract RefitClient;

        private readonly HttpClient _client;
        public ShareHolderController(HttpClient cl)
        {
            RefitClient = RestService.For<IShareHolderContract>("http://localhost:5069/api/Barcode");
            _client = cl;
            _client.BaseAddress = new Uri("http://localhost:5069/api/Barcode");

        }
        public IActionResult Index()
        {
            return View("SignIn");
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {

            var refitLogin = await RefitClient.Login(login);
            if (refitLogin.IsSuccessStatusCode)
            {
                var ResponseData = JsonConvert.DeserializeObject<DefaultResponse<ShareHolderViewModel>>(await refitLogin.Content.ReadAsStringAsync());
                var data = ResponseData.Data;
                HttpContext.Session.SetString("ShareHolderData",JsonConvert.SerializeObject(data));
                ViewBag.Name = data.Name;
                ViewBag.Email = data.emailAddress;
                // return RedirectToAction("ShareHoldingsDashboard", data);
                return RedirectToAction("ShareHoldingsDashboard");
            }
            return Ok("Login Failed");
        }


private async Task<ShareHolderViewModel?> GetShareHolderData(){
    var shareholderData=HttpContext.Session.GetString("ShareHolderData");
    if(shareholderData==null){
        return null;
    }
    var data=JsonConvert.DeserializeObject<ShareHolderViewModel>(shareholderData);
    return data;
}




        public async Task<IActionResult> ShareHoldingsDashboard()
        {
            var refitClient = RestService.For<IAGMContract>("http://localhost:5069/api/AGMRegistration");
            var response = await refitClient.GetActiveAgm();
            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonConvert.DeserializeObject<Gapplus.Web.Models.AccreditationResponse>(await response.Content.ReadAsStringAsync());
               await GetDataAndSetViewBag();
                ShareholderDashboardViewModel dashboardData = new();
                dashboardData.companies = responseData.companies;
                return View("ShareHoldingsDashboard", dashboardData);
            }
            return Unauthorized("INVALID LOGIN CREDENTIALS");
        }



        public IActionResult AllAGMPage()
        {

            return View();
        }

    private async Task<ShareHolderViewModel?> GetDataAndSetViewBag(){
    var data=await GetShareHolderData();
        ViewBag.Name=data.Name??"";
        ViewBag.Email=data.emailAddress;
        return data;
    }

        public async Task<PartialViewResult> GetAgmPartialViewsAsync()
        {
            var refitClient = RestService.For<IAGMContract>("http://localhost:5069/api/AGMRegistration");
            var response = await refitClient.GetActiveAgm();
            var responseData=new Gapplus.Web.Models.AccreditationResponse();
            if (response.IsSuccessStatusCode)
            {
                responseData = JsonConvert.DeserializeObject<Gapplus.Web.Models.AccreditationResponse>(await response.Content.ReadAsStringAsync());
                await GetDataAndSetViewBag();
            }
            return PartialView("_AllAgms",responseData);
        }





        public IActionResult BoardOfDirectors()
        {
            return View();
        }

        public IActionResult DocumentPage()
        {
            return View();
        }

        public IActionResult RegisterPage()
        {
            return View();
        }
        public IActionResult RegisterPage2()
        {
            return View();
        }

        public IActionResult RegisterPage3()
        {
            return View();
        }

        public IActionResult RegisterPage4()
        {
            return View();
        }


        public IActionResult ReviewAndConfirmPage()
        {
            return View();
        }

        public IActionResult SuccessPage()
        {
            return View();
        }


        public IActionResult MeetingLoginPage()
        {
            return View();
        }

        public IActionResult MeetingMessagePage()
        {
            return View();
        }

        public IActionResult MeetingPage()
        {
            return View("MeetingPage");
        }
        public IActionResult MeetingResolutionPage()
        {
            return View();
        }
    }
}
