using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class SplitModel
    {
        public int Id { get; set; }
        public int shareholder_Id { get; set; }
        public string Name { get; set; }
        public Int64 ShareholderNumber { get; set; }
        public string NewNumber { get; set; }
        public string Holding { get; set; }
        public string NewHolding { get; set; }
        public bool splitStatus { get; set; }
        public Int64 splitvalue { get; set; }
        public Int64 ParentNumber { get; set; }
        public string  Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }
        public string Q5 { get; set; }
        public string Q6 { get; set; }
        public string Q7 { get; set; }
        public string Q8 { get; set; }
        public string Q9 { get; set; }    
        public DateTime date { get; set; }
      
    }
}