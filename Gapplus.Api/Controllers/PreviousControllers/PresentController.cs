using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using Gapplus.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Syncfusion.XlsIO;

namespace BarcodeGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PresentController : ControllerBase
    {

        private readonly IViewBagManager _viewBagManager;
        private readonly ITempDataManager _tempDataManager;
        UsersContext db;
        UserAdmin ua;
        public PresentController(UsersContext context, IViewBagManager viewBagManager, ITempDataManager tempDataManager)
        {
            db = context;
            _viewBagManager = viewBagManager;
            ua = new UserAdmin(db);
        }










        private static string currentYear = DateTime.Now.Year.ToString();



















        //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

        //private static int RetrieveAGMUniqueID()
        //{
        //    UsersContext adb = new UsersContext();
        //    var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

        //    return AGMID;
        //}

        //private static int UniqueAGMId = RetrieveAGMUniqueID();










        // public async Task<ActionResult> Index()
        // {
        //     //ViewBag.presentcount = db.Present.Count();
        //     //ViewBag.feedbackcount = db.ShareholderFeedback.Count();
        //     //ViewBag.shareholders = db.BarcodeStore.Count();
        //     var returnUrl = HttpContext.Request.Url.AbsolutePath;
        //     string returnvalue = "";
        //     if (HttpContext.Request.QueryString.Count > 0)
        //     {
        //         returnvalue = HttpContext.Request.QueryString["rel"].ToString();
        //     }
        //     ViewBag.value = returnvalue.Trim();
        //     var response = await IndexAsync();
        //     if (String.IsNullOrEmpty(response.Company))
        //     {
        //         return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
        //     }

        //     return PartialView(response);


        // }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //ViewBag.presentcount = db.Present.Count();
            //ViewBag.feedbackcount = db.ShareholderFeedback.Count();
            //ViewBag.shareholders = db.BarcodeStore.Count();
            var returnUrl = HttpContext.Request.Path; // Get the path
            string returnvalue = "";
            if (HttpContext.Request.Query.ContainsKey("rel")) // Check if query string contains 'rel'
            {
                returnvalue = HttpContext.Request.Query["rel"].ToString(); // Retrieve 'rel' value from query string
            }
            //     ViewBag.value = returnvalue.Trim();
            _viewBagManager.SetValue("value", returnvalue.Trim());
            var response = await IndexAsync();
            if (String.IsNullOrEmpty(response.Company))
            {
                return RedirectToAction("GetCompanyInfo", "Account", new { returnUrl = returnUrl, returnValue = returnvalue.Trim() });
            }

            // return PartialView(response);
            return Ok(response);
        }


        private Task<PresentViewModel> IndexAsync()
        {
            var companyinfo = ua.GetUserCompanyInfo();
            var UniqueAGMId = ua.RetrieveAGMUniqueID();
            PresentViewModel model = new PresentViewModel();
            var user = db.UserProfiles.SingleOrDefault(u => u.UserName == User.Identity.Name);
            model.User = user;
            model.Company = companyinfo;


            if (UniqueAGMId != -1)
            {
                var setting = db.Settings.FirstOrDefault(s => s.AGMID == UniqueAGMId);
                if (setting != null)
                {
                    if (!String.IsNullOrEmpty(setting.Title))
                    {
                        model.AGMTitle = setting.Title.ToUpper();
                        model.agmid = UniqueAGMId;

                    }
                    else
                    {
                        model.AGMTitle = "";
                    }
                }
            }

            return Task.FromResult<PresentViewModel>(model);
        }



        [
            HttpGet
        ]
        public async Task<ActionResult> AjaxHandler([FromServices] HttpContext httpContext)
        {
            var response = await AjaxHandlerAsync(httpContext);

            return Ok(new { draw = response.draw, recordsFiltered = response.recordsTotal, recordsTotal = response.recordsTotal, data = response.displayedPresentList });
        }


        [HttpGet]
        public async Task<ActionResult> ProxyAjaxHandler([FromServices] HttpContext ctx)
        {
            var response = await ProxyAjaxHandlerAsync(ctx);

            return Ok(new { draw = response.draw, recordsFiltered = response.recordsTotal, recordsTotal = response.recordsTotal, data = response.displayedPresentList });
        }

        [HttpGet]
        public async Task<ActionResult> PreregisteredAjaxHandler([FromServices] HttpContext ctx)
        {
            var response = await preregisteredAjaxHandlerAsync(ctx);

            return Ok(new { draw = response.draw, recordsFiltered = response.recordsTotal, recordsTotal = response.recordsTotal, data = response.displayedPresentList });
        }

        // private Task<AjaxTableDto> AjaxHandlerAsync()
        // {
        //     try
        //     {
        //         //Creating instance of DatabaseContext class  
        //         using (UsersContext _context = new UsersContext())
        //         {
        //             var companyinfo = ua.GetUserCompanyInfo();
        //             var UniqueAGMId = ua.RetrieveAGMUniqueID();

        //             var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //             var start = Request.Form.GetValues("start").FirstOrDefault();
        //             var length = Request.Form.GetValues("length").FirstOrDefault();
        //             var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //             var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //             var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

        //             string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //             //Paging Size (10,20,50,100)    
        //             int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //             int skip = start != null ? Convert.ToInt32(start) : 0;
        //             int recordsTotal = 0;
        //             // Getting all Shareholder data 
        //             List<PresentModel> filteredpresent = new List<PresentModel>();
        //             SqlConnection conn =
        //                       new SqlConnection(connStr);

        //             if (!string.IsNullOrEmpty(searchValue))
        //             {


        //                 using (SqlConnection conn1 =
        //                          new SqlConnection(connStr))
        //                 {
        //                     var isNumeric = int.TryParse(searchValue, out _);
        //                     string query;
        //                     if (isNumeric)
        //                     {
        //                         query = "select * from PresentModels WHERE ( AGMID ='" + UniqueAGMId + "' AND present= 1 AND ShareholderNum = '" + searchValue + "')";
        //                         if (sortColumn != "")
        //                         {
        //                             query = "select * from PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND present= 1 AND ShareholderNum = '" + searchValue + "') Order By " + sortColumn + " " + sortColumnDir + "";
        //                             //query = "select * from PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND Name LIKE '%" + searchValue + "%' Order By " + sortColumn + " " + sortColumnDir + "";

        //                         }
        //                     }
        //                     else
        //                     {
        //                         //string query2 = "select * from BarcodeModels WHERE CONTAINS(Name,'" + searchValue + "')";
        //                         //string query = "select * from PresentModels WHERE AGMID = '"+ UniqueAGMId +"' AND Name LIKE '%" + searchValue + "%'";
        //                         query = "select * from PresentModels WHERE AGMID ='" + UniqueAGMId + "' AND present= 1 AND Name LIKE '%" + searchValue + "%'";
        //                         if (sortColumn != "")
        //                         {
        //                             query = "select * from PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND present= 1 AND Name LIKE '%" + searchValue + "%' Order By " + sortColumn + " " + sortColumnDir + "";
        //                             //query = "select * from PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND Name LIKE '%" + searchValue + "%' Order By " + sortColumn + " " + sortColumnDir + "";

        //                         }
        //                     }


        //                     conn1.Open();
        //                     SqlCommand cmd1 = new SqlCommand(query, conn1);
        //                     SqlDataReader read1 = cmd1.ExecuteReader();

        //                     while (read1.Read())
        //                     {
        //                         PresentModel model1 = new PresentModel
        //                         {
        //                             Id = int.Parse(read1["Id"].ToString()),
        //                             Name = (read1["Name"].ToString()),
        //                             Address = (read1["Address"].ToString()),
        //                             ShareholderNum = (Int64.Parse(read1["ShareholderNum"].ToString())),
        //                             Holding = double.Parse(read1["Holding"].ToString()),
        //                             PercentageHolding = double.Parse(read1["PercentageHolding"].ToString()),
        //                             newNumber = (Int64.Parse(read1["newNumber"].ToString())),
        //                             PhoneNumber = (read1["PhoneNumber"].ToString()),
        //                             emailAddress = (read1["emailAddress"].ToString()),
        //                             present = bool.Parse(read1["present"].ToString()),
        //                             proxy = bool.Parse(read1["proxy"].ToString()),
        //                             split = bool.Parse(read1["split"].ToString()),
        //                             TakePoll = bool.Parse(read1["TakePoll"].ToString()),
        //                             Clikapad = (read1["Clikapad"].ToString()),
        //                             GivenClikapad = bool.Parse(read1["GivenClikapad"].ToString()),
        //                             admitSource = (read1["admitSource"].ToString()),
        //                             ReturnedClikapad = bool.Parse(read1["ReturnedClikapad"].ToString()),
        //                             PermitPoll = byte.Parse(read1["PermitPoll"].ToString())


        //                         };
        //                         filteredpresent.Add(model1);
        //                     }
        //                     read1.Close();
        //                 }
        //             }
        //             //else
        //             //{
        //             //    filteredpresent = Allpresent.ToList();
        //             //}
        //             //Paging   
        //             var displayedpresent = filteredpresent
        //                      .Skip(skip)
        //                      .Take(pageSize);

        //             //total number of rows count     
        //             recordsTotal = filteredpresent.Count();

        //             AjaxTableDto dto = new AjaxTableDto
        //             {
        //                 draw = draw,
        //                 recordsTotal = recordsTotal,
        //                 recordsFiltered = recordsTotal,
        //                 displayedPresentList = displayedpresent
        //             };
        //             //Returning Ok Data    
        //             return Task.FromResult<AjaxTableDto>(dto);
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         //throw;
        //         return Task.FromResult<AjaxTableDto>(new AjaxTableDto());

        //     }

        // }


        private async Task<AjaxTableDto> AjaxHandlerAsync([FromServices] HttpContext httpContext)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID();

                var draw = httpContext.Request.Form["draw"].FirstOrDefault();
                var start = httpContext.Request.Form["start"].FirstOrDefault();
                var length = httpContext.Request.Form["length"].FirstOrDefault();
                var sortColumn = httpContext.Request.Form["columns[" + httpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDir = httpContext.Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = httpContext.Request.Form["search[value]"].FirstOrDefault();

                string connStr = DatabaseManager.GetConnectionString("DefaultConnection");
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                List<PresentModel> filteredPresent = new List<PresentModel>();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    string query;

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        var isNumeric = int.TryParse(searchValue, out _);
                        if (isNumeric)
                        {
                            query = $"SELECT * FROM PresentModels WHERE (AGMID = '{UniqueAGMId}' AND present = 1 AND ShareholderNum = '{searchValue}')";
                            if (!string.IsNullOrEmpty(sortColumn))
                                query = $"SELECT * FROM PresentModels WHERE (AGMID = '{UniqueAGMId}' AND present = 1 AND ShareholderNum = '{searchValue}') ORDER BY {sortColumn} {sortColumnDir}";
                        }
                        else
                        {
                            query = $"SELECT * FROM PresentModels WHERE (AGMID = '{UniqueAGMId}' AND present = 1 AND Name LIKE '%{searchValue}%')";
                            if (!string.IsNullOrEmpty(sortColumn))
                                query = $"SELECT * FROM PresentModels WHERE (AGMID = '{UniqueAGMId}' AND present = 1 AND Name LIKE '%{searchValue}%') ORDER BY {sortColumn} {sortColumnDir}";
                        }

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader read = await cmd.ExecuteReaderAsync())
                            {
                                while (await read.ReadAsync())
                                {
                                    PresentModel model = new PresentModel
                                    {
                                        Id = read.GetInt32(read.GetOrdinal("Id")),
                                        Name = read.GetString(read.GetOrdinal("Name")),
                                        Address = read.GetString(read.GetOrdinal("Address")),
                                        ShareholderNum = read.GetInt64(read.GetOrdinal("ShareholderNum")),
                                        Holding = read.GetDouble(read.GetOrdinal("Holding")),
                                        PercentageHolding = read.GetDouble(read.GetOrdinal("PercentageHolding")),
                                        newNumber = read.GetInt64(read.GetOrdinal("newNumber")),
                                        PhoneNumber = read.GetString(read.GetOrdinal("PhoneNumber")),
                                        emailAddress = read.GetString(read.GetOrdinal("emailAddress")),
                                        present = read.GetBoolean(read.GetOrdinal("present")),
                                        proxy = read.GetBoolean(read.GetOrdinal("proxy")),
                                        split = read.GetBoolean(read.GetOrdinal("split")),
                                        TakePoll = read.GetBoolean(read.GetOrdinal("TakePoll")),
                                        Clikapad = read.GetString(read.GetOrdinal("Clikapad")),
                                        GivenClikapad = read.GetBoolean(read.GetOrdinal("GivenClikapad")),
                                        admitSource = read.GetString(read.GetOrdinal("admitSource")),
                                        ReturnedClikapad = read.GetBoolean(read.GetOrdinal("ReturnedClikapad")),
                                        PermitPoll = read.GetByte(read.GetOrdinal("PermitPoll"))
                                    };
                                    filteredPresent.Add(model);
                                }
                            }
                        }
                    }

                    recordsTotal = filteredPresent.Count();

                    var displayedPresent = filteredPresent
                        .Skip(skip)
                        .Take(pageSize);

                    AjaxTableDto dto = new AjaxTableDto
                    {
                        draw = draw,
                        recordsTotal = recordsTotal,
                        recordsFiltered = recordsTotal,
                        displayedPresentList = displayedPresent.ToList()
                    };

                    return dto;
                }
            }
            catch (Exception e)
            {
                // Handle the exception appropriately
                return new AjaxTableDto();
            }
        }













        // private Task<AjaxTableDto> ProxyAjaxHandlerAsync()
        // {
        //     try
        //     {
        //         //Creating instance of DatabaseContext class  
        //         using (UsersContext _context = new UsersContext())
        //         {
        //             var companyinfo = ua.GetUserCompanyInfo();
        //             var UniqueAGMId = ua.RetrieveAGMUniqueID();

        //             var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //             var start = Request.Form.GetValues("start").FirstOrDefault();
        //             var length = Request.Form.GetValues("length").FirstOrDefault();
        //             var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //             var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //             var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

        //             string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //             //Paging Size (10,20,50,100)    
        //             int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //             int skip = start != null ? Convert.ToInt32(start) : 0;
        //             int recordsTotal = 0;
        //             // Getting all Shareholder data 
        //             List<PresentModel> filteredpresent = new List<PresentModel>();
        //             SqlConnection conn =
        //                       new SqlConnection(connStr);

        //             if (!string.IsNullOrEmpty(searchValue))
        //             {


        //                 using (SqlConnection conn1 =
        //                          new SqlConnection(connStr))
        //                 {
        //                     var isNumeric = int.TryParse(searchValue, out _);
        //                     string query;
        //                     if (isNumeric)
        //                     {
        //                         query = "select * from PresentModels WHERE ( AGMID ='" + UniqueAGMId + "' AND ShareholderNum = '" + searchValue + "' AND proxy = 1)";
        //                         if (sortColumn != "")
        //                         {
        //                             query = "select * from PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND ShareholderNum = '" + searchValue + "' AND proxy = 1) Order By " + sortColumn + " " + sortColumnDir + "";
        //                             //query = "select * from PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND Name LIKE '%" + searchValue + "%' Order By " + sortColumn + " " + sortColumnDir + "";

        //                         }
        //                     }
        //                     else
        //                     {
        //                         //string query2 = "select * from BarcodeModels WHERE CONTAINS(Name,'" + searchValue + "')";
        //                         //string query = "select * from PresentModels WHERE AGMID = '"+ UniqueAGMId +"' AND Name LIKE '%" + searchValue + "%'";
        //                         query = "select * from PresentModels WHERE AGMID ='" + UniqueAGMId + "' AND proxy = 1 AND Name LIKE '%" + searchValue + "%'";
        //                         if (sortColumn != "")
        //                         {
        //                             query = "select * from PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND proxy = 1 AND Name LIKE '%" + searchValue + "%' Order By " + sortColumn + " " + sortColumnDir + "";
        //                             //query = "select * from PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND Name LIKE '%" + searchValue + "%' Order By " + sortColumn + " " + sortColumnDir + "";

        //                         }
        //                     }


        //                     conn1.Open();
        //                     SqlCommand cmd1 = new SqlCommand(query, conn1);
        //                     SqlDataReader read1 = cmd1.ExecuteReader();

        //                     while (read1.Read())
        //                     {
        //                         PresentModel model1 = new PresentModel
        //                         {
        //                             Id = int.Parse(read1["Id"].ToString()),
        //                             Name = (read1["Name"].ToString()),
        //                             Address = (read1["Address"].ToString()),
        //                             ShareholderNum = (Int64.Parse(read1["ShareholderNum"].ToString())),
        //                             Holding = double.Parse(read1["Holding"].ToString()),
        //                             PercentageHolding = double.Parse(read1["PercentageHolding"].ToString()),
        //                             newNumber = (Int64.Parse(read1["newNumber"].ToString())),
        //                             PhoneNumber = (read1["PhoneNumber"].ToString()),
        //                             emailAddress = (read1["emailAddress"].ToString()),
        //                             present = bool.Parse(read1["present"].ToString()),
        //                             proxy = bool.Parse(read1["proxy"].ToString()),
        //                             split = bool.Parse(read1["split"].ToString()),
        //                             TakePoll = bool.Parse(read1["TakePoll"].ToString()),
        //                             Clikapad = (read1["Clikapad"].ToString()),
        //                             GivenClikapad = bool.Parse(read1["GivenClikapad"].ToString()),
        //                             admitSource = (read1["admitSource"].ToString()),
        //                             ReturnedClikapad = bool.Parse(read1["ReturnedClikapad"].ToString()),
        //                             PermitPoll = byte.Parse(read1["PermitPoll"].ToString())


        //                         };
        //                         filteredpresent.Add(model1);
        //                     }
        //                     read1.Close();
        //                 }
        //             }
        //             //else
        //             //{
        //             //    filteredpresent = Allpresent.ToList();
        //             //}
        //             //Paging   
        //             var displayedpresent = filteredpresent
        //                      .Skip(skip)
        //                      .Take(pageSize);

        //             //total number of rows count     
        //             recordsTotal = filteredpresent.Count();

        //             AjaxTableDto dto = new AjaxTableDto
        //             {
        //                 draw = draw,
        //                 recordsTotal = recordsTotal,
        //                 recordsFiltered = recordsTotal,
        //                 displayedPresentList = displayedpresent
        //             };
        //             //Returning Ok Data    
        //             return Task.FromResult<AjaxTableDto>(dto);
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         //throw;
        //         return Task.FromResult<AjaxTableDto>(new AjaxTableDto());

        //     }

        // }




        private async Task<AjaxTableDto> ProxyAjaxHandlerAsync(HttpContext httpContext)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID();

                var draw = httpContext.Request.Form["draw"].FirstOrDefault();
                var start = httpContext.Request.Form["start"].FirstOrDefault();
                var length = httpContext.Request.Form["length"].FirstOrDefault();
                var sortColumn = httpContext.Request.Form["columns[" + httpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDir = httpContext.Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = httpContext.Request.Form["search[value]"].FirstOrDefault();

                string connStr = DatabaseManager.GetConnectionString("DefaultConnection");
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                List<PresentModel> filteredPresent = new List<PresentModel>();

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    await conn.OpenAsync();

                    string query;

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        var isNumeric = int.TryParse(searchValue, out _);
                        if (isNumeric)
                        {
                            query = $"SELECT * FROM PresentModels WHERE (AGMID = '{UniqueAGMId}' AND ShareholderNum = '{searchValue}' AND proxy = 1)";
                            if (!string.IsNullOrEmpty(sortColumn))
                                query = $"SELECT * FROM PresentModels WHERE (AGMID = '{UniqueAGMId}' AND ShareholderNum = '{searchValue}' AND proxy = 1) ORDER BY {sortColumn} {sortColumnDir}";
                        }
                        else
                        {
                            query = $"SELECT * FROM PresentModels WHERE (AGMID = '{UniqueAGMId}' AND proxy = 1 AND Name LIKE '%{searchValue}%')";
                            if (!string.IsNullOrEmpty(sortColumn))
                                query = $"SELECT * FROM PresentModels WHERE (AGMID = '{UniqueAGMId}' AND proxy = 1 AND Name LIKE '%{searchValue}%') ORDER BY {sortColumn} {sortColumnDir}";
                        }

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader read = await cmd.ExecuteReaderAsync())
                            {
                                while (await read.ReadAsync())
                                {
                                    PresentModel model = new PresentModel
                                    {
                                        Id = read.GetInt32(read.GetOrdinal("Id")),
                                        Name = read.GetString(read.GetOrdinal("Name")),
                                        Address = read.GetString(read.GetOrdinal("Address")),
                                        ShareholderNum = read.GetInt64(read.GetOrdinal("ShareholderNum")),
                                        Holding = read.GetDouble(read.GetOrdinal("Holding")),
                                        PercentageHolding = read.GetDouble(read.GetOrdinal("PercentageHolding")),
                                        newNumber = read.GetInt64(read.GetOrdinal("newNumber")),
                                        PhoneNumber = read.GetString(read.GetOrdinal("PhoneNumber")),
                                        emailAddress = read.GetString(read.GetOrdinal("emailAddress")),
                                        present = read.GetBoolean(read.GetOrdinal("present")),
                                        proxy = read.GetBoolean(read.GetOrdinal("proxy")),
                                        split = read.GetBoolean(read.GetOrdinal("split")),
                                        TakePoll = read.GetBoolean(read.GetOrdinal("TakePoll")),
                                        Clikapad = read.GetString(read.GetOrdinal("Clikapad")),
                                        GivenClikapad = read.GetBoolean(read.GetOrdinal("GivenClikapad")),
                                        admitSource = read.GetString(read.GetOrdinal("admitSource")),
                                        ReturnedClikapad = read.GetBoolean(read.GetOrdinal("ReturnedClikapad")),
                                        PermitPoll = read.GetByte(read.GetOrdinal("PermitPoll"))
                                    };
                                    filteredPresent.Add(model);
                                }
                            }
                        }
                    }

                    recordsTotal = filteredPresent.Count();

                    var displayedPresent = filteredPresent
                        .Skip(skip)
                        .Take(pageSize);

                    AjaxTableDto dto = new AjaxTableDto
                    {
                        draw = draw,
                        recordsTotal = recordsTotal,
                        recordsFiltered = recordsTotal,
                        displayedPresentList = displayedPresent.ToList()
                    };

                    return dto;
                }
            }
            catch (Exception e)
            {
                // Handle the exception appropriately
                return new AjaxTableDto();
            }
        }













        //    private Task<AjaxTableDto> preregisteredAjaxHandlerAsync()
        //     {
        //         try
        //         {
        //             //Creating instance of DatabaseContext class  
        //             using (UsersContext _context = new UsersContext())
        //             {
        //                 var companyinfo = ua.GetUserCompanyInfo();
        //                 var UniqueAGMId = ua.RetrieveAGMUniqueID();

        //                 var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //                 var start = Request.Form.GetValues("start").FirstOrDefault();
        //                 var length = Request.Form.GetValues("length").FirstOrDefault();
        //                 var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //                 var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //                 var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

        //                 string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //                 //Paging Size (10,20,50,100)    
        //                 int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //                 int skip = start != null ? Convert.ToInt32(start) : 0;
        //                 int recordsTotal = 0;
        //                 // Getting all Shareholder data 
        //                 List<PresentModel> filteredpresent = new List<PresentModel>();
        //                 SqlConnection conn =
        //                           new SqlConnection(connStr);

        //                 if (!string.IsNullOrEmpty(searchValue))
        //                 {


        //                     using (SqlConnection conn1 =
        //                              new SqlConnection(connStr))
        //                     {
        //                         var isNumeric = int.TryParse(searchValue, out _);
        //                         string query;
        //                         if (isNumeric)
        //                         {
        //                             query = "select * from PresentModels WHERE ( AGMID ='" + UniqueAGMId + "' AND ShareholderNum = '" + searchValue + "' AND preregistered = 1)";
        //                             if (sortColumn != "")
        //                             {
        //                                 query = "select * from PresentModels WHERE (AGMID = '" + UniqueAGMId + "' AND ShareholderNum = '" + searchValue + "' AND preregistered = 1) Order By " + sortColumn + " " + sortColumnDir + "";
        //                                 //query = "select * from PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND Name LIKE '%" + searchValue + "%' Order By " + sortColumn + " " + sortColumnDir + "";

        //                             }
        //                         }
        //                         else
        //                         {
        //                             //string query2 = "select * from BarcodeModels WHERE CONTAINS(Name,'" + searchValue + "')";
        //                             //string query = "select * from PresentModels WHERE AGMID = '"+ UniqueAGMId +"' AND Name LIKE '%" + searchValue + "%'";
        //                             query = "select * from PresentModels WHERE AGMID ='" + UniqueAGMId + "' AND preregistered = 1 AND Name LIKE '%" + searchValue + "%'";
        //                             if (sortColumn != "")
        //                             {
        //                                 query = "select * from PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND preregistered = 1 AND Name LIKE '%" + searchValue + "%' Order By " + sortColumn + " " + sortColumnDir + "";
        //                                 //query = "select * from PresentModels WHERE AGMID = '" + UniqueAGMId + "' AND Name LIKE '%" + searchValue + "%' Order By " + sortColumn + " " + sortColumnDir + "";

        //                             }
        //                         }


        //                         conn1.Open();
        //                         SqlCommand cmd1 = new SqlCommand(query, conn1);
        //                         SqlDataReader read1 = cmd1.ExecuteReader();

        //                         while (read1.Read())
        //                         {
        //                             PresentModel model1 = new PresentModel
        //                             {
        //                                 Id = int.Parse(read1["Id"].ToString()),
        //                                 Name = (read1["Name"].ToString()),
        //                                 Address = (read1["Address"].ToString()),
        //                                 ShareholderNum = (Int64.Parse(read1["ShareholderNum"].ToString())),
        //                                 Holding = double.Parse(read1["Holding"].ToString()),
        //                                 PercentageHolding = double.Parse(read1["PercentageHolding"].ToString()),
        //                                 newNumber = (Int64.Parse(read1["newNumber"].ToString())),
        //                                 PhoneNumber = (read1["PhoneNumber"].ToString()),
        //                                 emailAddress = (read1["emailAddress"].ToString()),
        //                                 present = bool.Parse(read1["present"].ToString()),
        //                                 proxy = bool.Parse(read1["proxy"].ToString()),
        //                                 split = bool.Parse(read1["split"].ToString()),
        //                                 TakePoll = bool.Parse(read1["TakePoll"].ToString()),
        //                                 Clikapad = (read1["Clikapad"].ToString()),
        //                                 GivenClikapad = bool.Parse(read1["GivenClikapad"].ToString()),
        //                                 admitSource = (read1["admitSource"].ToString()),
        //                                 ReturnedClikapad = bool.Parse(read1["ReturnedClikapad"].ToString()),
        //                                 PermitPoll = byte.Parse(read1["PermitPoll"].ToString())


        //                             };
        //                             filteredpresent.Add(model1);
        //                         }
        //                         read1.Close();
        //                     }
        //                 }
        //                 //else
        //                 //{
        //                 //    filteredpresent = Allpresent.ToList();
        //                 //}
        //                 //Paging   
        //                 var displayedpresent = filteredpresent
        //                          .Skip(skip)
        //                          .Take(pageSize);

        //                 //total number of rows count     
        //                 recordsTotal = filteredpresent.Count();

        //                 AjaxTableDto dto = new AjaxTableDto
        //                 {
        //                     draw = draw,
        //                     recordsTotal = recordsTotal,
        //                     recordsFiltered = recordsTotal,
        //                     displayedPresentList = displayedpresent
        //                 };
        //                 //Returning Ok Data    
        //                 return Task.FromResult<AjaxTableDto>(dto);
        //             }
        //         }
        //         catch (Exception e)
        //         {
        //             //throw;
        //             return Task.FromResult<AjaxTableDto>(new AjaxTableDto());

        //         }

        //     }


