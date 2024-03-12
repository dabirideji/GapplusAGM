using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class PostValue
    {
        public int Id { get; set; }
        public string value { get; set; }
        public string data { get; set; }
        public bool status { get; set; }
        public string cchannel { get; set; }
        public int agmid { get; set; }
    }
}