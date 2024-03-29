// using WebMatrix.WebData;
// using BarcodeGenerator.Models;
// using System.Data;
// using System.Net.Mail;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Mvc;

// namespace BarcodeGenerator.Controllers
// {
    
//     public class AccountController : ControllerBase
//     {
//         private UsersContext db;
//         public AccountController(UsersContext context)
//         {
//         db = context;
//         }
      
      
//         //
//         // POST: /Account/Login

//         [HttpPost]
//         // [AllowAnonymous]
//         // [ValidateAntiForgeryToken]
//         // [HandleError]
//         public ActionResult Login(LoginModel model, string returnUrl)
//         {

//             if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
//             {
//                 var user = db.UserProfiles.Single(u => u.UserName == model.UserName);
//                 user.CompanyInfo = "";
//                 db.Entry(user).State = EntityState.Modified;
//                 db.SaveChanges();
//                 return RedirectToLocal(returnUrl);
//             }
//             // If we got this far, something failed, redisplay form
//             ModelState.AddModelError("", "The user name or password provided is incorrect.");
//             return Ok(model);
//         }

//         //
//         // POST: /Account/LogOff

//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public ActionResult LogOff()
//         {

//             WebSecurity.Logout();

//             return RedirectToAction("Index", "Dashboard");
//         }

//         //
//         // GET: /Account/Register

//         // [Authorize(Roles = "Admin")]
//         public ActionResult Register()
//         {
//             var identity = new System.Web.Mvc.SelectList(new[] { "Admin", "User", "Guest" });
//             // ViewBag.Identity = identity;
//             if (!WebSecurity.Initialized)
//             {
//                 WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
//             }
//             return Ok();
//         }

//         public ActionResult GetCompanyInfo(string returnUrl, string returnValue)
//         {
//             // ViewBag.ReturnUrl = returnUrl;
//             // ViewBag.value = returnValue;
//             var companies = db.Settings.Where(s => s.ArchiveStatus == false).Select(o => o.CompanyName).Distinct().OrderBy(k => k).ToList();
//             // ViewBag.companies = new System.Web.Mvc.SelectList(companies ?? new List<string>());
//             // return PartialView();
//             return Ok();
//         }

//         [HttpPost]
//         public ActionResult GetCompanyInfo(CompanyModel model, string returnUrl)
//         {

//             var user = db.UserProfiles.Single(u => u.UserName == User.Identity.Name.Trim());
//             user.CompanyInfo = model.CompanyInfo;
//             db.Entry(user).State = EntityState.Modified;
//             db.SaveChanges();
//             //return RedirectToAction("Index");
//             return RedirectToLocal(returnUrl);
//         }
//         // POST: /Account/Register

//         [HttpPost]
//         // [AllowAnonymous]
//         // [ValidateAntiForgeryToken]
//         public ActionResult Register(RegisterModel model)
//         {
//             if (ModelState.IsValid)
//             {
//                 // Attempt to register the user
//                 try
//                 {

//                     if (Membership.GetUser(model.UserName) == null)
//                     {
//                         WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { EmailId = model.EmailId });
//                         //WebSecurity.Login(model.UserName, model.Password);

//                         var roles = (SimpleRoleProvider)Roles.Provider;
//                         var membership = (SimpleMembershipProvider)Membership.Provider;
//                         if (!roles.GetRolesForUser(model.UserName).Contains(model.Identity))
//                         {
//                             roles.AddUsersToRoles(new[] { model.UserName }, new[] { model.Identity });

//                         }
//                         //WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
//                         //WebSecurity.Login(model.UserName, model.Password);
//                         var user = db.UserProfiles.Single(u => u.UserName == model.UserName);
//                         user.FirstName = model.FirstName;
//                         user.LastName = model.LastName;
//                         user.FullName = model.LastName + " " + model.FirstName;
//                         db.Entry(user).State = EntityState.Modified;
//                         db.SaveChanges();
//                         return View();
//                     }
//                 }
//                 catch (MembershipCreateUserException e)
//                 {
//                     ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
//                 }
//             }

//             // If we got this far, something failed, redisplay form
//             // return View(model);
//             return BadRequest("Something is not right !");
//         }

//         // [AllowAnonymous]
//         // public ActionResult ForgotPassword()
//         // {
//         //     // return View();
//         //     return Ok();
//         // }



