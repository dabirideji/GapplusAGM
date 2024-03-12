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
}