using BarcodeGenerator.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using BarcodeGenerator.Barcode;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Gapplus.Application.Helpers;




namespace BarcodeGenerator.Service
{


    public class AGMRegistrationService
    {
        UsersContext db;
        UserAdmin ua;
            string toEmailAddress=DatabaseManager.GetAppSetting("ToEmailAddress");


        public AGMRegistrationService(UsersContext context)
        {
            db = context;
            ua = new UserAdmin(db);
        }

        public Task<AGMCompaniesResponse> GetActiveAGMCompaniesAsync()
        {
            AGMCompaniesResponse response;
            try
            {

                var companyNameList = db.Settings.Where(s => s.ArchiveStatus == false).Select(o => new AGMCompanies { company = o.CompanyName, description = o.Description, agmid = o.AGMID, RegCode = o.RegCode, venue = o.Venue, dateTime = o.AgmDateTime, EnddateTime = o.AgmEndDateTime }).Distinct().OrderBy(k => k.company).ToList();
                // if (companyNameList != null)
                if (companyNameList != null && companyNameList.Count()!=0)
                {
                    response = new AGMCompaniesResponse
                    {
                        Code = "200",
                        Message = "",
                        Companies = companyNameList
                    };
                    return Task.FromResult<AGMCompaniesResponse>(response);
                }
                else
                {
                    response = new AGMCompaniesResponse
                    {
                        Code = "200",
                        Message = "No Active AGM Available",
                        Companies = new List<AGMCompanies>()
                    };
                    return Task.FromResult<AGMCompaniesResponse>(response);
                }


            }
            catch (Exception e)
            {
                response = new AGMCompaniesResponse
                {
                    Code = "500",
                    Message = "Service not available at this time.",
                    Companies = new List<AGMCompanies>()
                };
                return Task.FromResult<AGMCompaniesResponse>(response);
            }
        }


        public Task<AGMCompaniesResponse> GetActiveAGMCompanyAsync(string company)
        {
            AGMCompaniesResponse response;
            try
            {
                var companyNameList = db.Settings.Where(s => s.ArchiveStatus == false && s.CompanyName == company.Trim()).Select(o => new AGMCompanies { company = o.CompanyName, description = o.Description, agmid = o.AGMID, RegCode = o.RegCode, venue = o.Venue, dateTime = o.AgmDateTime, EnddateTime = o.AgmEndDateTime }).Distinct().OrderBy(k => k.company).ToList();

                if (companyNameList != null)
                {
                    response = new AGMCompaniesResponse
                    {
                        Code = "200",
                        Message = "",
                        Companies = companyNameList
                    };
                    return Task.FromResult<AGMCompaniesResponse>(response);
                }
                else
                {
                    response = new AGMCompaniesResponse
                    {
                        Code = "201",
                        Message = "No Active AGM Available for" + " " + company,
                        Companies = new List<AGMCompanies>()
                    };
                    return Task.FromResult<AGMCompaniesResponse>(response);
                }


            }
            catch (Exception e)
            {
                response = new AGMCompaniesResponse
                {
                    Code = "500",
                    Message = "Service not available at this time.",
                    Companies = new List<AGMCompanies>()
                };
                return Task.FromResult<AGMCompaniesResponse>(response);
            }
        }
        public BarcodeModelDto GetShareholderByCompanyAndEmail(string company, string email)
        {
            try
            {
                //var barcode = db.BarcodeStore.Where(s => s.Company.Trim().ToLower() == company.Trim().ToLower() && s.emailAddress.Trim().ToLower() == email.Trim().ToLower()).FirstOrDefault();
                var shareholders =
             from shareholder in db.BarcodeStore
             where shareholder.Company.Trim().ToLower() == company.Trim().ToLower() && shareholder.emailAddress.Trim().ToLower() == email.Trim().ToLower()


             select new BarcodeModelDto
             {
                 Id = shareholder.Id,
                 SN = shareholder.SN,

                 Company = shareholder.Company,
                 Name = shareholder.Name,
                 //Holding = shareholder.Holding,
                 Address = shareholder.Address,
                 //PercentageHolding = shareholder.PercentageHolding,
                 ShareholderNum = shareholder.ShareholderNum,
                 emailAddress = shareholder.emailAddress,
                 PhoneNumber = shareholder.PhoneNumber,
                 accesscode = shareholder.accesscode,
                 OnlineEventUrl = shareholder.OnlineEventUrl

             };

                return shareholders.FirstOrDefault();
                //   var barcode = shareholders.FirstOrDefault();

                //   //return Task.FromResult<List<AGMCompaniesResponse>>(companylist);
                //return barcode;

            }
            catch (Exception e)
            {
                return new BarcodeModelDto();
            }
        }



