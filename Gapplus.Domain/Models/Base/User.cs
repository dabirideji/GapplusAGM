using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gapplus.Domain.Categories;

namespace Gapplus.Domain
{
    public class User
    {


        public Guid UserId { get; set; }
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public String? FullName { get; set; }
        public String? UserPassword { get; set; }
        public String? EmailId { get; set; }
        public string CompanyInfo { get; set; }



        //NAVIGATTON PROPERTY FOR THE USER COMPANY
        // [ForeignKey("Company")]
        // public Guid CompanyId { get; set; }

        // public Company? Company { get; set; }
    }
}