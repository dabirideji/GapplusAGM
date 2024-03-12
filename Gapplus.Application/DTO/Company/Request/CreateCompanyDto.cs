using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gapplus.Application.DTO.Company.Request
{
    public class CreateCompanyDto
    {
        [Required]
        public String? CompanyName { get; set; }

        [Required]
        public String? CompanyAddress { get; set; }
    }
}
