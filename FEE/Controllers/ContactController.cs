using CaptchaMvc.HtmlHelpers;
using FEE.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FEE.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        private FEEDbContext _db = new FEEDbContext();

        public ActionResult Index()
        {
            var result = _db.WebInfos.SingleOrDefault();
            return View(result);
        }

        [HttpPost]
        public ActionResult Send(Contact model)
        {
            if (!this.IsCaptchaValid(errorText:""))
            {
                ModelState.AddModelError("Thông báo","Captcha không chính xác!");
                return RedirectToAction("Index");
            }
            else
            {
                model.CreateDate = DateTime.Now;
                model.Status = false;
                _db.Contacts.Add(model);
                _db.SaveChanges();
                ModelState.AddModelError("Thông báo", "Gửi phản hồi thành công!");
                return RedirectToAction("Index");
            }
        }
    }
}