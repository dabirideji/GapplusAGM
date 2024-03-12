using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class VoteCollation
    {
        public string For { get; set; }
        public string Against  { get; set; }
        public string Abstain { get; set; }
        public string VoteVoid { get; set; }
        public int QuestionId { get; set; }
        public bool abstainBtnChoice { get; set; }
    }
}