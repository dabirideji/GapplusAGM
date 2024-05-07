﻿using BarcodeGenerator.Barcode;
using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BarcodeGenerator.Controllers
{
    // [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RegisterPasswordAPIController :  ControllerBase
    {
        public RegisterPasswordAPIController(UsersContext _db)
        {
         db=_db;
          ua = new UserAdmin(db);
        }
        UsersContext db;
        UserAdmin ua;
        //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

        //private int RetrieveAGMUniqueID()
        //{
        //    var AGMUniqueID = db.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

        //    return AGMUniqueID;
        //}
        //// GET api/default1
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/default1/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/default1
        //public HttpResponseMessage Post([FromBody]ReturnValue post)
        [HttpPost]
        public async Task<ActionResult> post([FromBody]ReturnValue post,[FromServices]IWebHostEnvironment env)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                var shareholder = db.BarcodeStore.SingleOrDefault(u =>u.Company==companyinfo && u.Barcode == post.Identity);
                if (shareholder!=null)
                {
                barcodecs objbar = new barcodecs(env);
                var emailid = shareholder.emailAddress;
                shareholder.passwordToken = objbar.generateBarcode();
                db.Entry(shareholder).State = EntityState.Modified;
                string logo = "http://localhost:59373/Images/unitedSecuritiesLogo.png";
                string message1 = "Please use this token to create a logon password";

                var body = @"<div style='margin-top:100px;'><div style='position:relative;background-color:white;width:100%;height:50%;margin:20px 5px 2px 0px;padding:20px;'>
            <img src='{0}' style='position:relative;float:right;width:100px;margin-left:40px;'/><div style='padding-left:10px;padding-top:10px;border-top:1px solid #4800ff;'><p style='font-weight:bold;'> {1} </p><p style='font-weight:bold;'>{2}
            </p>
            <p style='font-weight:bold;'>
               {3}
            </p>
            <p style='font-weight:bold;'>
               {4}
            </p>
            </div> 
    </div>
        </div>";
                MailSettings mailsetting = db.mailsettings.SingleOrDefault(m => m.ServerName == "Mail Server 1");



                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(mailsetting.Username, mailsetting.Password);


                var message = new MailMessage();
                message.From = new MailAddress(mailsetting.SentFrom);
                message.To.Add(new MailAddress(emailid));  // replace with valid value
                message.Subject = "United Securities Shareholder Security PIN";
                message.Body = string.Format(body, logo, shareholder.Name, message1, shareholder.passwordToken);
                message.IsBodyHtml = true;


                using (var smtp = new SmtpClient())
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credentials;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.Host = mailsetting.smtpHost;
                    smtp.Port = mailsetting.smtpPort;
                    await smtp.SendMailAsync(message);
                    var response = StatusCode((int)HttpStatusCode.OK, "Verified OK");
                    db.SaveChanges();
                    return response;
                }
                }
                 return StatusCode((int)HttpStatusCode.BadRequest, "Try Again");

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.NoContent, "Empty Post");
            }
        }

    }
}