        public Task<PreRegistrationResponse> ShareHolderAGMRregistrationAsync(SearchModel request)
        {

            if (request.search == null && request.emailsearch == null)
            {

                return Task.FromResult<PreRegistrationResponse>(new PreRegistrationResponse() { Status = "failed", ResponseCode = "05", ResponseMessage = "Kindly enter a valid shareholder email or phone number" });
            }
            if (request.company == null)
            {

                return Task.FromResult<PreRegistrationResponse>(new PreRegistrationResponse() { Status = "failed", ResponseCode = "05", ResponseMessage = "Kindly select a company" });

            }

            var UniqueAGMId = ua.RetrieveAGMUniqueID(request.company);
            if (UniqueAGMId == -1)
            {

                return Task.FromResult<PreRegistrationResponse>(new PreRegistrationResponse() { Status = "failed", ResponseCode = "05", ResponseMessage = "Kindly select a company" });

            }
            //var setting = await _uow.Settings.GetSettings();
            //if (setting == null || setting.CompanyName == null)
            //{
            //    return new PreRegistrationResponse() { Status = "failed", ResponseCode = "05", ResponseMessage = "No Company Setup for AGM" };

            //    //setting.CompanyName
            //}
            //if (setting.CompanyName.Trim().ToLower() != request.company.Trim().ToLower())
            //{
            //    return new PreRegistrationResponse() { Status = "failed", ResponseCode = "05", ResponseMessage = "e - Accreditation is currently not available for " + request.company + ", Watch out for Accreditation period." };

            //}
            var shareholder = new BarcodeModelDto();
            try
            {
                if (request.search != null)
                {
                    shareholder = GetShareholderByCompanyAndEmail(request.company, request.search);
                }
                if (request.emailsearch != null)
                {
                    shareholder = GetShareholderByCompanyAndEmail(request.company, request.emailsearch);
                }
                //var setting = await db.Settings.GetSettings();
                BarcodeModel barcode = new BarcodeModel
                {
                    Id = shareholder.Id,
                    SN = shareholder.SN,

                    Company = shareholder.Company,
                    Name = shareholder.Name,
                    //Holding = shareholder.Holding,
                    Address = shareholder.Address,
                    //PercentageHolding = shareholder.PercentageHolding,
                    ShareholderNum = shareholder.ShareholderNum,
                    emailAddress = shareholder.emailAddress,
                    PhoneNumber = shareholder.PhoneNumber,
                    accesscode = shareholder.accesscode,
                    OnlineEventUrl = shareholder.OnlineEventUrl
                };

                if (barcode != null)
                {
                    //var response = SendEmailToShareHolder(barcodes, UniqueAGMId);
                    var response = SendEmailToShareHolderWithCalendar(barcode, UniqueAGMId);
                    if (response == "success")
                    {
                        return Task.FromResult<PreRegistrationResponse>(new PreRegistrationResponse() { Status = "success", ResponseCode = "00", ResponseMessage = "Barcode Information has been sent to your email" });
                    }

                    //return Task.FromResult<PreRegistrationResponse>(new PreRegistrationResponse() { Status = "failed", ResponseCode = "05", ResponseMessage = "Shareholder details could not be found, kindly update your information" });

                }
                return Task.FromResult<PreRegistrationResponse>(new PreRegistrationResponse() { Status = "failed", ResponseCode = "05", ResponseMessage = "Barcode Information Couldn't be sent to email" });


            }
            catch (Exception e)
            {
                //await _log.AddAuditTrail(new AuditTrailDto()
                //{
                //    Action = "ShareHolderPregistration",
                //    Description = "User Attempts to Pre Register",
                //    Message = $"An Error Occurred {e.Message}",
                //    User = "",
                //    Platform = _helper.GetCurrentUserEmail(),
                //});
                return Task.FromResult<PreRegistrationResponse>(new PreRegistrationResponse() { Status = "failed", ResponseCode = "05", ResponseMessage = "Barcode Information Couldn't be sent to email" });
            }
        }

        // private string CreateBody(string logo, string name, string Title, string number, string qrcode, string request, string accesscode)
        // {
        //     //var requestUri = $"{Convert.ToString(ConfigurationManager.AppSettings["VotingBaseAddress"])}/agm/api/PreregistrationAPI/GetPreregistrationComfirmation";
        //     //var query = emailaddress;
        //     //var request = $"{requestUri}?{query}";

        //     string body = string.Empty;
        //     string webRootPath = HttpContext.Current.Server.MapPath("~/MailTemplate/");

        //     var emailTemplatePath = Path.Combine(webRootPath, "EmailTemplate.html");
        //     using (StreamReader reader = new StreamReader(emailTemplatePath))
        //     {
        //         body = reader.ReadToEnd();
        //     }
        //     body = body.Replace("{0}", logo);
        //     body = body.Replace("{1}", name);
        //     body = body.Replace("{2}", Title);
        //     body = body.Replace("{3}", name);
        //     body = body.Replace("{4}", number);
        //     body = body.Replace("{5}", qrcode);
        //     body = body.Replace("{6}", request);
        //     body = body.Replace("{7}", accesscode);

