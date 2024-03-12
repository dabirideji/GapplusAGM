using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class ResolutionListModel
    {
        public List<Result> ResultFor { get; set; }
        public List<Result> ResultAgainst { get; set; }
        public List<Result> ResultAbstain { get; set; }
        public List<Question> questionList { get; set; }
        public Int64 TotalCount { get; set; }
        public Question question { get; set; }
        public bool abstainBtnChoice { get; set; }
        public string forBg { get; set; }
        public string againstBg { get; set; }
        public string abstainBg { get; set; }
        public string voidBg { get; set; }
    }
}