using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class Destination
    {
        public int id { get; set; }
        public string to { get; set; }
        public int messageId { get; set; }
    }
}