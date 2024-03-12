using BarcodeGenerator.Models;
using BarcodeGenerator.Models.ModelDTO;
using BarcodeGenerator.Util;
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
    public class PresentService
    {
        UserAdmin ua;
          private readonly UsersContext db;

        public PresentService(UsersContext _db)
        {
            db = _db;
            ua = new UserAdmin(db);
            agmM = new AGMManager(db);
        }
        AGMManager agmM ;
        private static string currentYear = DateTime.Now.Year.ToString();

        public  Task<GenericResponseDto<string>> MarkPregisteredShareholderAsync(string company, string email)
        {

            var UniqueAGMId = ua.RetrieveAGMUniqueID(company);

            List<BarcodeModel> multipleEntry = new List<BarcodeModel>();
            //var shareholder = db.BarcodeStore.Find(id);
            var count = 0;

            var AgmEvent = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
            if (AgmEvent == null)
            {
                return Task.FromResult(new GenericResponseDto<string> { 
                    Status = false,
                    Message="Specified event incorrect."
                });
            }
            if (AgmEvent.StopAdmittance)
            {
                return Task.FromResult(new GenericResponseDto<string>
                {
                    Status = false,
                    Message = "Admittance has been stopped."
                });
            }
            if (!String.IsNullOrEmpty(email))
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
                        Preregistered= bool.Parse(read["Preregistered"].ToString())

                    };
                    multipleEntry.Add(model);
                }
                read.Close();
                //var multipleEntry = db.BarcodeStore.Where(s => s.emailAddress == shareholder.emailAddress).ToList();

                    var checkIfAnyAccountIsProxy = multipleEntry.Any(s => s.PresentByProxy == true);
                BarcodeModel srecord;
                if (multipleEntry.Count > 1)
                {
                    var checkifConsolidate = multipleEntry.Any(c => c.Consolidated == true);
                    if (!checkifConsolidate)
                    {
                        srecord = agmM.ConsolidateRequest(company, multipleEntry);
                    }
                    else
                    {
                        var consolidatedvalue = multipleEntry.Where(s => s.Consolidated == true).Select(s => s.ConsolidatedValue).FirstOrDefault();

                        srecord = agmM.GetConsolidatedAccount(company, consolidatedvalue);
                    }

                }
                else
                {
                    srecord = multipleEntry.FirstOrDefault();
                }

                var cshareholder = db.Present.FirstOrDefault(u => u.AGMID == UniqueAGMId && u.ShareholderNum == srecord.ShareholderNum);

                    srecord.Preregistered = true;
                    PresentModel present = new PresentModel();
                    present.Name = srecord.Name;
                    present.Company = srecord.Company;
                    present.Address = srecord.Address;
                    present.admitSource = "Preregistration";
                    present.ShareholderNum = srecord.ShareholderNum;
                    present.Holding = srecord.Holding;
                    present.AGMID = UniqueAGMId;
                    present.PercentageHolding = srecord.PercentageHolding;
                    present.present = false;
                    present.proxy = false;
                    present.preregistered = true;
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

                    }
                cshareholder.preregistered = true;
                srecord.Date = DateTime.Today.ToString();
                db.Entry(srecord).State = EntityState.Modified;
                db.SaveChanges();
                    Functions.PresentCount(UniqueAGMId, true);
                return Task.FromResult(new GenericResponseDto<string>
                {
                    Status = true,
                    Message = "success"
                });

            }
            return Task.FromResult(new GenericResponseDto<string>
            {
                Status = false,
                Message = "Email provide is incorrect."
            });

        }


        public Task<GenericResponseDto<PresentModel>> MarkShareholderVirtuallyPresentAsync(string company, string email)
        {

            var UniqueAGMId = ua.RetrieveAGMUniqueID(company);

            List<BarcodeModel> multipleEntry = new List<BarcodeModel>();
            //var shareholder = db.BarcodeStore.Find(id);
            var count = 0;

            var AgmEvent = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
            if (AgmEvent == null)
            {
                return Task.FromResult(new GenericResponseDto<PresentModel>
                {
                    Status = false,
                    Message = "Specified event incorrect."
                });
            }
            if (AgmEvent.StopAdmittance)
            {
                return Task.FromResult(new GenericResponseDto<PresentModel>
                {
                    Status = false,
                    Message = "Admittance has been stopped."
                });
            }
            if (!String.IsNullOrEmpty(email))
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

                var checkIfAnyAccountIsProxy = multipleEntry.Any(s => s.PresentByProxy == true);
                BarcodeModel record;
                if (multipleEntry.Count > 1)
                {
                    var checkifConsolidate = multipleEntry.Any(c => c.Consolidated == true);
                    if (!checkifConsolidate)
                    {
                        record = agmM.ConsolidateRequest(company, multipleEntry);
                    }
                    else
                    {
                        var consolidatedvalue = multipleEntry.Where(s => s.Consolidated == true).Select(s => s.ConsolidatedValue).FirstOrDefault();

                        record = agmM.GetConsolidatedAccount(company, consolidatedvalue);
                    }

                }
                else
                {
                    record = multipleEntry.FirstOrDefault();
                }

                //var cshareholder = db.Present.FirstOrDefault(u => u.AGMID == UniqueAGMId && u.ShareholderNum == record.ShareholderNum);
                var shareholder = db.Present.Any(u => u.AGMID == UniqueAGMId && u.ShareholderNum == record.ShareholderNum);


                record.Present = true;
                PresentModel present = new PresentModel();
                present.Name = record.Name;
                present.Company = record.Company;
                present.Address = record.Address;
                present.admitSource = "Web";
                present.ShareholderNum = record.ShareholderNum;
                present.Holding = record.Holding;
                present.AGMID = UniqueAGMId;
                present.PercentageHolding = record.PercentageHolding;
                present.present = true;
                present.Year = currentYear;
                present.proxy = false;
                present.PresentTime = DateTime.Now;
                present.Timestamp = DateTime.Now.TimeOfDay;
                present.emailAddress = record.emailAddress;
                if (AgmEvent.StopAdmittance == true)
                {
                    present.PermitPoll = 0;
                }
                else if (AgmEvent.allChannels || AgmEvent.webChannel)
                {
                    present.PermitPoll = 1;
                }

                else
                {
                    present.PermitPoll = 0;
                }
                if (!String.IsNullOrEmpty(record.PhoneNumber))
                {
                    if (record.PhoneNumber.StartsWith("234"))
                    {
                        present.PhoneNumber = record.PhoneNumber;
                    }
                    else if (record.PhoneNumber.StartsWith("0"))
                    {
                        double number;
                        if (double.TryParse(record.PhoneNumber, out number))
                        {
                            number = double.Parse(record.PhoneNumber);
                            present.PhoneNumber = "234" + number.ToString();
                        }
                        else
                        {
                            char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
                            var phonenum = record.PhoneNumber.Split(delimiterChars);
                            //string phonenumberresult = string.Concat(phonenum);
                            if (double.TryParse(phonenum[0], out number))
                            {
                                number = double.Parse(phonenum[0]);
                                present.PhoneNumber = "234" + number.ToString();
                            }


                        }

                    }

                }
                present.Clikapad = record.Clikapad;
                if (record.PresentByProxy != true && !shareholder && AgmEvent.StartAdmittance)
                {
                    db.Present.Add(present);
                }
                record.Date = DateTime.Today.ToString();
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
                Functions.PresentCount(UniqueAGMId, true);
                return Task.FromResult(new GenericResponseDto<PresentModel>
                {
                    Status = true,
                    Message = "success",
                    Data = present
                });

            }
            return Task.FromResult(new GenericResponseDto<PresentModel>
            {
                Status = false,
                Message = "Email provide is incorrect."
            });

        }
    }
}