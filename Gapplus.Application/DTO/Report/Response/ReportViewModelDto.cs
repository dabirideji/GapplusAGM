using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class ReportViewModelDto
    {
        public string logo { get; set; }
        public string Company { get; set; }
        public string Year { get; set; }
        public string AGMTitle { get; set; }
        public string AGMDateTime { get; set; }
        public string AGMAddress { get; set; }
        public string PrintTitle { get; set; }
        public string Decision { get; set; }
        public int presentcount { get; set; }
        public int shareholders { get; set; }
        public int present { get; set; }
        public int AGMID { get; set; }
        public string TotalPercentageHoldingPresent { get; set; }
        public string Question { get; set; }
        public double Holding { get; set; }
        public int proxy { get; set; }
        public List<PresentModel> Voted { get; set; }
        public List<PresentArchive> VotedArchive { get; set; }
        public int Votes { get; set; }
        public int proxycount { get; set; }
        public string TotalPercentageHoldingProxy { get; set; }
        public double HoldingProxy { get; set; }
        public double ProxyHolding { get; set; }
        public double HoldingPresent { get; set; }
        public int TotalCount { get; set; }
        public string TotalHolding { get; set; }
        public string TotalPercentageHolding { get; set; }
        public string PercentageTotalHolding { get; set; }
        public string TotalPercentageProxyHolding { get; set; }
        public string PercentageResultAbstain { get; set; }
        public string PercentageResultAgainst { get; set; }
        public string PercentageResultFor { get; set; }
        public string PercentageResultAll { get; set; }
        public string PercentageResultVoid { get; set; }
        public string TotalPercentHolding { get; set; }
        public string ResultForPercentHoldingAbstain { get; set; }
        public string ResultForPercentHoldingAll{ get; set; }
        public string ResultForPercentHoldingVoid { get; set; }
        public string ResultDecisionPercentHolding { get; set; }
        public string ResultForHoldingAbstain { get; set; }
        public string ResultForHoldingAgainst { get; set; }
        public string ResultForPercentHoldingAgainst { get; set; }
        public string ResultDecisionHolding { get; set; }
        public string ResultForHolding { get; set; }
        public string ResultForHoldingAll { get; set; }
        public string ResultForHoldingVoid { get; set; }
        public string ResultForPercentHolding { get; set; }
        public int Id { get; set; }
        public string resolutionName { get; set; }
        public string percentagePresent { get; set; }
        public string percentageProxy { get; set; }
        public string ResolutionName { get; set; }
        public int ResultDecisionCount { get; set; }
        public int ResultForCount { get; set; }
        public int ResultAllCount { get; set; }
        public int ResultVoidCount { get; set; }
        public int ResultAgainstCount { get; set; }
        public int ResultAbstainCount { get; set; }
        public double PercentageAll { get; set; }
        public double PercentageVoid { get; set; }
        public double PercentageFor { get; set; }
        public double PercentageAgainst { get; set; }
        public double PercentageAbstain { get; set; }
        public string AGMVenue { get; set; }
        public string AGMTime { get; set; }
        public string SyncChoice { get; set; }
        public bool abstainBtnChoice { get; set; }
        public List<Result> ResultAll { get; set; }
        public List<Result> ResultVoid { get; set; }
        public List<Result> ResultFor { get; set; }
        public List<Result> ResultDecision{ get; set; }
        public List<Result> ResultAgainst { get; set; }
        public List<Result> ResultAbstain { get; set; }
        public List<ResultArchive> ResultArchiveAll { get; set; }
        public List<ResultArchive> ResultArchiveVoid { get; set; }
        public List<ResultArchive> ResultArchiveFor { get; set; }
        public List<ResultArchive> ResultArchiveAgainst { get; set; }
        public List<ResultArchive> ResultArchiveAbstain { get; set; }
        public List<Result> Result { get; set; }
        public List<ResultArchive> ResultArchive { get; set; }
        public UserProfile User { get; set; }
        public IEnumerable<Question> resolutions { get; set; }
        public IEnumerable<QuestionArchive> resolutionsArchive { get; set; }
        public string forBg { get; set; }
        public string againstBg { get; set; }
        public string abstainBg { get; set; }
        public string voidBg { get; set; }
    }
}