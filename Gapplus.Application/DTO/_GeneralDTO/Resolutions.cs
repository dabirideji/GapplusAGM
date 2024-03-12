using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class Resolutions
    {
        public int Id { get; set; }
        public string question { get; set; }
        public DateTime date { get; set; }
        public Boolean questionStatus { get; set; }
    }
}
