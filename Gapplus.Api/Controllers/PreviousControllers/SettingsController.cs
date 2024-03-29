﻿// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;
// using BarcodeGenerator.Util;
// using System;
// using System.Collections.Generic;
// using System.Configuration;
// using System.Data;
// using System.Data.Entity;
// using System.Data.SqlClient;
// using System.Linq;
// using System.Net.Mail;
// using System.Text;
// using System.Threading.Tasks;
// using System.Web;
// using System.Web.Mvc;

// namespace BarcodeGenerator.Controllers
// {
//     public class SettingsController : Controller
//     {
//         //
//         // GET: /Settings/
//         UsersContext db = new UsersContext();
        
//         //UserAdmin ua = new UserAdmin();

//         //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

//         //private int RetrieveAGMUniqueID()
//         //{
//         //    var AGMUniqueID = db.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

//         //    return AGMUniqueID;
//         //}

//         private string currentYear = DateTime.Now.Year.ToString();



//         public ActionResult Index()
//         {
//             //var companyinfo = ua.GetUserCompanyInfo();

//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             var mailserver = new SelectList(new[] { "Mail Server 1", "Mail Server 2" });
//             ViewBag.mailserver = mailserver;
//             var year = db.Settings.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             ViewBag.Year = new SelectList(year ?? new List<string>());
//             var companies = db.Settings.Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList();
//             ViewBag.companyfilter = new SelectList(companies ?? new List<string>());
//             //var dbcompanies = db.BarcodeStore.Select(b => b.Company).Distinct().OrderBy(k => k).ToList();
//             //var proxycompanies = db.ProxyList.Select(p => p.Company).Distinct().OrderBy(k => k).ToList();
//             SettingsViewModel model = new SettingsViewModel {
//                 settingsModel = db.Settings.Where(s => s.Year == currentYear).OrderByDescending(s => s.AGMID).ToList(),
//                 //Proxylistmodel = db.ProxyList.ToList(),
//                 user = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name.Trim()),
//                 //companiesUploaded = dbcompanies ?? new List<string>() 
//             //    companiesUploaded = dbcompanies,
//             //proxylistUploaded = proxycompanies 
//                 //Proxylistmodel = db.ProxyList.Where.ToList()
//             };

//             return PartialView(model);
//         }


//         [HttpPost]
//         public async Task<ActionResult> Index(CompanyModel pmodel)
//         {
//             var returnUrl = HttpContext.Request.Url.AbsolutePath;
//             //var companyinfo = ua.GetUserCompanyInfo();
//             if (String.IsNullOrEmpty(pmodel.Year))
//             {
//                 return View(new SettingsViewModel());
//             }
//             var year = db.Settings.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             ViewBag.Year = new SelectList(year ?? new List<string>());
//             var companies = db.Settings.Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList();
//             ViewBag.companyfilter = new SelectList(companies ?? new List<string>());
//             var mailserver = new SelectList(new[] { "Mail Server 1", "Mail Server 2" });
//             ViewBag.mailserver = mailserver;
//             var response = await IndexPostAsync(pmodel);

//             return PartialView(response);
//         }


//         private Task<SettingsViewModel> IndexPostAsync(CompanyModel pmodel)
//         {
//             //ReportViewModelDto model = new ReportViewModelDto();
//             //var companyinfo = ua.GetUserCompanyInfo();
//             SettingsViewModel model = new SettingsViewModel
//             {
//                 settingsModel = db.Settings.Where(p => p.Year == pmodel.Year.Trim()).OrderByDescending(s=>s.AGMID).ToList(),
//                 Proxylistmodel = db.ProxyList.ToList(),
//                 user = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name.Trim()),
           
//             };
//             //model.shareholders = db.BarcodeStore.Where(p => p.Company == companyinfo).Count();
//             return Task.FromResult<SettingsViewModel>(model);
//         }
//         //
//         // GET: /Settings/Details/5

//         public ActionResult Details(int id)
//         {
//             var setting = db.Settings.Find(id);
//             return PartialView(setting);
//         }


//         public async Task<bool> GetAGMStartStatus(string id)
//         {
//             var company = id;
//             var response = await GetAGMStartStatusAsync(company);
//             return response;
//         }


//         private Task<bool> GetAGMStartStatusAsync(string company)
//         {
//             var value = AutoManager.CheckAGMStart(company);

