using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class ProxyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Holding { get; set; }
        public string Address { get; set; }
        public string PercentageHolding { get; set; }
        public string ShareholderNum { get; set; }
        public string newNumber { get; set; }
        public Boolean TakePoll { get; set; }
        public Boolean split { get; set; }
        public Boolean present { get; set; }
        public Boolean proxy { get; set; }
        public string emailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? PresentTime { get; set; }
        public TimeSpan Timestamp { get; set; }
    }
}