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
    public class RoleController : Controller
    {
        private FEEDbContext _db = new FEEDbContext();
        [ClaimRequirementFilter(Command = CommandCode.VIEW, Function = FunctionCode.SYSTEM_ROLE)]
        public ActionResult Index()
        {
            var result = _db.Roles.Select(x => new RoleViewModel()
            {
                Id = x.RoleId,
                Name = x.Name
            }).ToList();
            return View(result);
        }
        [ClaimRequirementFilter(Command = CommandCode.CREATE, Function = FunctionCode.SYSTEM_ROLE)]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RoleViewModel viewmodel)
        {
            try
            {
                var model = new Role();

                model.Name = viewmodel.Name;
                _db.Roles.Add(model);
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
        [ClaimRequirementFilter(Command = CommandCode.UPDATE, Function = FunctionCode.SYSTEM_ROLE)]
        public ActionResult Update(int id)
        {
            var model = _db.Roles.Where(x => x.RoleId == id).SingleOrDefault();
            var viewModel = new RoleViewModel();
            viewModel.Name = model.Name;
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Update(RoleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _db.Roles.Where(x => x.RoleId == viewModel.Id).SingleOrDefault();
                model.Name = viewModel.Name;
                _db.SaveChanges();
                Notification.set_flash("Cập nhật thành công!", "success");
                return View();
            }
            return View(viewModel);
        }
        [ClaimRequirementFilter(Command = CommandCode.DELETE, Function = FunctionCode.SYSTEM_ROLE)]
        public JsonResult Delete(int id)
        {
            var users = _db.Users.Where(x => x.RoleId == id).ToList();
            if (users.Count() > 0)
            {
                Notification.set_flash("Không thể xóa!", "warning");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var model = _db.Roles.Where(x => x.RoleId == id).SingleOrDefault();
            _db.Roles.Remove(model);
            _db.SaveChanges();
            Notification.set_flash("Xóa thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}