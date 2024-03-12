using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int AGMID { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Company { get; set; }
        public string Year { get; set; }
        public double Holding { get; set; }
        public string Address { get; set; }
        public Int64 splitValue { get; set; }
        public Int64 ParentNumber { get; set; }
        public double PercentageHolding { get; set; }
        //public Int64 Identity { get; set; }
        public Int64 ShareholderNum { get; set; }
        public string phonenumber { get; set; }
        public string Clickapad { get; set; }
        public string VoteChoice { get; set; }
        //public string For{ get; set; }
        //public string Against{ get; set; }
        //public string Abstain { get; set; }
        public bool? VoteFor { get; set; }
        public bool? VoteAgainst { get; set; }
        public bool? VoteAbstain { get; set; }
        public bool? VoteVoid { get; set; }
        public string VoteStatus { get; set; }
        public string Source { get; set; }
        public DateTime date { get; set; }
        public TimeSpan Timestamp { get; set; }
        public Boolean Present { get; set; }
        public Boolean PresentByProxy { get; set; }
        public Boolean Pregistered { get; set; }
        public int QuestionId { get; set; }
        
    }
}