//         [HttpPost]
//         // [AllowAnonymous]
//         [ValidateAntiForgeryToken]
//         public ActionResult ForgotPassword(string UserName)
//         {
//             //check user existance
//             var user = Membership.GetUser(UserName);
//             if (user == null)
//             {
//                 TempData["Message"] = "User doesn't exist.";
//             }
//             else
//             {
//                 //generate password token
//                 var token = WebSecurity.GeneratePasswordResetToken(UserName);
//                 //create url with above token
//                 var resetLink = "<a href='" + Url.Action("ResetPasswordLink", "Account", new { rt = token }, "http") + "'>Reset Password</a>";
//                 //get user emailid
//                 UsersContext db = new UsersContext();
//                 var emailid = (from i in db.UserProfiles
//                                where i.UserName == UserName
//                                select i.EmailId).FirstOrDefault();
//                 //send mail
//                 string subject = "Password Reset Token";
//                 string body = "<b>Please find the Password Reset Token. Click resetLink within 24hrs as an alternative </b><br/>" + " " + resetLink + " " +
//                     "<br/><div>Reset Token:" + " " + token + "</div>";
//                 try
//                 {
//                     SendEMail(emailid, subject, body);
//                     TempData["Message"] = "Mail Sent.";
//                 }
//                 catch (Exception ex)
//                 {
//                     TempData["Message"] = "Error occured while sending email." + ex.Message;
//                 }
//                 //only for testing
//                 TempData["Message"] = "Mail has been sent to the your profile email address";
//                 return RedirectToAction("ResetPassword");
//             }

//             return View();
//         }


//         [AllowAnonymous]
//         public ActionResult ResetPassword()
//         {
//             ResetPasswordModel model = new ResetPasswordModel();
//             return View(model);
//         }

//         [AllowAnonymous]
//         [HttpPost]
//         public ActionResult ResetPassword(ResetPasswordModel model)
//         {
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     bool response = WebSecurity.ResetPassword(model.Token, model.NewPassword);
//                     if (response == true)
//                     {
//                         TempData["Message"] = "Password Reset is successful";
//                         return RedirectToAction("PasswordResetSuccess");

//                     }
//                     else
//                     {
//                         TempData["Message"] = "Wrong Token or Expired!. Please Try again";
//                         return View(model);
//                     }
//                 }
//                 catch (Exception e)
//                 {
//                     TempData["Message"] = "Something went wrong. Please Try again";
//                     return View(model);
//                 }
//             }
//             return View(model);
//         }

//         [AllowAnonymous]
//         public ActionResult ResetPasswordLink(string rt)
//         {
//             ResetPasswordModel model = new ResetPasswordModel();
//             model.Token = rt;
//             return View(model);
//         }

//         [AllowAnonymous]
//         [HttpPost]
//         public ActionResult ResetPasswordLink(ResetPasswordModel model)
//         {
//             try
//             {
//                 bool response = WebSecurity.ResetPassword(model.Token, model.NewPassword);
//                 if (response == true)
//                 {
//                     TempData["Message"] = "Password Reset is successful";
//                     return RedirectToAction("PasswordResetSuccess");

//                 }
//                 else
//                 {
//                     TempData["Message"] = "Wrong Token or Expired!. Please Try again";
//                     return View(model);
//                 }
//             }
//             catch (Exception e)
//             {
//                 TempData["Message"] = "Something went wrong. Please Try again";
//                 return View(model);
//             }

//         }

//         [AllowAnonymous]
//         public ActionResult PasswordResetSuccess()
//         {

//             return View();
//         }

//         [AllowAnonymous]
//         public ActionResult LogOffPage()
//         {

//             return View();
//         }



//         private void SendEMail(string emailid, string subject, string body)
//         {
//             MailSettings mailsetting = db.mailsettings.SingleOrDefault(m => m.ServerName == "Mail Server 1");

//             if (mailsetting == null)
//             {
//                 TempData["Message"] = "Mail Server Not Responding....Contact Administrator";

//             }
//             System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
//             client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
//             client.EnableSsl = true;
//             client.Host = mailsetting.smtpHost;
//             client.Port = mailsetting.smtpPort;


//             System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(mailsetting.Username, mailsetting.Password);
//             client.UseDefaultCredentials = false;
//             client.Credentials = credentials;

