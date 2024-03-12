using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class SMSResult
    {
        public int id { get; set; }
        public string messageId { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string text { get; set; }
        public string cleanText { get; set; }
        public string keyword { get; set; }
        public string receivedAt { get; set; }
        public string smsCount { get; set; }

    }
}