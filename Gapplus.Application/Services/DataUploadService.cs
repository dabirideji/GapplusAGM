using BarcodeGenerator.Models;
using BarcodeGenerator.Util;
using Gapplus.Application.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Service
{
    public class DataUploadService
    {

    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IConfiguration _configuration;

    public DataUploadService(IWebHostEnvironment webHostEnvironment, IConfiguration configuration,UsersContext _db)
    {
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
         db=_db;
         ua = new UserAdmin(db);
         dm = new dbManager();
         
    }

        UsersContext db;
        UserAdmin ua;
        dbManager dm;
        private static string connStr = ConfigurationHelper.GetConnectionString();
        string Companyinfo = "";
        string RegCode = "";
        private long FileUploadCount = 0;
        private SqlConnection connn =
        new SqlConnection(connStr);
        int newAGMID = 0;
      
        string companyinfo;









        // public Task<string> BulkUpload4AdditionalFileAsync(int id, HttpPostedFileBase file)
        // {
        //     try
        //     {
                    
        //             if (file.ContentLength > 0)
        //             { 

        //             Functions.RetrieveProgress("Intializing...");
        //             Log.Info("Upload file Initiated");
        //             string excelCS = string.Empty;

        //             var createIndexColumn = ExecuteColumnIndex();
        //             if(!createIndexColumn)
        //             {
        //                 Log.Error("SN Index was not dropped on BarcodeModels Table.");
        //                 return Task.FromResult<string>("Failed to Create Column Index.");
        //             }

        //             // string fileLocation = HttpContext.Current.Server.MapPath("~/Uploads/") + file.FileName;
        //             string fileLocation ="";
        //             string fileName = file.FileName;
        //             string fileExtension = System.IO.Path.GetExtension(fileName);
        //             if (System.IO.File.Exists(fileLocation))
        //             {

        //                 System.IO.File.Delete(fileLocation);
        //             }
        //             file.SaveAs(fileLocation);
        //               // Connection String to Excel Workbook  
        //             if (fileExtension == ".xls" || fileExtension == ".xlsx")
        //             {

        //                 excelCS = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
        //                     fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
        //                 //connection String for xls file format.
        //                 if (fileExtension == ".xls")
        //                 {
        //                     excelCS = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
        //                     fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
        //                 }
        //                 //connection String for xlsx file format.
        //                 else if (fileExtension == ".xlsx")
        //                 {
        //                     excelCS = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
        //                     fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
        //                 }
        //             }
        //             //excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", fileLocation);
        //             Functions.RetrieveProgress("Retrieving Data from Excel..");

        //             using (OleDbConnection con = new OleDbConnection(excelCS))
        //             {
        //                 //Create Connection to Excel work book and add oledb namespace
        //                 DataTable dtExcel = new DataTable();
        //                 //DataColumn shareholderNum = new DataColumn("ShareholderNum", typeof(long));
        //                 //shareholderNum.DefaultValue = 0;
        //                 //dtExcel.Columns.Add(shareholderNum);
        //                 DataColumn pHolding = new DataColumn("PercentageHolding", typeof(long));
        //                 pHolding.DefaultValue = 0;
        //                 dtExcel.Columns.Add(pHolding);
        //                 DataColumn Present = new DataColumn("Present", typeof(bool));
        //                 Present.DefaultValue = 0;
        //                 dtExcel.Columns.Add(Present);
        //                 DataColumn PresentByProxy = new DataColumn("PresentByProxy", typeof(bool));
        //                 PresentByProxy.DefaultValue = 0;
        //                 dtExcel.Columns.Add(PresentByProxy);
        //                 DataColumn split = new DataColumn("split", typeof(bool));
        //                 split.DefaultValue = 0;
        //                 dtExcel.Columns.Add(split);
        //                 DataColumn resolution = new DataColumn("resolution", typeof(bool));
        //                 resolution.DefaultValue = 0;
        //                 dtExcel.Columns.Add(resolution);
        //                 DataColumn combined = new DataColumn("combined", typeof(bool));
        //                 combined.DefaultValue = 0;
        //                 dtExcel.Columns.Add(combined);
        //                 DataColumn TakePoll = new DataColumn("TakePoll", typeof(bool));
        //                 TakePoll.DefaultValue = 0;
        //                 dtExcel.Columns.Add(TakePoll);
        //                 DataColumn ParentAccountNumber = new DataColumn("ParentAccountNumber", typeof(bool));
        //                 ParentAccountNumber.DefaultValue = 0;
        //                 dtExcel.Columns.Add(ParentAccountNumber);
        //                 DataColumn Selected = new DataColumn("Selected", typeof(bool));
        //                 Selected.DefaultValue = 0;
        //                 dtExcel.Columns.Add(Selected);
        //                 DataColumn Consolidated = new DataColumn("Consolidated", typeof(bool));
        //                 Consolidated.DefaultValue = 0;
        //                 dtExcel.Columns.Add(Consolidated);
        //                 DataColumn NotVerifiable = new DataColumn("NotVerifiable", typeof(bool));
        //                 NotVerifiable.DefaultValue = 0;
        //                 dtExcel.Columns.Add(NotVerifiable);
        //                 DataColumn AddedSplitAccount = new DataColumn("AddedSplitAccount", typeof(bool));
        //                 AddedSplitAccount.DefaultValue = 0;
        //                 dtExcel.Columns.Add(AddedSplitAccount);
        //                 DataColumn UserLoginHistory = new DataColumn("UserLoginHistory", typeof(bool));
        //                 UserLoginHistory.DefaultValue = 0;
        //                 dtExcel.Columns.Add(UserLoginHistory);

        //                 string query = "Select * from [Sheet1$]";
        //                 //string query = "Select * from [Sheet1$] WHERE NOT [ShareholderNum] IS NULL";


 
        //                 //var dataTable = dtExcel.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
        //                 //adapter.RowUpdating += new OleDbRowUpdatingEventHandler(oleDbDataAdapter1_RowUpdating);
        //                 adapter.Fill(dtExcel);

        //                 con.Close();

        //                 RegCode = dtExcel.Rows[0][14].ToString();
        //                 companyinfo = dtExcel.Rows[0][15].ToString();
        //                 Companyinfo = companyinfo;
        //                 FileUploadCount = dtExcel.Rows.Count;
        //                 Log.Info("Rows Count" + "-" + FileUploadCount);
        //                 //Log.Info("Null row number"+""+ dtExcel.Rows[0][13].ToString());

        //                     Functions.RetrieveProgress("Persisting Data to Database... ");
        //                     string connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //                     // Bulk Copy to SQL Server   
        //                     SqlBulkCopy bulkInsert = new SqlBulkCopy(connstr, SqlBulkCopyOptions.Default & SqlBulkCopyOptions.KeepIdentity);

        //                     bulkInsert.DestinationTableName = "BarcodeModels";

        //                     // Set up the event handler to notify after 50 rows.
        //                     //bulkInsert.SqlRowsCopied +=
        //                     //    new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
        //                     //bulkInsert.NotifyAfter = 50;

        //                     SqlBulkCopyColumnMapping SN = new SqlBulkCopyColumnMapping("SN", "SN");
        //                     bulkInsert.ColumnMappings.Add(SN);

        //                     SqlBulkCopyColumnMapping Name = new SqlBulkCopyColumnMapping("Name", "Name");
        //                     bulkInsert.ColumnMappings.Add(Name);

        //                     SqlBulkCopyColumnMapping Company = new SqlBulkCopyColumnMapping("Company", "Company");
        //                     bulkInsert.ColumnMappings.Add(Company);


        //                     SqlBulkCopyColumnMapping Holding = new SqlBulkCopyColumnMapping("Holding", "Holding");
        //                     bulkInsert.ColumnMappings.Add(Holding);

        //                     SqlBulkCopyColumnMapping Address = new SqlBulkCopyColumnMapping("Address", "Address");
        //                     bulkInsert.ColumnMappings.Add(Address);

        //                     SqlBulkCopyColumnMapping PercentageHolding = new SqlBulkCopyColumnMapping("PercentageHolding", "PercentageHolding");
        //                     bulkInsert.ColumnMappings.Add(PercentageHolding);

        //                     SqlBulkCopyColumnMapping ShareholderNum = new SqlBulkCopyColumnMapping("ShareholderNum", "ShareholderNum");
        //                     bulkInsert.ColumnMappings.Add(ShareholderNum);

        //                     SqlBulkCopyColumnMapping selected = new SqlBulkCopyColumnMapping("Selected", "Selected");
        //                     bulkInsert.ColumnMappings.Add(selected);

        //                     SqlBulkCopyColumnMapping consolidated = new SqlBulkCopyColumnMapping("Consolidated", "Consolidated");
        //                     bulkInsert.ColumnMappings.Add(consolidated);

        //                     SqlBulkCopyColumnMapping Prsent = new SqlBulkCopyColumnMapping("Present", "Present");
        //                     bulkInsert.ColumnMappings.Add(Prsent);
        //                     SqlBulkCopyColumnMapping PbyProxy = new SqlBulkCopyColumnMapping("PresentByProxy", "PresentByProxy");
        //                     bulkInsert.ColumnMappings.Add(PbyProxy);

        //                     SqlBulkCopyColumnMapping splitt = new SqlBulkCopyColumnMapping("split", "split");
        //                     bulkInsert.ColumnMappings.Add(splitt);
        //                     SqlBulkCopyColumnMapping Resolution = new SqlBulkCopyColumnMapping("resolution", "resolution");
        //                     bulkInsert.ColumnMappings.Add(Resolution);
        //                     SqlBulkCopyColumnMapping Combined = new SqlBulkCopyColumnMapping("combined", "combined");
        //                     bulkInsert.ColumnMappings.Add(Combined);

        //                     SqlBulkCopyColumnMapping Takepoll = new SqlBulkCopyColumnMapping("TakePoll", "TakePoll");
        //                     bulkInsert.ColumnMappings.Add(Takepoll);

        //                     SqlBulkCopyColumnMapping NotVerify = new SqlBulkCopyColumnMapping("NotVerifiable", "NotVerifiable");
        //                     bulkInsert.ColumnMappings.Add(NotVerify);

        //                     SqlBulkCopyColumnMapping AddedSplitAcc = new SqlBulkCopyColumnMapping("AddedSplitAccount", "AddedSplitAccount");
        //                     bulkInsert.ColumnMappings.Add(AddedSplitAcc);

        //                     SqlBulkCopyColumnMapping EmailAddress = new SqlBulkCopyColumnMapping("emailAddress", "emailAddress");
        //                     bulkInsert.ColumnMappings.Add(EmailAddress);

        //                     SqlBulkCopyColumnMapping ParentAccount = new SqlBulkCopyColumnMapping("ParentAccountNumber", "ParentAccountNumber");
        //                     bulkInsert.ColumnMappings.Add(ParentAccount);

        //                     SqlBulkCopyColumnMapping PhoneNumber = new SqlBulkCopyColumnMapping("PhoneNumber", "PhoneNumber");
        //                     bulkInsert.ColumnMappings.Add(PhoneNumber);

        //                     SqlBulkCopyColumnMapping UserLoginH = new SqlBulkCopyColumnMapping("UserLoginHistory", "UserLoginHistory");
        //                     bulkInsert.ColumnMappings.Add(UserLoginH);

        //                     bulkInsert.BulkCopyTimeout = 0;
        //                     bulkInsert.BatchSize = 5000;

        //                     bulkInsert.WriteToServer(dtExcel, DataRowState.Unchanged);
        //                     bulkInsert.Close();

        //             }

        //             //Functions.RetrieveProgress("Generating AccessCode");


        //             Functions.RetrieveProgress("Final Stage - Completing...");

        //             //Create AGMID and AGM Metadata.
        //             var response = UpdateExistingAGMSettingData(id);
        //             if(response!="Success")
        //             {
        //                 return Task.FromResult<string>(response);
        //             }

        //             //Create Holding and Percentage Holding for AGM
        //             response = CreateAGMHoldingAndPercentageHolding(id);
        //             if (response != "Success")
        //             {
        //                 return Task.FromResult<string>(response);
        //             }

        //             var dropColumnIndex = DropColumnIndex();
        //             if (!dropColumnIndex)
        //             {
        //                 Log.Error("SN Index was not dropped on BarcodeModels Table.");
        //                 return Task.FromResult<string>("BarcodeModel Table SN Index not droped");
        //             }

        //             return Task.FromResult<string>("Success");
                    

        //         }
        //         else
        //         {
        //             return Task.FromResult<string>("Empty");
        //         }

        //         //StopTimer();
        //         //Functions.RetrieveProgress(String.Format("File upload is {0}", "Empty"));

        //         //return "Empty";

        //     }
        //     catch (Exception e)
        //     {
        //         Log.Error(e.ToString());
        //         //StopTimer();
        //         Functions.RetrieveProgress(String.Format("File upload Error: {0}", "Undoing Data Persistence"));
        //         string insertQuery = "DELETE FROM BarcodeModels WHERE Company = '" + Companyinfo + "'";
        //         connn.Close();
        //         connn.Open();
        //         SqlCommand cmd2 = new SqlCommand(insertQuery, connn);
        //         cmd2.CommandTimeout = 180;
        //         cmd2.ExecuteNonQuery();
        //         connn.Close();
        //         var dropColumnIndex = DropColumnIndex();
        //         if(!dropColumnIndex)
        //         {
        //             Log.Error("SN Index was not dropped on BarcodeModels Table.");
        //         }
        //         Functions.RetrieveProgress(String.Format("File upload Error: {0}", "Something Went Wrong." + e));
        //         return Task.FromResult<string>("Something Went Wrong." + e);
        //         //return "Something Went Wrong." + e;
        //     }

        // }


 public async Task<string> BulkUpload4AdditionalFileAsync(int id, IFormFile file)
    {
        try
        {
            // Check if the file is empty
            if (file.Length == 0)
            {
                return "File is empty";
            }

            // Log progress
            Functions.RetrieveProgress("Initializing...");
            Log.Info("Upload file initiated");

            // Path to the directory where additional files will be stored
            string uploadDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");

            // Ensure the directory exists
            Directory.CreateDirectory(uploadDirectory);

            // Generate a unique filename for the uploaded file
            string uniqueFileName = $"{Guid.NewGuid()}_{ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')}";

            // Combine the upload directory path with the unique filename
            string filePath = Path.Combine(uploadDirectory, uniqueFileName);

            // Copy the contents of the uploaded file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Log progress
            Functions.RetrieveProgress("Retrieving data from Excel...");

            // Read connection string from appsettings.json
            string excelConnectionString = _configuration.GetConnectionString("ExcelConnection");

            // Rest of your logic for processing the Excel file
            // Make sure to replace references to HttpContext with appropriate ASP.NET Core equivalents

            return "Success";
        }
        catch (Exception e)
        {
            // Log error
            Log.Error(e.ToString());
            Functions.RetrieveProgress($"File upload error: {e.Message}");

            // Handle any cleanup or error reporting logic here
            return $"Error: {e.Message}";
        }


    }





        private string CreateAGMSettingData()
        {
            //Create Settings information for this upload

            var year = DateTime.Now.Year.ToString();
            //int newAGMID = 0;
            connn.Close();
            connn.Open();
            int codeout = 0;
            int maxvalue = 0;
            int max4mArchivevalue = 0;
            int max4mSettingsvalue = 0;

            int newRegCode = 0;

            if (!string.IsNullOrEmpty(RegCode) && int.TryParse(RegCode, out codeout))
            {
                newRegCode = codeout;
            }
            using (UsersContext dbn = db)
            {
                if (dbn.Settings.Any())
                {
                    string queryMaxValue = "SELECT MAX(AGMID) FROM SettingsModels";
                    //var ShareholderCount = db.BarcodeStore.Where(b => b.Company == companyinfo).Count();
                    var ShareholderCount = dm.GetCompanyCount(companyinfo);
                    //string query2 = "select * from BarcodeModels WHERE Name LIKE '%" + searchValue + "%' OR ShareholderNum LIKE '%" + searchValue + "%'";
                    SqlCommand cmd = new SqlCommand(queryMaxValue, connn);
                    cmd.CommandTimeout = 180;
                    SqlDataReader read = cmd.ExecuteReader();

                    //Int64 maxvalue = 0;
                    while (read.Read())
                    {
                        try
                        {
                            max4mSettingsvalue = read.GetInt32(0);
                        }
                        catch (InvalidCastException e)
                        {
                            Log.Error(e.StackTrace.ToString());
                            string insertQuery1 = "DELETE FROM BarcodeModels WHERE Company = '" + Companyinfo + "'";
                            connn.Close();
                            connn.Open();
                            SqlCommand cmd3 = new SqlCommand(insertQuery1, connn);
                            cmd3.CommandTimeout = 180;
                            cmd3.ExecuteNonQuery();
                            connn.Close();
                            Functions.RetrieveProgress(String.Format("File upload Error: {0}", "Something Went Wrong." + e));
                            return "Something Went Wrong." + e;
                        }

                    }
                    read.Close();

                    if (dbn.SettingsArchive.Any())
                    {
                        string queryMax4mValue = "SELECT MAX(AGMID) FROM SettingsModelArchives";
                        SqlCommand cmd4m = new SqlCommand(queryMax4mValue, connn);
                        cmd4m.CommandTimeout = 180;
                        SqlDataReader read4m = cmd4m.ExecuteReader();

                        //Int64 maxvalue = 0;
                        while (read4m.Read())
                        {
                            try
                            {
                                max4mArchivevalue = read4m.GetInt32(0);
                            }
                            catch (InvalidCastException e)
                            {
                                Log.Error(e.StackTrace.ToString());
                                string insertQuery1 = "DELETE FROM BarcodeModels WHERE Company = '" + Companyinfo + "'";
                                connn.Close();
                                connn.Open();
                                SqlCommand cmd3 = new SqlCommand(insertQuery1, connn);
                                cmd3.CommandTimeout = 180;
                                cmd3.ExecuteNonQuery();
                                connn.Close();
                                Functions.RetrieveProgress(String.Format("File upload Error: {0}", "Something Went Wrong." + e));
                                return "Something Went Wrong." + e;
                            }

                        }
                        read4m.Close();
                    }

                    maxvalue = max4mArchivevalue > max4mSettingsvalue ? max4mArchivevalue : max4mSettingsvalue;
                    newAGMID = maxvalue + 1;
                    string insertQuery = "INSERT INTO SettingsModels(Title,Description,CompanyName,ShareHolding,AGMID,RegCode,TotalRecordCount,Year,DateCreated,ArchiveStatus,AgmStart,AgmEnd,allChannels,mobileChannel,webChannel,smsChannel,proxyChannel,ussdChannel,AbstainBtnChoice,StopAdmittance,StartAdmittance) VALUES('" + companyinfo + " Annual General Meeting','" + companyinfo + " Annual General Meeting','" + companyinfo + "', '" + 0 + "', '" + newAGMID + "','" + newRegCode + "','" + ShareholderCount + "', '" + DateTime.Now.Year.ToString() + "', '" + DateTime.Now + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 1 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 1 + "','" + 0 + "','" + 0 + "')";
                    SqlCommand cmd2 = new SqlCommand(insertQuery, connn);
                    cmd2.CommandTimeout = 180;
                    cmd2.ExecuteNonQuery();
                    connn.Close();
                    return "Success";
                    //dbn.Settings.Add(
                    //    new SettingsModel() { Title = "Annual General Meeting", CompanyName = companyinfo.Trim(), Year = DateTime.Now.Year.ToString(),
                    //    DateCreated = DateTime.Now});
                }
                else
                {
                    //var ShareholderCount = db.BarcodeStore.Where(b => b.Company == companyinfo).Count();
                    var ShareholderCount = dm.GetCompanyCount(companyinfo);

                    if (dbn.SettingsArchive.Any())
                    {
                        string queryMax4mValue = "SELECT MAX(AGMID) FROM SettingsModelArchives";
                        SqlCommand cmd4m = new SqlCommand(queryMax4mValue, connn);
                        cmd4m.CommandTimeout = 180;
                        SqlDataReader read4m = cmd4m.ExecuteReader();

                        //Int64 maxvalue = 0;
                        while (read4m.Read())
                        {
                            try
                            {
                                max4mArchivevalue = read4m.GetInt32(0);
                            }
                            catch (InvalidCastException e)
                            {
                                string insertQuery1 = "DELETE FROM BarcodeModels WHERE Company = '" + Companyinfo + "'";
                                connn.Close();
                                connn.Open();
                                SqlCommand cmd3 = new SqlCommand(insertQuery1, connn);
                                cmd3.CommandTimeout = 180;
                                cmd3.ExecuteNonQuery();
                                connn.Close();
                                Functions.RetrieveProgress(String.Format("File upload Error: {0}", "Something Went Wrong." + e));
                                return "Something Went Wrong." + e;
                            }

                        }
                        read4m.Close();
                    }

                    maxvalue = max4mArchivevalue > max4mSettingsvalue ? max4mArchivevalue : max4mSettingsvalue;
                    newAGMID = maxvalue + 1;
                    string insertQuery = "INSERT INTO SettingsModels(Title,Description,CompanyName,ShareHolding,AGMID,RegCode,TotalRecordCount,Year,DateCreated,ArchiveStatus,AgmStart,AgmEnd,allChannels,mobileChannel,webChannel,smsChannel,proxyChannel,ussdChannel,AbstainBtnChoice,StopAdmittance,StartAdmittance) VALUES('" + companyinfo + " Annual General Meeting','" + companyinfo + " Annual General Meeting','" + companyinfo + "', '" + 0 + "', '" + newAGMID + "', '" + newRegCode + "','" + ShareholderCount + "','" + DateTime.Now.Year.ToString() + "', '" + DateTime.Now + "', '" + 0 + "','" + 0 + "','" + 0 + "','" + 1 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 1 + "','" + 0 + "','" + 0 + "')";
                    SqlCommand cmd2 = new SqlCommand(insertQuery, connn);
                    cmd2.CommandTimeout = 180;
                    cmd2.ExecuteNonQuery();
                    connn.Close();
                    return "Success";
                }

            }
        }



        private bool ExecuteColumnIndex()
        {
            try
            {
                string query = "CREATE UNIQUE INDEX SN_Index ON dbo.BarcodeModels(SN) WITH IGNORE_DUP_KEY";
                connn.Open();
                SqlCommand cmd = new SqlCommand(query, connn);
                cmd.CommandTimeout = 180;
                cmd.ExecuteNonQuery();
                connn.Close();
                return true;
            }
            catch(Exception e)
            {
                Log.Error(e.StackTrace.ToString());
                return false;
            }

        }

        private bool DropColumnIndex()
        {
            try
            {
                connn.Close();
                string query = "DROP INDEX [dbo].[BarcodeModels].SN_Index";
                connn.Open();
                SqlCommand cmd = new SqlCommand(query, connn);
                cmd.CommandTimeout = 180;
                cmd.ExecuteNonQuery();
                connn.Close();
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e.StackTrace.ToString());
                return false;
            }

        }

        private string UpdateExistingAGMSettingData(int id)
        {
            //Create Settings information for this upload
           
            using (UsersContext dbn =db)
            {
                try
                {
                    var agm = dbn.Settings.Find(id);

                    var ShareholderCount = dm.GetCompanyCount(companyinfo);
                    agm.TotalRecordCount = ShareholderCount;
                    dbn.SaveChanges();
                    return "Success";
                }
                catch(Exception e)
                {
                    Log.Error(e.StackTrace.ToString());
                    return "Something went wrong " + e.StackTrace;
                }


            }
        }

        private string CreateAGMHoldingAndPercentageHolding(int agmid)
        {
            try
            {
                Double TotalShareholding = 0;

                //Calculate Percentage Holding for all shareholder entry...
                SettingsModel setting;
                var CheckBarcodeModelStatus = db.Settings.Any();
                if (CheckBarcodeModelStatus)
                {
                    string query2 = "UPDATE SettingsModels SET ShareHolding = (SELECT ROUND(SUM(Holding),2) FROM BarcodeModels Where Company ='" + companyinfo + "') Where CompanyName = '" + companyinfo + "'";
                    connn.Open();
                    SqlCommand cmd2 = new SqlCommand(query2, connn);
                    cmd2.CommandTimeout = 180;
                    cmd2.ExecuteNonQuery();
                    connn.Close();
                    setting = db.Settings.FirstOrDefault(s => s.AGMID == agmid);

                    TotalShareholding = setting != null ? setting.ShareHolding : 0;
                    //return System.Threading.Tasks.Task.FromResult<bool>(true);
                }

                //calculate Percentage Holding for a company
                if (TotalShareholding != 0)
                {

                    //con1 = new SqlConnection(conn);
                    string query3 = "UPDATE BarcodeModels SET PercentageHolding = (Holding /'" + TotalShareholding + "') * 100 Where Company='" + companyinfo + "'";
                    connn.Open();
                    SqlCommand cmd3 = new SqlCommand(query3, connn);
                    cmd3.CommandTimeout = 180;
                    cmd3.ExecuteNonQuery();
                    connn.Close();
                }

                //Generate accesscodes
                var shareholders = db.BarcodeStore.Where(s => s.Company.ToLower() == companyinfo.Trim().ToLower() && s.emailAddress != null && s.accesscode == null).ToList();
                for (int i = 0; i < shareholders.Count; i++)
                {
                    var accesscode = ua.GetAccessCode();
                    var requesturi = Utilities.GenerateAGMUrl(companyinfo, agmid, shareholders[i].emailAddress);
                    //dbManager.UpdateBacodeModelWithAccessCode(shareholders[i].Id, accesscode, requesturi);
                    shareholders[i].accesscode = accesscode.Insert(0, "S");
                    shareholders[i].OnlineEventUrl = requesturi;
                }
                db.SaveChangesAsync();
                //StopTimer();
                Functions.RetrieveProgress(String.Format("Task completed @{0}", DateTime.Now.ToString("h:mm:ss")));
                return "Success";
            }
            catch(Exception e)
            {
                Log.Error(e.StackTrace.ToString());
                return "Something went wrong" + e.StackTrace;
            }
            
            //return String.Format("Task completed @{0}", DateTime.Now.ToString("h:mm:ss"));
            //}
            ////StopTimer();
            //Functions.RetrieveProgress(String.Format("File upload @{0}", "Exist"));
            //return Task.FromResult<string>("Full");
            ////return "Full";

        }

    }
}