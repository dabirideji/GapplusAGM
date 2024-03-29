using AutoMapper;
using BarcodeGenerator.Barcode;
using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using Gapplus.Application.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BarcodeController : ControllerBase
    {
        UsersContext db;
        UserAdmin ua;


        private readonly ITempDataManager _tempDataManager;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public BarcodeController(UsersContext _db, ITempDataManager tempDataManager,IWebHostEnvironment webHostEnviroment)
        {
            _webHostEnviroment = webHostEnviroment;
            db = _db;
            ua = new UserAdmin(db);
            _tempDataManager = tempDataManager;
        }
        //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

        //private int RetrieveAGMUniqueID()
        //{
        //    var AGMUniqueID = db.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

        //    return AGMUniqueID;
        //}

        public int PageSize = 1000;
        //
        // GET: /Barcode/

        [HttpGet]
        public ActionResult Index()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> AjaxHandler()
        {
            var response = await AjaxHandlerAsync();

            // return Json(new { draw = response.draw, recordsFiltered = response.recordsTotal, recordsTotal = response.recordsTotal, data = response.displayedList }, JsonRequestBehavior.AllowGet);
            var res = new { draw = response.draw, recordsFiltered = response.recordsTotal, recordsTotal = response.recordsTotal, data = response.displayedList };
            return Ok(res);
        }

        private Task<AjaxTableDto> AjaxHandlerAsync()
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                //Creating instance of DatabaseContext class  
                using (UsersContext _context = db)
                {


                    // var draw = Request.Form.GetValues("draw").FirstOrDefault();  
                    // var start = Request.Form.GetValues("start").FirstOrDefault();  
                    // var length = Request.Form.GetValues("length").FirstOrDefault();  
                    // var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();  
                    // var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();  
                    // var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();  

                    var draw = Request.Form["draw"].FirstOrDefault();
                    var start = Request.Form["start"].FirstOrDefault();
                    var length = Request.Form["length"].FirstOrDefault();
                    var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
                    var sortColumn = Request.Form["columns[" + sortColumnIndex + "][name]"].FirstOrDefault();
                    var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
                    var searchValue = Request.Form["search[value]"].FirstOrDefault();

                    //Paging Size (10,20,50,100)    
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int recordsTotal = 0;
                    // Getting all Shareholder data 

                    List<BarcodeModel> filteredShareholders = new List<BarcodeModel>();

                    //Sorting
                    //    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    //{
                    //    filteredShareholders = allCompanies.OrderBy(s => s.Id);
                    //}
                    //Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        // string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                        string connStr = DatabaseManager.GetConnectionString();

                        SqlConnection conn =
                                new SqlConnection(connStr);

                        var isNumeric = int.TryParse(searchValue, out _);
                        string query;
                        if (isNumeric)
                        {
                            query = "select * from BarcodeModels WHERE Company = '" + companyinfo + "' AND ShareholderNum = '" + searchValue + "'";

                            if (sortColumn != "")
                            {
                                query = "select * from BarcodeModels WHERE Company = '" + companyinfo + "' AND ShareholderNum = '" + searchValue + "' Order By " + sortColumn + " " + sortColumnDir + "";

                            }
                        }
                        else
                        {
                            query = "select * from BarcodeModels WHERE Company = '" + companyinfo + "' AND Name LIKE '%" + searchValue + "%'";

                            if (sortColumn != "")
                            {
                                query = "select * from BarcodeModels WHERE Company = '" + companyinfo + "' AND Name LIKE '%" + searchValue + "%' Order By " + sortColumn + " " + sortColumnDir + "";

                            }
                        }

                        //string query1 = "UPDATE BarcodeModels WHERE Company =  '" + companyinfo + "' AND Selected = 1";
                        //conn.Open();
                        //SqlCommand cmd1 = new SqlCommand(query1, conn);
                        //cmd1.ExecuteNonQuery();
                        //conn.Close();
                        //string query2 = "select * from BarcodeModels WHERE CONTAINS(Name,'" + searchValue + "')";
                        //string query2 = "select * from BarcodeModels WHERE Company = '" + companyinfo + "' AND Name LIKE '%" + searchValue + "%'";
                        //string query2 = "select * from BarcodeModels WHERE Consolidated = 'FALSE' AND (Name LIKE '%" + searchValue + "%' OR ShareholderNum LIKE '%" + searchValue + "%')";
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(query, conn);
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
                                Holding = double.Parse(read["Holding"].ToString()),
                                PercentageHolding = double.Parse(read["PercentageHolding"].ToString()),
                                Company = (read["Company"].ToString()),
                                PhoneNumber = (read["PhoneNumber"].ToString()),
                                emailAddress = (read["emailAddress"].ToString()),
                                Clikapad = (read["Clikapad"].ToString()),
                                Present = bool.Parse(read["Present"].ToString()),
                                Selected = bool.Parse(read["Selected"].ToString()),
                                Consolidated = bool.Parse(read["Consolidated"].ToString()),
                                PresentByProxy = bool.Parse(read["PresentByProxy"].ToString()),
                                split = bool.Parse(read["split"].ToString()),
                                resolution = bool.Parse(read["resolution"].ToString()),
                                TakePoll = bool.Parse(read["TakePoll"].ToString()),
                                NotVerifiable = bool.Parse(read["NotVerifiable"].ToString()),
                                accesscode = (read["accesscode"].ToString())

                            };
                            filteredShareholders.Add(model);
                        }
                        read.Close();
                    }

                    //Paging   
                    var displayedCompanies = filteredShareholders
                                 .Skip(skip)
                                 .Take(pageSize);

                    //total number of rows count     
                    recordsTotal = filteredShareholders.Count();

                    AjaxTableDto dto = new AjaxTableDto
                    {
                        draw = draw,
                        recordsTotal = recordsTotal,
                        recordsFiltered = recordsTotal,
                        displayedList = displayedCompanies
                    };

                    //Returning Json Data    
                    return Task.FromResult<AjaxTableDto>(dto);
                }
            }
            catch (Exception)
            {
                //throw;  
                return Task.FromResult<AjaxTableDto>(new AjaxTableDto());
            }

        }


        [HttpGet]
        public async Task<ActionResult> AjaxHandlerNonShareholder()
        {
            var response = await AjaxHandlerNonShareholderAsync();

            // return Json(new { draw = response.draw, recordsFiltered = response.recordsTotal, recordsTotal = response.recordsTotal, data = response.displayedNonShareholderList}, JsonRequestBehavior.AllowGet);
            var res = new { draw = response.draw, recordsFiltered = response.recordsTotal, recordsTotal = response.recordsTotal, data = response.displayedNonShareholderList };
            return Ok(res);
        }


        private Task<AjaxTableDto> AjaxHandlerNonShareholderAsync()
        {
            try
            {
                //Creating instance of DatabaseContext class  
                using (UsersContext _context = db)
                {
                    var companyinfo = ua.GetUserCompanyInfo();
                    var UniqueAGMId = ua.RetrieveAGMUniqueID();

                    var draw = Request.Form["draw"].FirstOrDefault();
                    var start = Request.Form["start"].FirstOrDefault();
                    var length = Request.Form["length"].FirstOrDefault();
                    var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
                    var sortColumn = Request.Form["columns[" + sortColumnIndex + "][name]"].FirstOrDefault();
                    var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
                    var searchValue = Request.Form["search[value]"].FirstOrDefault();









                    // string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    string connStr = DatabaseManager.GetConnectionString();
                    //Paging Size (10,20,50,100)    
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int recordsTotal = 0;
                    List<Facilitators> AllFacilitators = new List<Facilitators>();
                    List<Facilitators> filteredFacilitators = new List<Facilitators>();
                    SqlConnection conn =
                              new SqlConnection(connStr);
                    string query2 = "select * from Facilitators where AGMID = '" + UniqueAGMId + "'";

                    if (sortColumn != "")
                    {
                        query2 = "select * from Facilitators where AGMID = '" + UniqueAGMId + "' Order By " + sortColumn + " " + sortColumnDir + "";

                    }

                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query2, conn);
                    SqlDataReader read = cmd.ExecuteReader();

                    while (read.Read())
                    {
                        Facilitators model = new Facilitators
                        {


                            Id = int.Parse(read["Id"].ToString()),
                            SN = (read["SN"].ToString()),
                            Name = (read["Name"].ToString()),
                            Company = (read["Company"].ToString()),
                            FacilitatorCompany = (read["FacilitatorCompany"].ToString()),
                            PhoneNumber = (read["PhoneNumber"].ToString()),
                            emailAddress = (read["emailAddress"].ToString()),
                            ResourceType = (read["ResourceType"].ToString()),
                            AGMID = int.Parse(read["AGMID"].ToString()),
                            accesscode = (read["accesscode"].ToString())


                        };
                        AllFacilitators.Add(model);
                    }
                    read.Close();
                    //Sorting  
                    //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    //{
                    //    //filteredpresent = allPresent.OrderBy(s => s.Id);
                    //    filteredpresent = Allpresent.OrderBy(s=>s.Id).ToList();
                    //}
                    //Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {


                        using (SqlConnection conn1 =
                                 new SqlConnection(connStr))
                        {

                            query2 = "select * from Facilitators where AGMID = '" + UniqueAGMId + "' AND Name LIKE '%" + searchValue + "%'";

                            if (sortColumn != "")
                            {
                                query2 = "select * from Facilitators where AGMID = '" + UniqueAGMId + "' AND Name LIKE '%" + searchValue + "%' Order By " + sortColumn + " " + sortColumnDir + "";

                            }


                            conn1.Open();
                            SqlCommand cmd1 = new SqlCommand(query2, conn1);
                            SqlDataReader read1 = cmd1.ExecuteReader();

                            while (read1.Read())
                            {
                                Facilitators model1 = new Facilitators
                                {

                                    Id = int.Parse(read1["Id"].ToString()),
                                    SN = (read1["SN"].ToString()),
                                    Name = (read1["Name"].ToString()),
                                    Company = (read1["Company"].ToString()),
                                    FacilitatorCompany = (read1["FacilitatorCompany"].ToString()),
                                    PhoneNumber = (read1["PhoneNumber"].ToString()),
                                    emailAddress = (read1["emailAddress"].ToString()),
                                    ResourceType = (read1["ResourceType"].ToString()),
                                    AGMID = int.Parse(read1["AGMID"].ToString()),
                                    accesscode = (read["accesscode"].ToString())


                                };
                                filteredFacilitators.Add(model1);
                            }
                            read1.Close();
                        }
                    }
                    else
                    {
                        filteredFacilitators = AllFacilitators.ToList();
                    }
                    //Paging   
                    var displayedpresent = filteredFacilitators
                             .Skip(skip)
                             .Take(pageSize);

                    //total number of rows count     
                    recordsTotal = filteredFacilitators.Count();

                    AjaxTableDto dto = new AjaxTableDto
                    {
                        draw = draw,
                        recordsTotal = recordsTotal,
                        recordsFiltered = recordsTotal,
                        displayedNonShareholderList = displayedpresent
                    };
                    //Returning Json Data    
                    return Task.FromResult<AjaxTableDto>(dto);
                }
            }
            catch (Exception e)
            {
                //throw;  
                return Task.FromResult<AjaxTableDto>(new AjaxTableDto());
            }

        }


        // GET: /Barcode/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var response = await DetailsAsync(id);

            // return View(response);
            return Ok(response);
        }


        private Task<BarcodeModel> DetailsAsync(int id)
        {
            var barcode = db.BarcodeStore.Find(id);
            BarcodeModel model = new BarcodeModel();


            model.Name = barcode.Name.ToString();

            model.Barcode = barcode.Barcode.ToString();
            model.ImageUrl = barcode.BarcodeImage != null ? "data:image/jpg;base64," +
                 Convert.ToBase64String((byte[])barcode.BarcodeImage) : "";


            return Task.FromResult<BarcodeModel>(model);
        }

        //
        // GET: /Barcode/Create

        // public ActionResult Create()
        // {
        //     // return View();
        //     return Ok();
        // }

        //
        // POST: /Barcode/Create

        [HttpPost]
        public ActionResult Create([FromBody] BarcodeModelView model)
        // public ActionResult Create([FromBody] FakeBarCodeModelDto model, [FromServices] IMapper _mapper)
        {
            try
            {
                // TODO: Add insert logic here

                barcodecs objbar = new barcodecs(_webHostEnviroment);      //todo



                BarcodeModel objprod = new BarcodeModel()
                {
                    Name = model.FirstName,

                    Barcode = objbar.generateBarcode(), //todo
                    BarcodeImage = objbar.getBarcodeImage(objbar.generateBarcode(), model.LastName.ToUpper()) //todo
                   
                   
                    // Barcode = "",
                    // BarcodeImage = new byte[] { }
                };

                // var objprod = _mapper.Map<BarcodeModel>(model);
                // objprod.BarcodeImage = new byte[] {};


                db.BarcodeStore.Add(objprod);
                db.SaveChanges();
                // return Json("Success", JsonRequestBehavior.AllowGet);
                return Ok("success");
            }
            catch
            {
                // return Json("Failed", JsonRequestBehavior.AllowGet);
                return BadRequest("Failed");
            }
        }



        [HttpGet]
        public ActionResult CreateNonShareholder()
        {
            var agmid = ua.RetrieveAGMUniqueID();
            Facilitators model = new Facilitators();
            model.AGMID = agmid;
            // return PartialView(model);
            return Ok(model);
        }

        //
        // POST: /Barcode/Create

        [HttpPost]
        public ActionResult CreateNonShareholder([FromBody] Facilitators model)
        {
            try
            {

                Facilitators fa = new Facilitators()
                {
                    SN = GetSN(model.AGMID),
                    Name = model.Name,
                    Company = GetCompany(model.AGMID),
                    FacilitatorCompany = model.FacilitatorCompany,
                    ResourceType = model.ResourceType,
                    PhoneNumber = model.PhoneNumber,
                    emailAddress = model.emailAddress,
                    AGMID = model.AGMID
                };

                fa.accesscode = ua.GetAccessCode();
                // fa.OnlineEventUrl = GetEventUrl(model.emailAddress, model.AGMID);  ///MUST CHECK HERE LATER ///////////
                fa.OnlineEventUrl = "";  ///MUST CHECK HERE LATER ///////////

                db.Facilitators.Add(fa);
                db.SaveChanges();
                // return Json("Success", JsonRequestBehavior.AllowGet);
                return Ok("Success");
            }
            catch
            {
                // return Json("Failed", JsonRequestBehavior.AllowGet);
                return BadRequest("Failed");
            }
        }


        private string GetSN(int agmid)
        {
            var sn = db.Facilitators.Where(f => f.AGMID == agmid).Count();
            return (sn + 1).ToString();

        }

        private string GetCompany(int agmid)
        {
            var setting = db.Settings.SingleOrDefault(s => s.AGMID == agmid);
            if (setting != null)
            {
                return setting.CompanyName;
            }
            return "";
        }
































        //  private string GetAccessCode(string company)
        //     {

        //         DataUploadController du = new DataUploadController();
        //         var accesscode = du.GenerateFacilitatorsAccessCode(company);
        //         return accesscode;
        //     }





        //     private string GetEventUrl(string emailAddress, int AGMId)
        //     {

        //         DataUploadController du = new DataUploadController();
        //         var EventUrl = du.GenerateFacilitatorsAGMUrl(AGMId, emailAddress);

        //         return EventUrl;
        //     }








































        //
        // GET: /Barcode/Edit/5


        [HttpPut("{id:int}")]
        public ActionResult Edit(int id)
        {
            var model = db.Facilitators.Find(id);
            // return PartialView(model);
            return Ok(model);
        }

        [HttpPost("id")]
        public async Task<ActionResult> Edit([FromRoute] int id, [FromBody] Facilitators collection)
        {
            var response = await EditAsync(id, collection);

            // return Json(response, JsonRequestBehavior.AllowGet);
            return Ok(response);

        }



        private Task<string> EditAsync(int id, Facilitators collection)
        {
            try
            {
                var companyinfo = ua.GetUserCompanyInfo();
                var UniqueAGMId = ua.RetrieveAGMUniqueID();
                // TODO: Add update logic here
                var model = db.Facilitators.Find(collection.Id);

                model.PhoneNumber = collection.PhoneNumber;

                model.emailAddress = collection.emailAddress;

                model.Name = collection.Name;
                model.ResourceType = collection.ResourceType;
                model.FacilitatorCompany = collection.FacilitatorCompany;

                db.Entry(model).State = EntityState.Modified;

                db.SaveChanges();
                return Task.FromResult<string>("success");
            }
            catch
            {
                // TempData["Message1"] = "Cannot Edit database";
                _tempDataManager.SetTempData("Message1", "Cannot Edit database");
                return Task.FromResult<string>("failed");
            }
        }
        //
        // GET: /Barcode/Delete/5



        //
        // POST: /Barcode/Delete/5

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                // return View();
                return Ok();
            }
        }
    }
}
