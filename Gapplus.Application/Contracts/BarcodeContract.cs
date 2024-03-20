using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using BarcodeGenerator.Util;
using Gapplus.Application.Helpers;
using Gapplus.Application.Interfaces.IContracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Gapplus.Application.Contracts
{
    public class BarcodeContract : IBarcodeContract
    {
        private readonly IUserAdmin ua;
        private readonly UsersContext db;
        private readonly IAGMManager agmM;
        private readonly IClikapadContract cl;

        public BarcodeContract(UsersContext _db)
        {
            db = _db;
            agmM = new AGMManager(db);
            ua = new UserAdmin(db);
            cl = new ClikapadContract(db);
        }
        string currentYear = DateTime.Today.Year.ToString();  //IM TRYING TO GET THE CURRENT YEAR

        public Task<string> PresentAsync(int id, QuestionStatus data)

        {




            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();


            List<BarcodeModel> multipleEntry = new List<BarcodeModel>();
            var shareholder = db.BarcodeStore.Find(id);
            var count = 0;
            string Message = "";
            var AgmEvent = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);

            /// <summary>
            /// /
            /// 
            /// 
            /// 
                string connStr = DatabaseManager.GetConnectionString();

                SqlConnection conn =
                        new SqlConnection(connStr);
                //string query2 = "select * from BarcodeModels WHERE CONTAINS(Name,'" + searchValue + "')";
                //string query2 = "select * from BarcodeModels WHERE emailAddress LIKE '%" + shareholder.emailAddress + "%'";

                //string query2 = "select * from BarcodeModels WHERE Company =  '" + companyinfo + "' AND  emailAddress = '" + shareholder.emailAddress + "' AND NotVerifiable = 'FALSE'";
                string query2 = "select * from BarcodeModels WHERE Company =  '" + companyinfo + "' AND  emailAddress = '" + shareholder.emailAddress + "'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query2, conn);
                SqlDataReader read = cmd.ExecuteReader();

            /// </summary>
            /// <typeparam name="BarcodeModel"></typeparam>
            /// <returns></returns>
            if (AgmEvent == null)
            {
                return Task.FromResult<string>("AGMID Inconsistency.");
            }
            if (AgmEvent.StopAdmittance)
            {
                return Task.FromResult<string>("Admitance have been stopped");
            }
            if (!String.IsNullOrEmpty(shareholder.emailAddress))
            {
                // // string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                // string connStr = DatabaseManager.GetConnectionString();

                // SqlConnection conn =
                //         new SqlConnection(connStr);
                // //string query2 = "select * from BarcodeModels WHERE CONTAINS(Name,'" + searchValue + "')";
                // //string query2 = "select * from BarcodeModels WHERE emailAddress LIKE '%" + shareholder.emailAddress + "%'";

                // //string query2 = "select * from BarcodeModels WHERE Company =  '" + companyinfo + "' AND  emailAddress = '" + shareholder.emailAddress + "' AND NotVerifiable = 'FALSE'";
                // string query2 = "select * from BarcodeModels WHERE Company =  '" + companyinfo + "' AND  emailAddress = '" + shareholder.emailAddress + "'";
                // conn.Open();
                // SqlCommand cmd = new SqlCommand(query2, conn);
                // SqlDataReader read = cmd.ExecuteReader();

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
                //var multipleEntry = db.BarcodeStore.Where(s => s.emailAddress == shareholder.emailAddress).ToList();
                if (multipleEntry.Count() > 1 && data.status)
                {
                    var checkIfAnyAccountIsProxy = multipleEntry.Any(s => s.PresentByProxy == true);
                    //var srecord = shareholderRecords.First();
                    BarcodeModel srecord;
                    var checkifConsolidate = multipleEntry.Any(c => c.Consolidated == true);
                    if (!checkifConsolidate)
                    {
                        srecord = agmM.ConsolidateRequest(companyinfo, multipleEntry);
                    }
                    else
                    {
                        var consolidatedvalue = multipleEntry.Where(s => s.Consolidated == true).Select(s => s.ConsolidatedValue).FirstOrDefault();

                        srecord = agmM.GetConsolidatedAccount(companyinfo, consolidatedvalue);
                    }

                    var cshareholder = db.Present.FirstOrDefault(u => u.AGMID == UniqueAGMId && u.ShareholderNum == srecord.ShareholderNum);

                    srecord.Present = true;
                    PresentModel present = new PresentModel();
                    present.Name = srecord.Name;
                    present.Company = srecord.Company;
                    present.Address = srecord.Address;
                    present.admitSource = "Self";
                    present.ShareholderNum = srecord.ShareholderNum;
                    present.Holding = srecord.Holding;
                    present.AGMID = UniqueAGMId;
                    present.PercentageHolding = srecord.PercentageHolding;
                    present.present = true;
                    present.proxy = false;
                    present.Year = currentYear;
                    present.PresentTime = DateTime.Now;
                    present.Timestamp = DateTime.Now.TimeOfDay;
                    present.emailAddress = srecord.emailAddress;
                    if (AgmEvent.StopAdmittance || checkIfAnyAccountIsProxy)
                    {
                        present.PermitPoll = 0;
                    }
                    else
                    {
                        present.PermitPoll = 1;
                    }
                    if (!String.IsNullOrEmpty(srecord.PhoneNumber))
                    {
                        if (srecord.PhoneNumber.StartsWith("234"))
                        {
                            present.PhoneNumber = srecord.PhoneNumber;
                        }
                        else if (srecord.PhoneNumber.StartsWith("0"))
                        {
                            var number = double.Parse(srecord.PhoneNumber);
                            present.PhoneNumber = "234" + number.ToString();

                        }

                    }
                    present.Clikapad = srecord.Clikapad;
                    if (cshareholder == null)
                    {
                        db.Present.Add(present);
                        srecord.Date = DateTime.Today.ToString();
                        db.Entry(srecord).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    Functions.PresentCount(UniqueAGMId, true);
                    return Task.FromResult<string>("Accounts Consolidated.");
                }
                else if (multipleEntry.Count() > 1 && data.status == false)
                {
                    //shareholder.Present = data.status;

                    var presentEntry = db.Present.FirstOrDefault(r => r.AGMID == UniqueAGMId && r.ShareholderNum == shareholder.ShareholderNum);
                    //emailEntry.TakePoll = data.status;

                    var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == shareholder.ShareholderNum).ToArray();
                    if (result.Any())
                    {
                        db.Result.RemoveRange(result);
                    }
                    //for (int i = 0; i < result.Length; i++)
                    //{
                    //    db.Result.Remove(result[i]);

                    //}
                    if (presentEntry != null)
                    {

                        db.Present.Remove(presentEntry);
                        count++;
                    }

                    //checkifConsolidatedAccount
                    string message = "";

                    var checkifConsolidate = multipleEntry.Any(c => c.Consolidated == true);
                    if (checkifConsolidate)
                    {
                        //Unconsolidate Account
                        message = agmM.UnConsolidateAsync(companyinfo, UniqueAGMId, shareholder);
                    }

                    db.SaveChanges();

                    return Task.FromResult<string>(message);
                }
                else if (multipleEntry.Count() == 1)
                {
                    var emailentry = multipleEntry.First();
                    var entry = db.BarcodeStore.Find(emailentry.Id);

                    var present = db.Present.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == entry.ShareholderNum);
                    if (data.status == true && entry.PresentByProxy == true)
                    {
                        entry.Present = false;
                    }
                    else if (data.status == false)
                    {
                        entry.Present = data.status;
                        entry.TakePoll = data.status;
                        var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == entry.ShareholderNum).ToArray();
                        if (result.Any())
                        {
                            db.Result.RemoveRange(result);
                        }
                        //for (int i = 0; i < result.Length; i++)
                        //{
                        //    db.Result.Remove(result[i]);

                        //}
                        if (present.Any())
                        {
                            db.Present.RemoveRange(present);
                            //foreach(var item in present)
                            //{ db.Present.Remove(item);
                            //}

                        }

                    }
                    else
                    {
                        entry.Present = data.status;
                        if (!present.Any())
                        {

                            string phonenumber = "";
                            if (!String.IsNullOrEmpty(entry.PhoneNumber))
                            {
                                char[] arr = entry.PhoneNumber.Where(c => (char.IsLetterOrDigit(c))).ToArray();

                                entry.PhoneNumber = new string(arr);
                                if (entry.PhoneNumber.StartsWith("234"))
                                {
                                    phonenumber = entry.PhoneNumber;
                                }
                                else if (entry.PhoneNumber.StartsWith("0"))
                                {

                                    double number = double.Parse(entry.PhoneNumber);
                                    phonenumber = "234" + number.ToString();

                                }

                            }
                            PresentModel model = new PresentModel();
                            model.Name = entry.Name;
                            model.Address = entry.Address;
                            model.Company = entry.Company;
                            model.admitSource = "Self";
                            model.PermitPoll = 1;
                            model.Year = currentYear;
                            model.AGMID = UniqueAGMId;
                            model.ShareholderNum = entry.ShareholderNum;
                            model.ParentNumber = entry.ParentAccountNumber;
                            model.Holding = entry.Holding;
                            model.PhoneNumber = phonenumber;
                            if (!String.IsNullOrEmpty(entry.emailAddress))
                            {
                                model.emailAddress = entry.emailAddress;
                            }

                            model.PercentageHolding = entry.PercentageHolding;
                            model.present = true;
                            model.proxy = false;
                            model.PresentTime = DateTime.Now;
                            if (!String.IsNullOrEmpty(entry.Clikapad))
                            {
                                model.Clikapad = entry.Clikapad;
                                model.GivenClikapad = true;
                            }
                            model.Timestamp = DateTime.Now.TimeOfDay;
                            db.Present.Add(model);
                        }
                    }
                    // db.Entry(entry).State = EntityState.Modified;
                    db.SaveChanges();
                    Functions.PresentCount(UniqueAGMId, true);

                    return Task.FromResult<string>("Success");

                }
                else
                {
                    return Task.FromResult<string>("Empty");
                }
            }
            else
            {
                var present = db.Present.SingleOrDefault(r => r.AGMID == UniqueAGMId && r.ShareholderNum == shareholder.ShareholderNum);
                if (data.status == true && shareholder.PresentByProxy == true)
                {
                    shareholder.Present = false;
                }
                else if (data.status == false)
                {
                    shareholder.Present = data.status;
                    shareholder.TakePoll = data.status;
                    var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == shareholder.ShareholderNum).ToArray();
                    if (result.Any())
                    {
                        db.Result.RemoveRange(result);
                    }
                    //for (int i = 0; i < result.Length; i++)
                    //{
                    //    db.Result.Remove(result[i]);

                    //}
                    if (present != null)
                    {
                        db.Present.Remove(present);
                    }

                }
                else
                {
                    shareholder.Present = data.status;
                    if (present != null)
                    {
                        //Do Nothing
                    }
                    else
                    {

                        string phonenumber = "";
                        if (!String.IsNullOrEmpty(shareholder.PhoneNumber))
                        {
                            char[] arr = shareholder.PhoneNumber.Where(c => (char.IsLetterOrDigit(c))).ToArray();

                            shareholder.PhoneNumber = new string(arr);
                            if (shareholder.PhoneNumber.StartsWith("234"))
                            {
                                phonenumber = shareholder.PhoneNumber;
                            }
                            else if (shareholder.PhoneNumber.StartsWith("0"))
                            {

                                double number = double.Parse(shareholder.PhoneNumber);
                                phonenumber = "234" + number.ToString();

                            }

                        }
                        PresentModel model = new PresentModel();
                        model.Name = shareholder.Name;
                        model.Address = shareholder.Address;
                        model.Company = shareholder.Company;
                        model.admitSource = "Self";
                        model.PermitPoll = 1;
                        model.Year = currentYear;
                        model.AGMID = UniqueAGMId;
                        model.ShareholderNum = shareholder.ShareholderNum;
                        model.ParentNumber = shareholder.ParentAccountNumber;
                        model.Holding = shareholder.Holding;
                        model.PhoneNumber = phonenumber;
                        if (!String.IsNullOrEmpty(shareholder.emailAddress))
                        {
                            model.emailAddress = shareholder.emailAddress;
                        }

                        model.PercentageHolding = shareholder.PercentageHolding;
                        model.present = true;
                        model.proxy = false;
                        model.PresentTime = DateTime.Now;
                        model.Timestamp = DateTime.Now.TimeOfDay;
                        db.Present.Add(model);
                    }
                }
                //db.Entry(shareholder).State = EntityState.Modified;
                db.SaveChanges();
                Functions.PresentCount(UniqueAGMId, true);
                return Task.FromResult<string>("Success");
            }


        }





        public Task<string> EditAsync(int id, PresentModel collection)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID();
                // TODO: Add update logic here
                var model = db.BarcodeStore.Find(collection.Id);
                if (!String.IsNullOrEmpty(collection.PhoneNumber))
                {
                    if (collection.PhoneNumber.StartsWith("234"))
                    {
                        model.PhoneNumber = collection.PhoneNumber;
                    }
                    else if (collection.PhoneNumber.StartsWith("0"))
                    {
                        var number = double.Parse(collection.PhoneNumber);
                        model.PhoneNumber = "234" + number.ToString();

                    }

                }
                if (!String.IsNullOrEmpty(collection.emailAddress))
                {
                    model.emailAddress = collection.emailAddress;
                }
                model.Clikapad = collection.Clikapad;

                db.Entry(model).State = EntityState.Modified;
                var presentmodel = db.Present.FirstOrDefault(b => b.Company == companyinfo && b.ShareholderNum == model.ShareholderNum);
                if (presentmodel != null)
                {
                    if (!String.IsNullOrEmpty(collection.emailAddress))
                    {
                        presentmodel.emailAddress = collection.emailAddress;
                    }

                    if (!String.IsNullOrEmpty(collection.PhoneNumber))
                    {
                        //number = double.Parse(collection.PhoneNumber);
                        if (collection.PhoneNumber.StartsWith("234"))
                        {
                            presentmodel.PhoneNumber = collection.PhoneNumber;
                        }
                        else if (collection.PhoneNumber.StartsWith("0"))
                        {
                            var number = double.Parse(collection.PhoneNumber);
                            presentmodel.PhoneNumber = "234" + number.ToString();

                        }

                    }

                    presentmodel.Clikapad = collection.Clikapad;


                    if (!String.IsNullOrEmpty(collection.Clikapad))
                    {
                        presentmodel.GivenClikapad = true;
                        cl.UpdateClikapad(presentmodel, collection.Clikapad);
                    }
                    db.Entry(presentmodel).State = EntityState.Modified;

                }
                db.SaveChanges();
                return Task.FromResult<string>("success");
            }
            catch
            {
                // TempData["Message1"] = "Cannot Edit database";
                return Task.FromResult<string>("failed");
            }
        }










    }
}