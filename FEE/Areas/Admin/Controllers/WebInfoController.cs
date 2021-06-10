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
    public class WebInfoController : Controller
    {
        private FEEDbContext _db = new FEEDbContext();
        [ClaimRequirementFilter(Command = CommandCode.VIEW, Function = FunctionCode.MORE_WEBINFO)]
        public ActionResult Index()
        {
            var result = _db.WebInfos.Select(x => new WebInfoViewModel()
            {
                Id = x.WebInfoId,
                Logo = x.Logo,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address
            }).ToList();
            return View(result);
        }

        [HttpGet]
        [ClaimRequirementFilter(Command = CommandCode.CREATE, Function = FunctionCode.MORE_WEBINFO)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(WebInfoViewModel viewmodel)
        {
            try
            {
                var model = new WebInfo();

                model.Logo = viewmodel.Logo;
                model.Email = viewmodel.Email;
                model.Phone = viewmodel.Phone;
                model.Address = viewmodel.Address;
                model.Facebook = viewmodel.Facebook;
                model.Youtube = viewmodel.Youtube;
                model.Zalo = viewmodel.Zalo;
                _db.WebInfos.Add(model);
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
        [ClaimRequirementFilter(Command = CommandCode.UPDATE, Function = FunctionCode.MORE_WEBINFO)]
        public ActionResult Update(int id)
        {
            var model = _db.WebInfos.Where(x => x.WebInfoId == id).FirstOrDefault();

            var viewModel = new WebInfoViewModel();
            viewModel.Logo = model.Logo;
            viewModel.Email = model.Email;
            viewModel.Phone = model.Phone;
            viewModel.Address = model.Address;
            viewModel.Facebook = model.Facebook;
            viewModel.Youtube = model.Youtube;
            viewModel.Zalo = model.Zalo;
            ViewBag.Img = viewModel.Logo;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Update(WebInfoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _db.WebInfos.Where(x => x.WebInfoId == viewModel.Id).FirstOrDefault();
                model.Logo = viewModel.Logo;
                model.Email = viewModel.Email;
                model.Phone = viewModel.Phone;
                model.Address = viewModel.Address;
                model.Facebook = viewModel.Facebook;
                model.Youtube = viewModel.Youtube;
                model.Zalo = viewModel.Zalo;
                _db.SaveChanges();
                Notification.set_flash("Cập nhật thành công!", "success");
                ViewBag.Img = viewModel.Logo;
                return View();
            }
            return View(viewModel);
        }

        [ClaimRequirementFilter(Command = CommandCode.DELETE, Function = FunctionCode.MORE_WEBINFO)]
        public JsonResult Delete(int id)
        {
            var model = _db.WebInfos.Where(x => x.WebInfoId == id).FirstOrDefault();
            _db.WebInfos.Remove(model);
            _db.SaveChanges();
            Notification.set_flash("Xóa thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}