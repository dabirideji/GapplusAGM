using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using BarcodeGenerator.Util;
using Gapplus.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BarcodeGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ConsolidationController : ControllerBase
    {
        public ConsolidationController(UsersContext context)
        {
            db = context;
            ua = new UserAdmin(db);

        }
        UsersContext db;
        UserAdmin ua;
        private static string currentYear = DateTime.Now.Year.ToString();

        //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

        //private  int RetrieveAGMUniqueID()
        //{
        //    UsersContext adb = new UsersContext();
        //    var companyinfo = ua.GetUserCompanyInfo();
        //    var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

        //    return AGMID;
        //}

        //private static int UniqueAGMId = RetrieveAGMUniqueID();
        // GET: Consolidation
        [HttpGet]
        public ActionResult Index()
        {
            // return View();
            return Ok();
        }


        private async Task<string> UnSelectedAllItem(PostSelectModel post)
        {
            //Request.QueryString.

            var response = await UnSelectedAllItemAsync(post.ids);

            return response;
        }


        private Task<string> UnSelectedAllItemAsync(string val)
        {
            try
            {
                var jvalues = JsonConvert.DeserializeObject(val);
                JArray a = JArray.Parse(val);

                //Jvar IdsJson = Json.Parse(jvalues);
                //var value = jvalues.Split(',');
                foreach (var item in a)
                {
                    var id = int.Parse(item.ToString());
                    var shareholder = db.BarcodeStore.Find(id);
                    if (shareholder != null)
                    {
                        shareholder.Selected = false;
                        db.Entry(shareholder).State = EntityState.Modified;

                    }
                }
                db.SaveChanges();
                return Task.FromResult<string>("success");

            }
            catch (Exception e)
            {
                return Task.FromResult<string>("failure");

            }

        }
        [HttpPost]
        public async Task<ActionResult> ConsolidateIndex(PostSelectModel post)
        {
            var response = await ConsolidateIndexAsync(post.ids);

            // return PartialView(response);
            return Ok(response);
        }

        private Task<List<BarcodeModel>> ConsolidateIndexAsync(string val)
        {
            var companyinfo = ua.GetUserCompanyInfo();
            try
            {
                var jvalues = JsonConvert.DeserializeObject(val);
                JArray a = JArray.Parse(val);
                List<BarcodeModel> Shareholders = new List<BarcodeModel>();

                foreach (var item in a)
                {
                    var id = int.Parse(item.ToString());
                    var shareholder = db.BarcodeStore.Find(id);
                    if (shareholder != null)
                    {

                        Shareholders.Add(shareholder);

                    }
                }
                //string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                //SqlConnection conn =
                //        new SqlConnection(connStr);
                ////string query = "UPDATE BarcodeModels SET Selected = 0 WHERE Company ='" + companyinfo + "' AND Selected = 1";
                ////conn.Open();
                ////SqlCommand cmd = new SqlCommand(query, conn);
                ////cmd.ExecuteNonQuery();
                //string query2 = "select * from BarcodeModels WHERE Company = '" + companyinfo + "' AND Selected = 1";
                //conn.Open();
                //SqlCommand cmd = new SqlCommand(query2, conn);
                //SqlDataReader read = cmd.ExecuteReader();

                //while (read.Read())
                //{
                //    BarcodeModel model = new BarcodeModel
                //    {

                //        SN = (read["SN"].ToString()),
                //        Id = int.Parse(read["Id"].ToString()),
                //        Name = (read["Name"].ToString()),
                //        Address = (read["Address"].ToString()),
                //        ShareholderNum = (Int64.Parse(read["ShareholderNum"].ToString())),
                //        Holding = double.Parse(read["Holding"].ToString()),
                //        PercentageHolding = double.Parse(read["PercentageHolding"].ToString()),
                //        Company = (read["Company"].ToString()),
                //        PhoneNumber = (read["PhoneNumber"].ToString()),
                //        emailAddress = (read["emailAddress"].ToString()),
                //        Clikapad = (read["Clikapad"].ToString()),
                //        Present = bool.Parse(read["Present"].ToString()),
                //        Selected = bool.Parse(read["Selected"].ToString()),
                //        Consolidated = bool.Parse(read["Consolidated"].ToString()),
                //        PresentByProxy = bool.Parse(read["PresentByProxy"].ToString()),
                //        split = bool.Parse(read["split"].ToString()),
                //        resolution = bool.Parse(read["resolution"].ToString()),
                //        TakePoll = bool.Parse(read["TakePoll"].ToString())

                //    };
                //    Shareholders.Add(model);
                //}
                //read.Close();
                //conn.Close();

                return Task.FromResult<List<BarcodeModel>>(Shareholders);

            }
            catch (Exception e)
            {
                return Task.FromResult<List<BarcodeModel>>(new List<BarcodeModel>());
            }

        }


        private void CloseConsolidationSlide()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            try
            {
                // string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                string connStr = DatabaseManager.GetConnectionString();
                SqlConnection conn =
                        new SqlConnection(connStr);
                string query = "UPDATE BarcodeModels SET Selected = 0 WHERE Company ='" + companyinfo + "' AND Selected = 1";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }
        }


        //public void ClearSelect()
        //{

        //}

        //[HttpPost]

        [HttpPost]
        public async Task<ActionResult> SearchRequest(SearchModel post)
        {
            var response = await SearchRequestAsync(post);

            // return Json(new { data = response }, JsonRequestBehavior.AllowGet);
            return Ok(new { data = response });
        }

        private Task<BarcodeModel> SearchRequestAsync(SearchModel post)
        {
            try
            {

                var companyinfo = ua.GetUserCompanyInfo();
                BarcodeModel model = new BarcodeModel();
                if (!string.IsNullOrEmpty(post.search))
                {
                    var number = long.Parse(post.search);
                    model = db.BarcodeStore.FirstOrDefault(b => b.Company == companyinfo && b.ShareholderNum == number && b.Consolidated == false && b.Present == false);
                    if (model != null)
                    {
                        model.Selected = true;
                        db.Entry(model).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        return Task.FromResult<BarcodeModel>(new BarcodeModel());
                    }

                }
                return Task.FromResult<BarcodeModel>(model);
            }
            catch (Exception e)
            {
                return Task.FromResult<BarcodeModel>(new BarcodeModel());
            }
        }

        [HttpPost]
        public async Task<ActionResult> ConsolidateRequest(PostSelectModel post)
        {
            var response = await ConsolidateRequestAsync(post.ids);

            // return Json(new { data = response }, JsonRequestBehavior.AllowGet);
            return Ok(new { data = response });
        }

        private Task<BarcodeModel> ConsolidateRequestAsync(string val)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                var jvalues = JsonConvert.DeserializeObject(val);
                JArray a = JArray.Parse(val);

                //Jvar IdsJson = Json.Parse(jvalues);
                //var value = jvalues.Split(',');
                // string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                string connStr = DatabaseManager.GetConnectionString();

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
                var ConsolidatedAccounts = db.BarcodeStore.Where(b => b.Company == companyinfo && b.Selected == true);

                List<BarcodeModel> ConsolidatedList = new List<BarcodeModel>();
                ConsolidatedList.Clear();
                //var consolidatedIds = mod;
                //
                foreach (var item in a)
                {
                    var id = int.Parse(item.ToString());
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

                db.BarcodeStore.Add(consolidatedAccount);


                db.SaveChanges();

                return Task.FromResult<BarcodeModel>(consolidatedAccount);
            }
            catch (Exception e)
            {
                return Task.FromResult<BarcodeModel>(new BarcodeModel());
            }
        }


        [HttpPost]
        public async Task<ActionResult> RegularizeName(PostSelectModel post)
        {
            var response = await RegularizeNameAsync(post.ids, post.stringvalue);

            // return Json(new { value = response }, JsonRequestBehavior.AllowGet);
            return Ok(new { value = response });
        }

        private Task<string> RegularizeNameAsync(string val, string name)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                var jvalues = JsonConvert.DeserializeObject(val);
                JArray a = JArray.Parse(val);

                foreach (var item in a)
                {
                    var id = int.Parse(item.ToString());
                    var shareholder = db.BarcodeStore.Find(id);
                    if (shareholder != null)
                    {
                        shareholder.Name = name;
                        db.Entry(shareholder).State = EntityState.Modified;

                    }
                }
                //    var Names = db.BarcodeStore.Where(b =>b.Company ==companyinfo && b.Selected == true);
                //foreach(var item in Names)
                //{
                //    item.Name = id;
                //}
                db.SaveChanges();
                return Task.FromResult<string>(name);
            }
            catch (Exception e)
            {
                return Task.FromResult<string>("Empty");
            }
        }



        [HttpPost]
        public async Task<ActionResult> RegularizePhoneNumber(PostSelectModel post)
        {
            var response = await RegularizePhoneNumberAsync(post.ids, post.stringvalue);

            // return Json(new { value = response }, JsonRequestBehavior.AllowGet);
            return Ok(new { value = response });
        }

        private Task<string> RegularizePhoneNumberAsync(string val, string phonenumber)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();


                var jvalues = JsonConvert.DeserializeObject(val);
                JArray a = JArray.Parse(val);

                //Jvar IdsJson = Json.Parse(jvalues);
                //var value = jvalues.Split(',');
                foreach (var item in a)
                {
                    var id = int.Parse(item.ToString());
                    var shareholder = db.BarcodeStore.Find(id);
                    if (shareholder != null)
                    {
                        shareholder.PhoneNumber = phonenumber;
                        db.Entry(shareholder).State = EntityState.Modified;

                    }
                }
                //var Names = db.BarcodeStore.Where(b =>b.Company == companyinfo && b.Selected == true);
                //foreach (var item in Names)
                //{
                //    item.PhoneNumber = id;
                //}
                db.SaveChanges();
                return Task.FromResult<string>(phonenumber);
            }
            catch (Exception e)
            {
                return Task.FromResult<string>("Empty");
            }
        }


        [HttpPost]
        public async Task<ActionResult> RegularizeEmail(PostSelectModel post)
        {
            var response = await RegularizeEmailAsync(post.ids, post.stringvalue);

            // return Json(new { value = response }, JsonRequestBehavior.AllowGet);
            return Ok(new { value = response });
        }


        private Task<string> RegularizeEmailAsync(string val, string email)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();

                var jvalues = JsonConvert.DeserializeObject(val);
                JArray a = JArray.Parse(val);

                //Jvar IdsJson = Json.Parse(jvalues);
                //var value = jvalues.Split(',');
                foreach (var item in a)
                {
                    var id = int.Parse(item.ToString());
                    var shareholder = db.BarcodeStore.Find(id);
                    if (shareholder != null)
                    {
                        shareholder.emailAddress = email;
                        db.Entry(shareholder).State = EntityState.Modified;

                    }
                }
                //var Names = db.BarcodeStore.Where(b =>b.Company == companyinfo && b.Selected == true);
                //foreach (var item in Names)
                //{
                //    item.emailAddress = id;
                //}
                db.SaveChanges();
                return Task.FromResult<string>(email);
            }
            catch (Exception e)
            {
                return Task.FromResult<string>("Empty");
            }
        }



        private async Task<string> UnConsolidate(PostSelectModel post)
        {
            var response = await UnConsolidateAsync(post.ids);

            return response;
        }

        private Task<string> UnConsolidateAsync(string val)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID();

                var jvalues = JsonConvert.DeserializeObject(val);
                JArray arr = JArray.Parse(val);

                if (arr.Count() > 1)
                {
                    foreach (var item in arr)
                    {
                        var id = int.Parse(item.ToString());
                        var consolidated = db.BarcodeStore.Find(id);
                        if (consolidated != null)
                        {
                            var resolutionTaken = db.Result.Where(r => r.Company == companyinfo && r.AGMID == UniqueAGMId && r.ShareholderNum == consolidated.ShareholderNum).ToArray();
                            if (resolutionTaken.Any())
                            {
                                db.Result.RemoveRange(resolutionTaken);
                            }
                            //if (resolutionTaken.Length > 0)
                            //{
                            //    for (int i = 0; i < resolutionTaken.Length; i++)
                            //    {
                            //        db.Result.Remove(resolutionTaken[i]);
                            //    }

                            //}
                            var consolidatedAccount = db.Present.FirstOrDefault(p => p.Company == companyinfo && p.AGMID == UniqueAGMId && p.ShareholderNum == consolidated.ShareholderNum);
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
                            db.BarcodeStore.Remove(consolidated);
                            //}

                        }
                    }
                    db.SaveChanges();
                    Functions.PresentCount(UniqueAGMId, true);
                    return Task.FromResult<string>("success");
                }
                else if (arr.Count() == 1)
                {
                    var id = int.Parse(arr[0].ToString());
                    var consolidated = db.BarcodeStore.Find(id);
                    //foreach (var item in consolidated)
                    //{
                    if (consolidated != null)
                    {
                        var resolutionTaken = db.Result.Where(r => r.Company == companyinfo && r.AGMID == UniqueAGMId && r.ShareholderNum == consolidated.ShareholderNum).ToArray();
                        //if (resolutionTaken.Length > 0)
                        //{
                        //    for (int i = 0; i < resolutionTaken.Length; i++)
                        //    {
                        //        db.Result.Remove(resolutionTaken[i]);
                        //    }

                        //}
                        if (resolutionTaken.Any())
                        {
                            db.Result.RemoveRange(resolutionTaken);
                        }
                        var consolidatedAccount = db.Present.FirstOrDefault(p => p.Company == companyinfo && p.AGMID == UniqueAGMId && p.ShareholderNum == consolidated.ShareholderNum);
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
                        db.BarcodeStore.Remove(consolidated);
                        //}
                        db.SaveChanges();
                        Functions.PresentCount(UniqueAGMId, true);
                        return Task.FromResult<string>("success");
                    }
                    return Task.FromResult<string>("Not a Consolidated Account");
                }
                else
                {
                    return Task.FromResult<string>("Not a Consolidated Account");
                }

                //Jvar IdsJson = Json.Parse(jvalues);
                //var value = jvalues.Split(',');


                //var consolidated = db.BarcodeStore.Where(b =>b.Company == companyinfo && b.Selected == true && b.ConsolidatedParent == "true");
                //if(consolidated.Count() > 0)
                //{
                //    foreach (var item in consolidated)
                //    {
                //        var resolutionTaken = db.Result.Where(r =>r.Company == companyinfo && r.AGMID == UniqueAGMId && r.Identity == item.ShareholderNum).ToArray();
                //        if (resolutionTaken.Length > 0)
                //        {
                //            for (int i = 0; i < resolutionTaken.Length; i++)
                //            {
                //                db.Result.Remove(resolutionTaken[i]);
                //            }

                //        }
                //        var consolidatedAccount = db.Present.SingleOrDefault(p =>p.Company==companyinfo && p.AGMID == UniqueAGMId && p.ShareholderNum == item.ShareholderNum);
                //        if (consolidatedAccount != null)
                //        {
                //            db.Present.Remove(consolidatedAccount);
                //        }

                //        var allconsolidated = db.BarcodeStore.Where(a =>a.Company==companyinfo && a.ConsolidatedValue == item.ConsolidatedValue);
                //        foreach (var consolidate in allconsolidated)
                //        {
                //            consolidate.Consolidated = false;
                //            consolidate.NotVerifiable = false;
                //            //consolidate.PresentByProxy = false;
                //            consolidate.ConsolidatedValue = "";
                //            db.Entry(consolidate).State = EntityState.Modified;
                //        }
                //        db.BarcodeStore.Remove(item);
                //    }
                //    db.SaveChanges();
                //    return Task.FromResult<string>("success");

                //}
                //var unconsolidated = db.BarcodeStore.Where(b =>b.Company==companyinfo && b.Selected);
                //if(unconsolidated.Count() > 0)
                //{
                //    foreach(var item in unconsolidated)
                //    {
                //        item.Selected = false;
                //        db.Entry(item).State = EntityState.Modified;
                //    }
                //    db.SaveChanges();

                //}
                //return Task.FromResult<string>("Not a Consolidated Account");

            }
            catch
            {
                return Task.FromResult<string>("failed");
            }

        }
    }
}