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
    public class ImageController : Controller
    {
        private FEEDbContext _db = new FEEDbContext();

        [ClaimRequirementFilter(Command = CommandCode.VIEW, Function = FunctionCode.MORE_IMAGE)]
        public ActionResult Index()
        {
            var result = _db.Images.Select(x => new ImageViewModel()
            {
                ImageId = x.ImgId,
                Img = x.Img,
                CreateDate = x.CreateDate,
                Status = x.Status
            }).OrderByDescending(x => x.ImageId).ToList();
            return View(result);
        }
        [ClaimRequirementFilter(Command = CommandCode.CREATE, Function = FunctionCode.MORE_IMAGE)]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ImageViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = new Image();
                    model.Img = viewmodel.Img;
                    model.Status = viewmodel.Status;
                    model.CreateDate = DateTime.Now;
                    model.Deleted = false;
                    model.CreateBy = 1;
                    _db.Images.Add(model);
                    _db.SaveChanges();
                    Notification.set_flash("Lưu thành công!", "success");
                    ModelState.Clear();
                }
                else
                {
                    Notification.set_flash("Nhập đầy đủ thông tin!", "warning");
                }
                return View();
            }
            catch (Exception ex)
            {
                Notification.set_flash("Thêm thất bại!", "warning");
                throw ex;
            }
        }



        [HttpGet]
        [ClaimRequirementFilter(Command = CommandCode.UPDATE, Function = FunctionCode.MORE_IMAGE)]
        public ActionResult Update(int id)
        {
            var model = _db.Images.Where(x => x.ImgId == id).FirstOrDefault();
            var viewModel = new ImageViewModel();
            viewModel.ImageId = model.ImgId;
            viewModel.Img = model.Img;
            viewModel.Status = model.Status;
            ViewBag.Img = model.Img;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Update(ImageViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _db.Images.Where(x => x.ImgId == viewModel.ImageId).FirstOrDefault();
                model.Img = viewModel.Img;
                model.Status = viewModel.Status;
                model.UpdateDate = DateTime.Now;
                model.UpdateBy = 1;
                _db.SaveChanges();
                Notification.set_flash("Cập nhật thành công!", "success");
                ViewBag.Img = model.Img;
                return View();
            }
            return View(viewModel);
        }
        [ClaimRequirementFilter(Command = CommandCode.DELETE, Function = FunctionCode.MORE_IMAGE)]
        public JsonResult Delete(int id)
        {
            var model = _db.Images.Where(x => x.ImgId == id).FirstOrDefault();
            _db.Images.Remove(model);
            _db.SaveChanges();
            Notification.set_flash("Xóa thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeStatus(int id, bool status)
        {
            var model = _db.Images.Where(x => x.ImgId == id).FirstOrDefault();
            model.Status = status;
            _db.SaveChanges();
            Notification.set_flash("Cập nhật thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}