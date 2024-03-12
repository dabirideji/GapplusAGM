using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class UploadDatabase
    {
        public int Id { get; set; }
        public string SN { get; set; }
        public string Name { get; set; }
        public string Holding { get; set; }
        public string Address { get; set; }
        public string PercentageHolding { get; set; }
        public Int64 ShareholderNum { get; set; }
        public string emailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}