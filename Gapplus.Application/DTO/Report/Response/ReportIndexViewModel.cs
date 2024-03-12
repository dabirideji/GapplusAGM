using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class ReportIndexViewModel
    {
        public IEnumerable<Question> Questions { get; set; }
        public int presentcount { get; set; }
        public int shareholders { get; set; }
    }
}