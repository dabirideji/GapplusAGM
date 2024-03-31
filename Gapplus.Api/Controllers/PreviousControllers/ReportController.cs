using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using System.Text;
using System.Web.UI;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using TheArtOfDev.HtmlRenderer.Core;
using OpenHtmlToPdf;
using System.Data.SqlClient;

namespace BarcodeGenerator.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        UsersContext db = new UsersContext();
        UserAdmin ua = new UserAdmin();
        AGMManager am = new AGMManager();
        //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

        //private static int RetrieveAGMUniqueID()
        //{
        //    UsersContext adb = new UsersContext();
        //    var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

        //    return AGMID;
        //}

        //private static int UniqueAGMId = RetrieveAGMUniqueID();

        private static int Presentcount;
        private static double PresentHolding;
        private static string TotalPercentagePresentHolding;
        private static int ProxyCount;
        private static double ProxyHolding;
        private static string TotalPercentageProxyHolding;
        private static int PreregisteredCount;
        private static double PreregisteredHolding;
        private static string TotalPercentagePreregisteredHolding;
        private static int TotalCountPresent_Proxy_Preregistered;
        private static double TotalHoldingPresent_Proxy_Preregistered;
        private static string TotalPercentagePresent_Proxy_Preregistered;



        public async Task<ActionResult> Index()
        {
            var returnUrl = HttpContext.Request.Url.AbsolutePath;
            var companyinfo = ua.GetUserCompanyInfo();

            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            if (String.IsNullOrEmpty(companyinfo))
            {
                return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
            }
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await IndexAsync();


            return PartialView(response);
        }

        [HttpPost]
        public async Task<ActionResult> Index(CompanyModel pmodel)
        {
            var returnUrl = HttpContext.Request.Url.AbsolutePath;
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            //if (String.IsNullOrEmpty(pmodel.Year))
            //{
            //    return View(new ReportViewModelDto());
            //}
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            ViewBag.value = "tab";
            var response = await IndexPostAsync(pmodel);

            return PartialView(response);
        }

        private Task<ReportViewModelDto> IndexAsync()
        {
            ReportViewModelDto model = new ReportViewModelDto();
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var currentYear = DateTime.Now.Year.ToString();
            if (UniqueAGMId != -1)
            {
                model.presentcount = db.Present.Where(p => p.AGMID == UniqueAGMId).Count();
                //model.shareholders = db.BarcodeStore.Where(p => p.Company == companyinfo).Count();
                model.resolutions = db.Question.Where(p => p.AGMID == UniqueAGMId).ToList();

                var setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
                var forBg = "green";
                var againstBg = "red";
                var abstainBg = "blue";
                var voidBg = "black";
                if (setting != null)
                {
                    model.logo = setting.Image != null ? "data:image/jpg;base64," +
                               Convert.ToBase64String((byte[])setting.Image) : "";
                    model.Company = setting.CompanyName;
                    model.AGMTitle = setting.Title;
                    model.AGMID = UniqueAGMId;
                    if (!string.IsNullOrEmpty(setting.VoteForColorBg))
                    {
                        forBg = setting.VoteForColorBg;
                    }
                    if (!string.IsNullOrEmpty(setting.VoteAgainstColorBg))
                    {
                        againstBg = setting.VoteAgainstColorBg;
                    }
                    if (!string.IsNullOrEmpty(setting.VoteAbstaincolorBg))
                    {
                        abstainBg = setting.VoteAbstaincolorBg;
                    }
                    if (!string.IsNullOrEmpty(setting.VoteVoidColorBg))
                    {
                        voidBg = setting.VoteVoidColorBg;
                    }
                }
                model.forBg = forBg;
                model.againstBg = againstBg;
                model.abstainBg = abstainBg;
                model.voidBg = voidBg;

                return Task.FromResult<ReportViewModelDto>(model);
            }
            model.resolutions = new List<Question>();
            return Task.FromResult<ReportViewModelDto>(model);

        }




        private Task<ReportViewModelDto> IndexPostAsync(CompanyModel pmodel)
        {
            ReportViewModelDto model = new ReportViewModelDto();
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            if (UniqueAGMId != -1)
            {
                model.presentcount = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID).Count();
                //model.shareholders = db.BarcodeStore.Where(p => p.Company == companyinfo).Count();
                model.resolutionsArchive = db.QuestionArchive.Where(p => p.AGMID == pmodel.AGMID).ToList();

                var setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
                if (setting != null)
                {
                    model.logo = setting.Image != null ? "data:image/jpg;base64," +
                               Convert.ToBase64String((byte[])setting.Image) : "";
                    model.Company = setting.CompanyName;
                    model.AGMTitle = setting.Title;
                    model.AGMID = UniqueAGMId;
                }
                return System.Threading.Tasks.Task.FromResult<ReportViewModelDto>(model);
            }
            List<QuestionArchive> resolutionlist = new List<QuestionArchive>();
            model.resolutionsArchive = resolutionlist;
            return System.Threading.Tasks.Task.FromResult<ReportViewModelDto>(model);
        }


        //[HttpPost]
        //[ValidateInput(false)]
        //public FileResult Export(string GridHtml)
        //{
        //    using (MemoryStream stream = new System.IO.MemoryStream())
        //    {
        //        StringReader sr = new StringReader(GridHtml);
        //        Document pdfDoc = new Document(PageSize.A4);
        //        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
        //        pdfDoc.Open();              
        //        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
        //        pdfDoc.Close();
        //        return File(stream.ToArray(), "application/pdf", "Grid.pdf");
        //    }

        //}


        //[HttpPost]
        //[ValidateInput(false)]
        public async Task<FileResult> ExportReportDetailsToPdf()
        {
            var response = await VotingReportSummaryAsync();
            var viewInString = RenderToString(PartialView("ReportDetailspdf", response));

            var pdf = Pdf
                
            .From(viewInString)
            .WithGlobalSetting("orientation", "Landscape")
            .WithGlobalSetting("margin.right", "1cm")
            .WithGlobalSetting("margin.left", "1cm")
            .WithObjectSetting("web.defaultEncoding", "utf-8")
            .WithObjectSetting("header.fontSize", "16")
             .WithObjectSetting("header.fontName", "lato")
            .WithObjectSetting("header.line", "true")
            .WithObjectSetting("header.spacing", "1.8")
             .WithObjectSetting("footer.left", "Page [page] of [topage]")
              .WithObjectSetting("footer.right", "[date]")
              .WithObjectSetting("footer.fontName", "lato")
              .WithObjectSetting("footer.fontSize", "10")
            .Content();
            return File(pdf, "application/pdf", "ReportDetails.pdf");
           
            //using (MemoryStream stream = new System.IO.MemoryStream())
            //{
            //    Byte[] res = null;
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(testHtml, PdfSharp.PageSize.A4);
            //        pdf.Save(ms);
            //        res = ms.ToArray();
            //    }
            //    return File(res, "application/pdf", "Grid.pdf");
            //}

        }


        //[HttpPost]
        //[ValidateInput(false)]
        public async Task<FileResult> ExportGlobalSummaryToPdf()
        {
            var model = await GlobalSummaryAsync();
            var viewInString = RenderToString(PartialView("GlobalSummaryPdf", model));

            var pdf = Pdf
            .From(viewInString)
            .WithGlobalSetting("orientation", "Landscape")
            .WithGlobalSetting("margin.right", "1cm")
            .WithGlobalSetting("margin.left", "1cm")
            .WithObjectSetting("web.defaultEncoding", "utf-8")
            .WithObjectSetting("header.fontSize", "16")
             .WithObjectSetting("header.fontName", "lato")
            .WithObjectSetting("header.line", "true")
            .WithObjectSetting("header.spacing", "1.8")
             .WithObjectSetting("footer.left", "Page [page] of [topage]")
              .WithObjectSetting("footer.right", "[date]")
              .WithObjectSetting("footer.fontName", "lato")
              .WithObjectSetting("footer.fontSize", "10")
            .Content();
            return File(pdf, "application/pdf", "GlobalSummary.pdf");
        }

        //[HttpPost]
        //[ValidateInput(false)]
        public async Task<FileResult> ExportVotingReportSummaryToPdf()
        {
            var model = await GlobalSummaryAsync();
            var viewInString = RenderToString(PartialView("VotingReportSummaryPdf", model));

            var pdf = Pdf
            .From(viewInString)
            .WithGlobalSetting("orientation", "Landscape")
            .WithGlobalSetting("margin.right", "1cm")
            .WithGlobalSetting("margin.left", "1cm")
            .WithObjectSetting("web.defaultEncoding", "utf-8")
            .WithObjectSetting("header.fontSize", "16")
             .WithObjectSetting("header.fontName", "lato")
            .WithObjectSetting("header.line", "true")
            .WithObjectSetting("header.spacing", "1.8")
             .WithObjectSetting("footer.left", "Page [page] of [topage]")
              .WithObjectSetting("footer.right", "[date]")
              .WithObjectSetting("footer.fontName", "lato")
              .WithObjectSetting("footer.fontSize", "10")
            .Content();
            return File(pdf, "application/pdf", "GlobalSummary.pdf");
        }

        //public static Byte[] PdfSharpConvert(String html)
        //{
        //    Byte[] res = null;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
        //        pdf.Save(ms);
        //        res = ms.ToArray();
        //    }
        //    return res;
        //}

        public async Task<ActionResult> GlobalSummary()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = "tab";
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            var model = await GlobalSummaryAsync();

            return PartialView(model);
        }


        public async Task<ActionResult> ReportDetails()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            //var UniqueAGMId = ua.RetrieveAGMUniqueID();

            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = "tab";
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            var response = await VotingReportSummaryAsync();

            return PartialView(response);
        }


        public async Task<ActionResult> ReportDetailsPdf()
        {

            var response = await VotingReportSummaryAsync();

            return View(response);
        }

        public async Task<ActionResult> GlobalSummaryPdf()
        {

            var response = await GlobalSummaryAsync();

            return View(response);
        }

        public async Task<ActionResult> VotingReportSummaryPdf()
        {

            var response = await VotingReportSummaryAsync();

            return View(response);
        }



        public ActionResult Invoicepdf()
        {
            return View();
        }

        public async Task<ActionResult> VotingReportSummary()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = "tab";
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            var response = await VotingReportSummaryAsync();

            return PartialView(response);
        }


        private Task<List<ReportViewModelDto>> GlobalSummaryAsync()
        {

            List<ReportViewModelDto> globalSummaryList = new List<ReportViewModelDto>();

            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            if (UniqueAGMId != -1)
            {

                var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                var allResolution = db.Question.Where(r => r.AGMID == UniqueAGMId).ToList();

                foreach (var resolution in allResolution)
                {
                    ReportViewModelDto model = new ReportViewModelDto();
                    Double holding = 0;
                    Double percentHolding = 0;
                    Double holdingAgainst = 0;
                    Double percentHoldingAgainst = 0;
                    Double holdingAbstain = 0;
                    Double percentHoldingAbstain = 0;
                    Double holdingVoid = 0;
                    Double percentHoldingVoid = 0;
                    Double TotalHolding = 0;
                    Double TotalPercentHolding = 0;
                    //var resolution = db.Question.Find(id);
                    model.Id = resolution.Id;
                    model.resolutionName = resolution.question;



                    var abstainbtnchoice = true;
                    var forBg = "green";
                    var againstBg = "red";
                    var abstainBg = "blue";
                    var voidBg = "black";
                    if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
                    {
                        abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
                        if (!string.IsNullOrEmpty(agmEventSetting.VoteForColorBg))
                        {
                            forBg = agmEventSetting.VoteForColorBg;
                        }
                        if (!string.IsNullOrEmpty(agmEventSetting.VoteAgainstColorBg))
                        {
                            againstBg = agmEventSetting.VoteAgainstColorBg;
                        }
                        if (!string.IsNullOrEmpty(agmEventSetting.VoteAbstaincolorBg))
                        {
                            abstainBg = agmEventSetting.VoteAbstaincolorBg;
                        }
                        if (!string.IsNullOrEmpty(agmEventSetting.VoteVoidColorBg))
                        {
                            voidBg = agmEventSetting.VoteVoidColorBg;
                        }
                        model.AGMID = UniqueAGMId;
                    }
                    model.abstainBtnChoice = abstainbtnchoice;
                    model.forBg = forBg;
                    model.againstBg = againstBg;
                    model.abstainBg = abstainBg;
                    model.voidBg = voidBg;
                    model.Company = agmEventSetting.CompanyName;
                    model.AGMTitle = agmEventSetting.PrintOutTitle;
                    model.AGMDateTime = agmEventSetting.AgmDateTime.ToString();
                    //ResolutionResult resolutionResult = new ResolutionResult
                    //{
                    model.ResultFor = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteFor == true).ToList();
                    model.ResultAgainst = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAgainst == true).ToList();
                    model.ResultAbstain = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAbstain == true).ToList();
                    model.ResultVoid = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteVoid == true).ToList();
                    //};

                    var ResultForCount = model.ResultFor.Count();
                    var ResultAgainstCount = model.ResultAgainst.Count();
                    var ResultAbstainCount = model.ResultAbstain.Count();
                    var ResultVoidCount = model.ResultVoid.Count();
                    int resultCount = 0;
                    if (abstainbtnchoice)
                    {
                        resultCount = resolution.result.Where(r => r.AGMID == UniqueAGMId).Count();
                    }
                    else
                    {
                        resultCount = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAbstain == false).Count();
                    }

                    model.ResultForCount = ResultForCount;
                    model.ResultAgainstCount = ResultAgainstCount;
                    model.ResultAbstainCount = ResultAbstainCount;
                    model.ResultVoidCount = ResultVoidCount;
                    model.TotalCount = resultCount;

                    SqlConnection conn =
                    new SqlConnection(connStr);
                    //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                    string query = string.Empty;
                    if (agmEventSetting.PreregisteredVotes)
                    {
                        query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND [VoteFor] = 1";
                    }
                    else
                    {
                         query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND [VoteFor] = 1 AND Present=1)" +
                            "OR (QuestionId = '" + resolution.Id + "' AND[VoteFor] = 1 AND Present = 1 AND Pregistered = 1)" +
                            " OR (QuestionId='" + resolution.Id + "' AND [VoteFor] = 1 AND PresentByProxy = 1)";
                    }
                   
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    //object o = cmd.ExecuteScalar();
                    if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                    {
                        holding = Convert.ToDouble(cmd.ExecuteScalar());
                    }
                    conn.Close();
                    if (agmEventSetting.PreregisteredVotes)
                    {
                        query = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + resolution.Id + "' AND [VoteFor] = 1";
                    }
                    else
                    {
                        query = "SELECT SUM(PercentageHolding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND [VoteFor] = 1 AND Present=1) " + 
                            "OR (QuestionId='" + resolution.Id + "' AND [VoteFor] = 1 AND Present=1 AND Pregistered = 1) " +
                            "OR (QuestionId='" + resolution.Id + "' AND [VoteFor] = 1 AND PresentByProxy = 1)";
                    }
                    conn.Open();
                    SqlCommand cmd2 = new SqlCommand(query, conn);
                    if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                    {
                        percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                    }
                    conn.Close();

                    model.ResultForHolding = holding.ToString();
                    model.ResultForPercentHolding = percentHolding.ToString();

                    //            SqlConnection conn =
                    //new SqlConnection(connStr);
                    //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                    if (agmEventSetting.PreregisteredVotes)
                    {
                        query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteAgainst = 1";
                    }
                    else
                    {
                        query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteAgainst = 1 AND Present=1)" +
                            "OR (QuestionId='" + resolution.Id + "' AND VoteAgainst = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteAgainst = 1 AND PresentByProxy = 1) ";
                    }
                    conn.Open();
                    SqlCommand cmd3 = new SqlCommand(query, conn);

                    if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                    {
                        holdingAgainst = Convert.ToDouble(cmd3.ExecuteScalar());
                    }
                    conn.Close();
                    if (agmEventSetting.PreregisteredVotes)
                    {
                        query = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + resolution.Id + "' AND VoteAgainst = 1";
                    }
                    else
                    {
                        query = "SELECT SUM(PercentageHolding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteAgainst = 1 AND Present=1) " +
                            "OR (QuestionId='" + resolution.Id + "' AND VoteAgainst = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteAgainst = 1 AND PresentByProxy = 1)";
                    }
                    conn.Open();
                    SqlCommand cmd4 = new SqlCommand(query, conn);
                    if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                    {
                        percentHoldingAgainst = Convert.ToDouble(cmd4.ExecuteScalar());
                    }
                    conn.Close();

                    model.ResultForHoldingAgainst = holdingAgainst.ToString();
                    model.ResultForPercentHoldingAgainst = percentHoldingAgainst.ToString();
                    if (abstainbtnchoice)
                    {

                        if (agmEventSetting.PreregisteredVotes)
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE   QuestionId='" + resolution.Id + "' AND VoteAbstain = 1";
                        }
                        else
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteAbstain = 1 AND Present=1)" +
                                "OR (QuestionId='" + resolution.Id + "' AND VoteAbstain = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteAbstain = 1 AND PresentByProxy = 1)";
                        }
                        conn.Open();
                        SqlCommand cmd5 = new SqlCommand(query, conn);

                        if (!DBNull.Value.Equals(cmd5.ExecuteScalar()))
                        {
                            holdingAbstain = Convert.ToDouble(cmd5.ExecuteScalar());
                        }
                        conn.Close();
                        if (agmEventSetting.PreregisteredVotes)
                        {
                            query = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteAbstain = 1";
                        }
                        else
                        {
                            query = "SELECT SUM(PercentageHolding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteAbstain = 1 AND Present=1)" +
                                "OR (QuestionId='" + resolution.Id + "' AND VoteAbstain = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteAbstain = 1 AND PresentByProxy = 1)";
                        }
                            conn.Open();
                        SqlCommand cmd6 = new SqlCommand(query, conn);
                        if (!DBNull.Value.Equals(cmd6.ExecuteScalar()))
                        {
                            percentHoldingAbstain = Convert.ToDouble(cmd6.ExecuteScalar());
                        }
                        conn.Close();

                        model.ResultForHoldingAbstain = holdingAbstain.ToString();
                        model.ResultForPercentHoldingAbstain = percentHoldingAbstain.ToString();
                    }

                    if (agmEventSetting.PreregisteredVotes)
                    {
                        query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteVoid = 1";
                    }
                    else
                    {
                        query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteVoid = 1 AND Present=1)" +
                            "OR (QuestionId='" + resolution.Id + "' AND VoteVoid = 1 AND Present=1 AND Pregistered = 1)  OR (QuestionId='" + resolution.Id + "' AND VoteVoid = 1 AND PresentByProxy = 1)";
                    }
                    conn.Open();
                    SqlCommand cmd8 = new SqlCommand(query, conn);

                    if (!DBNull.Value.Equals(cmd8.ExecuteScalar()))
                    {
                        holdingVoid = Convert.ToDouble(cmd8.ExecuteScalar());
                    }
                    conn.Close();
                    if (agmEventSetting.PreregisteredVotes)
                    {
                        query = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + resolution.Id + "' AND VoteVoid = 1";
                    }
                    else
                    {
                        query = "SELECT SUM(PercentageHolding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteVoid = 1 AND Present=1)" +
                            "OR (QuestionId='" + resolution.Id + "' AND VoteVoid = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteVoid = 1 AND PresentByProxy = 1)";
                    }
                    conn.Open();
                    SqlCommand cmd9 = new SqlCommand(query, conn);
                    if (!DBNull.Value.Equals(cmd9.ExecuteScalar()))
                    {
                        percentHoldingVoid = Convert.ToDouble(cmd9.ExecuteScalar());
                    }
                    conn.Close();

                    model.ResultForHoldingVoid = holdingVoid.ToString();
                    model.ResultForPercentHoldingAgainst = percentHoldingVoid.ToString();

                    if (abstainbtnchoice)
                    {
                        if (agmEventSetting.PreregisteredVotes)
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "'";
                        }
                        else
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND Present=1)" +
                                "OR (QuestionId='" + resolution.Id + "' AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND PresentByProxy = 1)";
                        }
                        conn.Open();
                        SqlCommand cmd7 = new SqlCommand(query, conn);
                        if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
                        {
                            TotalHolding = Convert.ToDouble(cmd7.ExecuteScalar());
                        }
                        conn.Close();
                    }
                    else
                    {
                        if (agmEventSetting.PreregisteredVotes)
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteAbstain != 1";
                        }
                        else
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteAbstain != 1 AND Present=1)" +
                                "OR (QuestionId='" + resolution.Id + "' AND VoteAbstain != 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteAbstain != 1 AND PresentByProxy = 1)";
                        }
                        conn.Open();
                        SqlCommand cmd7 = new SqlCommand(query, conn);
                        if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
                        {
                            TotalHolding = Convert.ToDouble(cmd7.ExecuteScalar());
                        }
                        conn.Close();
                    }


                    model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0");


                    //calcuate percentage holding per resolution..
                    var pFor = (holding / TotalHolding) * 100;
                    model.PercentageResultFor = pFor.ToString();
                    var pAgainst = (holdingAgainst / TotalHolding) * 100;
                    model.PercentageResultAgainst = pAgainst.ToString();
                    double pAbstain = 0;
                    if (abstainbtnchoice)
                    {
                        pAbstain = (holdingAbstain / TotalHolding) * 100;
                        model.PercentageResultAbstain = pAbstain.ToString();
                    }
                    var pVoid = (holdingVoid / TotalHolding) * 100;
                    model.PercentageResultVoid = pVoid.ToString();
                    var sumAllPercentages = pFor + pAgainst + pAbstain + pVoid;
                    model.TotalPercentageHolding = sumAllPercentages.ToString();


                    globalSummaryList.Add(model);
                }

            }


            return Task.FromResult<List<ReportViewModelDto>>(globalSummaryList);
        }





        private Task<List<ReportViewModelDto>> VotingReportSummaryAsync()
        {

            List<ReportViewModelDto> globalSummaryList = new List<ReportViewModelDto>();

            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            if (UniqueAGMId != -1)
            {

                var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                var allResolution = db.Question.Where(r => r.AGMID == UniqueAGMId).ToList();

                foreach (var resolution in allResolution)
                {
                    ReportViewModelDto model = new ReportViewModelDto();
                    Double holding = 0;
                    Double percentHolding = 0;
                    Double holdingAgainst = 0;
                    Double percentHoldingAgainst = 0;
                    Double holdingAbstain = 0;
                    Double percentHoldingAbstain = 0;
                    Double holdingVoid = 0;
                    Double percentHoldingVoid = 0;
                    Double TotalHolding = 0;
                    Double TotalPercentHolding = 0;
                    //var resolution = db.Question.Find(id);
                    model.Id = resolution.Id;
                    model.resolutionName = resolution.question;



                    var abstainbtnchoice = true;
                    var forBg = "green";
                    var againstBg = "red";
                    var abstainBg = "blue";
                    var voidBg = "black";
                    if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
                    {
                        abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
                        if (!string.IsNullOrEmpty(agmEventSetting.VoteForColorBg))
                        {
                            forBg = agmEventSetting.VoteForColorBg;
                        }
                        if (!string.IsNullOrEmpty(agmEventSetting.VoteAgainstColorBg))
                        {
                            againstBg = agmEventSetting.VoteAgainstColorBg;
                        }
                        if (!string.IsNullOrEmpty(agmEventSetting.VoteAbstaincolorBg))
                        {
                            abstainBg = agmEventSetting.VoteAbstaincolorBg;
                        }
                        if (!string.IsNullOrEmpty(agmEventSetting.VoteVoidColorBg))
                        {
                            voidBg = agmEventSetting.VoteVoidColorBg;
                        }
                      
                    }
                    model.abstainBtnChoice = abstainbtnchoice;
                    model.forBg = forBg;
                    model.againstBg = againstBg;
                    model.abstainBg = abstainBg;
                    model.voidBg = voidBg;
                    model.Company = agmEventSetting.CompanyName;
                    model.AGMTitle = agmEventSetting.PrintOutTitle;
                    model.AGMDateTime = agmEventSetting.AgmDateTime.ToString();
                    model.AGMID = UniqueAGMId;
                    //ResolutionResult resolutionResult = new ResolutionResult
                    //{
                    model.ResultFor = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteFor == true).ToList();
                    model.ResultAgainst = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAgainst == true).ToList();
                    model.ResultAbstain = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAbstain == true).ToList();
                    model.ResultVoid = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteVoid == true).ToList();
                    //};

                    var ResultForCount = model.ResultFor.Count();
                    var ResultAgainstCount = model.ResultAgainst.Count();
                    var ResultAbstainCount = model.ResultAbstain.Count();
                    var ResultVoidCount = model.ResultVoid.Count();
                    int resultCount = 0;
                    if (abstainbtnchoice)
                    {
                        resultCount = resolution.result.Where(r => r.AGMID == UniqueAGMId).Count();
                    }
                    else
                    {
                        resultCount = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAbstain == false).Count();
                    }

                    model.ResultForCount = ResultForCount;
                    model.ResultAgainstCount = ResultAgainstCount;
                    model.ResultAbstainCount = ResultAbstainCount;
                    model.ResultVoidCount = ResultVoidCount;
                    model.TotalCount = resultCount;

                    model.Decision = am.GetAGMDecision(ResultForCount, ResultAgainstCount, ResultAbstainCount, ResultVoidCount);
                    SqlConnection conn =
                    new SqlConnection(connStr);
                    string query = string.Empty;

                    if (model.Decision == "FOR")
                    {
                        model.ResultDecisionCount = ResultForCount;
                        model.ResultDecision = model.ResultFor;
                        //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                      
                        if (agmEventSetting.PreregisteredVotes)
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND [VoteFor] = 1";
                        }
                        else
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND [VoteFor] = 1 AND Present=1)" +
                                "OR (QuestionId='" + resolution.Id + "' AND [VoteFor] = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND [VoteFor] = 1 AND PresentByProxy = 1)";
                        }
                      
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
                        //object o = cmd.ExecuteScalar();
                        if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                        {
                            holding = Convert.ToDouble(cmd.ExecuteScalar());
                        }
                        conn.Close();
                        if (agmEventSetting.PreregisteredVotes)
                        {
                            query = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + resolution.Id + "' AND [VoteFor] = 1";
                        }
                        else
                        {
                            query = "SELECT SUM(PercentageHolding) FROM Results WHERE ( QuestionId='" + resolution.Id + "' AND [VoteFor] = 1 AND Present=1)" +
                                "OR ( QuestionId='" + resolution.Id + "' AND [VoteFor] = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND [VoteFor] = 1 AND PresentByProxy = 1)";
                        }
                        conn.Open();
                        SqlCommand cmd2 = new SqlCommand(query, conn);
                        if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                        {
                            percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                        }
                        conn.Close();

                        model.ResultDecisionHolding = holding.ToString();
                        model.ResultDecisionPercentHolding = percentHolding.ToString();
                    }
                    else if (model.Decision == "AGAINST")
                    {
                        model.ResultDecisionCount = ResultAgainstCount;
                        model.ResultDecision = model.ResultAgainst;
                        if (agmEventSetting.PreregisteredVotes)
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteAgainst = 1";
                        }
                        else
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteAgainst = 1 AND Present=1)" +
                                "OR (QuestionId='" + resolution.Id + "' AND VoteAgainst = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteAgainst = 1 AND PresentByProxy = 1)";
                        }
                        conn.Open();
                        SqlCommand cmd3 = new SqlCommand(query, conn);

                        if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                        {
                            holdingAgainst = Convert.ToDouble(cmd3.ExecuteScalar());
                        }
                        conn.Close();
                        if (agmEventSetting.PreregisteredVotes)
                        {
                            query = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + resolution.Id + "' AND VoteAgainst = 1";
                        }
                        else
                        {
                            query = "SELECT SUM(PercentageHolding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteAgainst = 1 AND Present=1) " +
                                " OR (QuestionId='" + resolution.Id + "' AND VoteAgainst = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteAgainst = 1 AND PresentByProxy = 1)";
                        }
                        conn.Open();
                        SqlCommand cmd4 = new SqlCommand(query, conn);
                        if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                        {
                            percentHoldingAgainst = Convert.ToDouble(cmd4.ExecuteScalar());
                        }
                        conn.Close();

                        model.ResultDecisionHolding = holdingAgainst.ToString();
                        model.ResultDecisionPercentHolding = percentHoldingAgainst.ToString();
                    }
                    else if (model.Decision == "ABSTAIN")
                    {
                        if (abstainbtnchoice)
                        {

                            model.ResultDecisionCount = ResultAbstainCount;
                            model.ResultDecision = model.ResultAbstain;
                            if (agmEventSetting.PreregisteredVotes)
                            {
                                query = "SELECT SUM(Holding) FROM Results WHERE   QuestionId='" + resolution.Id + "' AND VoteAbstain = 1";
                            }
                            else
                            {
                                query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteAbstain = 1 AND Present=1)" +
                                    "OR (QuestionId='" + resolution.Id + "' AND VoteAbstain = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteAbstain = 1 AND PresentByProxy = 1)";
                            }
                            conn.Open();
                            SqlCommand cmd5 = new SqlCommand(query, conn);

                            if (!DBNull.Value.Equals(cmd5.ExecuteScalar()))
                            {
                                holdingAbstain = Convert.ToDouble(cmd5.ExecuteScalar());
                            }
                            conn.Close();
                            if (agmEventSetting.PreregisteredVotes)
                            {
                                query = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteAbstain = 1";
                            }
                            else
                            {
                                query = "SELECT SUM(PercentageHolding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteAbstain = 1 AND Present=1)" +
                                    "OR (QuestionId='" + resolution.Id + "' AND VoteAbstain = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteAbstain = 1 AND PresentByProxy = 1)";
                            }
                            conn.Open();
                            SqlCommand cmd6 = new SqlCommand(query, conn);
                            if (!DBNull.Value.Equals(cmd6.ExecuteScalar()))
                            {
                                percentHoldingAbstain = Convert.ToDouble(cmd6.ExecuteScalar());
                            }
                            conn.Close();

                            model.ResultDecisionHolding = holdingAbstain.ToString();
                            model.ResultDecisionPercentHolding = percentHoldingAbstain.ToString();
                        }

                    }
                    else if (model.Decision == "VOID")
                    {
                        model.ResultDecisionCount = ResultVoidCount;
                        model.ResultDecision = model.ResultVoid;
                        if (agmEventSetting.PreregisteredVotes)
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteVoid = 1";
                        }
                        else
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteVoid = 1 AND Present=1)" +
                                "OR (QuestionId='" + resolution.Id + "' AND VoteVoid = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteVoid = 1 AND PresentByProxy = 1)";
                        }
                        conn.Open();
                        SqlCommand cmd8 = new SqlCommand(query, conn);

                        if (!DBNull.Value.Equals(cmd8.ExecuteScalar()))
                        {
                            holdingVoid = Convert.ToDouble(cmd8.ExecuteScalar());
                        }
                        conn.Close();
                        if (agmEventSetting.PreregisteredVotes)
                        {
                            query = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + resolution.Id + "' AND VoteVoid = 1";
                        }
                        else
                        {
                            query = "SELECT SUM(PercentageHolding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteVoid = 1 AND Present=1)" +
                                "OR (QuestionId='" + resolution.Id + "' AND VoteVoid = 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteVoid = 1 AND PresentByProxy = 1)";
                        }
                        conn.Open();
                        SqlCommand cmd9 = new SqlCommand(query, conn);
                        if (!DBNull.Value.Equals(cmd9.ExecuteScalar()))
                        {
                            percentHoldingVoid = Convert.ToDouble(cmd9.ExecuteScalar());
                        }
                        conn.Close();

                        model.ResultDecisionHolding = holdingVoid.ToString();
                        model.ResultDecisionPercentHolding = percentHoldingVoid.ToString();
                    }
                    else
                    {
                        model.ResultDecision = new List<Result>();
                        model.ResultDecisionHolding = 0.ToString();
                        model.ResultDecisionPercentHolding = 0.ToString();
                    }

                    if (abstainbtnchoice)
                    {
                        if (agmEventSetting.PreregisteredVotes)
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "'";
                        }
                        else
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND Present=1)" +
                                "OR (QuestionId='" + resolution.Id + "' AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND PresentByProxy = 1)";
                        }
                        conn.Open();
                        SqlCommand cmd7 = new SqlCommand(query, conn);
                        if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
                        {
                            TotalHolding = Convert.ToDouble(cmd7.ExecuteScalar());
                        }
                        conn.Close();
                    }
                    else
                    {
                        if (agmEventSetting.PreregisteredVotes)
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteAbstain != 1";
                        }
                        else
                        {
                            query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + resolution.Id + "' AND VoteAbstain != 1 AND Present=1)" +
                                "OR (QuestionId='" + resolution.Id + "' AND VoteAbstain != 1 AND Present=1 AND Pregistered = 1) OR (QuestionId='" + resolution.Id + "' AND VoteAbstain != 1 AND PresentByProxy = 1)";
                        }
                        conn.Open();
                        SqlCommand cmd7 = new SqlCommand(query, conn);
                        if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
                        {
                            TotalHolding = Convert.ToDouble(cmd7.ExecuteScalar());
                        }
                        conn.Close();
                    }


                    model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0");


                    //calcuate percentage holding per resolution..
                    var pFor = (holding / TotalHolding) * 100;
                    model.PercentageResultFor = pFor.ToString();
                    var pAgainst = (holdingAgainst / TotalHolding) * 100;
                    model.PercentageResultAgainst = pAgainst.ToString();
                    double pAbstain = 0;
                    if (abstainbtnchoice)
                    {
                        pAbstain = (holdingAbstain / TotalHolding) * 100;
                        model.PercentageResultAbstain = pAbstain.ToString();
                    }
                    var pVoid = (holdingVoid / TotalHolding) * 100;
                    model.PercentageResultVoid = pVoid.ToString();
                    var sumAllPercentages = pFor + pAgainst + pAbstain + pVoid;
                    model.TotalPercentageHolding = sumAllPercentages.ToString();


                    globalSummaryList.Add(model);
                }

            }


            return Task.FromResult<List<ReportViewModelDto>>(globalSummaryList);
        }






        public async Task<ActionResult> RefreshIndex()
        {
            var model = await RefreshIndexAsync();

            return PartialView(model);
        }


        private Task<ReportViewModelDto> RefreshIndexAsync()
        {
            ReportViewModelDto model = new ReportViewModelDto();
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            if (UniqueAGMId == -1 || string.IsNullOrEmpty(companyinfo))
            {
                return Task.FromResult(new ReportViewModelDto());
            }
            var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
            if (agmEventSetting==null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }
            var currentYear = DateTime.Now.Year.ToString();
            if (UniqueAGMId != -1)
            {
                model.presentcount = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1).Count();
                //model.feedbackcount = db.ShareholderFeedback.Count();
                //model.shareholders = db.BarcodeStore.Where(p => p.Company == companyinfo).Count();
                int TotalCount = 0;
                Double TotalHolding = 0;
                Double TotalPercentageHolding = 0;
                Double presentholding = 0;
                Double presentpercentHolding = 0;
                Double proxyholding = 0;
                Double proxypercentHolding = 0;
                model.AGMID = UniqueAGMId;
                var present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.present == true && p.PermitPoll == 1).Count();
                var presentcount = present;
                model.present = presentcount;
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                string query = string.Empty;
                //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                if (agmEventSetting.PreregisteredVotes)
                {
                     query = "SELECT SUM(Holding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1)" +
                        " OR (AGMID = '" + UniqueAGMId + "' AND preregistered = 1 AND PermitPoll = 1)";
                }
                else
                {
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1)" +
                        "OR (AGMID = '" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1 AND preregistered = 1)";
                       
                     
                }
                
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    presentholding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND present= 1 AND PermitPoll = 1)" +
                        "OR (AGMID = '" + UniqueAGMId + "' AND preregistered = 1 AND PermitPoll = 1)";
                }
                else
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND present= 1 AND PermitPoll = 1)" +
                        "OR (AGMID = '" + UniqueAGMId + "' AND present= 1 AND PermitPoll = 1 AND preregistered = 1)";
                }
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageHolding = String.Format("{0:0.####}", presentpercentHolding);
                model.Holding = presentholding;

                var proxy = db.Present.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).ToList();
                var proxycount = proxy.Count();
                model.proxy = proxycount;

                string query3 = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd3 = new SqlCommand(query3, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                {
                    presentholding = Convert.ToDouble(cmd3.ExecuteScalar());
                }
                conn.Close();

                string query4 = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND proxy= 1";
                conn.Open();
                SqlCommand cmd4 = new SqlCommand(query4, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                {
                    proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
                }
                conn.Close();


                model.TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
                model.ProxyHolding = proxyholding;

                TotalCount = proxycount + presentcount;
                model.TotalCount = TotalCount;

                TotalHolding = presentholding + proxyholding;
                model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0");

                TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
                model.PercentageTotalHolding = String.Format("{0:0.####}", TotalPercentageHolding);
                Int64 shareholders = 1;

                string query6 = "SELECT COUNT ( DISTINCT Id ) FROM BarcodeModels Where Company ='" + companyinfo + "'";
                conn.Open();
                SqlCommand cmd6 = new SqlCommand(query6, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd6.ExecuteScalar()))
                {
                    shareholders = Convert.ToInt64(cmd6.ExecuteScalar());
                }
                conn.Close();

                var percentagePresent = ((double)presentcount / (double)shareholders) * 100;
                var percentageProxy = ((double)proxycount / (double)shareholders) * 100;
                model.percentagePresent = String.Format("{0:0.####}", percentagePresent);
                model.percentageProxy = String.Format("{0:0.####}", percentageProxy);


            }

            return System.Threading.Tasks.Task.FromResult<ReportViewModelDto>(model);

        }

        public async Task<ActionResult> PresentAnalysisIndex()
        {
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            var response = await PresentAnalysisIndexAsync();

            return PartialView(response);
        }

        public Task<ReportViewModelDto> PresentAnalysisIndexAsync()
        {
            Double holdingPresent = 0;
            Double percentHoldingPresent = 0;
            Double holdingProxy = 0;
            Double percentHoldingProxy = 0;
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            var currentYear = DateTime.Now.Year.ToString();
            ReportViewModelDto model = new ReportViewModelDto();
            var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }

            if (UniqueAGMId != -1)
            {
                var present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.present == true && p.PermitPoll == 1).Count();
                var presentcount = present;

                model.presentcount = presentcount;
                model.AGMID = UniqueAGMId;
                var proxy = db.Present.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).ToList();
                var proxycount = proxy.Count();
                model.proxycount = proxycount;
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                string query = string.Empty;
                //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                if (agmEventSetting.PreregisteredVotes)
                {
                    
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1)" +
                        "OR (AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1 AND preregistered = 1)";
                }
                else
                {
                   query = "SELECT SUM(Holding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1)" +
                        "OR (AGMID = '" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1 AND preregistered = 1)";
                }

                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    holdingPresent = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND present= 1 AND PermitPoll = 1) " +
                    "OR (AGMID = '" + UniqueAGMId + "' AND preregistered = 1 AND PermitPoll = 1)";
                }
                else
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND present= 1 AND PermitPoll = 1)" +
                   "OR (AGMID = '" + UniqueAGMId + "' AND present= 1 AND PermitPoll = 1 AND preregistered = 1)";
                }
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    percentHoldingPresent = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageHoldingPresent = String.Format("{0:0.##}", percentHoldingPresent);
                model.HoldingPresent = holdingPresent;

                string query3 = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd3 = new SqlCommand(query3, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                {
                    holdingProxy = Convert.ToDouble(cmd3.ExecuteScalar());
                }
                conn.Close();

                string query4 = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd4 = new SqlCommand(query4, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                {
                    percentHoldingProxy = Convert.ToDouble(cmd4.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageHoldingProxy = String.Format("{0:0.##}", percentHoldingProxy);
                model.HoldingProxy = holdingProxy;
                //ViewBag.feedbackcount = db.ShareholderFeedback.Count();
                //model.shareholders = db.BarcodeStore.Where(b=>b.Company==companyinfo).Count();

            }


            return Task.FromResult<ReportViewModelDto>(model);
        }

        public async Task<ActionResult> Voted()
        {
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            var response = await VotedAsync();
            return PartialView(response);
        }


        public Task<ReportViewModelDto> VotedAsync()
        {

            //Double TotalShareholding = 326700000;
            Double holding = 0;
            Double percentHolding = 0;

            ReportViewModelDto model = new ReportViewModelDto();
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }
            var currentYear = DateTime.Now.Year.ToString();

            if (UniqueAGMId != -1)
            {
                List<PresentModel> voted;
                if (agmEventSetting.PreregisteredVotes)
                {
                    voted = db.Present.Where(p => p.AGMID == UniqueAGMId && p.TakePoll == true).ToList();
                }
                else
                {
                    voted = db.Present.Where(p => p.AGMID == UniqueAGMId && p.TakePoll == true).ToList();
                }
                model.Voted = voted;
                var votedcount = voted.Count();
                model.Votes = voted.Count();

                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                string query = string.Empty;
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND present=1  AND TakePoll = 1)" +
                        "OR (AGMID = '" + UniqueAGMId + "' AND preregistered = 1 AND TakePoll = 1)";
                }
                else
                {
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND present=1  AND TakePoll = 1)" +
                        "OR (AGMID = '" + UniqueAGMId + "' AND preregistered = 1 AND TakePoll = 1 AND present=1)";
                }

                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    holding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
                if (agmEventSetting.PreregisteredVotes)
                {
                     query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND present=1 AND TakePoll= 1)" +
                    " OR (AGMID = '" + UniqueAGMId + "' AND preregistered = 1 AND TakePoll= 1)";
                }
                else
                {
                     query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND TakePoll= 1)" +
                    " OR (AGMID = '" + UniqueAGMId + "' AND preregistered = 1 AND TakePoll= 1 AND present=1)";
                }
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();


                model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
                model.Holding = holding;
                model.AGMID = UniqueAGMId;

            }



            return Task.FromResult<ReportViewModelDto>(model);
        }


        public async Task<ActionResult> VotedPerQuestion(int id)
        {
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            var response = await VotedPerQuestionAsync(id);

            return View(response);
        }

        public Task<ReportViewModelDto> VotedPerQuestionAsync(int id)
        {

            Double holding = 0;
            Double percentHolding = 0;
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }

            var question = db.Question.Find(id);
            ReportViewModelDto model = new ReportViewModelDto();
            model.Question = question.question;
            var resultPerQuestion = db.Result.Where(p => p.QuestionId == id).ToList();
            model.Result = resultPerQuestion;
            var resultPerQuestionCount = resultPerQuestion.Count();
            model.Votes = resultPerQuestionCount;

            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn =
                      new SqlConnection(connStr);
            string query = string.Empty;
            //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
            if (agmEventSetting.PreregisteredVotes)
            {
                query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "'";
            }
            else
            {
                query = "SELECT SUM(Holding) FROM Results WHERE (QuestionId='" + id + "' AND Present = 1)" +
                    "OR (QuestionId='" + id + "' AND Present = 1 AND Pregistered = 1 )" +
                     "OR (QuestionId='" + id + "' AND PresentByProxy= 1 )";
            }
               
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            //object o = cmd.ExecuteScalar();
            if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
            {
                holding = Convert.ToDouble(cmd.ExecuteScalar());
            }
            conn.Close();
            if (agmEventSetting.PreregisteredVotes)
            {
               query = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "'";
            }
            else
            {
                query = "SELECT SUM(PercentageHolding) FROM Results WHERE (QuestionId='" + id + "' AND Present = 1) " +
                                        "OR (QuestionId='" + id + "' AND Present = 1 AND Pregistered = 1 )" +
                     "OR (QuestionId='" + id + "' AND PresentByProxy= 1 )";
            }
            conn.Open();
            SqlCommand cmd2 = new SqlCommand(query, conn);
            //object o = cmd.ExecuteScalar();
            if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
            {
                percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
            }
            conn.Close();

            model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
            model.Holding = holding;
            model.AGMID = UniqueAGMId;

            return Task.FromResult<ReportViewModelDto>(model);
        }

        public async Task<ActionResult> Present()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await PresentAsync();
            return PartialView(response);
        }

        [HttpPost]
        public async Task<ActionResult> Present(CompanyModel pmodel)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            //if (String.IsNullOrEmpty(pmodel.Year))
            //{
            //    return View(new ReportViewModelDto());
            //}

            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            ViewBag.value = "present";
            var response = await PresentPostAsync(pmodel);
            return PartialView(response);
        }

        public Task<ReportViewModelDto> PresentAsync()
        {

            Double holding = 0;
            Double percentHolding = 0;
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }
            var currentYear = DateTime.Now.Year.ToString();
            ReportViewModelDto model = new ReportViewModelDto();

            if (UniqueAGMId != -1)
            {
                int present = 0;
                if (agmEventSetting.PreregisteredVotes)
                {
                    present = db.Present.Where(p => p.AGMID == UniqueAGMId  && p.PermitPoll == 1).Count();
                }
                else
                {
                    present = db.Present.Where(p => p.AGMID == UniqueAGMId &&  p.PermitPoll == 1 ||
                    p.AGMID == UniqueAGMId && p.present == true && p.preregistered == true && p.PermitPoll == 1).Count();

                }
                var proxy = db.Present.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).Count();
                var presentcount = present;
                var proxycount = proxy;
                model.present = presentcount + proxycount;
                model.proxy = proxycount;

                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                string query = string.Empty;
                if (agmEventSetting.PreregisteredVotes)
                {
                     query = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1";
                }
                else
                {
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "'AND present=1 AND PermitPoll = 1 )" +
                        "OR (AGMID = '" + UniqueAGMId + "'AND present=1 AND preregistered=1 AND PermitPoll = 1)";
                }
              
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    holding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();

                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1";
                }
                else
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "'AND present=1 AND PermitPoll = 1 )" +
                       "OR (AGMID = '" + UniqueAGMId + "'AND present=1 AND preregistered=1 AND PermitPoll = 1)";
                }
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
                model.Holding = holding;

                var setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
                if (setting != null)
                {
                    model.logo = setting.Image != null ? "data:image/jpg;base64," +
                               Convert.ToBase64String((byte[])setting.Image) : "";
                    model.Company = setting.CompanyName;
                    model.AGMTitle = setting.Title;
                    model.AGMID = UniqueAGMId;
                }
            }



            return Task.FromResult<ReportViewModelDto>(model);
        }

        public Task<ReportViewModelDto> PresentPostAsync(CompanyModel pmodel)
        {

            Double holding = 0;
            Double percentHolding = 0;
            var companyinfo = ua.GetUserCompanyInfo();
            //var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var agmEventSetting = db.SettingsArchive.SingleOrDefault(s => s.AGMID == pmodel.AGMID);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }
            ReportViewModelDto model = new ReportViewModelDto();
            var present = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.present == true && p.PermitPoll == 1).Count();
            var proxy = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.proxy == true).Count();
            var presentcount = present;
            var proxycount = proxy;
            model.present = presentcount;
            model.proxy = proxycount;
            model.AGMID = pmodel.AGMID;
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection conn =
                      new SqlConnection(connStr);
            //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
            //string query = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID = '" + pmodel.AGMID + "' AND present = 1";
            string query = string.Empty;
            if (agmEventSetting.PreregisteredVotes)
            {
                query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + pmodel.AGMID + "' AND PermitPoll = 1";
            }
            else
            {
               query = "SELECT SUM(Holding) FROM PresentArchives WHERE (AGMID = '" + pmodel.AGMID + "'AND present=1 AND  PermitPoll = 1)" +
                   "OR (AGMID = '" + pmodel.AGMID + "'AND present=1 AND preregistered=1 AND PermitPoll = 1)";
            }
           
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            //object o = cmd.ExecuteScalar();
            if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
            {
                holding = Convert.ToDouble(cmd.ExecuteScalar());
            }
            conn.Close();
            if (agmEventSetting.PreregisteredVotes)
            {
                query = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID = '" + pmodel.AGMID + "' AND PermitPoll = 1";
            }
            else
            {
                query = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE (AGMID = '" + pmodel.AGMID + "'AND present=1 AND  PermitPoll = 1)" +
                   "OR (AGMID = '" + pmodel.AGMID + "'AND present=1 AND preregistered=1 AND PermitPoll = 1)";
            }
                
            conn.Open();
            SqlCommand cmd2 = new SqlCommand(query, conn);
            //object o = cmd.ExecuteScalar();
            if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
            {
                percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
            }
            conn.Close();

            model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
            model.Holding = holding;

            var setting = db.Settings.FirstOrDefault(s => s.AGMID == pmodel.AGMID);
            if (setting != null)
            {
                model.logo = setting.Image != null ? "data:image/jpg;base64," +
                           Convert.ToBase64String((byte[])setting.Image) : "";
                model.Company = setting.CompanyName;
                model.AGMTitle = setting.Title;
               
            }

            return Task.FromResult<ReportViewModelDto>(model);
        }



        public ActionResult AjaxHandler()
        {
            try
            {
                //Creating instance of DatabaseContext class  
                using (UsersContext _context = new UsersContext())
                {
                    var companyinfo = ua.GetUserCompanyInfo();
                    var UniqueAGMId = ua.RetrieveAGMUniqueID();

                    var agmEventSetting = db.SettingsArchive.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                    if (agmEventSetting == null)
                    {
                        return Json(new { draw = string.Empty, recordsFiltered = 0, recordsTotal = 0, data = new List<PresentModel>()  }, JsonRequestBehavior.AllowGet);
                    }
                    var draw = Request.Form.GetValues("draw").FirstOrDefault();
                    var start = Request.Form.GetValues("start").FirstOrDefault();
                    var length = Request.Form.GetValues("length").FirstOrDefault();
                    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                    var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


                    //Paging Size (10,20,50,100)    
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int recordsTotal = 0;
                    var currentYear = DateTime.Now.Year.ToString();
                    List<PresentModel> allPresent = new List<PresentModel>();
                    // Getting all Shareholder data 
                    if (agmEventSetting.PreregisteredVotes)
                    {
                        allPresent = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1).ToList();
                    }
                    else
                    {
                        allPresent = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.present==true || p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.present == true && p.preregistered==true).ToList();
                    }
                  
                    IEnumerable<PresentModel> filteredpresent;

                    //Sorting  
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        filteredpresent = allPresent.OrderBy(s => s.Id);
                    }
                    //Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        filteredpresent = allPresent.ToList()
                                 .Where(c => c.Name.Contains(searchValue)

                                             ||
                                             c.ShareholderNum.ToString() == searchValue);
                    }
                    else
                    {
                        filteredpresent = allPresent;
                    }
                    //Paging
                    IEnumerable<PresentModel> displayedpresent;
                    if (pageSize == -1)
                    {
                        displayedpresent = filteredpresent;
                    }
                    else
                    {
                        displayedpresent = filteredpresent
                                                    .Skip(skip)
                                                    .Take(pageSize);
                    }

                    //var result = displayedCompanies;
                    var shareholderpresent = GetPresent(displayedpresent);
                    //var shareholderData = from c in displayedCompanies
                    //             //select c;
                    //  select new[] { Convert.ToString(c.SN),Convert.ToString(c.Id), c.Name, c.Address, c.ShareholderNum, c.Holding, c.PercentageHolding,
                    //      c.PhoneNumber, c.emailAddress,Convert.ToString(c.Present),Convert.ToString(c.PresentByProxy) };   

                    //total number of rows count     
                    recordsTotal = allPresent.Count();


                    //Returning Json Data    
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = shareholderpresent }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        private IEnumerable<PresentModel> GetPresent(IEnumerable<PresentModel> displayedpresent)
        {
            var shareholderpresent = from c in displayedpresent
                                     select new PresentModel()
                                     {

                                         Id = c.Id,
                                         Name = c.Name,
                                         Address = c.Address,
                                         Holding = c.Holding,
                                         PercentageHolding = c.PercentageHolding,
                                         ShareholderNum = c.ShareholderNum,
                                         newNumber = c.newNumber,
                                         TakePoll = c.TakePoll,
                                         present = c.present,
                                         proxy = c.proxy,
                                         split = c.split,
                                         emailAddress = c.emailAddress,
                                         PhoneNumber = c.PhoneNumber
                                     };

            return shareholderpresent.ToList();

        }

        public async Task<ActionResult> Proxy()
        {
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();

            var companyinfo = ua.GetUserCompanyInfo();
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await ProxyAsync();
            return PartialView(response);
        }

        public Task<ReportViewModelDto> ProxyAsync()
        {

            //Double TotalShareholding = 326700000;
            Double holding = 0;
            Double percentHolding = 0;
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            ReportViewModelDto model = new ReportViewModelDto();

            if (UniqueAGMId != -1)
            {
                var currentYear = DateTime.Now.Year.ToString();
                var proxy = db.Present.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).Count();
                var proxycount = proxy;
                model.present = proxycount;
                model.AGMID = UniqueAGMId;
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                string query = "SELECT SUM(Holding) FROM PresentModels where AGMID = '" + UniqueAGMId + "' AND Proxy = 1";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    holding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();

                string query1 = "SELECT SUM(PercentageHolding) FROM PresentModels where AGMID = '" + UniqueAGMId + "' AND Proxy = 1";
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query1, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
                model.Holding = holding;

            }


            return Task.FromResult<ReportViewModelDto>(model);
        }

        public async Task<ActionResult> TotalAttendees()
        {
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();

            var companyinfo = ua.GetUserCompanyInfo();


            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await TotalAttendeesAsync();

            return View(response);
        }


        [HttpPost]
        public async Task<ActionResult> TotalAttendees(CompanyModel pmodel)
        {
            //if (String.IsNullOrEmpty(pmodel.Year))
            //{
            //    return View(new ReportViewModelDto());
            //}

            var companyinfo = ua.GetUserCompanyInfo();

            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await TotalAttendeesPostAsync(pmodel);

            return PartialView(response);
        }

        public Task<ReportViewModelDto> TotalAttendeesAsync()
        {
            //Double TotalShareholding = 326700000;
            Double holding = 0;
            Double percentHolding = 0;
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }
            var currentYear = DateTime.Now.Year.ToString();

            ReportViewModelDto model = new ReportViewModelDto();
            if (UniqueAGMId != -1)
            {
                int totalAttaindees = 0;
                if (agmEventSetting.PreregisteredVotes)
                {
                     totalAttaindees = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1).Count();
                }
                else
                {
                    totalAttaindees = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.present ||
                    p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.present && p.preregistered).Count();
                }
                var totalAttaindeescount = totalAttaindees;
                model.present = totalAttaindeescount;
                model.AGMID = UniqueAGMId;
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                string query = string.Empty;
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(Holding) FROM PresentModels Where AGMID ='" + UniqueAGMId + "' AND PermitPoll = 1";
                }
                else
                {
                    query = "SELECT SUM(Holding) FROM PresentModels Where AGMID ='" + UniqueAGMId + "' AND present=1 AND PermitPoll = 1" +
                        " OR (AGMID ='" + UniqueAGMId + "' AND present=1 AND preregistered=1  AND PermitPoll = 1)";
                }
                
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    holding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels Where AGMID ='" + UniqueAGMId + "' AND PermitPoll = 1";
                }
                else
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels Where (AGMID ='" + UniqueAGMId + "' AND present=1 AND PermitPoll = 1 )" +
                          " OR (AGMID ='" + UniqueAGMId + "' AND present=1 AND preregistered=1  AND PermitPoll = 1)";
                }
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
                model.Holding = holding;
            }


            return Task.FromResult<ReportViewModelDto>(model);
            //return View(totalAttaindees);
        }


        private Task<ReportViewModelDto> TotalAttendeesPostAsync(CompanyModel pmodel)
        {

            //Double TotalShareholding = 326700000;
            Double holding = 0;
            Double percentHolding = 0;
            var companyinfo = ua.GetUserCompanyInfo();
            //var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == pmodel.AGMID);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }
            ReportViewModelDto model = new ReportViewModelDto();

            if (pmodel.AGMID != -1)
            {
                int totalAttaindees = 0;
                if (agmEventSetting.PreregisteredVotes)
                {
                    totalAttaindees = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.PermitPoll == 1).Count();
                }
                else
                {
                    totalAttaindees = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.PermitPoll == 1 && p.present == true ||
                    p.AGMID == pmodel.AGMID && p.PermitPoll == 1 && p.present == true && p.preregistered==true).Count();
                }
                var totalAttaindeescount = totalAttaindees;
                model.present = totalAttaindeescount;
                model.AGMID = pmodel.AGMID;
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                string query = string.Empty;
                if (agmEventSetting.PreregisteredVotes)
                {
                     query = "SELECT SUM(Holding) FROM PresentArchives Where AGMID = '" + pmodel.AGMID + "' AND PermitPoll = 1";
                }
                else
                {
                     query = "SELECT SUM(Holding) FROM PresentArchives Where AGMID ='" + pmodel.AGMID + "' AND present=1 AND PermitPoll = 1)" +
                         " OR (AGMID ='" + pmodel.AGMID + "' AND present=1 AND preregistered=1  AND PermitPoll = 1)";
                }
                
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    holding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentArchives Where  AGMID = '" + pmodel.AGMID + "' AND PermitPoll = 1";
                }
                else
                {
                     query = "SELECT SUM(PercentageHolding) FROM PresentArchives Where AGMID ='" + pmodel.AGMID + "' AND present=1 AND PermitPoll = 1)" +
                        " OR (AGMID ='" + pmodel.AGMID + "' AND present=1 AND preregistered=1  AND PermitPoll = 1)";
                }
                
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
                model.Holding = holding;

            }


            return Task.FromResult<ReportViewModelDto>(model);
            //return View(totalAttaindees);
        }


        public async Task<ActionResult> AttendeeSummary()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await AttendeeSummaryAsync();
            return PartialView(response);
        }

        [HttpPost]
        public async Task<ActionResult> AttendeeSummary(CompanyModel pmodel)
        {
            var companyinfo = ua.GetUserCompanyInfo();

            IEnumerable<SettingsModel> AGMDb;
            if (String.IsNullOrEmpty(pmodel.AGMID.ToString()))
            {
                AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
                ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
                ViewBag.value = "tab";
                return PartialView(new ReportViewModelDto());
            }
            AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            ViewBag.value = "tab";
            var response = await AttendeeSummaryPostAsync(pmodel);

            return PartialView(response);
        }

        public Task<ReportViewModelDto> AttendeeSummaryAsync()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }
            var currentYear = DateTime.Now.Year.ToString();

            ReportViewModelDto model = new ReportViewModelDto();

            if (UniqueAGMId != -1)
            {
                model.Year = currentYear;
                model.AGMID = UniqueAGMId;
                int presentcount = 0;
                if (agmEventSetting.PreregisteredVotes)
                {
                    presentcount = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1).Count();
                }
                else
                {
                    presentcount = db.Present.Where(p => p.AGMID == UniqueAGMId && p.present==true && p.PermitPoll == 1 || p.AGMID == UniqueAGMId && p.preregistered == true && p.PermitPoll == 1).Count();
                }
               
                //ViewBag.feedbackcount = db.ShareholderFeedback.Count();
                //model.shareholders = db.BarcodeStore.Where(b=>b.Company==companyinfo).Count();
                int TotalCount = 0;
                Double TotalHolding = 0;
                Double TotalPercentageHolding = 0;
                Double presentholding = 0;
                Double presentpercentHolding = 0;
                Double proxyholding = 0;
                Double proxypercentHolding = 0;

                //var present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.present == true && p.PermitPoll == 1).Count();
                //var presentcount = present;
                model.presentcount = presentcount;
                model.present = presentcount;
                model.AGMID = UniqueAGMId;
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                string query = string.Empty;
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1 ";
                }
                else
                {
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE ( AGMID = '" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1)" +
                        "OR ( AGMID = '" + UniqueAGMId + "' AND present = 1 AND preregistered=1 AND PermitPoll = 1) ";
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    presentholding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(PercentageHolding)  FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1";
                }
                else
                {
                    query = "SELECT SUM(PercentageHolding)  FROM PresentModels WHERE ( AGMID = '" + UniqueAGMId + "' AND present = 1 AND PermitPoll = 1)" +
                        "OR ( AGMID = '" + UniqueAGMId + "' AND present = 1 AND preregistered=1 AND PermitPoll = 1) ";
                }
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageHolding = String.Format("{0:0.####}", presentpercentHolding);
                model.Holding = presentholding;

                var proxy = db.Present.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).Count();
                var proxycount = proxy;
                model.proxy = proxycount;

                string query3 = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd3 = new SqlCommand(query3, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                {
                    proxyholding = Convert.ToDouble(cmd3.ExecuteScalar());
                }
                conn.Close();

                string query4 = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd4 = new SqlCommand(query4, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                {
                    proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
                model.ProxyHolding = proxyholding;

                TotalCount = proxycount + presentcount;
                model.TotalCount = TotalCount;

                TotalHolding = presentholding + proxyholding;
                model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0");

                TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
                model.PercentageTotalHolding = String.Format("{0:0.####}", TotalPercentageHolding);

                var setting = db.Settings.FirstOrDefault(f => f.AGMID == UniqueAGMId);
                if (setting != null)
                {
                    model.logo = setting.Image != null ? "data:image/jpg;base64," +
                               Convert.ToBase64String((byte[])setting.Image) : "";
                    model.Company = setting.CompanyName;
                    model.AGMTitle = setting.Title;
                }

            }


            return Task.FromResult<ReportViewModelDto>(model);
        }


        public Task<ReportViewModelDto> AttendeeSummaryPostAsync(CompanyModel pmodel)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            //var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == pmodel.AGMID);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }
            ReportViewModelDto model = new ReportViewModelDto();
            if (pmodel.AGMID != -1)
            {
                //model.Year = pmodel.Year.Trim();
                model.AGMID = pmodel.AGMID;
                model.presentcount = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.PermitPoll == 1).Count();
                //ViewBag.feedbackcount = db.ShareholderFeedback.Count();
                //model.shareholders = db.BarcodeStore.Where(b => b.Company == companyinfo).Count();
                int TotalCount = 0;
                Double TotalHolding = 0;
                Double TotalPercentageHolding = 0;
                Double presentholding = 0;
                Double presentpercentHolding = 0;
                Double proxyholding = 0;
                Double proxypercentHolding = 0;
                List<PresentArchive> present;
                if (agmEventSetting.PreregisteredVotes)
                {
                     present = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.present == true && p.PermitPoll == 1).ToList();
                }
                else
                {
                    present = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.present == true && p.PermitPoll == 1 || p.AGMID == pmodel.AGMID && p.present == true && p.preregistered == true && p.PermitPoll == 1).ToList();
                }
                var presentcount = present.Count();
                model.present = presentcount;
                model.AGMID = pmodel.AGMID;
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                string query = string.Empty;
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID ='" + pmodel.AGMID + "' AND present = 1 AND PermitPoll = 1";
                }
                else
                {
                    query = "SELECT SUM(Holding) FROM PresentArchives WHERE (AGMID ='" + pmodel.AGMID + "' AND present = 1 AND preregistered= 1 AND PermitPoll = 1)" +
                        "OR (AGMID ='" + pmodel.AGMID + "' AND present = 1 AND preregistered= 1 AND PermitPoll = 1)";
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    presentholding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(PercentageHolding)  FROM PresentArchives WHERE AGMID ='" + pmodel.AGMID + "' AND present = 1 AND PermitPoll = 1";
                }
                else
                {
                    query = "SELECT SUM(PercentageHolding)  FROM PresentArchives WHERE (AGMID ='" + pmodel.AGMID + "' AND present = 1 AND PermitPoll = 1)" +
                        "OR (AGMID ='" + pmodel.AGMID + "' AND present = 1 AND preregistered= 1 AND PermitPoll = 1)";
                }
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageHolding = String.Format("{0:0.####}", presentpercentHolding);
                model.Holding = presentholding;

                var proxy = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.proxy == true).ToList();
                var proxycount = proxy.Count();
                model.proxy = proxycount;

                    query = "SELECT SUM(Holding) FROM PresentArchive WHERE AGMID ='" + pmodel.AGMID + "' AND proxy = 1 ";
                conn.Open();
                SqlCommand cmd3 = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                {
                    proxyholding = Convert.ToDouble(cmd3.ExecuteScalar());
                }
                conn.Close();

                string query4 = "SELECT SUM(PercentageHolding) FROM PresentArchive WHERE AGMID ='" + pmodel.AGMID + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd4 = new SqlCommand(query4, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                {
                    proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
                model.ProxyHolding = proxyholding;

                TotalCount = proxycount + presentcount;
                model.TotalCount = TotalCount;

                TotalHolding = presentholding + proxyholding;
                model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0");

                TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
                model.PercentageTotalHolding = String.Format("{0:0.####}", TotalPercentageHolding);

                var setting = db.Settings.FirstOrDefault(f => f.AGMID == pmodel.AGMID);
                if (setting != null)
                {
                    model.logo = setting.Image != null ? "data:image/jpg;base64," +
                               Convert.ToBase64String((byte[])setting.Image) : "";
                    model.Company = setting.CompanyName;
                    model.AGMTitle = setting.Title;
                }
            }


            return Task.FromResult<ReportViewModelDto>(model);
        }



        [HttpPost]
        public async Task<ActionResult> TimeFilterAttendeeSummary(FormCollection form)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var t = form["time"];
            //ViewBag.value = "tab";
            if (!String.IsNullOrEmpty(t))
            {
                var response = await TimeFilterAttendeeSummaryAsync(form);

                return View(response);
            }
            return RedirectToAction("AttendeeSummary");
        }


        private Task<ReportViewModelDto> TimeFilterAttendeeSummaryAsync(FormCollection form)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }
            var currentYear = DateTime.Now.Year.ToString();
            ReportViewModelDto model = new ReportViewModelDto();

            if (UniqueAGMId != -1)
            {
                model.Year = currentYear;
                var t = form["time"];

                var time = DateTime.Parse(t);
                var timestamp = time.TimeOfDay;

                int TotalCount = 0;
                Double TotalHolding = 0;
                Double TotalPercentageHolding = 0;
                Double presentholding = 0;
                Double presentpercentHolding = 0;
                Double proxyholding = 0;
                Double proxypercentHolding = 0;

                List<PresentModel> present;

                if (agmEventSetting.PreregisteredVotes)
                {
                     present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1  && p.PresentTime <= time).ToList();
                }
                else
                {
                     present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.present == true && p.PresentTime <= time
                     || p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.present == true && p.preregistered == true && p.PresentTime <= time).ToList();
                }
                var presentcount = present.Count();
                model.present = presentcount;
                model.AGMID = UniqueAGMId;
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                string query = string.Empty;
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID='" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND PresentTime <= '" + time + "'";
                     
                }
                else
                {
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE (AGMID='" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND PresentTime <= '" + time + "')" +
                        "OR (AGMID='" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND preregistered=1 AND PresentTime <= '" + time + "')";
                }
               
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    presentholding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE AGMID ='" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND PresentTime <= '" + time + "'";
                }
                else
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE (AGMID ='" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND PresentTime <= '" + time + "')" +
                        "OR (AGMID ='" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND preregistered=1 AND PresentTime <= '" + time + "')";
                }
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageHolding = String.Format("{0:0.####}", presentpercentHolding);
                model.Holding = presentholding;

                var proxy = db.Present.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).ToList();
                var proxycount = proxy.Count();
                model.proxy = proxycount;

                string query3 = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID ='" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd3 = new SqlCommand(query3, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                {
                    proxyholding = Convert.ToDouble(cmd3.ExecuteScalar());
                }
                conn.Close();

                string query4 = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE AGMID ='" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd4 = new SqlCommand(query4, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                {
                    proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
                }
                conn.Close();


                model.TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
                model.ProxyHolding = proxyholding;

                TotalCount = proxycount + presentcount;
                model.TotalCount = TotalCount;

                TotalHolding = presentholding + proxyholding;
                model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0");

                TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
                model.PercentageTotalHolding = String.Format("{0:0.####}", TotalPercentageHolding);


                var setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
                if (setting != null)
                {
                    model.logo = setting.Image != null ? "data:image/jpg;base64," +
                               Convert.ToBase64String((byte[])setting.Image) : "";
                    model.Company = setting.CompanyName;
                    model.AGMTitle = setting.Title;
                }


            }

            return Task.FromResult<ReportViewModelDto>(model);
        }


        [HttpPost]
        public async Task<ActionResult> RangeFilterAttendeeSummary(FormCollection form)
        {
            var s = form["start"];
            var e = form["end"];
            //ViewBag.value = "tab";
            if (!String.IsNullOrEmpty(s) && !String.IsNullOrEmpty(e))
            {
                var response = await RangeFilterAttendeeSummaryAsync(form);

                return View(response);
            }
            return RedirectToAction("AttendeeSummary");
        }



        private Task<ReportViewModelDto> RangeFilterAttendeeSummaryAsync(FormCollection form)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            var currentYear = DateTime.Now.Year.ToString();
            ReportViewModelDto model = new ReportViewModelDto();

            var agmEventSetting = db.Settings.SingleOrDefault(d => d.AGMID == UniqueAGMId);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }

            model.Year = currentYear;
            var s = form["start"];
            var e = form["end"];

            var starttime = DateTime.Parse(s);
            var endtime = DateTime.Parse(e);
            var starttimestamp = starttime.TimeOfDay;
            var endtimestamp = endtime.TimeOfDay;

            int TotalCount = 0;
            Double TotalHolding = 0;
            Double TotalPercentageHolding = 0;
            Double presentholding = 0;
            Double presentpercentHolding = 0;
            Double proxyholding = 0;
            Double proxypercentHolding = 0;

            if (UniqueAGMId != -1)
            {
                int present = 0;
                if (agmEventSetting.PreregisteredVotes)
                {
                    present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1  && p.PresentTime >= starttime && p.PresentTime <= endtime).Count();
                }
                else
                {
                    present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.present && p.PresentTime >= starttime && p.PresentTime <= endtime ||
                    p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.present && p.preregistered && p.PresentTime >= starttime && p.PresentTime <= endtime).Count();
                }
                var presentcount = present;
                model.present = presentcount;
                model.AGMID = UniqueAGMId;
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                string query = string.Empty;
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID='" + UniqueAGMId + "' AND PermitPoll = 1  AND PresentTime >= '" + starttime + "' AND PresentTime <= '" + endtime + "'";
                }
                else
                {
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE (AGMID='" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND PresentTime >= '" + starttime + "' AND PresentTime <= '" + endtime + "')" +
                        "OR (AGMID='" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND preregistered=1 AND PresentTime >= '" + starttime + "' AND PresentTime <= '" + endtime + "')";
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    presentholding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE (AGMID='" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND PresentTime >= '" + starttime + "' AND PresentTime <= '" + endtime + "')";

                }
                else
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE (AGMID='" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND PresentTime >= '" + starttime + "' AND PresentTime <= '" + endtime + "')" +
                      "OR (AGMID='" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND preregistered=1 AND PresentTime >= '" + starttime + "' AND PresentTime <= '" + endtime + "')";
                }
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();

                model.TotalPercentageHolding = String.Format("{0:0.####}", presentpercentHolding);
                model.Holding = presentholding;

                var proxy = db.Present.Where(p => p.Company == companyinfo && p.AGMID == UniqueAGMId && p.proxy == true).ToList();
                var proxycount = proxy.Count();
                model.proxy = proxycount;
                string query3 = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID ='" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd3 = new SqlCommand(query3, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                {
                    proxyholding = Convert.ToDouble(cmd3.ExecuteScalar());
                }
                conn.Close();

                string query4 = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID ='" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd4 = new SqlCommand(query4, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                {
                    proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
                }
                conn.Close();


                model.TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
                model.ProxyHolding = proxyholding;

                TotalCount = proxycount + presentcount;
                model.TotalCount = TotalCount;


                TotalHolding = presentholding + proxyholding;
                model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0");

                TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
                model.PercentageTotalHolding = String.Format("{0:0.####}", TotalPercentageHolding);


                var setting = db.Settings.FirstOrDefault(f => f.AGMID == UniqueAGMId);
                if (setting != null)
                {
                    model.logo = setting.Image != null ? "data:image/jpg;base64," +
                               Convert.ToBase64String((byte[])setting.Image) : "";
                    model.Company = setting.CompanyName;
                    model.AGMTitle = setting.Title;
                }
            }


            return Task.FromResult<ReportViewModelDto>(model);

        }

        public async Task<ActionResult> PrintIndex()
        {
            var response = await PrintIndexAsync();

            return PartialView(response);
        }

        private Task<ReportViewModelDto> PrintIndexAsync()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            ReportViewModelDto model = new ReportViewModelDto();


            model.present = Presentcount;
            model.TotalPercentageHolding = TotalPercentagePresentHolding;
            model.Holding = PresentHolding;
            model.proxy = ProxyCount;
            model.TotalPercentageProxyHolding = TotalPercentageProxyHolding;
            model.ProxyHolding = ProxyHolding;
            model.TotalCount = TotalCountPresent_Proxy_Preregistered;
            model.TotalHolding = Convert.ToDouble(TotalHoldingPresent_Proxy_Preregistered).ToString("0,0");
            model.PercentageTotalHolding = TotalPercentagePresent_Proxy_Preregistered;
            if (UniqueAGMId != -1)
            {
                var setting = db.Settings.FirstOrDefault(f => f.AGMID == UniqueAGMId);
                if (setting != null)
                {
                    model.logo = setting.Image != null ? "data:image/jpg;base64," +
                               Convert.ToBase64String((byte[])setting.Image) : "";
                    model.Company = setting.CompanyName;
                    model.PrintTitle = setting.PrintOutTitle;
                    model.AGMVenue = setting.Venue;
                    model.AGMID = UniqueAGMId;
                    DateTime time;
                    if (setting.AgmDateTime != null)
                    {
                        time = (DateTime)setting.AgmDateTime;
                        model.AGMTime = time.ToString("dddd, dd MMMM yyyy");
                    }

                }
            }

            return Task.FromResult<ReportViewModelDto>(model);
        }

        public async Task<ActionResult> EndPrintIndex()
        {
            var response = await EndPrintIndexAsync();

            return PartialView(response);
        }

        private Task<ReportViewModelDto> EndPrintIndexAsync()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            ReportViewModelDto model = new ReportViewModelDto();
            model.present = Presentcount;
            model.TotalPercentageHolding = TotalPercentagePresentHolding;
            model.Holding = PresentHolding;
            model.proxy = ProxyCount;
            model.TotalPercentageProxyHolding = TotalPercentageProxyHolding;
            model.ProxyHolding = ProxyHolding;
            model.TotalCount = TotalCountPresent_Proxy_Preregistered;
            model.TotalHolding = Convert.ToDouble(TotalHoldingPresent_Proxy_Preregistered).ToString("0,0");
            model.PercentageTotalHolding = TotalPercentagePresent_Proxy_Preregistered;

            if (UniqueAGMId != -1)
            {
                var setting = db.Settings.FirstOrDefault(f => f.AGMID == UniqueAGMId);
                if (setting != null)
                {
                    model.logo = setting.Image != null ? "data:image/jpg;base64," +
                               Convert.ToBase64String((byte[])setting.Image) : "";
                    model.Company = setting.CompanyName;
                    model.PrintTitle = setting.PrintOutTitle;
                    model.AGMVenue = setting.Venue;
                    model.AGMID = UniqueAGMId;
                    DateTime time;
                    if (setting.AgmDateTime != null)
                    {
                        time = (DateTime)setting.AgmDateTime;
                        model.AGMTime = time.ToString("dddd, dd MMMM yyyy");
                    }


                }
            }

            return Task.FromResult<ReportViewModelDto>(model);
        }

        [HttpPost]
        public async Task<ActionResult> TimeIndex(FormCollection form)
        {
            var t = form["time"];

            if (!String.IsNullOrEmpty(t))
            {
                var response = await TimeIndexAsync(form);
                return RedirectToAction("PrintIndex");
            }
            return RedirectToAction("PrintIndex");
        }



        private Task<ReportViewModelDto> TimeIndexAsync(FormCollection form)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            var agmEventSetting = db.Settings.SingleOrDefault(d => d.AGMID == UniqueAGMId);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }
            ReportViewModelDto model = new ReportViewModelDto();
            var t = form["time"];
            var Year = form["Year"];
            var time = DateTime.Parse(t);
            var timestamp = time.TimeOfDay;
            int TotalCount = 0;
            Double TotalHolding = 0;
            Double TotalPercentageHolding = 0;
            Double presentholding = 0;
            Double presentpercentHolding = 0;
            Double proxyholding = 0;
            Double proxypercentHolding = 0;

            if (UniqueAGMId != -1)
            {
                List<PresentModel> present ;
                if (agmEventSetting.PreregisteredVotes)
                {
                    present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1  && p.PresentTime <= time).ToList();
                }
                else
                {
                    present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.present == true && p.PresentTime <= time|| p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.present == true  && p.preregistered == true && p.PresentTime <= time).ToList();
                }

                var presentcount = present.Count();
                Presentcount = presentcount;
                model.AGMID = UniqueAGMId;
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                string query = string.Empty;
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1  AND PresentTime <= '" + time + "'";
                }
                else
                {
                    query = "SELECT SUM(Holding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1 AND PresentTime <= '" + time + "')" +
                        "OR (AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND preregistered=1 AND PresentTime <= '" + time + "')";
                }
                
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    presentholding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
  
                if (agmEventSetting.PreregisteredVotes)
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1  AND PresentTime <= '" + time + "'";

                }
                else
                {
                    query = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1 AND PresentTime <= '" + time + "')" +
                        "OR (AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND preregistered=1 AND PresentTime <= '" + time + "')";

                }

                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();

                TotalPercentagePresentHolding = String.Format("{0:0.####}", presentpercentHolding);
                PresentHolding = presentholding;

                var proxy = db.Present.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).ToList();
                var proxycount = proxy.Count();
                ProxyCount = proxycount;
                string query3 = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd3 = new SqlCommand(query3, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                {
                    proxyholding = Convert.ToDouble(cmd3.ExecuteScalar());
                }
                conn.Close();

                string query4 = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd4 = new SqlCommand(query4, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                {
                    proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
                }
                conn.Close();

                TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
                ProxyHolding = proxyholding;

                TotalCount = proxycount + presentcount;
                TotalCountPresent_Proxy_Preregistered = TotalCount;

                TotalHolding = presentholding + proxyholding;
                TotalHoldingPresent_Proxy_Preregistered= TotalHolding;

                TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
                TotalPercentagePresent_Proxy_Preregistered = String.Format("{0:0.####}", TotalPercentageHolding);

            }

            return Task.FromResult<ReportViewModelDto>(model);

        }


        [HttpPost]
        public async Task<ActionResult> EndTimeIndex(FormCollection form)
        {
            var t = form["time"];
            if (!String.IsNullOrEmpty(t))
            {
                var response = await EndTimeIndexAsync(form);

                return RedirectToAction("EndPrintIndex");
            }

            return RedirectToAction("EndPrintIndex");
        }



        private Task<ReportViewModelDto> EndTimeIndexAsync(FormCollection form)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            var agmEventSetting = db.Settings.SingleOrDefault(d => d.AGMID == UniqueAGMId);
            if (agmEventSetting == null)
            {
                return Task.FromResult(new ReportViewModelDto());
            }
            ReportViewModelDto model = new ReportViewModelDto();
            var t = form["time"];
            var Year = form["Year"];
            var time = DateTime.Parse(t);
            var timestamp = time.TimeOfDay;

            int TotalCount = 0;
            Double TotalHolding = 0;
            Double TotalPercentageHolding = 0;
            Double presentholding = 0;
            Double presentpercentHolding = 0;
            Double proxyholding = 0;
            Double proxypercentHolding = 0;

            if (UniqueAGMId != -1)
            {
                List<PresentModel> present;
                if (agmEventSetting.PreregisteredVotes)
                {
                     present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1  && p.PresentTime <= time).ToList();
                }
                else
                {
                    present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.present == true && p.PresentTime <= time ||
                    p.AGMID == UniqueAGMId && p.PermitPoll == 1 && p.present == true && p.preregistered == true && p.PresentTime <= time).ToList();
                }
                

                var presentcount = present.Count();
                Presentcount = presentcount;
                model.AGMID = UniqueAGMId;
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                          new SqlConnection(connStr);
                string query = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1 AND  present=1 AND PresentTime <= '" + time + "'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    presentholding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();

                string query1 = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1 AND present=1 AND PresentTime <= '" + time + "'";
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query1, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();


                TotalPercentagePresentHolding = String.Format("{0:0.####}", presentpercentHolding);
                PresentHolding = presentholding;

                var proxy = db.Present.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).ToList();
                var proxycount = proxy.Count();
                ProxyCount = proxycount;
                string query3 = "SELECT SUM(Holding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd3 = new SqlCommand(query3, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                {
                    proxyholding = Convert.ToDouble(cmd3.ExecuteScalar());
                }
                conn.Close();

                string query4 = "SELECT SUM(PercentageHolding) FROM PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND proxy = 1";
                conn.Open();
                SqlCommand cmd4 = new SqlCommand(query4, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                {
                    proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
                }
                conn.Close();

                TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
                ProxyHolding = proxyholding;

                TotalCount = proxycount + presentcount;
                TotalCountPresent_Proxy_Preregistered = TotalCount;

                TotalHolding = presentholding + proxyholding;
                TotalHoldingPresent_Proxy_Preregistered = TotalHolding;

                TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
                TotalPercentagePresent_Proxy_Preregistered = String.Format("{0:0.####}", TotalPercentageHolding);

            }


            return Task.FromResult<ReportViewModelDto>(model);

        }


        public async Task<ActionResult> Resolution(int id)
        {
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            var response = await ResolutionAsync(id);

            return PartialView(response);
        }


        public Task<ReportViewModelDto> ResolutionAsync(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            ReportViewModelDto model = new ReportViewModelDto();
            Double holding = 0;
            Double percentHolding = 0;
            Double holdingAgainst = 0;
            Double percentHoldingAgainst = 0;
            Double holdingAbstain = 0;
            Double percentHoldingAbstain = 0;
            Double holdingVoid = 0;
            Double percentHoldingVoid = 0;
            Double TotalHolding = 0;
            Double TotalPercentHolding = 0;
            var resolution = db.Question.Find(id);
            model.Id = resolution.Id;
            model.resolutionName = resolution.question;

            if (UniqueAGMId != -1)
            {
                var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

                var abstainbtnchoice = true;
                var forBg = "green";
                var againstBg = "red";
                var abstainBg = "blue";
                var voidBg = "black";
                if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
                {
                    abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
                    if (!string.IsNullOrEmpty(agmEventSetting.VoteForColorBg))
                    {
                        forBg = agmEventSetting.VoteForColorBg;
                    }
                    if (!string.IsNullOrEmpty(agmEventSetting.VoteAgainstColorBg))
                    {
                        againstBg = agmEventSetting.VoteAgainstColorBg;
                    }
                    if (!string.IsNullOrEmpty(agmEventSetting.VoteAbstaincolorBg))
                    {
                        abstainBg = agmEventSetting.VoteAbstaincolorBg;
                    }
                    if (!string.IsNullOrEmpty(agmEventSetting.VoteVoidColorBg))
                    {
                        voidBg = agmEventSetting.VoteVoidColorBg;
                    }
                    model.AGMID = UniqueAGMId;
                }
                model.abstainBtnChoice = abstainbtnchoice;
                model.forBg = forBg;
                model.againstBg = againstBg;
                model.abstainBg = abstainBg;
                model.voidBg = voidBg;
              
                //ResolutionResult resolutionResult = new ResolutionResult
                //{
                model.ResultFor = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteFor == true).ToList();
                model.ResultAgainst = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAgainst == true).ToList();
                model.ResultAbstain = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAbstain == true).ToList();
                model.ResultVoid = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteVoid == true).ToList();
                //};

                var ResultForCount = model.ResultFor.Count();
                var ResultAgainstCount = model.ResultAgainst.Count();
                var ResultAbstainCount = model.ResultAbstain.Count();
                var ResultVoidCount = model.ResultVoid.Count();
                int resultCount = 0;
                if (abstainbtnchoice)
                {
                    resultCount = resolution.result.Where(r => r.AGMID == UniqueAGMId).Count();
                }
                else
                {
                    resultCount = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAbstain == false).Count();
                }

                model.ResultForCount = ResultForCount;
                model.ResultAgainstCount = ResultAgainstCount;
                model.ResultAbstainCount = ResultAbstainCount;
                model.ResultVoidCount = ResultVoidCount;
                model.TotalCount = resultCount;

                SqlConnection conn =
                new SqlConnection(connStr);
                //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                string query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND [VoteFor] = 1";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    holding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
                string query1 = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + resolution.Id + "' AND [VoteFor] = 1";
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query1, conn);
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();

                model.ResultForHolding = holding.ToString();
                model.ResultForPercentHolding = percentHolding.ToString();

                //            SqlConnection conn =
                //new SqlConnection(connStr);
                //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                string query3 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteAgainst = 1";
                conn.Open();
                SqlCommand cmd3 = new SqlCommand(query3, conn);

                if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                {
                    holdingAgainst = Convert.ToDouble(cmd3.ExecuteScalar());
                }
                conn.Close();
                string query4 = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + resolution.Id + "' AND VoteAgainst = 1";
                conn.Open();
                SqlCommand cmd4 = new SqlCommand(query4, conn);
                if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                {
                    percentHoldingAgainst = Convert.ToDouble(cmd4.ExecuteScalar());
                }
                conn.Close();

                model.ResultForHoldingAgainst = holdingAgainst.ToString();
                model.ResultForPercentHoldingAgainst = percentHoldingAgainst.ToString();
                if (abstainbtnchoice)
                {


                    string query5 = "SELECT SUM(Holding) FROM Results WHERE   QuestionId='" + resolution.Id + "' AND VoteAbstain = 1";
                    conn.Open();
                    SqlCommand cmd5 = new SqlCommand(query5, conn);

                    if (!DBNull.Value.Equals(cmd5.ExecuteScalar()))
                    {
                        holdingAbstain = Convert.ToDouble(cmd5.ExecuteScalar());
                    }
                    conn.Close();
                    string query6 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteAbstain = 1";
                    conn.Open();
                    SqlCommand cmd6 = new SqlCommand(query6, conn);
                    if (!DBNull.Value.Equals(cmd6.ExecuteScalar()))
                    {
                        percentHoldingAbstain = Convert.ToDouble(cmd6.ExecuteScalar());
                    }
                    conn.Close();

                    model.ResultForHoldingAbstain = holdingAbstain.ToString();
                    model.ResultForPercentHoldingAbstain = percentHoldingAbstain.ToString();
                }

                string query8 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteVoid = 1";
                conn.Open();
                SqlCommand cmd8 = new SqlCommand(query8, conn);

                if (!DBNull.Value.Equals(cmd8.ExecuteScalar()))
                {
                    holdingVoid = Convert.ToDouble(cmd8.ExecuteScalar());
                }
                conn.Close();
                string query9 = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + resolution.Id + "' AND VoteVoid = 1";
                conn.Open();
                SqlCommand cmd9 = new SqlCommand(query9, conn);
                if (!DBNull.Value.Equals(cmd9.ExecuteScalar()))
                {
                    percentHoldingVoid = Convert.ToDouble(cmd9.ExecuteScalar());
                }
                conn.Close();

                model.ResultForHoldingVoid = holdingVoid.ToString();
                model.ResultForPercentHoldingAgainst = percentHoldingVoid.ToString();

                if (abstainbtnchoice)
                {
                    string query7 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "'";
                    conn.Open();
                    SqlCommand cmd7 = new SqlCommand(query7, conn);
                    if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
                    {
                        TotalHolding = Convert.ToDouble(cmd7.ExecuteScalar());
                    }
                    conn.Close();
                }
                else
                {
                    string query7 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + resolution.Id + "' AND VoteAbstain != 1";
                    conn.Open();
                    SqlCommand cmd7 = new SqlCommand(query7, conn);
                    if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
                    {
                        TotalHolding = Convert.ToDouble(cmd7.ExecuteScalar());
                    }
                    conn.Close();
                }


                model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0");


                //calcuate percentage holding per resolution..
                var pFor = (holding / TotalHolding) * 100;
                model.PercentageResultFor = pFor.ToString();
                var pAgainst = (holdingAgainst / TotalHolding) * 100;
                model.PercentageResultAgainst = pAgainst.ToString();
                double pAbstain = 0;
                if (abstainbtnchoice)
                {
                    pAbstain = (holdingAbstain / TotalHolding) * 100;
                    model.PercentageResultAbstain = pAbstain.ToString();
                }
                var pVoid = (holdingVoid / TotalHolding) * 100;
                model.PercentageResultVoid = pVoid.ToString();
                var sumAllPercentages = pFor + pAgainst + pAbstain + pVoid;
                model.TotalPercentageHolding = sumAllPercentages.ToString();
            }
            return Task.FromResult<ReportViewModelDto>(model);
        }



        public async Task<JsonResult> Notify(int id)
        {

            var response = await NotifyAsync(id);

            return Json(response, JsonRequestBehavior.AllowGet);

        }

        private Task<List<ChartResult>> NotifyAsync(int id)
        {
            List<ChartResult> countitem = new List<ChartResult>();
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            var resolution = db.Question.Find(id);
            if (UniqueAGMId != -1)
            {
                ChartResult chartresult = new ChartResult
                {
                    ForCount = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteFor == true).Count(),
                    AgainstCount = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAgainst == true).Count(),
                    AbstainCount = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAbstain == true).Count()
                };

                countitem.Add(chartresult);

            }


            return Task.FromResult<List<ChartResult>>(countitem);
        }


        public async Task<ActionResult> ResolutionIndex()
        {
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            var returnUrl = HttpContext.Request.Url.AbsolutePath;
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            if (String.IsNullOrEmpty(companyinfo))
            {
                return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
            }
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await ResolutionIndexAsync();

            return PartialView(response);
        }



        [HttpPost]
        public async Task<ActionResult> ResolutionIndex(CompanyModel pmodel)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            IEnumerable<SettingsModel> AGMDb;
            if (String.IsNullOrEmpty(pmodel.AGMID.ToString()))
            {
                AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
                ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
                return View(new ReportViewModelDto());
            }
            AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            ViewBag.value = "tab";

            var response = await ResolutionIndexPostAsync(pmodel);

            return PartialView(response);
        }


        public Task<ReportViewModelDto> ResolutionIndexAsync()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();


            var currentYear = DateTime.Now.Year.ToString();

            ReportViewModelDto model = new ReportViewModelDto();
            if (UniqueAGMId != -1)
            {
                model.resolutions = db.Question.Any() ? db.Question.Where(q => q.AGMID == UniqueAGMId).ToList() : new List<Question>();
                model.presentcount = db.Present.Any() ? db.Present.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll == 1).Count() : 0;
            }

            //model.shareholders = db.BarcodeStore.Where(b => b.Company == companyinfo).Count();

            return Task.FromResult<ReportViewModelDto>(model);
        }

        public Task<ReportViewModelDto> ResolutionIndexPostAsync(CompanyModel pmodel)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            //var UniqueAGMId = ua.RetrieveAGMUniqueID();

            ReportViewModelDto model = new ReportViewModelDto();
            if (pmodel.AGMID != -1)
            {
                model.resolutionsArchive = db.QuestionArchive.Any() ? db.QuestionArchive.Where(q => q.AGMID == pmodel.AGMID).ToList() : new List<QuestionArchive>();
                model.presentcount = db.PresentArchive.Any() ? db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.PermitPoll == 1).Count() : 0;
            }

            model.shareholders = db.BarcodeStore.Any() ? db.BarcodeStore.Where(b => b.Company == companyinfo).Count() : 0;

            return Task.FromResult<ReportViewModelDto>(model);
        }




        public async Task<ActionResult> ResolutionList(int id)
        {
            var response = await ResolutionListAsync(id);

            return PartialView(response);
        }




        public Task<ReportViewModelDto> ResolutionListAsync(int id)
        {
            Double holding = 0;
            Double percentHolding = 0;
            Double holdingAgainst = 0;
            Double percentHoldingAgainst = 0;
            Double holdingAbstain = 0;
            Double percentHoldingAbstain = 0;
            Double holdingAll = 0;
            Double percentHoldingAll = 0;
            Double holdingVoid = 0;
            Double percentHoldingVoid = 0;
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            ReportViewModelDto model = new ReportViewModelDto();
            //Double TotalHolding = 0;
            //Double TotalPercentHolding = 0;
            var question = db.Question.Find(id);

            model.ResolutionName = question.question;
            if (UniqueAGMId != -1)
            {
                var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

                var abstainbtnchoice = true;
                if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
                {
                    abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
                }
                model.abstainBtnChoice = abstainbtnchoice;
                model.ResultAll = question.result.Where(r => r.AGMID == UniqueAGMId).ToList();
                model.ResultFor = question.result.Where(r => r.AGMID == UniqueAGMId && r.VoteFor == true).ToList();
                model.ResultAgainst = question.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAgainst == true).ToList();
                model.ResultAbstain = question.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAbstain == true).ToList();
                model.ResultVoid = question.result.Where(r => r.AGMID == UniqueAGMId && r.VoteVoid == true).ToList();
                //model.resolutions = db.Question.Where(q => q.Company == companyinfo).ToList();
                model.AGMID = UniqueAGMId;

                var ResultForCount = model.ResultFor.Count();
                var ResultAgainstCount = model.ResultAgainst.Count();
                var ResultAbstainCount = model.ResultAbstain.Count();
                model.ResultForCount = ResultForCount;
                model.ResultAgainstCount = ResultAgainstCount;
                model.ResultAbstainCount = ResultAbstainCount;
                model.ResultAllCount = model.ResultAll.Count();
                model.ResultVoidCount = model.ResultVoid.Count();


                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                new SqlConnection(connStr);
                //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                string query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND [VoteFor] = 1";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    holding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
                string query1 = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + id + "' AND [VoteFor] = 1";
                conn.Open();
                SqlCommand cmd1 = new SqlCommand(query1, conn);
                if (!DBNull.Value.Equals(cmd1.ExecuteScalar()))
                {
                    percentHolding = Convert.ToDouble(cmd1.ExecuteScalar());
                }
                conn.Close();

                model.ResultForHolding = holding.ToString();
                model.ResultForPercentHolding = percentHolding.ToString();


                string query2 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND VoteAgainst = 1";
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    holdingAgainst = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();
                string query3 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "' AND VoteAgainst = 1";
                conn.Open();
                SqlCommand cmd3 = new SqlCommand(query3, conn);
                if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                {
                    percentHoldingAgainst = Convert.ToDouble(cmd3.ExecuteScalar());
                }
                conn.Close();

                model.ResultForHoldingAgainst = holdingAgainst.ToString();
                model.ResultForPercentHoldingAgainst = percentHoldingAgainst.ToString();
                if (abstainbtnchoice)
                {
                    string query4 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND VoteAbstain = 1";
                    conn.Open();
                    SqlCommand cmd4 = new SqlCommand(query4, conn);
                    if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                    {
                        holdingAbstain = Convert.ToDouble(cmd4.ExecuteScalar());
                    }
                    conn.Close();
                    string query5 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "' AND VoteAbstain = 1";
                    conn.Open();
                    SqlCommand cmd5 = new SqlCommand(query5, conn);
                    if (!DBNull.Value.Equals(cmd5.ExecuteScalar()))
                    {
                        percentHoldingAbstain = Convert.ToDouble(cmd5.ExecuteScalar());
                    }
                    conn.Close();
                    model.ResultForHoldingAbstain = holdingAbstain.ToString();
                    model.ResultForPercentHoldingAbstain = percentHoldingAbstain.ToString();
                }
                string query6 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "'";
                conn.Open();
                SqlCommand cmd6 = new SqlCommand(query6, conn);
                if (!DBNull.Value.Equals(cmd6.ExecuteScalar()))
                {
                    holdingAll = Convert.ToDouble(cmd6.ExecuteScalar());
                }
                conn.Close();
                string query7 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "'";
                conn.Open();
                SqlCommand cmd7 = new SqlCommand(query7, conn);
                if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
                {
                    percentHoldingAll = Convert.ToDouble(cmd7.ExecuteScalar());
                }
                conn.Close();
                model.ResultForHoldingAll = holdingAll.ToString();
                model.ResultForPercentHoldingAll = percentHoldingAll.ToString();

                string query8 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND VoteVoid = 1";
                conn.Open();
                SqlCommand cmd8 = new SqlCommand(query8, conn);
                if (!DBNull.Value.Equals(cmd8.ExecuteScalar()))
                {
                    holdingVoid = Convert.ToDouble(cmd8.ExecuteScalar());
                }
                conn.Close();
                string query9 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "' AND VoteVoid = 1";
                conn.Open();
                SqlCommand cmd9 = new SqlCommand(query9, conn);
                if (!DBNull.Value.Equals(cmd9.ExecuteScalar()))
                {
                    percentHoldingVoid = Convert.ToDouble(cmd9.ExecuteScalar());
                }
                conn.Close();
                model.ResultForHoldingVoid = holdingVoid.ToString();
                model.ResultForPercentHoldingVoid = percentHoldingVoid.ToString();

            }


            return Task.FromResult<ReportViewModelDto>(model);
        }







        public Task<List<ReportViewModelDto>> ReportDetailsAsync()
        {
            Double holding = 0;
            Double percentHolding = 0;
            Double holdingAgainst = 0;
            Double percentHoldingAgainst = 0;
            Double holdingAbstain = 0;
            Double percentHoldingAbstain = 0;
            Double holdingAll = 0;
            Double percentHoldingAll = 0;
            Double holdingVoid = 0;
            Double percentHoldingVoid = 0;
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            List<ReportViewModelDto> ReportDetailsList = new List<ReportViewModelDto>();
            if (UniqueAGMId != -1)
            {
                return Task.FromResult<List<ReportViewModelDto>>(ReportDetailsList);
            }

            var resolutionList = db.Question.Where(r => r.AGMID == UniqueAGMId).ToList();
            if (resolutionList != null)
            {
                foreach (var resolution in resolutionList)
                {
                    ReportViewModelDto model = new ReportViewModelDto();
                    //Double TotalHolding = 0;
                    //Double TotalPercentHolding = 0;
                    //var question = db.Question.Find(id);

                    model.ResolutionName = resolution.question;
                    int id = resolution.Id;

                    var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

                    var abstainbtnchoice = true;
                    if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
                    {
                        abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
                    }
                    model.abstainBtnChoice = abstainbtnchoice;
                    model.ResultAll = resolution.result.Where(r => r.AGMID == UniqueAGMId).ToList();
                    model.ResultFor = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteFor == true).ToList();
                    model.ResultAgainst = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAgainst == true).ToList();
                    model.ResultAbstain = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAbstain == true).ToList();
                    model.ResultVoid = resolution.result.Where(r => r.AGMID == UniqueAGMId && r.VoteVoid == true).ToList();
                    //model.resolutions = db.Question.Where(q => q.Company == companyinfo).ToList();
                    model.AGMID = UniqueAGMId;

                    var ResultForCount = model.ResultFor.Count();
                    var ResultAgainstCount = model.ResultAgainst.Count();
                    var ResultAbstainCount = model.ResultAbstain.Count();
                    var ResultVoidCount = model.ResultVoid.Count();
                    model.ResultForCount = ResultForCount;
                    model.ResultAgainstCount = ResultAgainstCount;
                    model.ResultAbstainCount = ResultAbstainCount;
                    model.ResultAllCount = model.ResultAll.Count();
                    model.ResultVoidCount = model.ResultVoid.Count();
                    model.Decision = am.GetAGMDecision(ResultForCount, ResultAgainstCount, ResultAbstainCount, ResultVoidCount);

                    string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    SqlConnection conn =
                    new SqlConnection(connStr);
                    //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                    string query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND [VoteFor] = 1";
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    //object o = cmd.ExecuteScalar();
                    if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                    {
                        holding = Convert.ToDouble(cmd.ExecuteScalar());
                    }
                    conn.Close();
                    string query1 = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + id + "' AND [VoteFor] = 1";
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    if (!DBNull.Value.Equals(cmd1.ExecuteScalar()))
                    {
                        percentHolding = Convert.ToDouble(cmd1.ExecuteScalar());
                    }
                    conn.Close();

                    model.ResultForHolding = holding.ToString();
                    model.ResultForPercentHolding = percentHolding.ToString();


                    string query2 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND VoteAgainst = 1";
                    conn.Open();
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                    {
                        holdingAgainst = Convert.ToDouble(cmd2.ExecuteScalar());
                    }
                    conn.Close();
                    string query3 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "' AND VoteAgainst = 1";
                    conn.Open();
                    SqlCommand cmd3 = new SqlCommand(query3, conn);
                    if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                    {
                        percentHoldingAgainst = Convert.ToDouble(cmd3.ExecuteScalar());
                    }
                    conn.Close();

                    model.ResultForHoldingAgainst = holdingAgainst.ToString();
                    model.ResultForPercentHoldingAgainst = percentHoldingAgainst.ToString();
                    if (abstainbtnchoice)
                    {
                        string query4 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND VoteAbstain = 1";
                        conn.Open();
                        SqlCommand cmd4 = new SqlCommand(query4, conn);
                        if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                        {
                            holdingAbstain = Convert.ToDouble(cmd4.ExecuteScalar());
                        }
                        conn.Close();
                        string query5 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "' AND VoteAbstain = 1";
                        conn.Open();
                        SqlCommand cmd5 = new SqlCommand(query5, conn);
                        if (!DBNull.Value.Equals(cmd5.ExecuteScalar()))
                        {
                            percentHoldingAbstain = Convert.ToDouble(cmd5.ExecuteScalar());
                        }
                        conn.Close();
                        model.ResultForHoldingAbstain = holdingAbstain.ToString();
                        model.ResultForPercentHoldingAbstain = percentHoldingAbstain.ToString();
                    }
                    string query6 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "'";
                    conn.Open();
                    SqlCommand cmd6 = new SqlCommand(query6, conn);
                    if (!DBNull.Value.Equals(cmd6.ExecuteScalar()))
                    {
                        holdingAll = Convert.ToDouble(cmd6.ExecuteScalar());
                    }
                    conn.Close();
                    string query7 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "'";
                    conn.Open();
                    SqlCommand cmd7 = new SqlCommand(query7, conn);
                    if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
                    {
                        percentHoldingAll = Convert.ToDouble(cmd7.ExecuteScalar());
                    }
                    conn.Close();
                    model.ResultForHoldingAll = holdingAll.ToString();
                    model.ResultForPercentHoldingAll = percentHoldingAll.ToString();

                    string query8 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND VoteVoid = 1";
                    conn.Open();
                    SqlCommand cmd8 = new SqlCommand(query8, conn);
                    if (!DBNull.Value.Equals(cmd8.ExecuteScalar()))
                    {
                        holdingVoid = Convert.ToDouble(cmd8.ExecuteScalar());
                    }
                    conn.Close();
                    string query9 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "' AND VoteVoid = 1";
                    conn.Open();
                    SqlCommand cmd9 = new SqlCommand(query9, conn);
                    if (!DBNull.Value.Equals(cmd9.ExecuteScalar()))
                    {
                        percentHoldingVoid = Convert.ToDouble(cmd9.ExecuteScalar());
                    }
                    conn.Close();
                    model.ResultForHoldingVoid = holdingVoid.ToString();
                    model.ResultForPercentHoldingVoid = percentHoldingVoid.ToString();

                    ReportDetailsList.Add(model);

                }

            }


            return Task.FromResult<List<ReportViewModelDto>>(ReportDetailsList);
        }

        public async Task<ActionResult> MobileIndex()
        {

            var returnUrl = HttpContext.Request.Url.AbsolutePath;
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            var companyinfo = ua.GetUserCompanyInfo();
            if (String.IsNullOrEmpty(companyinfo))
            {
                return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
            }
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await ChannelIndexAsync();

            return PartialView(response);
        }

        public async Task<ActionResult> SMSIndex()
        {
            var returnUrl = HttpContext.Request.Url.AbsolutePath;
            var companyinfo = ua.GetUserCompanyInfo();
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            if (String.IsNullOrEmpty(companyinfo))
            {
                return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
            }

            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await ChannelIndexAsync();

            return PartialView(response);
        }

        public async Task<ActionResult> ClikapadIndex()
        {
            var returnUrl = HttpContext.Request.Url.AbsolutePath;
            var companyinfo = ua.GetUserCompanyInfo();
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            if (String.IsNullOrEmpty(companyinfo))
            {
                return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
            }
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await ChannelIndexAsync();

            return PartialView(response);
        }

        public async Task<ActionResult> ProxyIndex()
        {
            var returnUrl = HttpContext.Request.Url.AbsolutePath;
            var companyinfo = ua.GetUserCompanyInfo();
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            if (String.IsNullOrEmpty(companyinfo))
            {
                return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
            }
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await ChannelIndexAsync();

            return PartialView(response);
        }

        public async Task<ActionResult> WebIndex()
        {
            var returnUrl = HttpContext.Request.Url.AbsolutePath;
            var companyinfo = ua.GetUserCompanyInfo();
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();
            if (String.IsNullOrEmpty(companyinfo))
            {
                return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
            }
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await ChannelIndexAsync();

            return PartialView(response);
        }


        public async Task<ActionResult> AllChannelsIndex()
        {
            var returnUrl = HttpContext.Request.Url.AbsolutePath;
            string returnvalue = "";
            if (HttpContext.Request.QueryString.Count > 0)
            {
                returnvalue = HttpContext.Request.QueryString["rel"].ToString();
            }
            ViewBag.value = returnvalue.Trim();

            var companyinfo = ua.GetUserCompanyInfo();
            if (String.IsNullOrEmpty(companyinfo))
            {
                return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
            }
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());
            var response = await ChannelIndexAsync();
            return PartialView(response);
        }

        [HttpPost]
        public async Task<ActionResult> AllChannelsIndex(CompanyModel pmodel)
        {

            var companyinfo = ua.GetUserCompanyInfo();
            var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
            //if (pmodel.AGMID == 0)
            //{

            //    ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            //    return View(new ReportViewModelDto());
            //}

            ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
            ViewBag.value = "tab";
            //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
            //ViewBag.Year = new SelectList(companies ?? new List<string>());

            var response = await ChannelIndexPostAsync(pmodel);
            return PartialView(response);
        }

        private Task<ReportViewModelDto> ChannelIndexAsync()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();

            var currentYear = DateTime.Now.Year.ToString();
            ReportViewModelDto model = new ReportViewModelDto();
            if (UniqueAGMId != -1)
            {
                model.resolutions = db.Question.Where(p => p.AGMID == UniqueAGMId).ToList();
            }
            model.User = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
            //model.presentcount = db.Present.Where(p => p.Company == companyinfo).Count();
            //model.shareholders = db.BarcodeStore.Where(p => p.Company == companyinfo).Count();

            return Task.FromResult<ReportViewModelDto>(model);
        }


        private Task<ReportViewModelDto> ChannelIndexPostAsync(CompanyModel pmodel)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            ReportViewModelDto model = new ReportViewModelDto();
            if (string.IsNullOrEmpty(pmodel.AGMID.ToString()))
            {
                model.resolutionsArchive = db.QuestionArchive.Where(p => p.AGMID == pmodel.AGMID).ToList();
            }
            model.User = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
            //model.presentcount = db.Present.Where(p => p.Company == companyinfo).Count();
            //model.shareholders = db.BarcodeStore.Where(p => p.Company == companyinfo).Count();

            return Task.FromResult<ReportViewModelDto>(model);
        }





        public async Task<ActionResult> MobileList(int id)
        {
            string channel = "Mobile";
            var response = await ChannelListAsync(id, channel);


            return PartialView(response);
        }
        public async Task<ActionResult> WebList(int id)
        {
            string channel = "Web";
            var response = await ChannelListAsync(id, channel);


            return PartialView(response);
        }

        public async Task<ActionResult> SMSList(int id)
        {
            string channel = "SMS";
            var response = await ChannelListAsync(id, channel);

            return PartialView(response);
        }

        public async Task<ActionResult> ClikapadList(int id)
        {
            string channel = "Pad";
            var response = await ChannelListAsync(id, channel);

            return PartialView(response);
        }

        public async Task<ActionResult> ProxyList(int id)
        {
            string channel = "Proxy";
            var response = await ChannelListAsync(id, channel);

            return PartialView(response);
        }

        public async Task<ActionResult> AllChannelsList(int id)
        {
            string channel = "All";
            var response = await ChannelListAsync(id, channel);

            return PartialView(response);
        }

        private Task<ReportViewModelDto> ChannelListAsync(int id, string channel)
        {
            Double holding = 0;
            Double percentHolding = 0;
            Double holdingAgainst = 0;
            Double percentHoldingAgainst = 0;
            Double holdingAbstain = 0;
            Double percentHoldingAbstain = 0;
            Double percentHoldingAll = 0;
            Double holdingAll = 0;
            Double percentHoldingVoid = 0;
            Double holdingVoid = 0;
            //var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            ReportViewModelDto model = new ReportViewModelDto();
            var question = db.Question.Find(id);
            ViewBag.ResolutionName = question.question;
            if (UniqueAGMId != -1)
            {
                var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                model.AGMID = UniqueAGMId;
                var abstainbtnchoice = true;
                if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
                {
                    abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
                }
                model.abstainBtnChoice = abstainbtnchoice;
                if (channel != "All")
                {
                    model.ResultAll = question.result.Where(r => r.AGMID == UniqueAGMId && r.Source == channel).ToList();
                    model.ResultFor = question.result.Where(r => r.AGMID == UniqueAGMId && r.Source == channel && r.VoteFor == true).ToList();
                    model.ResultAgainst = question.result.Where(r => r.AGMID == UniqueAGMId && r.Source == channel && r.VoteAgainst == true).ToList();
                    model.ResultVoid = question.result.Where(r => r.AGMID == UniqueAGMId && r.Source == channel && r.VoteVoid == true).ToList();
                    if (abstainbtnchoice)
                    {
                        model.ResultAbstain = question.result.Where(r => r.AGMID == UniqueAGMId && r.Source == channel && r.VoteAbstain == true).ToList();
                    }

                    //model.resolutions = db.Question.Where(q => q.Company == companyinfo).ToList();
                }
                else
                {
                    model.ResultAll = question.result.Where(r => r.AGMID == UniqueAGMId).ToList();
                    model.ResultFor = question.result.Where(r => r.AGMID == UniqueAGMId && r.VoteFor == true).ToList();
                    model.ResultAgainst = question.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAgainst == true).ToList();
                    model.ResultVoid = question.result.Where(r => r.AGMID == UniqueAGMId && r.VoteVoid == true).ToList();
                    if (abstainbtnchoice)
                    {
                        model.ResultAbstain = question.result.Where(r => r.AGMID == UniqueAGMId && r.VoteAbstain == true).ToList();
                        var ResultAbstainCount = model.ResultAbstain.Count();
                        model.ResultAbstainCount = ResultAbstainCount;
                    }

                    //model.resolutions = db.Question.Where(q => q.Company == companyinfo).ToList();
                }


                var ResultAllCount = model.ResultAll.Count();
                var ResultForCount = model.ResultFor.Count();
                var ResultAgainstCount = model.ResultAgainst.Count();
                var ResultVoidCount = model.ResultVoid.Count();

                model.ResultForCount = ResultForCount;
                model.ResultAgainstCount = ResultAgainstCount;
                model.ResultAllCount = ResultAllCount;
                model.ResultVoidCount = ResultVoidCount;

                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection conn =
                new SqlConnection(connStr);
                //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
                string query;
                if (channel != "All")
                {
                    query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND Source='" + channel + "' AND [VoteFor] = 1";
                }
                else
                {
                    query = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND [VoteFor] = 1";
                }

                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                //object o = cmd.ExecuteScalar();
                if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
                {
                    holding = Convert.ToDouble(cmd.ExecuteScalar());
                }
                conn.Close();
                string query1;
                if (channel != "All")
                {
                    query1 = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + id + "'AND Source='" + channel + "' AND [VoteFor] = 1";
                }
                else
                {
                    query1 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "' AND [VoteFor] = 1";
                }

                conn.Open();
                SqlCommand cmd1 = new SqlCommand(query1, conn);
                if (!DBNull.Value.Equals(cmd1.ExecuteScalar()))
                {
                    percentHolding = Convert.ToDouble(cmd1.ExecuteScalar());
                }
                conn.Close();

                model.ResultForHolding = holding.ToString();
                model.ResultForPercentHolding = percentHolding.ToString();

                string query2;
                if (channel != "All")
                {
                    query2 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND Source='" + channel + "' AND VoteAgainst = 1";
                }
                else
                {
                    query2 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND  VoteAgainst = 1";
                }
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
                {
                    holdingAgainst = Convert.ToDouble(cmd2.ExecuteScalar());
                }
                conn.Close();
                string query3;
                if (channel != "All")
                {
                    query3 = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + id + "' AND Source='" + channel + "' AND VoteAgainst = 1";
                }
                else
                {
                    query3 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "' AND  VoteAgainst = 1";
                }

                conn.Open();
                SqlCommand cmd3 = new SqlCommand(query3, conn);
                if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
                {
                    percentHoldingAgainst = Convert.ToDouble(cmd3.ExecuteScalar());
                }
                conn.Close();

                model.ResultForHoldingAgainst = holdingAgainst.ToString();
                model.ResultForPercentHoldingAgainst = percentHoldingAgainst.ToString();

                if (abstainbtnchoice)
                {
                    string query4;
                    if (channel != "All")
                    {
                        query4 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND Source='" + channel + "' AND VoteAbstain = 1";
                    }
                    else
                    {
                        query4 = "SELECT SUM(Holding) FROM Results WHERE  QuestionId='" + id + "' AND  VoteAbstain = 1";
                    }

                    conn.Open();
                    SqlCommand cmd4 = new SqlCommand(query4, conn);
                    if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
                    {
                        holdingAbstain = Convert.ToDouble(cmd4.ExecuteScalar());
                    }
                    conn.Close();

                    string query5;
                    if (channel != "All")
                    {
                        query5 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "' AND Source='" + channel + "' AND VoteAbstain = 1";
                    }
                    else
                    {
                        query5 = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + id + "' AND VoteAbstain = 1";
                    }
                    conn.Open();
                    SqlCommand cmd5 = new SqlCommand(query5, conn);
                    if (!DBNull.Value.Equals(cmd5.ExecuteScalar()))
                    {
                        percentHoldingAbstain = Convert.ToDouble(cmd5.ExecuteScalar());
                    }
                    conn.Close();
                    model.ResultForHoldingAbstain = holdingAbstain.ToString();
                    model.ResultForPercentHoldingAbstain = percentHoldingAbstain.ToString();
                }
                string query6;
                if (channel != "All")
                {
                    query6 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND Source='" + channel + "'";
                }
                else
                {
                    query6 = "SELECT SUM(Holding) FROM Results WHERE  QuestionId='" + id + "' ";
                }

                conn.Open();
                SqlCommand cmd6 = new SqlCommand(query6, conn);
                if (!DBNull.Value.Equals(cmd6.ExecuteScalar()))
                {
                    holdingAll = Convert.ToDouble(cmd6.ExecuteScalar());
                }
                conn.Close();
                string query7;
                if (channel != "All")
                {
                    query7 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "' AND Source='" + channel + "'";
                }
                else
                {
                    query7 = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + id + "' ";
                }
                conn.Open();
                SqlCommand cmd7 = new SqlCommand(query7, conn);
                if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
                {
                    percentHoldingAll = Convert.ToDouble(cmd7.ExecuteScalar());
                }
                conn.Close();
                model.ResultForHoldingAll = holdingAll.ToString();
                model.ResultForPercentHoldingAll = percentHoldingAll.ToString();

                string query8;
                if (channel != "All")
                {
                    query8 = "SELECT SUM(Holding) FROM Results WHERE QuestionId='" + id + "' AND Source='" + channel + "' AND VoteVoid = 1";
                }
                else
                {
                    query8 = "SELECT SUM(Holding) FROM Results WHERE  QuestionId='" + id + "' AND  VoteVoid = 1";
                }

                conn.Open();
                SqlCommand cmd8 = new SqlCommand(query8, conn);
                if (!DBNull.Value.Equals(cmd8.ExecuteScalar()))
                {
                    holdingVoid = Convert.ToDouble(cmd8.ExecuteScalar());
                }
                conn.Close();

                string query9;
                if (channel != "All")
                {
                    query9 = "SELECT SUM(PercentageHolding) FROM Results WHERE QuestionId='" + id + "' AND Source='" + channel + "' AND VoteVoid = 1";
                }
                else
                {
                    query9 = "SELECT SUM(PercentageHolding) FROM Results WHERE  QuestionId='" + id + "' AND VoteVoid = 1";
                }
                conn.Open();
                SqlCommand cmd9 = new SqlCommand(query9, conn);
                if (!DBNull.Value.Equals(cmd9.ExecuteScalar()))
                {
                    percentHoldingVoid = Convert.ToDouble(cmd9.ExecuteScalar());
                }
                conn.Close();
                model.ResultForHoldingVoid = holdingVoid.ToString();
                model.ResultForPercentHoldingVoid = percentHoldingVoid.ToString();

            }


            return Task.FromResult<ReportViewModelDto>(model);
        }



        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Report/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Report/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Report/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Report/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Report/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Report/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult ExportPdfSharp()
        {
            /*
             Links: 
             https://www.nuget.org/packages/HtmlRenderer.PdfSharp/
             https://stackoverflow.com/questions/564650/convert-html-to-pdf-in-net/31944740#31944740
             https://docs.telerik.com/devtools/winforms/api/html/m_theartofdev_htmlrenderer_core_cssdata_addcssblock.htm
             */

            var viewInString = RenderToString(PartialView("ReportDetails"));
            var viewInBiteArray = PdfSharpConvert(viewInString);

            // Send the PDF file to browser
            FileResult fileResult = new FileContentResult(viewInBiteArray, "application/pdf");
            fileResult.FileDownloadName = "testeExportPdf.pdf";

            return fileResult;
        }

        private Byte[] PdfSharpConvert(String html)
        {
            Byte[] res = null;
            using (var ms = new MemoryStream())
            {
                var config = new PdfGenerateConfig()
                {
                    MarginBottom = 70,
                    MarginLeft = 20,
                    MarginRight = 20,
                    MarginTop = 70,
                };

                /*CssData cssData;
                var headerStyle = new Dictionary<string, string>();
                headerStyle.Add("position", "fixed");
                headerStyle.Add("left", "20px");
                headerStyle.Add("top", "20px");

                var footerStyle = new Dictionary<string, string>();
                footerStyle.Add("position", "fixed");
                footerStyle.Add("left", "20px");
                footerStyle.Add("top", "750");

                CssBlock blockHeader = new CssBlock("header", headerStyle);
                CssBlock blockFooter = new CssBlock("header", footerStyle);

                cssData.AddCssBlock("header", blockHeader);
                cssData.AddCssBlock("footer", blockFooter);*/

                var stylesUrl = Server.MapPath("~/Assets/PdfSharp/style.css");

                CssData css = PdfGenerator.ParseStyleSheet(System.IO.File.ReadAllText(stylesUrl));

                var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4, cssData: css);
                pdf.Save(ms);
                res = ms.ToArray();
            }
            return res;
        }

        private string RenderToString(PartialViewResult partialView)
        {
            var httpContext = System.Web.HttpContext.Current;

            if (httpContext == null)
                throw new NotSupportedException("An HTTP context is required to render the partial view to a string");

            var controllerName = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

            var controller = (ControllerBase)ControllerBuilder.Current.GetControllerFactory().CreateController(httpContext.Request.RequestContext, controllerName);
            var controllerContext = new ControllerContext(httpContext.Request.RequestContext, controller);
            var view = ViewEngines.Engines.FindPartialView(controllerContext, partialView.ViewName).View;

            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using (var tw = new HtmlTextWriter(sw))
                    view.Render(new ViewContext(controllerContext, view, partialView.ViewData, partialView.TempData, tw), tw);
            }

            return sb.ToString();
        }

    }
}
