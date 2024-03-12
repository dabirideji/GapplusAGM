using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Gapplus.Domain.Categories;

namespace Gapplus.Domain
{
    public class Company
    {
        [Key]
        public Guid CompanyId { get; set; }
        public String? CompanyName{ get; set; }
        public String? CompanyAddress { get; set; }
        public CompanyStatus CompanyStatus { get; set; }=CompanyStatus.Active;
        public DateTime CompanyAddedAt { get; set; }=DateTime.Now;
        public DateTime CompanyUpdatedAt { get; set; }=DateTime.MinValue;
    }
}