using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace BarcodeGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RegisterController : ControllerBase
    {
        //
        // GET: /Register/
         
        private readonly ITempDataManager _tempDataManager;
        private readonly IViewBagManager _viewBagManager;

        public RegisterController(UsersContext _db,ITempDataManager tempDataManager,IViewBagManager viewBagManager)
        {
            _viewBagManager = viewBagManager;
            _tempDataManager = tempDataManager;
            db=_db;
     ua = new UserAdmin(db);
        }
         UsersContext db;
        UserAdmin ua;

        //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

        //private static int RetrieveAGMUniqueID()
        //{
        //    UsersContext adb = new UsersContext();
        //    var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

        //    return AGMID;
    //}

        //private static int UniqueAGMId = RetrieveAGMUniqueID();

        //[HttpPost]
        //public ActionResult Index(SearchModel find)
        //{

        //    if (find.search == null)
        //    {

        //        return RedirectToAction("Empty");
        //    }
        //        try
        //        {
        //            var barcode = db.BarcodeStore.SingleOrDefault(u => u.Barcode == find.search);
        //       if (barcode != null)
        //        {
        //            BarcodeViewModel model = new BarcodeViewModel
        //            {
        //                id = barcode.Id,
        //                Name = barcode.Name,
        //                Holding = barcode.Holding,
        //                PercentageHolding = barcode.PercentageHolding,
        //                ShareholderNum = barcode.ShareholderNum,
        //                Barcode = barcode.Barcode,
        //                Address = barcode.Address,
        //                BarcodeImage = barcode.BarcodeImage,
        //                ImageUrl = barcode.BarcodeImage != null ? "data:image/jpg;base64," +
        //                    Convert.ToBase64String((byte[])barcode.BarcodeImage) : ""

        //            };
        //            ViewBag.Message = "SHAREHOLDER VERIFIED OK";
        //            var setting = db.Settings.First();
        //            ViewBag.logo = setting.logo;
        //            ViewBag.Company = setting.CompanyName;
        //            //barcode.Present = true;
        //            barcode.Date = DateTime.Today;
        //            db.Entry(barcode).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return PartialView(model);
        //       }

        //            return RedirectToAction("Wrong");
        //        }
        //        catch 
        //        {
        //            ViewBag.Message = "COULDNOT VERIFY SHAREHOLDER";
        //            return RedirectToAction("UserDetails","BarcodeLib");
        //        }



        //}

        [HttpPost]
        //  [ValidateAntiForgeryToken]
         public async Task<ActionResult> Index(SearchModel find)
         {

             if (find.search == null)
             {

                 return RedirectToAction("Empty");
             }
             try
             {
                var companyinfo = ua.GetUserCompanyInfo();
                var barcode = db.BarcodeStore.SingleOrDefault(u =>u.Company==companyinfo && u.PhoneNumber == find.search);
                 if (barcode != null)
                 {
                     var setting = db.Settings.ToArray();
                     string companyName;
                     if (setting.Length != 0)
                     {
                         companyName = setting[0].CompanyName;
                     }
                     else 
                     {
                          companyName = "";
                     }
                    
                     string name = barcode.Name;
                     var number = barcode.ShareholderNum;
                     string ImageUrl = barcode.BarcodeImage != null ? "data:image/jpg;base64," +
                     Convert.ToBase64String((byte[])barcode.BarcodeImage) : "";
                     string logo = "http://localhost:59373/Images/unitedSecuritiesLogo.png";
                     //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                     var body = @"<div style='margin-top:100px;'><div style='position:relative;background-color:white;width:350px;height:350px;margin:20px 5px 2px 0px;padding:20px;'>
            <img src='{0}' style='position:relative;float:right;width:100px;margin-left:40px;'/><div style='padding-left:10px;padding-top:10px;border-top:1px solid #4800ff;'><p style='font-weight:bold;'> {1} </p><p style='font-weight:bold;'>{2}
            </p>
            <p style='font-weight:bold;'>
               {3}
            </p>
            </div>
          <div style='width:100%;align-content:center;border-bottom:1px solid #4800ff;'>    
             <img src='cid:EmbeddedContent_1' />
           </div> 
    </div>
        </div>";
                     MailSettings mailsetting = db.mailsettings.First();
                     System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(mailsetting.Username, mailsetting.Password);
                     var message = new MailMessage();
                     message.To.Add(new MailAddress(barcode.emailAddress));  // replace with valid value 
                     message.From = new MailAddress(mailsetting.SentFrom);  // replace with valid value
                     message.Subject = "ShareHolder Barcode Registration Details";
                     message.Body = string.Format(body, logo, companyName, name, number, ImageUrl);
                     message.IsBodyHtml = true;

                     //Code to embed barcode image from database to sent image
                     AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                                                   message.Body,
                                                   Encoding.UTF8,
                                                   MediaTypeNames.Text.Html);
                     AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                                                  Regex.Replace(message.Body,
                                                                "<[^>]+?>",
                                                                string.Empty),
                                                  Encoding.UTF8,
                                                  MediaTypeNames.Text.Plain);

                     MemoryStream fileStream = new
                     MemoryStream(barcode.BarcodeImage);
                     string mediaType = MediaTypeNames.Image.Jpeg;
                     LinkedResource img = new LinkedResource(fileStream);

                     img.ContentId = "EmbeddedContent_1";
                     img.ContentType.MediaType = mediaType;
                     img.TransferEncoding = TransferEncoding.Base64;
                     img.ContentType.Name = img.ContentId;
                     img.ContentLink = new Uri("cid:" + img.ContentId);
                     htmlView.LinkedResources.Add(img);
                     message.AlternateViews.Add(plainView);
                     message.AlternateViews.Add(htmlView);
                     //End of embedding barcode Image

                     //Send Image Asynchronously...
                     using (var smtp = new SmtpClient())
                     {
                         smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                         smtp.EnableSsl = true;
                         smtp.Host = mailsetting.smtpHost;
                         smtp.Credentials = credentials;
                         smtp.Port = mailsetting.smtpPort;
                         await smtp.SendMailAsync(message);
                        //  TempData["Message"] = "Barcode Information has been sent to your email";
                         _tempDataManager.SetTempData("Message","Barcode Information has been sent to your email");

                         //barcode.PhoneNumber = model.PhoneNumber;
                         //barcode.emailAddress = model.FromEmail;
                         //db.Entry(barcode).State = EntityState.Modified;
                         //db.SaveChanges();
                         return RedirectToAction("Success", "Home");
                     }

                 }
                //  TempData["Message"] = "Couldn't Send Mail";
                         _tempDataManager.SetTempData("Message","Couldn't Send Mail");

                 return RedirectToAction("Wrong", "Home");
            }
            catch (Exception e)
            {
                // TempData["Message"]="Couldn't Send Mail";
                         _tempDataManager.SetTempData("Message","Couldn't Send Mail");

                return RedirectToAction("Failure","Home");
            }



         }

         [HttpGet]
        public ActionResult Wrong()
        {
            BarcodeViewModel model_void = new BarcodeViewModel();
            // ViewBag.Message = "BARCODE NUMBER DOESNOT EXIST.";
            _viewBagManager.SetValue("Message","BARCODE NUMBER DOESNOT EXIST.");

            return Ok(model_void);

        }
        

        [HttpGet]
        public ActionResult Empty()
        {
            // ViewBag.Message = "ENTER A SEARCH VALUE";
            _viewBagManager.SetValue("Message","ENTER A SEARCH VALUE");
            BarcodeViewModel model = new BarcodeViewModel();
            return Ok(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailFormModel model)
        {
           try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                var barcode = db.BarcodeStore.SingleOrDefault(u =>u.Company == companyinfo && u.PhoneNumber == model.PhoneNumber);
                var setting = db.Settings.First();

                string companyName = setting.CompanyName;
                string name = barcode.Name;
                var number = barcode.ShareholderNum;
                string ImageUrl = barcode.BarcodeImage != null ? "data:image/jpg;base64," +
                Convert.ToBase64String((byte[])barcode.BarcodeImage) : "";
                string logo = "http://localhost:59373/Images/unitedSecuritiesLogo.png";
                //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var body = @"<div style='margin-top:100px;'><div style='position:relative;background-color:white;width:350px;height:350px;margin:20px 5px 2px 0px;padding:20px;'>
            <img src='{0}' style='position:relative;float:right;width:100px;margin-left:40px;'/><div style='padding-left:10px;padding-top:10px;border-top:1px solid #4800ff;'><p style='font-weight:bold;'> {1} </p><p style='font-weight:bold;'>{2}
            </p>
            <p style='font-weight:bold;'>
               {3}
            </p>
            </div>
          <div style='width:100%;align-content:center;border-bottom:1px solid #4800ff;'>    
             <img src='cid:EmbeddedContent_1' />
           </div> 
    </div>
        </div>";
                MailSettings mailsetting = db.mailsettings.First();
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(mailsetting.Username, mailsetting.Password);
                var message = new MailMessage();
                message.To.Add(new MailAddress(model.FromEmail));  // replace with valid value 
                message.From = new MailAddress(mailsetting.SentFrom);  // replace with valid value
                message.Subject = "ShareHolder Barcode Registration Details";
                message.Body = string.Format(body,logo,companyName ,name, number, ImageUrl);
                message.IsBodyHtml = true;

               //Code to embed barcode image from database to sent image
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                                              message.Body,
                                              Encoding.UTF8,
                                              MediaTypeNames.Text.Html);
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                                             Regex.Replace(message.Body,
                                                           "<[^>]+?>",
                                                           string.Empty),
                                             Encoding.UTF8,
                                             MediaTypeNames.Text.Plain);

                MemoryStream fileStream = new
                MemoryStream(barcode.BarcodeImage);
                string mediaType = MediaTypeNames.Image.Jpeg;
                LinkedResource img = new LinkedResource(fileStream);

                img.ContentId = "EmbeddedContent_1";
                img.ContentType.MediaType = mediaType;
                img.TransferEncoding = TransferEncoding.Base64;
                img.ContentType.Name = img.ContentId;
                img.ContentLink = new Uri("cid:" + img.ContentId);
                htmlView.LinkedResources.Add(img);
                message.AlternateViews.Add(plainView);
                message.AlternateViews.Add(htmlView);
               //End of embedding barcode Image

               //Send Image Asynchronously...
                using (var smtp = new SmtpClient())
                {
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.Host = mailsetting.smtpHost;
                    smtp.Credentials = credentials;
                    smtp.Port = mailsetting.smtpPort;
                    await smtp.SendMailAsync(message);
                    // TempData["Message"] = "Barcode Information has been sent to your email";
                    _tempDataManager.SetTempData("Message","Barcode Information has been sent to your email");

                    barcode.PhoneNumber = model.PhoneNumber;
                    barcode.emailAddress = model.FromEmail;
                    db.Entry(barcode).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Success","Home");
                }

             
            }
            catch (Exception e)
            {
                // TempData["Message"]="Couldn't Send Mail";
                         _tempDataManager.SetTempData("Message","Couldn't Send Mail");

                return RedirectToAction("Failure","Home", model);
            }
        }

        
     
        // POST: /Register/Create

    }
}