[HttpGet]
        public async Task<AjaxTableDto> preregisteredAjaxHandlerAsync(HttpContext httpContext)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID();

                var draw = httpContext.Request.Form["draw"].FirstOrDefault();
                var start = httpContext.Request.Form["start"].FirstOrDefault();
                var length = httpContext.Request.Form["length"].FirstOrDefault();
                var sortColumn = httpContext.Request.Form["columns[" + httpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDir = httpContext.Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = httpContext.Request.Form["search[value]"].FirstOrDefault();

                string connStr = DatabaseManager.GetConnectionString("DefaultConnection");
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                List<PresentModel> filteredPresent = new List<PresentModel>();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        await conn.OpenAsync();

                        string query;
                        if (int.TryParse(searchValue, out _))
                        {
                            query = $"SELECT * FROM PresentModels WHERE AGMID = '{UniqueAGMId}' AND ShareholderNum = '{searchValue}' AND preregistered = 1";
                            if (!string.IsNullOrEmpty(sortColumn))
                            {
                                query = $"SELECT * FROM PresentModels WHERE AGMID = '{UniqueAGMId}' AND ShareholderNum = '{searchValue}' AND preregistered = 1 ORDER BY {sortColumn} {sortColumnDir}";
                            }
                        }
                        else
                        {
                            query = $"SELECT * FROM PresentModels WHERE AGMID = '{UniqueAGMId}' AND preregistered = 1 AND Name LIKE '%{searchValue}%'";
                            if (!string.IsNullOrEmpty(sortColumn))
                            {
                                query = $"SELECT * FROM PresentModels WHERE AGMID = '{UniqueAGMId}' AND preregistered = 1 AND Name LIKE '%{searchValue}%' ORDER BY {sortColumn} {sortColumnDir}";
                            }
                        }

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader read = await cmd.ExecuteReaderAsync())
                            {
                                while (await read.ReadAsync())
                                {
                                    PresentModel model = new PresentModel
                                    {
                                        Id = read.GetInt32(read.GetOrdinal("Id")),
                                        Name = read.GetString(read.GetOrdinal("Name")),
                                        Address = read.GetString(read.GetOrdinal("Address")),
                                        ShareholderNum = read.GetInt64(read.GetOrdinal("ShareholderNum")),
                                        Holding = read.GetDouble(read.GetOrdinal("Holding")),
                                        PercentageHolding = read.GetDouble(read.GetOrdinal("PercentageHolding")),
                                        newNumber = read.GetInt64(read.GetOrdinal("newNumber")),
                                        PhoneNumber = read.GetString(read.GetOrdinal("PhoneNumber")),
                                        emailAddress = read.GetString(read.GetOrdinal("emailAddress")),
                                        present = read.GetBoolean(read.GetOrdinal("present")),
                                        proxy = read.GetBoolean(read.GetOrdinal("proxy")),
                                        split = read.GetBoolean(read.GetOrdinal("split")),
                                        TakePoll = read.GetBoolean(read.GetOrdinal("TakePoll")),
                                        Clikapad = read.GetString(read.GetOrdinal("Clikapad")),
                                        GivenClikapad = read.GetBoolean(read.GetOrdinal("GivenClikapad")),
                                        admitSource = read.GetString(read.GetOrdinal("admitSource")),
                                        ReturnedClikapad = read.GetBoolean(read.GetOrdinal("ReturnedClikapad")),
                                        PermitPoll = read.GetByte(read.GetOrdinal("PermitPoll"))
                                    };
                                    filteredPresent.Add(model);
                                }
                            }
                        }
                    }
                }

                var displayedPresent = filteredPresent
                         .Skip(skip)
                         .Take(pageSize);

                recordsTotal = filteredPresent.Count();

                AjaxTableDto dto = new AjaxTableDto
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    displayedPresentList = displayedPresent.ToList()
                };

                return dto;
            }
            catch (Exception e)
            {
                // Handle the exception appropriately
                return new AjaxTableDto();
            }
        }










        // GET: /Present/Details/5
        [HttpGet]
        public async Task<ActionResult> PresentViewAjaxHandler([FromServices] HttpContext ctx)
        {
            var response = await PresentViewAjaxHandlerAsync(ctx);

            return Ok(new { draw = response.draw, recordsFiltered = response.recordsTotal, recordsTotal = response.recordsTotal, data = response.displayedPresentList });

        }


        // public Task<AjaxTableDto> PresentViewAjaxHandlerAsync()
        // {
        //     try
        //     {
        //         //Creating instance of DatabaseContext class  
        //         using (UsersContext _context = new UsersContext())
        //         {
        //             var companyinfo = ua.GetUserCompanyInfo();
        //             var UniqueAGMId = ua.RetrieveAGMUniqueID();

        //             var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //             var start = Request.Form.GetValues("start").FirstOrDefault();
        //             var length = Request.Form.GetValues("length").FirstOrDefault();
        //             var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //             var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //             var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

        //             string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //             //Paging Size (10,20,50,100)    
        //             int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //             int skip = start != null ? Convert.ToInt32(start) : 0;
        //             int recordsTotal = 0;
        //             // Getting all Shareholder data 
        //             List<PresentModel> filteredpresent = new List<PresentModel>();

        //             //Sorting  
        //             //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
        //             //{
        //             //    //filteredpresent = allPresent.OrderBy(s => s.Id);
        //             //    filteredpresent = Allpresent.OrderBy(s=>s.Id).ToList();
        //             //}
        //             //Search
        //             if (!string.IsNullOrEmpty(searchValue))
        //             {
        //                 using (SqlConnection conn1 =
        //                          new SqlConnection(connStr))
        //                 {
        //                     //string query2 = "select * from BarcodeModels WHERE CONTAINS(Name,'" + searchValue + "')";
        //                     //string query = "select * from PresentModels WHERE AGMID ='" + UniqueAGMId +"' AND Name LIKE '%" + searchValue + "%' OR ShareholderNum LIKE '%" + searchValue + "%'";
        //                     string query = "select * from PresentModels WHERE AGMID ='" + UniqueAGMId + "' AND Name LIKE '%" + searchValue + "%' ";
        //                     if (sortColumn != "")
        //                     {
        //                         query = "select * from PresentModels WHERE AGMID ='" + UniqueAGMId + "' AND Name LIKE '%" + searchValue + "%' OR ShareholderNum LIKE '%" + searchValue + "%' Order By " + sortColumn + " " + sortColumnDir + "";

        //                     }
        //                     conn1.Open();
        //                     SqlCommand cmd1 = new SqlCommand(query, conn1);
        //                     SqlDataReader read1 = cmd1.ExecuteReader();

        //                     while (read1.Read())
        //                     {
        //                         PresentModel model1 = new PresentModel
        //                         {


        //                             Id = int.Parse(read1["Id"].ToString()),
        //                             Name = (read1["Name"].ToString()),
        //                             Address = (read1["Address"].ToString()),
        //                             ShareholderNum = (Int64.Parse(read1["ShareholderNum"].ToString())),
        //                             Holding = double.Parse(read1["Holding"].ToString()),
        //                             PercentageHolding = double.Parse(read1["PercentageHolding"].ToString()),
        //                             newNumber = (Int64.Parse(read1["newNumber"].ToString())),
        //                             PhoneNumber = (read1["PhoneNumber"].ToString()),
        //                             emailAddress = (read1["emailAddress"].ToString()),
        //                             present = bool.Parse(read1["present"].ToString()),
        //                             proxy = bool.Parse(read1["proxy"].ToString()),
        //                             split = bool.Parse(read1["split"].ToString()),
        //                             TakePoll = bool.Parse(read1["TakePoll"].ToString()),
        //                             Clikapad = (read1["Clikapad"].ToString()),
        //                             GivenClikapad = bool.Parse(read1["GivenClikapad"].ToString()),
        //                             admitSource = (read1["admitSource"].ToString()),
        //                             ReturnedClikapad = bool.Parse(read1["ReturnedClikapad"].ToString()),
        //                             PermitPoll = Byte.Parse(read1["PermitPoll"].ToString())


        //                         };
        //                         filteredpresent.Add(model1);
        //                     }
        //                     read1.Close();
        //                 }
        //             }

        //             //Paging   
        //             var displayedpresent = filteredpresent
        //                      .Skip(skip)
        //                      .Take(pageSize);


        //             //total number of rows count     
        //             recordsTotal = filteredpresent.Count();

        //             AjaxTableDto dto = new AjaxTableDto
        //             {
        //                 draw = draw,
        //                 recordsTotal = recordsTotal,
        //                 recordsFiltered = recordsTotal,
        //                 displayedPresentList = displayedpresent
        //             };

        //             //Returning Ok Data    
        //             return Task.FromResult<AjaxTableDto>(dto);
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         //throw;
        //         return Task.FromResult<AjaxTableDto>(new AjaxTableDto());

        //     }

        // }


        private async Task<AjaxTableDto> PresentViewAjaxHandlerAsync(HttpContext httpContext)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID();

                var draw = httpContext.Request.Form["draw"].FirstOrDefault();
                var start = httpContext.Request.Form["start"].FirstOrDefault();
                var length = httpContext.Request.Form["length"].FirstOrDefault();
                var sortColumn = httpContext.Request.Form["columns[" + httpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDir = httpContext.Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = httpContext.Request.Form["search[value]"].FirstOrDefault();

                string connStr = DatabaseManager.GetConnectionString("DefaultConnection");
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                List<PresentModel> filteredPresent = new List<PresentModel>();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    using (SqlConnection conn = new SqlConnection(connStr))
                    {
                        await conn.OpenAsync();

                        string query = $"SELECT * FROM PresentModels WHERE AGMID = '{UniqueAGMId}' AND Name LIKE '%{searchValue}%' ";
                        if (!string.IsNullOrEmpty(sortColumn))
                        {
                            query = $"SELECT * FROM PresentModels WHERE AGMID = '{UniqueAGMId}' AND Name LIKE '%{searchValue}%' OR ShareholderNum LIKE '%{searchValue}%' ORDER BY {sortColumn} {sortColumnDir}";
                        }

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader read = await cmd.ExecuteReaderAsync())
                            {
                                while (await read.ReadAsync())
                                {
                                    PresentModel model = new PresentModel
                                    {
                                        Id = read.GetInt32(read.GetOrdinal("Id")),
                                        Name = read.GetString(read.GetOrdinal("Name")),
                                        Address = read.GetString(read.GetOrdinal("Address")),
                                        ShareholderNum = read.GetInt64(read.GetOrdinal("ShareholderNum")),
                                        Holding = read.GetDouble(read.GetOrdinal("Holding")),
                                        PercentageHolding = read.GetDouble(read.GetOrdinal("PercentageHolding")),
                                        newNumber = read.GetInt64(read.GetOrdinal("newNumber")),
                                        PhoneNumber = read.GetString(read.GetOrdinal("PhoneNumber")),
                                        emailAddress = read.GetString(read.GetOrdinal("emailAddress")),
                                        present = read.GetBoolean(read.GetOrdinal("present")),
                                        proxy = read.GetBoolean(read.GetOrdinal("proxy")),
                                        split = read.GetBoolean(read.GetOrdinal("split")),
                                        TakePoll = read.GetBoolean(read.GetOrdinal("TakePoll")),
                                        Clikapad = read.GetString(read.GetOrdinal("Clikapad")),
                                        GivenClikapad = read.GetBoolean(read.GetOrdinal("GivenClikapad")),
                                        admitSource = read.GetString(read.GetOrdinal("admitSource")),
                                        ReturnedClikapad = read.GetBoolean(read.GetOrdinal("ReturnedClikapad")),
                                        PermitPoll = read.GetByte(read.GetOrdinal("PermitPoll"))
                                    };
                                    filteredPresent.Add(model);
                                }
                            }
                        }
                    }
                }

                var displayedPresent = filteredPresent
                         .Skip(skip)
                         .Take(pageSize);

                recordsTotal = filteredPresent.Count();

                AjaxTableDto dto = new AjaxTableDto
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    displayedPresentList = displayedPresent.ToList()
                };

                return dto;
            }
            catch (Exception e)
            {
                // Handle the exception appropriately
                return new AjaxTableDto();
            }
        }























        //
        // POST: /BarcodeLib/Edit/5

        [HttpPost]
        public async Task<ActionResult> Edit(int id, PresentModel collection)
        {
            var response = await EditAsync(id, collection);

            return RedirectToAction("Index", "Present");

        }


        private async Task<string> EditAsync(int id, PresentModel collection)
        {
            try
            {

                var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID();
                // TODO: Add update logic here
                var model = db.Present.Find(collection.Id);
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
                if (!String.IsNullOrEmpty(collection.Clikapad))
                {

                    model.GivenClikapad = true;
                    UpdateClikapad(model, collection.Clikapad);
                }
                db.Entry(model).State = EntityState.Modified;
                var barcodemodel = db.BarcodeStore.FirstOrDefault(b => b.Company == companyinfo && b.ShareholderNum == model.ShareholderNum);
                if (!String.IsNullOrEmpty(collection.emailAddress))
                {
                    barcodemodel.emailAddress = collection.emailAddress;
                }

                if (!String.IsNullOrEmpty(collection.PhoneNumber))
                {
                    //number = double.Parse(collection.PhoneNumber);
                    if (collection.PhoneNumber.StartsWith("234"))
                    {
                        barcodemodel.PhoneNumber = collection.PhoneNumber;
                    }
                    else if (collection.PhoneNumber.StartsWith("0"))
                    {
                        var number = double.Parse(collection.PhoneNumber);
                        barcodemodel.PhoneNumber = "234" + number.ToString();

                    }

                }
                //if (!String.IsNullOrEmpty(collection.Clikapad))
                //{
                barcodemodel.Clikapad = collection.Clikapad;

                //}
                db.Entry(barcodemodel).State = EntityState.Modified;
                db.SaveChanges();
                // TempData["Message"] = "Edited";
                _tempDataManager.SetTempData("Message", "Edited");
                return "success";
            }
            catch
            {
                // TempData["Message1"] = "Cannot Edit database";
                _tempDataManager.SetTempData("Message", "Cannot Edit database");

                return "failed";
            }
        }


        private void UpdateClikapad(PresentModel model, string clikapad)
        {
            try
            {
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(clikapad))
                    {
                        var checkOTherAccountsExist = db.Present.Where(b => b.Company == model.Company && b.emailAddress == model.emailAddress);
                        if (checkOTherAccountsExist != null)
                        {
                            if (checkOTherAccountsExist.Count() == 1)
                            {
                                checkOTherAccountsExist.First().Clikapad = clikapad;
                                checkOTherAccountsExist.First().GivenClikapad = true;
                                var shareholdernumber = checkOTherAccountsExist.First().ShareholderNum;
                                var company = checkOTherAccountsExist.First().Company;
                                var checkifpresent = db.BarcodeStore.FirstOrDefault(f => f.Company == company && f.ShareholderNum == shareholdernumber);
                                if (checkifpresent != null)
                                {

                                    checkifpresent.Selected = false;
                                    checkifpresent.Clikapad = clikapad;
                                }

                                db.SaveChanges();
                            }
                            else if (checkOTherAccountsExist.Count() > 1)
                            {
                                foreach (var item in checkOTherAccountsExist)
                                {
                                    item.Clikapad = clikapad;
                                    item.GivenClikapad = true;
                                    var shareholdernumber = item.ShareholderNum;
                                    var checkifpresent = db.BarcodeStore.FirstOrDefault(f => f.Company == item.Company && f.ShareholderNum == shareholdernumber);
                                    if (checkifpresent != null)
                                    {

                                        checkifpresent.Selected = false;
                                        checkifpresent.Clikapad = clikapad;


                                    }
                                }
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

        }
        //
        // GET: /Present/Delete/5
        [HttpPost]
        public async Task<string> CheckClikapadStatus(int id, QuestionStatus data)
        {
            var response = await CheckClikapadStatusAsync(id, data);

            return response;
        }


        private Task<string> CheckClikapadStatusAsync(int id, QuestionStatus data)
        {
            try
            {
                var shareholder = db.Present.Find(id);
                if (shareholder != null)
                {
                    shareholder.ReturnedClikapad = data.status;
                    UpdateReturnedClikapad(shareholder, data.status);

                }

                db.Entry(shareholder).State = EntityState.Modified;
                db.SaveChanges();

                return Task.FromResult<string>("success");
            }
            catch (Exception e)
            {
                return Task.FromResult<string>("Failed");
            }
        }

        private void UpdateReturnedClikapad(PresentModel model, bool status)
        {
            try
            {
                if (model != null)
                {
                    var checkOTherAccountsExist = db.Present.Where(b => b.Company == model.Company && b.emailAddress == model.emailAddress);
                    if (checkOTherAccountsExist != null)
                    {
                        if (checkOTherAccountsExist.Count() == 1)
                        {

                            checkOTherAccountsExist.First().ReturnedClikapad = status;

                            db.SaveChanges();
                        }
                        else if (checkOTherAccountsExist.Count() > 1)
                        {
                            foreach (var item in checkOTherAccountsExist)
                            {
                                item.ReturnedClikapad = status;
                            }
                            db.SaveChanges();
                        }
                    }
                }

            }
            catch (Exception e)
            {

            }


        }



        // POST: /Present/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return Ok();
            }
        }


        // [HttpGet]
        //         public ActionResult ExportToExcel()
        //         {
        //             using (ExcelEngine excelEngine = new ExcelEngine())
        //             {
        //                 IApplication application = excelEngine.Excel;
        //                 application.DefaultVersion = ExcelVersion.Excel2016;

        //                 //Create a new workbook 
        //                 IWorkbook workbook = application.Workbooks.Create(1);
        //                 IWorksheet sheet = workbook.Worksheets[0];

        //                 if (sheet.ListObjects.Count == 0)
        //                 {
        //                     //Estabilishing the connection in the worksheet 
        //                     // string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //                     string connectionString = DatabaseManager.GetConnectionString("DefaultConnection");

        //                     string query = "SELECT * FROM PresentModels";

        //                     IConnection connection = workbook.Connections.Add("SQLConnection", "Export Present Attendees SQL Server", connectionString, query, ExcelCommandType.Sql);

        //                     //Create Excel table from external connection. 
        //                     sheet.ListObjects.AddEx(ExcelListObjectSourceType.SrcQuery, connection, sheet.Range["A1"]);
        //                 }

        //                 //Refresh Excel table to get updated values from database 
        //                 sheet.ListObjects[0].Refresh();

        //                 sheet.UsedRange.AutofitColumns();

        //                 //Save the file in the given path 
        //                 //Stream excelStream = File.Create(Path.GetFullPath(@"Output.xlsx"));

        //                 //workbook.SaveAs(excelStream);
        //                 workbook.SaveAs("Output.xlsx");
        //                 //excelStream.Dispose();

        //                 return RedirectToAction("Index");
        //             }
        //         }



        [HttpGet]
        public ActionResult ExportToExcel()
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Excel2016;

                    // Create a new workbook 
                    IWorkbook workbook = application.Workbooks.Create(1);
                    IWorksheet sheet = workbook.Worksheets[0];

                    if (sheet.ListObjects.Count == 0)
                    {
                        // Establish the connection in the worksheet 
                        string connectionString = DatabaseManager.GetConnectionString("DefaultConnection");

                        string query = "SELECT * FROM PresentModels";

                        IConnection connection = workbook.Connections.Add("SQLConnection", "Export Present Attendees SQL Server", connectionString, query, ExcelCommandType.Sql);

                        // Create Excel table from external connection. 
                        sheet.ListObjects.AddEx(ExcelListObjectSourceType.SrcQuery, connection, sheet.Range["A1"]);
                    }

                    // Refresh all connections associated with the workbook
                    foreach (IConnection connection in workbook.Connections)
                    {
                        //              TODO : HANDLE REFRESHING MANUALLY
                        // You may need to handle the refreshing of connections manually.
                        // Depending on the library you are using, this could involve closing and reopening the connection,
                        // or executing some kind of refresh command. Refer to the documentation of the library for details.
                    }
                    sheet.UsedRange.AutofitColumns();



                    // Save the file in the given path 
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Output.xlsx");
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Output.xlsx");
                    }
                    // Return the Excel file for download
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Output.xlsx");
                }
            }

            catch (Exception ex)
            {
                // Handle the exception appropriately, for example, logging it.
                return BadRequest("Error occurred while exporting to Excel: " + ex.Message);
            }


        }
    }
}

