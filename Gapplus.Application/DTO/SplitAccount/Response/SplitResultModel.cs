using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class SplitResultModel
    {
        public IEnumerable<Result> Result { get; set; }
        public IEnumerable<Question> question { get; set; }
        public bool abstainBtnChoice { get; set; }
    }
}