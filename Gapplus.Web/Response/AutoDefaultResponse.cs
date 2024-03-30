using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarcodeGenerator.Models.ModelDTO;

namespace Gapplus.Application.Response
{
    public class AutoDefaultResponse<T>
        where T : class
    {
        public DefaultResponse<T> ConvertToGood(String message, T? data=null)
        {
            var response = new DefaultResponse<T>()
            {
                Status = true,
                ResponseCode = "00",
                ResponseMessage = message,
            };
            response.Data = data;
            return response;
        }

        public DefaultResponse<T> ConvertToBad(String message)
        {
            return new DefaultResponse<T>
            {
                Status = false,
                ResponseCode = "99",
                ResponseMessage = message,
            };
        }
    }
    public class AutoApiResponse<T>
        where T : class
    {
        public GenericAPIResponseDTO<T> ConvertToGood(String message, T? data=null)
        {
            var response = new GenericAPIResponseDTO<T>()
            {
                status = true,
                responseCode = "00",
                responseMessage = message,
            };
            response.responseData = data;
            return response;
        }

        public GenericAPIResponseDTO<T> ConvertToBad(String message)
        {
            return new GenericAPIResponseDTO<T>
            {
                status = false,
                responseCode = "99",
                responseMessage = message,
            };
        }
    }
}
