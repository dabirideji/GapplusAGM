using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class VotingResponseMessage
    {
        public bool Status { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ActiveResolution { get; set; }
        public int ActiveResolutinId { get; set; }
        public string Company { get; set; }
    }


    public class JoinVotingDTO
    {
        public string Company { get; set; }
        public string ShareholderNum { get; set; }
    }

    public class VotingStatusDTO
    {
        public string Company { get; set; }
        public string Email { get; set; }
    }

    public class JoinVotingResponse
    {
        public bool VotingStatus { get; set; }
        public bool VotingEnded { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public List<Question> Resolution { get; set; }
    }
}