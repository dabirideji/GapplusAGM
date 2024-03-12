using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class SplitDeletePostModel
    {
        public string name { get; set; }
        public Int64 splitvalue { get; set; }
        public Int64 shareholdernum { get; set; }
        public Boolean status { get; set; }
    }
}