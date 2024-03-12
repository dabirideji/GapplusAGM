using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class Messages
    {
        public virtual int id { get; set; }
        public virtual string from { get; set; }
        public virtual string text { get; set; }
        public virtual string sendAt { get; set; }
        public virtual Boolean flash { get; set; }
        public virtual string transliteration { get; set; }
        public virtual Boolean intermediateReport { get; set; }
        public virtual string notifyUrl { get; set; }
        public virtual string notifyContentType { get; set; }
        public virtual string callbackData { get; set; }
        public virtual string validityPeriod { get; set; }
        public virtual ICollection<Destination> destinations { get; set; }
       
        //public Dictionary<string, time> deliveryTimeWindow {get;set;}
        //public Language language { get; set; }
    }
}