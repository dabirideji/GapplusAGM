// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;
// using BarcodeGenerator.Util;
// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Data.Entity;
// using System.Linq;
// using System.Threading.Tasks;
// using System.Web;
// using System.Web.Mvc;

// namespace BarcodeGenerator.Controllers
// {
//      [Authorize]
//     public class VerificationController : Controller
//     {
//         //
//         // GET: /Verification/

//         UsersContext db = new UsersContext();
//         UserAdmin ua = new UserAdmin();

//         //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

//         //private static int RetrieveAGMUniqueID()
//         //{
//         //    UsersContext adb = new UsersContext();
//         //    var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

//         //    return AGMID;
//         //}

//         //private static int UniqueAGMId = RetrieveAGMUniqueID();

//         private static string currentYear = DateTime.Now.Year.ToString();

//         public int PageSize = 10;


//         public async Task<ActionResult> Index()
//         {
//             //ViewBag.presentcount = db.Present.Count();
//             //ViewBag.feedbackcount = db.ShareholderFeedback.Count();
//             //ViewBag.shareholders = db.BarcodeStore.Count();
//             var returnUrl = HttpContext.Request.Url.AbsolutePath;
//             string returnvalue = "";
//             if (HttpContext.Request.QueryString.Count > 0)
//             {
//                 returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//             }
//             ViewBag.value = returnvalue.Trim();
//             var response = await IndexAsync();
//             if (String.IsNullOrEmpty(response.Company))
//             {
//                 return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
//             }
//             return PartialView(response);
//         }


//         public Task<BarcodeViewModel> IndexAsync()
//         {
//             var companyinfo = ua.GetUserCompanyInfo();
//             var UniqueAGMId = ua.RetrieveAGMUniqueID();

//             BarcodeViewModel model = new BarcodeViewModel();

//             model.Company = companyinfo;
//             var setting = db.Settings.FirstOrDefault(s=>s.AGMID == UniqueAGMId);
//             var user = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
//             model.User = user;
//             if (setting != null)
//             {
//                 model.ImageUrl = setting.Image != null ? "data:image/jpg;base64," +
//          Convert.ToBase64String((byte[])setting.Image) : "";


//                 if (!String.IsNullOrEmpty(setting.Title))
//                 {
//                     model.AGMTitle = setting.Title;

//                 }
//                 else
//                 {
//                     model.AGMTitle = "";
//                 }

//                 model.agmid = UniqueAGMId;
//             }
//             return Task.FromResult<BarcodeViewModel>(model);
//         }
//         public ActionResult VerificationViewIndex()
//         {
//             //ViewBag.presentcount = db.Present.Count();
//             //ViewBag.feedbackcount = db.ShareholderFeedback.Count();
//             //ViewBag.shareholders = db.BarcodeStore.Count();
//             return PartialView();
//         }

//         public ActionResult EditIndex()
//         {
//             //ViewBag.presentcount = db.Present.Count();
//             //ViewBag.feedbackcount = db.ShareholderFeedback.Count();
//             //ViewBag.shareholders = db.BarcodeStore.Count();
//             return PartialView();
//         }

//         //
//         // GET: /Verification/Details/5

//         [HttpPost]
//         public async Task<ActionResult> HolderDetails(SearchModel find)
//         {
//             var response = await HolderDetailsAsync(find);

//             return PartialView(response);
//         }
        

//         private Task<BarcodeViewModel> HolderDetailsAsync(SearchModel find)
//         {

//             if (find.search == null)
//             {
                
//                 BarcodeViewModel model = new BarcodeViewModel();
//                 model.Empty = "ENTER A SEARCH VALUE";
//                 return Task.FromResult<BarcodeViewModel>(model);
//             }
//             try
//             {
//                 var companyinfo = ua.GetUserCompanyInfo();
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();


//                 var setting = db.Settings.FirstOrDefault(s=>s.AGMID == UniqueAGMId);

//                 //var barcode = db.BarcodeStore.SingleOrDefault(u => u.emailAddress == find.search || u.ShareholderNum == find.search);

