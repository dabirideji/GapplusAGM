// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;
// using BarcodeGenerator.Util;
// using System;
// using System.Collections.Generic;
// using System.Configuration;
// using System.Data.Common;
// using System.Data.SqlClient;
// using System.Linq;
// using System.Threading.Tasks;
// using System.Web.Mvc;

// namespace BarcodeGenerator.Controllers
// {
//     public class ReportArchiveController : Controller
//     {
//         // GET: ReportArchive
//         // GET: /Report/

//         UsersContext db = new UsersContext();
//         UserAdmin ua = new UserAdmin();
//         //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

//         //private static int RetrieveAGMUniqueID()
//         //{
//         //    UsersContext adb = new UsersContext();
//         //    var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

//         //    return AGMID;
//         //}

//         //private static int UniqueAGMId = RetrieveAGMUniqueID();

//         private  int Presentcount;
//         private int _AGMID;
//         private  double PresentHolding;
//         private  string TotalPercentagePresentHolding;
//         private  int ProxyCount;
//         private  double ProxyHolding;
//         private  string TotalPercentageProxyHolding;
//         private  int TotalCountPresent_Proxy;
//         private  double TotalHoldingPresent_Proxy;
//         private  string TotalPercentagePresent_Proxy;



//         public async Task<ActionResult> Index()
//         {
//             var returnUrl = HttpContext.Request.Url.AbsolutePath;
//             //var companyinfo = ua.GetUserCompanyInfo();

//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             //if (String.IsNullOrEmpty(companyinfo))
//             //{
//             //    return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
//             //}          
//             var companies = db.SettingsArchive.Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList(); ;
//             ViewBag.AGMHistory = new SelectList(companies ?? new List<string>());
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var response = await IndexAsync();

//             return PartialView(response);
//         }


//         [HttpPost]
//         public async Task<ActionResult> Index(CompanyModel pmodel)
//         {
//             var returnUrl = HttpContext.Request.Url.AbsolutePath;
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             //if (String.IsNullOrEmpty(pmodel.Year))
//             //{
//             //    return View(new ReportViewModelDto());
//             //}
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             var companies = db.SettingsArchive.Where(s=>s.ArchiveStatus==true).Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList(); ;
//             ViewBag.AGMHistory = new SelectList(companies ?? new List<string>());
//             ViewBag.value = "tab";
//             var response = await IndexPostAsync(pmodel);


//             return PartialView(response);
//         }

//         private Task<ReportViewModelDto> IndexAsync()
//         {
//             ReportViewModelDto model = new ReportViewModelDto();
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             //var currentYear = DateTime.Now.Year.ToString();
//             //if (UniqueAGMId != -1)
//             //{
//             //    model.presentcount = db.Present.Where(p => p.AGMID == UniqueAGMId).Count();
//             //    //model.shareholders = db.BarcodeStore.Where(p => p.Company == companyinfo).Count();
//             model.resolutionsArchive = new List<QuestionArchive>();

//             //    var setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
//             //    if (setting != null)
//             //    {
//             //        model.logo = setting.Image != null ? "data:image/jpg;base64," +
//             //                   Convert.ToBase64String((byte[])setting.Image) : "";
//             //        model.Company = setting.CompanyName;
//             //        model.AGMTitle = setting.Title;
//             //    }
//             //}


//             return Task.FromResult<ReportViewModelDto>(model);
//         }

//         private Task<ReportViewModelDto> IndexPostAsync(CompanyModel pmodel)
//         {
//             ReportViewModelDto model = new ReportViewModelDto();
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             if (!string.IsNullOrEmpty(pmodel.AGMID.ToString()))
//             {
//                 var agmEventSetting = db.SettingsArchive.SingleOrDefault(s => s.AGMID == pmodel.AGMID);

//                 var abstainbtnchoice = true;
//                 var forBg = "green";
//                 var againstBg = "red";
//                 var abstainBg = "blue";
//                 var voidBg = "black";
//                 if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
//                 {
//                     abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
//                     if(!string.IsNullOrEmpty(agmEventSetting.VoteForColorBg))
//                     {
//                         forBg = agmEventSetting.VoteForColorBg;
//                     }
//                     if (!string.IsNullOrEmpty(agmEventSetting.VoteAgainstColorBg))
//                     {
//                         againstBg = agmEventSetting.VoteAgainstColorBg;
//                     }
//                     if (!string.IsNullOrEmpty(agmEventSetting.VoteAbstaincolorBg))
//                     {
//                         abstainBg = agmEventSetting.VoteAbstaincolorBg;
//                     }
//                     if (!string.IsNullOrEmpty(agmEventSetting.VoteVoidColorBg))
//                     {
//                         voidBg = agmEventSetting.VoteVoidColorBg;
//                     }

//                     //model.AGMID = agmEventSetting.AGMID;
//                 }
//                 model.abstainBtnChoice = abstainbtnchoice;
//                 model.forBg = forBg;
//                 model.againstBg = againstBg;
//                 model.abstainBg = abstainBg;
//                 model.voidBg = voidBg;
//                 model.AGMID = pmodel.AGMID;
//                 model.presentcount = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.PermitPoll==1).Count();
//                 //model.shareholders = db.BarcodeStore.Where(p => p.Company == companyinfo).Count();
//                 model.resolutionsArchive = db.QuestionArchive.Where(p => p.AGMID == pmodel.AGMID).ToList();

//                 //var setting = db.Settings.FirstOrDefault(s => s.AGMID == pmodel.AGMID);
//                 if (agmEventSetting != null)
//                 {
//                     model.logo = agmEventSetting.Image != null ? "data:image/jpg;base64," +
//                                Convert.ToBase64String((byte[])agmEventSetting.Image) : "";
//                     model.Company = agmEventSetting.CompanyName;
//                     model.AGMTitle = agmEventSetting.Title;
//                 }
//             }

//             return System.Threading.Tasks.Task.FromResult<ReportViewModelDto>(model);
//         }

//         [HttpPost]
//         public ActionResult GetAGMID(CompanyModel pmodel)
//         {
//             return Json(db.SettingsArchive.Where(s=>s.CompanyName==pmodel.CompanyInfo).Select(x => new
//             {
//                 AGMID = x.AGMID,
//                 Title = x.Title
//             }).ToList(), JsonRequestBehavior.AllowGet);
//         }

//         public async Task<string> ArchiveAGMData(int id)
//         {
//             var response = await ArchiveAGMDataAsync(id);

//             return response;
//         }
        
//         private Task<string> ArchiveAGMDataAsync(int id)
//         {
//             var setting = db.Settings.Find(id);
//             var UniqueAGMId = setting.AGMID;
//             var companyinfo = setting.CompanyName.Trim();

//             string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//             SqlConnection conn =
//                       new SqlConnection(connStr);
//             try
//             {
//                 if(setting.ArchiveStatus)
//                 {
//                    return Task.FromResult<string>("AGM has been Archived");
//                 }
//                 Functions.RetrieveProgress("Moving Present Table to Archive..");

//                 string query = "INSERT INTO PresentArchives ([AGMID],[Name],[Holding],[Company],[Address],[PercentageHolding],[ShareholderNum],[newNumber],[ParentNumber],[TakePoll],[split],[present],[proxy],[emailAddress],[PhoneNumber],[Clikapad],[GivenClikapad],[ReturnedClikapad],[PresentTime],[Year],[Timestamp],[admitSource],[PermitPoll]) SELECT" +
//                     "[AGMID],[Name],[Holding],[Company],[Address],[PercentageHolding],[ShareholderNum],[newNumber],[ParentNumber],[TakePoll],[split],[present],[proxy],[emailAddress],[PhoneNumber],[Clikapad],[GivenClikapad],[ReturnedClikapad],[PresentTime],[Year],[Timestamp],[admitSource],[PermitPoll] FROM PresentModels WHERE AGMID ='" + UniqueAGMId + "'";
//                 ////string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//                 //string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + UniqueAGMId + "' AND present = 1";
//                 conn.Open();
//                 SqlCommand cmd = new SqlCommand(query, conn);

//                 cmd.ExecuteNonQuery(); 

//                 Functions.RetrieveProgress("Removing values From Present Table..");

//                 string query2 = "DELETE FROM PresentModels WHERE AGMID='"+UniqueAGMId+"'";
//                 SqlCommand cmd2 = new SqlCommand(query2, conn);
//                 cmd2.ExecuteNonQuery();

//                 Functions.RetrieveProgress("Moving Resolution Table to Archive..");
//                 string query3 = "INSERT INTO QuestionArchives ([AGMID],[question],[Year],[Company],[date],[questionStatus],[syncStatus],[QuestionId]) SELECT" +
//                     "[AGMID],[question],[Year],[Company],[date],[questionStatus],[syncStatus],[Id] FROM Questions WHERE AGMID='" +UniqueAGMId+"'";
//                 ////string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//                 //string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + UniqueAGMId + "' AND present = 1"
//                 SqlCommand cmd3 = new SqlCommand(query3, conn);
//                 cmd3.ExecuteNonQuery();

//                 Functions.RetrieveProgress("Moving Results Table to Archive..");

//                 string query4 = "INSERT INTO ResultArchives ([AGMID],[Name],[EmailAddress],[Company],[Year],[Holding],[Address],[splitValue],[ParentNumber],[PercentageHolding],[phonenumber],[Clickapad],[VoteStatus],[Source],[date],[Timestamp],[Present],[PresentByProxy],[QuestionId],[ShareholderNum],[VoteFor],[VoteAgainst],[VoteAbstain],[VoteVoid]) SELECT " +
//                     "[AGMID],[Name],[EmailAddress],[Company],[Year],[Holding],[Address],[splitValue],[ParentNumber],[PercentageHolding],[phonenumber],[Clickapad],[VoteStatus],[Source],[date],[Timestamp],[Present],[PresentByProxy],[QuestionId],[ShareholderNum],[VoteFor],[VoteAgainst],[VoteAbstain],[VoteVoid] FROM Results WHERE AGMID='" + UniqueAGMId + "'";

//                 SqlCommand cmd4 = new SqlCommand(query4, conn);
//                 cmd4.ExecuteNonQuery();

//                 Functions.RetrieveProgress("Moving Non shareholders Table to Archive..");

//                 string query5 = "INSERT INTO FacilitatorsArchives ([SN],[Company],[Name],[FacilitatorCompany],[AGMID],[ResourceType],[BarcodeImage],[Barcode],[ImageUrl],[OnlineEventUrl],[emailAddress],[PhoneNumber],[accesscode],[Date]) SELECT " +
//                     "[SN],[Company],[Name],[FacilitatorCompany],[AGMID],[ResourceType],[BarcodeImage],[Barcode],[ImageUrl],[OnlineEventUrl],[emailAddress],[PhoneNumber],[accesscode],[Date] FROM Facilitators WHERE AGMID='" + UniqueAGMId + "'";

//                 SqlCommand cmd5 = new SqlCommand(query5, conn);
//                 cmd5.ExecuteNonQuery();

//                 Functions.RetrieveProgress("Removing values From Non Shareholders Table..");

//                 string query6 = "DELETE FROM Facilitators WHERE AGMID='" + UniqueAGMId + "'";
//                 SqlCommand cmd6 = new SqlCommand(query6, conn);
//                 cmd6.ExecuteNonQuery();



//                 Functions.RetrieveProgress("Removing values from Result Table..");
//                 //string query4 = "TRUNCATE TABLE Results";
//                 string query7 = "DELETE FROM Results WHERE AGMID='" + UniqueAGMId + "'";
//                 //string query4 = "Delete FROM Results where AGMID ='" + UniqueAGMId + "'";
//                 SqlCommand cmd7 = new SqlCommand(query7, conn);
//                 cmd7.ExecuteNonQuery();

//                 Functions.RetrieveProgress("Removing values from Resolution Table..");
//                 //string query7 = "ALTER TABLE [dbo].[SMSResults] DROP CONSTRAINT [FK_dbo.SMSResults_dbo.Questions_Question_Id]";
//                 ////conn.Open();
//                 //SqlCommand cmd7 = new SqlCommand(query7, conn);
//                 //cmd7.ExecuteNonQuery();

//                 //string query8 = "ALTER TABLE [dbo].[Results] DROP CONSTRAINT [FK_dbo.Results_dbo.Questions_QuestionId]";
//                 ////conn.Open();
//                 //SqlCommand cmd8 = new SqlCommand(query8, conn);
//                 //cmd8.ExecuteNonQuery();

//                 //string query9 = "ALTER TABLE [dbo].[SMSDeliveryLogs] DROP CONSTRAINT [FK_dbo.SMSDeliveryLogs_dbo.Questions_Question_Id]";
//                 ////conn.Open();
//                 //SqlCommand cmd9 = new SqlCommand(query9, conn);
//                 //cmd9.ExecuteNonQuery();

//                 string query8 = "DELETE FROM Questions WHERE AGMID='" + UniqueAGMId + "'";
//                 SqlCommand cmd8 = new SqlCommand(query8, conn);
//                 cmd8.ExecuteNonQuery();

