//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Net;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using BarcodeGenerator.Models;
//using BarcodeGenerator.Service;
//using BarcodeGenerator.Util;
//using Gapplus.Application.Helpers;
//using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace Gapplus.Application.Interfaces.IContracts
//{
//    public class BarCodeService
//    {
//        UsersContext db;
//        UserAdmin ua;
//        AGMManager agmM;





//        public BarCodeService(UsersContext usersContext, UserAdmin userAdmin, AGMManager agmManager)
//        {
//            db = usersContext;
//            ua  = new UserAdmin(db);
//            agmM  = new AGMManager(db);

//        }






//        //
//        // GET: /BarcodeLib/
//        byte[] BarCode;
//        byte[] qrcode;
//        private static string currentYear = DateTime.Now.Year.ToString();

//        //private static string companyinfo = ua.GetUserCompanyInfo();

//        //private int RetrieveAGMUniqueID()
//        //{
//        //    UsersContext adb = new UsersContext();
//        //    var companyinfo = ua.GetUserCompanyInfo();

//        //    if (string.IsNullOrEmpty(companyinfo))
//        //    {
//        //        return -1;
//        //    }

//        //   var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

//        //    return AGMID;
//        //}





//        public int PageSize = 10;
//        int page = 1;

//        // public async Task<ActionResult> Index()
//        // {

//        //     var response = await IndexAsync();
//        //     //if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
//        //     //var returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);
//        //     var returnUrl = HttpContext.Request.Url.AbsolutePath;
//        //     //var raw = HttpContext.Request.Url.AbsoluteUri;
//        //     //var raw1 = HttpContext.Request.Url.PathAndQuery;
//        //     string returnvalue = "";
//        //     if (HttpContext.Request.QueryString.Count > 0)
//        //     {
//        //         returnvalue = HttpContext.Request.QueryString["rel"].ToString();
//        //     }
//        //     ViewBag.value = returnvalue.Trim();
//        //     //string path = "";
//        //     //string queryString = "";
//        //     //System.Web.Routing.RouteData routeFromUrl = System.Web.Routing.RouteTable.Routes.GetRouteData(new HttpContextWrapper(new HttpContext(new HttpRequest(null, new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port, path).ToString(), queryString), new HttpResponse(new System.IO.StringWriter()))));
//        //     ////var id = HttpContext.Request.RequestContext.RouteData.GetRequiredString("rel");
//        //     //string id = routeFromUrl.Values["rel"].ToString();

//        //     if (String.IsNullOrEmpty(response.CompanyInfo))
//        //     {
//        //         return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
//        //     }

//        //     return PartialView(response);

//        // }



//        public Task<BarcodeModelIndexDto> IndexAsync()
//        {
//            // var user = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
//            var user =new UserProfile(); //NEED TO COME BACKHERE TO MAKE THE FIXES
//            var companyinfo = ua.GetUserCompanyInfo();
//            var UniqueAGMId = ua.RetrieveAGMUniqueID();
//            SettingsModel setting = new SettingsModel();
//            var abstainbtnchoice = true;
//            var forBg = "green";
//            var againstBg = "red";
//            var abstainBg = "blue";
//            var voidBg = "black";
//            var AGMTitle = "";

//            if (UniqueAGMId != -1)
//            {

//                setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
//                if (setting != null && !String.IsNullOrEmpty(setting.Title))
//                {
//                    AGMTitle = setting.Title.ToUpper();
//                    if (setting.AbstainBtnChoice != null)
//                    {
//                        abstainbtnchoice = (bool)setting.AbstainBtnChoice;
//                    }
//                    if (!string.IsNullOrEmpty(setting.VoteForColorBg))
//                    {
//                        forBg = setting.VoteForColorBg;
//                    }
//                    if (!string.IsNullOrEmpty(setting.VoteAgainstColorBg))
//                    {
//                        againstBg = setting.VoteAgainstColorBg;
//                    }
//                    if (!string.IsNullOrEmpty(setting.VoteAbstaincolorBg))
//                    {
//                        abstainBg = setting.VoteAbstaincolorBg;
//                    }
//                    if (!string.IsNullOrEmpty(setting.VoteVoidColorBg))
//                    {
//                        voidBg = setting.VoteVoidColorBg;
//                    }


//                }

//            }


//            BarcodeModelIndexDto dto = new BarcodeModelIndexDto
//            {
//                User = user,
//                Settings = setting,
//                AGMTitle = AGMTitle,
//                CompanyInfo = companyinfo,
//                abstainBtnChoice = abstainbtnchoice,
//                forBg = forBg,
//                againstBg = againstBg,
//                abstainBg = abstainBg,
//                voidBg = voidBg,
//                agmid = UniqueAGMId
//            };
//            return Task.FromResult<BarcodeModelIndexDto>(dto);
//        }




//        // public async Task<ActionResult> UpdateIndex()
//        // {
//        //     var response = await UpdateIndexAsync();

//        //     return View(response);

//        // }


//        public Task<BarcodeListViewModel> UpdateIndexAsync()
//        {
//            var companyinfo = ua.GetUserCompanyInfo();
//            var Barcodes = db.BarcodeStore.Where(b => b.Company == companyinfo);
//            BarcodeListViewModel model = new BarcodeListViewModel
//            {
//                barcodes = Barcodes
//                .OrderBy(s => s.Id)
//                .Skip((page - 1) * PageSize)
//                .Take(PageSize),
//                PagingInfo = new PagingInfo
//                {
//                    CurrentPage = page,
//                    ItemsPerPage = PageSize,
//                    TotalItems = Barcodes.Count()
//                }
//            };
//            return Task.FromResult<BarcodeListViewModel>(model);

//        }

//        // public async Task<ActionResult> indexTable()
//        // {
//        //     var response = await indexTableAsync();

//        //     return PartialView(response);

//        // }

//        public Task<List<BarcodeModel>> indexTableAsync()
//        {
//            var companyinfo = ua.GetUserCompanyInfo();
//            var barcode = db.BarcodeStore.Where(b => b.Company == companyinfo).ToList();

//            return Task.FromResult<List<BarcodeModel>>(barcode);

//        }


//        // public ActionResult Edit(int id)
//        // {
//        //     var model = db.BarcodeStore.Find(id);
//        //     return PartialView(model);
//        // }

//        //
//        // POST: /BarcodeLib/Edit/5

//        // public async Task<ActionResult> Edit(int id, PresentModel collection)
//        // {
//        //     var response = await EditAsync(id, collection);

//        //     return Json(response, JsonRequestBehavior.AllowGet);

//        // }


//        private Task<string> EditAsync(int id, PresentModel collection)
//        {
//            try
//            {
//                var companyinfo = ua.GetUserCompanyInfo();
//                var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                // TODO: Add update logic here
//                var model = db.BarcodeStore.Find(collection.Id);
//                if (!String.IsNullOrEmpty(collection.PhoneNumber))
//                {
//                    if (collection.PhoneNumber.StartsWith("234"))
//                    {
//                        model.PhoneNumber = collection.PhoneNumber;
//                    }
//                    else if (collection.PhoneNumber.StartsWith("0"))
//                    {
//                        var number = double.Parse(collection.PhoneNumber);
//                        model.PhoneNumber = "234" + number.ToString();

//                    }

//                }
//                if (!String.IsNullOrEmpty(collection.emailAddress))
//                {
//                    model.emailAddress = collection.emailAddress;
//                }
//                model.Clikapad = collection.Clikapad;

//                db.Entry(model).State = EntityState.Modified;
//                var presentmodel = db.Present.FirstOrDefault(b => b.Company == companyinfo && b.ShareholderNum == model.ShareholderNum);
//                if (presentmodel != null)
//                {
//                    if (!String.IsNullOrEmpty(collection.emailAddress))
//                    {
//                        presentmodel.emailAddress = collection.emailAddress;
//                    }

//                    if (!String.IsNullOrEmpty(collection.PhoneNumber))
//                    {
//                        //number = double.Parse(collection.PhoneNumber);
//                        if (collection.PhoneNumber.StartsWith("234"))
//                        {
//                            presentmodel.PhoneNumber = collection.PhoneNumber;
//                        }
//                        else if (collection.PhoneNumber.StartsWith("0"))
//                        {
//                            var number = double.Parse(collection.PhoneNumber);
//                            presentmodel.PhoneNumber = "234" + number.ToString();

//                        }

//                    }

//                    presentmodel.Clikapad = collection.Clikapad;


//                    if (!String.IsNullOrEmpty(collection.Clikapad))
//                    {
//                        presentmodel.GivenClikapad = true;
//                        UpdateClikapad(presentmodel, collection.Clikapad);
//                    }
//                    db.Entry(presentmodel).State = EntityState.Modified;

//                }
//                db.SaveChanges();
//                return Task.FromResult<string>("success");
//            }
//            catch
//            {
//                // //TempData["Message1"] = "Cannot Edit database";
//                return Task.FromResult<string>("failed");
//            }
//        }


//        private void UpdateClikapad(PresentModel model, string clikapad)
//        {
//            try
//            {
//                if (model != null)
//                {
//                    if (!string.IsNullOrEmpty(clikapad))
//                    {
//                        var checkOTherAccountsExist = db.Present.Where(b => b.Company == model.Company && b.emailAddress == model.emailAddress);
//                        if (checkOTherAccountsExist != null)
//                        {
//                            if (checkOTherAccountsExist.Count() == 1)
//                            {
//                                checkOTherAccountsExist.First().Clikapad = clikapad;
//                                checkOTherAccountsExist.First().GivenClikapad = true;
//                                var shareholdernumber = checkOTherAccountsExist.First().ShareholderNum;
//                                var company = checkOTherAccountsExist.First().Company;
//                                var checkifpresent = db.BarcodeStore.FirstOrDefault(f => f.Company == company && f.ShareholderNum == shareholdernumber);
//                                if (checkifpresent != null)
//                                {

//                                    checkifpresent.Selected = false;
//                                    checkifpresent.Clikapad = clikapad;
//                                }

//                                db.SaveChanges();
//                            }
//                            else if (checkOTherAccountsExist.Count() > 1)
//                            {
//                                foreach (var item in checkOTherAccountsExist)
//                                {
//                                    item.Clikapad = clikapad;
//                                    item.GivenClikapad = true;
//                                    var shareholdernumber = item.ShareholderNum;
//                                    var checkifpresent = db.BarcodeStore.FirstOrDefault(f => f.Company == item.Company && f.ShareholderNum == shareholdernumber);
//                                    if (checkifpresent != null)
//                                    {

//                                        checkifpresent.Selected = false;
//                                        checkifpresent.Clikapad = clikapad;


//                                    }
//                                }
//                                db.SaveChanges();
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception e)
//            {

//            }

//        }




//        public async Task<string> Clikapad(PostSelectModel post)
//        {

//            var response = await ClikapadAsync(post.stringvalue, post.clikapad);

//            return response;
//        }


//        private async Task<string> ClikapadAsync(string val, string value)
//        {
//            try
//            {

