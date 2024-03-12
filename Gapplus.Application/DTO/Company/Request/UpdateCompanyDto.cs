using System.ComponentModel.DataAnnotations;

namespace Gapplus.Application.DTO.Company.Request
{
    public class UpdateCompanyDto
    {
        [Required]
        public String? CompanyName { get; set; }

        [Required]
        public String? CompanyAddress { get; set; }
    }
}
