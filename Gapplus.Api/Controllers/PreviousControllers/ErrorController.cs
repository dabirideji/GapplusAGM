using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace BarcodeGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ErrorController : ControllerBase
    {
        // GET: Error
        [HttpGet]
        public ActionResult Index()
        {
            // return View();
            return Ok();
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            // return View();
            return Ok();
        }

        [HttpGet]
        public ActionResult Error()
        {
            // return View();
            return Ok();
        }

    }
}