//                var companyinfo = ua.GetUserCompanyInfo();
//                var UniqueAGMid = ua.RetrieveAGMUniqueID(companyinfo);
//                if (UniqueAGMid == -1)
//                {
//                    return "Cannot retrieve AGMID";
//                }
//                //var jvalues = JsonConvert.DeserializeObject(val);
//                //JArray a = JArray.Parse(val);

//                //Jvar IdsJson = Json.Parse(jvalues);
//                //var value = jvalues.Split(',');
//                //foreach (var item in aRegularizeEmailAddress
//                //{
//                int id;
//                if (!string.IsNullOrEmpty(val) || !string.IsNullOrEmpty(value))
//                {
//                    id = int.Parse(val.ToString());

//                    var shareholder = db.BarcodeStore.Find(id);
//                    if (shareholder != null)
//                    {
//                        if (!string.IsNullOrEmpty(shareholder.emailAddress))
//                        {
//                            var checkOTherAccountsExist = db.BarcodeStore.Where(b => b.Company == shareholder.Company && b.emailAddress == shareholder.emailAddress);
//                            if (checkOTherAccountsExist != null)
//                            {
//                                if (checkOTherAccountsExist.Count() == 1)
//                                {
//                                    var sholdernum = shareholder.ShareholderNum;
//                                    var cpresent = db.Present.FirstOrDefault(f => f.AGMID == UniqueAGMid && f.ShareholderNum == sholdernum);
//                                    if (cpresent != null)
//                                    {
//                                        cpresent.Clikapad = value;

//                                    }
//                                    else
//                                    {
//                                        await PresentAsync(shareholder.Id, new QuestionStatus { status = true });
//                                    }
//                                    shareholder.Selected = false;
//                                    shareholder.Clikapad = value;
//                                    db.SaveChanges();
//                                    return "Success";
//                                }
//                                else if (checkOTherAccountsExist.Count() > 1)
//                                {
//                                    BarcodeModel cshareholderRecord;
//                                    var checkifConsolidate = checkOTherAccountsExist.Any(c => c.Consolidated == true);
//                                    if (!checkifConsolidate)
//                                    {
//                                        cshareholderRecord = agmM.ConsolidateRequest(shareholder.Company, checkOTherAccountsExist.ToList());
//                                    }
//                                    else
//                                    {
//                                        var consolidatedvalue = checkOTherAccountsExist.Where(s => s.Consolidated == true).Select(s => s.ConsolidatedValue).FirstOrDefault();

//                                        cshareholderRecord = agmM.GetConsolidatedAccount(shareholder.Company, consolidatedvalue);
//                                    }

//                                    var shareholderNumber = cshareholderRecord.ShareholderNum;
//                                    var checkifpresent = db.Present.FirstOrDefault(f => f.AGMID == UniqueAGMid && f.ShareholderNum == shareholderNumber);
//                                    if (checkifpresent != null)
//                                    {
//                                        checkifpresent.Clikapad = value;
//                                    }
//                                    else
//                                    {
//                                        await PresentAsync(shareholder.Id, new QuestionStatus { status = true });
//                                    }
//                                    cshareholderRecord.Selected = false;
//                                    cshareholderRecord.Clikapad = value;
//                                    db.Entry(cshareholderRecord).State = EntityState.Modified;
//                                    //foreach (var item in checkOTherAccountsExist)
//                                    //{
//                                    //    var shareholdernumber = item.ShareholderNum;
//                                    //    var checkifpresent = db.Present.FirstOrDefault(f => f.AGMID == UniqueAGMid && f.ShareholderNum == shareholdernumber);
//                                    //    if (checkifpresent != null)
//                                    //   {
//                                    //            checkifpresent.Clikapad =  value;
//                                    //    }

//                                    //    item.Selected = false;
//                                    //    item.Clikapad = value;
//                                    //    db.Entry(item).State = EntityState.Modified;
//                                    //}
//                                    db.SaveChanges();
//                                    return "Success";
//                                }
//                            }
//                        }
//                        var shareholdernum = shareholder.ShareholderNum;
//                        var checkpresent = db.Present.FirstOrDefault(f => f.AGMID == UniqueAGMid && f.ShareholderNum == shareholdernum);
//                        if (checkpresent != null)
//                        {
//                            //checkpresent.Clikapad = value;

//                            checkpresent.Clikapad = value;
//                        }
//                        else
//                        {
//                            await PresentAsync(shareholder.Id, new QuestionStatus { status = true });
//                        }
//                        shareholder.Selected = false;
//                        //shareholder.Clikapad = value;

//                        shareholder.Clikapad = value;

//                        db.SaveChanges();
//                        return "Success";
//                    }
//                    return "Empty ID";
//                }
//                return "Empty ID ";
//            }
//            catch (Exception e)
//            {
//                return "Something went Wrong" + " " + e;
//            }


//        }

//        // [HttpPost]
//        // public async Task<ActionResult> PresentByProxy(int id, QuestionStatus data)
//        // {
//        //     var response = await PresentByProxyAsync(id, data);

//        //     return RedirectToAction("IndexTable");
//        // }

//        private Task<string> PresentByProxyAsync(int id, QuestionStatus data)
//        {
//            try
//            {
//                var companyinfo = ua.GetUserCompanyInfo();
//                var UniqueAGMId = ua.RetrieveAGMUniqueID();
//                var shareholder = db.BarcodeStore.Find(id);
//                var presentmodel = db.Present.FirstOrDefault(p => p.Company == companyinfo && p.AGMID == UniqueAGMId && p.ShareholderNum == shareholder.ShareholderNum);

//                if (data.status == true)
//                {
//                    shareholder.Present = false;
//                    if (presentmodel != null)
//                    {
//                        //Do Nothing
//                    }
//                    else
//                    {
//                        string phonenumber = null;
//                        if (!String.IsNullOrEmpty(shareholder.PhoneNumber))
//                        {
//                            double number = double.Parse(shareholder.PhoneNumber);
//                            phonenumber = "234" + number.ToString();
//                        }
//                        PresentModel proxy = new PresentModel();
//                        proxy.Name = shareholder.Name;
//                        proxy.Address = shareholder.Address;
//                        proxy.Company = shareholder.Company;
//                        proxy.admitSource = "Proxy";
//                        proxy.PermitPoll = 1;
//                        proxy.Year = currentYear;
//                        proxy.AGMID = UniqueAGMId;
//                        proxy.ShareholderNum = shareholder.ShareholderNum;
//                        proxy.Holding = shareholder.Holding;
//                        if (!String.IsNullOrEmpty(shareholder.emailAddress))
//                        {
//                            proxy.emailAddress = string.Format("proxy{0}", shareholder.emailAddress);
//                        }

//                        proxy.PhoneNumber = phonenumber;
//                        proxy.PercentageHolding = shareholder.PercentageHolding;
//                        proxy.proxy = true;
//                        proxy.PresentTime = DateTime.Now;
//                        proxy.Timestamp = DateTime.Now.TimeOfDay;
//                        db.Present.Add(proxy);
//                        shareholder.emailAddress = string.Format("proxy{0}", shareholder.emailAddress);

//                    }
//                }
//                shareholder.PresentByProxy = data.status;
//                //db.Entry(shareholder).State = EntityState.Modified;
//                db.SaveChanges();
//                Functions.PresentCount(UniqueAGMId, true);
//                HttpResponseMessage response = new HttpResponseMessage();
//                response.StatusCode = HttpStatusCode.OK;

//                return Task.FromResult<string>("success");
//            }
//            catch (Exception e)
//            {
//                return Task.FromResult<string>("Failed");
//            }

//        }



//        public async Task<string> SelectedAllItem(PostSelectModel post)
//        {
//            //Request.QueryString.

//            var response = await SelectedAllItemAsync(post.ids);

//            return response;
//        }


//        private Task<string> SelectedAllItemAsync(string val)
//        {
//            try
//            {
//                var jvalues = JsonConvert.DeserializeObject(val);
//                JArray a = JArray.Parse(val);

//                //Jvar IdsJson = Json.Parse(jvalues);
//                //var value = jvalues.Split(',');
//                foreach (var item in a)
//                {
//                    var id = int.Parse(item.ToString());
//                    var shareholder = db.BarcodeStore.Find(id);
//                    if (shareholder != null)
//                    {
//                        shareholder.Selected = true;
//                        db.Entry(shareholder).State = EntityState.Modified;

//                    }
//                }
//                db.SaveChanges();
//                return Task.FromResult<string>("success");

//            }
//            catch (Exception e)
//            {
//                return Task.FromResult<string>("failure");

//            }

//        }


//        public async Task<string> UnSelectedAllItem(PostSelectModel post)
//        {
//            //Request.QueryString.

//            var response = await UnSelectedAllItemAsync(post.ids);

//            return response;
//        }


//        private Task<string> UnSelectedAllItemAsync(string val)
//        {
//            try
//            {
//                var jvalues = JsonConvert.DeserializeObject(val);
//                JArray a = JArray.Parse(val);

//                //Jvar IdsJson = Json.Parse(jvalues);
//                //var value = jvalues.Split(',');
//                foreach (var item in a)
//                {
//                    var id = int.Parse(item.ToString());
//                    var shareholder = db.BarcodeStore.Find(id);
//                    if (shareholder != null)
//                    {
//                        shareholder.Selected = false;
//                        db.Entry(shareholder).State = EntityState.Modified;

//                    }
//                }
//                db.SaveChanges();
//                return Task.FromResult<string>("success");

//            }
//            catch (Exception e)
//            {
//                return Task.FromResult<string>("failure");

//            }

//        }
//        //[HttpPost]
//        public async Task<string> SelectedItem(int id, QuestionStatus data)
//        {
//            var response = await SelectedItemAsync(id, data);

//            return response;
//        }

//        private Task<string> SelectedItemAsync(int id, QuestionStatus data)
//        {
//            try
//            {
//                var shareholder = db.BarcodeStore.Find(id);
//                if (shareholder != null)
//                {
//                    shareholder.Selected = data.status;
//                    db.Entry(shareholder).State = EntityState.Modified;
//                    db.SaveChanges();
//                }
//                else
//                {
//                    return Task.FromResult<string>("failure");
//                }


//                return Task.FromResult<string>("success");

//            }
//            catch (Exception e)
//            {
//                return Task.FromResult<string>("failure");

//            }

//        }


//        private bool CheckifPhoneNumberIsRegular(List<BarcodeModel> list)
//        {
//            var value = false;
//            var firstItem = list.First();
//            for (int i = 1; i < list.Count(); i++)
//            {
//                if (list[i].PhoneNumber == null || list[i].PhoneNumber == "")
//                {
//                    value = true;
//                }
//                else if (firstItem.PhoneNumber == null || firstItem.PhoneNumber == "")
//                {
//                    value = true;
//                }
//                else if (list[i].PhoneNumber != firstItem.PhoneNumber)
//                {
//                    value = true;
//                }
//                else if (Regex.Match(list[i].PhoneNumber, @"/[^0-9]/g,''").Success)
//                {
//                    value = true;
//                }
//                else if (Regex.Match(firstItem.PhoneNumber, @"/[^0-9]/g,''").Success)
//                {
//                    value = true;
//                }
//            }
//            return value;
//        }


