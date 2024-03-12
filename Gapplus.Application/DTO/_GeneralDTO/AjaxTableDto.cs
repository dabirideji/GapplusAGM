using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class AjaxTableDto
    {
        public string draw { get; set; }
        public string start { get; set; }
        public string length { get; set; }
        public string sortColumn { get; set; }
        public string sortColumnDir { get; set; }
        public string searchValue { get; set; }
        public IEnumerable<BarcodeModel> displayedList { get; set; }
        public IEnumerable<PresentModel> displayedPresentList { get; set; }
        public IEnumerable<Facilitators> displayedNonShareholderList { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }
}
