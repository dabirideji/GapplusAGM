// using BarcodeGenerator.Models;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using System.Web;

// namespace BarcodeGenerator.Util
// {
//     private class MobileWebVotingSessionManager
//     {
//         private UsersContext db ;

//         public MobileWebVotingSessionManager(UsersContext _db)
//         {
//             db = _db;
            
//         }




//         public static Task<string> CreateUserLoginHistory(string companyinfo, long shareholderNum, string sessionid)
//         {
//             try
//             {
//                 //var password = Crypto.HashPassword(newPassword);
//                 //var userid = WebSecurity.GetUserId(name);
//                 //UserPasswordHistory passwordH = new UserPasswordHistory
//                 //{
//                 //    Password = Crypto.HashPassword(newPassword),
//                 //    PasswordVersion = DateTime.UtcNow
//                 //};
//                 string oldSessionVersion = "";
//                 var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.ShareholderNum == shareholderNum);
//                 if (user != null)
//                 {
//                     oldSessionVersion = user.SessionVersion;
//                     user.UserLoginHistory = false;
//                     user.Sessionid = sessionid;
//                     user.SessionVersion = DateTime.UtcNow.ToString();
//                     db.SaveChanges();
//                     return Task.FromResult<string>(oldSessionVersion);
//                 }
//                 return Task.FromResult<string>("");
//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<string>("");
//             }
//             //throw new NotImplementedException();
//         }

//         public static Task<bool> LogoutUserLoginHistory(string companyinfo, long shareholderNum)
//         {
//             try
//             {
//                 //var password = Crypto.HashPassword(newPassword);
//                 //var userid = WebSecurity.GetUserId(name);
//                 //UserPasswordHistory passwordH = new UserPasswordHistory
//                 //{
//                 //    Password = Crypto.HashPassword(newPassword),
//                 //    PasswordVersion = DateTime.UtcNow
//                 //};
//                 var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.ShareholderNum == shareholderNum);
//                 if (user != null)
//                 {
//                     user.UserLoginHistory = false;
//                     user.Sessionid = "";
//                     user.SessionVersion = "";
//                     db.SaveChanges();
//                     return Task.FromResult<bool>(true);
//                 }
//                 return Task.FromResult<bool>(false);

//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<bool>(false);
//             }
//             //throw new NotImplementedException();
//         }

//         public static bool CheckUserLoginHistory(string companyinfo, long shareholderNum, string sessionid)
//         {
//             var alreadylogin = false;
//             try
//             {
                
//                 //var password = Crypto.HashPassword(newPassword);
//                 //var userid = WebSecurity.GetUserId(name);
//                 var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.ShareholderNum == shareholderNum);
//                 //var passwords = (userProfile.UserPasswordHistory
//                 //                          .OrderByDescending(x => x.Id)
//                 //                          .Take(1))
//                 //                          .Select(x => x.Password);
//                 //var value = passwords.Any(previousPassword => Crypto.VerifyHashedPassword(previousPassword, newPassword));
//                 if (user != null && user.Sessionid != sessionid)
//                 {
//                     alreadylogin = true;
//                 }
//                 if (user != null && user.Sessionid == null)
//                 {
//                     alreadylogin = false;
//                 }
//                 if (user != null && user.Sessionid == "")
//                 {
//                     alreadylogin = false;
//                 }
//                 return alreadylogin;
//             }
//             catch (Exception e)
//             {
//                 return alreadylogin;
//             }
//             //throw new NotImplementedException();
//         }

//         public static Task<bool> CheckUserLoginHistoryAsync(string companyinfo, long shareholderNum, string sessionid)
//         {
//             var alreadylogin = false;
//             try
//             {
//                 //var password = Crypto.HashPassword(newPassword);
//                 //var userid = WebSecurity.GetUserId(name);
//                 var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.ShareholderNum == shareholderNum);
//                 //var passwords = (userProfile.UserPasswordHistory
//                 //                          .OrderByDescending(x => x.Id)
//                 //                          .Take(1))
//                 //                          .Select(x => x.Password);
//                 //var value = passwords.Any(previousPassword => Crypto.VerifyHashedPassword(previousPassword, newPassword));
//                 if (user != null && user.Sessionid != sessionid)
//                 {
//                     alreadylogin = true;
//                 }
//                 if (user != null && user.Sessionid == null)
//                 {
//                     alreadylogin = false;
//                 }
//                 if (user != null && user.Sessionid == "")
//                 {
//                     alreadylogin = false;
//                 }

//                 return Task.FromResult<bool>(alreadylogin);
//             }

//             catch (Exception e)
//             {
//                 return Task.FromResult<bool>(alreadylogin);
//             }
//             //throw new NotImplementedException();
//         }

//         public static Task<bool> GetShareholderLoginStatus(string companyinfo, long shareholderNum)
//         {
//             try
//             {
//                 UsersContext db = new UsersContext();

//                 var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.ShareholderNum == shareholderNum);
//                 if (user != null)
//                 {
//                     return Task.FromResult<bool>(user.UserLoginHistory);
//                 }
//                 return Task.FromResult<bool>(false);
//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<bool>(false);
//             }
//             //throw new NotImplementedException();
//         }

//         public static Task<byte> GetShareholderAttendanceStatus(int agmid, long shareholderNum)
//         {
//             try
//             {

//                 var user = db.Present.FirstOrDefault(x => x.AGMID == agmid && x.ShareholderNum == shareholderNum);

//                 if (user != null)
//                 {
//                     return Task.FromResult<byte>(user.PermitPoll);
//                 }
//                 return Task.FromResult<byte>(0);
//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<byte>(0);
//             }
//             //throw new NotImplementedException();
//         }


//         public static string GetShareholderSessionVersion(string companyinfo, long shareholderNum)
//         {
//             try
//             {

//                 var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.ShareholderNum == shareholderNum);
//                 if (user != null)
//                 {
//                     return user.SessionVersion;
//                 }
//                 return "";

//             }
//             catch (Exception e)
//             {
//                 return "";
//             }
//             //throw new NotImplementedException();
//         }

//         public static Task<string> GetShareholderSessionVersionAsync(string companyinfo, long shareholderNum)
//         {
//             try
//             {

//                 var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.ShareholderNum == shareholderNum);
//                 if (user != null)
//                 {
//                     return Task.FromResult<string>(user.SessionVersion);
//                 }
//                 return Task.FromResult<string>("");

//             }
//             catch (Exception e)
//             {
//                 return Task.FromResult<string>("");
//             }
//             //throw new NotImplementedException();
//         }
//         public static bool ShareholderLoginHistoryConfirmation(string companyinfo, long shareholderNum, bool status)
//         {
//             try
//             {
//                 UsersContext db = new UsersContext();

//                 var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.ShareholderNum == shareholderNum);
//                 if (user != null)
//                 {
//                     user.UserLoginHistory = status;
//                     db.SaveChanges();
//                     return true;
//                 }
//                 return false;

//             }
//             catch (Exception e)
//             {
//                 return false;
//             }
//             //throw new NotImplementedException();
//         }

       
//     }
// }