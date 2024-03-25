// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;
// using BarcodeGenerator.Util;
// using ClosedXML.Excel;
// using Gapplus.Application.Helpers;
// using Hangfire;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.EntityFrameworkCore;
// using Spire.Xls;
// using System;
// using System.Collections.Generic;
// using System.Configuration;
// using System.Data;
// using System.Data.Common;
// using System.Data.Odbc;
// using System.Data.OleDb;
// using System.Data.SqlClient;
// using System.Diagnostics;
// using System.Drawing;
// using System.IO;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Runtime.InteropServices;
// using System.Threading.Tasks;
// using System.Web;
// using System.Web.Mvc;
// using System.Xml;

// namespace BarcodeGenerator.Controllers
// {
//     public class DataUploadController : Controller
//     {
//         //
//         // GET: /DataUpload/

//         UsersContext db;
//         UserAdmin ua;
//         dbManager dm;
//         DataUploadService ds;

//         public DataUploadController(UsersContext context, IWebHostEnvironment _webHostEnv)
//         {
//             db = context;
//             ua = new UserAdmin(db);
//             dm = new dbManager(); //DATABASE MANAGER FOR RAW SQL COMMANDS
//             ds = new DataUploadService(db, _webHostEnv);
//         }
//         byte[] BarCode;
//         // private static string  connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//         private static string connStr = DatabaseManager.GetConnectionString();

//         private SqlConnection connn =
//                 new SqlConnection(connStr);
//         private string Company = "";
//         private long FileUploadCount = 0;
//         private static TimeSpan FileUploadTime;



//         public ActionResult Index()
//         {

//             return View();
//         }





//             //OUTDATED LOGIC ,----> UNSUPPORTED VERSION
//         // [HttpPost]
//         // public async Task<string> UploadExcel(HttpPostedFileBase file)
//         // {
//         //     string fileLocation = "";
//         //     string fileExtension = "";

//         //     if (Request.Files["file"].ContentLength > 0)
//         //     {
//         //         fileExtension =
//         //                              System.IO.Path.GetExtension(Request.Files["file"].FileName);

//         //         if (fileExtension == ".xls" || fileExtension == ".xlsx")
//         //         {
//         //             fileLocation = Server.MapPath("~/Uploads/") + Request.Files["file"].FileName;
//         //             if (System.IO.File.Exists(fileLocation))
//         //             {

//         //                 System.IO.File.Delete(fileLocation);
//         //             }
//         //             Request.Files["file"].SaveAs(fileLocation);

//         //         }

//         //     }
//         //     var response = await UploadExcelAsync(fileLocation, fileExtension);

//         //     return response;
//         // }


// [HttpPost]
//         public async Task<string> UploadExcel(IFormFile file)
//         {
//             if (file == null || file.Length == 0)
//                 return "No file uploaded";

//             string fileLocation = "";
//             string fileExtension = Path.GetExtension(file.FileName);

//             if (fileExtension == ".xls" || fileExtension == ".xlsx")
//             {
//                 fileLocation = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.FileName);

//                 if (System.IO.File.Exists(fileLocation))
//                     System.IO.File.Delete(fileLocation);

//                 using (var stream = new FileStream(fileLocation, FileMode.Create))
//                 {
//                     await file.CopyToAsync(stream);
//                 }
//             }

//             var response = await UploadExcelAsync(fileLocation, fileExtension);
//             return response;
//         }















//         public Task<string> UploadExcelAsync(string fileLocation, string fileExtension)
//         {

//             StartTimer();
//             //fileName = FileUpload1.ResolveClientUrl(FileUpload1.PostedFile.FileName);

//             string excelConnectionString = string.Empty;
//             SqlConnection con1;
//             try
//             {
//                 Functions.RetrieveProgress("Intializing...");
//                 var CheckIfTableExist = db.BarcodeStore;
//                 if (!(CheckIfTableExist.Count() > 0))
//                 {

//                     if (fileExtension != "" && fileLocation != "")
//                     {

//                         excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//                         fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
//                         //connection String for xls file format.
//                         if (fileExtension == ".xls")
//                         {
//                             excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
//                             fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
//                         }
//                         //connection String for xlsx file format.
//                         else if (fileExtension == ".xlsx")
//                         {
//                             excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//                             fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
//                         }
//                         //Create Connection to Excel work book and add oledb namespace
//                         DataTable dtExcel = new DataTable();
//                         //string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";
//                         OleDbConnection con = new OleDbConnection(excelConnectionString);
//                         string query = "Select * from [Sheet1$]";
//                         OleDbDataAdapter data = new OleDbDataAdapter(query, con);
//                         data.Fill(dtExcel);
//                         string companyinfo = dtExcel.Rows[0][1].ToString();
//                         Company = companyinfo;

//                         for (int i = 0; i < dtExcel.Rows.Count; i++)
//                         {
//                             // string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                             string conn = DatabaseManager.GetConnectionString();
//                             con1 = new SqlConnection(conn);
//                             string query2 = "Insert into BarcodeModels(SN,Company,ShareholderNum,Name,Address,Holding,emailAddress,PhoneNumber,PercentageHolding,Present,PresentByProxy,split,resolution,combined,TakePoll,ParentAccountNumber,Selected,Consolidated,NotVerifiable) Values('" +
//                             dtExcel.Rows[i][0].ToString() + "','" + dtExcel.Rows[i][1].ToString() +
//                             "','" + dtExcel.Rows[i][2].ToString() + "','" + dtExcel.Rows[i][3].ToString() +
//                             "','" + dtExcel.Rows[i][4].ToString() + "','" + dtExcel.Rows[i][5].ToString() + "','" + dtExcel.Rows[i][6].ToString() + "','" + dtExcel.Rows[i][7].ToString() + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "')";
//                             con1.Open();
//                             SqlCommand cmd = new SqlCommand(query2, con1);
//                             cmd.ExecuteNonQuery();
//                             con1.Close();

//                             //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
//                             Functions.SendProgress("Process in progress...", i, dtExcel.Rows.Count);
//                         }
//                         StopTimer();

//                         var currentYear = DateTime.Now.Year.ToString();
//                         var Date = DateTime.Now;
//                         if (!(db.Settings.Where(y => y.Year == currentYear).Count() > 0))
//                         {
//                             db.Settings.Add(
//                                 new SettingsModel() { Title = "Annual General Meeting", CompanyName = dtExcel.Rows[0][1].ToString(), Year = currentYear, DateCreated = Date }
//                                 );
//                             db.SaveChanges();
//                         }

//                         Functions.RetrieveProgress(String.Format("Task completed @{0}", DateTime.Now.ToString("h:mm:ss")));
//                         return System.Threading.Tasks.Task.FromResult<string>("Success");
//                         //return "Upload Completed Successfully!";
//                     }
//                     StopTimer();
//                     HttpResponseMessage response2 = new HttpResponseMessage();
//                     response2.StatusCode = HttpStatusCode.NoContent;
//                     TempData["Message1"] = "Upload File is Empty!";
//                     return System.Threading.Tasks.Task.FromResult<string>("Empty");
//                     //return "Upload File is Empty!";
//                 }
//                 return System.Threading.Tasks.Task.FromResult<string>("Full");
//             }
//             catch (OleDbException e)
//             {
//                 StopTimer();
//                 HttpResponseMessage response = new HttpResponseMessage();
//                 response.StatusCode = HttpStatusCode.NotAcceptable;
//                 TempData["Message1"] = "Something Went Wrong." + e;
//                 return System.Threading.Tasks.Task.FromResult<string>("Something Went Wrong." + e.Message);
//                 //return "Something Went Wrong." + e; 
//             }

//         }



//     //outdated  
//         // [HttpPost]
//         // public ActionResult ProxyListUpload(HttpPostedFileBase file)
//         // {

//         //     //fileName = FileUpload1.ResolveClientUrl(FileUpload1.PostedFile.FileName);

//         //     string excelConnectionString = string.Empty;
//         //     SqlConnection con1;
//         //     try
//         //     {

//         //         if (Request.Files["file"].ContentLength > 0)
//         //         {
//         //             string fileExtension =
//         //                                  System.IO.Path.GetExtension(Request.Files["file"].FileName);

//         //             if (fileExtension == ".xls" || fileExtension == ".xlsx")
//         //             {
//         //                 string fileLocation = Server.MapPath("~/Uploads/ProxyListUpload/") + Request.Files["file"].FileName;
//         //                 if (System.IO.File.Exists(fileLocation))
//         //                 {

//         //                     System.IO.File.Delete(fileLocation);
//         //                 }
//         //                 Request.Files["file"].SaveAs(fileLocation);

//         //                 excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//         //                 fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
//         //                 //connection String for xls file format.
//         //                 if (fileExtension == ".xls")
//         //                 {
//         //                     excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
//         //                     fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
//         //                 }
//         //                 //connection String for xlsx file format.
//         //                 else if (fileExtension == ".xlsx")
//         //                 {
//         //                     excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//         //                     fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
//         //                 }
//         //             }

//         //             //Create Connection to Excel work book and add oledb namespace
//         //             DataTable dtExcel = new DataTable();
//         //             //string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";
//         //             OleDbConnection con = new OleDbConnection(excelConnectionString);
//         //             string query = "Select * from [Sheet1$]";
//         //             OleDbDataAdapter data = new OleDbDataAdapter(query, con);
//         //             data.Fill(dtExcel);
//         //             // string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//         //             string conn = DatabaseManager.GetConnectionString();
//         //             con1 = new SqlConnection(conn);
//         //             for (int i = 0; i < dtExcel.Rows.Count; i++)
//         //             {
//         //                 //con1 = new SqlConnection(conn);
//         //                 string query1 = "Insert into Proxylists(ShareholderNum,Validity) Values('" +
//         //                 dtExcel.Rows[i][0].ToString() + "','" + 0 + "')";
//         //                 con1.Open();
//         //                 SqlCommand cmd1 = new SqlCommand(query1, con1);
//         //                 cmd1.ExecuteNonQuery();
//         //                 con1.Close();

//         //                 //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
//         //                 Functions.SendProgress("Process in progress...", i, dtExcel.Rows.Count);
//         //             }
//         //             var proxylist = db.ProxyList.ToList();
//         //             for (int i = 0; i < proxylist.Count; i++)
//         //             {
//         //                 var shareholdernum = proxylist[i].ShareholderNum;
//         //                 var proxy = db.BarcodeStore.SingleOrDefault(b => b.ShareholderNum == shareholdernum);
//         //                 if (proxy != null)
//         //                 {
//         //                     proxy.PresentByProxy = true;
//         //                     proxy.NotVerifiable = true;
//         //                     proxylist[i].Validity = true;

//         //                     var present = db.Present.Where(r => r.ShareholderNum == proxy.ShareholderNum);
//         //                     if (!present.Any())
//         //                     {
//         //                         string phonenumber = "";
//         //                         if (!String.IsNullOrEmpty(proxy.PhoneNumber))
//         //                         {
//         //                             char[] arr = proxy.PhoneNumber.Where(c => (char.IsLetterOrDigit(c))).ToArray();

//         //                             proxy.PhoneNumber = new string(arr);
//         //                             if (proxy.PhoneNumber.StartsWith("234"))
//         //                             {
//         //                                 phonenumber = proxy.PhoneNumber;
//         //                             }
//         //                             else if (proxy.PhoneNumber.StartsWith("0"))
//         //                             {

//         //                                 double number = double.Parse(proxy.PhoneNumber);
//         //                                 phonenumber = "234" + number.ToString();

//         //                             }

