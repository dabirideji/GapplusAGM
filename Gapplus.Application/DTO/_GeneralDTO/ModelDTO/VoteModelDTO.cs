using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models.ModelDTO
{

    public class ResolutionStatusDTO
    {

        public int resolutionid { get; set; }
        public string company { get; set; }


    }
    public class VoteModelDTO
    {

        public int resolutionid { get; set; }
        public string response { get; set; }
        public string company { get; set; }
        public string emailAddress { get; set; }
        public bool syncStatus { get; set; }

    }
}