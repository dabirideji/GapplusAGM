using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gapplus.Application.DTO.ShareHolder.Request;
using Gapplus.Application.DTO.ShareHolder.Response;
using Gapplus.Application.Interfaces.IContracts;
using Gapplus.Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace Gapplus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShareHolderController : ControllerBase
    {
        private readonly IShareHolderContract _service;
        private AutoDefaultResponse<ReadShareHolderDto> x=new();
        public ShareHolderController(IShareHolderContract service)
        {
            _service = service;
        }
         
         [HttpPost]
         public async Task<ActionResult<DefaultResponse<ReadShareHolderDto>>> CreateShareHolder([FromBody] CreateShareHolderDto dto)
         {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            try
            {
                var responseFromService=await _service.CreateShareHolder(dto);
                if(responseFromService==null){
                    return BadRequest(x.ConvertToBad("UNABLE TO CREATE SHAREHOLDER"));
                }
                var response=x.ConvertToGood("SHAREHOLDER CREATED  SUCCESSFULLY",responseFromService);
                return Ok(response);
            }
             catch (System.Exception ex)
            {
               return StatusCode(500,x.ConvertToBad($"{ex.Message}"));
            }
         }



         [HttpGet]
         public async Task<ActionResult<DefaultResponse<List<ReadShareHolderDto>>>> GetAllShareHolders()
         {
            try
            {
                var x=new AutoDefaultResponse<List<ReadShareHolderDto>>();
                var responseFromService=await _service.GetAllShareHolders();
                if(responseFromService==null||responseFromService.Count()==0){
                    return NotFound(x.ConvertToBad("NO DATA FOUND"));
                }
                var response=x.ConvertToGood("DATA FETCHED SUCCESSFULLY",responseFromService.ToList());
                return Ok(response);
            }
            catch (System.Exception ex)
            {
               return StatusCode(500,x.ConvertToBad($"{ex.Message}"));
            }
         }


         [HttpGet("{ShareHolderId}")]
         public async Task<ActionResult<DefaultResponse<ReadShareHolderDto>>> GetShareHolder([FromRoute]Guid ShareHolderId)
         {
            try
            {
                var responseFromService=await _service.GetShareHolder(ShareHolderId);
                if(responseFromService==null){
                    return NotFound(x.ConvertToBad("INVALID SHAREHOLDER ID || SHAREHOLDER NOT FOUND"));
                }
                var response=x.ConvertToGood("DATA FETCHED SUCCESSFULLY",responseFromService);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
               return StatusCode(500,x.ConvertToBad($"{ex.Message}"));
            }
         }


         [HttpGet("{ShareHolderId}/{MeetingId}")]
         public async Task<ActionResult<DefaultResponse<ReadShareHolderDto>>> RegisterForMeeting([FromRoute]Guid ShareHolderId,[FromRoute]Guid MeetingId)
         {
            try
            {
                var responseFromService=await _service.RegisterForMeeting(ShareHolderId,MeetingId);
                if(responseFromService==null){
                    return NotFound(x.ConvertToBad("INVALID SHAREHOLDER ID || SHAREHOLDER NOT FOUND"));
                }
                var response=x.ConvertToGood("REGISTRATION SUCCESSFUL");
                return Ok(response);
            }
            catch (System.Exception ex)
            {
               return StatusCode(500,x.ConvertToBad($"{ex.Message}"));
            }
         }


         [HttpDelete("{ShareHolderId}")]
         public async Task<ActionResult<DefaultResponse<ReadShareHolderDto>>> DeleteShareHolder([FromRoute]Guid ShareHolderId)
         {
            try
            {
                var responseFromService=await _service.DeleteShareHolder(ShareHolderId);
                if(responseFromService==null){
                    return NotFound(x.ConvertToBad("INVALID SHAREHOLDER ID || SHAREHOLDER NOT FOUND"));
                }
                var response=x.ConvertToGood("ACTION SUCCESSFULL");
                return Ok(response);
            }
            catch (System.Exception ex)
            {
               return StatusCode(500,x.ConvertToBad($"{ex.Message}"));
            }
         }


    }
}