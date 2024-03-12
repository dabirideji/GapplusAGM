using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class BarcodeViewModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public double Holding{ get; set; }
        public string Address{ get; set; }
        public bool Details { get; set; }
        public double PercentageHolding { get; set; }
        public byte[] BarcodeImage { get; set; }
        public Int64 ShareholderNum { get; set; }
        public string Barcode { get; set; }
        public string ImageUrl { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Clikapad { get; set; }

        public string Empty { get; set; }
        public string Void { get; set; }
        public string Message { get; set; }
        public string logo { get; set; }
        public string Company { get; set; }
        public string AGMTitle { get; set; }
        public UserProfile User { get; set; }
        public int agmid { get; set; }

    }
}