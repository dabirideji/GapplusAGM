using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class SMSDeliveryLog
    {
        public int id { get; set; }
        public string to { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string smsCount { get; set; }
        public string deliveryId { get; set; }
        public DateTime date { get; set; }
    }
}