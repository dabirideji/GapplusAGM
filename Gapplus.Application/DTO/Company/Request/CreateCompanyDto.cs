using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gapplus.Application.DTO.Company.Request
{
    public class CreateCompanyDto
    {
      
        public String CompanyName { get; set; }
    public String CompanyDescription { get; set; }
    public String CompanyImageUrl { get; set; }
    public String CompanyRegNo { get; set; }
    }




}