//             return Task.FromResult<bool>(value);
//         }


//         public async Task<bool> GetVotingStatus(string id)
//         {
//             var company = id;
//             var response = await GetVotingStatusAsync(company);
//             return response;
//         }


//         private Task<bool> GetVotingStatusAsync(string company)
//         {
//             var value = TimerControll.GetTimeStatus(company);

//             return Task.FromResult<bool>(value);
//         }
//         [HttpPost]
//         public async Task<string> AGMStartStatus(int id, QuestionStatus data)
//         {
//             var response = await AGMStartStatusAsync(id, data);

//             return response;
//         }


//         private Task<string> AGMStartStatusAsync(int id, QuestionStatus data)
//         {
//             try
//             {
//                 var setting = db.Settings.Find(id);
//                 if (setting != null)
//                 {
//                     if (data.status)
//                     {
//                         setting.AgmStart = true;
//                         Functions.RefreshPages(setting.CompanyName,"AGM has started");

//                     }
//                     else
//                     {
//                         setting.AgmStart = false;
//                         Functions.RefreshPages(setting.CompanyName,"AGM was stopped");
//                     }

//                     db.SaveChanges();

//                     return Task.FromResult<string>("Success");
//                 }
//                 else
//                 {
//                     return Task.FromResult<string>("failure");
//                 }
//             }
//             catch(Exception e)
//             {
//                 return Task.FromResult<string>(e.StackTrace.ToString());
//             }

//         }



//         [HttpPost]
//         public async Task<string> AGMEndStatus(int id, QuestionStatus data)
//         {
//             var response = await AGMEndStatusAsync(id, data);

//             return response;
//         }


//         private Task<string> AGMEndStatusAsync(int id, QuestionStatus data)
//         {
//             try
//             {
//                 var setting = db.Settings.Find(id);
//                 if (setting != null)
//                 {
//                     if (data.status)
//                     {
//                         setting.AgmEnd = true;

//                     }
//                     else
//                     {
//                         setting.AgmEnd = false;
//                     }

//                     db.SaveChanges();

//                     return Task.FromResult<string>("Success");
//                 }
//                 else
//                 {
//                     return Task.FromResult<string>("failure");
//                 }
//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<string>(e.StackTrace.ToString());
//             }

//         }

//         [HttpPost]
//         public async Task<string> AGMStartAdmittanceStatus(int id, QuestionStatus data)
//         {
//             var response = await AGMStartAdmittanceStatusAsync(id, data);

//             return response;
//         }


//         private Task<string> AGMStartAdmittanceStatusAsync(int id, QuestionStatus data)
//         {
//             try
//             {
//                 var setting = db.Settings.Find(id);
//                 if (setting != null)
//                 {
//                     if (data.status)
//                     {
//                         setting.StartAdmittance = true;

//                     }
//                     else
//                     {
//                         setting.StartAdmittance = false;
//                     }

//                     db.SaveChanges();

//                     return Task.FromResult<string>("Success");
//                 }
//                 else
//                 {
//                     return Task.FromResult<string>("failure");
//                 }
//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<string>(e.StackTrace.ToString());
//             }

//         }




//         [HttpPost]
//         public async Task<string> AGMAdmittanceStatus(int id, QuestionStatus data)
//         {
//             var response = await AGMAdmittanceStatusAsync(id, data);

//             return response;
//         }


//         private Task<string> AGMAdmittanceStatusAsync(int id, QuestionStatus data)
//         {
//             try
//             {
//                 var setting = db.Settings.Find(id);
//                 if (setting != null)
//                 {
//                     if (data.status)
//                     {
//                         setting.StopAdmittance = true;

//                     }
//                     else
//                     {
//                         setting.StopAdmittance = false;
//                     }

//                     db.SaveChanges();

//                     return Task.FromResult<string>("Success");
//                 }
//                 else
//                 {
//                     return Task.FromResult<string>("failure");
//                 }
//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<string>(e.StackTrace.ToString());
//             }

//         }
//         //
//         // GET: /Settings/Create

//         public ActionResult Create()
//         {
//             SettingsModel model = new SettingsModel();
//             return View(model);
//         }

//         //
//         // POST: /Settings/Create

//         [HttpPost]
//         public ActionResult Create(HttpPostedFileBase file)
//         {
//             try
//             {
//                 // TODO: Add insert logic here
//                 var setting = db.Settings.ToArray();
//                 if (setting.Length == 0 || setting.Length < 1 )
//                 {
                