//                 var barcodes = db.BarcodeStore.Where(u => u.Company == companyinfo && u.emailAddress == find.search);
//                 if (barcodes.Count() == 1)
//                 {
//                     if (!barcodes.First().NotVerifiable)
//                     {
//                         BarcodeViewModel model = new BarcodeViewModel
//                         {
//                             id = barcodes.First().Id,
//                             Name = barcodes.First().Name,
//                             Holding = barcodes.First().Holding,
//                             PercentageHolding = barcodes.First().PercentageHolding,
//                             ShareholderNum = barcodes.First().ShareholderNum,
//                             Status = true,
//                             Details = true,
//                             Barcode = barcodes.First().Barcode,
//                             EmailAddress = barcodes.First().emailAddress,
//                             PhoneNumber = barcodes.First().PhoneNumber,
//                             Address = barcodes.First().Address,
//                             BarcodeImage = barcodes.First().BarcodeImage,
//                             Clikapad = barcodes.First().Clikapad,
//                             Message = "SHAREHOLDER VERIFIED OK",
//                         ImageUrl = barcodes.First().BarcodeImage != null ? "data:image/jpg;base64," +
//                                 Convert.ToBase64String((byte[])barcodes.First().BarcodeImage) : ""

//                         };
                        
//                         //var setting = db.Settings.First();
//                         //ViewBag.logo = setting.Image != null ? "data:image/jpg;base64," +
//                         //        Convert.ToBase64String((byte[])setting.Image) : "";
//                         //ViewBag.Company = setting.CompanyName;
//                         var shareholder = db.Present.FirstOrDefault(u => u.Company == companyinfo && u.AGMID == UniqueAGMId && u.ShareholderNum == model.ShareholderNum);

//                         if (shareholder == null)
//                         {
//                             barcodes.First().Present = true;
//                             PresentModel present = new PresentModel();
//                             present.Name = barcodes.First().Name;
//                             present.Address = barcodes.First().Address;
//                             present.Company = barcodes.First().Company;
//                             present.admitSource = "Self";
//                             present.PermitPoll = 1;
//                             present.Year = currentYear;
//                             present.AGMID = UniqueAGMId;
//                             present.ShareholderNum = barcodes.First().ShareholderNum;
//                             present.Holding = barcodes.First().Holding;
//                             present.PercentageHolding = barcodes.First().PercentageHolding;
//                             present.present = true;
//                             present.proxy = false;
//                             present.ParentNumber = barcodes.First().ParentAccountNumber;
//                             present.PresentTime = DateTime.Now;
//                             present.Timestamp = DateTime.Now.TimeOfDay;
//                             present.emailAddress = barcodes.First().emailAddress;
//                             if (!String.IsNullOrEmpty(barcodes.First().PhoneNumber))
//                             {
//                                 string phoneNumber = barcodes.First().PhoneNumber;
//                                 phoneNumber = string.Concat(phoneNumber.Where(char.IsDigit));
//                                 if (phoneNumber.StartsWith("234"))
//                                 {
//                                     present.PhoneNumber = barcodes.First().PhoneNumber;
//                                 }
//                                 else if (phoneNumber.StartsWith("0"))
//                                 {
//                                     double number;
//                                     if (double.TryParse(phoneNumber, out number))
//                                     {
//                                         number = double.Parse(phoneNumber);
//                                         present.PhoneNumber = "234" + number.ToString();
//                                     }
//                                     else
//                                     {
//                                         char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                                         var phonenum = phoneNumber.Split(delimiterChars);
//                                         //string phonenumberresult = string.Concat(phonenum);
//                                         if (double.TryParse(phonenum[0], out number))
//                                         {
//                                             number = double.Parse(phonenum[0]);
//                                             present.PhoneNumber = "234" + number.ToString();
//                                         }

//                                     }

//                                 }

//                             }
//                             present.Clikapad = barcodes.First().Clikapad;
//                             db.Present.Add(present);
//                             barcodes.First().Date = DateTime.Today.ToString();
//                             db.Entry(barcodes.First()).State = EntityState.Modified;

//                             db.SaveChanges();
//                             if (setting != null)
//                             {
//                                 model.logo = setting.Image != null ? "data:image/jpg;base64," +
//                                            Convert.ToBase64String((byte[])setting.Image) : "";
//                                 model.Company = setting.CompanyName;
//                             }
//                             Functions.PresentCount(UniqueAGMId, true);
//                             return Task.FromResult<BarcodeViewModel>(model);
//                         }