//        // [HttpPost]
//        // public async Task<string> Present(int id, QuestionStatus data)
//        // {
//        //     var response = await PresentAsync(id, data);

//        //     return response;
//        // }


//        private Task<string> PresentAsync(int id, QuestionStatus data)
//        {
//            var companyinfo = ua.GetUserCompanyInfo();
//            var UniqueAGMId = ua.RetrieveAGMUniqueID();

//            List<BarcodeModel> multipleEntry = new List<BarcodeModel>();
//            var shareholder = db.BarcodeStore.Find(id);
//            var count = 0;
//            string Message = "";
//            var AgmEvent = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
//            if (AgmEvent == null)
//            {
//                return Task.FromResult<string>("AGMID Inconsistency.");
//            }
//            if (AgmEvent.StopAdmittance)
//            {
//                return Task.FromResult<string>("Admitance have been stopped");
//            }
//            if (!String.IsNullOrEmpty(shareholder.emailAddress))
//            {
//                string connStr = DatabaseManager.GetConnectionString();

//                SqlConnection conn =
//                        new SqlConnection(connStr);
//                //string query2 = "select * from BarcodeModels WHERE CONTAINS(Name,'" + searchValue + "')";
//                //string query2 = "select * from BarcodeModels WHERE emailAddress LIKE '%" + shareholder.emailAddress + "%'";

//                //string query2 = "select * from BarcodeModels WHERE Company =  '" + companyinfo + "' AND  emailAddress = '" + shareholder.emailAddress + "' AND NotVerifiable = 'FALSE'";
//                string query2 = "select * from BarcodeModels WHERE Company =  '" + companyinfo + "' AND  emailAddress = '" + shareholder.emailAddress + "'";
//                conn.Open();
//                SqlCommand cmd = new SqlCommand(query2, conn);
//                SqlDataReader read = cmd.ExecuteReader();

//                while (read.Read())
//                {
//                    BarcodeModel model = new BarcodeModel
//                    {

//                        SN = (Int64.Parse(read["SN"].ToString())),
//                        Id = int.Parse(read["Id"].ToString()),
//                        Name = (read["Name"].ToString()),
//                        Address = (read["Address"].ToString()),
//                        ShareholderNum = (Int64.Parse(read["ShareholderNum"].ToString())),
//                        ParentAccountNumber = (Int64.Parse(read["ParentAccountNumber"].ToString())),
//                        Holding = double.Parse(read["Holding"].ToString()),
//                        PercentageHolding = double.Parse(read["PercentageHolding"].ToString()),
//                        Company = (read["Company"].ToString()),
//                        PhoneNumber = (read["PhoneNumber"].ToString()),
//                        emailAddress = (read["emailAddress"].ToString()),
//                        Present = bool.Parse(read["Present"].ToString()),
//                        PresentByProxy = bool.Parse(read["PresentByProxy"].ToString()),
//                        split = bool.Parse(read["split"].ToString()),
//                        resolution = bool.Parse(read["resolution"].ToString()),
//                        Clikapad = (read["Clikapad"].ToString()),
//                        TakePoll = bool.Parse(read["TakePoll"].ToString()),
//                        Consolidated = bool.Parse(read["Consolidated"].ToString()),
//                        ConsolidatedParent = read["ConsolidatedParent"].ToString(),
//                        ConsolidatedValue = read["ConsolidatedValue"].ToString()

//                    };
//                    multipleEntry.Add(model);
//                }
//                read.Close();
//                //var multipleEntry = db.BarcodeStore.Where(s => s.emailAddress == shareholder.emailAddress).ToList();
//                if (multipleEntry.Count() > 1 && data.status)
//                {
//                    var checkIfAnyAccountIsProxy = multipleEntry.Any(s => s.PresentByProxy == true);
//                    //var srecord = shareholderRecords.First();
//                    BarcodeModel srecord;
//                    var checkifConsolidate = multipleEntry.Any(c => c.Consolidated == true);
//                    if (!checkifConsolidate)
//                    {
//                        srecord = agmM.ConsolidateRequest(companyinfo, multipleEntry);
//                    }
//                    else
//                    {
//                        var consolidatedvalue = multipleEntry.Where(s => s.Consolidated == true).Select(s => s.ConsolidatedValue).FirstOrDefault();

//                        srecord = agmM.GetConsolidatedAccount(companyinfo, consolidatedvalue);
//                    }

//                    var cshareholder = db.Present.FirstOrDefault(u => u.AGMID == UniqueAGMId && u.ShareholderNum == srecord.ShareholderNum);

//                    srecord.Present = true;
//                    PresentModel present = new PresentModel();
//                    present.Name = srecord.Name;
//                    present.Company = srecord.Company;
//                    present.Address = srecord.Address;
//                    present.admitSource = "Self";
//                    present.ShareholderNum = srecord.ShareholderNum;
//                    present.Holding = srecord.Holding;
//                    present.AGMID = UniqueAGMId;
//                    present.PercentageHolding = srecord.PercentageHolding;
//                    present.present = true;
//                    present.proxy = false;
//                    present.Year = currentYear;
//                    present.PresentTime = DateTime.Now;
//                    present.Timestamp = DateTime.Now.TimeOfDay;
//                    present.emailAddress = srecord.emailAddress;
//                    if (AgmEvent.StopAdmittance || checkIfAnyAccountIsProxy)
//                    {
//                        present.PermitPoll = 0;
//                    }
//                    else
//                    {
//                        present.PermitPoll = 1;
//                    }
//                    if (!String.IsNullOrEmpty(srecord.PhoneNumber))
//                    {
//                        if (srecord.PhoneNumber.StartsWith("234"))
//                        {
//                            present.PhoneNumber = srecord.PhoneNumber;
//                        }
//                        else if (srecord.PhoneNumber.StartsWith("0"))
//                        {
//                            var number = double.Parse(srecord.PhoneNumber);
//                            present.PhoneNumber = "234" + number.ToString();

//                        }

//                    }
//                    present.Clikapad = srecord.Clikapad;
//                    if (cshareholder == null)
//                    {
//                        db.Present.Add(present);
//                        srecord.Date = DateTime.Today.ToString();
//                        db.Entry(srecord).State = EntityState.Modified;
//                    }
//                    db.SaveChanges();
//                    Functions.PresentCount(UniqueAGMId, true);
//                    return Task.FromResult<string>("Accounts Consolidated.");
//                }
//                else if (multipleEntry.Count() > 1 && data.status == false)
//                {
//                    //shareholder.Present = data.status;

//                    var presentEntry = db.Present.FirstOrDefault(r => r.AGMID == UniqueAGMId && r.ShareholderNum == shareholder.ShareholderNum);
//                    //emailEntry.TakePoll = data.status;

//                    var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == shareholder.ShareholderNum).ToArray();
//                    if (result.Any())
//                    {
//                        db.Result.RemoveRange(result);
//                    }
//                    //for (int i = 0; i < result.Length; i++)
//                    //{
//                    //    db.Result.Remove(result[i]);

//                    //}
//                    if (presentEntry != null)
//                    {

//                        db.Present.Remove(presentEntry);
//                        count++;
//                    }

//                    //checkifConsolidatedAccount
//                    string message = "";

//                    var checkifConsolidate = multipleEntry.Any(c => c.Consolidated == true);
//                    if (checkifConsolidate)
//                    {
//                        //Unconsolidate Account
//                        message = agmM.UnConsolidateAsync(companyinfo, UniqueAGMId, shareholder);
//                    }

//                    db.SaveChanges();

//                    return Task.FromResult<string>(message);
//                }
//                else if (multipleEntry.Count() == 1)
//                {
//                    var emailentry = multipleEntry.First();
//                    var entry = db.BarcodeStore.Find(emailentry.Id);

//                    var present = db.Present.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == entry.ShareholderNum);
//                    if (data.status == true && entry.PresentByProxy == true)
//                    {
//                        entry.Present = false;
//                    }
//                    else if (data.status == false)
//                    {
//                        entry.Present = data.status;
//                        entry.TakePoll = data.status;
//                        var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == entry.ShareholderNum).ToArray();
//                        if (result.Any())
//                        {
//                            db.Result.RemoveRange(result);
//                        }
//                        //for (int i = 0; i < result.Length; i++)
//                        //{
//                        //    db.Result.Remove(result[i]);

//                        //}
//                        if (present.Any())
//                        {
//                            db.Present.RemoveRange(present);
//                            //foreach(var item in present)
//                            //{ db.Present.Remove(item);
//                            //}

//                        }

//                    }
//                    else
//                    {
//                        entry.Present = data.status;
//                        if (!present.Any())
//                        {

//                            string phonenumber = "";
//                            if (!String.IsNullOrEmpty(entry.PhoneNumber))
//                            {
//                                char[] arr = entry.PhoneNumber.Where(c => (char.IsLetterOrDigit(c))).ToArray();

//                                entry.PhoneNumber = new string(arr);
//                                if (entry.PhoneNumber.StartsWith("234"))
//                                {
//                                    phonenumber = entry.PhoneNumber;
//                                }
//                                else if (entry.PhoneNumber.StartsWith("0"))
//                                {

//                                    double number = double.Parse(entry.PhoneNumber);
//                                    phonenumber = "234" + number.ToString();

//                                }

//                            }
//                            PresentModel model = new PresentModel();
//                            model.Name = entry.Name;
//                            model.Address = entry.Address;
//                            model.Company = entry.Company;
//                            model.admitSource = "Self";
//                            model.PermitPoll = 1;
//                            model.Year = currentYear;
//                            model.AGMID = UniqueAGMId;
//                            model.ShareholderNum = entry.ShareholderNum;
//                            model.ParentNumber = entry.ParentAccountNumber;
//                            model.Holding = entry.Holding;
//                            model.PhoneNumber = phonenumber;
//                            if (!String.IsNullOrEmpty(entry.emailAddress))
//                            {
//                                model.emailAddress = entry.emailAddress;
//                            }

//                            model.PercentageHolding = entry.PercentageHolding;
//                            model.present = true;
//                            model.proxy = false;
//                            model.PresentTime = DateTime.Now;
//                            if (!String.IsNullOrEmpty(entry.Clikapad))
//                            {
//                                model.Clikapad = entry.Clikapad;
//                                model.GivenClikapad = true;
//                            }
//                            model.Timestamp = DateTime.Now.TimeOfDay;
//                            db.Present.Add(model);
//                        }
//                    }
//                    //db.Entry(entry).State = EntityState.Modified;
//                    db.SaveChanges();
//                    Functions.PresentCount(UniqueAGMId, true);

//                    return Task.FromResult<string>("Success");

