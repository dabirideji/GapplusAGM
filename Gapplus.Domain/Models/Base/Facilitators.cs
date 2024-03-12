using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class Facilitators
    {
        public int Id { get; set; }
        public string SN { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }       
        public string FacilitatorCompany { get; set; }
        public int AGMID { get; set; }
        public string ResourceType { get; set; }
        public byte[] BarcodeImage { get; set; }
        public string Barcode { get; set; }
        public string ImageUrl { get; set; }
        public string OnlineEventUrl { get; set; }
        public string emailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string accesscode { get; set; }
        public string Date { get; set; }
        public bool UserLoginHistory { get; set; } = false;
        public string Sessionid { get; set; }
        public string SessionVersion { get; set; }
    }
}