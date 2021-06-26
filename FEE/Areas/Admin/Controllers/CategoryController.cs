using FEE.Authorize;
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
    public class CategoryController : Controller
    {
        private FEEDbContext _db = new FEEDbContext();

        [ClaimRequirementFilter(Command = CommandCode.VIEW, Function = FunctionCode.CONTENT_CATEGORY)]
        public ActionResult Index()
        {
            var result = _db.Categories.Select(x => new CategoryViewModel()
            {
                CategoryId = x.CategoryId,
                Name = x.Name
            }).OrderByDescending(x => x.CategoryId).ToList();
            return View(result);
        }
        [ClaimRequirementFilter(Command = CommandCode.CREATE, Function = FunctionCode.CONTENT_CATEGORY)]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CategoryViewModel viewmodel)
        {
            try
            {
                var model = new Category();

                model.Name = viewmodel.Name;
                model.CreateDate = DateTime.Now;
                _db.Categories.Add(model);
                _db.SaveChanges();
                Notification.set_flash("Lưu thành công!", "success");
                ModelState.Clear();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Notification.set_flash("Thêm thất bại!", "warning");
                throw ex;
            }
        }

        [HttpGet]
        [ClaimRequirementFilter(Command = CommandCode.UPDATE, Function = FunctionCode.CONTENT_CATEGORY)]
        public ActionResult Update(int id)
        {
            var model = _db.Categories.Where(x => x.CategoryId == id).SingleOrDefault();
            var viewModel = new CategoryViewModel();
            viewModel.Name = model.Name;
            viewModel.CategoryId = model.CategoryId;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Update(CategoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _db.Categories.Where(x => x.CategoryId == viewModel.CategoryId).FirstOrDefault();
                model.Name = viewModel.Name;
                model.UpdateDate = DateTime.Now;
                _db.SaveChanges();
                Notification.set_flash("Cập nhật thành công!", "success");
                return View();
            }
            return View(viewModel);
        }
        [ClaimRequirementFilter(Command = CommandCode.DELETE, Function = FunctionCode.CONTENT_CATEGORY)]
        public JsonResult Delete(int id)
        {
            var posts = _db.Posts.Where(x => x.CategoryId == id).ToList();
            if (posts.Count() > 0)
            {
                Notification.set_flash("Không được phép xóa!", "warning");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var model = _db.Categories.Where(x => x.CategoryId == id).FirstOrDefault();
            _db.Categories.Remove(model);
            _db.SaveChanges();
            Notification.set_flash("Xóa thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}