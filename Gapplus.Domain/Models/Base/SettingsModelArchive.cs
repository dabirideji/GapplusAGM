using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class SettingsModelArchive
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Venue { get; set; }
        public string Address { get; set; }
        public bool AgmStart { get; set; }
        public bool AgmEnd { get; set; }
        public bool StopAdmittance { get; set; }
        public bool StartAdmittance { get; set; }
        public DateTime? AdmittanceDateTime { get; set; }
        public bool proxyChannel { get; set; }
        public bool smsChannel { get; set; }
        public bool webChannel { get; set; }
        public bool mobileChannel { get; set; }
        public bool ussdChannel { get; set; }
        public bool allChannels { get; set; }
        public DateTime? AgmDateTime { get; set; }
        public DateTime? AgmEndDateTime { get; set; }
        public string OnlineUrllink { get; set; }
        public string Description { get; set; }
        public string SyncChoice { get; set; }
        public bool? AbstainBtnChoice { get; set; }
        public bool PreregisteredVotes { get; set; }
        public int AGMID { get; set; }
        public int RegCode { get; set; }
        public int TotalRecordCount { get; set; }
        public double ShareHolding { get; set; }
        public string CompanyName { get; set; }
        public bool ArchiveStatus { get; set; }
        public string Year { get; set; }
        public DateTime DateCreated { get; set; }
        public string PrintOutTitle { get; set; }
        public string Location { get; set; }
        public string When { get; set; }
        public string feebackEmailAddress { get; set; }
        public string feebackCCEmailAddress { get; set; }
        public string ImageSource { get; set; }
        public byte[] Image { get; set; }
        public string VoteForColorBg { get; set; }
        public string VoteAgainstColorBg { get; set; }
        public string VoteAbstaincolorBg { get; set; }
        public string VoteVoidColorBg { get; set; }
    }
}