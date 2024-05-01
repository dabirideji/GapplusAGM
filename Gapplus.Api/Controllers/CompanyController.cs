using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gapplus.Application.DTO.Company.Request;
using Gapplus.Application.DTO.Company.Response;
using Gapplus.Application.Interfaces.Contracts;
using Gapplus.Application.Interfaces.IContracts;
using Gapplus.Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace Gapplus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CompanyController : ControllerBase
    {
        AutoDefaultResponse<ReadCompanyDto>x=new();
        private readonly ICompanyContract _service;

        public CompanyController(ICompanyContract service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> CreateCompany([FromBody] CreateCompanyDto dto)
        {
            try
            {
                var response = await _service.CreateCompany(dto);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
             return StatusCode(500,x.ConvertToBad($"{ex.Message}"));
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCompanies()
        {
            try
            {
                var response = await _service.GetAllCompany();
                return Ok(response);
            }
           catch (System.Exception ex)
            {
             return StatusCode(500,x.ConvertToBad($"{ex.Message}"));
            }
        }

        
            [HttpGet("{ShareHolderId}")]
            public async Task<ActionResult> GetShareHolderCompanies([FromRoute] Guid ShareHolderId)
            {
                try
                {
                    var x=new AutoDefaultResponse<List<ReadCompanyDto>>();
                    var responseFromService = await _service.GetShareHolderCompanies(ShareHolderId);
                    if(responseFromService==null||responseFromService.Count()==0){
                        return NotFound(x.ConvertToBad("NO DATA FOUND"));
                    }
                    var response=x.ConvertToGood("DATA FETHED SUCCESSFULLY",responseFromService.ToList());
                    return Ok(response);
                }
               catch (System.Exception ex)
            {
             return StatusCode(500,x.ConvertToBad($"{ex.Message}"));
            }
            }

    }}