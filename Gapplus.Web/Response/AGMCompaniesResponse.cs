using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class AGMCompaniesResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public List<AGMCompanies> Companies { get; set; }
    }

    public class AGMCompanies
    {
        public string company { get; set; }
        public string description { get; set; }
        public int agmid { get; set; }
        public int RegCode { get; set; }
        public string venue { get; set; }
        public DateTime? dateTime { get; set; }
        public DateTime? EnddateTime { get; set; }
    }
}