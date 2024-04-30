using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gapplus.Domain.Categories;

namespace Gapplus.Application.DTO.Company.Response
{
    public class ReadCompanyDto
    {
        public Guid CompanyId { get; set; }
        public String? CompanyName { get; set; }
    public String CompanyDescription { get; set; }
       public String CompanyImageUrl { get; set; }
       
     public String CompanyRegNo { get; set; }
    public List<String>? Tags { get; set; }
    public CompanyStatus CompanyStatus { get; set; }
    public DateTime CompanyCreatedAt { get; set; }
    public DateTime CompanyUpdatedAt { get; set; }
    }
}


    


