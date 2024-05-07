#region PREVIOUS
    // using System;
    // using System.Collections.Generic;
    // using System.Linq;
    // using System.Threading.Tasks;
    // using Microsoft.AspNetCore.Mvc;
    // using BarcodeGenerator.Models;
    // using BarcodeGenerator.Hubs;
    // using Microsoft.AspNetCore.SignalR;
    
    // namespace Gapplus.Api.Controllers
    // {
    // [ApiController]
    // [Route("api/[controller]/[action]")]
    // public class FakeResolutionController : ControllerBase
    // {
    //     private readonly IHubContext<FakeResolutionHub> _hub;
    //     private readonly UsersContext _context;
    
    //     public FakeResolutionController(IHubContext<FakeResolutionHub> hubContext,UsersContext fakeResolutions)
    //     {
    //         _hub = hubContext;
    //         _context=fakeResolutions;
    //     }
    
    //     [HttpGet]
    //     public ActionResult<IEnumerable<FakeResolutionModel>> Get()
    //     {
    //         return Ok(_context.FakeResolutions.ToList());
    //     }
    
    //     [HttpGet("{id}")]
    //     public ActionResult<FakeResolutionModel> Get(int id)
    //     {
    //         var resolution = _context.FakeResolutions.FirstOrDefault(r => r.Id == id);
    //         if (resolution == null)
    //         {
    //             return NotFound();
    //         }
    //         return Ok(resolution);
    //     }
    
    //     [HttpPost]
    //     public async Task<IActionResult> Post([FromBody] FakeResolutionModel model)
    //     {
    //         if (!ModelState.IsValid)
    //         {
    //             return BadRequest(ModelState);
    //         }
    //         model.Id = _context.FakeResolutions.Count() + 1;
    //         _context.FakeResolutions.Add(model);
    //         await _context.SaveChangesAsync();
    //         return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
    //     }
    
    //     [HttpPut("{id}")]
    //     public async Task<IActionResult> Put(int id, [FromBody] FakeResolutionModel model)
    //     {
    //         var resolution = _context.FakeResolutions.FirstOrDefault(r => r.Id == id);
    //         if (resolution == null)
    //         {
    //             return NotFound();
    //         }
    //         resolution.Question = model.Question;
    //         resolution.CoundownTime = model.CoundownTime;
    //         resolution.IsDisplayed = model.IsDisplayed;
    //         await _context.SaveChangesAsync();
    //         return NoContent();
    //     }
    
    //     [HttpDelete("{id}")]
    //     public async Task<IActionResult> Delete(int id)
    //     {
    //         var resolution = _context.FakeResolutions.FirstOrDefault(r => r.Id == id);
    //         if (resolution == null)
    //         {
    //             return NotFound();
    //         }
    //         _context.FakeResolutions.Remove(resolution);
    //         await _context.SaveChangesAsync();
    //         return NoContent();
    //     }
    
    //     [HttpPost("{id}")]
    //     public async Task<IActionResult> StartResolution(int id)
    //     {
    //         var resolution = _context.FakeResolutions.FirstOrDefault(r => r.Id == id);
    //         if (resolution == null)
    //         {
    //             return NotFound("Resolution not found");
    //         }
    
    //         // Publish the resolution to the frontend
    //     //    await _hub.Clients.All.SendAsync("resolutionStarted",resolution);
    //        await _hub.Clients.All.SendAsync("resolutionStarted",resolution);
    
    //         return Ok(resolution);
    //     }
    // }
    
    
    // }
    
#endregion





using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BarcodeGenerator.Models;
using BarcodeGenerator.Hubs;
using Microsoft.AspNetCore.SignalR;
using Gapplus.Api.Services;

