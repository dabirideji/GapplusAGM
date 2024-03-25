// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Web;
// using System.Web.Mvc;

// namespace BarcodeGenerator.Controllers
// {
//     public class BarcodeGenController : Controller
//     {
        
//         //
//         // GET: /NeoBarcode/
//         public void GetBarcodeImage(string valueToEncode)
//         {

//             IDAutomation.NetAssembly.FontEncoder FontEncoder = new IDAutomation.NetAssembly.FontEncoder();

//             valueToEncode = "123456789";
//             string encodedtxt = FontEncoder.Code39(valueToEncode);

//             //Create an instance of BarcodeProfessional class
//             using (Neodynamic.Web.MVC.Barcode.BarcodeProfessional bcp = new
//                             Neodynamic.Web.MVC.Barcode.BarcodeProfessional())
//             {
//                 //Set the desired barcode type or symbology
//                 bcp.Symbology = Neodynamic.Web.MVC.Barcode.Symbology.Code39;
//                 //Set value to encode
//                 bcp.Code = encodedtxt;
//                 //Generate barcode image
//                 byte[] imgBuffer = bcp.GetBarcodeImage(
//                              System.Drawing.Imaging.ImageFormat.Png);
//                 //Write image buffer to Response obj
//                 System.Web.HttpContext.Current.Response.ContentType = "image/png";
//                 System.Web.HttpContext.Current.Response.BinaryWrite(imgBuffer);
//             }
//         }

//         public ActionResult Index()
//         {
//             return View();
//         }

//         //
//         // GET: /NeoBarcode/Details/5

//         public ActionResult Details(int id)
//         {
//             return View();
//         }

//         //
//         // GET: /NeoBarcode/Create

//         public ActionResult Create()
//         {
//             return View();
//         }

//         //
//         // POST: /NeoBarcode/Create

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
//         // GET: /NeoBarcode/Edit/5

//         public ActionResult Edit(int id)
//         {
//             return View();
//         }

//         //
//         // POST: /NeoBarcode/Edit/5

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
//         // GET: /NeoBarcode/Delete/5

//         public ActionResult Delete(int id)
//         {
//             return View();
//         }

//         //
//         // POST: /NeoBarcode/Delete/5

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
