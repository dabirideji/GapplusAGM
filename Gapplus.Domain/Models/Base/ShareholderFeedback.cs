using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class ShareholderFeedback
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string phonenumber { get; set; }
        public string Message { get; set; }
        public DateTime When { get; set; }
    }
}