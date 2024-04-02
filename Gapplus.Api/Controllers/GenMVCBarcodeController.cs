using BarcodeGenerator.Barcode;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GenMVCBarcodeController : ControllerBase
    {
        private readonly IViewBagManager _viewBagManager;

        //

        public GenMVCBarcodeController(IViewBagManager viewBagManager)
        {
            _viewBagManager = viewBagManager;
        }
        // GET: /GenMVCBarcode/


        [HttpGet]
        public ActionResult Index()
        {
            string vCode = "nha3mien.com";
            string barCode = BarCodeToHTML.get39(vCode, 2, 20);


            // ViewBag.Message = "MVC WEB BARCODE!!";
            // ViewBag.htmlBarcode = barCode;
            // ViewBag.vCode = vCode;
            _viewBagManager.SetValue("Message", "MVC WEB BARCODE!!");
            _viewBagManager.SetValue("htmlBarcode", barCode);
            _viewBagManager.SetValue("vCode", vCode);


            // return View();
            return Ok();

        }



        // [HttpPost]
        // public ActionResult Index(FormCollection f)
        // {
        //     ViewBag.Message = "MVC WEB BARCODE!";
        //     var vCode = f["txtcode"];
        //     string barCode = BarCodeToHTML.get39(vCode, 2, 20);
        //     ViewBag.htmlBarcode = barCode;
        //     ViewBag.vCode = vCode;

        //     return View();
        // }

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            _viewBagManager.SetValue("Message", "MVC WEB BARCODE!");
            var vCode = form["txtcode"];
            string barCode = BarCodeToHTML.get39(vCode, 2, 20);
            _viewBagManager.SetValue("htmlBarcode", barCode);
            _viewBagManager.SetValue("vCode", vCode);

            return Ok(); // or return View();
        }

        //
        // GET: /GenMVCBarcode/Details/5

        // public ActionResult Details(int id)
        // {
        //     // return View();
        //     return Ok();
        // }

        //
        // GET: /GenMVCBarcode/Create

        // public ActionResult Create()
        // {
        //     // return View();
        //     return Ok();
        // }

        //
        // POST: /GenMVCBarcode/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                // return View();
                return BadRequest();
            }
        }

        //
        // GET: /GenMVCBarcode/Edit/5

        // public ActionResult Edit(int id)
        // {
        //     // return View();
        //     return Ok();
        // }

        //
        // POST: /GenMVCBarcode/Edit/5

        [HttpPut]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                // return View();
                return Ok();
            }
        }

        //
        // GET: /GenMVCBarcode/Delete/5

        // public ActionResult Delete(int id)
        // {
        //     // return View();
        //     return Ok();
        // }

        //
        // POST: /GenMVCBarcode/Delete/5

        [HttpDelete]
        public ActionResult Delete(int id, FormCollection collection)
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