//                 //string query11 = "ALTER TABLE [dbo].[SMSResults] ADD CONSTRAINT [FK_dbo.SMSResults_dbo.Questions_Question_Id] FOREIGN KEY ([Question_Id]) REFERENCES [dbo].[Questions] ([Id])";
//                 ////conn.Open();
//                 //SqlCommand cmd11 = new SqlCommand(query11, conn);
//                 //cmd11.ExecuteNonQuery();

//                 //string query12 = "ALTER TABLE [dbo].[Results] ADD CONSTRAINT [FK_dbo.Results_dbo.Questions_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Questions] ([Id])";
//                 ////conn.Open();
//                 //SqlCommand cmd12 = new SqlCommand(query12, conn);
//                 //cmd12.ExecuteNonQuery();

//                 //string query13 = "ALTER TABLE [dbo].[SMSDeliveryLogs] ADD CONSTRAINT [FK_dbo.SMSDeliveryLogs_dbo.Questions_Question_Id] FOREIGN KEY ([Question_Id]) REFERENCES [dbo].[Questions] ([Id])";
//                 ////conn.Open();
//                 //SqlCommand cmd13 = new SqlCommand(query13, conn);
//                 //cmd13.ExecuteNonQuery();

//                 conn.Close();


//                 //Change the AGM setting from Active to Archived

//                 setting.ArchiveStatus = true;
//                 db.SaveChanges();


//                 Functions.RetrieveProgress("Moving Settings Table to Archive..");

//                 string querysettingsarchive = "INSERT INTO [dbo].[SettingsModelArchives] ([Title],[ShareHolding],[CompanyName],[Year],[DateCreated],[PrintOutTitle],[Location],[When],[feebackEmailAddress],[feebackCCEmailAddress],[ImageSource],[Image],[Description],[AGMID],[RegCode],[TotalRecordCount],[ArchiveStatus],[Venue],[Address],[AgmStart],[AgmDateTime],[OnlineUrllink],[SyncChoice],[AbstainBtnChoice],[AgmEnd],[proxyChannel],[smsChannel],[webChannel],[mobileChannel],[ussdChannel],[allChannels],[StopAdmittance],[AgmEndDateTime],[VoteForColorBg],[VoteAgainstColorBg],[VoteAbstaincolorBg],[VoteVoidColorBg]) SELECT " +
//                     "[Title],[ShareHolding],[CompanyName],[Year],[DateCreated],[PrintOutTitle],[Location],[When],[feebackEmailAddress],[feebackCCEmailAddress],[ImageSource],[Image],[Description],[AGMID],[RegCode],[TotalRecordCount],[ArchiveStatus],[Venue],[Address],[AgmStart],[AgmDateTime],[OnlineUrllink],[SyncChoice],[AbstainBtnChoice],[AgmEnd],[proxyChannel],[smsChannel],[webChannel],[mobileChannel],[ussdChannel],[allChannels],[StopAdmittance],[AgmEndDateTime],[VoteForColorBg],[VoteAgainstColorBg],[VoteAbstaincolorBg],[VoteVoidColorBg] FROM [dbo].[SettingsModels] WHERE AGMID='" + UniqueAGMId + "'";

//                 conn.Open();

//                 SqlCommand cmdqsa = new SqlCommand(querysettingsarchive, conn);
                
//                 cmdqsa.ExecuteNonQuery();


//                 Functions.RetrieveProgress("Removing values From Settings Table ..");

//                 string querydeletesettingsarchive = "DELETE FROM [dbo].[SettingsModels] WHERE AGMID='" + UniqueAGMId + "'";
                
//                 SqlCommand cmddqsa = new SqlCommand(querydeletesettingsarchive, conn);
//                 cmddqsa.ExecuteNonQuery();



//                 Functions.RetrieveProgress("Removing values From Proxylist Table ..");

//                 string querydeletesproxylist = "DELETE FROM [dbo].[Proxylists] WHERE Company='" + companyinfo + "'";

//                 SqlCommand cmdpl = new SqlCommand(querydeletesproxylist, conn);
//                 cmdpl.ExecuteNonQuery();


//                 Functions.RetrieveProgress("Removing Raw record entries...");

//                 string querydeleterecords = "DELETE FROM BarcodeModels WHERE Company='" + companyinfo + "'";
               
//                 SqlCommand cmddeleterecords = new SqlCommand(querydeleterecords, conn);
//                 cmddeleterecords.CommandTimeout = 175;
//                 cmddeleterecords.ExecuteNonQuery();

//                 conn.Close();
//                 //using (UsersContext context = new UsersContext())
//                 //{
//                 //    var allsettings = context.Settings.Where(s => s.AGMID == UniqueAGMId);
//                 //    foreach(var item in allsettings)
//                 //    {
//                 //        item.ArchiveStatus = true;
//                 //    }
//                 //    context.SaveChanges();
//                 //}

//                 Functions.RetrieveProgress(String.Format("Archiving Task completed @{0}", DateTime.Now.ToString("h:mm:ss")));
//                 return Task.FromResult<string>("success");
//             }
//             catch (DbException e)
//             {

//                 conn.Close();
//                 return Task.FromResult<string>("Error"+" "+ e);
//             }


//         }

       
//         [HttpPost]
//         public async Task<ActionResult> PresentAnalysisIndex(CompanyModel pmodel)
//         {
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             var response = await PresentAnalysisIndexAsync(pmodel);

//             return PartialView(response);
//         }

//         public Task<ReportViewModelDto> PresentAnalysisIndexAsync(CompanyModel pmodel)
//         {
//             Double holdingPresent = 0;
//             Double percentHoldingPresent = 0;
//             Double holdingProxy = 0;
//             Double percentHoldingProxy = 0;
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var currentYear = DateTime.Now.Year.ToString();
//             ReportViewModelDto model = new ReportViewModelDto();

//             if (pmodel.AGMID != -1)
//             {
//                 var present = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.present == true && p.PermitPoll== 1).Count();
//                 var presentcount = present;

//                 model.presentcount = presentcount;
//                 var proxy = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.proxy == true).ToList();
//                 var proxycount = proxy.Count();
//                 model.proxycount = proxycount;
//                 string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                 SqlConnection conn =
//                           new SqlConnection(connStr);
//                 //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//                 string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + pmodel.AGMID + "' AND present = 1 AND [PermitPoll] = 1";
//                 conn.Open();
//                 SqlCommand cmd = new SqlCommand(query, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                 {
//                     holdingPresent = Convert.ToDouble(cmd.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query1 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID = '" + pmodel.AGMID + "' AND present= 1 AND [PermitPoll] = 1";
//                 conn.Open();
//                 SqlCommand cmd2 = new SqlCommand(query1, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                 {
//                     percentHoldingPresent = Convert.ToDouble(cmd2.ExecuteScalar());
//                 }
//                 conn.Close();

//                 model.TotalPercentageHoldingPresent = String.Format("{0:0.##}", percentHoldingPresent);
//                 model.HoldingPresent = holdingPresent;

//                 string query3 = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + pmodel.AGMID + "' AND proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd3 = new SqlCommand(query3, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
//                 {
//                     holdingProxy = Convert.ToDouble(cmd3.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query4 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID = '" + pmodel.AGMID + "' AND proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd4 = new SqlCommand(query4, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
//                 {
//                     percentHoldingProxy = Convert.ToDouble(cmd4.ExecuteScalar());
//                 }
//                 conn.Close();

//                 model.TotalPercentageHoldingProxy = String.Format("{0:0.##}", percentHoldingProxy);
//                 model.HoldingProxy = holdingProxy;
//                 //ViewBag.feedbackcount = db.ShareholderFeedback.Count();
//                 //model.shareholders = db.BarcodeStore.Where(b=>b.Company==companyinfo).Count();

//             }


//             return Task.FromResult<ReportViewModelDto>(model);
//         }

//         public async Task<ActionResult> Voted()
//         {
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             var response = await VotedAsync();
//             return PartialView(response);
//         }


//         public Task<ReportViewModelDto> VotedAsync()
//         {

//             //Double TotalShareholding = 326700000;
//             Double holding = 0;
//             Double percentHolding = 0;

//             ReportViewModelDto model = new ReportViewModelDto();
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             var currentYear = DateTime.Now.Year.ToString();

//             if (UniqueAGMId != -1)
//             {
//                 var voted = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId && p.TakePoll == true && p.PermitPoll== 1).ToList();
//                 model.VotedArchive = voted;
//                 var votedcount = voted.Count();
//                 model.Votes = voted.Count();

//                 string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                 SqlConnection conn =
//                           new SqlConnection(connStr);
//                 //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//                 string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1 AND TakePoll = 1";
//                 conn.Open();
//                 SqlCommand cmd = new SqlCommand(query, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                 {
//                     holding = Convert.ToDouble(cmd.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query1 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID = '" + UniqueAGMId + "' AND PermitPoll = 1 AND TakePoll= 1";
//                 conn.Open();
//                 SqlCommand cmd2 = new SqlCommand(query1, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                 {
//                     percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//                 }
//                 conn.Close();


//                 model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
//                 model.Holding = holding;

//             }



//             return Task.FromResult<ReportViewModelDto>(model);
//         }


//         public async Task<ActionResult> VotedPerQuestion(int id)
//         {
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             var response = await VotedPerQuestionAsync(id);

//             return View(response);
//         }

//         public Task<ReportViewModelDto> VotedPerQuestionAsync(int id)
//         {

//             Double holding = 0;
//             Double percentHolding = 0;
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             var question = db.QuestionArchive.Find(id);
//             ReportViewModelDto model = new ReportViewModelDto();
//             model.Question = question.question;
//             var resultPerQuestion = db.ResultArchive.Where(p => p.QuestionId == question.QuestionId).ToList();
//             model.ResultArchive = resultPerQuestion;
//             var resultPerQuestionCount = resultPerQuestion.Count();
//             model.Votes = resultPerQuestionCount;

//             string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//             SqlConnection conn =
//                       new SqlConnection(connStr);
//             //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//             string query = "SELECT SUM(Holding) FROM ResultsArchive WHERE QuestionId='" + question.QuestionId + "'";
//             conn.Open();
//             SqlCommand cmd = new SqlCommand(query, conn);
//             //object o = cmd.ExecuteScalar();
//             if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//             {
//                 holding = Convert.ToDouble(cmd.ExecuteScalar());
//             }
//             conn.Close();

//             string query1 = "SELECT SUM(PercentageHolding) FROM ResultsArchive WHERE QuestionId='" + question.QuestionId + "'";
//             conn.Open();
//             SqlCommand cmd2 = new SqlCommand(query1, conn);
//             //object o = cmd.ExecuteScalar();
//             if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//             {
//                 percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//             }
//             conn.Close();

//             model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
//             model.Holding = holding;


//             return Task.FromResult<ReportViewModelDto>(model);
//         }

//         public async Task<ActionResult> Present()
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             var companies = db.SettingsArchive.Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList(); ;
//             ViewBag.AGMHistory = new SelectList(companies ?? new List<string>());
           
//             //var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
//             //ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var response = await PresentAsync();

//             return PartialView(response);
//         }

//         //[HttpPost]
//         public async Task<ActionResult> Resolution(int id, CompanyModel pmodel)
//         {
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             var response = await ResolutionAsync(id, pmodel);

//             return PartialView(response);
//         }

//         public Task<ReportViewModelDto> ResolutionAsync(int id, CompanyModel pmodel)
//         {
//             string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             ReportViewModelDto model = new ReportViewModelDto();
//             Double holding = 0;
//             Double percentHolding = 0;
//             Double holdingAgainst = 0;
//             Double percentHoldingAgainst = 0;
//             Double holdingAbstain = 0;
//             Double percentHoldingAbstain = 0;
//             Double TotalHolding = 0;
//             Double TotalPercentHolding = 0;
//             Double holdingVoid = 0;
//             Double percentHoldingVoid = 0;
//             var resolution = db.QuestionArchive.Find(id);
//             model.Id = resolution.Id;
//             model.resolutionName = resolution.question;

//             if (pmodel.AGMID != -1)
//             {
//                 var agmEventSetting = db.SettingsArchive.SingleOrDefault(s => s.AGMID == pmodel.AGMID);

//                 var abstainbtnchoice = true;
//                 var forBg = "green";
//                 var againstBg = "red";
//                 var abstainBg = "blue";
//                 var voidBg = "black";
//                 if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
//                 {
//                     abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
//                     forBg = agmEventSetting.VoteForColorBg;
//                     againstBg = agmEventSetting.VoteAgainstColorBg;
//                     abstainBg = agmEventSetting.VoteAbstaincolorBg;
//                     voidBg = agmEventSetting.VoteVoidColorBg;
//                 }
//                 model.abstainBtnChoice = abstainbtnchoice;
//                 model.forBg = forBg;
//                 model.againstBg = againstBg;
//                 model.abstainBg = abstainBg;
//                 model.voidBg = voidBg;
//                 //ResolutionResult resolutionResult = new ResolutionResult
//                 //{
//                 model.ResultArchiveFor = db.ResultArchive.Where(r =>r.QuestionId == resolution.QuestionId && r.VoteFor == true).ToList();
//                 model.ResultArchiveAgainst = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId && r.VoteAgainst == true).ToList();
//                 if(abstainbtnchoice)
//                 {
//                     model.ResultArchiveAbstain = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId && r.VoteAbstain == true).ToList(); 
                    