namespace Gapplus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FakeResolutionController : ControllerBase
    {
        private readonly IHubContext<FakeResolutionHub> _hub;
        private readonly IFakeResolutionService _resolutionService;

        public FakeResolutionController(IHubContext<FakeResolutionHub> hubContext, IFakeResolutionService resolutionService)
        {
            _hub = hubContext;
            _resolutionService = resolutionService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FakeResolutionModel>> Get()
        {
            return Ok(_resolutionService.FakeResolutions);
        }
        [HttpGet]
        public ActionResult<IEnumerable<FakeResolutionModel>> GetDisplayedResolutions()
        {
            return Ok(_resolutionService.FakeResolutions.Where(x=>x.IsDisplayed==true).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<FakeResolutionModel> Get(int id)
        {
            var resolution = _resolutionService.FakeResolutions.FirstOrDefault(r => r.Id == id);
            if (resolution == null)
            {
                return NotFound();
            }
            return Ok(resolution);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FakeResolutionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.Id = _resolutionService.FakeResolutions.Count + 1;
            _resolutionService.FakeResolutions.Add(model);
            return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FakeResolutionModel model)
        {
            var resolution = _resolutionService.FakeResolutions.FirstOrDefault(r => r.Id == id);
            if (resolution == null)
            {
                return NotFound();
            }
            resolution.Question = model.Question;
            resolution.CoundownTime = model.CoundownTime;
            resolution.IsDisplayed = model.IsDisplayed;
            return NoContent();
        }


private async Task SendResolutionTimeoutSignal(FakeResolutionModel resolution)
{
    // Send a signal indicating that the resolution has timed out
    await _hub.Clients.All.SendAsync("resolutionTimeout", resolution);
}


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resolution = _resolutionService.FakeResolutions.FirstOrDefault(r => r.Id == id);
            if (resolution == null)
            {
                return NotFound();
            }
            _resolutionService.FakeResolutions.Remove(resolution);
            return NoContent();
        }

        // [HttpPost("{id}")]
        // public async Task<IActionResult> StartResolution(int id)
        // {
        //     var resolution = _resolutionService.FakeResolutions.FirstOrDefault(r => r.Id == id);
        //     if (resolution == null)
        //     {
        //         return NotFound("Resolution not found");
        //     }

        //     // Publish the resolution to the frontend
        //     await _hub.Clients.All.SendAsync("resolutionStarted", resolution);

        //     return Ok(resolution);
        // }


//         [HttpPost("{id}")]
// public async Task<IActionResult> StartResolution(int id)
// {
//     var resolution = _resolutionService.FakeResolutions.FirstOrDefault(r => r.Id == id);
//     if (resolution == null)
//     {
//         return NotFound("Resolution not found");
//     }

//     // Calculate the end time based on the current time and countdown duration
//     var currentTime = DateTime.Now;
//     var endTime = currentTime.AddMinutes(resolution.CoundownTime);

//     // Prepare the response object with the countdown duration, end time, and resolution question
//     var response = new 
//     {
//         Question = resolution.Question,
//         CountdownDuration = resolution.CoundownTime,
//         EndTime = endTime
//     };

//     // Send the response to the frontend
//     await _hub.Clients.All.SendAsync("resolutionStarted", response);

//     // Schedule a timer to call the resolution timeout method
//     var timeoutDuration = TimeSpan.FromMinutes(resolution.CoundownTime);
//     var timer = new Timer(async _ => await SendResolutionTimeoutSignal(resolution), null, timeoutDuration, Timeout.InfiniteTimeSpan);

//     return Ok(response);
// }



[HttpPost("{id}")]
public async Task<IActionResult> StartResolution(int id)
{
    // Cancel any currently open resolution
    await _hub.Clients.All.SendAsync("resolutionCancelled");

    var resolution = _resolutionService.FakeResolutions.FirstOrDefault(r => r.Id == id);
    if (resolution == null)
    {
        return NotFound("Resolution not found");
    }

    // Calculate the end time based on the current time and countdown duration
    var currentTime = DateTime.Now;
    var endTime = currentTime.AddMinutes(resolution.CoundownTime);
    
    // Prepare the response object with the countdown duration, end time, and resolution question
    var response = new 
    {
        Id=resolution.Id,
        Question = resolution.Question,
        CountdownDuration = resolution.CoundownTime,
        EndTime = endTime
    };
    resolution.IsDisplayed=true;
    // Publish the new resolution to the frontend
    await _hub.Clients.All.SendAsync("resolutionStarted", response);

    return Ok(response);
}




    }
}
