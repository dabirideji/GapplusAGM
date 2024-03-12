using BarcodeGenerator.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Service
{
    public class EmailManager
    {
        
        public EmailManager(UsersContext _db)
        {
            db = _db;
            ua = new UserAdmin(db);
            
        }

        private readonly UsersContext db ;
        UserAdmin ua;













        public static string username = ConfigurationManager.AppSettings["smtpUser"];
        public static string password = ConfigurationManager.AppSettings["smtpPass"];
        public static string emailHost = ConfigurationManager.AppSettings["smtpServer"];
        public static string port = ConfigurationManager.AppSettings["smtpPort"];
        public static string destinationEmail = ConfigurationManager.AppSettings["ToEmailAddress"];
        public static int emailport = int.Parse(port);



















        public bool SendEmail(AGMQuestion model)
        {

            try
            {

                System.Net.NetworkCredential credentials;
                var body = AutoCompleteBody(model);
                         
                if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    credentials = new System.Net.NetworkCredential(username, password);
                }
                else
                {
                    return false;
                }
                if (body != "empty")
                {

                    var message = new MailMessage();
                    message.To.Add(new MailAddress(destinationEmail));  // replace with valid value 
                    message.From = new MailAddress(username);  // replace with valid value
                    message.ReplyToList.Add(new MailAddress(model.emailAddress));
                    message.Subject = "Feedback from "+ model.Company + " AGM";
                    //message.Body = string.Format(body, user.FullName, issue.Title, issue.TakenBy, issue.Status);
                    message.Body = body;
                    message.IsBodyHtml = true;

                    //Send Image Asynchronously...
                    using (var smtp = new SmtpClient())
                    {
                        smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        smtp.Host = emailHost;
                        smtp.Credentials = credentials;
                        smtp.Port = emailport;
                        smtp.Send(message);
                        return false;
                    }
                }
                else
                {
                    return false;

                }
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(e);
                return false;
            }

        }

        private static string AutoCompleteBody(AGMQuestion agmmodel)
        {
            if(agmmodel!=null)
            {
                var USL = "<a style='color:blue' href='" + "https://www.coronationregistrars.com" + "'>Coronation Registrars Limited</a>";
                // Add body
                StringBuilder body = new StringBuilder();
                {
                    //                If you believe that the request has not been resolved, please reply to this email.
                    //If there is no response from you, we will assume that the issue has been resolved and the request will automatically close after 48 hours.
                    body.Append("<div style='font-family:Palatino Linotype,serif;font-size:14px;color:#333399;margin-bottom:20px;'>");
                    body.Append("<p>" + agmmodel.Company + " AGM FEEDBACK<br>");
                    body.Append("<strong>Feedback Time:</strong> " + agmmodel.datetime + "<br>");
                    body.Append("</p>");
                    body.Append("<br>");
                    body.Append("<p>FEEDBACK FROM:<br>");                
                    body.Append("<strong>Shareholder Name:</strong> " + agmmodel.ShareholderName + "<br>");
                    body.Append("<strong>Holding:</strong> " + Convert.ToDouble(agmmodel.holding).ToString("0,0") + "<br>");
                    body.Append("<strong>Shareholder Number:</strong> " + agmmodel.ShareholderNumber + "<br>");
                    body.Append("</p>");
                    body.Append("<br>");
                    body.Append("<p>FEEDBACK BODY:<br>");
                    body.Append("" + agmmodel.shareholderquestion + "<br>");
                    body.Append("</p>");
                    //body.Append("<p>Kindly acknowledge by replying for closure.</p>");
                    body.Append("<br>");
                    body.Append("<p>Thank you,</p>");
                    body.Append("<p style='margin-bottom:20px;'></p>");
                    //body.Append("<br>");

                    body.Append("<p>" + agmmodel.ShareholderName + ",<br>");
                    body.Append("" + agmmodel.emailAddress + ",<br>");
                    body.Append("" + agmmodel.phoneNumber + ".<br>");
                    body.Append("</p>");

                    body.Append("</div>");

                    body.Append("<div>");

                    body.Append("<hr>");
                    body.AppendFormat("<p style='color:#3315E9;font-family:Arial,sans-serif;font-size:10px;'> This mail is from {0}. The information transmitted is intended only for the person or entity to which it is addressed and may contain confidential and/or privileged material. Any review, re-transmission, dissemination and other use of, or taking of any action in reliance upon this information by persons or entities other than the intended recipient(s) is prohibited. If you have received this mail in error, please contact the sender and delete the material from any computer. <br><br>", USL);
                    body.AppendFormat("Finally, the recipient should check this email and any attachments for the presence of viruses. {0} and its employees accept no liability for any damage caused by any virus transmitted by this email. <br>", USL);
                    body.Append("</p>");
                    body.Append("<hr>");
                    body.Append("<p style='color:#3315E9;font-weight:bold;font-size:11px;'>Scanned by Eset Smart Security</p>");
                    body.Append("</div>");
                }

                return body.ToString();
            }
            return "empty";
          
        }
    }
}