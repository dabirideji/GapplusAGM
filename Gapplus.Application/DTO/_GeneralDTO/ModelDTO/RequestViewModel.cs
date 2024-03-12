using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models.ModelDTO
{
    public class RequestViewModel
    {
        public string company { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }
}