//                     //string ImageName = System.IO.Path.GetFileName(file.FileName);
//                     //string physicalPath = Server.MapPath("~/LogoFile/" + ImageName);

//                     //// save image in folder
//                     //file.SaveAs(physicalPath);

//                     SettingsModel model = new SettingsModel();
//                     model.Title = Request.Form["Title"];
//                     model.ShareHolding = double.Parse(Request.Form["ShareHolding"]);
//                     model.CompanyName = Request.Form["CompanyName"];
//                     model.Location = Request.Form["Location"];
//                     model.When = Request.Form["When"];
//                     model.feebackEmailAddress = Request.Form["feebackEmailAddress"];
//                     model.feebackCCEmailAddress = Request.Form["feebackCCEmailAddress"];
//                     //model.logo = "LogoFile/" + ImageName;
//                     if (file != null)
//                     {
//                         model.ImageSource = file.ContentType;
//                         model.Image = new byte[file.ContentLength];
//                         file.InputStream.Read(model.Image, 0, file.ContentLength);
//                     }

//                     db.Settings.Add(model);
//                     db.SaveChanges();
//                     return RedirectToAction("Index");
//                 }
//                 TempData["Message"] = "Modify existing Setting";
//                 return RedirectToAction("Index");

//             }
//             catch(Exception e)
//             {
//                 TempData["Message"] = "Couldnt Save Settings";
//                 return View();
//             }
//         }

//         //
//         // GET: /Settings/Edit/5

//         public ActionResult Edit(int id)
//         {
//             //var companies = db.Question.Select(o => o.Year).Distinct().OrderBy(k => k).ToList();
//             //ViewBag.Year = new SelectList(companies ?? new List<string>());
//             var setting = db.Settings.Find(id);
//             return PartialView(setting);
//         }

//         //
//         // POST: /Settings/Edit/5

//         [HttpPost]
//         public ActionResult Edit(int id, SettingsModel collection)
//         {
//             try
//             {
//                 // TODO: Add update logic here
//                 if(!ModelState.IsValid)
//                 {
//                     return RedirectToAction("Index");
//                 }
//                 var model = db.Settings.Find(collection.Id);
//                 //model.CompanyName = collection.CompanyName;
//                 //model.feebackEmailAddress = collection.feebackEmailAddress;
//                 //model.ShareHolding = collection.ShareHolding;
//                 model.Title = collection.Title;
//                 model.RegCode = collection.RegCode;
//                 model.ProxyVoteResult = collection.ProxyVoteResult;
//                 model.PrintOutTitle = collection.PrintOutTitle;
//                 model.Address = collection.Address;
//                 model.Venue = collection.Venue;
//                 model.VoteForColorBg = collection.VoteForColorBg;
//                 model.VoteAgainstColorBg = collection.VoteAgainstColorBg;
//                 model.VoteAbstaincolorBg = collection.VoteAbstaincolorBg;
//                 model.VoteVoidColorBg = collection.VoteVoidColorBg;
//                 if(!string.IsNullOrEmpty(collection.OnlineUrllink))
//                 {
//                     if (!collection.OnlineUrllink.Contains("?autoplay=1"))
//                     {
//                         model.OnlineUrllink = collection.OnlineUrllink + "?autoplay=1";
//                         Functions.RefreshPages(model.CompanyName, "AGM Streaming link is available");
//                     }
//                     else
//                     {
//                         model.OnlineUrllink = collection.OnlineUrllink;
//                         Functions.RefreshPages(model.CompanyName,"AGM Streaming link is available");
//                     }
//                 }
               
//                 model.Description = collection.Description;
//                 if(collection.AgmDateTime!=null)
//                 {
//                     model.AgmDateTime = collection.AgmDateTime;
//                     var currentTime = DateTime.Now;
//                     if (!(collection.AgmDateTime < currentTime))
//                     {
//                        if(!BackEndTimer.IsRunning(model.CompanyName))
//                         {
//                             BackEndTimer.SetTimer(model.CompanyName);
//                         }
                        
