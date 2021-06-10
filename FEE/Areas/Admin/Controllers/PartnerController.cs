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
    public class PartnerController : Controller
    {
        private FEEDbContext _db = new FEEDbContext();

        [ClaimRequirementFilter(Command = CommandCode.VIEW, Function = FunctionCode.MORE_PARTNER)]
        public ActionResult Index()
        {
            var result = _db.Partners.Select(x => new PartnerViewModel()
            {
                PartnerId = x.PartId,
                Img = x.Img,
                CreateDate = x.CreateDate,
                Url = x.Url,
                Status = x.Status
            }).ToList().OrderBy(x => x.CreateDate).ToList();
            return View(result);
        }
        [ClaimRequirementFilter(Command = CommandCode.CREATE, Function = FunctionCode.MORE_PARTNER)]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PartnerViewModel viewmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = new Partner();
                    model.Img = viewmodel.Img;
                    model.Status = viewmodel.Status;
                    model.CreateDate = DateTime.Now;
                    model.Deleted = false;
                    model.Url = viewmodel.Url;
                    model.CreateBy = 1;
                    _db.Partners.Add(model);
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
        [ClaimRequirementFilter(Command = CommandCode.UPDATE, Function = FunctionCode.MORE_PARTNER)]
        public ActionResult Update(int id)
        {
            var model = _db.Partners.Where(x => x.PartId == id).SingleOrDefault();
            var viewModel = new PartnerViewModel();
            viewModel.PartnerId = model.PartId;
            viewModel.Img = model.Img;
            viewModel.Url = model.Url;
            viewModel.Status = model.Status;
            ViewBag.Img = model.Img;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Update(PartnerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _db.Partners.Where(x => x.PartId == viewModel.PartnerId).SingleOrDefault();
                model.Img = viewModel.Img;
                model.Status = viewModel.Status;
                model.UpdateDate = DateTime.Now;
                model.Url = viewModel.Url;
                model.UpdateBy = 1;
                _db.SaveChanges();
                Notification.set_flash("Cập nhật thành công!", "success");
                ViewBag.Img = model.Img;
                return View();
            }
            return View(viewModel);
        }
        [ClaimRequirementFilter(Command = CommandCode.DELETE, Function = FunctionCode.MORE_PARTNER)]
        public JsonResult Delete(int id)
        {
            var model = _db.Partners.Where(x => x.PartId == id).SingleOrDefault();
            _db.Partners.Remove(model);
            _db.SaveChanges();
            Notification.set_flash("Xóa thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeStatus(int id, bool status)
        {
            var model = _db.Partners.Where(x => x.PartId == id).SingleOrDefault();
            model.Status = status;
            _db.SaveChanges();
            Notification.set_flash("Cập nhật thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}