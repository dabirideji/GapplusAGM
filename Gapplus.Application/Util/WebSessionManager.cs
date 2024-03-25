using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using BarcodeGenerator.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Util
{
    public class WebSessionManager
    {
        private readonly UsersContext db;

        public WebSessionManager(UsersContext _db)
        {
            db = _db;
            agmM = new AGMManager(db);

        }
        AGMManager agmM;
        //public static Task<bool> GetLoginUserStatus(string companyinfo, string email, string sessionid)
        //{
        //    try
        //    {
        //        UsersContext db = new UsersContext();
        //        var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
        //        if (user.Sessionid == sessionid)
        //            return Task.FromResult<bool>(true);
        //        return Task.FromResult<bool>(false);
        //    }
        //    catch(Exception e)
        //    {
        //        return Task.FromResult<bool>(false);
        //    }
        //}
        public static Task<string> CreateUserLoginHistory(string companyinfo, string email, string sessionid)
        {
            try
            {

                //var password = Crypto.HashPassword(newPassword);
                //var userid = WebSecurity.GetUserId(name);
                //UserPasswordHistory passwordH = new UserPasswordHistory
                //{
                //    Password = Crypto.HashPassword(newPassword),
                //    PasswordVersion = DateTime.UtcNow
                //};
                string oldSessionVersion = "";
                // var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new BarcodeModel();
                if (user != null)
                {
                    oldSessionVersion = user.SessionVersion;
                    user.UserLoginHistory = false;
                    user.Sessionid = sessionid;
                    user.SessionVersion = DateTime.UtcNow.ToString();
                    // db.SaveChanges();
                    return Task.FromResult<string>(oldSessionVersion);
                }
                return Task.FromResult<string>("");
            }
            catch (Exception e)
            {
                return Task.FromResult<string>("");
            }
            //throw new NotImplementedException();
        }

        public static Task<bool> LogoutUserLoginHistory(string companyinfo, string email)
        {
            try
            {
                //var password = Crypto.HashPassword(newPassword);
                //var userid = WebSecurity.GetUserId(name);
                //UserPasswordHistory passwordH = new UserPasswordHistory
                //{
                //    Password = Crypto.HashPassword(newPassword),
                //    PasswordVersion = DateTime.UtcNow
                //};
                // var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new BarcodeModel();
                if (user != null)
                {
                    user.UserLoginHistory = false;
                    user.Sessionid = "";
                    user.SessionVersion = "";
                    // db.SaveChanges();
                    return Task.FromResult<bool>(true);
                }
                return Task.FromResult<bool>(false);

            }
            catch (Exception e)
            {
                return Task.FromResult<bool>(false);
            }
            //throw new NotImplementedException();
        }

        public static bool CheckUserLoginHistory(string companyinfo, string email, string sessionid)
        {
            try
            {
                //var password = Crypto.HashPassword(newPassword);
                //var userid = WebSecurity.GetUserId(name);
                // var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new BarcodeModel();
                //var passwords = (userProfile.UserPasswordHistory
                //                          .OrderByDescending(x => x.Id)
                //                          .Take(1))
                //                          .Select(x => x.Password);
                //var value = passwords.Any(previousPassword => Crypto.VerifyHashedPassword(previousPassword, newPassword));
                if (user != null && user.Sessionid == sessionid)
                {
                    return true;
                }
                else if (user != null && string.IsNullOrEmpty(user.Sessionid))
                {
                    return false;
                }

                return false;
            }

            catch (Exception e)
            {
                return false;
            }
            //throw new NotImplementedException();
        }

        public static Task<bool> CheckUserLoginHistoryAsync(string companyinfo, string email, string sessionid)
        {
            try
            {
                //var password = Crypto.HashPassword(newPassword);
                //var userid = WebSecurity.GetUserId(name);
                // var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new BarcodeModel();
                //var passwords = (userProfile.UserPasswordHistory
                //                          .OrderByDescending(x => x.Id)
                //                          .Take(1))
                //                          .Select(x => x.Password);
                //var value = passwords.Any(previousPassword => Crypto.VerifyHashedPassword(previousPassword, newPassword));
                if (user != null && user.Sessionid == sessionid)
                {
                    return Task.FromResult<bool>(true);
                }
                else if (user != null && string.IsNullOrEmpty(user.Sessionid))
                {
                    return Task.FromResult<bool>(true);
                }

                return Task.FromResult<bool>(false);
            }

            catch (Exception e)
            {
                return Task.FromResult<bool>(false);
            }
            //throw new NotImplementedException();
        }

        public static Task<bool> GetShareholderLoginStatus(string companyinfo, string email)
        {
            try
            {

                // var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new Facilitators();
                if (user != null)
                {
                    return Task.FromResult<bool>(user.UserLoginHistory);
                }
                return Task.FromResult<bool>(false);
            }
            catch (Exception e)
            {
                return Task.FromResult<bool>(false);
            }
            //throw new NotImplementedException();
        }

        public static Task<byte> GetShareholderAttendanceStatus(int agmid, string email)
        {
            try
            {

                // var user = db.Present.FirstOrDefault(x => x.AGMID == agmid && x.emailAddress == email);
                var user = new PresentModel();

                if (user != null)
                {
                    return Task.FromResult<byte>(user.PermitPoll);
                }
                return Task.FromResult<byte>(0);
            }
            catch (Exception e)
            {
                return Task.FromResult<byte>(0);
            }
            //throw new NotImplementedException();
        }


        public static Task<string> GetShareholderSessionVersion(string companyinfo, string email)
        {
            try
            {

                // var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new BarcodeModel();
                if (user != null)
                {
                    return Task.FromResult<string>(user.SessionVersion);
                }
                return Task.FromResult<string>("");

            }
            catch (Exception e)
            {
                return Task.FromResult<string>("");
            }
            //throw new NotImplementedException();
        }



        public Task<bool> GetShareholderRegistrationStatus(string company, string email)
        {


            List<BarcodeModel> multipleEntry = new List<BarcodeModel>();
            //var shareholder = db.BarcodeStore.Find(id);
            var count = 0;


            if (!String.IsNullOrEmpty(email) || !String.IsNullOrEmpty(company))
            {
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                SqlConnection conn =
                        new SqlConnection(connStr);
                string query2 = "select * from BarcodeModels WHERE Company =  '" + company + "' AND  emailAddress = '" + email + "'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query2, conn);
                SqlDataReader read = cmd.ExecuteReader();

                while (read.Read())
                {
                    BarcodeModel model = new BarcodeModel
                    {

                        SN = (Int64.Parse(read["SN"].ToString())),
                        Id = int.Parse(read["Id"].ToString()),
                        Name = (read["Name"].ToString()),
                        Address = (read["Address"].ToString()),
                        ShareholderNum = (Int64.Parse(read["ShareholderNum"].ToString())),
                        ParentAccountNumber = (Int64.Parse(read["ParentAccountNumber"].ToString())),
                        Holding = double.Parse(read["Holding"].ToString()),
                        PercentageHolding = double.Parse(read["PercentageHolding"].ToString()),
                        Company = (read["Company"].ToString()),
                        PhoneNumber = (read["PhoneNumber"].ToString()),
                        emailAddress = (read["emailAddress"].ToString()),
                        Present = bool.Parse(read["Present"].ToString()),
                        PresentByProxy = bool.Parse(read["PresentByProxy"].ToString()),
                        split = bool.Parse(read["split"].ToString()),
                        resolution = bool.Parse(read["resolution"].ToString()),
                        Clikapad = (read["Clikapad"].ToString()),
                        TakePoll = bool.Parse(read["TakePoll"].ToString()),
                        Consolidated = bool.Parse(read["Consolidated"].ToString()),
                        ConsolidatedParent = read["ConsolidatedParent"].ToString(),
                        ConsolidatedValue = read["ConsolidatedValue"].ToString(),
                        Preregistered = bool.Parse(read["Preregistered"].ToString())

                    };
                    multipleEntry.Add(model);
                }
                read.Close();
                //var multipleEntry = db.BarcodeStore.Where(s => s.emailAddress == shareholder.emailAddress).ToList();


                BarcodeModel srecord;
                if (multipleEntry.Count > 1)
                {

                    var consolidatedvalue = multipleEntry.Where(s => s.Consolidated == true).Select(s => s.ConsolidatedValue).FirstOrDefault();

                    srecord = agmM.GetConsolidatedAccount(company, consolidatedvalue);


                }
                else
                {
                    srecord = multipleEntry.FirstOrDefault();
                }

                return Task.FromResult(srecord.Preregistered);


            }
            return Task.FromResult(false);

        }
        public static bool ShareholderLoginHistoryConfirmation(string companyinfo, string email, bool status)
        {
            try
            {

                // var user = db.BarcodeStore.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new Facilitators();
                if (user != null)
                {
                    user.UserLoginHistory = status;
                    // db.SaveChanges();
                    return true;
                }
                return false;

            }
            catch (Exception e)
            {
                return false;
            }
            //throw new NotImplementedException();
        }

        public static Task<string> CreateFacilitatorLoginHistory(string companyinfo, string email, string sessionid)
        {
            try
            {
                //var password = Crypto.HashPassword(newPassword);
                //var userid = WebSecurity.GetUserId(name);
                //UserPasswordHistory passwordH = new UserPasswordHistory
                //{
                //    Password = Crypto.HashPassword(newPassword),
                //    PasswordVersion = DateTime.UtcNow
                //};
                string oldSessionVersion = "";
                // var user = db.Facilitators.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new Facilitators();
                if (user != null)
                {

                    oldSessionVersion = user.SessionVersion;
                    user.UserLoginHistory = false;
                    user.Sessionid = sessionid;
                    user.SessionVersion = DateTime.UtcNow.ToString();
                    // db.SaveChanges();
                    return Task.FromResult<string>(oldSessionVersion);
                }
                return Task.FromResult<string>("");

            }
            catch (Exception e)
            {
                return Task.FromResult<string>("");
            }
            //throw new NotImplementedException();
        }

        public static Task<bool> GetFacilatorLoginStatus(string companyinfo, string email)
        {
            try
            {

                // var user = db.Facilitators.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new Facilitators();
                if (user != null)
                {
                    return Task.FromResult<bool>(user.UserLoginHistory);
                }
                return Task.FromResult<bool>(false);
            }
            catch (Exception e)
            {
                return Task.FromResult<bool>(false);
            }
            //throw new NotImplementedException();
        }



        public static Task<string> GetFacilatorSessionVersion(string companyinfo, string email)
        {
            try
            {

                // var user = db.Facilitators.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new Facilitators();
                if (user != null)
                {
                    return Task.FromResult<string>(user.SessionVersion);
                }
                return Task.FromResult<string>("");
            }
            catch (Exception e)
            {
                return Task.FromResult<string>("");
            }
            //throw new NotImplementedException();
        }

        public static bool FacilitatorLoginHistoryConfirmation(string companyinfo, string email, bool status)
        {
            try
            {

                // var user = db.Facilitators.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new Facilitators();
                if (user != null)
                {
                    user.UserLoginHistory = status;
                    // db.SaveChanges();

                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
            //throw new NotImplementedException();
        }

        public static Task<bool> LogoutFacilitatorLoginHistory(string companyinfo, string email)
        {
            try
            {
                //var password = Crypto.HashPassword(newPassword);
                //var userid = WebSecurity.GetUserId(name);
                //UserPasswordHistory passwordH = new UserPasswordHistory
                //{
                //    Password = Crypto.HashPassword(newPassword),
                //    PasswordVersion = DateTime.UtcNow
                //};
                // var user = db.Facilitators.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new Facilitators();
                if (user != null)
                {
                    user.UserLoginHistory = false;
                    user.Sessionid = "";
                    user.SessionVersion = "";
                    // db.SaveChanges();
                    return Task.FromResult<bool>(true);
                }
                return Task.FromResult<bool>(false);
            }
            catch (Exception e)
            {
                return Task.FromResult<bool>(false);
            }
            //throw new NotImplementedException();
        }

        public static bool CheckFacilitatorLoginHistory(string companyinfo, string email, string sessionid)
        {
            try
            {
                //var password = Crypto.HashPassword(newPassword);
                //var userid = WebSecurity.GetUserId(name);
                // var user = db.Facilitators.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new Facilitators();
                //var passwords = (userProfile.UserPasswordHistory
                //                          .OrderByDescending(x => x.Id)
                //                          .Take(1))
                //                          .Select(x => x.Password);
                //var value = passwords.Any(previousPassword => Crypto.VerifyHashedPassword(previousPassword, newPassword));
                if (user != null && user.Sessionid == sessionid)
                {
                    return true;
                }
                else if (user != null && string.IsNullOrEmpty(user.Sessionid))
                {
                    return false;
                }
                return false;

            }
            catch (Exception e)
            {
                return false;
            }
            //throw new NotImplementedException();
        }


        public static Task<bool> CheckFacilitatorLoginHistoryAsync(string companyinfo, string email, string sessionid)
        {
            try
            {
                //var password = Crypto.HashPassword(newPassword);
                //var userid = WebSecurity.GetUserId(name);
                // var user = db.Facilitators.FirstOrDefault(x => x.Company == companyinfo && x.emailAddress == email);
                var user = new Facilitators();
                //var passwords = (userProfile.UserPasswordHistory
                //                          .OrderByDescending(x => x.Id)
                //                          .Take(1))
                //                          .Select(x => x.Password);
                //var value = passwords.Any(previousPassword => Crypto.VerifyHashedPassword(previousPassword, newPassword));
                if (user != null && user.Sessionid == sessionid)
                {
                    return Task.FromResult<bool>(true);
                }
                else if (user != null && string.IsNullOrEmpty(user.Sessionid))
                {
                    return Task.FromResult<bool>(false);
                }
                return Task.FromResult<bool>(false);

            }
            catch (Exception e)
            {
                return Task.FromResult<bool>(false);
            }
            //throw new NotImplementedException();
        }

    }
}