//                }
//                else
//                {
//                    return Task.FromResult<string>("Empty");
//                }
//            }
//            else
//            {
//                var present = db.Present.SingleOrDefault(r => r.AGMID == UniqueAGMId && r.ShareholderNum == shareholder.ShareholderNum);
//                if (data.status == true && shareholder.PresentByProxy == true)
//                {
//                    shareholder.Present = false;
//                }
//                else if (data.status == false)
//                {
//                    shareholder.Present = data.status;
//                    shareholder.TakePoll = data.status;
//                    var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == shareholder.ShareholderNum).ToArray();
//                    if (result.Any())
//                    {
//                        db.Result.RemoveRange(result);
//                    }
//                    //for (int i = 0; i < result.Length; i++)
//                    //{
//                    //    db.Result.Remove(result[i]);

//                    //}
//                    if (present != null)
//                    {
//                        db.Present.Remove(present);
//                    }

//                }
//                else
//                {
//                    shareholder.Present = data.status;
//                    if (present != null)
//                    {
//                        //Do Nothing
//                    }
//                    else
//                    {

//                        string phonenumber = "";
//                        if (!String.IsNullOrEmpty(shareholder.PhoneNumber))
//                        {
//                            char[] arr = shareholder.PhoneNumber.Where(c => (char.IsLetterOrDigit(c))).ToArray();

//                            shareholder.PhoneNumber = new string(arr);
//                            if (shareholder.PhoneNumber.StartsWith("234"))
//                            {
//                                phonenumber = shareholder.PhoneNumber;
//                            }
//                            else if (shareholder.PhoneNumber.StartsWith("0"))
//                            {

//                                double number = double.Parse(shareholder.PhoneNumber);
//                                phonenumber = "234" + number.ToString();

//                            }

//                        }
//                        PresentModel model = new PresentModel();
//                        model.Name = shareholder.Name;
//                        model.Address = shareholder.Address;
//                        model.Company = shareholder.Company;
//                        model.admitSource = "Self";
//                        model.PermitPoll = 1;
//                        model.Year = currentYear;
//                        model.AGMID = UniqueAGMId;
//                        model.ShareholderNum = shareholder.ShareholderNum;
//                        model.ParentNumber = shareholder.ParentAccountNumber;
//                        model.Holding = shareholder.Holding;
//                        model.PhoneNumber = phonenumber;
//                        if (!String.IsNullOrEmpty(shareholder.emailAddress))
//                        {
//                            model.emailAddress = shareholder.emailAddress;
//                        }

//                        model.PercentageHolding = shareholder.PercentageHolding;
//                        model.present = true;
//                        model.proxy = false;
//                        model.PresentTime = DateTime.Now;
//                        model.Timestamp = DateTime.Now.TimeOfDay;
//                        db.Present.Add(model);
//                    }
//                }
//                //db.Entry(shareholder).State = EntityState.Modified;
//                db.SaveChanges();
//                Functions.PresentCount(UniqueAGMId, true);
//                return Task.FromResult<string>("Success");
//            }


//        }




//        // public async Task<ActionResult> RegularizeMultiplePhoneNumber(int id, string phone)
//        // {

//        //     var response = await RegularizeMultiplePhoneNumberAsync(id, phone);

//        //     return Json(new { value = response }, JsonRequestBehavior.AllowGet);
//        // }


//        private Task<string> RegularizeMultiplePhoneNumberAsync(int id, string phone)
//        {
//            var companyinfo = ua.GetUserCompanyInfo();
//            var UniqueAGMId = ua.RetrieveAGMUniqueID();

//            var shareholder = db.BarcodeStore.Find(id);
//            var AllEntries = db.BarcodeStore.Where(s => s.Company == companyinfo && s.emailAddress == shareholder.emailAddress);
//            foreach (var item in AllEntries)
//            {
//                item.PhoneNumber = phone;
//            }
//            db.SaveChanges();
//            return Task.FromResult<string>(phone);
//        }

//        // public async Task<ActionResult> Resolution(int id)
//        // {
//        //     var response = await ResolutionAsync(id);

//        //     return View(response);
//        // }


//        public Task<ResolutionModel> ResolutionAsync(int id)
//        {
//            var shareholder = db.BarcodeStore.Find(id);
//            var companyinfo = ua.GetUserCompanyInfo();
//            var UniqueAGMId = ua.RetrieveAGMUniqueID();

//            var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);

//            ResolutionModel model = new ResolutionModel
//            {
//                Name = shareholder.Name,
//                shareholdernum = shareholder.ShareholderNum,
//                NewHolding = shareholder.Holding.ToString(),
//                resolutionstatus = shareholder.resolution,
//                splitvalue = shareholder.ShareholderNum,
//                shareholder_Id = shareholder.Id,
//                abstainBtnChoice = abstainbtnchoice,
//                Question = db.Question.Where(q => q.AGMID == UniqueAGMId).ToList()



//            };

//            return Task.FromResult<ResolutionModel>(model);
//        }

//        // public async Task<ActionResult> ResolutionIndex(int id)
//        // {
//        //     var response = await ResolutionIndexAsync(id);

//        //     return PartialView(response);

//        // }


//        //private Task<ResolutionModel> ResolutionIndexAsync(int id)
//        //{
//        //    var shareholder = db.BarcodeStore.Find(id);
//        //    var companyinfo = ua.GetUserCompanyInfo();
//        //    var UniqueAGMId = ua.RetrieveAGMUniqueID();

//        //    var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//        //    ResolutionModel model = new ResolutionModel
//        //    {
//        //        Name = shareholder.Name,
//        //        shareholdernum = shareholder.ShareholderNum,
//        //        NewHolding = shareholder.Holding.ToString(),
//        //        resolutionstatus = shareholder.resolution,
//        //        splitvalue = shareholder.ShareholderNum,
//        //        shareholder_Id = shareholder.Id,
//        //        ParentNumber = shareholder.ParentAccountNumber,
//        //        abstainBtnChoice = abstainbtnchoice,
//        //        Question = db.Question.Where(q => q.AGMID == UniqueAGMId).ToList()



//        //    };

//        //    return Task.FromResult<ResolutionModel>(model);
//        //}

//        //// public async Task<ActionResult> RefreshResolution(Int64 id)
//        //// {
//        ////     var response = await RefreshResolutionAsync(id);

//        ////     return PartialView(response);
//        //// }

//        //private Task<ResolutionModel> RefreshResolutionAsync(Int64 id)
//        //{
//        //    var shareholdernum = id;
//        //    var companyinfo = ua.GetUserCompanyInfo();
//        //    var UniqueAGMId = ua.RetrieveAGMUniqueID();

//        //    var shareholder = db.BarcodeStore.SingleOrDefault(b => b.Company == companyinfo && b.ShareholderNum == shareholdernum);
//        //    ResolutionModel model = new ResolutionModel
//        //    {
//        //        Name = shareholder.Name,
//        //        shareholdernum = shareholder.ShareholderNum,
//        //        NewHolding = shareholder.Holding.ToString(),
//        //        resolutionstatus = shareholder.resolution,
//        //        splitvalue = shareholder.ShareholderNum,
//        //        shareholder_Id = shareholder.Id,

//        //        Question = db.Question.ToList()



//        //    };

//        //    return Task.FromResult<ResolutionModel>(model);
//        //}

//        // [HttpPost]
//        // public async Task<string> Resolutionstatus(int id, SplitDeletePostModel post)
//        // {
//        //     var response = await ResolutionstatusAsync(id, post);

//        //     return response;

//        // }


//        private Task<string> ResolutionstatusAsync(int id, SplitDeletePostModel post)
//        {
//            try
//            {
//                var companyinfo = ua.GetUserCompanyInfo();
//                var UniqueAGMId = ua.RetrieveAGMUniqueID();

//                var shareholder = db.BarcodeStore.Find(id);
//                var presentmodel = db.Present.SingleOrDefault(r => r.AGMID == UniqueAGMId && r.ShareholderNum == shareholder.ShareholderNum);
//                var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.splitValue == post.splitvalue && r.Name == post.name).ToArray();

//                if (post.status == false && shareholder.split == false && shareholder.Present == false)
//                {
//                    shareholder.PresentByProxy = false;
//                    shareholder.TakePoll = false;
//                    shareholder.resolution = false;
//                    for (int i = 0; i < result.Length; i++)
//                    {
//                        db.Result.Remove(result[i]);
//                    }

//                    db.Present.Remove(presentmodel);
//                }
//                else if (post.status == false && shareholder.split == true || shareholder.Present == true)
//                {
//                    shareholder.resolution = false;

//                }
//                else if (post.status == true && shareholder.split == true || shareholder.Present == true)
//                {

//                    //TempData["Message"] = "Account may have been splited already or marked Present";
//                    //return RedirectToAction("Resolution", new { id = id });
//                    return Task.FromResult<string>("Slitted already or marked present");
//                }
//                else
//                {
//                    shareholder.PresentByProxy = true;
//                    shareholder.TakePoll = false;
//                    shareholder.resolution = post.status;
//                    if (presentmodel != null)
//                    {
//                        //Do Nothing
//                    }
//                    else
//                    {
//                        string phonenumber = null;
//                        if (!String.IsNullOrEmpty(shareholder.PhoneNumber))
//                        {
//                            double number = double.Parse(shareholder.PhoneNumber);
//                            phonenumber = "234" + number.ToString();
//                        }
//                        PresentModel proxy = new PresentModel();
//                        proxy.Name = shareholder.Name;
//                        proxy.Address = shareholder.Address;
//                        proxy.Company = shareholder.Company;
//                        proxy.admitSource = "Proxy";
//                        proxy.PermitPoll = 1;
//                        proxy.Year = currentYear;
//                        proxy.AGMID = UniqueAGMId;
//                        proxy.ShareholderNum = shareholder.ShareholderNum;
//                        proxy.Holding = shareholder.Holding;
//                        if (!String.IsNullOrEmpty(shareholder.emailAddress))
//                        {
//                            proxy.emailAddress = string.Format("proxy{0}", shareholder.emailAddress);
//                        }
//                        proxy.PhoneNumber = phonenumber;
//                        proxy.PercentageHolding = shareholder.PercentageHolding;
//                        proxy.proxy = true;
//                        proxy.present = false;
//                        proxy.PresentTime = DateTime.Now;
//                        proxy.Timestamp = DateTime.Now.TimeOfDay;
//                        db.Present.Add(proxy);
//                        shareholder.emailAddress = string.Format("proxy{0}", shareholder.emailAddress);
//                    }
//                }

//                db.Entry(shareholder).State = EntityState.Modified;
//                db.SaveChanges();
//                Functions.PresentCount(UniqueAGMId, true);
//                HttpResponseMessage response = new HttpResponseMessage();
//                response.StatusCode = HttpStatusCode.OK;
//                return Task.FromResult<string>("Success");

//                //return RedirectToAction("Resolution", new { id = id });
//            }
//            catch (Exception e)
//            {
//                return Task.FromResult<string>("Failed");
//            }
//        }


