using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class SplittedAccount
    {
        public int Id { get; set; }
        public string ParentNumber { get; set; }
        public string CommonNumber { get; set; }
        public string Holding { get; set; }
        public string ShareholderNum { get; set; }
        public int AccountId { get; set; }

    }
}