//         //                         }
//         //                         PresentModel model = new PresentModel();
//         //                         model.Name = proxy.Name;
//         //                         model.Address = proxy.Address;
//         //                         model.Company = proxy.Company;
//         //                         model.ShareholderNum = proxy.ShareholderNum;
//         //                         model.ParentNumber = proxy.ParentAccountNumber;
//         //                         model.Holding = proxy.Holding;
//         //                         model.PhoneNumber = phonenumber;
//         //                         if (!String.IsNullOrEmpty(proxy.emailAddress))
//         //                         {
//         //                             model.emailAddress = proxy.emailAddress;
//         //                         }

//         //                         model.PercentageHolding = proxy.PercentageHolding;
//         //                         model.proxy = true;
//         //                         model.PresentTime = DateTime.Now;
//         //                         model.Timestamp = DateTime.Now.TimeOfDay;

//         //                         //db.Entry(emailEntry).State = EntityState.Modified;
//         //                         db.Present.Add(model);
//         //                     }

//         //                 }


//         //                 //    string query2 = "UPDATE BarcodeModels SET PresentByProxy = 'TRUE' WHEN ShareholderNum = '" + proxylist[i].ShareholderNum + "'" +
//         //                 //"ELSE UPDATE ProxyLists SET Validity =''";
//         //                 //    con1.Open();
//         //                 //    SqlCommand cmd2 = new SqlCommand(query2, con1);
//         //                 //    cmd2.ExecuteNonQuery();
//         //                 //    con1.Close();
//         //             }
//         //             db.SaveChanges();
//         //             HttpResponseMessage response = new HttpResponseMessage();
//         //             response.StatusCode = HttpStatusCode.OK;
//         //             TempData["Message"] = "Upload Completed Successfully!";
//         //             return RedirectToAction("Index", "BarcodeLib");
//         //         }
//         //         HttpResponseMessage response2 = new HttpResponseMessage();
//         //         response2.StatusCode = HttpStatusCode.NoContent;
//         //         TempData["Message1"] = "Upload File is Empty!";
//         //         return RedirectToAction("Index", "BarcodeLib");
//         //     }
//         //     catch (OleDbException e)
//         //     {
//         //         HttpResponseMessage response = new HttpResponseMessage();
//         //         response.StatusCode = HttpStatusCode.NotAcceptable;
//         //         TempData["Message1"] = "Something Went Wrong." + e.Message;
//         //         return RedirectToAction("Index", "BarcodeLib");
//         //     }

//         // }
        
        
        
        
        
        
        
        
        
        
//         //
//         // GET: /DataUpload/Create

//         public string Createbarcode()
//         {

//             try
//             {

//                 var data = db.BarcodeStore.Where(b => b.emailAddress != "").ToArray();
//                 if (data.Count() > 0)
//                 {
//                     var i = 0;
//                     //for (int i = 0; i < data.Length; i++)
//                     foreach (var item in data)
//                     {

//                         var qrgenerator = new Qrcode();
//                         //byte[] qrcode;
//                         string qrcode;
//                         if (item.emailAddress != "" || item.emailAddress != null)
//                         {
//                             qrcode = qrgenerator.GenerateMyQCCode(item.emailAddress, item.ShareholderNum.ToString());
//                             item.Barcode = item.emailAddress;
//                             item.ImageUrl = qrcode;
//                             db.Entry(item).State = EntityState.Modified;
//                         }
//                         Functions.SendProgress("Process in progress...", i, data.Count());
//                         i++;
//                     }
//                     db.SaveChanges();

//                     //    barcodecs objbar = new barcodecs();
//                     //    //string numberToEncode = objbar.generateBarcode();
//                     //    string numberToEncode = data[i].ShareholderNum;
//                     //    int W = Convert.ToInt32(200);
//                     //    int H = Convert.ToInt32(80);
//                     //    BarcodeLib.Barcode b = new BarcodeLib.Barcode(numberToEncode);
//                     //    b.Alignment = BarcodeLib.AlignmentPositions.LEFT;
//                     //    BarcodeLib.TYPE type = BarcodeLib.TYPE.UNSPECIFIED;
//                     //    type = BarcodeLib.TYPE.CODE128;

//                     //    b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
//                     //    b.IncludeLabel = true;
//                     //    b.Encode(type,
//                     //  b.RawData.ToUpper(), Color.Black, Color.White, W, H);
//                     //    using (MemoryStream Mmst = new MemoryStream())
//                     //    {
//                     //        b.SaveImage(Mmst, BarcodeLib.SaveTypes.JPG);
//                     //        BarCode = Mmst.GetBuffer();

//                     //    }
//                     //    using (UsersContext dbo = new UsersContext())
//                     //    {
//                     //        var model = dbo.BarcodeStore.Find(data[i].Id);
//                     //        model.Barcode = numberToEncode;
//                     //        model.BarcodeImage = BarCode;
//                     //        dbo.Entry(model).State = EntityState.Modified;
//                     //        dbo.SaveChanges();
//                     //    };
//                     //    //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
//                     //    Functions.SendProgress("Process in progress...", i, data.Length);
//                     //}

//                     return " Barcode Generation Completed Successfully!";
//                 }

//                 return "No account with email address!";
//             }
//             catch (Exception e)
//             {
//                 var MessageException = " Barcode Generation Failed " + " " + e + " " + " - Clear Database and re-upload data!";
//                 return MessageException;
//             }


//         }

//         [HttpPost]
//         public async Task<string> BulkUploadAadditionalFile(int id)
//         {
//             var uploadedFile = Request.Files["file"];
//             var response = await ds.BulkUpload4AdditionalFileAsync(id, uploadedFile);

//             return response;
//         }

//         [HttpPost]
//         public async Task<string> BulkUpload2()
//         {

//             var response = await BulkUploadAsync();

//             return response;
//         }


//         private Task<string> BulkUploadAsync()
//         {
//             try
//             {

//                 if (Request.Files["file"].ContentLength > 0)
//                 {
//                     Functions.RetrieveProgress("Intializing...");
//                     //ResetTimer();
//                     //StartTimer();
//                     Log.Info("Upload file Initiated");
//                     string excelCS = string.Empty;
//                     string companyinfo;
//                     string RegCode;

//                     string fileLocation = Server.MapPath("~/Uploads/") + Request.Files["file"].FileName;
//                     string fileName = Request.Files["file"].FileName;
//                     string fileExtension = System.IO.Path.GetExtension(fileName);
//                     if (System.IO.File.Exists(fileLocation))
//                     {

//                         System.IO.File.Delete(fileLocation);
//                     }
//                     Request.Files["file"].SaveAs(fileLocation);
//                     //string path = string.Concat(Server.MapPath("~/UploadFile/" + file.FileName));
//                     //file.SaveAs(path);
//                     // Connection String to Excel Workbook  
//                     if (fileExtension == ".xls" || fileExtension == ".xlsx")
//                     {

//                         excelCS = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//                             fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
//                         //connection String for xls file format.
//                         if (fileExtension == ".xls")
//                         {
//                             excelCS = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
//                             fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
//                         }
//                         //connection String for xlsx file format.
//                         else if (fileExtension == ".xlsx")
//                         {
//                             excelCS = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//                             fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
//                         }
//                     }
//                     //excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", fileLocation);
//                     Functions.RetrieveProgress("Retrieving Data from Excel..");

//                     using (OleDbConnection con = new OleDbConnection(excelCS))
//                     {
//                         //Create Connection to Excel work book and add oledb namespace
//                         DataTable dtExcel = new DataTable();
//                         //DataColumn shareholderNum = new DataColumn("ShareholderNum", typeof(long));
//                         //shareholderNum.DefaultValue = 0;
//                         //dtExcel.Columns.Add(shareholderNum);
//                         DataColumn pHolding = new DataColumn("PercentageHolding", typeof(long));
//                         pHolding.DefaultValue = 0;
//                         dtExcel.Columns.Add(pHolding);
//                         DataColumn Present = new DataColumn("Present", typeof(bool));
//                         Present.DefaultValue = 0;
//                         dtExcel.Columns.Add(Present);
//                         DataColumn PresentByProxy = new DataColumn("PresentByProxy", typeof(bool));
//                         PresentByProxy.DefaultValue = 0;
//                         dtExcel.Columns.Add(PresentByProxy);
//                         DataColumn Preregistered = new DataColumn("Preregistered", typeof(bool));
//                         Preregistered.DefaultValue = 0;
//                         dtExcel.Columns.Add(Preregistered);
//                         DataColumn split = new DataColumn("split", typeof(bool));
//                         split.DefaultValue = 0;
//                         dtExcel.Columns.Add(split);
//                         DataColumn resolution = new DataColumn("resolution", typeof(bool));
//                         resolution.DefaultValue = 0;
//                         dtExcel.Columns.Add(resolution);
//                         DataColumn combined = new DataColumn("combined", typeof(bool));
//                         combined.DefaultValue = 0;
//                         dtExcel.Columns.Add(combined);
//                         DataColumn TakePoll = new DataColumn("TakePoll", typeof(bool));
//                         TakePoll.DefaultValue = 0;
//                         dtExcel.Columns.Add(TakePoll);
//                         DataColumn ParentAccountNumber = new DataColumn("ParentAccountNumber", typeof(bool));
//                         ParentAccountNumber.DefaultValue = 0;
//                         dtExcel.Columns.Add(ParentAccountNumber);
//                         DataColumn Selected = new DataColumn("Selected", typeof(bool));
//                         Selected.DefaultValue = 0;
//                         dtExcel.Columns.Add(Selected);
//                         DataColumn Consolidated = new DataColumn("Consolidated", typeof(bool));
//                         Consolidated.DefaultValue = 0;
//                         dtExcel.Columns.Add(Consolidated);
//                         DataColumn NotVerifiable = new DataColumn("NotVerifiable", typeof(bool));
//                         NotVerifiable.DefaultValue = 0;
//                         dtExcel.Columns.Add(NotVerifiable);
//                         DataColumn AddedSplitAccount = new DataColumn("AddedSplitAccount", typeof(bool));
//                         AddedSplitAccount.DefaultValue = 0;
//                         dtExcel.Columns.Add(AddedSplitAccount);
//                         DataColumn UserLoginHistory = new DataColumn("UserLoginHistory", typeof(bool));
//                         UserLoginHistory.DefaultValue = 0;
//                         dtExcel.Columns.Add(UserLoginHistory);

//                         string query = "Select * from [Sheet1$]";
//                         //string query = "Select * from [Sheet1$] WHERE NOT [ShareholderNum] IS NULL";


//                         OleDbCommand cmd = new OleDbCommand(query, con);

//                         //using (OleDbDataReader dr = cmd.ExecuteReader())
//                         //{
//                         //    while (dr.Read())
//                         //    {
//                         //        var row1Col0 = dr[0];
//                         //        Console.WriteLine(row1Col0);
//                         //    }
//                         //}

//                         OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
//                         //var dataTable = dtExcel.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
//                         //adapter.RowUpdating += new OleDbRowUpdatingEventHandler(oleDbDataAdapter1_RowUpdating);
//                         adapter.Fill(dtExcel);
//                         //// add handlers
//                         ////adapter.RowUpdating += new OleDbRowUpdatingEventHandler(OnRowUpdating);
//                         ////adapter.RowUpdating += new OleDbRowUpdatingEventHandler(oleDbDataAdapter1_RowUpdating);

//                         con.Close();
//                         RegCode = dtExcel.Rows[0][15].ToString();
//                         companyinfo = dtExcel.Rows[0][16].ToString();
//                         //RegCode = dtExcel.Rows[0][14].ToString();
//                         Company = companyinfo;
//                         FileUploadCount = dtExcel.Rows.Count;
//                         Log.Info("Rows Count" + "-" + FileUploadCount);
//                         //Log.Info("Null row number"+""+ dtExcel.Rows[0][13].ToString());

