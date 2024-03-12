using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Util
{
    public static class Utilities
    {
        public static string GenerateAGMUrl(string company, int AGMID, string emailAddress)
        {
            string request = "";
            var requestUri = $"{Convert.ToString(ConfigurationManager.AppSettings["AGMBaseAddress"])}/Accreditation/AccreditationConfirmation/";
            if (!string.IsNullOrEmpty(requestUri) || !string.IsNullOrWhiteSpace(requestUri))
            {
                var query = string.Format("{0}|{1}", emailAddress, AGMID);
                var encryptedtext = query.Encrypt();
                request = $"{requestUri}?query={encryptedtext}";
                return request;

            }
            return request;
        }

     
    }
}