//                     }
//                 }
//                 if (collection.AdmittanceDateTime != null)
//                 {
//                     model.AdmittanceDateTime= collection.AdmittanceDateTime;
//                     var currentTime = DateTime.Now;
//                     if (!(collection.AdmittanceDateTime < currentTime))
//                     {
//                         if (!AdmittanceTimer.IsRunning(model.CompanyName))
//                         {
//                             AdmittanceTimer.SetTimer(model.CompanyName);
//                         }

//                     }
//                 }
//                 if(collection.StartVoting == true)
//                 {
//                     model.StartVoting = collection.StartVoting;
//                     model.StopVoting = false;
//                 }
//                 else
//                 {
//                     model.StartVoting = collection.StartVoting;
//                 }
//                if(collection.StopVoting == true)
//                 {
//                     model.StopVoting = collection.StopVoting;
//                     model.StartVoting = false;
//                 }
//                 else
//                 {
//                     model.StopVoting = collection.StopVoting;
//                 }
             
//                 model.allChannels = collection.allChannels;
                
//                 model.webChannel = collection.webChannel;
//                 if (model.allChannels|| model.webChannel)
//                 {
//                     ResetPermitPollStatus(true, model.AGMID);
//                 }
//                 else
//                 {
//                     ResetPermitPollStatus(false, model.AGMID);
//                 }
//                 model.smsChannel = collection.smsChannel;
//                 model.mobileChannel = collection.mobileChannel;
//                 if(collection.AbstainBtnChoice== null|| collection.AbstainBtnChoice==false)
//                 {
//                     model.AbstainBtnChoice = false;
//                 }
//                else if(collection.AbstainBtnChoice == true)
//                 {
//                     model.AbstainBtnChoice = true;
//                 }
//                 model.SyncChoice = collection.SyncChoice;
//                 model.MessagingChoice = collection.MessagingChoice;
//                 model.PreregisteredVotes = collection.PreregisteredVotes;
//                 model.CountDownValue = collection.CountDownValue;
//                 //model.When = collection.Location;
//                 //model.feebackCCEmailAddress = collection.feebackCCEmailAddress;
                
//                 db.Entry(model).State = EntityState.Modified;
//                 db.SaveChanges();
//                 return RedirectToAction("Index");
//             }
//             catch
//             {
//                 return View();
//             }
//         }

//         [HttpPost]
//         public string  UpdateChannels(PostValue post )
//         {
//             if(!ModelState.IsValid)
//             {
//                 return "Failed";
//             }
//             if(post!=null && post.agmid!=0)
//             {
//                 var agmSetting = db.Settings.SingleOrDefault(s => s.AGMID == post.agmid);
//                 if(agmSetting!=null)
//                 {
//                     if(post.cchannel=="all")
//                     {
//                         agmSetting.allChannels = post.status;
//                         ResetPermitPollStatus(post.status, post.agmid);
//                     }
//                     else if(post.cchannel == "mobile")
//                     {
//                         agmSetting.mobileChannel = post.status;
//                         ResetPermitPollStatus(post.status, post.agmid);
//                     }
//                     else if(post.cchannel == "sms")
//                     {
//                         agmSetting.smsChannel = post.status;
//                     }
//                     else if(post.cchannel == "web")
//                     {
//                         agmSetting.webChannel = post.status;
//                     }
//                     db.SaveChanges();
//                     return "success";
//                 }

//             }
//             return "failed";
//         }


//         private void ResetPermitPollStatus(bool status, int agmid)
//         {
//             if(status)
//             {
//                 var allwebvoters = db.Present.Where(p => p.AGMID == agmid && p.admitSource == "Web" && p.PermitPoll == 0).ToList();
//                 if(allwebvoters.Any())
//                 {
//                     foreach(var item in allwebvoters)
//                     {
//                         item.PermitPoll = 1;
//                     }
//                     db.SaveChanges();
//                 }
//             }
//             else
//             {
//                 var allwebvoters = db.Present.Where(p => p.AGMID == agmid && p.admitSource == "Web" && p.PermitPoll == 1).ToList();
//                 if (allwebvoters.Any())
//                 {

//                     foreach (var item in allwebvoters)
//                     {
//                         item.PermitPoll = 0;
//                         var result = db.Result.Where(r => r.AGMID == agmid && r.ShareholderNum == item.ShareholderNum).ToArray();
//                         for (int i = 0; i < result.Length; i++)
//                         {
//                             db.Result.Remove(result[i]);

//                         }
//                     }
//                     db.SaveChanges();
//                 }
//             }
//         }



