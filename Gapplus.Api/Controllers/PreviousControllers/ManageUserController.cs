// using BarcodeGenerator.Filters;
// using BarcodeGenerator.Models;
// using Microsoft.Web.WebPages.OAuth;
// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Data.Entity;
// using System.Linq;
// using System.Net.Mail;
// using System.Text;
// using System.Web;
// using System.Web.Configuration;
// using System.Web.Mvc;
// using System.Web.Security;
// using WebMatrix.WebData;

// namespace BarcodeGenerator.Controllers
// {
//     [Authorize]
//     [InitializeSimpleMembership]
//     public class ManageUserController : Controller
//     {
//         //
//         // GET: /ManageUser/
//         private UsersContext db = new UsersContext();

//         public ActionResult Index()
//         {
//             var users = db.UserProfiles.ToList();
//             return PartialView(users);
//         }

//         //
//         // GET: /ManageUser/Edit/5
//         public ActionResult Create()
//         {
//             var identity = new SelectList(new[] { "Admin", "User","Guest", "Messaging" });
//             ViewBag.Identity = identity;
//             if (!WebSecurity.Initialized)
//             {
//                 WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
//             }

//             RegisterModel model = new RegisterModel();

//             return PartialView(model);
//         }

//         //
//         // POST: /ManageUser/Create


//         [HttpPost]
//         //[AllowAnonymous]
//         //[Authorize(Roles = "Admin")]
//         [ValidateAntiForgeryToken]
//         public ActionResult Create(RegisterModel model, HttpPostedFileBase file)
//         {
//             if (ModelState.IsValid)
//             {

//                 // Attempt to register the user
//                 try
//                 {
//                     List<UserProfile> userlist = new List<UserProfile>();
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
//                         user.Identity = model.Identity;
//                         user.EmailId = model.EmailId;
//                         db.Entry(user).State = EntityState.Modified;
//                         db.SaveChanges();

//                         userlist.Add(user);
//                         GenerateLoginMail(user.UserName);
//                         return PartialView("SearchIndex", userlist);
//                     }
//                     return PartialView("SearchIndex", userlist);
//                 }
//                 catch (MembershipCreateUserException e)
//                 {
//                     //ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
//                     return PartialView("SearchIndex", new List<UserProfile>());
//                 }
//             }

//             // If we got this far, something failed, redisplay form
//             return PartialView("SearchIndex", new List<UserProfile>());
//         }


//         public ActionResult SelfServicePasswordReset(int id)
//         {
//             var user = db.UserProfiles.Find(id);
//             var response = GenerateLoginMail(user.UserName);
//             var message = "Mail Service";
//             if (response)
//             {
//                 message = "Password Change email Sent";
//             }
//             else
//             {
//                 message = "Mail not sent";
//             }

//             return Json(message, JsonRequestBehavior.AllowGet);
//         }

//         private bool GenerateLoginMail(string UserName)
//         {
//             //generate password token
//             var token = WebSecurity.GeneratePasswordResetToken(UserName);

//             var baseAddress = WebConfigurationManager.AppSettings["baseAddress"].Trim();
//             //create url with above token
//             var resetLink = "<a href='" + baseAddress+"ManageUser/ResetPasswordLink/"+ token +"'> Change Password</a>";
//             //get user emailid
//             UsersContext db = new UsersContext();
//             var emailid = (from i in db.UserProfiles
//                            where i.UserName == UserName
//                            select i.EmailId).FirstOrDefault();

//             var userName = (from i in db.UserProfiles
//                            where i.UserName == UserName
//                            select i.FirstName).FirstOrDefault();
//             //send mail
//             string subject = "New Account on AGM Register";
//             StringBuilder sb = new StringBuilder("<html><div>");
//             sb.AppendLine("<p>Welcome"+" "+ userName +"</p>");
//             sb.AppendLine("<p>A new account has been created for you on AGM Register that requires activation.</p>");
//             sb.AppendLine("<p>Kindly click this Link within 24hrs to activate account" + " " + resetLink + "</p>");
//             sb.AppendLine("<p></p>");
//             sb.AppendLine("<p></p>");
//             sb.AppendLine("<p>Warm regards,</p>");
//             sb.AppendLine("<p></p>");
//             sb.AppendLine("<p>From: Coronation Registrars AGM Server</p>");
//             sb.AppendLine("</div></html>");
//             string body = sb.ToString();

//             try
//             {
//                 var response = SendEMail(emailid, subject, body);
//                 TempData["Message"] = "Mail Sent.";
//                 return response;
//             }
//             catch (Exception ex)
//             {
//                 TempData["Message"] = "Error occured while sending email." + ex.Message;
//                 return false;
                
//             }

//             //return RedirectToAction("ResetPassword");
//         }

