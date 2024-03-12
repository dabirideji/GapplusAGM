using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class sendSMS
    {
        public string bulkId { get; set; }
        public ICollection<Messages> messages { get; set; }
        public Tracking tracking { get; set; }
    }
}