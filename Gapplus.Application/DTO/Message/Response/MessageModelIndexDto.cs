namespace BarcodeGenerator.Models
{
    public class MessageModelIndexDto
    {
        public UserProfile User { get; set; }
        public SettingsModel Settings { get; set; }
        public string AGMTitle { get; set; }
        public int AGMID { get; set; }
        public string CompanyInfo { get; set; }
        public List<AGMQuestion> Messages { get; set; }
    }
}
