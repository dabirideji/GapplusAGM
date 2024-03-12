using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class VoteCount
    {
        public int ForCount { get; set; }
        public int AgainstCount { get; set; }
        public int AbstainCount { get; set; }
    }
}