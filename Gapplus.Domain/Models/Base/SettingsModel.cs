using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class SettingsModel
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
        public bool StopVoting { get; set; }
        public bool StartVoting { get; set; }
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
        public bool MessagingChoice { get; set; }
        public bool PreregisteredVotes { get; set; }

        public bool ProxyVoteResult { get; set; }
        public int AGMID { get; set; }
        public int RegCode { get; set; }
        public int CountDownValue { get; set; }
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
















    public static class SettingsModelFakeData
    {
        public static List<SettingsModel> GenerateFakeSettingsData()
        {
            var fakeSettingsData = new List<SettingsModel>();

            // Generate fake data for multiple settings
            for (int i = 0; i < 10; i++)
            {
                var settings = new SettingsModel
                {
                    Id = i + 1,
                    Title = $"Title {i + 1}",
                    Venue = $"Venue {i + 1}",
                    Address = $"Address {i + 1}",
                    AgmStart = GetRandomBool(),
                    AgmEnd = GetRandomBool(),
                    StopAdmittance = GetRandomBool(),
                    StartAdmittance = GetRandomBool(),
                    StopVoting = GetRandomBool(),
                    StartVoting = GetRandomBool(),
                    AdmittanceDateTime = GetRandomDateTime(),
                    proxyChannel = GetRandomBool(), // Change to proxyChannel
                    smsChannel = GetRandomBool(), // Change to smsChannel
                    webChannel = GetRandomBool(), // Change to webChannel
                    mobileChannel = GetRandomBool(), // Change to mobileChannel
                    ussdChannel = GetRandomBool(), // Change to ussdChannel
                    allChannels = GetRandomBool(), // Change to allChannels
                    AgmDateTime = GetRandomDateTime(),
                    AgmEndDateTime = GetRandomDateTime(),
                    OnlineUrllink = $"https://example.com/{i}",
                    Description = $"Description {i + 1}",
                    SyncChoice = $"Sync Choice {i + 1}",
                    AbstainBtnChoice = GetRandomBool(),
                    MessagingChoice = GetRandomBool(),
                    PreregisteredVotes = GetRandomBool(),
                    ProxyVoteResult = GetRandomBool(),
                    AGMID = i + 1000,
                    RegCode = i + 2000,
                    CountDownValue = i + 10,
                    TotalRecordCount = i + 50,
                    ShareHolding = i * 0.5,
                    CompanyName = $"Company {i + 1}",
                    ArchiveStatus = GetRandomBool(),
                    Year = $"Year {i + 2020}",
                    DateCreated = DateTime.Now.AddDays(-i),
                    PrintOutTitle = $"PrintOut Title {i + 1}",
                    Location = $"Location {i + 1}",
                    When = $"When {i + 1}",
                    feebackEmailAddress = $"feedback{i + 1}@example.com", // Change to feebackEmailAddress
                    feebackCCEmailAddress = $"ccfeedback{i + 1}@example.com", // Change to feebackCCEmailAddress
                    ImageSource = $"Image Source {i + 1}",
                    Image = null, // Assign your image bytes here if needed
                    VoteForColorBg = $"Vote For Color {i + 1}",
                    VoteAgainstColorBg = $"Vote Against Color {i + 1}",
                    VoteAbstaincolorBg = $"Vote Abstain Color {i + 1}",
                    VoteVoidColorBg = $"Vote Void Color {i + 1}"
                };


                fakeSettingsData.Add(settings);
            }

            return fakeSettingsData;
        }

        // Helper method to generate random boolean values
        private static bool GetRandomBool()
        {
            return new Random().Next(2) == 0;
        }

        // Helper method to generate random DateTime values
        private static DateTime? GetRandomDateTime()
        {
            return DateTime.Now.AddDays(new Random().Next(30));
        }
    }

}