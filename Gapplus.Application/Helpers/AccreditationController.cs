// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using BarcodeGenerator.Models;
// using Gapplus.Application.Contracts;
// using Microsoft.AspNetCore.Mvc;

// namespace Gapplus.Api.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class AccreditationController : ControllerBase
//     {
//         private readonly AccreditationService _service; // Assuming AccreditationContract is the contract for your Accreditation service

//         public AccreditationController(AccreditationService service)
//         {
//             _service = service;
//         }

//         [HttpGet]
//         public async Task<ActionResult<AccreditationResponse>> Index()
//         {
//             try
//             {
//                 var response = await _service.IndexAsync();
//                 return Ok(response);
//             }
//             catch (Exception e)
//             {
//                 // Log the exception
//                 return StatusCode(500, "An error occurred while processing the request.");
//             }
//         }
//     }
// }