using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class PresentViewModel
    {
        public string Company { get; set; }
        public string ImageUrl { get; set; }
        public string AGMTitle { get; set; }
        public UserProfile User { get; set; }
        public int agmid { get; set; }
    }
}