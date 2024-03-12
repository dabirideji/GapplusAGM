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
        public String? CompanyAddress { get; set; }
        public CompanyStatus CompanyStatus { get; set; }
        public DateTime CompanyAddedAt { get; set; }
        public DateTime CompanyUpdatedAt { get; set; }
    }
}
