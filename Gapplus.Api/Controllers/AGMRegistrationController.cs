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


        // GET: AGMRegistration

        [HttpGet]
        public async Task<ActionResult> GetActiveAgm()
        {
            AccreditationResponse model = new AccreditationResponse();
            try
            {
                

                //var response = await _client.GetAsync<ServiceResponse<List<string>>>($"{ApiRoutes.getcompany}");
                // var response = await _AGMService.GetActiveAGMCompaniesAsync();

                var response=await _AGMService.GenerateAgmEvent(3);
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