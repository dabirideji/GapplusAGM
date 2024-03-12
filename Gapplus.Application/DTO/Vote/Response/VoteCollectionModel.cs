using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class VoteCollectionModel
    {
        public int allPresent { get; set; }
        public int allVoted { get; set; }
        public double VotePercentage { get; set; }
    }
}