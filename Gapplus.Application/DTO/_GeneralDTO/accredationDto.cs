using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class accredationDto
    { 
        public string company { get; set; }
        public string accesscode { get; set; }
        public string ResourceType { get; set; }
        public int agmid { get; set; }
        public int shareholderNum { get; set; }
    }
}