//                         var model1 = new BarcodeViewModel()
//                         {
//                             Message = find.search + " - " + "Verified OR Present by Proxy!"
//                         };

//                         if (setting != null)
//                         {
//                             model1.logo = setting.Image != null ? "data:image/jpg;base64," +
//                                        Convert.ToBase64String((byte[])setting.Image) : "";
//                             model1.Company = setting.CompanyName;
//                         }
//                         return Task.FromResult<BarcodeViewModel>(model1);
//                     }

//                     BarcodeViewModel model2 = new BarcodeViewModel();
//                     model2.Status = false;
//                     model2.Details = false;
//                     model2.Void = find.search + "-" + "May have been verified by proxy!";
//                     if (setting != null)
//                     {
//                         model2.logo = setting.Image != null ? "data:image/jpg;base64," +
//                                    Convert.ToBase64String((byte[])setting.Image) : "";
//                         model2.Company = setting.CompanyName;
//                     }
//                     return Task.FromResult<BarcodeViewModel>(model2);

//                 }
//                 else if(barcodes.Count()>1)
//                 {
//                     var pcount = 0;
//                     //var acount = 0;
//                     foreach (var barcode in barcodes)
//                     {
//                         if (!barcode.NotVerifiable)
//                         {
//                             var shareholder = db.Present.FirstOrDefault(u => u.AGMID == UniqueAGMId && u.ShareholderNum == barcode.ShareholderNum);

//                             if (shareholder == null)
//                             {

//                                 barcode.Present = true;
//                                 PresentModel present = new PresentModel();
//                                 present.Name = barcode.Name;
//                                 present.Address = barcode.Address;
//                                 present.Company = barcode.Company;
//                                 present.admitSource = "Self";
//                                 present.Year = currentYear;
//                                 present.AGMID = UniqueAGMId;
//                                 present.ShareholderNum = barcode.ShareholderNum;
//                                 present.ParentNumber = barcode.ParentAccountNumber;
//                                 present.Holding = barcode.Holding;
//                                 present.PercentageHolding = barcode.PercentageHolding;
//                                 present.present = true;
//                                 present.proxy = false;
//                                 present.PresentTime = DateTime.Now;
//                                 present.Timestamp = DateTime.Now.TimeOfDay;
//                                 present.emailAddress = barcode.emailAddress;
//                                 if (!String.IsNullOrEmpty(barcode.PhoneNumber))
//                                 {
//                                     string phoneNumber = barcode.PhoneNumber;
//                                     phoneNumber = string.Concat(phoneNumber.Where(char.IsDigit));
//                                     //number = double.Parse(collection.PhoneNumber);
//                                     if (phoneNumber.StartsWith("234"))
//                                     {
//                                         present.PhoneNumber = phoneNumber;
//                                     }
//                                     else if (phoneNumber.StartsWith("0"))
//                                     {
//                                         double number;
//                                         if (double.TryParse(phoneNumber, out number))
//                                         {
//                                             number = double.Parse(phoneNumber);
//                                             present.PhoneNumber = "234" + number.ToString();
//                                         }
//                                         else
//                                         {
//                                             char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                                             var phonenum = phoneNumber.Split(delimiterChars);
//                                             //string phonenumberresult = string.Concat(phonenum);
//                                             if (double.TryParse(phonenum[0], out number))
//                                             {
//                                                 number = double.Parse(phonenum[0]);
//                                                 present.PhoneNumber = "234" + number.ToString();
//                                             }

//                                         }

//                                     }

//                                 }
//                                 present.Clikapad = barcode.Clikapad;
//                                 db.Present.Add(present);
//                                 barcode.Date = DateTime.Today.ToString();
//                                 db.Entry(barcode).State = EntityState.Modified;
                               

//                             }
//                             pcount++;
//                         }
//                         }
//                         db.SaveChanges();
//                         BarcodeViewModel model = new BarcodeViewModel
//                         {
//                             Status = true,
//                             Details = false,
//                             Message = "Account(s) Verified OK!"
//                          };
//                     if (setting != null)
//                     {
//                         model.logo = setting.Image != null ? "data:image/jpg;base64," +
//                                    Convert.ToBase64String((byte[])setting.Image) : "";
//                         model.Company = setting.CompanyName;
//                     }
//                     Functions.PresentCount(UniqueAGMId, true);
//                     return Task.FromResult< BarcodeViewModel>(model);
                    
