using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class SplittedAccountViewModel
    {
        public int shareholderId { get; set; }       
        public string parentNumber { get; set; }
        public string commonNumber { get; set; }
        public string splitNumber { get; set; }
        public string splitHolding { get; set; }

    }
}