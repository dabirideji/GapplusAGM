using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class AppLog
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string idenity { get; set; }
        public string Status { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime EventTime { get; set; }
        public string AGMTitle { get; set; }
        public int AGMID { get; set; }
    }
}