//        // [HttpPost]
//        // public async Task<ActionResult> ResolutionResult(int id, SplitResultPostModel post)
//        // {
//        //     var response = await ResolutionResultAsync(id, post);

//        //     return PartialView(response);
//        // }




//        private Task<SplitResultModel> ResolutionResultAsync(int id, SplitResultPostModel post)
//        {
//            try
//            {
//                var companyinfo = ua.GetUserCompanyInfo();
//                var UniqueAGMId = ua.RetrieveAGMUniqueID();

//                var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);
//                var shareholder = db.BarcodeStore.Find(id);

//                var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == shareholder.ShareholderNum).ToArray();
//                var question = db.Question.Where(q => q.AGMID == UniqueAGMId);
//                SplitResultModel model = new SplitResultModel
//                {
//                    Result = result,
//                    question = question,
//                    abstainBtnChoice = abstainbtnchoice
//                };

//                return Task.FromResult<SplitResultModel>(model);
//            }
//            catch
//            {
//                SplitResultModel Model = new SplitResultModel();
//                Model.Result = null;
//                Model.question = null;

//                return Task.FromResult<SplitResultModel>(new SplitResultModel());
//            }

//        }

//        // [HttpPost]
//        // public async Task<ActionResult> PresentResult(SplitResultPostModel post)
//        // {
//        //     var response = await PresentResultAsync(post);

//        //     return PartialView(response);

//        // }


//        public Task<SplitResultModel> PresentResultAsync(SplitResultPostModel post)
//        {
//            try
//            {
//                var companyinfo = ua.GetUserCompanyInfo();
//                var UniqueAGMId = ua.RetrieveAGMUniqueID();

//                var abstainbtnchoice = ua.GetAbstainBtnChoice(UniqueAGMId);

//                var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == post.value).ToArray();
//                var question = db.Question.Where(q => q.Company == companyinfo && q.AGMID == UniqueAGMId);
//                SplitResultModel model = new SplitResultModel
//                {
//                    Result = result,
//                    question = question,
//                    abstainBtnChoice = abstainbtnchoice
//                };

//                return Task.FromResult<SplitResultModel>(model);
//            }
//            catch
//            {
//                return Task.FromResult<SplitResultModel>(new SplitResultModel());
//            }

//        }


//        // public async Task<double> TotalHolding(int id)
//        // {
//        //     var returnresponse = await TotalHoldingAsync(id);

//        //     return returnresponse;
//        // }

//        public Task<double> TotalHoldingAsync(int id)
//        {
//            try
//            {

//                var settingmodel = db.Settings.Find(id);
//                if (settingmodel != null)
//                {
//                    var companyinfo = settingmodel.CompanyName;
//                    var agmid = settingmodel.AGMID;
//                    Double TotalShareholding = 0;

//                    //var companyinfo = ua.GetUserCompanyInfo();
//                    //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//                    string connStr = DatabaseManager.GetConnectionString();

//                    SqlConnection conn =
//                            new SqlConnection(connStr);
//                    try
//                    {
//                        SettingsModel setting;
//                        var response = 0.0;
//                        var CheckBarcodeModelStatus = db.BarcodeStore.Any();
//                        if (CheckBarcodeModelStatus)
//                        {
//                            string query2 = "UPDATE SettingsModels SET ShareHolding = (SELECT ROUND(SUM(Holding),2) FROM BarcodeModels WHERE Company='" + companyinfo + "') WHERE CompanyName =  '" + companyinfo + "' AND AGMID = '" + agmid + "'";
//                            conn.Open();
//                            SqlCommand cmd = new SqlCommand(query2, conn);
//                            cmd.ExecuteNonQuery();
//                            conn.Close();
//                            setting = db.Settings.Find(id);
//                            if (setting != null)
//                            {
//                                response = setting.ShareHolding;
//                            }

//                            ////TempData["Message"] = "Percentage(%) Holding Calculated Successfully!";
//                            return System.Threading.Tasks.Task.FromResult<double>(response);
//                        }
//                        //setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
//                        return Task.FromResult<double>(response);


//                    }
//                    catch (Exception e)
//                    {
//                        var response = 0.0;
//                        return Task.FromResult<double>(response);
//                    }
//                }
//                var response2 = 0.0;
//                return Task.FromResult<double>(response2);
//            }
//            catch (Exception e)
//            {

//                ////TempData["Message1"] = e;
//                var response3 = 0.0;
//                return Task.FromResult<double>(response3);
//                //return "Undetermined!";
//            }
//        }

//        // public async Task<bool> CaculateHolding(int id)
//        // {
//        //     var response = await CaculateHoldingAsync(id);

//        //     return response;
//        // }

//        public Task<bool> CaculateHoldingAsync(int id)
//        {
//            try
//            {
//                //var companyinfo = ua.GetUserCompanyInfo();
//                //var UniqueAGMId = ua.RetrieveAGMUniqueID();

//                var setting = db.Settings.Find(id);
//                var companyinfo = setting.CompanyName;
//                //var setting = db.Settings.FirstOrDefault(s=>s.AGMID==UniqueAGMId);
//                if (setting == null || setting.ShareHolding == 0)
//                {
//                    // //TempData["Message1"] = "Please value for Total Holding";
//                    //return "Enter a Value for Total Holding!";
//                    return Task.FromResult<bool>(false);
//                }

//                var totalHolding = setting.ShareHolding;
//                double TotalShareholding = totalHolding;

//                string connStr = DatabaseManager.GetConnectionString();

//                SqlConnection conn =
//                        new SqlConnection(connStr);
//                //con1 = new SqlConnection(conn);
//                string query2 = "UPDATE BarcodeModels WHERE Company =  '" + companyinfo + "' SET PercentageHolding = (Holding /'" + totalHolding + "') * 100";
//                conn.Open();
//                SqlCommand cmd = new SqlCommand(query2, conn);
//                cmd.ExecuteNonQuery();
//                conn.Close();


//                //TempData["Message"] = "Percentage(%) Holding Calculated Successfully!";
//                //return "Successful";
//                return Task.FromResult<bool>(true);
//            }
//            catch (Exception e)
//            {

//                //TempData["Message1"] = e;
//                return Task.FromResult<bool>(false);
//            }
//        }

//        // public async Task<ActionResult> RegularizePhoneNumber(int id, string phone)
//        // {
//        //     var response = await RegularizePhoneNumberAsync(id, phone);

//        //     return Json(new { value = phone }, JsonRequestBehavior.AllowGet);
//        // }

//        public Task<string> RegularizePhoneNumberAsync(int id, string phone)
//        {
//            var shareholder = db.BarcodeStore.Find(id);
//            shareholder.PhoneNumber = phone;
//            db.SaveChanges();
//            return Task.FromResult<string>(phone);
//        }


//        // public async Task<ActionResult> RegularizeEmailAddress(int id, string email)
//        // {
//        //     var response = await RegularizeEmailAddressAsync(id, email);

//        //     return Json(new { value = email }, JsonRequestBehavior.AllowGet);
//        // }


//        public Task<string> RegularizeEmailAddressAsync(int id, string email)
//        {
//            var shareholder = db.BarcodeStore.Find(id);
//            shareholder.emailAddress = email;
//            db.SaveChanges();
//            return Task.FromResult<string>(email);
//        }

//        // public async Task<ActionResult> PresentAll()
//        // {
//        //     var response = await PresentAllAsync();

//        //     return RedirectToAction("Index");
//        // }



//        public Task<string> PresentAllAsync()
//        {
//            try
//            {
//                var companyinfo = ua.GetUserCompanyInfo();
//                var UniqueAGMId = ua.RetrieveAGMUniqueID();

//                var shareholders = db.BarcodeStore.Where(b => b.Company == companyinfo).ToList();
//                var Total = shareholders.Count();

//                for (int i = 0; i < shareholders.Count; i++)
//                {
//                    string phonenumber = null;
//                    if (!String.IsNullOrEmpty(shareholders[i].PhoneNumber))
//                    {
//                        double number = double.Parse(shareholders[i].PhoneNumber);
//                        phonenumber = "234" + number.ToString();
//                    }
//                    shareholders[i].Present = true;
//                    PresentModel present = new PresentModel();
//                    present.Name = shareholders[i].Name;
//                    present.Address = shareholders[i].Address;
//                    present.Company = shareholders[i].Company;
//                    present.Year = currentYear;
//                    present.AGMID = UniqueAGMId;
//                    present.ShareholderNum = shareholders[i].ShareholderNum;
//                    present.PhoneNumber = phonenumber;
//                    if (!String.IsNullOrEmpty(shareholders[i].emailAddress))
//                    {
//                        present.emailAddress = shareholders[i].emailAddress;
//                    }
//                    present.Holding = shareholders[i].Holding;
//                    present.PercentageHolding = shareholders[i].PercentageHolding;
//                    present.present = true;
//                    present.proxy = false;
//                    present.PermitPoll = 1;
//                    present.PresentTime = DateTime.Now;
//                    present.Timestamp = DateTime.Now.TimeOfDay;
//                    db.Present.Add(present);
//                    db.Entry(shareholders[i]).State = EntityState.Modified;
//                    db.SaveChanges();
//                    //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
//                    Functions.SendProgress("Process in progress...", i, Total);
//                }


//                // //TempData["Message"] = "Successful!";
//                return Task.FromResult<string>("success");
//            }
//            catch (Exception e)
//            {

//                // //TempData["Message1"] = e;
//                return Task.FromResult<string>("failed");
//            }
//        }


//        // public ActionResult UserDetails()
//        // {
//        //     return View();
//        // }

//        // [HttpPost]
//        // public async Task<ActionResult> ShareHolderDetails(SearchModel find)
//        // {
//        //     var response = await ShareHolderDetailsAsync(find);

//        //     return PartialView(response);

//        // }


//        private Task<BarcodeViewModel> ShareHolderDetailsAsync(SearchModel find)
//        {

//            if (find.search == null)
//            {
//                var model = new BarcodeViewModel();
//                model.Empty = "ENTER A SEARCH VALUE";

//                return Task.FromResult<BarcodeViewModel>(model);
//            }
//            try
//            {
//                var companyinfo = ua.GetUserCompanyInfo();
//                var UniqueAGMId = ua.RetrieveAGMUniqueID();

//                var barcode = db.BarcodeStore.SingleOrDefault(u => u.Company == companyinfo && u.Barcode == find.search);
//                if (barcode != null)
//                {
//                    BarcodeViewModel model = new BarcodeViewModel
//                    {
//                        id = barcode.Id,
//                        Name = barcode.Name,
//                        Holding = barcode.Holding,
//                        PercentageHolding = barcode.PercentageHolding,
//                        Barcode = barcode.Barcode,
//                        BarcodeImage = barcode.BarcodeImage,
//                        Message = "SHAREHOLDER VERIFIED OK",
//                        ImageUrl = barcode.BarcodeImage != null ? "data:image/jpg;base64," +
//                            Convert.ToBase64String((byte[])barcode.BarcodeImage) : ""