//         private bool SendEMail(string emailid, string subject, string body)
//         {
//             //MailSettings mailsetting = db.mailsettings.SingleOrDefault(m => m.ServerName == "Mail Server 1");
//             try
//             {
//                 var smtpHost = WebConfigurationManager.AppSettings["smtpServer"].Trim();
//                 var smtpPort = WebConfigurationManager.AppSettings["smtpPort"].Trim();
//                 var Username = WebConfigurationManager.AppSettings["smtpUser"].Trim();
//                 var Password = WebConfigurationManager.AppSettings["smtpPass"].Trim();
//                 var SentFrom = Username;
//                 System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
//                 client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
//                 client.EnableSsl = true;
//                 client.Host = smtpHost;
//                 client.Port = int.Parse(smtpPort);

//                 System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(Username, Password);
//                 client.UseDefaultCredentials = false;
//                 client.Credentials = credentials;

//                 System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
//                 msg.From = new MailAddress(Username);
//                 msg.To.Add(new MailAddress(emailid));

//                 msg.Subject = subject;
//                 msg.IsBodyHtml = true;
//                 msg.Body = body;

//                 client.Send(msg);

//                 return true;
//             }
//             catch(Exception e)
//             {
//                 return false;
//             }
           


           

//         }


//         public ActionResult UserIndex()
//         {
//             var User = db.UserProfiles.ToList();
//             //User.UserId = id;
//             return PartialView(User);

//         }

//         //[HttpPost]
//         public ActionResult SearchIndex(CompanyModel pmodel)
//        {
//             List<UserProfile> users = new List<UserProfile>();
            
//             if(!string.IsNullOrEmpty(pmodel.search))
//             {
//                 string userSearch = pmodel.search.ToLower();
//                 users = db.UserProfiles.Where(u => u.FullName.ToLower().Contains(userSearch)|| u.UserName.ToLower().Contains(userSearch)).ToList();
//             }

            
//             return PartialView(users);
//         }

//         public ActionResult Edit(int id)
//         {
//             var User = db.UserProfiles.Find(id);
//             User.UserId = id;
//             return PartialView(User);

//         }

//         //
//         // POST: /ManageUser/Edit/5

//         [HttpPost]
//         public ActionResult Edit(int id, UserProfile collection)
//         {
//             try
//             {
//                 // TODO: Add update logic here
//               List<UserProfile> userlist = new List<UserProfile>();
//                 if (ModelState.IsValid)
//                 {
//                   var user = db.UserProfiles.Find(collection.UserId);
//                     user.FirstName = collection.FirstName;
//                     user.LastName = collection.LastName;
//                     user.FullName = collection.FirstName + " " +collection.LastName;

//                     db.Entry(user).State = EntityState.Modified;
//                     db.SaveChanges();

//                     userlist.Add(user);
//                     //return Json(user, JsonRequestBehavior.AllowGet);
//                     return PartialView("SearchIndex", userlist);
//                 }
//                 //TempData["Message"] = "Invalid input received";
//                 return PartialView("SearchIndex", userlist);
//             }
//             catch
//             {
//                 return PartialView("SearchIndex", new List<UserProfile>());
//             }
//         }

//         //[Authorize(Roles = "Admin")]
//         public ActionResult RoleChange(int id)
//         {
//             ViewBag.Message = TempData["Message"];
//             var identity = new SelectList(new[] { "Admin", "User","Guest", "Messaging" });
//             ViewBag.Identity = identity;
//             var User = db.UserProfiles.Find(id);

//             return PartialView(User);
//         }

//         //
//         // POST: /Account/Register

//         [HttpPost]
//         //[Authorize(Roles = "Admin")]
//         [ValidateAntiForgeryToken]
//         public ActionResult RoleChange(int id, UserProfile model)
//         {
//             var identity = new SelectList(new[] { "Admin", "User","Guest","Messaging" });
//             ViewBag.Identity = identity;
//             if (ModelState.IsValid)
//             {

//                 // Attempt to register the user
//                 try
//                 {
//                     List<UserProfile> userlist = new List<UserProfile>();
//                     if (Membership.GetUser(model.UserName) != null)
//                     {
//                         //WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { EmailId = model.EmailId });
//                         //WebSecurity.Login(model.UserName, model.Password);

//                         var roles = (SimpleRoleProvider)Roles.Provider;
//                         var membership = (SimpleMembershipProvider)Membership.Provider;
//                         var userrole = roles.GetRolesForUser(model.UserName);
//                         roles.RemoveUsersFromRoles(new[] { model.UserName }, userrole);
//                         if (!roles.GetRolesForUser(model.UserName).Contains(model.Identity))
//                         {
//                             roles.AddUsersToRoles(new[] { model.UserName }, new[] { model.Identity });

//                         }

