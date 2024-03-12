using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class SearchModel
    {
        public string search { get; set; }
        public int[] Id { get; set; }
        public string emailsearch { get; set; }
        public string company { get; set; }

    }


    public class Consolidate
    {
        public string Id { get; set; }
    }
}