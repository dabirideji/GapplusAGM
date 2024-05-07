using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarcodeGenerator.Models;
using BarcodeGenerator.Models.ModelDTO;
using Gapplus.Application.Contracts;
using Gapplus.Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace Gapplus.Api.Controllers.ShareHolder
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BarcodeController : ControllerBase
    {
        private AutoApiResponse<string> x = new();
        private readonly BarcodeContract _contract;

        public BarcodeController(UsersContext ctx)
        {
            _contract = new BarcodeContract(ctx);
        }











        [HttpPost]
        public async Task<ActionResult<GenericAPIResponseDTO<BarcodeModel>>> Create([FromBody] BarcodeModelDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var x=new AutoApiResponse<BarcodeModel>();
                var createdBarcode = await _contract.Create(model);
                if (createdBarcode == null)
                {
                    return BadRequest(x.ConvertToBad("UNABE TO CREATE A BARCODE (SHAREHOLDER)"));
                }
                return x.ConvertToGood("Barcode created successfully", createdBarcode);
            }
            catch (Exception ex)
            {
                return StatusCode(500, x.ConvertToBad($"Failed to create barcode: {ex.Message}"));
            }
        }


















        [HttpPost("{id}")]
        public async Task<ActionResult<GenericAPIResponseDTO<String>>> Present([FromRoute] int id, string name,[FromBody] QuestionStatus dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                SessionManager.SetSessionData("Name",name);

                var presentRequest = await _contract.PresentAsync(id, dto);
                if (presentRequest.ToString().ToLower() != "success")
                {
                    return BadRequest(x.ConvertToBad($"ACTION FAILED || {presentRequest}"));
                }
                return x.ConvertToGood("ACTION SUCCESSFUL", "Success");
            }
            catch (System.Exception ex)
            {

                return StatusCode(500, x.ConvertToBad($"{ex.Message}"));
            }
        }

     

        [HttpPost("{id}")]
        public async Task<ActionResult<GenericAPIResponseDTO<String>>> Edit([FromRoute] int id, [FromBody] PresentModel dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var presentRequest = await _contract.EditAsync(id, dto);
                if (presentRequest.ToString().ToLower() != "success")
                {
                    return BadRequest(x.ConvertToBad($"ACTION FAILED || {presentRequest}"));
                }
                return x.ConvertToGood("ACTION SUCCESSFUL", "Success");
            }
            catch (System.Exception ex)
            {

                return StatusCode(500, x.ConvertToBad($"{ex.Message}"));
            }
        }
    }
}