//             System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
//             msg.From = new MailAddress(mailsetting.SentFrom);
//             msg.To.Add(new MailAddress(emailid));

//             msg.Subject = subject;
//             msg.IsBodyHtml = true;
//             msg.Body = body;

//             client.Send(msg);

//         }



//         //
//         // POST: /Account/Disassociate

//         //[HttpPost]
//         //[ValidateAntiForgeryToken]
//         //public ActionResult Disassociate(string provider, string providerUserId)
//         //{
//         //    string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
//         //    ManageMessageId? message = null;

//         //    // Only disassociate the account if the currently logged in user is the owner
//         //    if (ownerAccount == User.Identity.Name)
//         //    {
//         //        // Use a transaction to prevent the user from deleting their last login credential
//         //        using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
//         //        {
//         //            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
//         //            if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
//         //            {
//         //                OAuthWebSecurity.DeleteAccount(provider, providerUserId);
//         //                scope.Complete();
//         //                message = ManageMessageId.RemoveLoginSuccess;
//         //            }
//         //        }
//         //    }

//         //    return RedirectToAction("Manage", new { Message = message });
//         //}

//         //
//         // GET: /Account/Manage

//         public ActionResult Manage(ManageMessageId? message)
//         {
//             ViewBag.StatusMessage =
//                 message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
//                 : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
//                 : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
//                 : "";
//             ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
//             ViewBag.ReturnUrl = Url.Action("Manage");
//             return View();
//         }

//         //
//         // POST: /Account/Manage

//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public ActionResult Manage(LocalPasswordModel model)
//         {
//             bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
//             ViewBag.HasLocalPassword = hasLocalAccount;
//             ViewBag.ReturnUrl = Url.Action("Manage");
//             if (hasLocalAccount)
//             {
//                 if (ModelState.IsValid)
//                 {
//                     // ChangePassword will throw an exception rather than return false in certain failure scenarios.
//                     bool changePasswordSucceeded;
//                     try
//                     {
//                         changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
//                     }
//                     catch (Exception)
//                     {
//                         changePasswordSucceeded = false;
//                     }

//                     if (changePasswordSucceeded)
//                     {
//                         return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
//                     }
//                     else
//                     {
//                         ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
//                     }
//                 }
//             }
//             else
//             {
//                 // User does not have a local password so remove any validation errors caused by a missing
//                 // OldPassword field
//                 ModelState state = ModelState["OldPassword"];
//                 if (state != null)
//                 {
//                     state.Errors.Clear();
//                 }

//                 if (ModelState.IsValid)
//                 {
//                     try
//                     {
//                         WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
//                         return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
//                     }
//                     catch (Exception)
//                     {
//                         ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
//                     }
//                 }
//             }

//             // If we got this far, something failed, redisplay form
//             return View(model);
//         }

//         //
//         // POST: /Account/ExternalLogin

//         [HttpPost]
//         [AllowAnonymous]
//         [ValidateAntiForgeryToken]
//         public ActionResult ExternalLogin(string provider, string returnUrl)
//         {
//             return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
//         }

//         //
//         // GET: /Account/ExternalLoginCallback

//         [AllowAnonymous]
//         public ActionResult ExternalLoginCallback(string returnUrl)
//         {
//             AuthenticationResult result = OAuthWebSecurity.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
//             if (!result.IsSuccessful)
//             {
//                 return RedirectToAction("ExternalLoginFailure");
//             }

//             if (OAuthWebSecurity.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
//             {
//                 return RedirectToLocal(returnUrl);
//             }

//             if (User.Identity.IsAuthenticated)
//             {
//                 // If the current user is logged in add the new account
//                 OAuthWebSecurity.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
//                 return RedirectToLocal(returnUrl);
//             }
//             else
//             {
//                 // User is new, ask for their desired membership name
//                 string loginData = OAuthWebSecurity.SerializeProviderUserId(result.Provider, result.ProviderUserId);
//                 ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(result.Provider).DisplayName;
//                 ViewBag.ReturnUrl = returnUrl;
//                 return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
//             }
//         }

//         //
//         // POST: /Account/ExternalLoginConfirmation

//         [HttpPost]
//         [AllowAnonymous]
//         [ValidateAntiForgeryToken]
//         public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
//         {
//             string provider = null;
//             string providerUserId = null;

