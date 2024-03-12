using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class ClikapadReceiveDto
    {
        public string pVotes { get; set; }
        public string ptime { get; set; }
        public string pKeypad { get; set; }
        public string pKeyvalue { get; set; }
        public string pcompany { get; set; }
        public string pagmid { get; set; }
      
    }


    public class ClikapadValidateDto
    {
        public string Clikapad { get; set; }


    }
}