//                         //return Task.FromResult<string>("Test Done");
//                         //var companies = db.BarcodeStore.Select(o => o.Company).Distinct().OrderBy(k => k).ToList();
//                         var companies = dm.GetBacodeModelCompanies();
//                         if (!companies.Contains(companyinfo))
//                         {
//                             Functions.RetrieveProgress("Persisting Data to Database... ");
//                             // string connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                             string connstr = DatabaseManager.GetConnectionString();
//                             // Bulk Copy to SQL Server   
//                             SqlBulkCopy bulkInsert = new SqlBulkCopy(connstr, SqlBulkCopyOptions.Default & SqlBulkCopyOptions.KeepIdentity);

//                             bulkInsert.DestinationTableName = "BarcodeModels";

//                             // Set up the event handler to notify after 50 rows.
//                             //bulkInsert.SqlRowsCopied +=
//                             //    new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
//                             //bulkInsert.NotifyAfter = 50;

//                             SqlBulkCopyColumnMapping SN = new SqlBulkCopyColumnMapping("SN", "SN");
//                             bulkInsert.ColumnMappings.Add(SN);

//                             SqlBulkCopyColumnMapping Regcode = new SqlBulkCopyColumnMapping("RegCode", "RegCode");
//                             bulkInsert.ColumnMappings.Add(Regcode);

//                             SqlBulkCopyColumnMapping Name = new SqlBulkCopyColumnMapping("Name", "Name");
//                             bulkInsert.ColumnMappings.Add(Name);

//                             SqlBulkCopyColumnMapping Company = new SqlBulkCopyColumnMapping("Company", "Company");
//                             bulkInsert.ColumnMappings.Add(Company);


//                             SqlBulkCopyColumnMapping Holding = new SqlBulkCopyColumnMapping("Holding", "Holding");
//                             bulkInsert.ColumnMappings.Add(Holding);

//                             SqlBulkCopyColumnMapping Address = new SqlBulkCopyColumnMapping("Address", "Address");
//                             bulkInsert.ColumnMappings.Add(Address);

//                             SqlBulkCopyColumnMapping PercentageHolding = new SqlBulkCopyColumnMapping("PercentageHolding", "PercentageHolding");
//                             bulkInsert.ColumnMappings.Add(PercentageHolding);

//                             SqlBulkCopyColumnMapping ShareholderNum = new SqlBulkCopyColumnMapping("ShareholderNum", "ShareholderNum");
//                             bulkInsert.ColumnMappings.Add(ShareholderNum);

//                             SqlBulkCopyColumnMapping selected = new SqlBulkCopyColumnMapping("Selected", "Selected");
//                             bulkInsert.ColumnMappings.Add(selected);

//                             SqlBulkCopyColumnMapping consolidated = new SqlBulkCopyColumnMapping("Consolidated", "Consolidated");
//                             bulkInsert.ColumnMappings.Add(consolidated);

//                             SqlBulkCopyColumnMapping Prsent = new SqlBulkCopyColumnMapping("Present", "Present");
//                             bulkInsert.ColumnMappings.Add(Prsent);

//                             SqlBulkCopyColumnMapping PbyProxy = new SqlBulkCopyColumnMapping("PresentByProxy", "PresentByProxy");
//                             bulkInsert.ColumnMappings.Add(PbyProxy);
//                             SqlBulkCopyColumnMapping Pregistered = new SqlBulkCopyColumnMapping("Preregistered", "Preregistered");
//                             bulkInsert.ColumnMappings.Add(Pregistered);

//                             SqlBulkCopyColumnMapping splitt = new SqlBulkCopyColumnMapping("split", "split");
//                             bulkInsert.ColumnMappings.Add(splitt);
//                             SqlBulkCopyColumnMapping Resolution = new SqlBulkCopyColumnMapping("resolution", "resolution");
//                             bulkInsert.ColumnMappings.Add(Resolution);
//                             SqlBulkCopyColumnMapping Combined = new SqlBulkCopyColumnMapping("combined", "combined");
//                             bulkInsert.ColumnMappings.Add(Combined);

//                             SqlBulkCopyColumnMapping Takepoll = new SqlBulkCopyColumnMapping("TakePoll", "TakePoll");
//                             bulkInsert.ColumnMappings.Add(Takepoll);

//                             SqlBulkCopyColumnMapping NotVerify = new SqlBulkCopyColumnMapping("NotVerifiable", "NotVerifiable");
//                             bulkInsert.ColumnMappings.Add(NotVerify);

//                             SqlBulkCopyColumnMapping AddedSplitAcc = new SqlBulkCopyColumnMapping("AddedSplitAccount", "AddedSplitAccount");
//                             bulkInsert.ColumnMappings.Add(AddedSplitAcc);

//                             SqlBulkCopyColumnMapping EmailAddress = new SqlBulkCopyColumnMapping("emailAddress", "emailAddress");
//                             bulkInsert.ColumnMappings.Add(EmailAddress);

//                             SqlBulkCopyColumnMapping ParentAccount = new SqlBulkCopyColumnMapping("ParentAccountNumber", "ParentAccountNumber");
//                             bulkInsert.ColumnMappings.Add(ParentAccount);

//                             SqlBulkCopyColumnMapping PhoneNumber = new SqlBulkCopyColumnMapping("PhoneNumber", "PhoneNumber");
//                             bulkInsert.ColumnMappings.Add(PhoneNumber);

//                             SqlBulkCopyColumnMapping UserLoginH = new SqlBulkCopyColumnMapping("UserLoginHistory", "UserLoginHistory");
//                             bulkInsert.ColumnMappings.Add(UserLoginH);

//                             bulkInsert.BulkCopyTimeout = 0;
//                             bulkInsert.BatchSize = 5000;

//                             bulkInsert.WriteToServer(dtExcel, DataRowState.Unchanged);
//                             bulkInsert.Close();


//                         }
//                         else
//                         {
//                             //StopTimer();
//                             Functions.RetrieveProgress(String.Format("{0} data Exist", companyinfo));
//                             return Task.FromResult<string>("Full");
//                             //return "Full";
//                         }
//                     }

//                     Functions.RetrieveProgress("Generating AccessCode");



//                     //string UpdateAccessCodeQuery = "UPDATE BarcodeModels SET accesscode = (Holding /'" + TotalShareholding + "') * 100 Where Company='" + companyinfo + "'";
//                     //connn.Open();
//                     //SqlCommand cmd3 = new SqlCommand(UpdateAccessCodeQuery, connn);
//                     //cmd3.ExecuteNonQuery();
//                     //connn.Close();


//                     Functions.RetrieveProgress("Final Stage - Completing...");

//                     //Create Settings information for this upload



//                     var year = DateTime.Now.Year.ToString();
//                     int newAGMID = 0;
//                     int codeout = 0;
//                     int newRegCode = 0;

//                     if (!string.IsNullOrEmpty(RegCode) && int.TryParse(RegCode, out codeout))
//                     {
//                         newRegCode = codeout;
//                     }

//                     connn.Close();
//                     connn.Open();
//                     int maxvalue = 0;
//                     int max4mArchivevalue = 0;
//                     int max4mSettingsvalue = 0;
//                     using (UsersContext dbn = new UsersContext())
//                     {
//                         if (dbn.Settings.Any())
//                         {

//                             string queryMaxValue = "SELECT MAX(AGMID) FROM SettingsModels";
//                             //var ShareholderCount = db.BarcodeStore.Where(b => b.Company == companyinfo).Count();
//                             var ShareholderCount = dm.GetCompanyCount(companyinfo);
//                             //string query2 = "select * from BarcodeModels WHERE Name LIKE '%" + searchValue + "%' OR ShareholderNum LIKE '%" + searchValue + "%'";
//                             SqlCommand cmd = new SqlCommand(queryMaxValue, connn);
//                             cmd.CommandTimeout = 180;
//                             SqlDataReader read = cmd.ExecuteReader();

//                             //Int64 maxvalue = 0;
//                             while (read.Read())
//                             {
//                                 try
//                                 {
//                                     max4mSettingsvalue = read.GetInt32(0);
//                                 }
//                                 catch (InvalidCastException e)
//                                 {
//                                     string insertQuery1 = "DELETE FROM BarcodeModels WHERE Company = '" + Company + "'";
//                                     connn.Close();
//                                     connn.Open();
//                                     SqlCommand cmd3 = new SqlCommand(insertQuery1, connn);
//                                     cmd3.CommandTimeout = 180;
//                                     cmd3.ExecuteNonQuery();
//                                     connn.Close();
//                                     Functions.RetrieveProgress(String.Format("File upload Error: {0}", "Something Went Wrong." + e));
//                                     return Task.FromResult<string>("Something Went Wrong." + e);
//                                 }

//                             }
//                             read.Close();

//                             if (dbn.SettingsArchive.Any())
//                             {
//                                 string queryMax4mValue = "SELECT MAX(AGMID) FROM SettingsModelArchives";
//                                 SqlCommand cmd4m = new SqlCommand(queryMax4mValue, connn);
//                                 cmd4m.CommandTimeout = 180;
//                                 SqlDataReader read4m = cmd4m.ExecuteReader();

//                                 //Int64 maxvalue = 0;
//                                 while (read4m.Read())
//                                 {
//                                     try
//                                     {
//                                         max4mArchivevalue = read4m.GetInt32(0);
//                                     }
//                                     catch (InvalidCastException e)
//                                     {
//                                         string insertQuery1 = "DELETE FROM BarcodeModels WHERE Company = '" + Company + "'";
//                                         connn.Close();
//                                         connn.Open();
//                                         SqlCommand cmd3 = new SqlCommand(insertQuery1, connn);
//                                         cmd3.CommandTimeout = 180;
//                                         cmd3.ExecuteNonQuery();
//                                         connn.Close();
//                                         Functions.RetrieveProgress(String.Format("File upload Error: {0}", "Something Went Wrong." + e));
//                                         return Task.FromResult<string>("Something Went Wrong." + e);
//                                     }

//                                 }
//                                 read4m.Close();
//                             }

//                             maxvalue = max4mArchivevalue > max4mSettingsvalue ? max4mArchivevalue : max4mSettingsvalue;
//                             newAGMID = maxvalue + 1;
//                             string insertQuery = "INSERT INTO SettingsModels(Title,Description,CompanyName,ShareHolding,AGMID,RegCode,ProxyVoteResult,TotalRecordCount,Year,DateCreated,ArchiveStatus,AgmStart,AgmEnd,allChannels,mobileChannel,webChannel,smsChannel,proxyChannel,ussdChannel,AbstainBtnChoice,MessagingChoice,StopAdmittance,StartAdmittance,StopVoting,StartVoting,CountDownValue) VALUES('" + companyinfo + " Annual General Meeting','" + companyinfo + " Annual General Meeting','" + companyinfo + "', '" + 0 + "', '" + newAGMID + "', '" + newRegCode + "','" + 0 + "','" + ShareholderCount + "', '" + DateTime.Now.Year.ToString() + "', '" + DateTime.Now + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 1 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 1 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 30 + "')";
//                             SqlCommand cmd2 = new SqlCommand(insertQuery, connn);
//                             cmd2.CommandTimeout = 180;
//                             cmd2.ExecuteNonQuery();
//                             connn.Close();
//                             //dbn.Settings.Add(
//                             //    new SettingsModel() { Title = "Annual General Meeting", CompanyName = companyinfo.Trim(), Year = DateTime.Now.Year.ToString(),
//                             //    DateCreated = DateTime.Now});
//                         }
//                         else
//                         {
//                             //var ShareholderCount = db.BarcodeStore.Where(b => b.Company == companyinfo).Count();
//                             var ShareholderCount = dm.GetCompanyCount(companyinfo);

//                             if (dbn.SettingsArchive.Any())
//                             {
//                                 string queryMax4mValue = "SELECT MAX(AGMID) FROM SettingsModelArchives";
//                                 SqlCommand cmd4m = new SqlCommand(queryMax4mValue, connn);
//                                 cmd4m.CommandTimeout = 180;
//                                 SqlDataReader read4m = cmd4m.ExecuteReader();