//                         var user = db.UserProfiles.Single(u => u.UserName == model.UserName);
//                         user.Identity = model.Identity;
//                         db.SaveChanges();
//                         userlist.Add(user);
//                         return PartialView("SearchIndex", userlist);

//                         //return Json(user, JsonRequestBehavior.AllowGet);
//                     }
                    
//                 }
//                 catch (MembershipCreateUserException e)
//                 {
//                     ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
//                     //return Json(new UserProfile(), JsonRequestBehavior.AllowGet);
//                     return PartialView("SearchIndex", new List<UserProfile>());
//                 }
//             }

//             // If we got this far, something failed, redisplay form
//             //return View(model);
//             return PartialView("SearchIndex", new List<UserProfile>());
//             //return Json(new UserProfile(), JsonRequestBehavior.AllowGet);
//         }




//         //
//         // GET: /ManageUser/Delete/5
//         public JsonResult Delete(int id)
//         {
//             try
//             {
//                 var user = db.UserProfiles.Find(id);
//                 if (!(Membership.GetUser(user.UserName) == null))
//                 {
//                     if (user.FullName != "Administrator")
//                     {
//                         var roles = (SimpleRoleProvider)Roles.Provider;
//                         var membership = (SimpleMembershipProvider)Membership.Provider;

//                         if (user.Identity != null)
//                         {
//                             roles.RemoveUsersFromRoles(new[] { user.UserName }, new[] { user.Identity });
//                             membership.DeleteAccount(user.UserName);
//                             db.UserProfiles.Remove(user);
//                             try
//                             {
//                                 db.SaveChanges();
//                                 var successmessage = "Profile Removed Successfully";
//                                 return Json(successmessage, JsonRequestBehavior.AllowGet);
//                             }
//                             catch (Exception e)
//                             {
//                                 var error1message = e.Message.ToString();
//                                 return Json(error1message, JsonRequestBehavior.AllowGet);
//                             }

//                         }
//                         var IdentityMessage = "Set User Identity and try again";
//                         return Json(IdentityMessage, JsonRequestBehavior.AllowGet);
//                     }

//                     var message = "Cannot Delete Default Admin";
//                     return Json(message, JsonRequestBehavior.AllowGet);
//                 }
//                 var InvalidMessage = "Invalid Input received!";
//                 return Json(InvalidMessage, JsonRequestBehavior.AllowGet);
//             }
//             catch (MembershipCreateUserException e)
//             {
//                 ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
//                 var errormessage = e.Message.ToString();
//                 return Json(errormessage, JsonRequestBehavior.AllowGet);
//             }



//         }

//         //
//         // POST: /ManageUser/Delete/5

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


//         [AllowAnonymous]
//         public ActionResult ResetPassword(int id)
//         {
//             //check user existance
//             var user = db.UserProfiles.Find(id);
//             //generate password token
//             var token = WebSecurity.GeneratePasswordResetToken(user.UserName);
//             ResetPasswordModel model = new ResetPasswordModel();
//             model.Token = token;
//             ViewBag.Name = user.FullName;
//             return PartialView(model);

//         }

//         [AllowAnonymous]
//         [HttpPost]
//         public JsonResult ResetPassword(ResetPasswordModel model)
//         {
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     bool response = WebSecurity.ResetPassword(model.Token, model.NewPassword);
//                     if (response == true)
//                     {
//                         var successmessage = "Password Reset is successful";
//                         return Json(successmessage, JsonRequestBehavior.AllowGet);
//                         //return View(model);

//                     }
//                     else
//                     {
//                         var expiredmessage = "Wrong Token or Expired!. Please Try again";
//                         return Json(expiredmessage, JsonRequestBehavior.AllowGet);
//                         //return View(model);
//                     }
//                 }
//                 catch (Exception e)
//                 {
//                     var errormessage = "Something went wrong. Please Try again";
//                     //return View(model);
//                     return Json(errormessage, JsonRequestBehavior.AllowGet);
//                 }
//             }
//             var message = "Password doesn't Match";
//             //return View(model);
//             return Json(message, JsonRequestBehavior.AllowGet);
//         }


//         [AllowAnonymous]
//         public ActionResult ResetPasswordLink(string id)
//         {
//             ResetPasswordModel model = new ResetPasswordModel();
//             model.Token = id;
//             TempData["Message"] = "";
//             return PartialView(model);
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
//                     return RedirectToAction("Index","Home");

//                 }
//                 else
//                 {
//                     TempData["Message"] = "Wrong Token or Expired!. Please Try again";
//                     return PartialView(model);
//                 }
//             }
//             catch (Exception e)
//             {
//                 TempData["Message"] = "Something went wrong. Please Try again";
//                 return PartialView(model);
//             }

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
//                 return RedirectToAction("Index", "Home");
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
