using FEE.Authorize;
using FEE.Dtos;
using FEE.Library;
using FEE.Models;
using FEE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTEHY.Model.Constants;

namespace FEE.Areas.Admin.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly FEEDbContext _db = new FEEDbContext();

        [ClaimRequirementFilter(Command = CommandCode.REPLY, Function = FunctionCode.CONTENT_CONTACT)]
        public ActionResult List()
        {
            var result = _db.Contacts.Select(x => new ContactViewModel()
            {
                ContactId = x.ContactId,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Status = x.Status,
                CreateDate = x.CreateDate,
                Content = x.Content
            }).OrderByDescending(x => x.ContactId).ToList();
            return View(result);
        }

        [ClaimRequirementFilter(Command = CommandCode.REPLY, Function = FunctionCode.CONTENT_CONTACT)]
        [HttpGet]
        public ActionResult Reply(int id)
        {
            var model = _db.Contacts.Where(x => x.ContactId == id).Select(x => new ContactViewModel()
            {
                ContactId = x.ContactId,
                Content = x.Content,
                Email = x.Email,
                Name = x.Name,
                Phone = x.Phone,
                CreateDate = x.CreateDate
            }).FirstOrDefault();
            return View(model);
        }
        [ClaimRequirementFilter(Command = CommandCode.DELETE, Function = FunctionCode.CONTENT_CONTACT)]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var model = _db.Contacts.Where(x => x.ContactId == id).FirstOrDefault();
            _db.Contacts.Remove(model);
            _db.SaveChanges();
            Notification.set_flash("Xóa vĩnh viễn!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Reply(ContactViewModel model)
        {
            var user = (UserSession)Session["USER"];
            var contact = _db.Contacts.Find(model.ContactId);
            if (ModelState.IsValid)
            {
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Areas/Admin/Views/Mail/G_Mail.html"));
                content = content.Replace("{{FullName}}", contact.Name);
                content = content.Replace("{{AdminName}}", user.Name);
                content = content.Replace("{{RQ}}", model.Message);
                string subject = "Phản hồi liên hệ từ Khoa Điện - Điện tử, trường Đại học Sư phạm Kỹ thuật Hưng Yên";
                new MailHelper().SendMail(contact.Email, subject, content);

                contact.Status = true;
                contact.ReplyDate = DateTime.Now;
                _db.SaveChanges();
                Notification.set_flash("Đã trả lời liên hệ!", "success");
                return RedirectToAction("List");
            }
            Notification.set_flash("Trả lời thất bại!", "warning");
            return View("Reply");
        }
    }
}