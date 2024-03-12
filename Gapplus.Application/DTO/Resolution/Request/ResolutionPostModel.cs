using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class ResolutionPostModel
    {
        public string name { get; set; }
        public Int64 number { get; set; }
        public Int64 newnumber { get; set; }
        public string Holding { get; set; }
        public Int64 splitvalue { get; set; }
        public Int64 ParentNumber { get; set; }
    }
}