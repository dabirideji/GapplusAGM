using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BarcodeGenerator.Models
{
    public class ResolutionModel
    {
        
        public string Name { get; set; }
        public int shareholder_Id { get; set; }
        public Int64 shareholdernum { get; set; }
        public Int64 NewNumber { get; set; }
        public string NewHolding { get; set; }
        public Int64 splitvalue { get; set; }
        public Int64 ParentNumber { get; set; }
        public bool resolutionstatus { get; set; }      
        public string RG1 { get; set; }
        public string RG2 { get; set; }
        public string RG3 { get; set; }
        public string RG4 { get; set; }
        public string RG5 { get; set; }
        public string RG6 { get; set; }
        public string RG7 { get; set; }
        public string RG8 { get; set; }
        public string RG9 { get; set; }
        public bool abstainBtnChoice { get; set; }
        public ICollection<Question> Question { get; set; }
       
    }

    public class FakeResolutionModel{
        [Key]
        public int Id   {get;set;}
        public String Question {get;set;}
        public int CoundownTime {get;set;}
        public bool IsDisplayed {get;set;}=false;
        public DateTime CreatedAt {get;set;}=DateTime.Now;
    }
}