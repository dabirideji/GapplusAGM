using BarcodeGenerator.Models;
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
    public class GenBarcodeController : ControllerBase
    {
        //
        // GET: /GenBarcode/

//PREVIOUS
        // public ActionResult Index()
        // {
            
        //     //IDAutomation.NetAssembly.FontEncoder FontEncoder = new IDAutomation.NetAssembly.FontEncoder();

        //     string Code = "*12345678*"; 
        //     //string encoded = FontEncoder.Code39(Code);

        //     int w = Code.Length * 40;
        //     byte[] BarCode;
        //     string BarCodeImage;
        //         // Create a bitmap object of the width that we calculated and height of 100
        //         Bitmap oBitmap = new Bitmap(w, 150);
        //         // then create a Graphic object for the bitmap we just created.
        //         Graphics oGraphics = Graphics.FromImage(oBitmap);
        //         // Now create a Font object for the Barcode Font
        //         // (in this case the IDAutomationHC39M) of 18 point size
        //         Font oFont = new Font("Free 3 of 9 Extended", 62);
        //         // Let's create the Point and Brushes for the barcode
        //         PointF oPoint = new PointF(2f, 2f);
        //         SolidBrush oBrushWrite = new SolidBrush(Color.Black);
        //         SolidBrush oBrush = new SolidBrush(Color.White);
        //         // Now lets create the actual barcode image
        //         // with a rectangle filled with white color
        //         oGraphics.FillRectangle(oBrush, 0, 0, w, 100);
        //         // We have to put prefix and sufix of an asterisk (*),
        //         // in order to be a valid barcode
        //         oGraphics.DrawString(Code, oFont, oBrushWrite, oPoint);
        //         // Then we send the Graphics with the actual barcode
        //         System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
        //         using (MemoryStream Mmst = new MemoryStream())
        //         {
        //             oBitmap.Save(Mmst, ImageFormat.Png);
        //             BarCode = Mmst.GetBuffer();
        //             BarCodeImage = BarCode != null ? "data:image/jpg;base64," + Convert.ToBase64String((byte[])BarCode) : "";

        //         }
        //         oBitmap.Dispose();
        //        testModel model = new testModel();
        //        model.ImageUrl = BarCodeImage;
        //        return View(model);
        // }












//CURRENT 
  [HttpGet]
        public ActionResult<string> Index()
        {
            string code = "*12345678*";
            int w = code.Length * 40;

            byte[] barCode;
            string barCodeImage;

            using (Bitmap oBitmap = new Bitmap(w, 150))
            {
                using (Graphics oGraphics = Graphics.FromImage(oBitmap))
                {
                    using (Font oFont = new Font("Free 3 of 9 Extended", 62))
                    {
                        PointF oPoint = new PointF(2f, 2f);
                        SolidBrush oBrushWrite = new SolidBrush(Color.Black);
                        SolidBrush oBrush = new SolidBrush(Color.White);

                        oGraphics.FillRectangle(oBrush, 0, 0, w, 100);
                        oGraphics.DrawString(code, oFont, oBrushWrite, oPoint);

                        using (MemoryStream Mmst = new MemoryStream())
                        {
                            oBitmap.Save(Mmst, ImageFormat.Png);
                            barCode = Mmst.ToArray();
                            barCodeImage = barCode != null ? "data:image/png;base64," + Convert.ToBase64String(barCode) : "";
                        }
                    }
                }
            }

            return Ok(barCodeImage);
        }




        //
        // GET: /GenBarcode/Details/5

        // public ActionResult Details(int id)
        // {
        //     return Ok();
        // }

        //
        // GET: /GenBarcode/Create

        // public ActionResult Create()
        // {
        //     return Ok();
        // }

        //
        // POST: /GenBarcode/Create

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
        // GET: /GenBarcode/Edit/5
        [HttpPut("{id}")]
        public ActionResult Edit(int id)
        {
            return Ok();
        }

        //
        // POST: /GenBarcode/Edit/5

        [HttpPost("{id}")]
        public ActionResult Edit(int id, FormCollection collection)
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
        // GET: /GenBarcode/Delete/5
     
        // public ActionResult Delete(int id)
        // {
        //     return Ok();
        // }

        //
        // POST: /GenBarcode/Delete/5

          [HttpDelete("{id}")]
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
