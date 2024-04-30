using System.ComponentModel.DataAnnotations;

namespace Gapplus.Application.DTO.Company.Request
{
    public class UpdateCompanyDto
    {
  
        public String CompanyName { get; set; }
    public String CompanyDescription { get; set; }
    public String CompanyImageUrl { get; set; }
    }
}
