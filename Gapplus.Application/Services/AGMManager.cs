using BarcodeGenerator.Models;
using Gapplus.Application.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Service
{

    public interface IAGMManager
    {
        BarcodeModel ConsolidateRequest(string companyinfo, List<BarcodeModel> consolidees);
        string UnConsolidateAsync(string companyinfo, int agmid, BarcodeModel consolidated);
        BarcodeModel GetConsolidatedAccount(string companyinfo, string consolidatedvalue);
        public string GetAGMDecision(int forCount, int againstCount, int abstainCount, int voidCount);
    }

    public class AGMManager:IAGMManager
    {
        private UsersContext db;

        public AGMManager(UsersContext _db)
        {
            db = _db;
        }
        // private static string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static string connStr =ConfigurationHelper.GetConnectionString();
        SqlConnection conn =
                  new SqlConnection(connStr);

        public BarcodeModel ConsolidateRequest(string companyinfo, List<BarcodeModel> consolidees)
        {
            try
            {
                //var companyinfo = ua.GetUserCompanyInfo();
                //var jvalues = JsonConvert.DeserializeObject(val);
                //JArray a = JArray.Parse(val);

                //Jvar IdsJson = Json.Parse(jvalues);
                //var value = jvalues.Split(',');
                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                Int64 maxvalue = 0;
                SqlConnection conn =
                        new SqlConnection(connStr);
                string query = "SELECT MAX(ShareholderNum) FROM BarcodeModels WHERE Company='" + companyinfo + "'";
                //string query2 = "select * from BarcodeModels WHERE Name LIKE '%" + searchValue + "%' OR ShareholderNum LIKE '%" + searchValue + "%'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader read = cmd.ExecuteReader();

                //Int64 maxvalue = 0;
                while (read.Read())
                {
                    maxvalue = read.GetInt64(0);
                }
                read.Close();
                conn.Close();

                var setting = db.Settings.ToArray();
                //var ConsolidatedAccounts = db.BarcodeStore.Where(b => b.Company == companyinfo && b.Selected == true);
                var ConsolidatedAccounts = consolidees;
                List<BarcodeModel> ConsolidatedList = new List<BarcodeModel>();
                ConsolidatedList.Clear();
                //var consolidatedIds = mod;
                //
                foreach (var item in ConsolidatedAccounts)
                {
                    var id = item.Id;
                    var consolidatedItem = db.BarcodeStore.Find(id);
                    if (consolidatedItem != null)
                    {

                        consolidatedItem.Consolidated = true;
                        consolidatedItem.ConsolidatedValue = (maxvalue + 1).ToString();
                        consolidatedItem.Selected = false;
                        consolidatedItem.NotVerifiable = true;
                        //consolidatedItem.PresentByProxy = true;
                        //consolidatedItem.AddedSplitAccount = false;
                        consolidatedItem.split = false;

                        ConsolidatedList.Add(consolidatedItem);

                    }
                }
                //foreach (var consolidatedAcount in ConsolidatedAccounts)
                //{
                //    var id = consolidatedAcount.Id;

                //}         

                BarcodeModel consolidatedAccount = new BarcodeModel();
                foreach (var item in ConsolidatedList)
                {
                    consolidatedAccount.Holding += item.Holding;
                    consolidatedAccount.PercentageHolding += item.PercentageHolding;
                }
                consolidatedAccount.Name = ConsolidatedList[0].Name;
                consolidatedAccount.Address = ConsolidatedList[0].Address;
                consolidatedAccount.emailAddress = ConsolidatedList[0].emailAddress;
                consolidatedAccount.Address = ConsolidatedList[0].Address;
                consolidatedAccount.ConsolidatedValue = (maxvalue + 1).ToString();
                consolidatedAccount.ShareholderNum = (maxvalue + 1);
                consolidatedAccount.ConsolidatedParent = "true";
                consolidatedAccount.Company = ConsolidatedList[0].Company;
                //consolidatedAccount.PresentByProxy = false;
                consolidatedAccount.PhoneNumber = ConsolidatedList[0].PhoneNumber;
                consolidatedAccount.Clikapad = ConsolidatedList[0].Clikapad;

                db.BarcodeStore.Add(consolidatedAccount);


                db.SaveChanges();

                return consolidatedAccount;
            }
            catch (Exception e)
            {
                return new BarcodeModel();
            }
        }


        public string UnConsolidateAsync(string companyinfo, int agmid, BarcodeModel consolidated)
        {
            try
            {
                if (consolidated != null)
                {
                    var consolidatedAcct = db.BarcodeStore.Find(consolidated.Id);
                    var resolutionTaken = db.Result.Where(r => r.Company == companyinfo && r.AGMID == agmid && r.ShareholderNum == consolidated.ShareholderNum).ToArray();

                    if (resolutionTaken.Any())
                    {
                        db.Result.RemoveRange(resolutionTaken);
                    }
                    var consolidatedAccount = db.Present.FirstOrDefault(p => p.Company == companyinfo && p.AGMID == agmid && p.ShareholderNum == consolidated.ShareholderNum);
                    if (consolidatedAccount != null)
                    {
                        db.Present.Remove(consolidatedAccount);
                    }

                    var allconsolidated = db.BarcodeStore.Where(a => a.Company == companyinfo && a.ConsolidatedValue == consolidated.ConsolidatedValue);
                    foreach (var consolidate in allconsolidated)
                    {
                        consolidate.Consolidated = false;
                        consolidate.NotVerifiable = false;
                        //consolidate.PresentByProxy = false;
                        consolidate.ConsolidatedValue = "";
                        db.Entry(consolidate).State = EntityState.Modified;
                    }
                    db.BarcodeStore.Remove(consolidatedAcct);
                    //}
                    db.SaveChanges();
                    return "success";
                }
                return "Not a Consolidated Account";
            }
            catch (Exception e)
            {
                return "Error while Unconsolidating Account. " + e.StackTrace;
            }

        }


        public BarcodeModel GetConsolidatedAccount(string companyinfo, string consolidatedvalue)
        {
            var shareholdernum = Int64.Parse(consolidatedvalue);
            var shareholder = db.BarcodeStore.FirstOrDefault(s => s.Company == companyinfo && s.ShareholderNum == shareholdernum);

            return shareholder;
        }


        public string GetAGMDecision(int forCount, int againstCount, int abstainCount, int voidCount)
        {
            int[] numbers = new int[] { forCount, againstCount, abstainCount, voidCount };
            int maximumNumber = numbers.Max();
            if (forCount == maximumNumber)
            {
                return "FOR";
            }
            else if (againstCount == maximumNumber)
            {
                return "AGAINST";
            }
            else if (abstainCount == maximumNumber)
            {
                return "ABSTAIN";
            }
            else if (voidCount == maximumNumber)
            {
                return "VOID";
            }
            else
            {
                return "";
            }
        }
    }

}