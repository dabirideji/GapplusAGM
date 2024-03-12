using BarcodeGenerator.Models;
using BarcodeGenerator.Models.ModelDTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Service
{
    public class EmailService
    {
        public static string username = ConfigurationManager.AppSettings["smtpUser"];
        public static string password = ConfigurationManager.AppSettings["smtpPass"];
        public static string emailHost = ConfigurationManager.AppSettings["smtpServer"];
        public static string port = ConfigurationManager.AppSettings["smtpPort"];
        public static string enableSSL = ConfigurationManager.AppSettings["EnableSsl"];
        public static int emailport = int.Parse(port);
        public static bool enableSsl = bool.Parse(enableSSL);
        public static string customerCare = ConfigurationManager.AppSettings["ToEmailAddress"];

        public Task<bool> SendEmailToCustomerService(AGMQuestion shareholder)
        {
            try
            {


                         var body = BuildMessageBody(shareholder);

                        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(username, password);

                                var message = new MailMessage();
                                message.To.Add(new MailAddress(customerCare));  // replace with valid value 
                                message.From = new MailAddress(username);  // replace with valid value
                                //message.ReplyToList.Add(mailsetting.SentFrom);
                                message.Subject = "AGM EVENT: Feedback/Complaint from "+ shareholder.ShareholderName ;
                                message.Body = body;
                                message.IsBodyHtml = true;

                                //End of embedding barcode Image

                                //Send Image Asynchronously...
                                using (var smtp = new SmtpClient())
                                {
                                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                                    smtp.EnableSsl = true;
                                    smtp.Host = emailHost;
                                    smtp.Credentials = credentials;
                                    smtp.Port = emailport;
                                    smtp.Send(message);

                                }

                            return System.Threading.Tasks.Task.FromResult<bool>(true);


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return System.Threading.Tasks.Task.FromResult<bool>(false);
            }

        }


        public Task<GenericResponseDto<string>> SendEmailToShareholderEmailAddress(BarcodeModelDto shareholder)
        {
            try
            {


                var body = PreregistrationMessageBody(shareholder);

                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(username, password);

                var message = new MailMessage();
                message.To.Add(new MailAddress(shareholder.emailAddress));  // replace with valid value 
                message.From = new MailAddress(username);  // replace with valid value
                                                           //message.ReplyToList.Add(mailsetting.SentFrom);
                message.Subject = string.Format("{0} - Access Token", shareholder.Company);
                message.Body = body;
                message.IsBodyHtml = true;

                //End of embedding barcode Image

                //Send Image Asynchronously...
                using (var smtp = new SmtpClient())
                {
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.Host = emailHost;
                    smtp.Credentials = credentials;
                    smtp.Port = emailport;
                    smtp.Send(message);

                }

                return Task.FromResult(new GenericResponseDto<string> { 
                    Status = true,
                    Message = "success"
                });


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return Task.FromResult(new GenericResponseDto<string>
                {
                    Status = false,
                    Message = "We couldn't send token to your email. Please try again later."
                });
            }

        }


        private static string BuildMessageBody(AGMQuestion dto)
        {
            var CRL = "<a style='color:blue' href='" + "https://www.coronationregistrars.com" + "'>United Securities Limited</a>";
            // Add body
            StringBuilder body = new StringBuilder();
            {
                body.Append("<div style='font-family:Palatino Linotype,serif;font-size:12px;color:#333399;margin-bottom:20px;'>");
                body.Append("<p>Dear Customer Service,</p>");
                body.Append("<p>This is a feedback/Complaint from "+dto.Company+" AGM Event. Time: "+dto.datetime.ToString("dddd, dd MMMM yyyy")+" </p>");
                body.Append("<p>Message: "+dto.shareholderquestion+"<br>");
                //body.Append("If there is no response from you, we will assume that the issue has been resolved and the request will automatically close after 24 hours.</p>");
                //body.Append("<p>Kindly acknowledge by replying for closure.</p>");
                body.Append("<br>");
                body.Append("<p>Warm regards,</p>");
                body.Append("<p>"+dto.ShareholderName+"</p>");
                body.Append("<p>" + dto.ShareholderNumber + "</p>");
                body.Append("<p style='margin-bottom:30px;'>" + dto.emailAddress + "</p>");
                //body.Append("<br>");
                body.Append("</p>");
                //body.Append("<br>");
                body.Append("</div>");

                body.Append("<div>");
                body.Append("<img width='25%' id='1' src='cid:usl.jpg'>");
                body.Append("<hr>");
                body.AppendFormat("<p style='color:#3315E9;font-family:Arial,sans-serif;font-size:10px;'> This mail is from {0}. The information transmitted is intended only for the person or entity to which it is addressed and may contain confidential and/or privileged material. Any review, re-transmission, dissemination and other use of, or taking of any action in reliance upon this information by persons or entities other than the intended recipient(s) is prohibited. If you have received this mail in error, please contact the sender and delete the material from any computer. <br><br>",CRL);
                body.AppendFormat("Finally, the recipient should check this email and any attachments for the presence of viruses. {0} and its employees accept no liability for any damage caused by any virus transmitted by this email. <br>", CRL);
                body.Append("</p>");
                body.Append("<hr>");
                body.Append("<p style='color:#3315E9;font-weight:bold;'>Scanned by Eset Smart Security</p>");
                body.Append("</div>");
            }

            return body.ToString();
        }


        private static string PreregistrationMessageBody(BarcodeModelDto dto)
        {
            var CRL = "<a style='color:blue' href='" + "https://www.coronationregistrars.com" + "'>Coronation Registrars Limited</a>";
            // Add body
            StringBuilder body = new StringBuilder();
            {
                body.Append("<div style='font-family:Palatino Linotype,serif;font-size:12px;color:#333399;margin-bottom:20px;'>");
                body.Append("<p>Dear "+dto.Name+",</p>");
                body.Append("<p>Please use this generated token to access event pre-registration </p>");
                body.Append("<p>Token: " + dto.Token + "<br>");
                body.Append("<br>");
                body.Append("<p>Regards,</p>");
                body.Append("<p> Coronation Registrars </p>");
                //body.Append("<br>");
                body.Append("</p>");
                //body.Append("<br>");
                body.Append("</div>");

                body.Append("<div>");
                body.Append("<img width='25%' id='1' src='cid:usl.jpg'>");
                body.Append("<hr>");
                body.AppendFormat("<p style='color:#3315E9;font-family:Arial,sans-serif;font-size:10px;'> This mail is from {0}. The information transmitted is intended only for the person or entity to which it is addressed and may contain confidential and/or privileged material. Any review, re-transmission, dissemination and other use of, or taking of any action in reliance upon this information by persons or entities other than the intended recipient(s) is prohibited. If you have received this mail in error, please contact the sender and delete the material from any computer. <br><br>", CRL);
                body.AppendFormat("Finally, the recipient should check this email and any attachments for the presence of viruses. {0} and its employees accept no liability for any damage caused by any virus transmitted by this email. <br>", CRL);
                body.Append("</p>");
                body.Append("<hr>");
                body.Append("<p style='color:#3315E9;font-weight:bold;'>Scanned by Eset Smart Security</p>");
                body.Append("</div>");
            }

            return body.ToString();
        }

    }
}