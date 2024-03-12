using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class AccreditationResponse
    {
        public List<AGMCompanies> companies { get; set; }
        public Dictionary<string,string> ResourceTypes { get; set; }
    }
}