
using BarcodeGenerator.Models;
using BarcodeGenerator.Models.ModelDTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


using System.Configuration;

namespace BarcodeGenerator.Service
{
    public class EmailAuthentication
    {
        public static string AppId = ConfigurationManager.AppSettings["appId"];
        public static string TenantId = ConfigurationManager.AppSettings["tenantId"];
        public static string ClientSecret = ConfigurationManager.AppSettings["clientSecret"];

        //ClientSecretCredential credentials;
        //UsernamePasswordCredential credentials;
        //UsernamePasswordCredentialOptions options;
        public EmailAuthentication()
        {
            //// The client credentials flow requires that you request the
            //// /.default scope, and pre-configure your permissions on the
            //// app registration in Azure. An administrator must grant consent
            //// to those permissions beforehand.
            //var scopes = new[] { "https://graph.microsoft.com/.default" };

            //// Values from app registration
            //var clientId = "YOUR_CLIENT_ID";
            //var tenantId = "YOUR_TENANT_ID";
            //var clientSecret = "YOUR_CLIENT_SECRET";

            //// using Azure.Identity;
            // The client credentials flow requires that you request the
            // /.default scope, and pre-configure your permissions on the
            // app registration in Azure. An administrator must grant consent
            // to those permissions beforehand.
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            // Values from app registration
            var clientId = AppId;
            var tenantId = TenantId;
            var clientSecret = ClientSecret;

            //// using Azure.Identity;
            //var options = new ClientSecretCredentialOptions
            //{
            //    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
            //};

            //// https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            //credentials = new ClientSecretCredential(
            //    tenantId, clientId, clientSecret, options);


        }


        //public async Task<GenericResponseDto<string>> SendEmailToShareholderEmailAddress(BarcodeModelDto shareholder)
        //{
        //    try
        //    {
                //var scopes = new[] { "User.Read" };

                //// Multi-tenant apps can use "common",
                //// single-tenant apps must use the tenant ID from the Azure portal
                //var clientId = AppId;
                //var tenantId = TenantId;

                //// using Azure.Identity;
                //var options = new UsernamePasswordCredentialOptions
                //{
                //    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                //};

                //var userName = "fomonaiye@gapplusng.com";
                //var password = "John-femy03";

                //// https://learn.microsoft.com/dotnet/api/azure.identity.usernamepasswordcredential
                //var userNamePasswordCredential = new UsernamePasswordCredential(
                //    userName, password, tenantId, clientId, options);





                //var body = PreregistrationMessageBody(shareholder);

                //var graphClient = new GraphServiceClient(userNamePasswordCredential, scopes);


                //var subject = string.Format("{0} - Access Token", shareholder.Company);


                ////End of embedding barcode Image

                //var requestBody = new SendMailPostRequestBody
                //{
                //    Message = new Message
                //    {
                //        Subject = subject,
                //        Body = new ItemBody
                //        {
                //            ContentType = BodyType.Html,
                //            Content = body,
                //        },
                //        ToRecipients = new List<Recipient>
                //        {
                //            new Recipient
                //            {
                //                EmailAddress = new EmailAddress
                //                {
                //                    Address = shareholder.emailAddress,
                //                },
                //            },
                //        }
                //        //,
                //        //    CcRecipients = new List<Recipient>
                //        //{
                //        //    new Recipient
                //        //    {
                //        //        EmailAddress = new EmailAddress
                //        //        {
                //        //            Address = "danas@contoso.com",
                //        //        },
                //        //    },
                //        //},
                //    },
                //    SaveToSentItems = false,
                //};

                //// To initialize your graphClient, see https://learn.microsoft.com/en-us/graph/sdks/create-client?from=snippets&tabs=csharp
                //await graphClient.Me.SendMail.PostAsync(requestBody);


        //        return new GenericResponseDto<string>
        //        {
        //            Status = true,
        //            Message = "success"
        //        };


        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.WriteLine(e);
        //        return new GenericResponseDto<string>
        //        {
        //            Status = false,
        //            Message = "We couldn't send token to your email. Please try again later."
        //        };
        //    }

        //}


        private static string PreregistrationMessageBody(BarcodeModelDto dto)
        {
            var CRL = "<a style='color:blue' href='" + "https://www.coronationregistrars.com" + "'>Coronation Registrars Limited</a>";
            // Add body
            StringBuilder body = new StringBuilder();
            {
                body.Append("<div style='font-family:Palatino Linotype,serif;font-size:12px;color:#333399;margin-bottom:20px;'>");
                body.Append("<p>Dear " + dto.Name + ",</p>");
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