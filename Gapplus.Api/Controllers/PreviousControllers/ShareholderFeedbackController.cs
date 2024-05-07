using BarcodeGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BarcodeGenerator.Controllers
{   
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ShareholderFeedbackController : ControllerBase
    {
        //
        // GET: /ShareholderFeedback/

        UsersContext db;
        private readonly IViewBagManager _viewBagManager;
        public ShareholderFeedbackController(UsersContext _db,IViewBagManager viewBagManager)
        {

            db = _db;
            _viewBagManager=viewBagManager;
        }

[HttpGet]
        public ActionResult Index()
        {
            // ViewBag.presentcount = db.Present.Count();
            // ViewBag.feedbackcount = db.ShareholderFeedback.Count();
            // ViewBag.shareholders = db.BarcodeStore.Count();
            _viewBagManager.SetValue("presentcount", db.Present.Count());
_viewBagManager.SetValue("feedbackcount", db.ShareholderFeedback.Count());
_viewBagManager.SetValue("shareholders", db.BarcodeStore.Count());


            var feedbacks = db.ShareholderFeedback.ToList();

            return Ok(feedbacks);
        }

        //
        // GET: /ShareholderFeedback/Details/5

[HttpGet("{id}")]
        public ActionResult Details(int id)
        {
            return Ok();
        }

        //
        // GET: /ShareholderFeedback/Create



        //
        // POST: /ShareholderFeedback/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ShareholderFeedback collection)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    collection.When = DateTime.Now;
                     var setting = db.Settings.ToArray();
                    var ToAddress = setting[0].feebackEmailAddress;
                     string companyName = setting[0].CompanyName;
                string name = collection.Name;
                //string number = barcode.ShareholderNum;
                string feedback = collection.Message;
                string number = collection.phonenumber;
                string providedemail =  collection.Email;
                string date = collection.When.ToString();
                string logo = "http://localhost:59373/Image-s/unitedSecuritiesLogo.png";
                //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var body = @"<div style='margin-top:100px;'><div style='position:relative;background-color:white;margin:20px 5px 2px 0px;padding:20px;'>
           <div style='padding-left:10px;padding-top:10px;border-top:1px solid #4800ff;'><p style='font-weight:bold;'> {1} </p><p style='font-weight:bold;'>{2}
            </p>
            <p style='font-weight:bold;'>
               {3}
            </p>
            <p style='font-weight:bold;'>
               {4}
            </p>
            <p style='font-weight:bold;'>
               {5}
            </p>
            </div>
          <div style='width:100%;align-content:center;border-bottom:1px solid #4800ff;'>    
             <img src='{0}' style='position:relative;float:right;width:100px;margin-left:40px;'/>
           </div> 
    </div>
        </div>";
                MailSettings mailsetting = db.mailsettings.SingleOrDefault(m => m.ServerName == "Mail Server 1");
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(mailsetting.Username, mailsetting.Password);
                var message = new MailMessage();
                 message.To.Add(new MailAddress(ToAddress));
                message.CC.Add(new MailAddress("johnfemy@yahoo.com"));                
                    // replace with valid value 
                message.From = new MailAddress(mailsetting.SentFrom);  // replace with valid value
                message.Subject = "AGM Portal Feedback Notification";
                message.Body = string.Format(body,logo,name ,feedback, number, providedemail, date);
                message.IsBodyHtml = true;

               //Code to embed barcode image from database to sent image
                //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                //                              message.Body,
                //                              Encoding.UTF8,
                //                              MediaTypeNames.Text.Html);
                //AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                //                             Regex.Replace(message.Body,
                //                                           "<[^>]+?>",
                //                                           string.Empty),
                //                             Encoding.UTF8,
                //                             MediaTypeNames.Text.Plain);

                //MemoryStream fileStream = new
                //MemoryStream(barcode.BarcodeImage);
                //string mediaType = MediaTypeNames.Image.Jpeg;
                //LinkedResource img = new LinkedResource(fileStream);

                //img.ContentId = "EmbeddedContent_1";
                //img.ContentType.MediaType = mediaType;
                //img.TransferEncoding = TransferEncoding.Base64;
                //img.ContentType.Name = img.ContentId;
                //img.ContentLink = new Uri("cid:" + img.ContentId);
                //htmlView.LinkedResources.Add(img);
                //message.AlternateViews.Add(plainView);
                //message.AlternateViews.Add(htmlView);
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
                    }
                
                db.ShareholderFeedback.Add(collection);
                db.SaveChanges();
                return RedirectToAction("Success");
           }
                // TODO: Add insert logic here

            return RedirectToAction("Invalid");
       }
       catch
      {
         return RedirectToAction("Failure");
       }
   }




        private void Success()
        {
            // ViewBag.Message = "Your Feedback has been receive. Our agent will process immediately.";
            _viewBagManager.SetValue("Message", "Your Feedback has been receive. Our agent will process immediately.");
        }

        private void Failure()
        {
            // ViewBag.Message = "Couldnt process your feedback. Please try again later.";
            _viewBagManager.SetValue("Message", "Couldnt process your feedback. Please try again later.");

        }

        private void Invalid()
        {
            // ViewBag.Message = "Something went wrong. Cannot process the data supplied";
            _viewBagManager.SetValue("Message", "Something went wrong. Cannot process the data supplied");

        }
        
        //
        // GET: /ShareholderFeedback/Edit/5
[HttpPut("{id}")]
        public ActionResult Edit(int id)
        {
            return Ok();
        }

        //
        // POST: /ShareholderFeedback/Edit/5

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
                return Ok();
            }
        }

        //
        // GET: /ShareholderFeedback/Delete/5


        //
        // POST: /ShareholderFeedback/Delete/5

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
                return Ok();
            }
        }
    }
}
