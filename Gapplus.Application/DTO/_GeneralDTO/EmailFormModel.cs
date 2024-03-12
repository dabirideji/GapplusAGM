using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class EmailFormModel
    {
        [Required]
        public string ShareholderNum { get; set; }
        [Required, Display(Name = "Your email"), EmailAddress]
        public string FromEmail { get; set; }
        public string PhoneNumber { get; set; }
    }
}