using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class ResultListViewModel
    {
        public IEnumerable<Result> Result { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public Question Question { get; set; }
        public bool abstainBtnChoice { get; set; }
    }
}