//             if (User.Identity.IsAuthenticated || !OAuthWebSecurity.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
//             {
//                 return RedirectToAction("Manage");
//             }

//             if (ModelState.IsValid)
//             {
//                 // Insert a new user into the database
//                 using (UsersContext db = new UsersContext())
//                 {
//                     UserProfile user = db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
//                     // Check if user already exists
//                     if (user == null)
//                     {
//                         // Insert name into the profile table
//                         db.UserProfiles.Add(new UserProfile { UserName = model.UserName });
//                         db.SaveChanges();

//                         OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
//                         OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie: false);

//                         return RedirectToLocal(returnUrl);
//                     }
//                     else
//                     {
//                         ModelState.AddModelError("UserName", "User name already exists. Please enter a different user name.");
//                     }
//                 }
//             }

//             ViewBag.ProviderDisplayName = OAuthWebSecurity.GetOAuthClientData(provider).DisplayName;
//             ViewBag.ReturnUrl = returnUrl;
//             return View(model);
//         }

//         //
//         // GET: /Account/ExternalLoginFailure

//         [AllowAnonymous]
//         public ActionResult ExternalLoginFailure()
//         {
//             return View();
//         }

//         [AllowAnonymous]
//         [ChildActionOnly]
//         public ActionResult ExternalLoginsList(string returnUrl)
//         {
//             ViewBag.ReturnUrl = returnUrl;
//             return PartialView("_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData);
//         }

//         [ChildActionOnly]
//         public ActionResult RemoveExternalLogins()
//         {
//             ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
//             List<ExternalLogin> externalLogins = new List<ExternalLogin>();
//             foreach (OAuthAccount account in accounts)
//             {
//                 AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

//                 externalLogins.Add(new ExternalLogin
//                 {
//                     Provider = account.Provider,
//                     ProviderDisplayName = clientData.DisplayName,
//                     ProviderUserId = account.ProviderUserId,
//                 });
//             }

//             ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
//             return PartialView("_RemoveExternalLoginsPartial", externalLogins);
//         }

//         #region Helpers
//         private ActionResult RedirectToLocal(string returnUrl)
//         {
//             if (Url.IsLocalUrl(returnUrl))
//             {
//                 return Redirect(returnUrl);
//             }
//             else
//             {
//                 return RedirectToAction("Index", "Dashboard");
//             }
//         }

//         public enum ManageMessageId
//         {
//             ChangePasswordSuccess,
//             SetPasswordSuccess,
//             RemoveLoginSuccess,
//         }

//         internal class ExternalLoginResult : ActionResult
//         {
//             public ExternalLoginResult(string provider, string returnUrl)
//             {
//                 Provider = provider;
//                 ReturnUrl = returnUrl;
//             }

//             public string Provider { get; private set; }
//             public string ReturnUrl { get; private set; }

//             public override void ExecuteResult(ControllerContext context)
//             {
//                 OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
//             }
//         }

//         private static string ErrorCodeToString(MembershipCreateStatus createStatus)
//         {
//             // See http://go.microsoft.com/fwlink/?LinkID=177550 for
//             // a full list of status codes.
//             switch (createStatus)
//             {
//                 case MembershipCreateStatus.DuplicateUserName:
//                     return "User name already exists. Please enter a different user name.";

//                 case MembershipCreateStatus.DuplicateEmail:
//                     return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

//                 case MembershipCreateStatus.InvalidPassword:
//                     return "The password provided is invalid. Please enter a valid password value.";

//                 case MembershipCreateStatus.InvalidEmail:
//                     return "The e-mail address provided is invalid. Please check the value and try again.";

//                 case MembershipCreateStatus.InvalidAnswer:
//                     return "The password retrieval answer provided is invalid. Please check the value and try again.";

//                 case MembershipCreateStatus.InvalidQuestion:
//                     return "The password retrieval question provided is invalid. Please check the value and try again.";

//                 case MembershipCreateStatus.InvalidUserName:
//                     return "The user name provided is invalid. Please check the value and try again.";

//                 case MembershipCreateStatus.ProviderError:
//                     return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

//                 case MembershipCreateStatus.UserRejected:
//                     return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

//                 default:
//                     return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
//             }
//         }
//         #endregion
//     }
// }
