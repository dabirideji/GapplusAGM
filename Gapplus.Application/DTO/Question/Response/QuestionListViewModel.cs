using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class QuestionListViewModel
    {
        public IEnumerable<Question> question { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public bool abstainBtnChoice { get; set; }
        public string forBg { get; set; }
        public string againstBg { get; set; }
        public string abstainBg { get; set; }
        public string voidBg { get; set; }
        public int agmid { get; set; }
    }
}