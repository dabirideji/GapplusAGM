using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gapplus.Web.Models
{
  
public class AGMCompanies
    {
        public string company { get; set; }
        public string description { get; set; }
        public int agmid { get; set; }
        public int RegCode { get; set; }
        public string venue { get; set; }
        public DateTime? dateTime { get; set; }
        public DateTime? EnddateTime { get; set; }
    }




public class ShareholderDashboardViewModel{


    public List<AGMCompanies> companies { get; set; }
}
 public class AccreditationResponse
    {
        public List<AGMCompanies> companies { get; set; }
        public Dictionary<string,string> ResourceTypes { get; set; }
    }

}