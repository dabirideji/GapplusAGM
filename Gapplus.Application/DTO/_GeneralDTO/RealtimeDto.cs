using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models{
    public class RealtimeDto
    {
        public int ResolutionId { get; set; }
        public string Resolution { get; set; }
        public int ResultForCount { get; set; }
        public int ResultAgainstCount { get; set; }
        public int ResultAbstainCount { get; set; }
        public int ResultVoidCount { get; set; }
        public double PercentageFor { get; set; }
        public string PercentageForValue { get; set; }
        public double PercentageAgainst { get; set; }
        public string PercentageAgainstValue { get; set; }
        public double PercentageAbstain { get; set; }
        public string PercentageAbstainValue { get; set; }
        public double PercentageVoid { get; set; }
        public string TotalHoldingFor { get; set; }
        public string TotalHoldingAgainst { get; set; }
        public string TotalHoldingAbstain { get; set; }
        public string TotalHoldingVoid { get; set; }
        public bool proxyChannel {get; set;}
        public bool smsChannel {get; set;}
        public bool webChannel {get; set;}
        public bool mobileChannel {get; set;}
        public bool ussdChannel {get; set;}
        public bool allChannels {get; set;}
        public bool abstainBtnChoice { get; set; }    
        public int agmid { get; set; }
        public string syncChoiceVoid { get; set; }
        public string forBg { get; set; }
        public string againstBg { get; set; }
        public string abstainBg { get; set; }
        public string voidBg { get; set; }
        public string voteType { get; set; }
        public int  TotalCount { get; set; }
        public double TotalPercentage { get; set; }
        public string TotalPercentageValue { get; set; }
        public string TotalHolding{ get; set; }
        public string Companylogo { get; set; }
        public string CompanyName { get; set; }
        public List<Question> questions { get; set; }
        public int ResolutinIndex { get; set; }
    }
}