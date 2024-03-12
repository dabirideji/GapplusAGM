using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class BarcodeListViewModel
    {
        public IEnumerable<BarcodeModel> barcodes { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}