        //     return body;
        // }
        private string CreateBody(string logo, string name, string title, string number, string qrcode, string request, string accesscode)
        {
            // Get the service provider from the static accessor
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IWebHostEnvironment>() // Register a dummy IWebHostEnvironment instance
                .BuildServiceProvider();

            // Resolve the IWebHostEnvironment service
            var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();


            string body = string.Empty;

            // Get the path to the mail template
            string webRootPath = webHostEnvironment.WebRootPath; // Assuming you have set up wwwroot as the web root
            string emailTemplatePath = Path.Combine(webRootPath, "MailTemplate", "EmailTemplate.html");

            // Read the email template
            using (StreamReader reader = new StreamReader(emailTemplatePath))
            {
                body = reader.ReadToEnd();
            }

            // Replace placeholders in the template with provided values
            body = body.Replace("{0}", logo);
            body = body.Replace("{1}", name);
            body = body.Replace("{2}", title);
            body = body.Replace("{3}", name);
            body = body.Replace("{4}", number);
            body = body.Replace("{5}", qrcode);
            body = body.Replace("{6}", request);
            body = body.Replace("{7}", accesscode);

            return body;
        }
        public Task<AGMAccesscodeResponse> GetShareholderAccessCodeAsync(PreregistrationDto post)
        {
            try
            {
                if (!string.IsNullOrEmpty(post.Company) && !string.IsNullOrEmpty(post.emailAddress))
                {
                    var shareholder = db.BarcodeStore.FirstOrDefault(s => s.Company.ToLower() == post.Company.ToLower() && s.emailAddress.ToLower() == post.emailAddress.ToLower());

                    var acccescodeResponse = new AGMAccesscodeResponse { company = shareholder.Company, accesscode = shareholder.accesscode };

                    return Task.FromResult<AGMAccesscodeResponse>(acccescodeResponse);
                }

                return Task.FromResult<AGMAccesscodeResponse>(new AGMAccesscodeResponse());
            }
            catch (Exception e)
            {
                return Task.FromResult<AGMAccesscodeResponse>(new AGMAccesscodeResponse());
            }

        }


        public Task<int> GetPreregistrationAGMID(string company)
        {
            var companyAGMID = db.Settings.Where(s => s.CompanyName == company && s.ArchiveStatus == false).Select(o => o.AGMID).OrderByDescending(k => k).FirstOrDefault();
            if (companyAGMID > 0)
            {
                return Task.FromResult<int>(companyAGMID);
            }
            return Task.FromResult<int>(-1);
        }


        private string SendEmailToShareHolder(BarcodeModel barcode, int agmid)
        {
            var setting = db.Settings.SingleOrDefault(s => s.AGMID == agmid);
            //foreach (var barcode in barcodes)
            //{
            //settingmodel.CompanyName = qrcode;
            //await _uow.CompleteAsync();
            string companyName;
            string Title = "";
            string logo = "";
            if (setting != null)
            {
                companyName = setting.CompanyName;
                Title = setting.Title;
                //logo = setting.Image != null ? "data:image/jpg;base64," +
                //Convert.ToBase64String((byte[])setting.Image) : "";
            }
            else
            {
                companyName = "";
            }


            string name = barcode.Name;
            string number = barcode.ShareholderNum.ToString();
            var qrgenerator = new Qrcode();
            //byte[] qrcode;
            string qrcode;
            var value = new KeyValuePair<string, string>("company", barcode.Company);
            //int agmid = UniqueAGMId;
            //get AGMID to check expiration status
            //try
            //{
            //    var _client = new VotingServiceClient();
            //    var response = await _client.GetAsync<ServiceResponse<int>>($"/agm/api/PreregistrationAPI/GetPreregistrationAGMID", value);
            //    if (!response.IsValid)
            //    {
            //        agmid = -1;
            //    }
            //    agmid = response.Object;
            //}
            //catch (Exception e)
            //{
            //    agmid = -1;
            //}

            //var valuesToEncode = new[]
            //{
            //       new KeyValuePair<string, string>("identity", barcode.emailAddress),
            //       new KeyValuePair<string, string>("company", barcode.Company),
            //       new KeyValuePair<string, int>("agmid", agmid)

            //};

            //var encodedContent = new FormUrlEncodedContent(valuesToEncode);
            //var query = await encodedContent.ReadAsStringAsync();

            var requesturl = barcode.OnlineEventUrl;
            var meetingAccesscode = barcode.accesscode;

            if (string.IsNullOrEmpty(barcode.ImageUrl))
            {
                qrcode = qrgenerator.GenerateMyQCCode(barcode.emailAddress, barcode.PhoneNumber);
                var bar = db.BarcodeStore.Find(barcode.Id);
                bar.ImageUrl = qrcode;
                db.SaveChanges();
            }
            else
            {
                qrcode = barcode.ImageUrl;
            }
            //qrgenerator.ReadQRCode();
            //string ImageUrl = barcode.BarcodeImage != null ? "data:image/jpg;base64," +
            //Convert.ToBase64String((byte[])barcode.BarcodeImage) : "";
            // string ImageUrl = qrcode != null ? "data:image/jpg;base64," +
            //Convert.ToBase64String(qrcode) : "";
            string ImageUrl = qrcode;
            // logo = $"{Convert.ToString(ConfigurationManager.AppSettings["baseAddress"])}/Images/logo.png";
            logo = $"{DatabaseManager.GetAppSetting<string>("baseAddress")}/Images/logo.png";

            //"https://drive.google.com/file/d/13h8oyK5QGIeMLmoAbAFCFhFYYFr8XbEQ/view?usp=sharing";
            //var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            //                    var body = @"<div style='margin-top:100px;'><div style='position:relative;background-color:white;width:350px;height:350px;margin:20px 5px 2px 0px;padding:20px;'>
            //        <img src='{0}' style='position:relative;float:right;width:100px;margin-left:40px;'/><div style='padding-left:10px;padding-top:10px;border-top:1px solid #4800ff;'><p style='font-weight:bold;'> {1} </p><p style='font-weight:bold;'>{2}
            //        </p>
            //        <p style='font-weight:bold;'>
            //           {3}
            //        </p>
            //        </div>
            //      <div style='width:100%;align-content:center;border-bottom:1px solid #4800ff;'>    
            //         <img src='cid:EmbeddedContent_1' />
            //       </div> 
            //</div>
            //    </div>";
            //MailSettings mailsetting = await db.MailSettings.GetMailSettings();
            // var smtpUser=ConfigurationManager.AppSettings["smtpUser"];
            var smtpUser=DatabaseManager.GetAppSetting("smtpUser");


            // var smtpPass=ConfigurationManager.AppSettings["smtpPass"];
            var smtpPass=DatabaseManager.GetAppSetting("smtpPass");


            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(smtpUser,smtpPass);
            var message = new MailMessage();
            message.To.Add(new MailAddress(barcode.emailAddress));  // replace with valid value 

            // var toEmailAddress=ConfigurationManager.AppSettings["ToEmailAddress"];
            var toEmailAddress=DatabaseManager.GetAppSetting("ToEmailAddress");

            message.From = new MailAddress(toEmailAddress);  // replace with valid value
                                                                                                 //message.Subject = "ShareHolder QR Code Registration Details";
            message.Subject = "e-Accreditation Success!";
            //message.Body = string.Format(body, logo, companyName, name, number, ImageUrl);
            message.Body = CreateBody(logo, name, Title, number, ImageUrl, requesturl, meetingAccesscode);
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

            //MemoryStream fileStream = new
            //MemoryStream(qrcode);
            //string mediaType = MediaTypeNames.Image.Jpeg;
            //LinkedResource img = new LinkedResource(fileStream);

            //img.ContentId = "EmbeddedContent_1";
            //img.ContentType.MediaType = mediaType;
            //img.TransferEncoding = TransferEncoding.Base64;
            //img.ContentType.Name = img.ContentId;
            //img.ContentLink = new Uri("cid:" + img.ContentId);
            //htmlView.LinkedResources.Add(img);
            message.AlternateViews.Add(plainView);
            message.AlternateViews.Add(htmlView);
            //End of embedding barcode Image

            //Send Image Asynchronously...

            using (var smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;


                // smtp.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]);
                smtp.EnableSsl = DatabaseManager.GetAppSetting<bool>("EnableSsl");

                // smtp.Host = ConfigurationManager.AppSettings["smtpServer"];
                smtp.Host = DatabaseManager.GetAppSetting("smtpServer");

                smtp.Credentials = credentials;
                // smtp.Port = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);
                smtp.Port = DatabaseManager.GetAppSetting<int>("smtpPort");

                smtp.Send(message);


            }

            //}
            return "success";
        }


