using FEE.Library;
using FEE.Models;
using FEE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEE.Areas.Admin.Controllers
{
    public class TagController : Controller
    {
        // GET: Admin/Tag
        private FEEDbContext _db = new FEEDbContext();

        public ActionResult Index()
        {
            var result = _db.Tags.Select(x => new TagViewModel()
            {
                TagId = x.Id,
                Name = x.Name
            }).OrderByDescending(x=>x.TagId).ToList();
            return View(result);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TagViewModel viewmodel)
        {
            try
            {
                var model = new Tag();

                model.Name = viewmodel.Name;
                _db.Tags.Add(model);
                _db.SaveChanges();
                Notification.set_flash("Lưu thành công!", "success");
                ModelState.Clear();
                return View();
            }
            catch (Exception ex)
            {
                Notification.set_flash("Thêm thất bại!", "warning");
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var model = _db.Tags.Where(x => x.Id == id).FirstOrDefault();
            var viewModel = new TagViewModel();
            viewModel.Name = model.Name;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Update(TagViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _db.Tags.Where(x => x.Id == viewModel.TagId).FirstOrDefault();
                model.Name = viewModel.Name;
                _db.SaveChanges();
                Notification.set_flash("Cập nhật thành công!", "success");
                return View();
            }
            return View(viewModel);
        }

        public JsonResult Delete(int id)
        {
            var users = _db.Posts.Where(x => x.Tag.Contains(id.ToString())).ToList();
            if (users.Count() > 0)
            {
                Notification.set_flash("Không được phép xóa!", "warning");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var model = _db.Tags.Where(x => x.Id == id).FirstOrDefault();
            _db.Tags.Remove(model);
            _db.SaveChanges();
            Notification.set_flash("Xóa thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}