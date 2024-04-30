using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gapplus.Application.DTO.Company.Request;
using Gapplus.Application.Interfaces.Contracts;
using Gapplus.Application.Interfaces.IContracts;
using Microsoft.AspNetCore.Mvc;

namespace Gapplus.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CompanyController : ControllerBase
    {
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
                var response=await _service.CreateCompany(dto);
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                 // TODO

                 throw;
            }
         }
         
         [HttpGet]
         public async Task<ActionResult> GetAllCompanies()
         {
            try
            {
                var response=await _service.GetAllCompany();
                return Ok(response);
            }
            catch (System.Exception ex)
            {
                 // TODO

                 throw;
            }
         }


    }
}