        private string SendEmailToShareHolderWithCalendar(BarcodeModel barcode, int agmid)
        {
            var setting = db.Settings.SingleOrDefault(s => s.AGMID == agmid);
            //foreach (var barcode in barcodes)
            //{
            //settingmodel.CompanyName = qrcode;
            //await _uow.CompleteAsync();
            string companyName;
            string Title = "";
            string logo = "";
            if (setting != null)
            {
                companyName = setting.CompanyName;
                Title = setting.Title;
                //logo = setting.Image != null ? "data:image/jpg;base64," +
                //Convert.ToBase64String((byte[])setting.Image) : "";
            }
            else
            {
                companyName = "";
            }


            string name = barcode.Name;
            string number = barcode.ShareholderNum.ToString();
            var qrgenerator = new Qrcode();
            string qrcode;
            var value = new KeyValuePair<string, string>("company", barcode.Company);

            var requesturl = barcode.OnlineEventUrl;
            var meetingAccesscode = barcode.accesscode;

            if (string.IsNullOrEmpty(barcode.ImageUrl))
            {
                qrcode = qrgenerator.GenerateMyQCCode(barcode.emailAddress, barcode.PhoneNumber);
                var bar = db.BarcodeStore.Find(barcode.Id);
                bar.ImageUrl = qrcode;
                db.SaveChanges();
            }
            else
            {
                qrcode = barcode.ImageUrl;
            }

            string ImageUrl = qrcode;
            // var baseAddress=ConfigurationManager.AppSettings["baseAddress"];
            var baseAddress=DatabaseManager.GetAppSetting("baseAddress");
            logo = $"{baseAddress}/Images/logo.png";


            //MailSettings mailsetting = await db.MailSettings.GetMailSettings();

            // var smtpUser=ConfigurationManager.AppSettings["smtpUser"];
            var smtpUser=DatabaseManager.GetAppSetting("smtpUser");

            // var smtpPass= ConfigurationManager.AppSettings["smtpPass"];
            var smtpPass= DatabaseManager.GetAppSetting("smtpPass");


            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(smtpUser,smtpPass);
            //var message = new MailMessage();
            //message.To.Add(new MailAddress(barcode.emailAddress));  // replace with valid value 
            //message.From = new MailAddress(ConfigurationManager.AppSettings["ToEmailAddress"]);  // replace with valid value
            //                                                                                     //message.Subject = "ShareHolder QR Code Registration Details";
            //message.Subject = "e-Accreditation Success!";
            //message.Body = CreateBody(logo, name, Title, number, ImageUrl, requesturl, meetingAccesscode);
            //message.IsBodyHtml = true;

            //using (var smtp = new SmtpClient())
            //{
            //    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            //    smtp.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]);
            //    smtp.Host = ConfigurationManager.AppSettings["smtpServer"];
            //    smtp.Credentials = credentials;
            //    smtp.Port = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);

            //    smtp.Send(message);

            //}
            string startTime1 = Convert.ToDateTime(setting.AgmDateTime).ToString("yyyyMMddTHHmmssZ");
            string endTime1 = Convert.ToDateTime(setting.AgmEndDateTime).ToString("yyyyMMddTHHmmssZ");
            //SmtpClient sc = new SmtpClient("");
            MailMessage msg = new MailMessage();

            // msg.From = new MailAddress(ConfigurationManager.AppSettings["ToEmailAddress"], "Coronation Registrars");
            msg.From = new MailAddress(toEmailAddress, "Coronation Registrars");
            msg.To.Add(new MailAddress(barcode.emailAddress));
            msg.Subject = "e-Accreditation Success!";
            msg.Body = CreateBody(logo, name, Title, number, ImageUrl, requesturl, meetingAccesscode);

            //msg.Body = "Please Attend the meeting with this schedule";
            //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
            //                              msg.Body,
            //                              Encoding.UTF8,
            //                              MediaTypeNames.Text.Html);
            //AlternateView plainView = AlternateView.CreateAlternateViewFromString(
            //                             Regex.Replace(msg.Body,
            //                                           "<[^>]+?>",
            //                                           string.Empty),
            //                             Encoding.UTF8,
            //                             MediaTypeNames.Text.Plain);
            //msg.AlternateViews.Add(plainView);
            //msg.AlternateViews.Add(htmlView);

            DateTime date = DateTime.UtcNow;

            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            if (setting?.AgmDateTime != null)
            { startTime = (DateTime)setting?.AgmDateTime; }

            if (setting?.AgmEndDateTime != null)
            {
                endTime = (DateTime)setting?.AgmEndDateTime;
            }

            string now = DateTime.Now.ToString("yyyyMMddTHHmmssZ");

            StringBuilder str = new StringBuilder();

            str.AppendLine("VERSION:2.0");
            str.AppendLine("BEGIN:VCALENDAR");
            str.AppendLine("PRODID:-//Schedule a Meeting");
            str.AppendLine("METHOD:REQUEST");
            str.AppendLine("BEGIN:VEVENT");
            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", DateTime.Now.AddMinutes(+10)));
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", DateTime.Now.AddMinutes(+60)));
            str.AppendLine("LOCATION: CGI");
            str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
            //str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
            str.AppendLine(string.Format("SUMMARY:{0}", setting.Description));
            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));

            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

