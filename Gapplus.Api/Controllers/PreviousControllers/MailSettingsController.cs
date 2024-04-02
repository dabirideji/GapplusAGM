// using BarcodeGenerator.Models;
// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Data.Entity;
// using System.Linq;
// using System.Web;
// using System.Web.Mvc;

// namespace BarcodeGenerator.Controllers
// {
//     public class MailSettingsController : Controller
//     {
//         //
//         private UsersContext db = new UsersContext();
//         //
//         // GET: /MailSettings/

//         public ActionResult Index()
//         {
//             var mailserver = new SelectList(new[] { "Mail Server 1", "Mail Server 2" });
//             ViewBag.mailserver = mailserver;

//             var mailsettings = db.mailsettings.ToList();
//             return View(mailsettings);
//         }

//         //
//         // GET: /MailSettings/Details/5

//         public ActionResult Details(int id = 0)
//         {
//             var mailsettings = db.mailsettings.Find(id);
//             return View(mailsettings);
//         }

//         //
//         // GET: /MailSettings/Create

//         public ActionResult Create()
//         {
//             var mailserver = new SelectList(new[] { "Mail Server 1", "Mail Server 2" });
//             ViewBag.mailserver = mailserver;
//             MailSettings mailsettings = new MailSettings();

//             return PartialView(mailsettings);
//         }

//         //
//         // POST: /MailSettings/Create

//         [HttpPost]
//         public ActionResult Create(MailSettings collection)
//         {
//             var mailserver = new SelectList(new[] { "Mail Server 1", "Mail Server 2" });
//             ViewBag.mailserver = mailserver;
//             try
//             {
//                 if (ModelState.IsValid)
//                 {
//                     var setting = db.mailsettings.ToArray();
//                     if (setting.Length == 0 || setting.Length < 1)
//                     {
//                         db.mailsettings.Add(collection);
//                         db.SaveChanges();
                       
//                     }
//                     return RedirectToAction("Index", "Settings");
//                 }
//                 // TODO: Add insert logic here

//                 return RedirectToAction("Index","Settings");
//             }
//             catch
//             {
//                 return RedirectToAction("Index", "Settings");
//             }
//         }

//         //
//         // GET: /MailSettings/Edit/5

//         public ActionResult Edit(int id = 0)
//         {
//             var mailserver = new SelectList(new[] { "Mail Server 1", "Mail Server 2" });
//             ViewBag.mailserver = mailserver;
//             var mailsettings = db.mailsettings.Find(id);
//             if (mailsettings == null)
//             {
//                 return HttpNotFound();
//             }
//             return PartialView(mailsettings);
//         }

//         //
//         // POST: /MailSettings/Edit/5

//         [HttpPost]
//         public ActionResult Edit(int id, MailSettings collection)
//         {
//             var mailserver = new SelectList(new[] { "Mail Server 1", "Mail Server 2" });
//             ViewBag.mailserver = mailserver;
//             try
//             {
//                 if (ModelState.IsValid)
//                 {
//                     db.Entry(collection).State = EntityState.Modified;
//                     db.SaveChanges();
//                     return RedirectToAction("Index","Settings");
//                 }

//                 return RedirectToAction("Index", "Settings");
//             }
//             catch
//             {
//                 return RedirectToAction("Index", "Settings");
//             }
//         }

//         //
//         // GET: /MailSettings/Delete/5

//         public ActionResult Delete(int id)
//         {
//             MailSettings mailsettings = db.mailsettings.Find(id);
//             if (mailsettings == null)
//             {
//                 return HttpNotFound();
//             }
//             return View(mailsettings);

//         }

//         //
//         // POST: /MailSettings/Delete/5

//         [HttpPost]
//         public ActionResult Delete(int id, FormCollection collection)
//         {
//             try
//             {
//                 MailSettings mailsettings = db.mailsettings.Find(id);
//                 db.mailsettings.Remove(mailsettings);
//                 db.SaveChanges();
//                 return RedirectToAction("Index");
//             }
//             catch
//             {
//                 return View(collection);
//             }
//         }
//     }
// }
