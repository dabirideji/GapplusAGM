using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class Proxylist
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public Int64 ShareholderNum { get; set; }
        [DefaultValue(false)]
        public bool Validity { get; set; } = false;
    }
}