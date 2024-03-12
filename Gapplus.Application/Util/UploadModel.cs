using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class UploadModel
    {
        public string companyinfo { get; set; }
        // public HttpPostedFileBase file { get; set; }
        public object file { get; set; }
    }
}