//                 }              
//               //};

//                 var ResultForCount = model.ResultArchiveFor.Count();
//                 var ResultAgainstCount = model.ResultArchiveAgainst.Count();
//                 int ResultAbstainCount = 0;
//                 int resultCount = 0;
//                 if (abstainbtnchoice)
//                 {
//                     ResultAbstainCount = model.ResultArchiveAbstain.Count();
//                     model.ResultAbstainCount = ResultAbstainCount;
//                 }
//               if(abstainbtnchoice)
//                 {
//                     resultCount = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId).Count();
//                 }
//                 else
//                 {
//                     resultCount = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId && r.VoteAbstain==false).Count();
//                 }

//                 model.ResultForCount = ResultForCount;
//                 model.ResultAgainstCount = ResultAgainstCount;
               
//                 model.TotalCount = resultCount;

//                 SqlConnection conn =
//                 new SqlConnection(connStr);
//                 //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//                 string query = "SELECT SUM(Holding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "' AND [VoteFor] = 1";
//                 conn.Open();
//                 SqlCommand cmd = new SqlCommand(query, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                 {
//                     holding = Convert.ToDouble(cmd.ExecuteScalar());
//                 }
//                 conn.Close();
//                 string query1 = "SELECT SUM(PercentageHolding) FROM ResultArchives WHERE  QuestionId='" + resolution.QuestionId + "' AND [VoteFor] = 1";
//                 conn.Open();
//                 SqlCommand cmd2 = new SqlCommand(query1, conn);
//                 if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                 {
//                     percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//                 }
//                 conn.Close();

//                 model.ResultForHolding = holding.ToString();
//                 model.ResultForPercentHolding = percentHolding.ToString();

//                 //            SqlConnection conn =
//                 //new SqlConnection(connStr);
//                 //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//                 string query3 = "SELECT SUM(Holding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "' AND VoteAgainst = 1";
//                 conn.Open();
//                 SqlCommand cmd3 = new SqlCommand(query3, conn);

//                 if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
//                 {
//                     holdingAgainst = Convert.ToDouble(cmd3.ExecuteScalar());
//                 }
//                 conn.Close();
//                 string query4 = "SELECT SUM(PercentageHolding) FROM ResultArchives WHERE  QuestionId='" + resolution.QuestionId + "' AND VoteAgainst = 1";
//                 conn.Open();
//                 SqlCommand cmd4 = new SqlCommand(query4, conn);
//                 if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
//                 {
//                     percentHoldingAgainst = Convert.ToDouble(cmd4.ExecuteScalar());
//                 }
//                 conn.Close();

//                 model.ResultForHoldingAgainst = holdingAgainst.ToString();
//                 model.ResultForPercentHoldingAgainst = percentHoldingAgainst.ToString();

//                 if(abstainbtnchoice)
//                 {
//                     string query5 = "SELECT SUM(Holding) FROM ResultArchives WHERE   QuestionId='" + resolution.QuestionId + "' AND VoteAbstain = 1";
//                     conn.Open();
//                     SqlCommand cmd5 = new SqlCommand(query5, conn);

//                     if (!DBNull.Value.Equals(cmd5.ExecuteScalar()))
//                     {
//                         holdingAbstain = Convert.ToDouble(cmd5.ExecuteScalar());
//                     }
//                     conn.Close();
//                     string query6 = "SELECT SUM(PercentageHolding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "' AND VoteAbstain = 1";
//                     conn.Open();
//                     SqlCommand cmd6 = new SqlCommand(query6, conn);
//                     if (!DBNull.Value.Equals(cmd6.ExecuteScalar()))
//                     {
//                         percentHoldingAgainst = Convert.ToDouble(cmd6.ExecuteScalar());
//                     }
//                     conn.Close();

//                     model.ResultForHoldingAbstain = holdingAbstain.ToString();
//                     model.ResultForPercentHoldingAbstain = percentHoldingAbstain.ToString();
//                 }

//                 string query8 = "SELECT SUM(Holding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "' AND VoteVoid = 1";
//                 conn.Open();
//                 SqlCommand cmd8 = new SqlCommand(query8, conn);

//                 if (!DBNull.Value.Equals(cmd8.ExecuteScalar()))
//                 {
//                     holdingVoid = Convert.ToDouble(cmd8.ExecuteScalar());
//                 }
//                 conn.Close();
//                 string query9 = "SELECT SUM(PercentageHolding) FROM ResultArchives WHERE  QuestionId='" + resolution.QuestionId + "' AND VoteVoid = 1";
//                 conn.Open();
//                 SqlCommand cmd9 = new SqlCommand(query9, conn);
//                 if (!DBNull.Value.Equals(cmd9.ExecuteScalar()))
//                 {
//                     percentHoldingVoid = Convert.ToDouble(cmd9.ExecuteScalar());
//                 }
//                 conn.Close();

//                 model.ResultForHoldingVoid = holdingVoid.ToString();
//                 model.ResultForPercentHoldingVoid = percentHoldingVoid.ToString();

//                 if (abstainbtnchoice)
//                 {
//                     string query7 = "SELECT SUM(Holding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "'";
//                     conn.Open();
//                     SqlCommand cmd7 = new SqlCommand(query7, conn);
//                     if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
//                     {
//                         TotalHolding = Convert.ToDouble(cmd7.ExecuteScalar());
//                     }
//                     conn.Close();
//                 }
//                 else
//                 {
//                     string query7 = "SELECT SUM(Holding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "' AND VoteAbstain !=1";
//                     conn.Open();
//                     SqlCommand cmd7 = new SqlCommand(query7, conn);
//                     if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
//                     {
//                         TotalHolding = Convert.ToDouble(cmd7.ExecuteScalar());
//                     }
//                     conn.Close();
//                 }


//                 model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0"); 
//                 //model.TotalPercentHolding = TotalPercentHolding.ToString();

//                 //calcuate percentage holding per resolution..
//                 var pFor = (holding / TotalHolding) * 100;
//                 model.PercentageResultFor = pFor.ToString();
//                 var pAgainst = (holdingAgainst / TotalHolding) * 100;
//                 model.PercentageResultAgainst = pAgainst.ToString();
//                 var pVoid = (holdingAgainst / TotalHolding) * 100;
//                 model.PercentageResultVoid = pVoid.ToString();
//                 double pAbstain = 0;
//                 if(abstainbtnchoice)
//                 {
//                     pAbstain = (holdingAbstain / TotalHolding) * 100;
//                     model.PercentageResultAbstain = pAbstain.ToString();
//                 }
//                 var sumAllPercentages = pFor + pAgainst + pAbstain + pVoid;
//                 model.TotalPercentageHolding = sumAllPercentages.ToString();
//             }


//             return Task.FromResult<ReportViewModelDto>(model);
//         }

//         [HttpPost]
//         public async Task<ActionResult> Present(CompanyModel pmodel)
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //if (String.IsNullOrEmpty(pmodel.Year))
//             //{
//             //    return View(new ReportViewModelDto());
//             //}
//             var companies = db.SettingsArchive.Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList(); 
//             ViewBag.AGMHistory = new SelectList(companies ?? new List<string>());
//             //var AGMDb = db.Settings.Where(s => s.CompanyName == companyinfo);
//             //ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             ViewBag.value = "tab";
//             var response = await PresentPostAsync(pmodel);
//             return PartialView(response);
//         }

//         public Task<ReportViewModelDto> PresentAsync()
//         {

//             //Double holding = 0;
//             //Double percentHolding = 0;
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             //var currentYear = DateTime.Now.Year.ToString();
//             ReportViewModelDto model = new ReportViewModelDto();

//             //if (UniqueAGMId != -1)
//             //{
//             //    var present = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId && p.present == true).Count();
//             //    var proxy = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).Count();
//             //    var presentcount = present;
//             //    var proxycount = proxy;
//             //    model.present = presentcount + proxycount;
//             //    model.proxy = proxycount;

//             //    string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//             //    SqlConnection conn =
//             //              new SqlConnection(connStr);
//             //    //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//             //    string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + UniqueAGMId + "' AND present = 1 ";
//             //    conn.Open();
//             //    SqlCommand cmd = new SqlCommand(query, conn);
//             //    //object o = cmd.ExecuteScalar();
//             //    if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//             //    {
//             //        holding = Convert.ToDouble(cmd.ExecuteScalar());
//             //    }
//             //    conn.Close();

//             //    string query1 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID = '" + UniqueAGMId + "' AND present = 1";
//             //    conn.Open();
//             //    SqlCommand cmd2 = new SqlCommand(query1, conn);
//             //    //object o = cmd.ExecuteScalar();
//             //    if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//             //    {
//             //        percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//             //    }
//             //    conn.Close();

//             //    model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
//             //    model.Holding = holding;

//             //    var setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
//             //    if (setting != null)
//             //    {
//             //        model.logo = setting.Image != null ? "data:image/jpg;base64," +
//             //                   Convert.ToBase64String((byte[])setting.Image) : "";
//             //        model.Company = setting.CompanyName;
//             //        model.AGMTitle = setting.Title;
//             //    }
//             //}

//             return Task.FromResult<ReportViewModelDto>(model);
//         }

//         public Task<ReportViewModelDto> PresentPostAsync(CompanyModel pmodel)
//         {

//             Double holding = 0;
//             Double percentHolding = 0;
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             ReportViewModelDto model = new ReportViewModelDto();
//             var present = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.present == true && p.PermitPoll==1).Count();
//             var proxy = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.proxy == true).Count();
//             var presentcount = present;
//             var proxycount = proxy;
//             model.present = presentcount+proxycount;
//             model.proxy = proxycount;
//             model.AGMID = pmodel.AGMID;

//             string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//             SqlConnection conn =
//                       new SqlConnection(connStr);
//             //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//             //string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + pmodel.AGMID + "' AND present = 1";
//             string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + pmodel.AGMID + "' AND PermitPoll=1";
//             conn.Open();
//             SqlCommand cmd = new SqlCommand(query, conn);
//             //object o = cmd.ExecuteScalar();
//             if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//             {
//                 holding = Convert.ToDouble(cmd.ExecuteScalar());
//             }
//             conn.Close();

//             string query1 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID = '" + pmodel.AGMID + "' AND PermitPoll=1";
//             conn.Open();
//             SqlCommand cmd2 = new SqlCommand(query1, conn);
//             //object o = cmd.ExecuteScalar();
//             if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//             {
//                 percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//             }
//             conn.Close();

//             model.TotalPercentageHolding = String.Format("{0:0.####}", percentHolding);
//             model.Holding = holding;

//             var setting = db.SettingsArchive.FirstOrDefault(s => s.AGMID == pmodel.AGMID);
//             if (setting != null)
//             {
//                 model.logo = setting.Image != null ? "data:image/jpg;base64," +
//                            Convert.ToBase64String((byte[])setting.Image) : "";
//                 model.Company = setting.CompanyName;
//                 model.AGMTitle = setting.Title;
//             }

//             return Task.FromResult<ReportViewModelDto>(model);
//         }



//         public ActionResult AjaxHandler(CompanyModel pmodel)
//         {
//             try
//             {
//                 //Creating instance of DatabaseContext class  
//                 using (UsersContext _context = new UsersContext())
//                 {
//                     //var companyinfo = ua.GetUserCompanyInfo();
//                     //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//                     var draw = Request.Form.GetValues("draw").FirstOrDefault();
//                     var start = Request.Form.GetValues("start").FirstOrDefault();
//                     var length = Request.Form.GetValues("length").FirstOrDefault();
//                     var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
//                     var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
//                     var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


//                     //Paging Size (10,20,50,100)    
//                     int pageSize = length != null ? Convert.ToInt32(length) : 0;
//                     int skip = start != null ? Convert.ToInt32(start) : 0;
//                     int recordsTotal = 0;
//                     var currentYear = DateTime.Now.Year.ToString();
//                     // Getting all Shareholder data 
//                     var allPresent = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.PermitPoll == 1).ToList();
//                     IEnumerable<PresentArchive> filteredpresent;

//                     //Sorting  
//                     if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
//                     {
//                         filteredpresent = allPresent.OrderBy(s => s.Id);
//                     }
//                     //Search
//                     if (!string.IsNullOrEmpty(searchValue))
//                     {
//                         filteredpresent = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.PermitPoll == 1).ToList()
//                                  .Where(c => c.Name.Contains(searchValue)

//                                              ||
//                                              c.ShareholderNum.ToString() == searchValue);
//                     }
//                     else
//                     {
//                         filteredpresent = allPresent;
//                     }
//                     //Paging   
//                     var displayedpresent = filteredpresent
//                              .Skip(skip)
//                              .Take(pageSize);
//                     //var result = displayedCompanies;
//                     var shareholderpresent = GetPresent(displayedpresent);
//                     //var shareholderData = from c in displayedCompanies
//                     //             //select c;
//                     //  select new[] { Convert.ToString(c.SN),Convert.ToString(c.Id), c.Name, c.Address, c.ShareholderNum, c.Holding, c.PercentageHolding,
//                     //      c.PhoneNumber, c.emailAddress,Convert.ToString(c.Present),Convert.ToString(c.PresentByProxy) };   

