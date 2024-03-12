using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class ResolutionResult
    {
        public List<Result> ResultFor { get; set; }
        public List<Result> ResultAgainst { get; set; }
        public List<Result> ResultAbstain { get; set; }

    }
}