//                    };


//                    var setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
//                    if (setting != null)
//                    {
//                        // ViewBag.logo = setting.Image != null ? "data:image/jpg;base64," +
//                    //    Convert.ToBase64String((byte[])setting.Image) : "";
//                        // ViewBag.Company = setting.CompanyName;
//                    }
//                    db.SaveChanges();
//                    return Task.FromResult<BarcodeViewModel>(model);
//                }

//                var model1 = new BarcodeViewModel();
//                model1.Void = find.search + "-" + "BARCODE NUMBER DOESNOT EXIST.";
//                return Task.FromResult<BarcodeViewModel>(model1);
//            }
//            catch
//            {
//                var model2 = new BarcodeViewModel();
//                model2.Message = "COULD NOT VERIFY SHAREHOLDER";
//                return Task.FromResult<BarcodeViewModel>(model2);
//            }
//        }


//        // [HttpPost]
//        // public async Task<ActionResult> search(SearchModel find, int page = 1)
//        // {
//        //     var response = await searchAsync(find, page);

//        //     return PartialView(response);
//        // }


//        private Task<BarcodeListViewModel> searchAsync(SearchModel find, int page = 1)
//        {
//            var companyinfo = ua.GetUserCompanyInfo();
//            var UniqueAGMId = ua.RetrieveAGMUniqueID();

//            var barcode = db.BarcodeStore.Where(s => s.Company == companyinfo && s.Name.Contains(find.search))
//                                .OrderBy(s => s.Id)
//                .Skip((page - 1) * PageSize)
//                .Take(PageSize).ToArray();
//            List<BarcodeModel> model = new List<BarcodeModel>();

//            for (int i = 0; i < barcode.Length; i++)
//            {

//                var p = barcode[i].BarcodeImage;
//                BarcodeModel barcodemodel = new BarcodeModel()
//                {
//                    Id = barcode[i].Id,
//                    Name = barcode[i].Name.ToUpper().ToString(),

//                    Barcode = barcode[i].Barcode.ToString(),
//                    ImageUrl = barcode[i].BarcodeImage != null ? "data:image/jpg;base64," +
//                        Convert.ToBase64String((byte[])barcode[i].BarcodeImage) : ""
//                };

//                model.Add(barcodemodel);
//            }
//            BarcodeListViewModel displaymodel = new BarcodeListViewModel
//            {
//                barcodes = model,
//                PagingInfo = new PagingInfo
//                {
//                    CurrentPage = page,
//                    ItemsPerPage = PageSize,
//                    TotalItems = model.Count()
//                }
//            };

//            return Task.FromResult<BarcodeListViewModel>(displaymodel);

//        }
//        //
//        // GET: /BarcodeLib/Details/5
//        // public async Task<ActionResult> Details(int id)
//        // {
//        //     var response = await DetailsAsync(id);

//        //     return PartialView(response);
//        // }


//        public Task<BarcodeViewModel> DetailsAsync(int id)
//        {
//            try
//            {
//                var barcode = db.BarcodeStore.Find(id);
//                var companyinfo = ua.GetUserCompanyInfo();
//                var UniqueAGMId = ua.RetrieveAGMUniqueID();

//                BarcodeViewModel model = new BarcodeViewModel();

//                model.id = barcode.Id;
//                model.Name = barcode.Name;
//                model.Holding = barcode.Holding;
//                model.PercentageHolding = barcode.PercentageHolding;
//                model.ShareholderNum = barcode.ShareholderNum;
//                model.Barcode = barcode.Barcode;
//                model.BarcodeImage = barcode.BarcodeImage;
//                model.ImageUrl = barcode.BarcodeImage != null ? "data:image/jpg;base64," +
//                    Convert.ToBase64String((byte[])barcode.BarcodeImage) : "";


//                var setting = db.Settings.Where(s => s.AGMID == UniqueAGMId).ToArray();
//                if (setting.Length != 0)
//                {
//                    model.logo = setting[0].Image != null ? "data:image/jpg;base64," +
//                       Convert.ToBase64String((byte[])setting[0].Image) : "";
//                    model.Company = setting[0].CompanyName;
//                }
//                ////TempData["Message"] = "Shareholder Verified OK.";
//                return Task.FromResult<BarcodeViewModel>(model);
//            }
//            catch
//            {
//                var model = new BarcodeViewModel();
//                model.Void = "ERROR ACCESSING BARCODE.";
//                return Task.FromResult<BarcodeViewModel>(new BarcodeViewModel());
//            }

//        }



//        // public async Task<ActionResult> Reset()
//        // {
//        //     var response = await ResetAsync();

//        //     return RedirectToAction("Index");
//        // }


//        private Task<string> ResetAsync()
//        {
//            try
//            {
//                var companyinfo = ua.GetUserCompanyInfo();
//                var UniqueAGMId = ua.RetrieveAGMUniqueID();

//                var shareholders = db.BarcodeStore.Where(b => b.Company == companyinfo).ToArray();
//                var results = db.Result.Where(r => r.Company == companyinfo && r.Year == currentYear).ToArray();
//                var present = db.Present.Where(r => r.Company == companyinfo && r.AGMID == UniqueAGMId).ToArray();
//                for (int i = 0; i < shareholders.Length; i++)
//                {
//                    shareholders[i].Present = false;
//                    shareholders[i].PresentByProxy = false;
//                    shareholders[i].split = false;
//                    shareholders[i].resolution = false;
//                    shareholders[i].TakePoll = false;
//                    db.Entry(shareholders[i]).State = EntityState.Modified;
//                    db.Present.Remove(present[i]);

//                    //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
//                    Functions.SendProgress("Process in progress...", i, shareholders.Length);
//                }
//                for (int j = 0; j < results.Length; j++)
//                {
//                    db.Result.Remove(results[j]);
//                    //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
//                    Functions.SendProgress("Process in progress...", j, results.Length);
//                }
//                db.SaveChanges();

//                return Task.FromResult<string>("success");
//            }
//            catch
//            {

//                return Task.FromResult<string>("failed");
//            }
//        }


//        // GET: /BarcodeLib/Create

//        // public ActionResult Create()
//        // {
//        //     return View();
//        // }

//        //
//        // POST: /BarcodeLib/Create

//        // [HttpPost]




//        /// <summary>
//        /// WILL GET BACK HERE LATER

//        // public async Task Create(BarcodeModelView model)
//        // {
//        //     try
//        //     {

//        //         // TODO: Add insert logic here
//        //         if (model.FirstName == null)
//        //         {
//        //             // return RedirectToAction("UserDetails", "BarcodeLib");
//        //             throw new Exception("FIRSTNAME  CANNOT BE NULL");
//        //         }
//        //         if (model.LastName == null)
//        //         {
//        //             // return RedirectToAction("UserDetails", "BarcodeLib");
//        //             throw new Exception("LASTNAME CANNOT BE NULL");
//        //         }

//        //         barcodecs objbar = new barcodecs();
//        //         // string numberToEncode = objbar.generateBarcode();
//        //         string numberToEncode = model.ShareholderNum.ToString();
//        //         int W = Convert.ToInt32(200);
//        //         int H = Convert.ToInt32(80);
//        //         BarcodeLib.Barcode b = new BarcodeLib.Barcode(numberToEncode);
//        //         b.Alignment = BarcodeLib.AlignmentPositions.LEFT;
//        //         BarcodeLib.TYPE type = BarcodeLib.TYPE.UNSPECIFIED;
//        //         type = BarcodeLib.TYPE.CODE128;

//        //         b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
//        //         b.IncludeLabel = true;
//        //         b.Encode(type,
//        //       b.RawData.ToUpper(), Color.Black, Color.White, W, H);
//        //         using (MemoryStream Mmst = new MemoryStream())
//        //         {
//        //             b.SaveImage(Mmst, BarcodeLib.SaveTypes.JPG);
//        //             BarCode = Mmst.GetBuffer();

//        //         }
//        //         BarcodeModel objprod = new BarcodeModel()
//        //         {
//        //             Name = model.FirstName,
//        //             ShareholderNum = model.ShareholderNum,
//        //             Barcode = numberToEncode,
//        //             BarcodeImage = BarCode
//        //         };
//        //         db.BarcodeStore.Add(objprod);
//        //         db.SaveChanges();
//        //         // return RedirectToAction("UserDetails", "BarcodeLib");
//        //     }
//        //     catch
//        //     {
//        //         throw;
//        //     }
//        // }




//        //
//        // GET: /BarcodeLib/Edit/5

//        // public ActionResult Edit(int id)
//        //{
//        //   var model = db.BarcodeStore.Find(id);
//        //     return PartialView(model);
//        // }

//        // //
//        // // POST: /BarcodeLib/Edit/5

//        // [HttpPost]
//        // public ActionResult Edit(int id, BarcodeModel collection)
//        // {
//        //     try
//        //     {
//        //         // TODO: Add update logic here
//        //         var model = db.BarcodeStore.Find(collection.Id);
//        //         model.PhoneNumber= collection.PhoneNumber;
//        //         model.emailAddress = collection.emailAddress;
//        //         db.Entry(model).State = EntityState.Modified;
//        //         db.SaveChanges();
//        //         //TempData["Message"] = "Edited";
//        //         return RedirectToAction("Index","BarcodeLib");
//        //     }
//        //     catch
//        //     {
//        //         //TempData["Message1"] = "Cannot Edit database";
//        //         return RedirectToAction("Index","BarcodeLib");
//        //     }
//        // }

//        //
//        // GET: /BarcodeLib/Delete/5
//        // public async Task<string> Delete(int id)
//        // {
//        //     var response = await DeleteAsync(id);

//        //     return response;
//        // }


//        private Task<string> DeleteAsync(int id)
//        {
//            try
//            {
//                var delete = db.BarcodeStore.Find(id);
//                db.BarcodeStore.Remove(delete);
//                db.SaveChanges();
//                var Message = " Item Deleted";
//                return Task.FromResult<string>(Message);
//            }
//            catch
//            {
//                var Message = " Couldn't delete item";
//                return Task.FromResult<string>(Message);
//            }


//        }


//        public async Task<string> MarkProxy(PostSelectModel post)
//        {
//            var response = await MarkProxyAsync(post.ids);

//            return response;
//        }


//        public Task<string> MarkProxyAsync(string val)
//        {
//            var companyinfo = ua.GetUserCompanyInfo();
//            var UniqueAGMId = ua.RetrieveAGMUniqueID();


//            var jvalues = JsonConvert.DeserializeObject(val);
//            JArray a = JArray.Parse(val);