//                 }
//                 else
//                 {
//                     BarcodeViewModel model= new BarcodeViewModel();
//                     model.Status = false;
//                     model.Details = false;
//                     model.Void = find.search + "-" + "QRCODE DOESNOT EXIST.";
//                     if (setting != null)
//                     {
//                         model.logo = setting.Image != null ? "data:image/jpg;base64," +
//                                    Convert.ToBase64String((byte[])setting.Image) : "";
//                         model.Company = setting.CompanyName;
//                     }
//                     return Task.FromResult<BarcodeViewModel>(model);
//                 }


//             }
//             catch(Exception e)
//             {

//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();

//                 var setting = db.Settings.FirstOrDefault(s =>s.AGMID == UniqueAGMId);

//                 BarcodeViewModel model = new BarcodeViewModel();
//                 model.Status = false;
//                 model.Void = "ALREADY VERIFIED AND MARKED AS PROXY";
//                 if (setting != null)
//                 {
//                     model.logo = setting.Image != null ? "data:image/jpg;base64," +
//                                Convert.ToBase64String((byte[])setting.Image) : "";
//                     model.Company = setting.CompanyName;
//                 }
//                 return Task.FromResult<BarcodeViewModel>(model);
//             }

//         }


//         public ActionResult PrintList()
//         {
//             var barcode = db.BarcodeStore.ToArray();
//             IList<BarcodeModel> model = new List<BarcodeModel>();

//             for (int i = 0; i < barcode.Length; i++)
//             {
//                 var p = barcode[i].BarcodeImage;
//                 BarcodeModel barcodemodel = new BarcodeModel()
//                 {
//                     Id = barcode[i].Id,
//                    Name = barcode[i].Name.ToUpper().ToString(),
                    
//                     Barcode = barcode[i].Barcode.ToString(),
//                     ImageUrl = barcode[i].BarcodeImage != null ? "data:image/jpg;base64," +
//                         Convert.ToBase64String((byte[])barcode[i].BarcodeImage) : ""
//                 };

//                 model.Add(barcodemodel);

//             }
//             var qrcodelist = model.Take(40);
//             return PartialView(qrcodelist);

//         }
//         //
//         // GET: /Verification/Create

//         public ActionResult Create()
//         {
//             return View();
//         }

//         //
//         // POST: /Verification/Create

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
//         // GET: /Verification/Edit/5

//         //public ActionResult Edit(int id)
//         //{
//         //    var model = db.BarcodeStore.Find(id);
//         //    return PartialView(model);
//         //}

//         //
//         // POST: /BarcodeLib/Edit/5

//         [HttpPost]
//         public void Edit(int id, BarcodeViewModel collection)
//         {
//             try
//             {
//                 //string phonenumber = null;
//                 //double number = 0;
//                 //if (!String.IsNullOrEmpty(collection.PhoneNumber))
//                 //{
//                 //    //number = double.Parse(collection.PhoneNumber);
//                 //    if(collection.PhoneNumber.StartsWith("234"))
//                 //    {

//                 //    }
//                 //    else if(collection.PhoneNumber.StartsWith("0"))
//                 //    {

//                 //    }

//                 //}

   
//                 //    string phonenumber = null;

//                 //    if (!String.IsNullOrEmpty(collection.PhoneNumber))
//                 //    {
//                 //        if (!collection.PhoneNumber.StartsWith("234", StringComparison.OrdinalIgnoreCase))
//                 //            {
//                 //                phonenumber = collection.PhoneNumber;
//                 //            }

//                 //else if (!collection.PhoneNumber.StartsWith("+234", StringComparison.OrdinalIgnoreCase))
//                 //{
//                 //    phonenumber = collection.PhoneNumber.Substring(1);
//                 //}
//                 //else if(collection.PhoneNumber.StartsWith("0", StringComparison.OrdinalIgnoreCase))
//                 //        {
//                 //            double number = double.Parse(collection.PhoneNumber);
//                 //            phonenumber = "234" + number.ToString();
//                 //        }
//                 //else
//                 //            {
//                 //                SearchModel find1 = new SearchModel();
//                 //                find1.search = collection.ShareholderNum;
//                 //                TempData["Message1"] = "Cannot Edit database";
//                 //                return RedirectToAction("EditIndex");
//                 //             }
    
                        
//                 var model = db.BarcodeStore.Find(collection.id);

