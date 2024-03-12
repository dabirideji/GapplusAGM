using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models.ModelDTO
{
    public class PreregistrationResultDto
    {
        public string Email { get; set; }
        public string Company { get; set; }
        public int ResolutionId { get; set; }
        public string ResultChoice { get; set; }
        //public List<ResolutionChoices> Choices { get; set; }
    }

    public class ResolutionChoices
    {
        public int ResolutionId { get; set; }
        public string ResultChoice { get; set; }
    }
}