//                     //total number of rows count     
//                     recordsTotal = allPresent.Count();


//                     //Returning Json Data    
//                     return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = shareholderpresent }, JsonRequestBehavior.AllowGet);
//                 }
//             }
//             catch (Exception)
//             {
//                 throw;
//             }

//         }

//         private IEnumerable<PresentArchive> GetPresent(IEnumerable<PresentArchive> displayedpresent)
//         {
//             var shareholderpresent = from c in displayedpresent
//                                      select new PresentArchive()
//                                      {

//                                          Id = c.Id,
//                                          Name = c.Name,
//                                          Address = c.Address,
//                                          Holding = c.Holding,
//                                          PercentageHolding = c.PercentageHolding,
//                                          ShareholderNum = c.ShareholderNum,
//                                          newNumber = c.newNumber,
//                                          TakePoll = c.TakePoll,
//                                          present = c.present,
//                                          proxy = c.proxy,
//                                          split = c.split,
//                                          emailAddress = c.emailAddress,
//                                          admitSource = c.admitSource,
//                                          PhoneNumber = c.PhoneNumber
//                                      };

//             return shareholderpresent.ToList();

//         }

//         public async Task<ActionResult> Proxy()
//         {
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();

//             var companyinfo = ua.GetUserCompanyInfo();
//             var AGMDb = db.SettingsArchive.Where(s => s.CompanyName == companyinfo);
//             ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var response = await ProxyAsync();
//             return PartialView(response);
//         }

//         public Task<ReportViewModelDto> ProxyAsync()
//         {

//             //Double TotalShareholding = 326700000;
//             Double holding = 0;
//             Double percentHolding = 0;
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             ReportViewModelDto model = new ReportViewModelDto();

//             if (UniqueAGMId != -1)
//             {
//                 var currentYear = DateTime.Now.Year.ToString();
//                 var proxy = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).Count();
//                 var proxycount = proxy;
//                 model.present = proxycount;

//                 string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                 SqlConnection conn =
//                           new SqlConnection(connStr);
//                 //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//                 string query = "SELECT SUM(Holding) FROM PresentArchives where AGMID = '" + UniqueAGMId + "' AND Proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd = new SqlCommand(query, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                 {
//                     holding = Convert.ToDouble(cmd.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query1 = "SELECT SUM(PercentageHolding) FROM PresentArchives where AGMID = '" + UniqueAGMId + "' AND Proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd2 = new SqlCommand(query1, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                 {
//                     percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//                 }
//                 conn.Close();

//                 model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
//                 model.Holding = holding;

//             }


//             return Task.FromResult<ReportViewModelDto>(model);
//         }

//         public async Task<ActionResult> TotalAttendees()
//         {
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();

//             var companyinfo = ua.GetUserCompanyInfo();


//             var AGMDb = db.SettingsArchive.Where(s => s.CompanyName == companyinfo);
//             ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var response = await TotalAttendeesAsync();

//             return View(response);
//         }


//         [HttpPost]
//         public async Task<ActionResult> TotalAttendees(CompanyModel pmodel)
//         {
//             //if (String.IsNullOrEmpty(pmodel.Year))
//             //{
//             //    return View(new ReportViewModelDto());
//             //}

//             //var companyinfo = ua.GetUserCompanyInfo();

//             var AGMDb = db.SettingsArchive.Where(s => s.CompanyName == pmodel.CompanyInfo);
//             ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var response = await TotalAttendeesPostAsync(pmodel);

//             return PartialView(response);
//         }

//         public Task<ReportViewModelDto> TotalAttendeesAsync()
//         {
//             //Double TotalShareholding = 326700000;
//             Double holding = 0;
//             Double percentHolding = 0;
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             var currentYear = DateTime.Now.Year.ToString();

//             ReportViewModelDto model = new ReportViewModelDto();
//             if (UniqueAGMId != -1)
//             {
//                 var totalAttaindees = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll==1).Count();
//                 var totalAttaindeescount = totalAttaindees;
//                 model.present = totalAttaindeescount;

//                 string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                 SqlConnection conn =
//                           new SqlConnection(connStr);
//                 string query = "SELECT SUM(Holding) FROM PresentArchives Where AGMID ='" + UniqueAGMId + "' AND PermitPoll = 1";
//                 conn.Open();
//                 SqlCommand cmd = new SqlCommand(query, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                 {
//                     holding = Convert.ToDouble(cmd.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query1 = "SELECT SUM(PercentageHolding) FROM PresentArchives Where AGMID ='" + UniqueAGMId + "' AND PermitPoll=1";
//                 conn.Open();
//                 SqlCommand cmd2 = new SqlCommand(query1, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                 {
//                     percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//                 }
//                 conn.Close();

//                 model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
//                 model.Holding = holding;
//             }


//             return Task.FromResult<ReportViewModelDto>(model);
//             //return View(totalAttaindees);
//         }


//         private Task<ReportViewModelDto> TotalAttendeesPostAsync(CompanyModel pmodel)
//         {

//             //Double TotalShareholding = 326700000;
//             Double holding = 0;
//             Double percentHolding = 0;
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             ReportViewModelDto model = new ReportViewModelDto();

//             if (pmodel.AGMID != -1)
//             {
//                 var totalAttaindees = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.PermitPoll == 1).Count();
//                 var totalAttaindeescount = totalAttaindees;
//                 model.present = totalAttaindeescount;

//                 string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                 SqlConnection conn =
//                           new SqlConnection(connStr);
//                 string query = "SELECT SUM(Holding) FROM PresentArchives Where AGMID = '" + pmodel.AGMID + "' AND PermitPoll=1";
//                 conn.Open();
//                 SqlCommand cmd = new SqlCommand(query, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                 {
//                     holding = Convert.ToDouble(cmd.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query1 = "SELECT SUM(PercentageHolding) FROM PresentArchives Where  AGMID = '" + pmodel.AGMID + "' AND PermitPoll=1";
//                 conn.Open();
//                 SqlCommand cmd2 = new SqlCommand(query1, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                 {
//                     percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//                 }
//                 conn.Close();

//                 model.TotalPercentageHolding = String.Format("{0:0.##}", percentHolding);
//                 model.Holding = holding;

//             }


//             return Task.FromResult<ReportViewModelDto>(model);
//             //return View(totalAttaindees);
//         }


//         public async Task<ActionResult> AttendeeSummary()
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             var companies = db.SettingsArchive.Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList(); ;
//             ViewBag.AGMHistory = new SelectList(companies ?? new List<string>());
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var response = await AttendeeSummaryAsync();
//             return PartialView(response);
//         }

//         [HttpPost]
//         public async Task<ActionResult> AttendeeSummary(CompanyModel pmodel)
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();

//             IEnumerable<SettingsModel> AGMDb;
//             List<string> companies;
//             if (String.IsNullOrEmpty(pmodel.AGMID.ToString()))
//             {
//                 companies = db.SettingsArchive.Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList(); ;
//                 ViewBag.AGMHistory = new SelectList(companies ?? new List<string>());
//                 ViewBag.value = "tab";
//                 return PartialView(new ReportViewModelDto());
//             }

//             companies = db.SettingsArchive.Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList(); ;
//             ViewBag.AGMHistory = new SelectList(companies ?? new List<string>());
//             ViewBag.value = "tab";
//             var response = await AttendeeSummaryPostAsync(pmodel);

//             return PartialView(response);
//         }

//         public Task<ReportViewModelDto> AttendeeSummaryAsync()
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             //var currentYear = DateTime.Now.Year.ToString();

//             ReportViewModelDto model = new ReportViewModelDto();
//             model.AGMID = 0;
//             //if (UniqueAGMId != -1)
//             //{
//             //    model.Year = currentYear;
//             //    model.presentcount = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId).Count();
//             //    //ViewBag.feedbackcount = db.ShareholderFeedback.Count();
//             //    //model.shareholders = db.BarcodeStore.Where(b=>b.Company==companyinfo).Count();
//             //    int TotalCount = 0;
//             //    Double TotalHolding = 0;
//             //    Double TotalPercentageHolding = 0;
//             //    Double presentholding = 0;
//             //    Double presentpercentHolding = 0;
//             //    Double proxyholding = 0;
//             //    Double proxypercentHolding = 0;

//             //    var present = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId && p.present == true).Count();
//             //    var presentcount = present;
//             //    model.present = presentcount;
//             //    string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//             //    SqlConnection conn =
//             //              new SqlConnection(connStr);
//             //    string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + UniqueAGMId + "' AND present = 1 ";
//             //    conn.Open();
//             //    SqlCommand cmd = new SqlCommand(query, conn);
//             //    //object o = cmd.ExecuteScalar();
//             //    if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//             //    {
//             //        presentholding = Convert.ToDouble(cmd.ExecuteScalar());
//             //    }
//             //    conn.Close();

//             //    string query1 = "SELECT SUM(PercentageHolding)  FROM PresentArchives WHERE AGMID = '" + UniqueAGMId + "' AND present = 1";
//             //    conn.Open();
//             //    SqlCommand cmd2 = new SqlCommand(query1, conn);
//             //    //object o = cmd.ExecuteScalar();
//             //    if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//             //    {
//             //        presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//             //    }
//             //    conn.Close();

//             //    model.TotalPercentageHolding = String.Format("{0:0.####}", presentpercentHolding);
//             //    model.Holding = presentholding;

//             //    var proxy = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).Count();
//             //    var proxycount = proxy;
//             //    model.proxy = proxycount;

//             //    string query3 = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + UniqueAGMId + "' AND proxy = 1";
//             //    conn.Open();
//             //    SqlCommand cmd3 = new SqlCommand(query3, conn);
//             //    //object o = cmd.ExecuteScalar();
//             //    if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
//             //    {
//             //        proxyholding = Convert.ToDouble(cmd3.ExecuteScalar());
//             //    }
//             //    conn.Close();

//             //    string query4 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID = '" + UniqueAGMId + "' AND proxy = 1";
//             //    conn.Open();
//             //    SqlCommand cmd4 = new SqlCommand(query4, conn);
//             //    //object o = cmd.ExecuteScalar();
//             //    if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
//             //    {
//             //        proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
//             //    }
//             //    conn.Close();

//             //    model.TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
//             //    model.ProxyHolding = proxyholding;

//             //    TotalCount = proxycount + presentcount;
//             //    model.TotalCount = TotalCount;

//             //    TotalHolding = presentholding + proxyholding;
//             //    model.TotalHolding = TotalHolding;

//             //    TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
//             //    model.PercentageTotalHolding = String.Format("{0:0.####}", TotalPercentageHolding);

//             //    var setting = db.Settings.FirstOrDefault(f => f.AGMID == UniqueAGMId);
//             //    if (setting != null)
//             //    {
//             //        model.logo = setting.Image != null ? "data:image/jpg;base64," +
//             //                   Convert.ToBase64String((byte[])setting.Image) : "";
//             //        model.Company = setting.CompanyName;
//             //        model.AGMTitle = setting.Title;
//             //    }

//             //}
//             return Task.FromResult<ReportViewModelDto>(model);
//         }


//         public Task<ReportViewModelDto> AttendeeSummaryPostAsync(CompanyModel pmodel)
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             ReportViewModelDto model = new ReportViewModelDto();
//             if (pmodel.AGMID != -1)
//             {
//                 //model.Year = pmodel.Year.Trim();

//                 model.AGMID = pmodel.AGMID;
//                 model.presentcount = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.PermitPoll == 1).Count();
//                 //ViewBag.feedbackcount = db.ShareholderFeedback.Count();
//                 //model.shareholders = db.BarcodeStore.Where(b => b.Company == companyinfo).Count();
//                 int TotalCount = 0;
//                 Double TotalHolding = 0;
//                 Double TotalPercentageHolding = 0;
//                 Double presentholding = 0;
//                 Double presentpercentHolding = 0;
//                 Double proxyholding = 0;
//                 Double proxypercentHolding = 0;

//                 var present = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.present == true && p.PermitPoll== 1).ToList();
//                 var presentcount = present.Count();
//                 model.present = presentcount;
//                 string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                 SqlConnection conn =
//                           new SqlConnection(connStr);
//                 string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID ='" + pmodel.AGMID + "' AND present = 1  AND PermitPoll=1";
//                 conn.Open();
//                 SqlCommand cmd = new SqlCommand(query, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                 {
//                     presentholding = Convert.ToDouble(cmd.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query1 = "SELECT SUM(PercentageHolding)  FROM PresentArchives WHERE AGMID ='" + pmodel.AGMID + "' AND present = 1 AND PermitPoll=1";
//                 conn.Open();
//                 SqlCommand cmd2 = new SqlCommand(query1, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                 {
//                     presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//                 }
//                 conn.Close();

//                 model.TotalPercentageHolding = String.Format("{0:0.####}", presentpercentHolding);
//                 model.Holding = presentholding;

//                 var proxy = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.proxy == true).ToList();
//                 var proxycount = proxy.Count();
//                 model.proxy = proxycount;