//                                 //Int64 maxvalue = 0;
//                                 while (read4m.Read())
//                                 {
//                                     try
//                                     {
//                                         max4mArchivevalue = read4m.GetInt32(0);
//                                     }
//                                     catch (InvalidCastException e)
//                                     {
//                                         string insertQuery1 = "DELETE FROM BarcodeModels WHERE Company = '" + Company + "'";
//                                         connn.Close();
//                                         connn.Open();
//                                         SqlCommand cmd3 = new SqlCommand(insertQuery1, connn);
//                                         cmd3.CommandTimeout = 180;
//                                         cmd3.ExecuteNonQuery();
//                                         connn.Close();
//                                         Functions.RetrieveProgress(String.Format("File upload Error: {0}", "Something Went Wrong." + e));
//                                         return Task.FromResult<string>("Something Went Wrong." + e);
//                                     }

//                                 }
//                                 read4m.Close();
//                             }

//                             maxvalue = max4mArchivevalue > max4mSettingsvalue ? max4mArchivevalue : max4mSettingsvalue;
//                             newAGMID = maxvalue + 1;
//                             string insertQuery = "INSERT INTO SettingsModels(Title,Description,CompanyName,ShareHolding,AGMID,RegCode,ProxyVoteResult,TotalRecordCount,Year,DateCreated,ArchiveStatus,AgmStart,AgmEnd,allChannels,mobileChannel,webChannel,smsChannel,proxyChannel,ussdChannel,AbstainBtnChoice,MessagingChoice,StopAdmittance,StartAdmittance,StopVoting,StartVoting,CountDownValue) VALUES('" + companyinfo + " Annual General Meeting','" + companyinfo + " Annual General Meeting','" + companyinfo + "', '" + 0 + "','" + newAGMID + "','" + newRegCode + "','" + 0 + "','" + ShareholderCount + "','" + DateTime.Now.Year.ToString() + "', '" + DateTime.Now + "', '" + 0 + "','" + 0 + "','" + 0 + "','" + 1 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 1 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 0 + "','" + 30 + "')";
//                             SqlCommand cmd2 = new SqlCommand(insertQuery, connn);
//                             cmd2.CommandTimeout = 180;
//                             cmd2.ExecuteNonQuery();
//                             connn.Close();

//                         }

//                     }

//                     Double TotalShareholding = 0;

//                     //Calculate Percentage Holding for all shareholder entry...
//                     SettingsModel setting;
//                     var CheckBarcodeModelStatus = db.Settings.Any();
//                     if (CheckBarcodeModelStatus)
//                     {
//                         string query2 = "UPDATE SettingsModels SET ShareHolding = (SELECT ROUND(SUM(Holding),2) FROM BarcodeModels Where Company ='" + companyinfo + "') Where CompanyName = '" + companyinfo + "'";
//                         connn.Open();
//                         SqlCommand cmd2 = new SqlCommand(query2, connn);
//                         cmd2.CommandTimeout = 180;
//                         cmd2.ExecuteNonQuery();
//                         connn.Close();
//                         setting = db.Settings.FirstOrDefault(s => s.AGMID == newAGMID);

//                         TotalShareholding = setting != null ? setting.ShareHolding : 0;
//                         //return System.Threading.Tasks.Task.FromResult<bool>(true);
//                     }

//                     //calculate Percentage Holding for a company
//                     if (TotalShareholding != 0)
//                     {

//                         //con1 = new SqlConnection(conn);
//                         string query3 = "UPDATE BarcodeModels SET PercentageHolding = (Holding /'" + TotalShareholding + "') * 100 Where Company='" + companyinfo + "'";
//                         connn.Open();
//                         SqlCommand cmd3 = new SqlCommand(query3, connn);
//                         cmd3.CommandTimeout = 180;
//                         cmd3.ExecuteNonQuery();
//                         connn.Close();
//                     }

//                     //Generate accesscodes
//                     var shareholders = db.BarcodeStore.Where(s => s.Company.ToLower() == companyinfo.Trim().ToLower() && s.emailAddress != null && s.accesscode == null).ToArray();
//                     for (int i = 0; i < shareholders.Length; i++)
//                     {
//                         var accesscode = ua.GetAccessCode();
//                         var requesturi = Utilities.GenerateAGMUrl(companyinfo, newAGMID, shareholders[i].emailAddress);
//                         //dbManager.UpdateBacodeModelWithAccessCode(shareholders[i].Id, accesscode, requesturi);
//                         shareholders[i].accesscode = accesscode.Insert(0, "S");
//                         shareholders[i].OnlineEventUrl = requesturi;
//                     }
//                     db.SaveChangesAsync();
//                     //StopTimer();
//                     Functions.RetrieveProgress(String.Format("Task completed @{0}", DateTime.Now.ToString("h:mm:ss")));
//                     return Task.FromResult<string>("Success");
//                     //return String.Format("Task completed @{0}", DateTime.Now.ToString("h:mm:ss"));
//                     //}
//                     ////StopTimer();
//                     //Functions.RetrieveProgress(String.Format("File upload @{0}", "Exist"));
//                     //return Task.FromResult<string>("Full");
//                     ////return "Full";


//                 }
//                 //StopTimer();
//                 Functions.RetrieveProgress(String.Format("File upload is {0}", "Empty"));
//                 return Task.FromResult<string>("Empty");
//                 //return "Empty";

//             }
//             catch (Exception e)
//             {
//                 Log.Error(e.ToString());
//                 //StopTimer();
//                 Functions.RetrieveProgress(String.Format("File upload Error: {0}", "Undoing Data Persistence"));
//                 string insertQuery = "DELETE FROM BarcodeModels WHERE Company = '" + Company + "'";
//                 connn.Close();
//                 connn.Open();
//                 SqlCommand cmd2 = new SqlCommand(insertQuery, connn);
//                 cmd2.CommandTimeout = 180;
//                 cmd2.ExecuteNonQuery();
//                 connn.Close();
//                 Functions.RetrieveProgress(String.Format("File upload Error: {0}", "Something Went Wrong." + e));
//                 return Task.FromResult<string>("Something Went Wrong." + e.Message);
//                 //return "Something Went Wrong." + e;
//             }

//         }

//         public string ManualGenerateAccessCode(string company)
//         {
//             Functions.RetrieveProgress("Generating AccessCode");
//             try
//             {
//                 var shareholders = db.BarcodeStore.Where(s => s.Company.ToLower() == company.Trim().ToLower() && s.emailAddress != null && s.accesscode == null).ToList();
//                 //var shareholders = dbManager.GetBacodeModelWithOutAccessCode(company);
//                 if (shareholders.Any())
//                 {
//                     var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
//                     if (UniqueAGMId != -1)
//                     {
//                         for (int i = 0; i < shareholders.Count; i++)
//                         {
//                             var accesscode = ua.GetAccessCode();
//                             var requesturi = Utilities.GenerateAGMUrl(company, UniqueAGMId, shareholders[i].emailAddress);
//                             //dbManager.UpdateBacodeModelWithAccessCode(shareholders[i].Id, accesscode, requesturi);
//                             shareholders[i].accesscode = accesscode.Insert(0, "S");
//                             shareholders[i].OnlineEventUrl = requesturi;
//                         }
//                         db.SaveChangesAsync();
//                         Functions.RetrieveProgress("Accesscode generation complete");
//                         return "success";
//                     }

//                 }
//                 return "No account qualify";
//             }
//             catch (Exception e)
//             {
//                 return "failed";
//             }

//         }







//         public string GenerateFacilitatorsAccessCode(string company)
//         {
//             Functions.RetrieveProgress("Generating AccessCode");
//             try
//             {
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
//                 if (UniqueAGMId != -1)
//                 {
//                     var facilitators = db.Facilitators.Where(s => s.AGMID == UniqueAGMId && s.emailAddress != null && s.accesscode == null).ToList();
//                     if (facilitators.Any())
//                     {
//                         for (int i = 0; i < facilitators.Count; i++)
//                         {
//                             var accesscode = ua.GetAccessCode();
//                             var requesturi = GenerateFacilitatorsAGMUrl(UniqueAGMId, facilitators[i].emailAddress);
//                             facilitators[i].accesscode = accesscode.Insert(0, "N");
//                             facilitators[i].OnlineEventUrl = requesturi;
//                         }
//                         db.SaveChangesAsync();
//                         Functions.RetrieveProgress("Accesscode generation complete");
//                         return "success";

//                     }
//                     return "No account qualify";
//                 }
//                 return "System couldnot retrieve AGMID.";
//             }
//             catch (Exception e)
//             {
//                 return "failed";
//             }

//         }



//         public string GenerateFacilitatorsAGMUrl(int AGMID, string emailAddress)
//         {
//             string request = "";
//             var requestUri = $"{Convert.ToString(ConfigurationManager.AppSettings["AGMBaseAddress"])}/Accreditation/ExternalAdmission/";
//             if (!string.IsNullOrEmpty(requestUri) || !string.IsNullOrWhiteSpace(requestUri))
//             {
//                 var query = string.Format("{0}|{1}", emailAddress, AGMID);
//                 var encryptedtext = query.Encrypt();
//                 request = $"{requestUri}?query={encryptedtext}";
//                 return request;

//             }
//             return request;
//         }
//         //public ActionResult ExportToExcel(int id)
//         //{
//         //    var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == id);
//         //    if (agmEventSetting != null)
//         //    {
//         //        SqlCommand command = new SqlCommand();
//         //        command.CommandText = "SELECT Name,shareholderNum,Company,Holding,emailAddress,PhoneNumber,accesscode FROM BarcodeModels Where Company='" + agmEventSetting.CompanyName +"' AND accesscode!=''";
//         //        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command.CommandText, connn))
//         //        {
//         //            DataTable t = new DataTable();
//         //            dataAdapter.Fill(t);
//         //            Workbook book = new Workbook();
//         //            Worksheet sheet = book.Worksheets[0];
//         //            sheet.InsertDataTable(t, true, 1, 1);
//         //            string fileLocation = "";
//         //            var fileName = ConfigurationManager.AppSettings["AccessExportFileName"];
//         //            fileLocation = Server.MapPath("~/Downloads/Accesscode/") + fileName;
//         //            byte[] fileBytes;
//         //            if (!System.IO.File.Exists(fileLocation))
//         //            {
//         //                book.SaveToFile(fileLocation);
//         //              fileBytes = System.IO.File.ReadAllBytes(fileLocation);
//         //                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
//         //            }

//         //            System.IO.File.Delete(fileLocation);
//         //            book.SaveToFile(fileLocation);
//         //            fileBytes = System.IO.File.ReadAllBytes(fileLocation);
//         //            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

//         //        }

//         //    }
//         //    //SqlConnection connection = new SqlConnection();
//         //    //connection.ConnectionString = @"Data Source=server;Initial Catalog=db;User ID=test;Password=test;";
//         //    return new EmptyResult();

//         //}