//            if (a.Count() > 1)
//            {
//                foreach (var item in a)
//                {
//                    var id = int.Parse(item.ToString());
//                    var proxy = db.BarcodeStore.Find(id);
//                    if (proxy != null)
//                    {
//                        //foreach (var item in proxy)
//                        //{
//                        //var present = db.Present.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == proxy.ShareholderNum);
//                        if (!db.Present.Any(r => r.AGMID == UniqueAGMId && r.ShareholderNum == proxy.ShareholderNum))
//                        {
//                            proxy.PresentByProxy = true;
//                            proxy.NotVerifiable = true;
//                            proxy.Selected = false;
//                            string phonenumber = "";
//                            if (!String.IsNullOrEmpty(proxy.PhoneNumber))
//                            {
//                                char[] arr = proxy.PhoneNumber.Where(c => (char.IsLetterOrDigit(c))).ToArray();

//                                proxy.PhoneNumber = new string(arr);
//                                if (proxy.PhoneNumber.StartsWith("234"))
//                                {
//                                    phonenumber = proxy.PhoneNumber;
//                                }
//                                else if (proxy.PhoneNumber.StartsWith("0"))
//                                {

//                                    double number;
//                                    if (double.TryParse(proxy.PhoneNumber, out number))
//                                    {
//                                        number = double.Parse(proxy.PhoneNumber);
//                                        proxy.PhoneNumber = "234" + number.ToString();
//                                    }
//                                    else
//                                    {
//                                        char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                                        var phonenum = proxy.PhoneNumber.Split(delimiterChars);
//                                        //string phonenumberresult = string.Concat(phonenum);
//                                        if (double.TryParse(phonenum[0], out number))
//                                        {
//                                            number = double.Parse(phonenum[0]);
//                                            proxy.PhoneNumber = "234" + number.ToString();
//                                        }

//                                    }

//                                }

//                            }
//                            PresentModel model = new PresentModel();
//                            model.Name = proxy.Name;
//                            model.Address = proxy.Address;
//                            model.Company = proxy.Company;
//                            model.admitSource = "Proxy";
//                            model.PermitPoll = 1;
//                            model.Year = currentYear;
//                            model.AGMID = UniqueAGMId;
//                            model.ShareholderNum = proxy.ShareholderNum;
//                            model.ParentNumber = proxy.ParentAccountNumber;
//                            model.Holding = proxy.Holding;
//                            model.PhoneNumber = phonenumber;
//                            if (!String.IsNullOrEmpty(proxy.emailAddress))
//                            {
//                                model.emailAddress = string.Format("proxy{0}", proxy.emailAddress);
//                            }

//                            model.PercentageHolding = proxy.PercentageHolding;
//                            model.proxy = true;
//                            model.PresentTime = DateTime.Now;
//                            model.Timestamp = DateTime.Now.TimeOfDay;

//                            //db.Entry(emailEntry).State = EntityState.Modified;
//                            db.Present.Add(model);
//                            proxy.emailAddress = string.Format("proxy{0}", proxy.emailAddress);
//                            var resolutions = db.Question.Where(q => q.AGMID == UniqueAGMId).ToList();
//                            foreach (var resolution in resolutions)
//                            {

//                                TakeResolutionAsync(proxy, resolution.Id, UniqueAGMId);
//                            }
//                        }
//                        //}



//                    }
//                }
//                db.SaveChanges();
//                Functions.PresentCount(UniqueAGMId, true);
//                return Task.FromResult<string>("success");
//            }
//            else if (a.Count == 1)
//            {
//                var id = int.Parse(a[0].ToString());
//                var proxy = db.BarcodeStore.Find(id);
//                if (proxy != null)
//                {
//                    //foreach (var item in proxy)
//                    //{
//                    var present = db.Present.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == proxy.ShareholderNum);
//                    if (!present.Any())
//                    {
//                        proxy.PresentByProxy = true;
//                        proxy.NotVerifiable = true;
//                        proxy.Selected = false;
//                        string phonenumber = "";
//                        if (!String.IsNullOrEmpty(proxy.PhoneNumber))
//                        {
//                            char[] arr = proxy.PhoneNumber.Where(c => (char.IsLetterOrDigit(c))).ToArray();

//                            proxy.PhoneNumber = new string(arr);
//                            if (proxy.PhoneNumber.StartsWith("234"))
//                            {
//                                phonenumber = proxy.PhoneNumber;
//                            }
//                            else if (proxy.PhoneNumber.StartsWith("0"))
//                            {

//                                double number;
//                                if (double.TryParse(proxy.PhoneNumber, out number))
//                                {
//                                    number = double.Parse(proxy.PhoneNumber);
//                                    proxy.PhoneNumber = "234" + number.ToString();
//                                }
//                                else
//                                {
//                                    char[] delimiterChars = { ' ', ',', '.', ':', '-', '\t' };
//                                    var phonenum = proxy.PhoneNumber.Split(delimiterChars);
//                                    //string phonenumberresult = string.Concat(phonenum);
//                                    if (double.TryParse(phonenum[0], out number))
//                                    {
//                                        number = double.Parse(phonenum[0]);
//                                        proxy.PhoneNumber = "234" + number.ToString();
//                                    }

//                                }

//                            }

//                        }
//                        PresentModel model = new PresentModel();
//                        model.Name = proxy.Name;
//                        model.Address = proxy.Address;
//                        model.Company = proxy.Company;
//                        model.admitSource = "Proxy";
//                        model.PermitPoll = 1;
//                        model.Year = currentYear;
//                        model.AGMID = UniqueAGMId;
//                        model.ShareholderNum = proxy.ShareholderNum;
//                        model.ParentNumber = proxy.ParentAccountNumber;
//                        model.Holding = proxy.Holding;
//                        model.PhoneNumber = phonenumber;
//                        if (!String.IsNullOrEmpty(proxy.emailAddress))
//                        {
//                            model.emailAddress = string.Format("proxy{0}", proxy.emailAddress);
//                        }

//                        model.PercentageHolding = proxy.PercentageHolding;
//                        model.proxy = true;
//                        model.PresentTime = DateTime.Now;
//                        model.Timestamp = DateTime.Now.TimeOfDay;

//                        //db.Entry(emailEntry).State = EntityState.Modified;
//                        db.Present.Add(model);
//                        proxy.emailAddress = string.Format("proxy{0}", proxy.emailAddress);
//                        var resolutions = db.Question.Where(q => q.AGMID == UniqueAGMId).ToList();
//                        foreach (var resolution in resolutions)
//                        {

//                            TakeResolutionAsync(proxy, resolution.Id, UniqueAGMId);
//                        }
//                        db.SaveChanges();
//                        Functions.PresentCount(UniqueAGMId, true);
//                        return Task.FromResult<string>("success");
//                    }

//                }
//                return Task.FromResult<string>("failed");
//            }
//            else
//            {
//                return Task.FromResult<string>("failed");
//            }


//        }



//        public async Task<string> UnMarkProxy(PostSelectModel post)
//        {
//            var response = await UnMarkProxyAsync(post.ids);

//            return response;
//        }


//        public Task<string> UnMarkProxyAsync(string val)
//        {
//            try
//            {
//                var companyinfo = ua.GetUserCompanyInfo();
//                var UniqueAGMId = ua.RetrieveAGMUniqueID();


//                var jvalues = JsonConvert.DeserializeObject(val);
//                JArray a = JArray.Parse(val);

//                if (a.Count() > 1)
//                {
//                    foreach (var item in a)
//                    {
//                        var id = int.Parse(item.ToString());
//                        var proxy = db.BarcodeStore.Find(id);
//                        if (proxy != null)
//                        {
//                            var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == proxy.ShareholderNum).ToList();
//                            if (result.Any())
//                            {
//                                db.Result.RemoveRange(result);
//                            }
//                            //for (int i = 0; i < result.Length; i++)
//                            //{
//                            //    db.Result.Remove(result[i]);

//                            //}
//                            var present = db.Present.FirstOrDefault(p => p.AGMID == UniqueAGMId && p.ShareholderNum == proxy.ShareholderNum);
//                            if (present != null)
//                            {
//                                db.Present.Remove(present);
//                            }
//                            //if (present.Any())
//                            //{
//                            //    for (int i = 0; i < present.Length; i++)
//                            //    {
//                            //        db.Present.Remove(present[i]);

//                            //    }

//                            //}
//                            proxy.PresentByProxy = false;
//                            proxy.TakePoll = false;
//                            proxy.NotVerifiable = false;
//                            proxy.Selected = false;
//                            proxy.resolution = false;
//                            if (proxy.emailAddress != null && proxy.emailAddress.StartsWith("proxy"))
//                            {
//                                proxy.emailAddress = proxy.emailAddress.Substring(5);
//                            }

//                        }


//                    }
//                    db.SaveChanges();
//                    Functions.PresentCount(UniqueAGMId, true);
//                    return Task.FromResult<string>("success");

//                }
//                else if (a.Count() == 1)
//                {
//                    var id = int.Parse(a[0].ToString());
//                    var proxy = db.BarcodeStore.Find(id);
//                    if (proxy != null)
//                    {
//                        var result = db.Result.Where(r => r.AGMID == UniqueAGMId && r.ShareholderNum == proxy.ShareholderNum).ToList();
//                        if (result.Any())
//                        {
//                            db.Result.RemoveRange(result);
//                        }
//                        //for (int i = 0; i < result.Length; i++)
//                        //{
//                        //    db.Result.Remove(result[i]);

//                        //}
//                        var present = db.Present.FirstOrDefault(p => p.AGMID == UniqueAGMId && p.ShareholderNum == proxy.ShareholderNum);
//                        if (present != null)
//                        {
//                            db.Present.Remove(present);
//                        }
//                        //if (present!= null)
//                        //{
//                        //    for (int i = 0; i < present.Length; i++)
//                        //    {
//                        //        db.Present.Remove(present[i]);

//                        //    }

//                        //}
//                        proxy.PresentByProxy = false;
//                        proxy.TakePoll = false;
//                        proxy.NotVerifiable = true;
//                        proxy.Selected = false;
//                        proxy.resolution = false;
//                        if (proxy.emailAddress != null && proxy.emailAddress.StartsWith("proxy"))
//                        {
//                            proxy.emailAddress = proxy.emailAddress.Substring(5);
//                        }
//                        db.SaveChanges();
//                        Functions.PresentCount(UniqueAGMId, true);
//                        return Task.FromResult<string>("success");
//                    }
//                    return Task.FromResult<string>("Item Not Selected");
//                }
//                else
//                {
//                    return Task.FromResult<string>("Item Not Selected");
//                }
//            }
//            catch (Exception e)
//            {
//                return Task.FromResult<string>(string.Format("Error {0}", e.Message.ToString()));
//            }

//        }
//        //
//        // POST: /BarcodeLib/Delete/5

//        // public ActionResult Deleteall()
//        // {
//        //     return PartialView();
//        // }

//        //[HttpPost]
//        //public ActionResult AllDelete()
//        //{
//        //    try
//        //    {
//        //        var resultdata = db.Result.ToArray();
//        //        for (int i = 0; i < resultdata.Length; i++)
//        //        {
//        //            db.Result.Remove(resultdata[i]);
//        //            db.SaveChanges();
//        //            //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
//        //            Functions.SendProgress("Process in progress...", i, resultdata.Length);
//        //        }