//                 string query3 = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID ='" + pmodel.AGMID + "' AND proxy = 1 ";
//                 conn.Open();
//                 SqlCommand cmd3 = new SqlCommand(query3, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
//                 {
//                     proxyholding = Convert.ToDouble(cmd3.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query4 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID ='" + pmodel.AGMID + "' AND proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd4 = new SqlCommand(query4, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
//                 {
//                     proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
//                 }
//                 conn.Close();

//                 model.TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
//                 model.ProxyHolding = proxyholding;

//                 TotalCount = proxycount + presentcount;
//                 model.TotalCount = TotalCount;

//                 TotalHolding = presentholding + proxyholding;
//                 model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0");

//                 TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
//                 model.PercentageTotalHolding = String.Format("{0:0.####}", TotalPercentageHolding);

//                 var setting = db.Settings.FirstOrDefault(f => f.AGMID == pmodel.AGMID);
//                 if (setting != null)
//                 {
//                     model.logo = setting.Image != null ? "data:image/jpg;base64," +
//                                Convert.ToBase64String((byte[])setting.Image) : "";
//                     model.Company = setting.CompanyName;
//                     model.AGMTitle = setting.Title;
//                 }
//             }


//             return Task.FromResult<ReportViewModelDto>(model);
//         }



//         [HttpPost]
//         public async Task<ActionResult> TimeFilterAttendeeSummary(FormCollection form)
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var t = form["time"];
//             if (!String.IsNullOrEmpty(t))
//             {
//                 var response = await TimeFilterAttendeeSummaryAsync(form);

//                 return View(response);
//             }
//             return RedirectToAction("AttendeeSummary");
//         }


//         private Task<ReportViewModelDto> TimeFilterAttendeeSummaryAsync(FormCollection form)
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             var currentYear = DateTime.Now.Year.ToString();
//             ReportViewModelDto model = new ReportViewModelDto();
//             var UniqueAGMId = int.Parse(form["AGMID"].ToString());
//             if (UniqueAGMId != -1)
//             {
//                 model.Year = currentYear;
//                 var t = form["time"];

//                 var time = DateTime.Parse(t);
//                 var timestamp = time.TimeOfDay;

//                 int TotalCount = 0;
//                 Double TotalHolding = 0;
//                 Double TotalPercentageHolding = 0;
//                 Double presentholding = 0;
//                 Double presentpercentHolding = 0;
//                 Double proxyholding = 0;
//                 Double proxypercentHolding = 0;
//                 var present = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId &&  p.PermitPoll== 1 && p.present == true && p.PresentTime <= time).ToList();

//                 var presentcount = present.Count();
//                 model.present = presentcount;

//                 string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                 SqlConnection conn =
//                           new SqlConnection(connStr);
//                 string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID='" + UniqueAGMId + "' AND PermitPoll=1 AND present=1 AND PresentTime <= '" + time + "'";
//                 conn.Open();
//                 SqlCommand cmd = new SqlCommand(query, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                 {
//                     presentholding = Convert.ToDouble(cmd.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query1 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID ='" + UniqueAGMId + "' AND PermitPoll=1 AND present=1 AND PresentTime <= '" + time + "'";
//                 conn.Open();
//                 SqlCommand cmd2 = new SqlCommand(query1, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                 {
//                     presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//                 }
//                 conn.Close();

//                 model.TotalPercentageHolding = String.Format("{0:0.####}", presentpercentHolding);
//                 model.Holding = presentholding;

//                 var proxy = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).ToList();
//                 var proxycount = proxy.Count();
//                 model.proxy = proxycount;

//                 string query3 = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID ='" + UniqueAGMId + "' AND proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd3 = new SqlCommand(query3, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
//                 {
//                     proxyholding = Convert.ToDouble(cmd3.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query4 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID ='" + UniqueAGMId + "' AND proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd4 = new SqlCommand(query4, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
//                 {
//                     proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
//                 }
//                 conn.Close();


//                 model.TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
//                 model.ProxyHolding = proxyholding;

//                 TotalCount = proxycount + presentcount;
//                 model.TotalCount = TotalCount;

//                 TotalHolding = presentholding + proxyholding;
//                 model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0");

//                 TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
//                 model.PercentageTotalHolding = String.Format("{0:0.####}", TotalPercentageHolding);


//                 var setting = db.SettingsArchive.FirstOrDefault(s => s.AGMID == UniqueAGMId);
//                 if (setting != null)
//                 {
//                     model.logo = setting.Image != null ? "data:image/jpg;base64," +
//                                Convert.ToBase64String((byte[])setting.Image) : "";
//                     model.Company = setting.CompanyName;
//                     model.AGMTitle = setting.Title;
//                 }


//             }

//             return Task.FromResult<ReportViewModelDto>(model);
//         }


//         [HttpPost]
//         public async Task<ActionResult> RangeFilterAttendeeSummary(FormCollection form)
//         {
//             var s = form["start"];
//             var e = form["end"];
//             if (!String.IsNullOrEmpty(s) && !String.IsNullOrEmpty(e))
//             {
//                 var response = await RangeFilterAttendeeSummaryAsync(form);

//                 return View(response);
//             }
//             return RedirectToAction("AttendeeSummary");
//         }



//         private Task<ReportViewModelDto> RangeFilterAttendeeSummaryAsync(FormCollection form)
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var currentYear = DateTime.Now.Year.ToString();
//             ReportViewModelDto model = new ReportViewModelDto();


//             model.Year = currentYear;
//             var s = form["start"];
//             var e = form["end"];

//             var starttime = DateTime.Parse(s);
//             var endtime = DateTime.Parse(e);
//             var starttimestamp = starttime.TimeOfDay;
//             var endtimestamp = endtime.TimeOfDay;

//             int TotalCount = 0;
//             Double TotalHolding = 0;
//             Double TotalPercentageHolding = 0;
//             Double presentholding = 0;
//             Double presentpercentHolding = 0;
//             Double proxyholding = 0;
//             Double proxypercentHolding = 0;
//             var UniqueAGMId =int.Parse(form["start"].ToString());
//             if (UniqueAGMId != -1)
//             {
//                 var present = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId && p.PermitPoll==1 &&  p.present == true && p.PresentTime >= starttime && p.PresentTime <= endtime).Count();
//                 var presentcount = present;
//                 model.present = presentcount;
//                 string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                 SqlConnection conn =
//                           new SqlConnection(connStr);
//                 string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID='" + UniqueAGMId + "' AND PermitPoll=1 AND present=1 AND PresentTime >= '" + starttime + "' AND PresentTime <= '" + endtime + "'";
//                 conn.Open();
//                 SqlCommand cmd = new SqlCommand(query, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                 {
//                     presentholding = Convert.ToDouble(cmd.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query1 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID='" + UniqueAGMId + "'  AND PermitPoll=1 AND present=1 AND PresentTime >= '" + starttime + "' AND PresentTime <= '" + endtime + "'";
//                 conn.Open();
//                 SqlCommand cmd2 = new SqlCommand(query1, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                 {
//                     presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//                 }
//                 conn.Close();

//                 model.TotalPercentageHolding = String.Format("{0:0.####}", presentpercentHolding);
//                 model.Holding = presentholding;

//                 var proxy = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId && p.proxy == true).ToList();
//                 var proxycount = proxy.Count();
//                 model.proxy = proxycount;
//                 string query3 = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID ='" + UniqueAGMId + "' AND proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd3 = new SqlCommand(query3, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
//                 {
//                     proxyholding = Convert.ToDouble(cmd3.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query4 = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID ='" + UniqueAGMId + "' AND proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd4 = new SqlCommand(query4, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
//                 {
//                     proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
//                 }
//                 conn.Close();


//                 model.TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
//                 model.ProxyHolding = proxyholding;

//                 TotalCount = proxycount + presentcount;
//                 model.TotalCount = TotalCount;


//                 TotalHolding = presentholding + proxyholding;
//                 model.TotalHolding = Convert.ToDouble(TotalHolding).ToString("0,0");

//                 TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
//                 model.PercentageTotalHolding = String.Format("{0:0.####}", TotalPercentageHolding);


//                 var setting = db.Settings.FirstOrDefault(f => f.AGMID == UniqueAGMId);
//                 if (setting != null)
//                 {
//                     model.logo = setting.Image != null ? "data:image/jpg;base64," +
//                                Convert.ToBase64String((byte[])setting.Image) : "";
//                     model.Company = setting.CompanyName;
//                     model.AGMTitle = setting.Title;
//                 }
//             }


//             return Task.FromResult<ReportViewModelDto>(model);

//         }

//         public async Task<ActionResult> PrintIndex()
//         {
//             var response = await PrintIndexAsync();

//             return PartialView(response);
//         }

//         private Task<ReportViewModelDto> PrintIndexAsync()
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var AGMID = _AGMID;

//             ReportViewModelDto model = new ReportViewModelDto();


//             model.present = Presentcount;
//             model.TotalPercentageHolding = TotalPercentagePresentHolding;
//             model.Holding = PresentHolding;
//             model.proxy = ProxyCount;
//             model.TotalPercentageProxyHolding = TotalPercentageProxyHolding;
//             model.ProxyHolding = ProxyHolding;
//             model.TotalCount = TotalCountPresent_Proxy;
//             model.TotalHolding = Convert.ToDouble(TotalHoldingPresent_Proxy).ToString("0,0");
//             model.PercentageTotalHolding = TotalPercentagePresent_Proxy;
//             if (AGMID != -1)
//             {
//                 var setting = db.SettingsArchive.FirstOrDefault(f => f.AGMID == AGMID);
//                 if (setting != null)
//                 {
//                     model.logo = setting.Image != null ? "data:image/jpg;base64," +
//                                Convert.ToBase64String((byte[])setting.Image) : "";
//                     model.Company = setting.CompanyName;
//                     model.PrintTitle = setting.PrintOutTitle;
//                     model.AGMVenue = setting.Venue;
//                     DateTime time;
//                     if (setting.AgmDateTime != null)
//                     {
//                         time = (DateTime)setting.AgmDateTime;
//                         model.AGMTime = time.ToString("dddd, dd MMMM yyyy");
//                     }

//                 }
//             }

//             return Task.FromResult<ReportViewModelDto>(model);
//         }

//         public async Task<ActionResult> EndPrintIndex()
//         {
//             var response = await EndPrintIndexAsync();

//             return PartialView(response);
//         }

//         private Task<ReportViewModelDto> EndPrintIndexAsync()
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var AGMID = _AGMID;

//             ReportViewModelDto model = new ReportViewModelDto();
//             model.present = Presentcount;
//             model.TotalPercentageHolding = TotalPercentagePresentHolding;
//             model.Holding = PresentHolding;
//             model.proxy = ProxyCount;
//             model.TotalPercentageProxyHolding = TotalPercentageProxyHolding;
//             model.ProxyHolding = ProxyHolding;
//             model.TotalCount = TotalCountPresent_Proxy;
//             model.TotalHolding = Convert.ToDouble(TotalHoldingPresent_Proxy).ToString("0,0");
//             model.PercentageTotalHolding = TotalPercentagePresent_Proxy;

//             if (AGMID != -1)
//             {
//                 var setting = db.SettingsArchive.FirstOrDefault(f => f.AGMID == AGMID);
//                 if (setting != null)
//                 {
//                     model.logo = setting.Image != null ? "data:image/jpg;base64," +
//                                Convert.ToBase64String((byte[])setting.Image) : "";
//                     model.Company = setting.CompanyName;
//                     model.PrintTitle = setting.PrintOutTitle;
//                     model.AGMVenue = setting.Venue;
//                     DateTime time;
//                     if (setting.AgmDateTime != null)
//                     {
//                         time = (DateTime)setting.AgmDateTime;
//                         model.AGMTime = time.ToString("dddd, dd MMMM yyyy");
//                     }


//                 }
//             }

//             return Task.FromResult<ReportViewModelDto>(model);
//         }

//         [HttpPost]
//         public async Task<ActionResult> TimeIndex(FormCollection form)
//         {
//             var t = form["time"];
//             if (!String.IsNullOrEmpty(t))
//             {
//                 var response = await TimeIndexAsync(form);
//                 return RedirectToAction("PrintIndex");
//             }
//             return RedirectToAction("PrintIndex");
//         }



//         private Task<ReportViewModelDto> TimeIndexAsync(FormCollection form)
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             ReportViewModelDto model = new ReportViewModelDto();
//             var t = form["time"].ToString();
//             var AGMID = int.Parse(form["AGMID"].ToString());
//             var time = DateTime.Parse(t);
//             var timestamp = time.TimeOfDay;
//             int TotalCount = 0;
//             Double TotalHolding = 0;
//             Double TotalPercentageHolding = 0;
//             Double presentholding = 0;
//             Double presentpercentHolding = 0;
//             Double proxyholding = 0;
//             Double proxypercentHolding = 0;
//             _AGMID = AGMID;

//             if (AGMID != -1)
//             {
//                 var present = db.PresentArchive.Where(p => p.AGMID == AGMID && p.PermitPoll== 1 && p.present == true && p.PresentTime <= time).ToList();