//                 if (!String.IsNullOrEmpty(collection.PhoneNumber))
//                 {
//                     string phoneNumber = collection.PhoneNumber;
//                     phoneNumber = string.Concat(phoneNumber.Where(char.IsDigit));
//                     //number = double.Parse(collection.PhoneNumber);
//                     if (phoneNumber.StartsWith("234"))
//                     {
//                         model.PhoneNumber = phoneNumber;
//                     }
//                     else if (phoneNumber.StartsWith("0"))
//                     {
//                        var number = double.Parse(phoneNumber);
//                         model.PhoneNumber = "234" + number.ToString();

//                     }

//                 }
//                 if (!String.IsNullOrEmpty(collection.EmailAddress))
//                 {
//                     model.emailAddress = collection.EmailAddress;
//                 }

//                 if (!String.IsNullOrEmpty(collection.Clikapad))
//                 {
//                     model.Clikapad= collection.Clikapad;
//                 }
//                 db.Entry(model).State = EntityState.Modified;
//                 //db.SaveChanges();
//                 //TempData["Message"] = "Edited";
//                 //return RedirectToAction("Index", "Present");
//                 // TODO: Add update logic here
//                 //var companyinfo = ua.GetUserCompanyInfo();   
//                 var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                 var modelpresent = db.Present.Where(p=>p.AGMID == UniqueAGMId && p.ShareholderNum==collection.ShareholderNum).ToArray();
//                 foreach(var item in modelpresent)
//                 {
//                     if (!String.IsNullOrEmpty(collection.PhoneNumber))
//                     {
//                         string phoneNumber = collection.PhoneNumber;
//                         phoneNumber = string.Concat(phoneNumber.Where(char.IsDigit));
//                         //number = double.Parse(collection.PhoneNumber);
//                         if (phoneNumber.StartsWith("234"))
//                         {
//                             model.PhoneNumber = phoneNumber;
//                         }
//                         else if (phoneNumber.StartsWith("0"))
//                         {
//                             double number;
//                             if (double.TryParse(phoneNumber, out number))
//                             {
//                                 number = double.Parse(phoneNumber);
//                                 model.PhoneNumber = "234" + number.ToString();
//                             }
//                             else
//                             {
//                                 char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                                 var phonenum = phoneNumber.Split(delimiterChars);
//                                 //string phonenumberresult = string.Concat(phonenum);
//                                 if (double.TryParse(phonenum[0], out number))
//                                 {
//                                     number = double.Parse(phonenum[0]);
//                                     model.PhoneNumber = "234" + number.ToString();
//                                 }

//                             }

//                         }

//                     }
//                     if (!String.IsNullOrEmpty(collection.EmailAddress))
//                     {
//                         item.emailAddress = collection.EmailAddress;

//                     }
//                     if (!String.IsNullOrEmpty(collection.Clikapad))
//                     {
//                         item.Clikapad = collection.Clikapad;
//                         item.GivenClikapad = true;
//                     }
//                     db.Entry(item).State = EntityState.Modified;
//                 }
//                 db.SaveChanges();
//                 SearchModel find = new SearchModel();
//                 find.search=collection.ShareholderNum.ToString();
//                 TempData["Message"] = "Edited";
//                 //return RedirectToAction("HolderDetails", "Verification", new { find = find });
//                 //return RedirectToAction("EditIndex");
//             }
//             catch
//             {
//                 SearchModel find = new SearchModel();
//                 find.search = collection.ShareholderNum.ToString();
//                 TempData["Message1"] = "Cannot Edit database";
//                 //return RedirectToAction("EditIndex");
//                 //return RedirectToAction("HolderDetails", "Verification", new { find = find });
//             }
//         }

//         //
//         // GET: /Verification/Delete/5

//         public ActionResult Delete(int id)
//         {
//             return View();
//         }

//         //
//         // POST: /Verification/Delete/5

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
