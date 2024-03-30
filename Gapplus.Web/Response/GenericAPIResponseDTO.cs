using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models.ModelDTO
{
    public class GenericAPIResponseDTO <T> where T : class
    {
        public bool status { get; set; }
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public T responseData { get; set; }
        
    }

    public class GenericAPIListResponseDTO<T> where T : class
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public List<T> responseData { get; set; }

    }
}