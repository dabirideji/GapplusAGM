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
    public class AGMRegistrationController : ControllerBase
    {

        AGMRegistrationService _AGMService;

        ITempDataManager _tempDataManager;
        public AGMRegistrationController(UsersContext context,ITempDataManager tempDataManager)
        {
        _tempDataManager = tempDataManager;
            _AGMService =new AGMRegistrationService(context);
        }





public class AgmRegistrationDto
{
    public string AttendType { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string IdentificationNumber { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public bool ProxySelection { get; set; }
    // public IFormFile ShareHolderCertificate { get; set; }
    // public IFormFile ShareHolderPassport { get; set; }
}

public class dej{
    public IFormFile sch { get; set; }
}

//   [HttpPost("processFormData")]
//         public async Task<IActionResult> ProcessFormData()
//         {
//             try
//             {
//                 // Check if the request contains form data
//                 if (!Request.HasFormContentType)
//                 {
//                     return BadRequest("Request does not contain form data.");
//                 }

//                 // Access the form data
//                 var form = await Request.ReadFormAsync();

//                 // Extract individual form fields
//                 var attendType = form["attendType"];
//                 var email = form["email"];
//                 var firstName = form["firstName"];
//                 var identificationNumber = form["identificationNumber"];
//                 var lastName = form["lastName"];

//                 // Access file uploads
//                 var shareHolderCertificateFile = form.Files["shareHolderCertificate"];
//                 var shareHolderPassportFile = form.Files["shareHolderPassport"];

//                 // Process the form data and file uploads as needed
//                 // Example: save files to disk, database operations, etc.

//                 return Ok("Form data processed successfully.");
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, $"An error occurred: {ex.Message}");
//             }
//         }

        [HttpPost("{company}")]
        public async Task<IActionResult> RegisterAGM([FromRoute]string company,dej dto){
            // var x=new AutoDefaultResponse<AgmCompanies>();
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            try
            {


                return Ok();
                // Check if the request contains form data
                if (!Request.HasFormContentType)
                {
                    return BadRequest("Request does not contain form data.");
                }

                // Access the form data
                var form = await Request.ReadFormAsync();

                // Extract individual form fields
                var attendType = form["attendType"];
                var email = form["email"];
                var firstName = form["firstName"];
                var identificationNumber = form["identificationNumber"];
                var lastName = form["lastName"];

                // Access file uploads
                var shareHolderCertificateFile = form.Files["shareHolderCertificate"];
                var shareHolderPassportFile = form.Files["shareHolderPassport"];

                // Process the form data and file uploads as needed
                // Example: save files to disk, database operations, etc.

                return Ok("Form data processed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
    
        }









        // GET: AGMRegistration


        [HttpGet]
        public async Task<ActionResult> GetActiveAgm()
        {
            AccreditationResponse model = new AccreditationResponse();
            try
            {
                

                //var response = await _client.GetAsync<ServiceResponse<List<string>>>($"{ApiRoutes.getcompany}");
                // var response = await _AGMService.GetActiveAGMCompaniesAsync();

                var response=await _AGMService.GenerateAgmEvent(15);
                if (response==null)
                {

                    model.companies = new List<AGMCompanies>();
                }

                // model.companies = response.Companies ?? new List<AGMCompanies>(); 
                model.companies=response;

            }
            catch
            {
                model.companies = new List<AGMCompanies>();
            }

            // return View(model);
            return Ok(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(SearchModel find)
        {
            if (find.search == null && find.emailsearch == null)
            {

                return RedirectToAction("Empty");
            }
            if (find.company == null)
            {

                return RedirectToAction("Empty");
            }

            try
            {
                var response = await _AGMService.ShareHolderAGMRregistrationAsync(find);
                if (response.ResponseCode == "05")
                {

                    // TempData["Message"] = "Barcode Information Couldn't be sent to email";
                    _tempDataManager.SetTempData("Message", "Barcode Information Couldn't be sent to email");
                    // return RedirectToAction("Failure", "Register");
                    return BadRequest("FAILED");

                }
                else if (response.ResponseCode == "00")
                {
                    // TempData["Message"] = response.ResponseMessage;
                    _tempDataManager.SetTempData("Message",response.ResponseMessage);

                    // return RedirectToAction("Success", "Register");
                    return Ok("Success");

                }
                // TempData["Message"] = "Barcode Information Couldn't be sent to email";

                _tempDataManager.SetTempData("Message","Barcode Information Couldn't be sent to email");

                // return RedirectToAction("Wrong", "Register");
                return BadRequest("Failed to Send Mail");

            }
            catch (Exception e)
            {
                // TempData["Message"] = "Barcode Information Couldn't be sent to email";
                _tempDataManager.SetTempData("Message","Barcode Information Couldn't be sent to email");
                // return RedirectToAction("Failure", "Register");
                return BadRequest("Failed to send mail");

            }


        }
    }
}