//         public void ExportToExcel(int id)
//         {
//             var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == id);
//             if (agmEventSetting != null)
//             {
//                 string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                 using (SqlConnection con = new SqlConnection(constr))
//                 {
//                     var fileName = ConfigurationManager.AppSettings["AccessExportFileName"] + DateTime.Now.ToString() + ".xlsx";
//                     using (SqlCommand cmd = new SqlCommand("SELECT Name,shareholderNum,Company,Holding,emailAddress,PhoneNumber,accesscode,OnlineEventUrl FROM BarcodeModels Where Company='" + agmEventSetting.CompanyName + "' AND accesscode!=''"))
//                     {
//                         using (SqlDataAdapter sda = new SqlDataAdapter())
//                         {
//                             cmd.Connection = con;
//                             sda.SelectCommand = cmd;
//                             using (DataTable dt = new DataTable())
//                             {
//                                 sda.Fill(dt);
//                                 using (XLWorkbook wb = new XLWorkbook())
//                                 {
//                                     wb.Worksheets.Add(dt, "Shareholders");
//                                     Response.Clear();
//                                     Response.Buffer = true;
//                                     Response.Charset = "";
//                                     Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
//                                     Response.AddHeader("content-disposition", "attachment;filename=" + fileName + "");
//                                     using (MemoryStream MyMemoryStream = new MemoryStream())
//                                     {
//                                         wb.SaveAs(MyMemoryStream);
//                                         MyMemoryStream.WriteTo(Response.OutputStream);
//                                         Response.Flush();
//                                         Response.End();
//                                     }
//                                 }
//                             }
//                         }
//                     }
//                 }
//             }
//         }


//         public void ExportFacilitatorsToExcel(int id)
//         {
//             var agmEventSetting = db.Settings.SingleOrDefault(s => s.AGMID == id);
//             if (agmEventSetting != null)
//             {
//                 string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                 using (SqlConnection con = new SqlConnection(constr))
//                 {
//                     var fileName = ConfigurationManager.AppSettings["NONShareholderExportFileName"] + DateTime.Now.ToString() + ".xlsx";
//                     using (SqlCommand cmd = new SqlCommand("SELECT Company,Name,FacilitatorCompany,ResourceType,emailAddress,PhoneNumber,accesscode,OnlineEventUrl FROM Facilitators Where AGMID ='" + agmEventSetting.AGMID + "' AND accesscode!=''"))
//                     {
//                         using (SqlDataAdapter sda = new SqlDataAdapter())
//                         {
//                             cmd.Connection = con;
//                             sda.SelectCommand = cmd;
//                             using (DataTable dt = new DataTable())
//                             {
//                                 sda.Fill(dt);
//                                 using (XLWorkbook wb = new XLWorkbook())
//                                 {
//                                     wb.Worksheets.Add(dt, "NONShareholders");
//                                     Response.Clear();
//                                     Response.Buffer = true;
//                                     Response.Charset = "";
//                                     Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
//                                     Response.AddHeader("content-disposition", "attachment;filename=" + fileName + "");
//                                     using (MemoryStream MyMemoryStream = new MemoryStream())
//                                     {
//                                         wb.SaveAs(MyMemoryStream);
//                                         MyMemoryStream.WriteTo(Response.OutputStream);
//                                         Response.Flush();
//                                         Response.End();
//                                     }
//                                 }
//                             }
//                         }
//                     }
//                 }
//             }
//         }
//         //public async Task<string> DirectFileUpload2()
//         //{

//         //    var response = await DirectFileUploadAsync();

//         //    return response;
//         //}


//         //public Task<string> DirectFileUploadAsync()
//         //{
//         //    StartTimer();
//         //    string excelConnectionString = string.Empty;
//         //    try
//         //    {

//         //        var CheckIfTableExist = db.BarcodeStore;
//         //        string companyinfo;

//         //        if (!(CheckIfTableExist.Count() > 0))
//         //        {
//         //            var fileName = ConfigurationManager.AppSettings["FileName"];

//         //            string fileExtension = System.IO.Path.GetExtension(fileName);
//         //            string fileLocation = string.Concat(Server.MapPath("~/Uploads/" + fileName));

//         //            if (fileExtension == ".xls" || fileExtension == ".xlsx")
//         //            {

//         //                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//         //                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
//         //                //connection String for xls file format.
//         //                if (fileExtension == ".xls")
//         //                {
//         //                    excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
//         //                    fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
//         //                }
//         //                //connection String for xlsx file format.
//         //                else if (fileExtension == ".xlsx")
//         //                {
//         //                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//         //                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
//         //                }
//         //            }

//         //            if (System.IO.File.Exists(fileLocation))
//         //            {
//         //                Functions.RetrieveProgress("Retrieving data from excel...");

//         //                using (OleDbConnection con = new OleDbConnection(excelConnectionString))
//         //                {
//         //                    //Create Connection to Excel work book and add oledb namespace
//         //                    DataTable dtExcel = new DataTable();
//         //                    DataColumn pHolding = new DataColumn("PercentageHolding", typeof(long));
//         //                    pHolding.DefaultValue = 0;
//         //                    dtExcel.Columns.Add(pHolding);
//         //                    DataColumn Present = new DataColumn("Present", typeof(bool));
//         //                    Present.DefaultValue = 0;
//         //                    dtExcel.Columns.Add(Present);
//         //                    DataColumn PresentByProxy = new DataColumn("PresentByProxy", typeof(bool));
//         //                    PresentByProxy.DefaultValue = 0;
//         //                    dtExcel.Columns.Add(PresentByProxy);
//         //                    DataColumn split = new DataColumn("split", typeof(bool));
//         //                    split.DefaultValue = 0;
//         //                    dtExcel.Columns.Add(split);
//         //                    DataColumn resolution = new DataColumn("resolution", typeof(bool));
//         //                    resolution.DefaultValue = 0;
//         //                    dtExcel.Columns.Add(resolution);
//         //                    DataColumn combined = new DataColumn("combined", typeof(bool));
//         //                    combined.DefaultValue = 0;
//         //                    dtExcel.Columns.Add(combined);
//         //                    DataColumn TakePoll = new DataColumn("TakePoll", typeof(bool));
//         //                    TakePoll.DefaultValue = 0;
//         //                    dtExcel.Columns.Add(TakePoll);
//         //                    DataColumn ParentAccountNumber = new DataColumn("ParentAccountNumber", typeof(bool));
//         //                    ParentAccountNumber.DefaultValue = 0;
//         //                    dtExcel.Columns.Add(ParentAccountNumber);
//         //                    DataColumn Selected = new DataColumn("Selected", typeof(bool));
//         //                    Selected.DefaultValue = 0;
//         //                    dtExcel.Columns.Add(Selected);
//         //                    DataColumn Consolidated = new DataColumn("Consolidated", typeof(bool));
//         //                    Consolidated.DefaultValue = 0;
//         //                    dtExcel.Columns.Add(Consolidated);
//         //                    DataColumn NotVerifiable = new DataColumn("NotVerifiable", typeof(bool));
//         //                    NotVerifiable.DefaultValue = 0;
//         //                    dtExcel.Columns.Add(NotVerifiable);
//         //                    DataColumn AddedSplitAccount = new DataColumn("AddedSplitAccount", typeof(bool));
//         //                    AddedSplitAccount.DefaultValue = 0;
//         //                    dtExcel.Columns.Add(AddedSplitAccount);


//         //                    string query = "Select * from [Sheet1$]";

//         //                    OleDbCommand cmd = new OleDbCommand(query, con);

//         //                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

//         //                    // add handlers
//         //                    //adapter.RowUpdating += new OleDbRowUpdatingEventHandler(OnRowUpdating);
//         //                    adapter.Fill(dtExcel);



//         //                    con.Close();

//         //                    companyinfo = dtExcel.Rows[0][1].ToString();

//         //                    string connstr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//         //                    // Bulk Copy to SQL Server   
//         //                    SqlBulkCopy bulkInsert = new SqlBulkCopy(connstr, SqlBulkCopyOptions.Default & SqlBulkCopyOptions.KeepIdentity);

//         //                    bulkInsert.DestinationTableName = "BarcodeModels";

//         //                    // Set up the event handler to notify after 50 rows.
//         //                    bulkInsert.SqlRowsCopied +=
//         //                        new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
//         //                    bulkInsert.NotifyAfter = 50;

//         //                    SqlBulkCopyColumnMapping SN = new SqlBulkCopyColumnMapping("SN", "SN");
//         //                    bulkInsert.ColumnMappings.Add(SN);

//         //                    SqlBulkCopyColumnMapping Company = new SqlBulkCopyColumnMapping("Company", "Company");
//         //                    bulkInsert.ColumnMappings.Add(Company);

//         //                    SqlBulkCopyColumnMapping ShareholderNumber = new SqlBulkCopyColumnMapping("ShareholderNum", "ShareholderNum");
//         //                    bulkInsert.ColumnMappings.Add(ShareholderNumber);

//         //                    SqlBulkCopyColumnMapping Name = new SqlBulkCopyColumnMapping("Name", "Name");
//         //                    bulkInsert.ColumnMappings.Add(Name);

//         //                    SqlBulkCopyColumnMapping Address = new SqlBulkCopyColumnMapping("Address", "Address");
//         //                    bulkInsert.ColumnMappings.Add(Address);

//         //                    SqlBulkCopyColumnMapping Holding = new SqlBulkCopyColumnMapping("Holding", "Holding");
//         //                    bulkInsert.ColumnMappings.Add(Holding);

//         //                    SqlBulkCopyColumnMapping EmailAddress = new SqlBulkCopyColumnMapping("emailAddress", "emailAddress");
//         //                    bulkInsert.ColumnMappings.Add(EmailAddress);

//         //                    SqlBulkCopyColumnMapping PercentageHolding = new SqlBulkCopyColumnMapping("PercentageHolding", "PercentageHolding");
//         //                    bulkInsert.ColumnMappings.Add(PercentageHolding);

//         //                    SqlBulkCopyColumnMapping Prsent = new SqlBulkCopyColumnMapping("Present", "Present");
//         //                    bulkInsert.ColumnMappings.Add(Prsent);
//         //                    SqlBulkCopyColumnMapping PbyProxy = new SqlBulkCopyColumnMapping("PresentByProxy", "PresentByProxy");
//         //                    bulkInsert.ColumnMappings.Add(PbyProxy);
//         //                    SqlBulkCopyColumnMapping splitt = new SqlBulkCopyColumnMapping("split", "split");
//         //                    bulkInsert.ColumnMappings.Add(splitt);
//         //                    SqlBulkCopyColumnMapping Resolution = new SqlBulkCopyColumnMapping("resolution", "resolution");
//         //                    bulkInsert.ColumnMappings.Add(Resolution);
//         //                    SqlBulkCopyColumnMapping Combined = new SqlBulkCopyColumnMapping("combined", "combined");
//         //                    bulkInsert.ColumnMappings.Add(Combined);
//         //                    SqlBulkCopyColumnMapping Takepoll = new SqlBulkCopyColumnMapping("TakePoll", "TakePoll");
//         //                    bulkInsert.ColumnMappings.Add(Takepoll);
//         //                    SqlBulkCopyColumnMapping ParentAccount = new SqlBulkCopyColumnMapping("ParentAccountNumber", "ParentAccountNumber");
//         //                    bulkInsert.ColumnMappings.Add(ParentAccount);
//         //                    SqlBulkCopyColumnMapping selected = new SqlBulkCopyColumnMapping("Selected", "Selected");
//         //                    bulkInsert.ColumnMappings.Add(selected);
//         //                    SqlBulkCopyColumnMapping consolidated = new SqlBulkCopyColumnMapping("Consolidated", "Consolidated");
//         //                    bulkInsert.ColumnMappings.Add(consolidated);
//         //                    SqlBulkCopyColumnMapping NotVerify = new SqlBulkCopyColumnMapping("NotVerifiable", "NotVerifiable");
//         //                    bulkInsert.ColumnMappings.Add(NotVerify);
//         //                    SqlBulkCopyColumnMapping AddedSplitAcc = new SqlBulkCopyColumnMapping("AddedSplitAccount", "AddedSplitAccount");
//         //                    bulkInsert.ColumnMappings.Add(AddedSplitAcc);

//         //                    bulkInsert.BulkCopyTimeout = 0;
//         //                    bulkInsert.BatchSize = 5000;

//         //                    bulkInsert.WriteToServer(dtExcel);


//         //                }

//         //                Functions.RetrieveProgress("Final Stage - Completing...");

