using BarcodeGenerator.Barcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarcodeGenerator.Controllers
{
    public class GenMVCBarcodeController : Controller
    {
        //
        // GET: /GenMVCBarcode/

        public ActionResult Index()
        {
            ViewBag.Message = "MVC WEB BARCODE!!";
            string vCode = "nha3mien.com";
            string barCode = BarCodeToHTML.get39(vCode, 2, 20);
            ViewBag.htmlBarcode = barCode;
            ViewBag.vCode = vCode;
            return View();

        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            ViewBag.Message = "MVC WEB BARCODE!";
            var vCode = f["txtcode"];
            string barCode = BarCodeToHTML.get39(vCode, 2, 20);
            ViewBag.htmlBarcode = barCode;
            ViewBag.vCode = vCode;

            return View();
        }
        //
        // GET: /GenMVCBarcode/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /GenMVCBarcode/Create

        public ActionResult Create()
        {
            return View();
        }

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
                return View();
            }
        }

        //
        // GET: /GenMVCBarcode/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /GenMVCBarcode/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /GenMVCBarcode/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /GenMVCBarcode/Delete/5

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
                return View();
            }
        }
    }
}
