using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class SettingsViewModel
    {
        public ICollection<MailSettings> mailsetting { get; set; }
        public ICollection<SettingsModel> settingsModel { get; set; }
        public ICollection<Proxylist> Proxylistmodel{ get; set; }
        public UserProfile user { get; set; }
        public List<string> companiesUploaded { get; set; }
        public List<string> proxylistUploaded { get; set; }
        public int agmid { get; set; }
        public int RegCode { get; set; }
        public bool ProxyVoteResult { get; set; }
    }
}