//                 var presentcount = present.Count();
//                 Presentcount = presentcount;
//                 string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                 SqlConnection conn =
//                           new SqlConnection(connStr);
//                 string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + AGMID + "' AND PermitPoll=1 AND present=1 AND PresentTime <= '" + time + "'";
//                 conn.Open();
//                 SqlCommand cmd = new SqlCommand(query, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                 {
//                     presentholding = Convert.ToDouble(cmd.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query1 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID = '" + AGMID + "' AND PermitPoll=1 AND present=1 AND PresentTime <= '" + time + "'";
//                 conn.Open();
//                 SqlCommand cmd2 = new SqlCommand(query1, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                 {
//                     presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//                 }
//                 conn.Close();

//                 TotalPercentagePresentHolding = String.Format("{0:0.####}", presentpercentHolding);
//                 PresentHolding = presentholding;

//                 var proxy = db.PresentArchive.Where(p => p.proxy == true).ToList();
//                 var proxycount = proxy.Count();
//                 ProxyCount = proxycount;
//                 string query3 = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + AGMID + "' AND proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd3 = new SqlCommand(query3, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
//                 {
//                     proxyholding = Convert.ToDouble(cmd3.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query4 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID = '" + AGMID + "' AND proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd4 = new SqlCommand(query4, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
//                 {
//                     proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
//                 }
//                 conn.Close();

//                 TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
//                 ProxyHolding = proxyholding;

//                 TotalCount = proxycount + presentcount;
//                 TotalCountPresent_Proxy = TotalCount;

//                 TotalHolding = presentholding + proxyholding;
//                 TotalHoldingPresent_Proxy = TotalHolding;

//                 TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
//                 TotalPercentagePresent_Proxy = String.Format("{0:0.####}", TotalPercentageHolding);

//             }

//             return Task.FromResult<ReportViewModelDto>(model);

//         }


//         [HttpPost]
//         public async Task<ActionResult> EndTimeIndex(FormCollection form)
//         {
//             var t = form["time"];
//             if (!String.IsNullOrEmpty(t))
//             {
//                 var response = await EndTimeIndexAsync(form);

//                 return RedirectToAction("EndPrintIndex");
//             }

//             return RedirectToAction("EndPrintIndex");
//         }



//         private Task<ReportViewModelDto> EndTimeIndexAsync(FormCollection form)
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             ReportViewModelDto model = new ReportViewModelDto();
//             var t = form["time"].ToString();
//             var AGMID = int.Parse(form["AGMID"].ToString());
//             var time = DateTime.Parse(t);
//             var timestamp = time.TimeOfDay;

//             int TotalCount = 0;
//             Double TotalHolding = 0;
//             Double TotalPercentageHolding = 0;
//             Double presentholding = 0;
//             Double presentpercentHolding = 0;
//             Double proxyholding = 0;
//             Double proxypercentHolding = 0;
//             _AGMID = AGMID;

//             if (AGMID != -1)
//             {
//                 var present = db.PresentArchive.Where(p => p.AGMID == AGMID && p.PermitPoll==1 &&  p.present == true && p.PresentTime <= time).ToList();

//                 var presentcount = present.Count();
//                 Presentcount = presentcount;
//                 string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                 SqlConnection conn =
//                           new SqlConnection(connStr);
//                 string query = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + AGMID + "' AND PermitPoll=1 AND present=1 AND PresentTime <= '" + time + "'";
//                 conn.Open();
//                 SqlCommand cmd = new SqlCommand(query, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                 {
//                     presentholding = Convert.ToDouble(cmd.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query1 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID = '" + AGMID + "' AND PermitPoll=1 AND present=1 AND PresentTime <= '" + time + "'";
//                 conn.Open();
//                 SqlCommand cmd2 = new SqlCommand(query1, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                 {
//                     presentpercentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//                 }
//                 conn.Close();


//                 TotalPercentagePresentHolding = String.Format("{0:0.####}", presentpercentHolding);
//                 PresentHolding = presentholding;

//                 var proxy = db.PresentArchive.Where(p => p.AGMID == AGMID && p.proxy == true).ToList();
//                 var proxycount = proxy.Count();
//                 ProxyCount = proxycount;
//                 string query3 = "SELECT SUM(Holding) FROM PresentArchives WHERE AGMID = '" + AGMID + "' AND proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd3 = new SqlCommand(query3, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
//                 {
//                     proxyholding = Convert.ToDouble(cmd3.ExecuteScalar());
//                 }
//                 conn.Close();

//                 string query4 = "SELECT SUM(PercentageHolding) FROM PresentArchives WHERE AGMID = '" + AGMID + "' AND proxy = 1";
//                 conn.Open();
//                 SqlCommand cmd4 = new SqlCommand(query4, conn);
//                 //object o = cmd.ExecuteScalar();
//                 if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
//                 {
//                     proxypercentHolding = Convert.ToDouble(cmd4.ExecuteScalar());
//                 }
//                 conn.Close();

//                 TotalPercentageProxyHolding = String.Format("{0:0.####}", proxypercentHolding);
//                 ProxyHolding = proxyholding;

//                 TotalCount = proxycount + presentcount;
//                 TotalCountPresent_Proxy = TotalCount;

//                 TotalHolding = presentholding + proxyholding;
//                 TotalHoldingPresent_Proxy = TotalHolding;

//                 TotalPercentageHolding = presentpercentHolding + proxypercentHolding;
//                 TotalPercentagePresent_Proxy = String.Format("{0:0.####}", TotalPercentageHolding);

//             }
//             return Task.FromResult<ReportViewModelDto>(model);

//         }


//         //public async Task<ActionResult> Resolution(int id)
//         //{
//         //    string returnvalue = "";
//         //    if (HttpContext.Request.QueryString.Count > 0)
//         //    {
//         //        returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//         //    }
//         //    ViewBag.value = returnvalue.Trim();
//         //    var response = await ResolutionAsync(id);

//         //    return PartialView(response);
//         //}


//         //public Task<ReportViewModelDto> ResolutionAsync(int id)
//         //{
//         //    string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//         //    var companyinfo = ua.GetUserCompanyInfo();
//         //    var UniqueAGMId = ua.RetrieveAGMUniqueID();
//         //    ReportViewModelDto model = new ReportViewModelDto();
//         //    Double holding = 0;
//         //    Double percentHolding = 0;
//         //    Double holdingAgainst = 0;
//         //    Double percentHoldingAgainst = 0;
//         //    Double holdingAbstain = 0;
//         //    Double percentHoldingAbstain = 0;
//         //    Double TotalHolding = 0;
//         //    Double TotalPercentHolding = 0;
//         //    var resolution = db.QuestionArchive.Find(id);
//         //    if (resolution != null)
//         //    {
//         //        model.Id = resolution.Id;
//         //        model.resolutionName = resolution.question;

//         //        if (UniqueAGMId != -1)
//         //        {
//         //            //ResolutionResult resolutionResult = new ResolutionResult
//         //            //{
//         //            model.ResultArchiveFor = db.ResultArchive.Where(r => r.QuestionId == resolution.Id && r.AGMID == UniqueAGMId && r.For == "FOR").ToList();
//         //            model.ResultArchiveAgainst = db.ResultArchive.Where(r => r.QuestionId == resolution.Id && r.AGMID == UniqueAGMId && r.Against == "AGAINST").ToList();
//         //            model.ResultArchiveAbstain = db.ResultArchive.Where(r => r.QuestionId == resolution.Id && r.AGMID == UniqueAGMId && r.Abstain == "ABSTAIN").ToList();
//         //            //};

//         //            var ResultForCount = model.ResultFor.Count();
//         //            var ResultAgainstCount = model.ResultAgainst.Count();
//         //            var ResultAbstainCount = model.ResultAbstain.Count();
//         //            var resultCount = db.ResultArchive.Where(r => r.AGMID == UniqueAGMId).Count();
//         //            model.ResultForCount = ResultForCount;
//         //            model.ResultAgainstCount = ResultAgainstCount;
//         //            model.ResultAbstainCount = ResultAbstainCount;
//         //            model.TotalCount = resultCount;

//         //            SqlConnection conn =
//         //            new SqlConnection(connStr);
//         //            //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//         //            string query = "SELECT SUM(Holding) FROM ResultsArchive WHERE QuestionId='" + resolution.Id + "' AND [For] = 'FOR'";
//         //            conn.Open();
//         //            SqlCommand cmd = new SqlCommand(query, conn);
//         //            //object o = cmd.ExecuteScalar();
//         //            if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//         //            {
//         //                holding = Convert.ToDouble(cmd.ExecuteScalar());
//         //            }
//         //            conn.Close();
//         //            string query1 = "SELECT SUM(PercentageHolding) FROM ResultsArchive WHERE  QuestionId='" + resolution.Id + "' AND [For] = 'FOR'";
//         //            conn.Open();
//         //            SqlCommand cmd2 = new SqlCommand(query1, conn);
//         //            if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//         //            {
//         //                percentHolding = Convert.ToDouble(cmd2.ExecuteScalar());
//         //            }
//         //            conn.Close();

//         //            model.ResultForHolding = holding.ToString();
//         //            model.ResultForPercentHolding = percentHolding.ToString();

//         //            //            SqlConnection conn =
//         //            //new SqlConnection(connStr);
//         //            //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//         //            string query3 = "SELECT SUM(Holding) FROM ResultsArchive WHERE QuestionId='" + resolution.Id + "' AND Against = 'AGAINST'";
//         //            conn.Open();
//         //            SqlCommand cmd3 = new SqlCommand(query3, conn);

//         //            if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
//         //            {
//         //                holdingAgainst = Convert.ToDouble(cmd3.ExecuteScalar());
//         //            }
//         //            conn.Close();
//         //            string query4 = "SELECT SUM(PercentageHolding) FROM ResultsArchive WHERE  QuestionId='" + resolution.Id + "' AND Against = 'AGAINST'";
//         //            conn.Open();
//         //            SqlCommand cmd4 = new SqlCommand(query4, conn);
//         //            if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
//         //            {
//         //                percentHoldingAgainst = Convert.ToDouble(cmd4.ExecuteScalar());
//         //            }
//         //            conn.Close();

//         //            model.ResultForHoldingAgainst = holdingAgainst.ToString();
//         //            model.ResultForPercentHoldingAgainst = percentHoldingAgainst.ToString();

//         //            string query5 = "SELECT SUM(Holding) FROM ResultsArchive WHERE   QuestionId='" + resolution.Id + "' AND Abstain = 'ABSTAIN'";
//         //            conn.Open();
//         //            SqlCommand cmd5 = new SqlCommand(query5, conn);

//         //            if (!DBNull.Value.Equals(cmd5.ExecuteScalar()))
//         //            {
//         //                holdingAbstain = Convert.ToDouble(cmd5.ExecuteScalar());
//         //            }
//         //            conn.Close();
//         //            string query6 = "SELECT SUM(PercentageHolding) FROM ResultsArchive WHERE QuestionId='" + resolution.Id + "' AND Abstain = 'ABSTAIN'";
//         //            conn.Open();
//         //            SqlCommand cmd6 = new SqlCommand(query6, conn);
//         //            if (!DBNull.Value.Equals(cmd6.ExecuteScalar()))
//         //            {
//         //                percentHoldingAgainst = Convert.ToDouble(cmd6.ExecuteScalar());
//         //            }
//         //            conn.Close();

//         //            model.ResultForHoldingAbstain = holdingAbstain.ToString();
//         //            model.ResultForPercentHoldingAbstain = percentHoldingAbstain.ToString();

//         //            string query7 = "SELECT SUM(Holding) FROM ResultsArchive WHERE QuestionId='" + resolution.Id + "'";
//         //            conn.Open();
//         //            SqlCommand cmd7 = new SqlCommand(query7, conn);
//         //            if (!DBNull.Value.Equals(cmd7.ExecuteScalar()))
//         //            {
//         //                TotalHolding = Convert.ToDouble(cmd7.ExecuteScalar());
//         //            }
//         //            conn.Close();

//         //            model.TotalHolding = TotalHolding;
//         //            model.TotalPercentHolding = TotalPercentHolding.ToString();

//         //            //calcuate percentage holding per resolution..
//         //            var pFor = (holding / TotalHolding) * 100;
//         //            model.PercentageResultFor = pFor.ToString();
//         //            var pAgainst = (holdingAgainst / TotalHolding) * 100;
//         //            model.PercentageResultAgainst = pAgainst.ToString();
//         //            var pAbstain = (holdingAbstain / TotalHolding) * 100;
//         //            model.PercentageResultAbstain = pAbstain.ToString();
//         //            var sumAllPercentages = pFor + pAgainst + pAbstain;
//         //            model.TotalPercentageHolding = sumAllPercentages.ToString();
//         //        }
//         //    }

//         //    return Task.FromResult<ReportViewModelDto>(model);
//         //}



//         public async Task<JsonResult> Notify(int id, CompanyModel pmodel)
//         {

//             var response = await NotifyAsync(id, pmodel);

//             return Json(response, JsonRequestBehavior.AllowGet);

//         }

