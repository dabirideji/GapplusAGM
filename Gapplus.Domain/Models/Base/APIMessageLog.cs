using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class APIMessageLog
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public DateTime EventTime { get; set; }
        public string EventUrl { get; set; }
        public string requestQuery { get; set; }
        public bool EventStatus { get; set; }
        public bool abstainBtnChoice { get; set; }
        public bool MessagingChoice { get; set; }
        public bool allChannel { get; set; }
        public bool webChannel { get; set; }
        public bool mobileChannel { get; set; }
        public bool UserLoginStatus { get; set; }
        public byte UserPollStatus { get; set; }
        public bool UserProxyStatus { get; set; }
        public bool Preregistered { get; set; }
        public string sessionVersion { get; set; }
        public string AGMTitle { get; set; }
        public string ResourceType { get; set; }
        public int AGMID { get; set; }
        public string consolidateAccountMessage { get; set; }
        public PresentModel shareholder { get; set; }
        public BarcodeModel PreregisterationShareholder { get; set; }
        public Facilitators facilitator { get; set; }
        public string forBg { get; set; }
        public string againstBg { get; set; }
        public string abstainBg { get; set; }
        public string voidBg { get; set; }
        public string Companylogo { get; set; }
        public List<Question> Rsolutions { get; set; }
        public List<Question> message { get; set; }
        public List<Result> VotingResult { get; set; }
        public List<AGMQuestion> Messages { get; set; }
    }
}