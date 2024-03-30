using Gapplus.Application.Response;
using Gapplus.Web.DTO.ShareHolder;
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
            RefitClient=RestService.For<IShareHolderContract>("http://localhost:5069/api/Barcode");
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

            var refitLogin=await RefitClient.Login(login);
            if(refitLogin.IsSuccessStatusCode){
                var ResponseData=JsonConvert.DeserializeObject<DefaultResponse<ShareHolderViewModel>>(await refitLogin.Content.ReadAsStringAsync());
                var data=ResponseData.Data;
                ViewBag.Name=data.Name;
                ViewBag.Email=data.emailAddress;
                return View();
            }

                var JsonString=JsonConvert.SerializeObject(login);
            StringContent content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            var request = await _client.PostAsync("http://localhost:5069/api/Barcode/Login", content);
            if (request.IsSuccessStatusCode) {
                return View();
            }
            return Ok();
        
        }



        public IActionResult ShareHoldingsDashboard()
        {
            return View();
        }


    }
}
