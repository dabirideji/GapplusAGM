using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class KeypadResults
    {
        public int id { get; set; }
        public int AGMID { get; set; }
        public string voteReceived { get; set; }
        public string Company { get; set; }
        public string TimeReceived { get; set; }
        public string Keypad { get; set; }
        public string Keyvalue { get; set; }
        [DefaultValue(false)]
        public bool Valuechecked {get;set;}


    }
}