//         //                if (!db.BarcodeStore.Select(o => o.Company).Distinct().Contains(companyinfo.Trim()))
//         //                {
//         //                    db.Settings.Add(
//         //                        new SettingsModel() { Title = "Annual General Meeting", CompanyName = companyinfo.Trim() }
//         //                        );
//         //                    db.SaveChanges();
//         //                }


//         //                Double TotalShareholding = 0;

//         //                string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

//         //                SqlConnection conn =
//         //                        new SqlConnection(connStr);

//         //                SettingsModel setting;
//         //                var CheckBarcodeModelStatus = db.BarcodeStore.Any();
//         //                if (CheckBarcodeModelStatus)
//         //                {
//         //                    string query2 = "UPDATE SettingsModels SET ShareHolding = (SELECT ROUND(SUM(Holding),2) FROM BarcodeModels Where Company ='" + companyinfo + "')";
//         //                    conn.Open();
//         //                    SqlCommand cmd2 = new SqlCommand(query2, conn);
//         //                    cmd2.ExecuteNonQuery();
//         //                    conn.Close();
//         //                    setting = db.Settings.FirstOrDefault();
//         //                    TotalShareholding = setting.ShareHolding;
//         //                    //return System.Threading.Tasks.Task.FromResult<bool>(true);
//         //                }
//         //                else
//         //                {
//         //                    setting = db.Settings.FirstOrDefault();
//         //                    setting.ShareHolding = 0;
//         //                    db.SaveChanges();

//         //                }

//         //                if (TotalShareholding == 0)
//         //                {
//         //                    StopTimer();
//         //                    return System.Threading.Tasks.Task.FromResult<string>("Please value for Total Holding");
//         //                }

//         //                //con1 = new SqlConnection(conn);
//         //                string query3 = "UPDATE BarcodeModels SET PercentageHolding = (Holding /'" + TotalShareholding + "') * 100 Where Company='" + companyinfo + "'";
//         //                conn.Open();
//         //                SqlCommand cmd3 = new SqlCommand(query3, conn);
//         //                cmd3.ExecuteNonQuery();
//         //                conn.Close();

//         //                StopTimer();
//         //                Functions.RetrieveProgress(String.Format("Task completed @{0}", DateTime.Now.ToString("h:mm:ss")));
//         //                return Task.FromResult<string>(String.Format("Task completed @{0}", DateTime.Now.ToString("h:mm:ss")));
//         //            }
//         //            StopTimer();
//         //            Functions.RetrieveProgress(String.Format("File upload is @{0}", "Empty"));
//         //            return System.Threading.Tasks.Task.FromResult<string>("Empty");
//         //        }
//         //        StopTimer();
//         //        Functions.RetrieveProgress(String.Format("File upload @{0}", "Exist"));
//         //        return System.Threading.Tasks.Task.FromResult<string>("Full");
//         //    }
//         //    catch (OleDbException e)
//         //    {
//         //        StopTimer();
//         //        Functions.RetrieveProgress(String.Format("File upload Error: @{0}", "Something Went Wrong." + e));
//         //        return System.Threading.Tasks.Task.FromResult<string>("Something Went Wrong." + e);

//         //    }

//         //}


//         private void OnSqlRowsCopied(
//     object sender, SqlRowsCopiedEventArgs e)
//         {
//             var v = e.RowsCopied / FileUploadCount;
//             var percentageCount = v * 100;

//             //TimeSpan time = UploadTimer.GetTime();
//             Functions.UploadProgress(String.Format("Persisting Data {0}", percentageCount));

//         }

//         private void oleDbDataAdapter1_RowUpdating(object sender, OleDbRowUpdatingEventArgs e)
//         {
//             // Inserting  
//             if (e.StatementType == StatementType.Insert)
//             {
//                 Functions.UploadProgress("Inserting");
//             }
//             // Updating  
//             else if (e.StatementType == StatementType.Update)
//             {
//                 //                TimeSpan time1 = UploadTimer.GetTime();
//                 //Functions.RetrieveProgress(String.Format("Retrieving Data... Time - {0}:{1}:{2}", time1.Hours, time1.Minutes, time1.Seconds));
//                 Functions.UploadProgress("Updating");
//             }
//             // Deleting  
//             else if (e.StatementType == StatementType.Delete)
//             {
//                 Functions.UploadProgress("Deleting");
//             }
//             //Selecting  
//             else if (e.StatementType == StatementType.Select)
//             {
//                 Functions.UploadProgress("Selecting");
//             }
//         }


//         [HttpPost]
//         public async Task<string> FacilitatorsFileUpload()
//         {
//             string fileLocation = "";
//             string fileExtension = "";

//             if (Request.Files["file"].ContentLength > 0)
//             {
//                 fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

//                 if (fileExtension == ".xls" || fileExtension == ".xlsx")
//                 {
//                     fileLocation = Server.MapPath("~/Uploads/FacilitatorsListUpload/") + Request.Files["file"].FileName;
//                     if (System.IO.File.Exists(fileLocation))
//                     {

//                         System.IO.File.Delete(fileLocation);
//                     }
//                     Request.Files["file"].SaveAs(fileLocation);

//                 }
//             }
//             var response = await FacilitatorsFileUploadAsync(fileLocation, fileExtension);

//             return response;
//         }


//         public Task<string> FacilitatorsFileUploadAsync(string fileLocation, string fileExtension)
//         {

//             string excelConnectionString = string.Empty;
//             SqlConnection con1;
//             try
//             {
//                 Functions.RetrieveProgress("Intializing...");

//                 if (fileExtension != "" && fileLocation != "")
//                 {

//                     excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//                     fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
//                     //connection String for xls file format.
//                     if (fileExtension == ".xls")
//                     {
//                         excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
//                         fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
//                     }
//                     //connection String for xlsx file format.
//                     else if (fileExtension == ".xlsx")
//                     {
//                         excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//                         fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
//                     }
//                     //Create Connection to Excel work book and add oledb namespace
//                     DataTable dtExcel = new DataTable();
//                     //string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + fileName + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";
//                     OleDbConnection con = new OleDbConnection(excelConnectionString);
//                     string query = "Select * from [Sheet1$]";
//                     OleDbDataAdapter data = new OleDbDataAdapter(query, con);
//                     data.Fill(dtExcel);
//                     string agmid = dtExcel.Rows[0][1].ToString();
//                     int AGMID;
//                     if (!int.TryParse(agmid, out AGMID))
//                     {
//                         return System.Threading.Tasks.Task.FromResult<string>("AGMID value must be integer.");
//                     }


//                     //var companies = db.Facilitators.Select(o => o.Company).Distinct().OrderBy(k => k).ToList();
//                     //if (!companies.Contains(companyinfo))
//                     //{
//                     //var AGMID = ua.RetrieveAGMUniqueID(companyinfo);
//                     if (AGMID != -1)
//                     {
//                         string companyinfo;
//                         var settings = db.Settings.SingleOrDefault(s => s.AGMID == AGMID);
//                         if (settings != null)
//                         {
//                             companyinfo = settings.CompanyName;
//                         }
//                         else
//                         {
//                             return System.Threading.Tasks.Task.FromResult<string>("System Cannot retrieve Company Name with AGMID.");
//                         }
//                         for (int i = 0; i < dtExcel.Rows.Count; i++)
//                         {
//                             string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//                             con1 = new SqlConnection(conn);
//                             string query2 = "Insert into Facilitators(SN,AGMID,Name,FacilitatorCompany,emailAddress,PhoneNumber,ResourceType,Company) Values('" +
//                             dtExcel.Rows[i][0].ToString() + "','" + dtExcel.Rows[i][1].ToString() +
//                             "','" + dtExcel.Rows[i][2].ToString() + "','" + dtExcel.Rows[i][3].ToString() + "','" + dtExcel.Rows[i][4].ToString() + "','" + dtExcel.Rows[i][5].ToString() +
//                             "','" + dtExcel.Rows[i][6].ToString() + "','" + companyinfo + "')";
//                             con1.Open();
//                             SqlCommand cmd = new SqlCommand(query2, con1);
//                             cmd.ExecuteNonQuery();
//                             con1.Close();

//                             //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
//                             Functions.SendProgress("Process in progress...", i, dtExcel.Rows.Count);
//                         }
//                         return System.Threading.Tasks.Task.FromResult<string>("Success");
//                     }
//                     else
//                     {
//                         return System.Threading.Tasks.Task.FromResult<string>("Failed to retrieve AGMID, Check AGM Company Name");
//                     }
//                     //}
//                     //else
//                     //{
//                     //    return System.Threading.Tasks.Task.FromResult<string>("Clear Existing AGM Information for this company to upload new.");
//                     //}
//                     //Functions.RetrieveProgress(String.Format("Task completed @{0}", DateTime.Now.ToString("h:mm:ss")));
//                     //return System.Threading.Tasks.Task.FromResult<string>("Success");
//                     //return "Upload Completed Successfully!";
//                 }
//                 HttpResponseMessage response2 = new HttpResponseMessage();
//                 response2.StatusCode = HttpStatusCode.NoContent;
//                 TempData["Message1"] = "Upload File is Empty!";
//                 return System.Threading.Tasks.Task.FromResult<string>("Empty");
//                 //return "Upload File is Empty!";

//             }
//             catch (OleDbException e)
//             {
//                 HttpResponseMessage response = new HttpResponseMessage();
//                 response.StatusCode = HttpStatusCode.NotAcceptable;
//                 TempData["Message1"] = "Something Went Wrong." + e;
//                 return System.Threading.Tasks.Task.FromResult<string>("Something Went Wrong." + e.Message);
//                 //return "Something Went Wrong." + e; 
//             }

//         }


//         [HttpPost]
//         public async Task<string> ProxyFileUpload()
//         {
//             string fileLocation = "";
//             string fileExtension = "";

//             if (Request.Files["file"].ContentLength > 0)
//             {
//                 fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

//                 if (fileExtension == ".xls" || fileExtension == ".xlsx")
//                 {
//                     fileLocation = Server.MapPath("~/Uploads/ProxyListUpload/") + Request.Files["file"].FileName;
//                     if (System.IO.File.Exists(fileLocation))
//                     {

//                         System.IO.File.Delete(fileLocation);
//                     }
//                     Request.Files["file"].SaveAs(fileLocation);

//                 }
//             }
//             var response = await ProxyFileUploadAsync(fileLocation, fileExtension);

//             return response;
//         }


//         public Task<string> ProxyFileUploadAsync(string fileLocation, string fileExtension)
//         {
//             //ResetTimer();
//             //StartTimer();
//             //fileName = FileUpload1.ResolveClientUrl(FileUpload1.PostedFile.FileName);
//             string excelConnectionString = string.Empty;
//             string companyinfo = string.Empty;

//             SqlConnection con1;
//             try
//             {

//                 string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

//                 if (fileLocation != "" && fileExtension != "")
//                 {

//                     excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//                     fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
//                     //connection String for xls file format.
//                     if (fileExtension == ".xls")
//                     {
//                         excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
//                         fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
//                     }
//                     //connection String for xlsx file format.
//                     else if (fileExtension == ".xlsx")
//                     {
//                         excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
//                         fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
//                     }
//                     //Create Connection to Excel work book and add oledb namespace
//                     using (OleDbConnection con = new OleDbConnection(excelConnectionString))
//                     {
//                         DataTable dtExcel = new DataTable();
//                         DataColumn validity = new DataColumn("Validity", typeof(bool));
//                         validity.DefaultValue = 0;
//                         dtExcel.Columns.Add(validity);
//                         //string query = "Select * from [Sheet1$]";
//                         //OleDbDataAdapter data = new OleDbDataAdapter(query, con);
//                         OleDbCommand cmd = new OleDbCommand("select * from [Sheet1$]", con);
//                         OleDbDataAdapter data = new OleDbDataAdapter(cmd);
//                         data.Fill(dtExcel);

