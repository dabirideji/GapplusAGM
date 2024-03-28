using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gapplus.Application.Helpers;

namespace Gapplus.Application.Helpers
{
    public static class DatabaseManager
    {
        private static readonly IConfiguration _configuration;

        static DatabaseManager()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
        public static string GetConnectionString(string connectionInstance = "DefaultConnection")
        {
            var c = _configuration.GetConnectionString(connectionInstance);
            return c;
        }




        // Method to retrieve a specific configuration value
        public static string GetAppSetting(string key)
        {
            return _configuration[key];
        }

        // Method to retrieve a specific configuration value with type conversion
        public static T GetAppSetting<T>(string key)
        {
            string value = _configuration[key];
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }

}