            str.AppendLine("BEGIN:VALARM");
            str.AppendLine("TRIGGER:-PT15M");
            str.AppendLine("ACTION:DISPLAY");
            str.AppendLine("DESCRIPTION:Reminder");
            str.AppendLine("END:VALARM");
            str.AppendLine("END:VEVENT");
            str.AppendLine("END:VCALENDAR");

            System.Net.Mime.ContentType contype = new System.Net.Mime.ContentType("text/calendar");
            contype.Parameters.Add("method", "REQUEST");
            contype.Parameters.Add("name", "Meeting.ics");
            //contype.Parameters.Add("method", "REQUEST");
            //contype.Parameters.Add("name", "Meeting.ics");
            AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), contype);

            msg.AlternateViews.Add(avCal);

            AlternateView body1 = AlternateView.CreateAlternateViewFromString(msg.Body, new System.Net.Mime.ContentType("text/html"));
            msg.AlternateViews.Add(body1);

            byte[] bytes = Encoding.UTF8.GetBytes(str.ToString());
            MemoryStream stream = new MemoryStream(bytes);
            Attachment icsAttachment = new Attachment(stream, "invitation.ics", "text/calendar");
            msg.Attachments.Add(icsAttachment);
            msg.IsBodyHtml = true;
            //msg.Body = str.ToString();

            //StringBuilder str = new StringBuilder();
            //    str.AppendLine("BEGIN:VCALENDAR");

            //    //PRODID: identifier for the product that created the Calendar object
            //    str.AppendLine("PRODID:-//ABC Company//Outlook MIMEDIR//EN");
            //    str.AppendLine("VERSION:2.0");
            //    str.AppendLine("METHOD:REQUEST");

            //    str.AppendLine("BEGIN:VEVENT");

            //    str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", startTime1));//TimeZoneInfo.ConvertTimeToUtc("BeginTime").ToString("yyyyMMddTHHmmssZ")));
            //    str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
            //    str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", endTime1));//TimeZoneInfo.ConvertTimeToUtc("EndTime").ToString("yyyyMMddTHHmmssZ")));
            //    str.AppendLine(string.Format("LOCATION: {0}", "Location"));

            //    // UID should be unique.
            //    str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
            //    str.AppendLine(string.Format("DESCRIPTION:{0}", msg.Body));
            //    str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", msg.Body));
            //    str.AppendLine(string.Format("SUMMARY:{0}", msg.Subject));

            //    str.AppendLine("STATUS:CONFIRMED");
            //    str.AppendLine("BEGIN:VALARM");
            //    str.AppendLine("TRIGGER:-PT15M");
            //    str.AppendLine("ACTION:Accept");
            //    str.AppendLine("DESCRIPTION:Reminder");
            //    str.AppendLine("X-MICROSOFT-CDO-BUSYSTATUS:BUSY");
            //    str.AppendLine("END:VALARM");
            //    str.AppendLine("END:VEVENT");

            //    str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", msg.From.Address));
            //    str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", msg.To[0].DisplayName, msg.To[0].Address));

            //    str.AppendLine("END:VCALENDAR");
            //    System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType("text/calendar");
            //    ct.Parameters.Add("method", "REQUEST");
            //    ct.Parameters.Add("name", "meeting.ics");
            //    AlternateView avCal = AlternateView.CreateAlternateViewFromString(str.ToString(), ct);
            //    //sc.Send(msg);

            using (var smtp = new SmtpClient())
            {


                  smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;


                // smtp.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]);
                smtp.EnableSsl = DatabaseManager.GetAppSetting<bool>("EnableSsl");

                // smtp.Host = ConfigurationManager.AppSettings["smtpServer"];
                smtp.Host = DatabaseManager.GetAppSetting("smtpServer");

                smtp.Credentials = credentials;
                // smtp.Port = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);
                smtp.Port = DatabaseManager.GetAppSetting<int>("smtpPort");

                smtp.Send(msg);

            }
            //}
            return "success";
        }

        public Task<PreRegistrationResponse> GetPreregistrationComfirmationAsync(VoteModel query)
        {
            try
            {
                string companyinfo = query.company;
                string emailAddress = query.identity;
                int agmid = query.agmid;

                var agmstatus = db.Settings.FirstOrDefault(s => s.AGMID == agmid);
                if (agmstatus.ArchiveStatus)
                {
                    var MessageLog5 = new AppLog()
                    {
                        Status = "Failed Accreditation",
                        ResponseCode = "205",
                        ResponseMessage = "Failed Accreditation Attempt - Empty Company parameter or Email provided. Email:" + " " + emailAddress,
                        EventTime = DateTime.Now
                    };
                    db.AppLogs.Add(MessageLog5);
                    db.SaveChanges();

                }
                if (agmstatus.StartAdmittance)
                {
                    var MessageLog5 = new AppLog()
                    {
                        Status = "Failed Accreditation",
                        ResponseCode = "205",
                        ResponseMessage = "Admittance has been stopped." + " " + emailAddress,
                        EventTime = DateTime.Now
                    };
                    db.AppLogs.Add(MessageLog5);
                    db.SaveChanges();
                }

                if (!string.IsNullOrEmpty(companyinfo) && !string.IsNullOrEmpty(emailAddress))
                {
                    var UniqueAGMId = ua.RetrieveAGMUniqueID(companyinfo);
                    if (UniqueAGMId != -1)
                    {
                        var shareholderRecords = db.BarcodeStore.Where(u => u.Company.ToLower() == companyinfo.ToLower() && u.emailAddress.ToLower() == emailAddress.ToLower());
                        if (shareholderRecords != null)
                        {
                            foreach (var sr in shareholderRecords)
                            {
                                var shareholder = db.Present.FirstOrDefault(u => u.AGMID == UniqueAGMId && u.ShareholderNum == sr.ShareholderNum);

                                if (sr.PresentByProxy != true && shareholder == null)
                                {
                                    sr.Present = true;
                                    PresentModel present = new PresentModel();
                                    present.Name = sr.Name;
                                    present.Address = sr.Address;
                                    present.ShareholderNum = sr.ShareholderNum;
                                    present.Holding = sr.Holding;
                                    present.AGMID = UniqueAGMId;
                                    present.PercentageHolding = sr.PercentageHolding;
                                    present.present = true;
                                    present.proxy = false;
                                    present.PresentTime = DateTime.Now;
                                    present.Timestamp = DateTime.Now.TimeOfDay;
                                    present.emailAddress = sr.emailAddress;
                                    if (!String.IsNullOrEmpty(sr.PhoneNumber))
                                    {
                                        if (sr.PhoneNumber.StartsWith("234"))
                                        {
                                            present.PhoneNumber = sr.PhoneNumber;
                                        }
                                        else if (sr.PhoneNumber.StartsWith("0"))
                                        {
                                            var number = double.Parse(sr.PhoneNumber);
                                            present.PhoneNumber = "234" + number.ToString();

                                        }

                                    }
                                    present.Clikapad = sr.Clikapad;
                                    db.Present.Add(present);
                                    sr.Date = DateTime.Today.ToString();
                                    db.Entry(sr).State = EntityState.Modified;

                                    //var sucessoutput = JsonConvert.SerializeObject(successresponse);
                                    var MessageLog = new AppLog()
                                    {
                                        Status = "successfully Accredited",
                                        ResponseCode = "200",
                                        ResponseMessage = sr.ShareholderNum + " " + "Successful Accreditation. Email Address" + " " + emailAddress,
                                        EventTime = DateTime.Now
                                    };
                                    db.AppLogs.Add(MessageLog);
                                    //db.SaveChanges();
                                }
                                else
                                {
                                    var MessageLog2 = new AppLog()
                                    {
                                        Status = "Already Accredited",
                                        ResponseCode = "202",
                                        ResponseMessage = sr.ShareholderNum + " " + "Successful Accreditation. Email:" + " " + emailAddress,
                                        EventTime = DateTime.Now
                                    };
                                    db.AppLogs.Add(MessageLog2);
                                }


                            }
                        }
                        else
                        {
                            var MessageLog3 = new AppLog()
                            {
                                Status = "Failed",
                                ResponseCode = "205",
                                ResponseMessage = "Failed e-accreditation Attempt - The Company's AGM may not be enlisted or incorrect email address. Email:" + " " + query.identity,
                                EventTime = DateTime.Now
                            };
                            db.AppLogs.Add(MessageLog3);

                        }
                    }
                    else
                    {
                        var MessageLog4 = new AppLog()
                        {
                            Status = "Failed Accreditation",
                            ResponseCode = "205",
                            ResponseMessage = "Failed Accreditation Attempt - AGM not available for company. Email:" + " " + emailAddress,
                            EventTime = DateTime.Now
                        };
                        db.AppLogs.Add(MessageLog4);
                    }

                }
                else
                {
                    var MessageLog5 = new AppLog()
                    {
                        Status = "Failed Accreditation",
                        ResponseCode = "205",
                        ResponseMessage = "Failed Accreditation Attempt - Empty Company parameter or Email provided. Email:" + " " + emailAddress,
                        EventTime = DateTime.Now
                    };
                    db.AppLogs.Add(MessageLog5);
                }

                db.SaveChanges();

                return Task.FromResult<PreRegistrationResponse>(new PreRegistrationResponse()
                {
                    Status = "Success",
                    ResponseCode = "200",
                    ResponseMessage = "Post received and processed"

                });

            }
            catch (Exception e)
            {
                var MessageLog7 = new AppLog()
                {
                    Status = "Error",
                    ResponseCode = "500",
                    ResponseMessage = "Something went wrong while processing this request from " + query.identity + " " + "Error Message: " + e.Message + " " + "Please contact admin",
                    EventTime = DateTime.Now
                };
                db.AppLogs.Add(MessageLog7);
                db.SaveChanges();
                return Task.FromResult<PreRegistrationResponse>(new PreRegistrationResponse()
                {
                    Status = "Error",
                    ResponseCode = "500",
                    ResponseMessage = "Something went wrong while processing this request from " + query.identity + " " + "Error Message: " + e.Message + " " + "Please contact admin",
                });
            }

        }


        public Task<PreRegistrationResponse> GetPreregistrationRegister(PreregistrationDto[] post)
        {

            try
            {
                AppLog log;
                foreach (PreregistrationDto v in post)
                {
                    Int64 shareholdernum = 0;
                    string companyinfo = v.Company;
                    if (!string.IsNullOrEmpty(companyinfo) && v.ShareholderNum != null)
                    {
                        shareholdernum = Int64.Parse(v.ShareholderNum);
                        var UniqueAGMId = ua.RetrieveAGMUniqueID(companyinfo);
                        if (UniqueAGMId != -1)
                        {
                            var barcode = db.BarcodeStore.FirstOrDefault(u => u.Company.ToLower() == companyinfo.ToLower() && u.ShareholderNum == shareholdernum);
                            if (barcode != null)
                            {

                                var shareholder = db.Present.FirstOrDefault(u => u.AGMID == UniqueAGMId && u.ShareholderNum == barcode.ShareholderNum);

                                if (barcode.PresentByProxy != true && shareholder == null)
                                {
                                    barcode.Present = true;
                                    PresentModel present = new PresentModel();
                                    present.Name = barcode.Name;
                                    present.Address = barcode.Address;
                                    present.ShareholderNum = barcode.ShareholderNum;
                                    present.Holding = barcode.Holding;
                                    present.AGMID = UniqueAGMId;
                                    present.PercentageHolding = barcode.PercentageHolding;
                                    present.present = true;
                                    present.proxy = false;
                                    present.PresentTime = DateTime.Now;
                                    present.Timestamp = DateTime.Now.TimeOfDay;
                                    present.emailAddress = v.emailAddress;
                                    if (!String.IsNullOrEmpty(v.PhoneNumber))
                                    {
                                        if (barcode.PhoneNumber.StartsWith("234"))
                                        {
                                            present.PhoneNumber = barcode.PhoneNumber;
                                        }
                                        else if (barcode.PhoneNumber.StartsWith("0"))
                                        {
                                            var number = double.Parse(barcode.PhoneNumber);
                                            present.PhoneNumber = "234" + number.ToString();

                                        }

                                    }
                                    present.Clikapad = barcode.Clikapad;
                                    db.Present.Add(present);
                                    barcode.Date = DateTime.Today.ToString();
                                    db.Entry(barcode).State = EntityState.Modified;

                                    //var sucessoutput = JsonConvert.SerializeObject(successresponse);
                                    var MessageLog = new APIMessageLog()
                                    {
                                        Status = "successfully Accredited",
                                        ResponseCode = "200",
                                        ResponseMessage = v.ShareholderNum + " " + "Successful Accreditation. Email Address" + " " + v.emailAddress,
                                        EventTime = DateTime.Now
                                    };
                                    log = new AppLog();
                                    log.Status = MessageLog.Status;
                                    log.ResponseCode = MessageLog.ResponseCode;
                                    log.ResponseMessage = MessageLog.ResponseMessage;
                                    log.EventTime = MessageLog.EventTime;
                                    db.AppLogs.Add(log);
                                    //db.SaveChanges();
                                }
                                else
                                {
                                    var MessageLog = new APIMessageLog()
                                    {
                                        Status = "Already Accredited",
                                        ResponseCode = "202",
                                        ResponseMessage = v.ShareholderNum + " " + "Successful Accreditation. Email:" + " " + v.emailAddress,
                                        EventTime = DateTime.Now
                                    };
                                    log = new AppLog();
                                    log.Status = MessageLog.Status;
                                    log.ResponseCode = MessageLog.ResponseCode;
                                    log.ResponseMessage = MessageLog.ResponseMessage;
                                    log.EventTime = MessageLog.EventTime;
                                    db.AppLogs.Add(log);
                                }


                                //return new PreRegistrationResponse()
                                //{
                                //    Status = "Already Accredited",
                                //    ResponseCode = "02",
                                //    ResponseMessage = "Successful Accreditation"
                                //};
                            }
                            else
                            {
                                var MessageLog = new APIMessageLog()
                                {
                                    Status = "Failed",
                                    ResponseCode = "205",
                                    ResponseMessage = v.ShareholderNum + " " + "Failed Accreditation Attempt - The Company's AGM may not be enlisted or Shareholder number incorrect. Email:" + " " + v.emailAddress,
                                    EventTime = DateTime.Now
                                };
                                log = new AppLog();
                                log.Status = MessageLog.Status;
                                log.ResponseCode = MessageLog.ResponseCode;
                                log.ResponseMessage = MessageLog.ResponseMessage;
                                log.EventTime = MessageLog.EventTime;
                                db.AppLogs.Add(log);

                            }

                            //return new PreRegistrationResponse()
                            //{
                            //    Status = "Failed",
                            //    ResponseCode = "05",
                            //    ResponseMessage = "Failed Accreditation - The Company's AGM may not be enlisted or Shareholder number incorrect"
                            //};
                        }
                        else
                        {
                            var MessageLog = new APIMessageLog()
                            {
                                Status = "Failed Accreditation",
                                ResponseCode = "205",
                                ResponseMessage = v.ShareholderNum + " " + "Failed Accreditation Attempt - AGM not available for company Email:" + " " + v.emailAddress,
                                EventTime = DateTime.Now
                            };
                            log = new AppLog();
                            log.Status = MessageLog.Status;
                            log.ResponseCode = MessageLog.ResponseCode;
                            log.ResponseMessage = MessageLog.ResponseMessage;
                            log.EventTime = MessageLog.EventTime;
                            db.AppLogs.Add(log);
                        }

                        //return new PreRegistrationResponse()
                        //{
                        //    Status = "Failed Accreditation",
                        //    ResponseCode = "05",
                        //    ResponseMessage = "Failed Accreditation - AGM not available for company "
                        //};

                    }
                    else
                    {
                        var MessageLog = new APIMessageLog()
                        {
                            Status = "Failed Accreditation",
                            ResponseCode = "205",
                            ResponseMessage = "Failed Accreditation Attempt - Empty Company parameter or Shareholder Number. Email:" + " " + v.emailAddress,
                            EventTime = DateTime.Now
                        };
                        log = new AppLog();
                        log.Status = MessageLog.Status;
                        log.ResponseCode = MessageLog.ResponseCode;
                        log.ResponseMessage = MessageLog.ResponseMessage;
                        log.EventTime = MessageLog.EventTime;
                        db.AppLogs.Add(log);
                    }
                }
                db.SaveChanges();

                return Task.FromResult<PreRegistrationResponse>(new PreRegistrationResponse()
                {
                    Status = "Success",
                    ResponseCode = "200",
                    ResponseMessage = "Post received and processed"

                });
            }
            catch (Exception e)
            {
                var MessageLog = new APIMessageLog()
                {
                    Status = "Error",
                    ResponseCode = "500",
                    ResponseMessage = "Something went wrong while processing this request from " + post[0].emailAddress + " " + "Error Message: " + e.Message + " " + "Please contact admin",
                    EventTime = DateTime.Now
                };
                var log = new AppLog();
                log.Status = MessageLog.Status;
                log.ResponseCode = MessageLog.ResponseCode;
                log.ResponseMessage = MessageLog.ResponseMessage;
                log.EventTime = MessageLog.EventTime;
                db.AppLogs.Add(log);
                db.SaveChanges();
                return Task.FromResult<PreRegistrationResponse>(new PreRegistrationResponse()
                {
                    Status = "Error",
                    ResponseCode = "500",
                    ResponseMessage = "Something went wrong while processing this request from " + post[0].emailAddress + " " + "Error Message: " + e.Message + " " + "Please contact admin",
                });
            }

        }
    }
}