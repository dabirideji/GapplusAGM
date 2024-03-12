using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class BarcodeModelDto
    {
        public int Id { get; set; }
        public Int64 SN { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Holding { get; set; }
        public string Address { get; set; }
        public string PercentageHolding { get; set; }
        public Int64 ShareholderNum { get; set; }
        public int RegCode { get; set; }
        public string emailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string accesscode { get; set; }
        public string Token { get; set; }
        public string OnlineEventUrl { get; set; }
    }
}