//                         companyinfo = dtExcel.Rows[0][1].ToString();

//                         var companies = db.ProxyList.Select(o => o.Company).Distinct().OrderBy(k => k).ToList();
//                         if (!companies.Contains(companyinfo))
//                         {
//                             // Bulk Copy to SQL Server   
//                             SqlBulkCopy bulkInsert = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default & SqlBulkCopyOptions.KeepIdentity);

//                             bulkInsert.DestinationTableName = "Proxylists";

//                             SqlBulkCopyColumnMapping company = new SqlBulkCopyColumnMapping("Company", "Company");
//                             bulkInsert.ColumnMappings.Add(company);

//                             SqlBulkCopyColumnMapping ShareholderNumber = new SqlBulkCopyColumnMapping("ShareholderNum", "ShareholderNum");
//                             bulkInsert.ColumnMappings.Add(ShareholderNumber);

//                             SqlBulkCopyColumnMapping Validity = new SqlBulkCopyColumnMapping("Validity", "Validity");
//                             bulkInsert.ColumnMappings.Add(Validity);
//                             bulkInsert.BulkCopyTimeout = 0;
//                             bulkInsert.BatchSize = 5000;

//                             bulkInsert.WriteToServer(dtExcel);
//                             bulkInsert.Close();
//                             con.Close();
//                         }
//                         else
//                         {
//                             //StopTimer();
//                             Functions.RetrieveProgress(String.Format("This file has been upload already"));
//                             return System.Threading.Tasks.Task.FromResult<string>("Full");
//                         }
//                     }

//                     //con1 = new SqlConnection(conn);

//                     con1 = new SqlConnection(conn);
//                     con1.Open();
//                     var proxylist = db.ProxyList.Where(p => p.Company == companyinfo && p.Validity == false).ToArray();
//                     int proxylistCount = proxylist.Length;
//                     var AGMID = ua.RetrieveAGMUniqueID(companyinfo);
//                     for (int i = 0; i < proxylistCount; i++)
//                     {
//                         try
//                         {
//                             var shareholdernum = proxylist[i].ShareholderNum;
//                             //if (!db.Present.Any(p=>p.AGMID==AGMID && p.ShareholderNum== shareholdernum))
//                             //{
//                             var query1 = "SELECT * FROM BarcodeModels WHERE Company = '" + companyinfo + "' AND ShareholderNum  = '" + shareholdernum + "'";

//                             SqlCommand cmd1 = new SqlCommand(query1, con1);
//                             SqlDataReader read = cmd1.ExecuteReader();

//                             string phonenumber = "";
//                             string emailAddress = "";
//                             string address = "";
//                             PresentModel model = new PresentModel();
//                             while (read.Read())
//                             {
//                                 model.Name = (read["Name"].ToString());
//                                 model.Address = (read["Address"].ToString());
//                                 model.ShareholderNum = (Int64.Parse(read["ShareholderNum"].ToString()));
//                                 model.Holding = double.Parse(read["Holding"].ToString());
//                                 model.PercentageHolding = double.Parse(read["PercentageHolding"].ToString());
//                                 model.Company = (read["Company"].ToString());
//                                 if (!String.IsNullOrEmpty((read["PhoneNumber"].ToString())))
//                                 {
//                                     char[] arr = (read["PhoneNumber"].ToString()).Where(c => (char.IsLetterOrDigit(c))).ToArray();

//                                     var newPhoneNumber = new string(arr);
//                                     if (newPhoneNumber.StartsWith("234"))
//                                     {
//                                         phonenumber = newPhoneNumber;
//                                     }
//                                     else if (newPhoneNumber.StartsWith("0"))
//                                     {

//                                         double number = double.Parse(newPhoneNumber);
//                                         phonenumber = "234" + number.ToString();

//                                     }

//                                 }
//                                 if (!String.IsNullOrEmpty((read["emailAddress"].ToString())))
//                                 {
//                                     emailAddress = string.Format("proxy{0}", (read["emailAddress"].ToString()));
//                                 }
//                                 if (!String.IsNullOrEmpty((read["Address"].ToString())))
//                                 {
//                                     address = (read["Address"].ToString());
//                                 }
//                                 if (!String.IsNullOrEmpty((read["ParentAccountNumber"].ToString())))
//                                 {
//                                     model.ParentNumber = long.Parse(read["ParentAccountNumber"].ToString());
//                                 }
//                                 model.PresentTime = DateTime.Now;
//                                 model.Timestamp = DateTime.Now.TimeOfDay;
//                                 model.AGMID = AGMID;
//                                 var year = DateTime.Now.Year.ToString();
//                                 //db.Present.Add(model);
//                                 if (model.ShareholderNum != 0)
//                                 {
//                                     //Insert into Present Table
//                                     string query2 = @"IF NOT EXISTS(SELECT * FROM PresentModels WHERE Company = @companyinfo AND ShareholderNum  = @sholdernum AND Proxy=@proxy)" +
//                                         "BEGIN;" +
//                                         "Insert into PresentModels(Name,Address,Company,ShareholderNum,ParentNumber,Holding,PhoneNumber,emailAddress,PercentageHolding,Proxy,PresentTime,Timestamp,newNumber,TakePoll,split,present,GivenClikapad,ReturnedClikapad,AGMID,PermitPoll,Year,admitSource) Values(@Name,@Address,@Company,@ShareholderNum,@ParentNumber,@Holding,@phonenumber," +
//                                                     "@emailAddress,@PercentageHolding,@Proxy,@PresentTime,@Timestamp,@newNumber,@TakePoll,@split,@present,@GivenClikapad,@ReturnedClikapad,@AGMID,@PermitPoll,@Year,@admitSource);" +
//                                         "Update BarcodeModels SET PresentByProxy = 'TRUE', NotVerifiable = 'TRUE', TakePoll='TRUE',resolution='TRUE', Proxyupload ='1' WHERE Company = '" + companyinfo + "' AND ShareholderNum  = '" + shareholdernum + "';" +
//                                         "Update Proxylists SET Validity = 'TRUE' WHERE Company = '" + companyinfo + "' AND ShareholderNum  = '" + shareholdernum + "';" +

//                                         "END;";
//                                     //con1.Open();
//                                     SqlCommand cmd2 = new SqlCommand(query2, con1);
//                                     cmd2.Parameters.AddWithValue("@companyinfo", companyinfo);
//                                     cmd2.Parameters.AddWithValue("@sholdernum", shareholdernum);
//                                     cmd2.Parameters.AddWithValue("@Name", model.Name);
//                                     cmd2.Parameters.AddWithValue("@Address", address);
//                                     cmd2.Parameters.AddWithValue("@Company", model.Company);
//                                     cmd2.Parameters.AddWithValue("@ShareholderNum", model.ShareholderNum);
//                                     cmd2.Parameters.AddWithValue("@ParentNumber", model.ParentNumber);
//                                     cmd2.Parameters.AddWithValue("@Holding", model.Holding);
//                                     cmd2.Parameters.AddWithValue("@PhoneNumber", phonenumber);
//                                     cmd2.Parameters.AddWithValue("@emailAddress", emailAddress);
//                                     cmd2.Parameters.AddWithValue("@PercentageHolding", model.PercentageHolding);
//                                     cmd2.Parameters.AddWithValue("@Proxy", 1);
//                                     cmd2.Parameters.AddWithValue("@PresentTime", model.PresentTime);
//                                     cmd2.Parameters.AddWithValue("@Timestamp", model.Timestamp);
//                                     cmd2.Parameters.AddWithValue("@newNumber", 0);
//                                     cmd2.Parameters.AddWithValue("@TakePoll", 0);
//                                     cmd2.Parameters.AddWithValue("@split", 0);
//                                     cmd2.Parameters.AddWithValue("@present", 0);
//                                     cmd2.Parameters.AddWithValue("@GivenClikapad", 0);
//                                     cmd2.Parameters.AddWithValue("@ReturnedClikapad", 0);
//                                     cmd2.Parameters.AddWithValue("@AGMID", model.AGMID);
//                                     cmd2.Parameters.AddWithValue("@PermitPoll", 1);
//                                     cmd2.Parameters.AddWithValue("@Year", year);
//                                     cmd2.Parameters.AddWithValue("@admitSource", "Proxy");

//                                     cmd2.ExecuteNonQuery();
//                                     if (db.Question.Any())
//                                     {
//                                         var resolutions = db.Question.Where(q => q.AGMID == AGMID).ToArray();

//                                         for (int j = 0; j < resolutions.Length; j++)
//                                         {
//                                             var questionid = resolutions[j].Id;
//                                             var checkifVoteExist = db.Result.Any(r => r.QuestionId == questionid && r.ShareholderNum == model.ShareholderNum);
//                                             if (!checkifVoteExist)
//                                             {
//                                                 string query3 = "Insert into Results([Name],[EmailAddress],[Company],[Year],[Holding],[Address],[splitValue],[ParentNumber],[PercentageHolding],[phonenumber],[VoteStatus],[Source],[date],[Timestamp],[Present],[PresentByProxy],[QuestionId],[AGMID],[ShareholderNum],[VoteFor],[VoteAgainst],[VoteAbstain]) Values(" +
//                                                             "@Name,@emailAddress,@Company,@year,@Holding,@Address,@splitValue,@ParentNumber,@PercentageHolding,@PhoneNumber,@VoteStatus,@Source,@date,@Timestamp,@Present,@PresentByProxy,@QuestionId,@AGMID,@ShareholderNum,@VoteFor,@VoteAgainst,@VoteAbstain)";
//                                                 SqlCommand cmd3 = new SqlCommand(query3, con1);
//                                                 cmd3.Parameters.AddWithValue("@Name", model.Name);
//                                                 cmd3.Parameters.AddWithValue("@emailAddress", emailAddress);
//                                                 cmd3.Parameters.AddWithValue("@Company", model.Company);
//                                                 cmd3.Parameters.AddWithValue("@year", year);
//                                                 cmd3.Parameters.AddWithValue("@Holding", model.Holding);
//                                                 cmd3.Parameters.AddWithValue("@Address", address);
//                                                 cmd3.Parameters.AddWithValue("@splitValue", 0);
//                                                 cmd3.Parameters.AddWithValue("@ParentNumber", model.ParentNumber);
//                                                 cmd3.Parameters.AddWithValue("@PercentageHolding", model.PercentageHolding);
//                                                 cmd3.Parameters.AddWithValue("@PhoneNumber", phonenumber);
//                                                 cmd3.Parameters.AddWithValue("@VoteStatus", "Voted");
//                                                 cmd3.Parameters.AddWithValue("@Source", "Proxy");
//                                                 cmd3.Parameters.AddWithValue("@date", DateTime.Now);
//                                                 cmd3.Parameters.AddWithValue("@Timestamp", model.Timestamp);
//                                                 cmd3.Parameters.AddWithValue("@Present", 0);
//                                                 cmd3.Parameters.AddWithValue("@PresentByProxy", 1);
//                                                 cmd3.Parameters.AddWithValue("@QuestionId", questionid);
//                                                 cmd3.Parameters.AddWithValue("@AGMID", model.AGMID);
//                                                 cmd3.Parameters.AddWithValue("@ShareholderNum", model.ShareholderNum);
//                                                 cmd3.Parameters.AddWithValue("@VoteFor", 1);
//                                                 cmd3.Parameters.AddWithValue("@VoteAgainst", 0);
//                                                 cmd3.Parameters.AddWithValue("@VoteAbstain", 0);
//                                                 cmd3.ExecuteNonQuery();
//                                             }

//                                         }

