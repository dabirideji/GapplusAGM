using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class singleSMS
    {
        public string from { get; set; }
        public string[] to { get; set; }
        public string text { get; set; }
    }
}