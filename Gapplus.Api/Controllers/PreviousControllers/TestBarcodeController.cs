using BarcodeGenerator.Models;
using BarCodeGenerator.Barcode;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;


namespace BarcodeGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestBarcodeController : ControllerBase
    {
        //
        // GET: /TestBarcode/
[HttpGet]
        public ActionResult Index()
        {
            string Code = "12345678";
            byte[] BarCode;
            string BarCodeImage;
            //byte[] BarCodeImage;
            // Bitmap objBitmap = new Bitmap(Code.Length * 28, 100);
            Bitmap objBitmap = new Bitmap(Code.Length * 40, 150);
            using (Graphics graphic = Graphics.FromImage(objBitmap))
            {
                //Font newFont = new Font("IDAutomationHC39M Free Version", 18, FontStyle.Regular);
                Font newFont = new System.Drawing.Font("Free 3 of 9", 30);
                PointF point = new PointF(2f, 2f);
                SolidBrush balck = new SolidBrush(Color.Black);
                SolidBrush white = new SolidBrush(Color.White);
                graphic.FillRectangle(white, 0, 0, objBitmap.Width, objBitmap.Height);
                graphic.DrawString("*" + Code + "*", newFont, balck, point);
            }
            using (MemoryStream Mmst = new MemoryStream())
            {
                objBitmap.Save(Mmst, ImageFormat.Png);
                BarCode = Mmst.GetBuffer();
                BarCodeImage = BarCode != null ? "data:image/jpg;base64," + Convert.ToBase64String((byte[])BarCode) : "";
               
            }
            testModel model = new testModel();
            model.ImageUrl = BarCodeImage;
            return Ok(model);
        }

        //
        // GET: /TestBarcode/Details/5

 

        //
        // GET: /TestBarcode/Create

     

        //
        // POST: /TestBarcode/Create

        [HttpPost]
        public ActionResult Create(IFormFile collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return Ok();
            }
        }

        //
        // GET: /TestBarcode/Edit/5


        //
        // POST: /TestBarcode/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, IFormFile collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return Ok();
            }
        }

        //
        // GET: /TestBarcode/Delete/5



        //
        // POST: /TestBarcode/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, IFormFile collection)
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
    }
}
