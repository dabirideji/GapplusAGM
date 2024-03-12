using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Gapplus.Application.Helpers
{
    public class ConfigurationHelper
    {

        public static String GetConnectionString(string DbConnection="DefaultConnection"){
            IConfiguration configuration=new Microsoft.Extensions.Configuration.ConfigurationManager();
            return configuration.GetConnectionString(DbConnection);
        }
    }










public static class WebRootHelper
{
    private static string _webRootPath;

    public static void Initialize(Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
    {
        _webRootPath = hostingEnvironment.WebRootPath;
    }

    public static string GetPath(string relativePath)
    {
        if (_webRootPath == null)
        {
            throw new InvalidOperationException("Web root path is not initialized.");
        }

        return Path.Combine(_webRootPath, relativePath);
    }
}







}