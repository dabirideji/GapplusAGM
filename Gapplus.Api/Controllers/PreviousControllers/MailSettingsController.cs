using BarcodeGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BarcodeGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MailSettingsController : ControllerBase
    {
        //
        private UsersContext db;
        private readonly IViewBagManager _viewBagManager;

        //
        // GET: /MailSettings/

        public MailSettingsController(UsersContext _db,IViewBagManager viewBagManager)
        {
            db=_db;
        _viewBagManager = viewBagManager;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var mailserver = new SelectList(new[] { "Mail Server 1", "Mail Server 2" });
            // ViewBag.mailserver = mailserver;
            _viewBagManager.SetValue("mailserver",mailserver);


            var mailsettings = db.mailsettings.ToList();
            return Ok(mailsettings);
        }

        //
        // GET: /MailSettings/Details/5
    [HttpGet("{id}")]
        public ActionResult Details(int id = 0)
        {
            var mailsettings = db.mailsettings.Find(id);
            return Ok(mailsettings);
        }

        //
            // GET: /MailSettings/Create
        [HttpGet]
        public ActionResult Create()
        {
            var mailserver = new SelectList(new[] { "Mail Server 1", "Mail Server 2" });
            // ViewBag.mailserver = mailserver;
            _viewBagManager.SetValue("mailserver",mailserver);
            MailSettings mailsettings = new MailSettings();

            return Ok(mailsettings);
        }

        //
        // POST: /MailSettings/Create

        [HttpPost]
        public ActionResult Create(MailSettings collection)
        {
            var mailserver = new SelectList(new[] { "Mail Server 1", "Mail Server 2" });
            // ViewBag.mailserver = mailserver;
            _viewBagManager.SetValue("mailserver",mailserver);
            try
            {
                if (ModelState.IsValid)
                {
                    var setting = db.mailsettings.ToArray();
                    if (setting.Length == 0 || setting.Length < 1)
                    {
                        db.mailsettings.Add(collection);
                        db.SaveChanges();
                       
                    }
                    return RedirectToAction("Index", "Settings");
                }
                // TODO: Add insert logic here

                return RedirectToAction("Index","Settings");
            }
            catch
            {
                return RedirectToAction("Index", "Settings");
            }
        }

        //
        // GET: /MailSettings/Edit/5
        [HttpPut("{id}")]
            public ActionResult Edit(int id = 0)
        {
            var mailserver = new SelectList(new[] { "Mail Server 1", "Mail Server 2" });
            _viewBagManager.SetValue("mailserver",mailserver);
            // ViewBag.mailserver = mailserver;
            var mailsettings = db.mailsettings.Find(id);
            if (mailsettings == null)
            {
                return NotFound();
            }
            return Ok(mailsettings);
        }

        //
        // POST: /MailSettings/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, MailSettings collection)
        {
            var mailserver = new SelectList(new[] { "Mail Server 1", "Mail Server 2" });
            _viewBagManager.SetValue("mailserver",mailserver);
            // ViewBag.mailserver = mailserver;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(collection).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index","Settings");
                }

                return RedirectToAction("Index", "Settings");
            }
            catch
            {
                return RedirectToAction("Index", "Settings");
            }
        }

        //
        // GET: /MailSettings/Delete/5
        [HttpDelete("{id}")]
            public ActionResult Delete(int id)
        {
            MailSettings mailsettings = db.mailsettings.Find(id);
            if (mailsettings == null)
            {
                return NotFound();
            }
            return Ok(mailsettings);

        }

        //
        // POST: /MailSettings/Delete/5

        [HttpPost("{id}")]
        public ActionResult Delete(int id, IFormFile collection)
        {
            try
            {
                MailSettings mailsettings = db.mailsettings.Find(id);
                db.mailsettings.Remove(mailsettings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Ok(collection);
            }
        }
    }
}
