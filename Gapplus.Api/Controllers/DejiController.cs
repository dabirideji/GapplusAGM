using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gapplus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DejiController : ControllerBase
    {
        [HttpGet]
        public ActionResult  Dej(){
            return Ok();
        }
    }
}