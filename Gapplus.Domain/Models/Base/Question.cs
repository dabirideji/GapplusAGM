using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int AGMID { get; set; }
        public string question { get; set; }
        public string Year { get; set; }
        public string Company { get; set; }
        public string voteType { get; set; }
        public DateTime date { get; set; }
        public Boolean questionStatus { get; set; }
        public Boolean syncStatus { get; set; }
        public virtual List<Result> result { get; set; }
        public virtual ICollection<SMSDeliveryLog> SMSdeliveryLog { get; set; }
        public virtual ICollection<SMSResult> SMSResult { get; set; }

    }
}