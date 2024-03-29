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
                var JsonString=JsonConvert.SerializeObject(login);
            StringContent content = new StringContent(JsonString, Encoding.UTF8, "application/json");

            var request = await _client.PostAsync("http://localhost:5069/api/Barcode/Login", content);
            if (request.IsSuccessStatusCode) {
                return View();
            }
            return Ok();
        
        }

    }
}
