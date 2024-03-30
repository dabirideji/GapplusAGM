using System.ComponentModel;

namespace Gapplus.Web.DTO.ShareHolder
{
    public class LoginDto
    {
        
        public string Email { get; set; }
        public string Password { get; set; }
    }


    public class ShareHolderViewModel {
          public int Id { get; set; }
        public Int64 SN { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public double Holding { get; set; }
        public string Address { get; set; }
        [DefaultValue(0)]
        public double PercentageHolding { get; set; } = 0;
        [DefaultValue(0)]
        public Int64 ShareholderNum { get; set; }
        public int RegCode { get; set; }
        public byte[] BarcodeImage { get; set; }
        public string Barcode { get; set; }
        public string ImageUrl { get; set; }
        public string OnlineEventUrl { get; set; }
        public string Proxyupload { get; set; }
        [DefaultValue(false)]
        public bool Selected { get; set; } = false;
        [DefaultValue(false)]
        public bool Consolidated { get; set; } = false;
        public string ConsolidatedValue { get; set; }
        public string ConsolidatedParent { get; set; }
        [DefaultValue(false)]
        public bool Present { get; set; } = false;
        [DefaultValue(false)]
        public bool PresentByProxy { get; set; } = false;
        [DefaultValue(false)]
        public bool Preregistered { get; set; } = false;
       
        [DefaultValue(false)]
        public bool split { get; set; } = false;
        [DefaultValue(false)]
        public bool resolution { get; set; } = false;
        [DefaultValue(false)]
        public bool? combined { get; set; } = false;
        [DefaultValue(false)]
        public bool TakePoll { get; set; } = false;
        [DefaultValue(false)]
        public bool NotVerifiable { get; set; } = false;
        [DefaultValue(false)]
        public bool? AddedSplitAccount { get; set; } = false;
        public string emailAddress { get; set; }
        [DefaultValue(0)]
        public Int64 ParentAccountNumber { get; set; } = 0;
        public string? Clikapad { get; set; }
        public string? PhoneNumber { get; set; }
        public string? password { get; set; }
        public string passwordToken { get; set; }
        public string accesscode { get; set; }
        public string Date { get; set; }
        [DefaultValue(false)]
        public bool UserLoginHistory { get; set; } = false;
        public string Sessionid { get; set; }
        public string SessionVersion { get; set; }
    }
}
