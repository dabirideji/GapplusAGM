using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models.ModelDTO
{
    public class GenericResponseDto<T> where T : class
    {
        public int Code { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string EmailRegistraionUrl { get; set; }
        public T Data { get; set; }
        public List<T> ListData {get; set;}
    }
}