//         private Task<List<ChartResult>> NotifyAsync(int id, CompanyModel pmodel)
//         {
//             List<ChartResult> countitem = new List<ChartResult>();
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var UniqueAGMId = pmodel.AGMID;
//             var resolution = db.QuestionArchive.Find(id);
//             if (resolution != null)
//             {
//                 if (UniqueAGMId != -1)
//                 {
//                     ChartResult chartresult = new ChartResult
//                     {
//                         ForCount = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId && r.AGMID == UniqueAGMId && r.VoteFor == true).Count(),
//                         AgainstCount = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId && r.AGMID == UniqueAGMId && r.VoteAgainst == true).Count(),
//                         AbstainCount = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId && r.AGMID == UniqueAGMId && r.VoteAbstain == true).Count()
//                     };

//                     countitem.Add(chartresult);

//                 }
//             }


//             return Task.FromResult<List<ChartResult>>(countitem);
//         }


//         public async Task<ActionResult> ResolutionIndex()
//         {
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             var returnUrl = HttpContext.Request.Url.AbsolutePath;
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             //if (String.IsNullOrEmpty(companyinfo))
//             //{
//             //    return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl });
//             //}
//             var companies = db.SettingsArchive.Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList(); ;
//             ViewBag.AGMHistory = new SelectList(companies ?? new List<string>());
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var response = await ResolutionIndexAsync();

//             return PartialView(response);
//         }



//         [HttpPost]
//         public async Task<ActionResult> ResolutionIndex(CompanyModel pmodel)
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             IEnumerable<SettingsModel> AGMDb;
//             List<string> companies;
//             if (String.IsNullOrEmpty(pmodel.AGMID.ToString()))
//             {
//                 companies = db.SettingsArchive.Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList(); ;
//                 ViewBag.AGMHistory = new SelectList(companies ?? new List<string>());
//                 ViewBag.value = "tab";
//                 return View(new ReportViewModelDto());
//             }

//             companies = db.SettingsArchive.Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList(); ;
//             ViewBag.AGMHistory = new SelectList(companies ?? new List<string>());
//             ViewBag.value = "tab";

//             var response = await ResolutionIndexPostAsync(pmodel);

//             return PartialView(response);
//         }


//         public Task<ReportViewModelDto> ResolutionIndexAsync()
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();


//             //var currentYear = DateTime.Now.Year.ToString();

//             ReportViewModelDto model = new ReportViewModelDto();
//             model.resolutionsArchive = new List<QuestionArchive>();
//             //if (UniqueAGMId != -1)
//             //{
//             //    model.resolutionsArchive = db.QuestionArchive.Where(q => q.AGMID == UniqueAGMId).ToList();
//             //    model.presentcount = db.PresentArchive.Where(p => p.AGMID == UniqueAGMId).Count();
//             //}


//             //var shareholderCount = GetShareholders(UniqueAGMId);
//             //model.shareholders = shareholderCount;
//             //model.shareholders = db.BarcodeStore.Where(b => b.Company == companyinfo).Count();

//             return Task.FromResult<ReportViewModelDto>(model);
//         }

//         public Task<ReportViewModelDto> ResolutionIndexPostAsync(CompanyModel pmodel)
//         {

//             ReportViewModelDto model = new ReportViewModelDto();
//             if (pmodel.AGMID != -1)
//             {
//                 model.resolutionsArchive = db.QuestionArchive.Where(q => q.AGMID == pmodel.AGMID).ToList();
//                 model.presentcount = db.PresentArchive.Where(p => p.AGMID == pmodel.AGMID && p.PermitPoll == 1).Count();
//                 model.AGMID = pmodel.AGMID;
//             }

//             var shareholderCount = GetShareholders(pmodel.AGMID);
//             model.shareholders = shareholderCount;
            
//             //model.shareholders = db.BarcodeStore.Where(b => b.Company == companyinfo).Count();

//             return Task.FromResult<ReportViewModelDto>(model);
//         }


//         private int GetShareholders( int UniqueAGMID)
//         {
//             if(UniqueAGMID != -1)
//             {
//                 var setting = db.SettingsArchive.FirstOrDefault(s => s.AGMID == UniqueAGMID);
//                 return setting.TotalRecordCount;
//             }
//             return 0;
            

            
//         }


//         public async Task<ActionResult> ResolutionList(int id, CompanyModel pmodel)
//         {
//             var response = await ResolutionListAsync(id,pmodel);

//             return PartialView(response);
//         }




//         public Task<ReportViewModelDto> ResolutionListAsync(int id,CompanyModel pmodel)
//         {
//             Double holding = 0;
//             Double percentHolding = 0;
//             Double holdingAgainst = 0;
//             Double percentHoldingAgainst = 0;
//             Double holdingAbstain = 0;
//             Double percentHoldingAbstain = 0;
//             Double holdingVoid = 0;
//             Double percentHoldingVoid = 0;
//             //var companyinfo = ua.GetUserCompanyInfo();
//             //var UniqueAGMId = ua.RetrieveAGMUniqueID();
//             var UniqueAGMId = pmodel.AGMID;
//             ReportViewModelDto model = new ReportViewModelDto();
//             //Double TotalHolding = 0;
//             //Double TotalPercentHolding = 0;
//             var resolution = db.QuestionArchive.Find(id);
//             if (resolution != null)
//             {
//                 model.ResolutionName = resolution.question;
//                 if (UniqueAGMId != -1)
//                 {
//                     var agmEventSetting = db.SettingsArchive.SingleOrDefault(s => s.AGMID == UniqueAGMId);

//                     var abstainbtnchoice = true;
//                     if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
//                     {
//                         abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
//                     }
//                     model.abstainBtnChoice = abstainbtnchoice;
//                     model.ResultArchiveFor = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId &&  r.VoteFor == true).ToList();
//                     model.ResultArchiveAgainst = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId && r.VoteAgainst == true).ToList();
//                     if(abstainbtnchoice)
//                     {
//                         model.ResultArchiveAbstain = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId && r.VoteAbstain == true).ToList();
//                     }

//                     //model.resolutions = db.Question.Where(q => q.Company == companyinfo).ToList();


//                     var ResultForCount = model.ResultArchiveFor.Count();
//                     var ResultAgainstCount = model.ResultArchiveAgainst.Count();
//                     var ResultAbstainCount = model.ResultArchiveAbstain.Count();
//                     model.ResultForCount = ResultForCount;
//                     model.ResultAgainstCount = ResultAgainstCount;
//                     model.ResultAbstainCount = ResultAbstainCount;

//                     string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                     SqlConnection conn =
//                     new SqlConnection(connStr);
//                     //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//                     string query = "SELECT SUM(Holding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "' AND [VoteFor] = 1";
//                     conn.Open();
//                     SqlCommand cmd = new SqlCommand(query, conn);
//                     //object o = cmd.ExecuteScalar();
//                     if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                     {
//                         holding = Convert.ToDouble(cmd.ExecuteScalar());
//                     }
//                     conn.Close();
//                     string query1 = "SELECT SUM(PercentageHolding) FROM ResultArchives WHERE  QuestionId='" + resolution.QuestionId + "' AND [VoteFor] = 1";
//                     conn.Open();
//                     SqlCommand cmd1 = new SqlCommand(query1, conn);
//                     if (!DBNull.Value.Equals(cmd1.ExecuteScalar()))
//                     {
//                         percentHolding = Convert.ToDouble(cmd1.ExecuteScalar());
//                     }
//                     conn.Close();

//                     model.ResultForHolding = holding.ToString();
//                     model.ResultForPercentHolding = percentHolding.ToString();


//                     string query2 = "SELECT SUM(Holding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "' AND VoteAgainst = 1";
//                     conn.Open();
//                     SqlCommand cmd2 = new SqlCommand(query2, conn);
//                     if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                     {
//                         holdingAgainst = Convert.ToDouble(cmd2.ExecuteScalar());
//                     }
//                     conn.Close();
//                     string query3 = "SELECT SUM(PercentageHolding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "' AND VoteAgainst = 1";
//                     conn.Open();
//                     SqlCommand cmd3 = new SqlCommand(query3, conn);
//                     if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
//                     {
//                         percentHoldingAgainst = Convert.ToDouble(cmd3.ExecuteScalar());
//                     }
//                     conn.Close();

//                     model.ResultForHoldingAgainst = holdingAgainst.ToString();
//                     model.ResultForPercentHoldingAgainst = percentHoldingAgainst.ToString();

//                     if (abstainbtnchoice)
//                     {

//                         string query4 = "SELECT SUM(Holding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "' AND VoteAbstain = '1'";
//                         conn.Open();
//                         SqlCommand cmd4 = new SqlCommand(query4, conn);
//                         if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
//                         {
//                             holdingAbstain = Convert.ToDouble(cmd4.ExecuteScalar());
//                         }
//                         conn.Close();
//                         string query5 = "SELECT SUM(PercentageHolding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "' AND VoteAbstain = '1'";
//                         conn.Open();
//                         SqlCommand cmd5 = new SqlCommand(query5, conn);
//                         if (!DBNull.Value.Equals(cmd5.ExecuteScalar()))
//                         {
//                             percentHoldingAbstain = Convert.ToDouble(cmd5.ExecuteScalar());
//                         }
//                         conn.Close();

//                         model.ResultForHoldingAbstain = holdingAbstain.ToString();
//                         model.ResultForPercentHoldingAbstain = percentHoldingAbstain.ToString();
//                     }

//                     string query8 = "SELECT SUM(Holding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "' AND VoteVoid = 1";
//                     conn.Open();
//                     SqlCommand cmd8 = new SqlCommand(query8, conn);
//                     if (!DBNull.Value.Equals(cmd8.ExecuteScalar()))
//                     {
//                         holdingVoid = Convert.ToDouble(cmd8.ExecuteScalar());
//                     }
//                     conn.Close();
//                     string query9 = "SELECT SUM(PercentageHolding) FROM ResultArchives WHERE QuestionId='" + resolution.QuestionId + "' AND VoteVoid = 1";
//                     conn.Open();
//                     SqlCommand cmd9 = new SqlCommand(query9, conn);
//                     if (!DBNull.Value.Equals(cmd9.ExecuteScalar()))
//                     {
//                         percentHoldingVoid = Convert.ToDouble(cmd9.ExecuteScalar());
//                     }
//                     conn.Close();

//                     model.ResultForHoldingVoid = holdingVoid.ToString();
//                     model.ResultForPercentHoldingVoid = percentHoldingVoid.ToString();

//                 }
//             }

//             return Task.FromResult<ReportViewModelDto>(model);
//         }


//         public async Task<ActionResult> MobileIndex()
//         {

//             var returnUrl = HttpContext.Request.Url.AbsolutePath;
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             var companyinfo = ua.GetUserCompanyInfo();
//             if (String.IsNullOrEmpty(companyinfo))
//             {
//                 return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl });
//             }
//             var AGMDb = db.SettingsArchive.Where(s => s.CompanyName == companyinfo);
//             ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var response = await ChannelIndexAsync();

//             return PartialView(response);
//         }

//         public async Task<ActionResult> SMSIndex()
//         {
//             var returnUrl = HttpContext.Request.Url.AbsolutePath;
//             var companyinfo = ua.GetUserCompanyInfo();
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             if (String.IsNullOrEmpty(companyinfo))
//             {
//                 return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl });
//             }

//             var AGMDb = db.SettingsArchive.Where(s => s.CompanyName == companyinfo);
//             ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var response = await ChannelIndexAsync();

//             return PartialView(response);
//         }

//         public async Task<ActionResult> ClikapadIndex()
//         {
//             var returnUrl = HttpContext.Request.Url.AbsolutePath;
//             var companyinfo = ua.GetUserCompanyInfo();
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             if (String.IsNullOrEmpty(companyinfo))
//             {
//                 return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl });
//             }
//             var AGMDb = db.SettingsArchive.Where(s => s.CompanyName == companyinfo);
//             ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var response = await ChannelIndexAsync();

//             return PartialView(response);
//         }

//         public async Task<ActionResult> ProxyIndex()
//         {
//             var returnUrl = HttpContext.Request.Url.AbsolutePath;
//             var companyinfo = ua.GetUserCompanyInfo();
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             if (String.IsNullOrEmpty(companyinfo))
//             {
//                 return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl });
//             }
//             var AGMDb = db.SettingsArchive.Where(s => s.CompanyName == companyinfo);
//             ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var response = await ChannelIndexAsync();

//             return PartialView(response);
//         }

//         public async Task<ActionResult> AllChannelsIndex()
//         {
//             var returnUrl = HttpContext.Request.Url.AbsolutePath;
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();

//             var companyinfo = ua.GetUserCompanyInfo();
//             if (String.IsNullOrEmpty(companyinfo))
//             {
//                 return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl });
//             }
//             var AGMDb = db.SettingsArchive.Where(s => s.CompanyName == companyinfo);
//             ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var response = await ChannelIndexAsync();
//             return PartialView(response);
//         }

//         [HttpPost]
//         public async Task<ActionResult> AllChannelsIndex(CompanyModel pmodel)
//         {

//             var companyinfo = ua.GetUserCompanyInfo();
//             var AGMDb = db.SettingsArchive.Where(s => s.CompanyName == companyinfo);
//             //if (pmodel.AGMID == 0)
//             //{

//             //    ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
//             //    return View(new ReportViewModelDto());
//             //}

