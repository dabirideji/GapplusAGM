using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gapplus.Application.Response
{
    public class DefaultResponse<T> where T:class
    {
        public bool Status { get; set; }
        public String? ResponseCode { get; set; }
        public String? ResponseMessage { get; set; }
        public T? Data { get; set; }
    }
}