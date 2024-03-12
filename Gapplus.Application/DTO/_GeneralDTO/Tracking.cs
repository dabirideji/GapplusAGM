using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class Tracking
    {
        public string track { get; set; }
        public string type { get; set; }
    }

    public class Language
    {
        public string languageCode { get; set; }
    }

    public class time
    {
        public string hour { get; set; }
        public string minute { get; set; }
        public string[] days { get; set; }
    }
}