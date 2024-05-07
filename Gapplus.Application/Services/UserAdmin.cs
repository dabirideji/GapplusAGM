using BarcodeGenerator.Models;
using BarcodeGenerator.Models.ModelDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Service
{

    public interface IUserAdmin{
         string GetUserCompanyInfo();
         string GetUserCompanyInfo(string identity);
         int RetrieveAGMUniqueID();
         int RetrieveAGMUniqueID(string companyinfo);
         int RetrieveFromArchiveAGMUniqueID(string companyinfo);
         string GetAccessCode();
         Task<string> GetPregistrationToken();
         Task<GenericResponseDto<BarcodeModel>> GetShareHolderAccount(string company, string email);
         bool GetAbstainBtnChoice(int UniqueAGMId);
         Task<bool> CheckIfClikapadIsAssigned(string clikapad);
    }

    public class UserAdmin:IUserAdmin
    {

       private readonly UsersContext db;

        public UserAdmin(UsersContext _db)
        {
            db = _db;
            agmmanager = new AGMManager(db);
        }
        AGMManager agmmanager ;


        public string GetUserCompanyInfo()
        {
            // var identity = HttpContext.Current.User.Identity.Name.Trim();
            // var identity ="";
            var identity=SessionManager.GetSessionData<String>("Name");            UsersContext dbd = db;
            UserProfile user;
            string companyinfo = "";
            try
            {
                if (!string.IsNullOrEmpty(identity))
                {
                    user = dbd.UserProfiles.SingleOrDefault(u => u.UserName == identity);
                    companyinfo = user.CompanyInfo;
                }

                return companyinfo;

            }
            catch(Exception e)
            {
                return companyinfo;

            }
        }

        public string GetUserCompanyInfo(string identity)
        {
            string companyinfo = "";
            try
            {
                if (!string.IsNullOrEmpty(identity))
                {
                    UsersContext dbd = db;
                    UserProfile user;

                    if (!string.IsNullOrEmpty(identity))
                    {
                        user = dbd.UserProfiles.SingleOrDefault(u => u.UserName == identity);
                        companyinfo = user.CompanyInfo;
                    }

                    return companyinfo;
                }
                return companyinfo;

            }catch(Exception e)
            {
                return companyinfo;
            }
            //var identity = HttpContext.Current.User.Identity.Name.Trim();

        }




        public int RetrieveAGMUniqueID()
        {
            UsersContext adb = db;
            var companyinfo = GetUserCompanyInfo();
            int AGMID =-1;
            try
            {
                if (string.IsNullOrEmpty(companyinfo) || string.IsNullOrWhiteSpace(companyinfo))
                {
                    return AGMID;
                }
                var AGMIDList = adb.Settings.Where(s => s.CompanyName == companyinfo && s.ArchiveStatus == false);
                if (AGMIDList.Any())
                {
                    AGMID = AGMIDList.OrderByDescending(ai => ai.AGMID).FirstOrDefault().AGMID;
                }

                return AGMID;

            }
            catch(Exception e)
            {
                return AGMID;
            }

        }



        public int RetrieveAGMUniqueID(string companyinfo)
        {
            UsersContext adb = db;
            //var companyinfo = GetUserCompanyInfo();
            var AGMID = -1;
            try
            {
              
                if (string.IsNullOrEmpty(companyinfo) || string.IsNullOrWhiteSpace(companyinfo))
                {
                    return AGMID;
                }

                // var agmid = adb.Settings.Where(s => s.CompanyName.ToLower() == companyinfo.ToLower() && s.ArchiveStatus == false).OrderByDescending(ai => ai.AGMID).First().AGMID;
                
                var FakeSettings=SettingsModelFakeData.GenerateFakeSettingsData();
                
                var agm = FakeSettings.Where(s => s.CompanyName.ToLower() == companyinfo.ToLower() && s.ArchiveStatus == false).OrderByDescending(ai => ai.AGMID).First();
                var agmid=agm.AGMID;
                if(agmid!=0 && agmid!=null)
                {
                    return AGMID = agmid;
                }
                return AGMID;

            }
            catch(Exception e)
            {
                return AGMID;
            }

        }


        public int RetrieveFromArchiveAGMUniqueID(string companyinfo)
        {
            UsersContext adb = db;
            //var companyinfo = GetUserCompanyInfo();
            var AGMID = -1;
            try
            {

                if (string.IsNullOrEmpty(companyinfo) || string.IsNullOrWhiteSpace(companyinfo))
                {
                    return AGMID;
                }

                var agmid = adb.SettingsArchive.Where(s => s.CompanyName.ToLower() == companyinfo.ToLower() && s.ArchiveStatus == false).OrderByDescending(ai => ai.AGMID).First().AGMID;
                if (agmid != null)
                {
                    return AGMID = agmid;
                }
                return AGMID;

            }
            catch (Exception e)
            {
                return AGMID;
            }

        }
        public string GetAccessCode()
        {
            try
            {
                var accesscode = Guid.NewGuid().ToString().Remove(8).ToUpper();

                return accesscode;

            }
            catch(Exception e)
            {
                return "";

            }

        }

        public Task<string> GetPregistrationToken()
        {
            try
            {
                var accesscode = Guid.NewGuid().ToString().Remove(6).ToUpper();

                return Task.FromResult(accesscode);

            }
            catch (Exception e)
            {
                return Task.FromResult(string.Empty);

            }

        }

        public  Task<GenericResponseDto<BarcodeModel>> GetShareHolderAccount(string company, string email)
        {
            try
            {
                List<BarcodeModel> multipleEntry = new List<BarcodeModel>();
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
                        ConsolidatedValue = read["ConsolidatedValue"].ToString()

                    };
                    multipleEntry.Add(model);
                }
                read.Close();

                BarcodeModel cshareholderRecord;

                if (multipleEntry.Count == 0)
                {
                    return Task.FromResult(new GenericResponseDto<BarcodeModel>
                    {
                        Code = 201,
                        Status = false,
                        Message = "Email provided is not registered.",
                        
                    });
                }
                    var checkIfAnyAccountIsProxy = multipleEntry.Any(s => s.PresentByProxy == true);
                    if (checkIfAnyAccountIsProxy)
                    {
                        return Task.FromResult(new GenericResponseDto<BarcodeModel>
                        {
                            Code = 205,
                            Status = false,
                            Message = "Shareholder account has registered as proxy.",
                            
                        });
                    }
                    if (multipleEntry.Count > 1)
                    {
                        var checkifConsolidate = multipleEntry.Any(c => c.Consolidated == true);
                        if (!checkifConsolidate)
                        {
                            cshareholderRecord = agmmanager.ConsolidateRequest(company, multipleEntry);
                        }
                        else
                        {
                            var consolidatedvalue = multipleEntry.Where(s => s.Consolidated == true).Select(s => s.ConsolidatedValue).FirstOrDefault();

                            cshareholderRecord = agmmanager.GetConsolidatedAccount(company, consolidatedvalue);
                        }
                    }
                    else
                    {
                        cshareholderRecord = multipleEntry.FirstOrDefault();
                    }

                    return Task.FromResult(new GenericResponseDto<BarcodeModel>
                {
                    Code = 200,
                    Status = true,
                    Message = "success",
                    Data = cshareholderRecord
                    });
            }
            catch(Exception ex)
            {
                return Task.FromResult(new GenericResponseDto<BarcodeModel>
                {
                    Code = 500,
                    Status = false,
                    Message = "The system could not fetch shareholder records."
                });
            }
            
        }



        public bool GetAbstainBtnChoice(int UniqueAGMId)
        {
            var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);

            var abstainbtnchoice = true;
            if (agmEventSetting != null && agmEventSetting.AbstainBtnChoice != null)
            {
                abstainbtnchoice = (bool)agmEventSetting.AbstainBtnChoice;
            }

            return abstainbtnchoice;
        }



        public Task<bool> CheckIfClikapadIsAssigned(string clikapad)
        {
            bool status;
            var pad = clikapad;
            if (string.IsNullOrEmpty(pad) || pad == "0")
            {
                status = false;
                return Task.FromResult(status);
            }

            status = db.BarcodeStore.Any(p => p.Clikapad.Trim() == pad.Trim()) || db.Present.Any(p => p.Clikapad.Trim() == pad.Trim());
            return Task.FromResult(status);
        }
        //public string GetIndexUserCompanyInfo(string returnUrl)
        //{
        //    var identity = HttpContext.Current.User.Identity.Name.Trim();
        //    var user = db.UserProfiles.SingleOrDefault(u => u.UserName == identity);
        //    var companyinfo = user.CompanyInfo != null ? user.CompanyInfo : "";

        //    return companyinfo;
        //}

    }

}