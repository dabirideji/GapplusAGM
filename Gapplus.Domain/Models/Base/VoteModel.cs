using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class VoteModel
    {
        public int Id { get; set; }
        public string question { get; set; }
        public string response { get; set; }
        public string company { get; set; }
        public string identity { get; set; }
        public int shareholderNum { get; set; }
        public bool syncStatus { get; set; }
        public int agmid { get; set; }
        public string[] mod { get; set; }
    }
}