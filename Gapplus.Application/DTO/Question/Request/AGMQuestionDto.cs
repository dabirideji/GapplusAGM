namespace BarcodeGenerator.Models
{
    public class AGMQuestionDto
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string ShareholderName { get; set; }
        public string shareholderquestion { get; set; }
        public Int64 ShareholderNumber { get; set; }
        public double holding { get; set; }
        public double PercentageHolding { get; set; }
        public string emailAddress { get; set; }
        public string phoneNumber { get; set; }
        public int AGMID { get; set; }
        public string MessageType { get; set; }
        public DateTime datetime { get; set; }
        public string datetimeString { get; set; }
        public string Firstletter { get; set; }
        public string ReplyToName { get; set; }
        public string ReplyToEmail { get; set; }
        public string ReplyToMessage { get; set; }
    }
}