//         public ActionResult EditImage(int id)
//         {
//             var image = db.Settings.Find(id);


//             return PartialView(image);
//         }

//         [HttpPost]
//         public ActionResult EditImage(HttpPostedFileBase file)
//         {
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     if (file != null)
//                     {
//                         var id = int.Parse(Request.Form["Id"].ToString());
//                         var logo = db.Settings.Find(id);
//                         logo.ImageSource = file.ContentType;
//                         logo.Image= new byte[file.ContentLength];
//                         file.InputStream.Read(logo.Image, 0, file.ContentLength);
//                         db.Entry(logo).State = EntityState.Modified;

//                     }
//                     else
//                     {
//                         var id = int.Parse(Request.Form["Id"].ToString());
//                         var logo = db.Settings.Find(id);
//                         logo.ImageSource = null;
//                         logo.Image = null;

//                         db.Entry(logo).State = EntityState.Modified;
//                     }

//                     db.SaveChanges();
//                     return RedirectToAction("Index");
//                 }
//                 catch (Exception e)
//                 {
//                     return RedirectToAction("Index");
//                 }
//             }
//             return RedirectToAction("Index");
//         }
//         //
//         // GET: /Settings/Delete/5

//         public ActionResult Delete(int id)
//         {
//             var setting = db.Settings.Find(id);
//             db.Settings.Remove(setting);
//             db.SaveChanges();
//             return RedirectToAction("Index");
//         }

//         //
//         // POST: /Settings/Delete/5

//         [HttpPost]
//         public ActionResult Delete(int id, FormCollection collection)
//         {
//             try
//             {
//                 // TODO: Add delete logic here

//                 return RedirectToAction("Index");
//             }
//             catch
//             {
//                 return View();
//             }
//         }


//         public ActionResult Download()
//         {
//             string fileLocation = "";
//             var fileName = ConfigurationManager.AppSettings["FileName"];
//             fileLocation = Server.MapPath("~/Uploads/Template/") + fileName;
//             if (System.IO.File.Exists(fileLocation))
//             {
//                 byte[] fileBytes = System.IO.File.ReadAllBytes(fileLocation);

//                 return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
//             }
//            return new EmptyResult();
//         }

//         public ActionResult ProxyListDownload()
//         {
//             string fileLocation = "";
//             var fileName = ConfigurationManager.AppSettings["ProxylistFileName"];
//             fileLocation = Server.MapPath("~/Uploads/Template/") + fileName;
//             if (System.IO.File.Exists(fileLocation))
//             {
//                 byte[] fileBytes = System.IO.File.ReadAllBytes(fileLocation);

//                 return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
//             }
//             return new EmptyResult();
//         }

//         public ActionResult FacilitatorListDownload()
//         {
//             string fileLocation = "";
//             var fileName = ConfigurationManager.AppSettings["FacilitatorlistFileName"];
//             fileLocation = Server.MapPath("~/Uploads/Template/") + fileName;
//             if (System.IO.File.Exists(fileLocation))
//             {
//                 byte[] fileBytes = System.IO.File.ReadAllBytes(fileLocation);

//                 return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
//             }
//             return new EmptyResult();
//         }
//         //public void SendMail()
//         //{
//         //    MailMessage message = new MailMessage();
//         //    //MailAddress Sender = new MailAddress(ConfigurationManager.AppSettings["smtpUser"]);
//         //    //MailAddress receiver = new MailAddress(txtemail.Text);
//         //    SmtpClient smtp = new SmtpClient()
//         //    {
//         //        Host = ConfigurationManager.AppSettings["smtpServer"],
//         //        Port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]),
//         //        EnableSsl = true,

//         //        Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtpUser"], ConfigurationManager.AppSettings["smtpPass"])

//         //    };
//         //    message.From = new MailAddress(ConfigurationManager.AppSettings["smtpUser"], "\\Shareholder Name\\");
//         //    //message.Sender = new MailAddress("johnfemy@yahoo.com", "Shareholder Name");
//         //    message.ReplyToList.Add(new MailAddress("Johnfemy@yahoo.com"));

//         //    message.To.Add("solution1@unitedsecuritieslimited.com");
//         //    message.Subject = "Mail Response Test";
//         //    message.Body = "Testing Mail Delivery";
//         //    message.IsBodyHtml = true;
//         //    smtp.Send(message);
//         //}
//     }
// }
