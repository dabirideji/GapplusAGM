using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class PresentModel
    {
        public int Id { get; set; }
        public int AGMID { get; set; }
        public string Name { get; set; }
        public double Holding { get; set; }
        public string Company { get; set; }
        public byte PermitPoll { get; set; }
        public string Address { get; set; }
        public string admitSource { get; set; }
        public double PercentageHolding { get; set; }
        public Int64 ShareholderNum { get; set; }
        public Int64 newNumber { get; set; }
        public Int64 ParentNumber { get; set; }
        [DefaultValue(false)]
        public bool TakePoll { get; set; }
        [DefaultValue(false)]
        public bool split { get; set; }
        [DefaultValue(false)]
        public bool present { get; set; }
        [DefaultValue(false)]
        public bool proxy { get; set; }
        [DefaultValue(false)]
        public bool preregistered { get; set; }
        public string emailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Clikapad { get; set; }
        [DefaultValue(false)]
        public bool GivenClikapad { get; set; }
        [DefaultValue(false)]
        public bool ReturnedClikapad { get; set; }
        public DateTime? PresentTime { get; set; }
        public string Year { get; set; }
        public TimeSpan? Timestamp { get; set; }=TimeSpan.MinValue;
    }
}