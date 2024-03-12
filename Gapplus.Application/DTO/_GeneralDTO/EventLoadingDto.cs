using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class EventLoadingDto
    {
        public TimeSpan timespan { get; set; }
        public string company { get; set; }
        public string AGMDescription { get; set; }
        public DateTime MeetingDate { get; set; }
        public string MeetingVenue { get; set; }
    }
}