//        //        var pdata = db.Present.ToArray();
//        //        for (int i = 0; i < pdata.Length; i++)
//        //        {
//        //            db.Present.Remove(pdata[i]);
//        //            db.SaveChanges();
//        //            //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
//        //            Functions.SendProgress("Process in progress...", i, pdata.Length);
//        //        }
//        //        var sdata = db.SplittedAccounts.ToArray();
//        //        for (int i = 0; i < sdata.Length; i++)
//        //        {
//        //            db.SplittedAccounts.Remove(sdata[i]);
//        //            db.SaveChanges();
//        //            //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
//        //            Functions.SendProgress("Process in progress...", i, sdata.Length);
//        //        }
//        //        // TODO: Add delete logic here
//        //        var data= db.BarcodeStore.ToArray();
//        //       for (int i = 0; i < data.Length; i++ )
//        //       {
//        //           db.BarcodeStore.Remove(data[i]);
//        //           db.SaveChanges();
//        //           //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
//        //           Functions.SendProgress("Process in progress...", i, data.Length);
//        //       }
//        //        ViewBag.Message = " Database has been Cleared";
//        //        return RedirectToAction("Index");
//        //    }
//        //    catch
//        //    {
//        //        ViewBag.Message1 = " Something Went Wrong, Could not clear database";
//        //        return RedirectToAction("Index");
//        //    }
//        //}

//        //[HttpPost]

//        public async Task<string> AllDelete(string id)
//        {
//            var response = await AllDeleteAsync(id);

//            return response;
//        }

//        public Task<string> AllDeleteAsync(string company)
//        {
//            //var companyinfo = ua.GetUserCompanyInfo();

//            string connStr = DatabaseManager.GetConnectionString();

//            SqlConnection conn =
//                    new SqlConnection(connStr);
//            try
//            {

//                //    string query1 = "TRUNCATE TABLE Results";
//                //    conn.Open();
//                //    SqlCommand cmd1 = new SqlCommand(query1, conn);
//                //    cmd1.ExecuteNonQuery();

//                //string query2 = "TRUNCATE TABLE SMSResults";
//                ////conn.Open();
//                //SqlCommand cmd2 = new SqlCommand(query2, conn);
//                //cmd2.ExecuteNonQuery();


//                //string query3 = "TRUNCATE TABLE SMSDeliveryLogs";
//                ////conn.Open();
//                //SqlCommand cmd3 = new SqlCommand(query3, conn);
//                //cmd3.ExecuteNonQuery();

//                //string query4 = "TRUNCATE TABLE PresentModels";
//                //    //conn.Open();
//                //    SqlCommand cmd4 = new SqlCommand(query4, conn);
//                //    cmd4.ExecuteNonQuery();

//                //string query5 = "TRUNCATE TABLE ProxyLists";
//                ////conn.Open();
//                //SqlCommand cmd5 = new SqlCommand(query5, conn);
//                //cmd5.ExecuteNonQuery();

//                //string query4 = "TRUNCATE TABLE SplittedAccounts";
//                ////conn.Open();
//                //SqlCommand cmd4 = new SqlCommand(query4, conn);
//                //cmd4.ExecuteNonQuery();

//                //string query5 = "ALTER TABLE APIMessageLogs DROP CONSTRAINT [FK_dbo.APIMessageLogs_dbo.BarcodeModels_shareholder_Id]";
//                ////conn.Open();
//                //SqlCommand cmd5 = new SqlCommand(query5, conn);
//                //cmd5.ExecuteNonQuery();

//                //string query6 = "TRUNCATE TABLE BarcodeModels" ;
//                //conn.Open();
//                //SqlCommand cmd6 = new SqlCommand(query6, conn);
//                //cmd6.ExecuteNonQuery();
//                var companyinfo = company.Trim();
//                string query6 = "DELETE FROM BarcodeModels WHERE Company='" + companyinfo + "'";
//                conn.Open();
//                SqlCommand cmd6 = new SqlCommand(query6, conn);
//                cmd6.ExecuteNonQuery();


//                //string query7 = "ALTER TABLE [dbo].[SMSResults] DROP CONSTRAINT [FK_dbo.SMSResults_dbo.Questions_Question_Id]";
//                ////conn.Open();
//                //SqlCommand cmd7 = new SqlCommand(query7, conn);
//                //cmd7.ExecuteNonQuery();

//                //string query8 = "ALTER TABLE [dbo].[Results] DROP CONSTRAINT [FK_dbo.Results_dbo.Questions_QuestionId]";
//                ////conn.Open();
//                //SqlCommand cmd8 = new SqlCommand(query8, conn);
//                //cmd8.ExecuteNonQuery();

//                //string query9 = "ALTER TABLE [dbo].[SMSDeliveryLogs] DROP CONSTRAINT [FK_dbo.SMSDeliveryLogs_dbo.Questions_Question_Id]";
//                ////conn.Open();
//                //SqlCommand cmd9 = new SqlCommand(query9, conn);
//                //cmd9.ExecuteNonQuery();

//                //string query10 = "TRUNCATE TABLE Questions";
//                ////conn.Open();
//                //SqlCommand cmd10 = new SqlCommand(query10, conn);
//                //cmd10.ExecuteNonQuery();

//                //string query11= "ALTER TABLE [dbo].[SMSResults] ADD CONSTRAINT [FK_dbo.SMSResults_dbo.Questions_Question_Id] FOREIGN KEY ([Question_Id]) REFERENCES [dbo].[Questions] ([Id])";
//                ////conn.Open();
//                //SqlCommand cmd11 = new SqlCommand(query11, conn);
//                //cmd11.ExecuteNonQuery();

//                //string query12 = "ALTER TABLE [dbo].[Results] ADD CONSTRAINT [FK_dbo.Results_dbo.Questions_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Questions] ([Id])";
//                ////conn.Open();
//                //SqlCommand cmd12 = new SqlCommand(query12, conn);
//                //cmd12.ExecuteNonQuery();

//                //string query13 = "ALTER TABLE [dbo].[SMSDeliveryLogs] ADD CONSTRAINT [FK_dbo.SMSDeliveryLogs_dbo.Questions_Question_Id] FOREIGN KEY ([Question_Id]) REFERENCES [dbo].[Questions] ([Id])";
//                ////conn.Open();
//                //SqlCommand cmd13 = new SqlCommand(query13, conn);
//                //cmd13.ExecuteNonQuery();

//                //string query7 = "ALTER TABLE APIMessageLogs ADD CONSTRAINT [FK_dbo.APIMessageLogs_dbo.BarcodeModels_shareholder_Id] FOREIGN KEY ([shareholder_Id]) REFERENCES [dbo].[BarcodeModels] ([Id])";
//                ////conn.Open();
//                //SqlCommand cmd7 = new SqlCommand(query7, conn);
//                //cmd7.ExecuteNonQuery();

//                conn.Close();

//                //var setting = db.Settings.FirstOrDefault(s=>s.CompanyName == companyinfo && s.AGMID == UniqueAGMId);
//                ////setting.ShareHolding = 0;
//                //db.SaveChanges();

//                // ViewBag.Message = " Database has been Cleared";
//                return Task.FromResult<string>("Success");

//            }
//            catch (Exception e)
//            {
//                conn.Close();

//                return Task.FromResult<string>("Something Went Wrong, Could not clear database" + " " + e);
//            }
//        }


//        public Task TakeResolutionAsync(BarcodeModel shareholder, int questionid, int AgmId)
//        {
//            var response = "FOR";
//            var question = db.Question.Find(questionid);
//            var checkresult = db.Result.FirstOrDefault(r => r.ShareholderNum == shareholder.ShareholderNum && r.QuestionId == questionid);
//            if (checkresult != null)
//            {
//                checkresult.date = DateTime.Now;
//                checkresult.Timestamp = DateTime.Now.TimeOfDay;
//                //checkresult.Holding = float.Parse(post.NewHolding);
//                checkresult.Holding = shareholder.PercentageHolding;
//                checkresult.VoteStatus = "Voted";
//                checkresult.Source = "Proxy";
//                if (response == "FOR")
//                {
//                    checkresult.VoteFor = true;
//                    checkresult.VoteAgainst = false;
//                    checkresult.VoteAbstain = false;
//                }
//                else if (response == "AGAINST")
//                {
//                    checkresult.VoteAgainst = true;
//                    checkresult.VoteFor = false;
//                    checkresult.VoteAbstain = false;
//                }
//                else if (response == "ABSTAIN")
//                {
//                    checkresult.VoteAbstain = true;
//                    checkresult.VoteFor = false;
//                    checkresult.VoteAgainst = false;
//                }
//                db.Entry(checkresult).State = EntityState.Modified;
//                try
//                {
//                    db.SaveChanges();
//                    return Task.FromResult<Result>(checkresult);

//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    ////TempData["Message"] = "Error Adding your poll Information";

//                    return Task.FromResult<Result>(new Result());
//                }
//            }
//            else
//            {
//                Result result = new Result();
//                result.ShareholderNum = shareholder.ShareholderNum;
//                result.ParentNumber = shareholder.ShareholderNum;
//                result.phonenumber = shareholder.PhoneNumber;
//                result.Company = shareholder.Company;
//                result.Year = DateTime.Now.Year.ToString();
//                result.AGMID = AgmId;
//                //result.Holding = double.Parse(post.NewHolding);
//                result.Holding = shareholder.Holding;
//                result.Name = shareholder.Name;
//                result.splitValue = 0;
//                result.Address = shareholder.Address;
//                result.PercentageHolding = shareholder.PercentageHolding;
//                result.QuestionId = questionid;
//                result.VoteStatus = "Voted";
//                result.date = DateTime.Now;
//                result.Timestamp = DateTime.Now.TimeOfDay;
//                result.PresentByProxy = true;
//                result.Source = "Proxy";

//                if (response == "FOR")
//                {
//                    result.VoteFor = true;
//                    result.VoteAgainst = false;
//                    result.VoteAbstain = false;

//                }
//                else if (response == "AGAINST")
//                {
//                    result.VoteAgainst = true;
//                    result.VoteFor = false;
//                    result.VoteAbstain = false;
//                }
//                else if (response == "ABSTAIN")
//                {
//                    result.VoteAbstain = true;
//                    result.VoteFor = false;
//                    result.VoteAgainst = false;
//                }
//                question.result.Add(result);

//                try
//                {
//                    db.SaveChanges();
//                    return Task.FromResult<Result>(result);

//                }
//                catch (DbUpdateConcurrencyException e)
//                {


//                    return Task.FromResult<Result>(new Result());
//                }
//            }
//        }

//        #region Helpers
//        // private ActionResult RedirectToLocal(string returnUrl)
//        // {
//        //     if (Url.IsLocalUrl(returnUrl))
//        //     {
//        //         return Redirect(returnUrl);
//        //     }
//        //     else
//        //     {
//        //         return RedirectToAction("Index", "Home");
//        //     }
//        // }
//        #endregion
//    }
//}