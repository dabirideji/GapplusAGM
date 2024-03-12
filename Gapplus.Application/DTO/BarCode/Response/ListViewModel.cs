using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class ListViewModel
    {
        public IEnumerable<BarcodeModel> barcodes { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}