//             ViewBag.AGMHistory = new SelectList(AGMDb, "AGMID", "Title");
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());

//             var response = await ChannelIndexPostAsync(pmodel);
//             return PartialView(response);
//         }

//         private Task<ReportViewModelDto> ChannelIndexAsync()
//         {
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             var currentYear = DateTime.Now.Year.ToString();
//             ReportViewModelDto model = new ReportViewModelDto();
//             if (UniqueAGMId != -1)
//             {
//                 model.resolutionsArchive = db.QuestionArchive.Where(p => p.AGMID == UniqueAGMId).ToList();
//             }
//             model.User = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
//             //model.presentcount = db.Present.Where(p => p.Company == companyinfo).Count();
//             //model.shareholders = db.BarcodeStore.Where(p => p.Company == companyinfo).Count();

//             return Task.FromResult<ReportViewModelDto>(model);
//         }


//         private Task<ReportViewModelDto> ChannelIndexPostAsync(CompanyModel pmodel)
//         {
//             var companyinfo = ua.GetUserCompanyInfo();
//             ReportViewModelDto model = new ReportViewModelDto();
//             if (string.IsNullOrEmpty(pmodel.AGMID.ToString()))
//             {
//                 model.resolutionsArchive = db.QuestionArchive.Where(p => p.AGMID == pmodel.AGMID).ToList();
//             }
//             model.User = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
//             //model.presentcount = db.Present.Where(p => p.Company == companyinfo).Count();
//             //model.shareholders = db.BarcodeStore.Where(p => p.Company == companyinfo).Count();

//             return Task.FromResult<ReportViewModelDto>(model);
//         }





//         public async Task<ActionResult> MobileList(int id, CompanyModel pmodel)
//         {
//             string channel = "Mobile";
//             var response = await ChannelListAsync(id,pmodel, channel);


//             return PartialView(response);
//         }

//         public async Task<ActionResult> SMSList(int id, CompanyModel pmodel)
//         {
//             string channel = "SMS";
//             var response = await ChannelListAsync(id, pmodel, channel);

//             return PartialView(response);
//         }

//         public async Task<ActionResult> ClikapadList(int id, CompanyModel pmodel)
//         {
//             string channel = "Pad";
//             var response = await ChannelListAsync(id, pmodel, channel);

//             return PartialView(response);
//         }

//         public async Task<ActionResult> ProxyList(int id, CompanyModel pmodel)
//         {
//             string channel = "Proxy";
//             var response = await ChannelListAsync(id, pmodel, channel);

//             return PartialView(response);
//         }

//         public async Task<ActionResult> AllChannelsList(int id, CompanyModel pmodel)
//         {
//             string channel = "All";
//             var response = await ChannelListAsync(id, pmodel, channel);

//             return PartialView(response);
//         }

//         private Task<ReportViewModelDto> ChannelListAsync(int id, CompanyModel pmodel, string channel)
//         {
//             Double holding = 0;
//             Double percentHolding = 0;
//             Double holdingAgainst = 0;
//             Double percentHoldingAgainst = 0;
//             Double holdingAbstain = 0;
//             Double percentHoldingAbstain = 0;
//             Double holdingVoid = 0;
//             Double percentHoldingVoid = 0;
//             //var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = pmodel.AGMID;
//             ReportViewModelDto model = new ReportViewModelDto();
//             var resolution = db.QuestionArchive.Find(id);
//             if (resolution != null)
//             {
//                 ViewBag.ResolutionName = resolution.question;
//                 if (UniqueAGMId != -1)
//                 {
//                     var agmEventSetting = db.SettingsArchive.SingleOrDefault(s => s.AGMID == UniqueAGMId);
//                     var abstainbtnchoice = true;
//                     if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
//                     {
//                         abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
//                     }
//                     model.abstainBtnChoice = abstainbtnchoice;
//                     if (channel != "All")
//                     {
//                         model.ResultArchiveFor = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId &&  r.Source == channel && r.VoteFor == true).ToList();
//                         model.ResultArchiveAgainst = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId &&  r.Source == channel && r.VoteAgainst == true).ToList();
                       
//                         if(abstainbtnchoice)
//                         {
//                             model.ResultArchiveAbstain = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId && r.Source == channel && r.VoteAbstain == true).ToList();
//                             var ResultAbstainCount = model.ResultArchiveAbstain.Count();
//                             model.ResultAbstainCount = ResultAbstainCount;
//                         }
//                         model.ResultArchiveVoid = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId && r.Source == channel && r.VoteVoid == true).ToList();
//                         //model.resolutions = db.Question.Where(q => q.Company == companyinfo).ToList();
//                     }
//                     else
//                     {
//                         model.ResultArchiveFor = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId  && r.VoteFor == true).ToList();
//                         model.ResultArchiveAgainst = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId &&  r.VoteAgainst == true).ToList();
//                         if(abstainbtnchoice)
//                         {
//                             model.ResultArchiveAbstain = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId && r.VoteAbstain == true).ToList();
//                             var ResultAbstainCount = model.ResultArchiveAbstain.Count();
//                             model.ResultAbstainCount = ResultAbstainCount;
//                         }
//                         model.ResultArchiveVoid = db.ResultArchive.Where(r => r.QuestionId == resolution.QuestionId && r.Source == channel && r.VoteVoid == true).ToList();
//                         //model.resolutions = db.Question.Where(q => q.Company == companyinfo).ToList();
//                     }



//                     var ResultForCount = model.ResultArchiveFor.Count();
//                     var ResultAgainstCount = model.ResultArchiveAgainst.Count();
//                     var ResultVoidCount = model.ResultArchiveVoid.Count();

//                     model.ResultForCount = ResultForCount;
//                     model.ResultAgainstCount = ResultAgainstCount;
//                     model.ResultVoidCount = ResultVoidCount;
                   

//                     string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                     SqlConnection conn =
//                     new SqlConnection(connStr);
//                     //string query = "select * from Results WHERE QuestionId='" + resolution.Id + "' AND For = FOR";
//                     string query;
//                     if (channel != "All")
//                     {
//                         query = "SELECT SUM(Holding) FROM ResultsArchive WHERE QuestionId='" + resolution.QuestionId + "' AND Source='" + channel + "' AND [VoteFor] = 1";
//                     }
//                     else
//                     {
//                         query = "SELECT SUM(Holding) FROM ResultsArchive WHERE QuestionId='" + resolution.QuestionId + "' AND [VoteFor] = 1";
//                     }

//                     conn.Open();
//                     SqlCommand cmd = new SqlCommand(query, conn);
//                     //object o = cmd.ExecuteScalar();
//                     if (!DBNull.Value.Equals(cmd.ExecuteScalar()))
//                     {
//                         holding = Convert.ToDouble(cmd.ExecuteScalar());
//                     }
//                     conn.Close();
//                     string query1;
//                     if (channel != "All")
//                     {
//                         query1 = "SELECT SUM(PercentageHolding) FROM ResultsArchive WHERE  QuestionId='" + resolution.QuestionId + "'AND Source='" + channel + "' AND [VoteFor] = 1";
//                     }
//                     else
//                     {
//                         query1 = "SELECT SUM(PercentageHolding) FROM ResultsArchive WHERE QuestionId='" + resolution.QuestionId + "' AND [VoteFor] = 1";
//                     }

//                     conn.Open();
//                     SqlCommand cmd1 = new SqlCommand(query1, conn);
//                     if (!DBNull.Value.Equals(cmd1.ExecuteScalar()))
//                     {
//                         percentHolding = Convert.ToDouble(cmd1.ExecuteScalar());
//                     }
//                     conn.Close();

//                     model.ResultForHolding = holding.ToString();
//                     model.ResultForPercentHolding = percentHolding.ToString();

//                     string query2;
//                     if (channel != "All")
//                     {
//                         query2 = "SELECT SUM(Holding) FROM ResultsArchive WHERE QuestionId='" + resolution.QuestionId + "' AND Source='" + channel + "' AND VoteAgainst = 1";
//                     }
//                     else
//                     {
//                         query2 = "SELECT SUM(Holding) FROM ResultsArchive WHERE QuestionId='" + resolution.QuestionId + "' AND  VoteAgainst = 1";
//                     }
//                     conn.Open();
//                     SqlCommand cmd2 = new SqlCommand(query2, conn);
//                     if (!DBNull.Value.Equals(cmd2.ExecuteScalar()))
//                     {
//                         holdingAgainst = Convert.ToDouble(cmd2.ExecuteScalar());
//                     }
//                     conn.Close();
//                     string query3;
//                     if (channel != "All")
//                     {
//                         query3 = "SELECT SUM(PercentageHolding) FROM ResultsArchive WHERE  QuestionId='" + resolution.QuestionId + "' AND Source='" + channel + "' AND VoteAgainst = 1";
//                     }
//                     else
//                     {
//                         query3 = "SELECT SUM(PercentageHolding) FROM ResultsArchive WHERE QuestionId='" + resolution.QuestionId + "' AND  VoteAgainst = 1";
//                     }

//                     conn.Open();
//                     SqlCommand cmd3 = new SqlCommand(query1, conn);
//                     if (!DBNull.Value.Equals(cmd3.ExecuteScalar()))
//                     {
//                         percentHoldingAgainst = Convert.ToDouble(cmd3.ExecuteScalar());
//                     }
//                     conn.Close();

//                     model.ResultForHoldingAgainst = holdingAgainst.ToString();
//                     model.ResultForPercentHoldingAgainst = percentHoldingAgainst.ToString();

//                     if(abstainbtnchoice)
//                     {
//                         string query4;
//                         if (channel != "All")
//                         {
//                             query4 = "SELECT SUM(Holding) FROM ResultsArchive WHERE QuestionId='" + resolution.QuestionId + "' AND Source='" + channel + "' AND VoteAbstain = 1";
//                         }
//                         else
//                         {
//                             query4 = "SELECT SUM(Holding) FROM ResultsArchive WHERE  QuestionId='" + resolution.QuestionId + "' AND  VoteAbstain = 1";
//                         }

//                         conn.Open();
//                         SqlCommand cmd4 = new SqlCommand(query4, conn);
//                         if (!DBNull.Value.Equals(cmd4.ExecuteScalar()))
//                         {
//                             holdingAbstain = Convert.ToDouble(cmd4.ExecuteScalar());
//                         }
//                         conn.Close();
//                         string query5;
//                         if (channel != "All")
//                         {
//                             query5 = "SELECT SUM(PercentageHolding) FROM ResultsArchive WHERE QuestionId='" + resolution.QuestionId + "' AND Source='" + channel + "' AND VoteAbstain = 1";
//                         }
//                         else
//                         {
//                             query5 = "SELECT SUM(PercentageHolding) FROM ResultsArchive WHERE  QuestionId='" + resolution.QuestionId + "' AND VoteAbstain = 1";
//                         }
//                         conn.Open();
//                         SqlCommand cmd5 = new SqlCommand(query5, conn);
//                         if (!DBNull.Value.Equals(cmd5.ExecuteScalar()))
//                         {
//                             percentHoldingAbstain = Convert.ToDouble(cmd5.ExecuteScalar());
//                         }
//                         conn.Close();
//                         model.ResultForHoldingAbstain = holdingAbstain.ToString();
//                         model.ResultForPercentHoldingAbstain = percentHoldingAbstain.ToString();
//                     }

//                     string query8;
//                     if (channel != "All")
//                     {
//                         query8 = "SELECT SUM(Holding) FROM ResultsArchive WHERE QuestionId='" + resolution.QuestionId + "' AND Source='" + channel + "' AND VoteVoid = 1";
//                     }
//                     else
//                     {
//                         query8 = "SELECT SUM(Holding) FROM ResultsArchive WHERE QuestionId='" + resolution.QuestionId + "' AND  VoteVoid = 1";
//                     }
//                     conn.Open();
//                     SqlCommand cmd8 = new SqlCommand(query8, conn);
//                     if (!DBNull.Value.Equals(cmd8.ExecuteScalar()))
//                     {
//                         holdingVoid = Convert.ToDouble(cmd8.ExecuteScalar());
//                     }
//                     conn.Close();
//                     string query9;
//                     if (channel != "All")
//                     {
//                         query9 = "SELECT SUM(PercentageHolding) FROM ResultsArchive WHERE  QuestionId='" + resolution.QuestionId + "' AND Source='" + channel + "' AND VoteVoid = 1";
//                     }
//                     else
//                     {
//                         query9 = "SELECT SUM(PercentageHolding) FROM ResultsArchive WHERE QuestionId='" + resolution.QuestionId + "' AND  VoteVoid = 1";
//                     }

//                     conn.Open();
//                     SqlCommand cmd9 = new SqlCommand(query1, conn);
//                     if (!DBNull.Value.Equals(cmd9.ExecuteScalar()))
//                     {
//                         percentHoldingVoid = Convert.ToDouble(cmd9.ExecuteScalar());
//                     }
//                     conn.Close();

//                     model.ResultForHoldingVoid = holdingVoid.ToString();
//                     model.ResultForPercentHoldingVoid = percentHoldingVoid.ToString();

//                 }
//             }

//             return Task.FromResult<ReportViewModelDto>(model);
//         }
//     }
// }