//                                     }
//                                     string query4 = "UPDATE BarcodeModels SET emailAddress = '" + emailAddress + "' Where Company='" + companyinfo + "' AND ShareholderNum = '" + shareholdernum + "'";
//                                     SqlCommand cmd4 = new SqlCommand(query4, con1);
//                                     cmd4.CommandTimeout = 180;
//                                     cmd4.ExecuteNonQuery();

//                                 }
//                                 Functions.RetrieveProgress(String.Format("Processing {0} of {1}", i, proxylistCount));
//                             }
//                             read.Close();

//                         }
//                         catch (SqlException e)
//                         {
//                             Functions.RetrieveProgress(String.Format("Process Error {0}", e));
//                             return System.Threading.Tasks.Task.FromResult<string>("Something Went Wrong." + e);
//                         }


//                     }
//                     con1.Close();

//                     Functions.RetrieveProgress(String.Format("Task Completed @ {0}", DateTime.Now.ToString("h:mm:ss")));
//                     return System.Threading.Tasks.Task.FromResult<string>("Success");
//                 }
//                 //StopTimer();

//                 //return "No file was Uploaded";
//                 Functions.RetrieveProgress(String.Format("Empty File Upload"));
//                 return System.Threading.Tasks.Task.FromResult<string>("Empty");

//             }
//             catch (DbException e)
//             {
//                 //StopTimer();

//                 Functions.RetrieveProgress(String.Format("Process Error {0}", e.Message));
//                 return System.Threading.Tasks.Task.FromResult<string>("Something Went Wrong." + e.Message);

//             }

//         }


//         public async Task<string> UndoProxylistUpload(string company)
//         {
//             var response = await UndoProxylistUploadAsync(company);

//             return response;
//         }


//         private Task<string> UndoProxylistUploadAsync(string company)
//         {
//             try
//             {
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
//                 var proxylist = db.BarcodeStore.Where(b => b.Company == company && b.Proxyupload == "1").ToArray();
//                 for (int i = 0; i < proxylist.Length; i++)
//                 {
//                     var shareholdernum = proxylist[i].ShareholderNum;

//                     var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == shareholdernum).ToArray();
//                     if (result.Any())
//                     {
//                         db.Result.RemoveRange(result);

//                     }
//                     //for (int j = 0; j < result.Length; j++)
//                     //{

//                     //}
//                     var present = db.Present.Where(p => p.AGMID == UniqueAGMId && p.ShareholderNum == shareholdernum).ToArray();
//                     if (present.Any())
//                     {
//                         //for (int k = 0; k < present.Length; k++)
//                         //{
//                         db.Present.RemoveRange(present);

//                         //}

//                     }
//                     var proxylistitems = db.ProxyList.Where(p => p.Company == company);
//                     if (proxylist.Any())
//                     {

//                         db.ProxyList.RemoveRange(proxylistitems);

//                     }

//                     proxylist[i].PresentByProxy = false;
//                     proxylist[i].TakePoll = false;
//                     proxylist[i].NotVerifiable = false;
//                     proxylist[i].Selected = false;
//                     proxylist[i].resolution = false;
//                     proxylist[i].Proxyupload = "0";
//                     if (proxylist[i].emailAddress != null && proxylist[i].emailAddress.StartsWith("proxy"))
//                     {
//                         proxylist[i].emailAddress = proxylist[i].emailAddress.Substring(5);
//                     }
//                     //    }


//                     //}
//                     Functions.RetrieveProgress(String.Format("Processing {0} of {1}", i, proxylist.Length));
//                 }
//                 db.SaveChanges();
//                 return Task.FromResult<string>("success");
//             }
//             catch (Exception e)
//             {
//                 db.SaveChanges();
//                 return Task.FromResult<string>("Failed");
//             }
//         }

//         public async Task<string> ClearProxyList(string id)
//         {
//             var response = await ClearProxyListAsync(id);

//             return response;
//         }


//         private Task<string> ClearProxyListAsync(string company)
//         {

//             string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

//             SqlConnection conn =
//                     new SqlConnection(connStr);
//             try
//             {
//                 //string query4 = "TRUNCATE TABLE Proxylists";
//                 //    conn.Open();
//                 //    SqlCommand cmd4 = new SqlCommand(query4, conn);
//                 //cmd4.ExecuteNonQuery();
//                 //conn.Close();
//                 var companyinfo = company.Trim();
//                 string query4 = "DELETE FROM [dbo].[Proxylists] WHERE Company='" + companyinfo + "'";
//                 conn.Open();
//                 SqlCommand cmd4 = new SqlCommand(query4, conn);
//                 cmd4.ExecuteNonQuery();
//                 conn.Close();


//                 ViewBag.Message = "Proxy List Cleared";
//                 return System.Threading.Tasks.Task.FromResult<string>("Success");

//             }
//             catch (Exception e)
//             {
//                 ViewBag.Message1 = " Something Went Wrong, Could not clear List";
//                 return System.Threading.Tasks.Task.FromResult<string>("Something Went Wrong, Could not clear List" + " " + e);
//             }
//         }

//         //private static void ResetIIS()
//         //{
//         //    Process iisReset = new Process();
//         //    iisReset.StartInfo.FileName = @"C:\WINDOWS\system32\iisreset.exe";
//         //    iisReset.StartInfo.RedirectStandardOutput = true;
//         //    iisReset.StartInfo.UseShellExecute = false;
//         //    iisReset.Start();
//         //    iisReset.WaitForExit();
//         //}
//         //public ActionResult BulkUpload(HttpPostedFileBase file)
//         //{
//         //    if (Request.Files["file"].ContentLength > 0)
//         //    {

//         //        //FIRST, SAVE THE SELECTED FILE IN THE ROOT DIRECTORY.
//         //        string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
//         //        if (System.IO.File.Exists(fileLocation))
//         //        {

//         //            System.IO.File.Delete(fileLocation);
//         //        }
//         //        Request.Files["file"].SaveAs(fileLocation);

//         //        SqlBulkCopy oSqlBulk = null;

//         //        // SET A CONNECTION WITH THE EXCEL FILE.
//         //        string excelCS = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", fileLocation);

//         //        using (OleDbConnection con = new OleDbConnection(excelCS))
//         //        {
//         //            OleDbCommand cmd = new OleDbCommand("select * from [Sheet1$]", con);
//         //            con.Open();
//         //            // Create DbDataReader to Data Worksheet  
//         //            DbDataReader dr = cmd.ExecuteReader();
//         //            // SQL Server Connection String  
//         //            //string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
//         //            string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
//         //            // Bulk Copy to SQL Server   
//         //            SqlBulkCopy bulkInsert = new SqlBulkCopy(conn);
//         //            bulkInsert.DestinationTableName = "BarcodeModels";
//         //            bulkInsert.WriteToServer(dr);
//         //            //BindGridview();
//         //            //lblMessage.Text = "Your file uploaded successfully";
//         //            //lblMessage.ForeColor = System.Drawing.Color.Green;

//         //            HttpResponseMessage response = new HttpResponseMessage();
//         //            response.StatusCode = HttpStatusCode.OK;
//         //            TempData["Message"] = "Upload Completed Successfully!";
//         //            return RedirectToAction("Index", "BarcodeLib");
//         //            //lblConfirm.Text = "DATA IMPORTED SUCCESSFULLY.";
//         //            //lblConfirm.Attributes.Add("style", "color:green");
//         //        }

//         //        }
//         //        catch (OleDbException e)
//         //        {
//         //            HttpResponseMessage response = new HttpResponseMessage();
//         //            response.StatusCode = HttpStatusCode.NotAcceptable;
//         //            TempData["Message1"] = "Something Went Wrong." + e;
//         //            return RedirectToAction("Index", "BarcodeLib");

//         //            //lblConfirm.Text = ex.Message;
//         //            //lblConfirm.Attributes.Add("style", "color:red");

//         //        }
//         //        finally
//         //        {
//         //            // CLEAR.
//         //            oSqlBulk.Close();
//         //            oSqlBulk = null;
//         //            myExcelConn.Close();
//         //            myExcelConn = null;
//         //        }
//         //    }
//         //    HttpResponseMessage response2 = new HttpResponseMessage();
//         //    response2.StatusCode = HttpStatusCode.NoContent;
//         //    TempData["Message1"] = "Upload File is Empty!";
//         //    return RedirectToAction("Index", "BarcodeLib");
//         //}
//         //
//         // POST: /DataUpload/Create

//         public async Task<ActionResult> PullProxyList()
//         {
//             var response = await PullProxyListAsync();

//             return PartialView(response);

//         }

//         public Task<List<Proxylist>> PullProxyListAsync()
//         {
//             var companyinfo = ua.GetUserCompanyInfo();
//             List<Proxylist> proxylist;
//             if (!string.IsNullOrEmpty(companyinfo))
//             {
//                 proxylist = db.ProxyList.Where(p => p.Company == companyinfo).ToList();
//             }
//             else
//             {
//                 proxylist = new List<Proxylist>();
//             }

//             return Task.FromResult<List<Proxylist>>(proxylist);
//         }

//         [HttpPost]
//         public async Task<ActionResult> PullProxyList(CompanyModel pmodel)
//         {

//             //ViewBag.value = "tab";
//             var response = await PullProxyListPostAsync(pmodel);
//             return PartialView(response);
//         }


//         private Task<List<Proxylist>> PullProxyListPostAsync(CompanyModel pmodel)
//         {
//             List<Proxylist> proxylist;
//             if (!string.IsNullOrEmpty(pmodel.CompanyInfo))
//             {
//                 proxylist = db.ProxyList.Where(p => p.Company == pmodel.CompanyInfo).ToList();
//             }
//             else
//             {
//                 proxylist = new List<Proxylist>();
//             }
//             return Task.FromResult<List<Proxylist>>(proxylist);

//         }

//         public async Task<string> StartTimer()
//         {
//             var response = await StartTimerAsync();

//             return response;
//         }

//         private Task<string> StartTimerAsync()
//         {
//             UploadTimer.setTime();

//             return System.Threading.Tasks.Task.FromResult<string>("Success");
//         }

//         public async Task<string> StopTimer()
//         {
//             var response = await StopTimerAsync();

//             return response;
//         }

//         private Task<string> StopTimerAsync()
//         {
//             UploadTimer.stopTime();

//             return System.Threading.Tasks.Task.FromResult<string>("Success");
//         }


//         public async Task<string> ResetTimer()
//         {
//             var response = await ResetTimerAsync();

//             return response;
//         }

//         private Task<string> ResetTimerAsync()
//         {
//             UploadTimer.ResetTime();

//             return System.Threading.Tasks.Task.FromResult<string>("Success");
//         }

//         public async Task<ActionResult> GetTime()
//         {
//             TimeSpan time = await GetTimeAsync();

//             return PartialView(time);
//         }


//         private Task<TimeSpan> GetTimeAsync()
//         {
//             TimeSpan time = UploadTimer.GetTime();

//             return System.Threading.Tasks.Task.FromResult<TimeSpan>(time);

//         }


//         [HttpPost]
//         public ActionResult Create(FormCollection collection)
//         {
//             try
//             {
//                 // TODO: Add insert logic here

//                 return RedirectToAction("Index");
//             }
//             catch
//             {
//                 return View();
//             }
//         }

//         //
//         // GET: /DataUpload/Edit/5

//         public ActionResult Edit(int id)
//         {
//             return View();
//         }

//         //
//         // POST: /DataUpload/Edit/5

//         [HttpPost]
//         public ActionResult Edit(int id, FormCollection collection)
//         {
//             try
//             {
//                 // TODO: Add update logic here

//                 return RedirectToAction("Index");
//             }
//             catch
//             {
//                 return View();
//             }
//         }

//         //
//         // GET: /DataUpload/Delete/5

//         public ActionResult Delete(int id)
//         {
//             return View();
//         }

//         //
//         // POST: /DataUpload/Delete/5

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
//     }
// }
