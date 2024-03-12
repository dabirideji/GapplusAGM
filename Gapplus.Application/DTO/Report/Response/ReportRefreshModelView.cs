using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class ReportRefreshModelView
    {
        public int presentcount { get; set; }
        public int feedbackcount { get; set; }
        public int shareholders { get; set; }
        public int present { get; set; }
        public string TotalPercentageHolding { get; set; }
        public double Holding { get; set; }
        public int proxy { get; set; }
        public string TotalPercentageProxyHolding { get; set; }
        public double ProxyHolding { get; set; }
        public int TotalCount { get; set; }
        public double TotalHolding { get; set; }
        public string PercentageTotalHolding { get; set; }
        public string percentagePresent { get; set; }
        public string percentageProxy { get; set; }
        public string ResolutionName { get; set; }
        public int ResultForCount { get; set; }
        public int ResultAgainstCount { get; set; }
        public int ResultAbstainCount { get; set; }
        public double PercentageFor { get; set; }
        public double PercentageAgainst { get; set; }
        public double PercentageAbstain { get; set; }
        public List<Question> resolutions { get; set; }
        public string logo { get; set; }
        public string Company { get; set; }
        public string AGMTitle { get; set; }
        public bool abstainBtnChoice { get; set; }
        public string syncChoiceVoid { get; set; }
        public string forBg { get; set; }
        public string againstBg { get; set; }
        public string abstainBg { get; set; }
        public string voidBg { get; set; }
        public int agmid { get; set; }


    }
}