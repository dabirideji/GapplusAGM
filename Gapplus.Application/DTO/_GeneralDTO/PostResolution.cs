using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class PostResolution
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Int64 shareholdernum { get; set; }
        public Int64 NewNumber { get; set; }
        public string? NewHolding { get; set; }
        public Int64 splitvalue { get; set; }
        public Int64 ParentNumber { get; set; }
        public string? RG { get; set; }
        public int agmid { get; set; }
    }
}