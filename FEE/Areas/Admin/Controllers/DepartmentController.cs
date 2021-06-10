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
    public class DepartmentController : Controller
    {
        // GET: Admin/Department
        private FEEDbContext _db = new FEEDbContext();
        [ClaimRequirementFilter(Command = CommandCode.VIEW, Function = FunctionCode.SYSTEM_DEPARTMENT)]
        public ActionResult Index()
        {
            var result = _db.Departments.Select(x => new DepartmentViewModel()
            {
                Id = x.DepartmentId,
                Name = x.Name
            }).OrderByDescending(x => x.Id).ToList();
            return View(result);
        }
        [ClaimRequirementFilter(Command = CommandCode.CREATE, Function = FunctionCode.SYSTEM_DEPARTMENT)]
        [HttpGet]
        public ActionResult Create()
        {
            var result = _db.Menus.Select(x => new MenuViewModel()
            {
                Id = x.MenuId,
                Name = x.Name,
                ParentId = x.ParentId,
                DisplayOrder = x.DisplayOrder,
                URL = x.URL

            }).ToList();

            List<MenuViewModel> listResult = new List<MenuViewModel>();

            foreach (var item in result)
            {
                if (item.ParentId == 0)
                {
                    foreach (var menu in result)
                    {
                        if (menu.ParentId == item.Id)
                        {
                            item.SubItem.Add(menu);
                        }

                    }
                    listResult.Add(item);
                }
                else
                {
                    foreach (var sub in result)
                    {
                        if (sub.ParentId == item.Id)
                        {
                            item.SubItem.Add(sub);
                        }
                    }
                }
            }
            var listMenu = listResult.ToList().OrderBy(x => x.DisplayOrder).ToList();
            DepartmentViewModel model = new DepartmentViewModel();
            model.Menus = listMenu;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(DepartmentViewModel viewmodel)
        {
            try
            {
                var model = new Department();

                model.Name = viewmodel.Name;
                _db.Departments.Add(model);
                _db.SaveChanges();
                if(viewmodel.ListMenuIds.Count()> 0)
                {
                    foreach (var item in viewmodel.ListMenuIds)
                    {
                        DepartmentInMenu entity = new DepartmentInMenu();
                        entity.DepartmentId = model.DepartmentId;
                        entity.MenuId = item;
                        _db.DepartmentInMenus.Add(entity);
                    }
                    _db.SaveChanges();
                }
                Notification.set_flash("Lưu thành công!", "success");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Notification.set_flash("Thêm thất bại!", "warning");
                throw ex;
            }
        }
        [ClaimRequirementFilter(Command = CommandCode.UPDATE, Function = FunctionCode.SYSTEM_DEPARTMENT)]
        [HttpGet]
        public ActionResult Update(int id)
        {
            var result = _db.Menus.Select(x => new MenuViewModel()
            {
                Id = x.MenuId,
                Name = x.Name,
                ParentId = x.ParentId,
                DisplayOrder = x.DisplayOrder,
                URL = x.URL

            }).ToList();

            List<MenuViewModel> listResult = new List<MenuViewModel>();

            foreach (var item in result)
            {
                if (item.ParentId == 0)
                {
                    foreach (var menu in result)
                    {
                        if (menu.ParentId == item.Id)
                        {
                            item.SubItem.Add(menu);
                        }

                    }
                    listResult.Add(item);
                }
                else
                {
                    foreach (var sub in result)
                    {
                        if (sub.ParentId == item.Id)
                        {
                            item.SubItem.Add(sub);
                        }
                    }
                }
            }
            var listMenu = listResult.ToList().OrderBy(x => x.DisplayOrder).ToList();
            var model = _db.Departments.Find(id);
            var viewModel = new DepartmentViewModel();
            viewModel.Name = model.Name;
            viewModel.Menus = listMenu;
            viewModel.Id = model.DepartmentId;
            return View(viewModel);
        }
        [HttpGet]
        public ActionResult GetUpdate(int id)
        {
            var model = from d in _db.Departments
                        join _d in _db.DepartmentInMenus
                        on d.DepartmentId equals _d.DepartmentId
                        where d.DepartmentId == id
                        select new
                        {
                            d.DepartmentId,
                            d.Name,
                            _d.MenuId
                        };
            return Json(model,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Update(DepartmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = _db.Departments.Where(x => x.DepartmentId == viewModel.Id).FirstOrDefault();
                model.Name = viewModel.Name;
                var list = _db.DepartmentInMenus.Where(x => x.DepartmentId == viewModel.Id).ToList();
                foreach(var entity in list)
                {
                    _db.DepartmentInMenus.Remove(entity);
                }
                foreach (var item in viewModel.ListMenuIds)
                {
                    DepartmentInMenu entity = new DepartmentInMenu();
                    entity.DepartmentId = model.DepartmentId;
                    entity.MenuId = item;
                    _db.DepartmentInMenus.Add(entity);
                }
                _db.SaveChanges();
                Notification.set_flash("Lưu thành công!", "success");
                ModelState.Clear();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [ClaimRequirementFilter(Command = CommandCode.DELETE, Function = FunctionCode.SYSTEM_DEPARTMENT)]
        public JsonResult Delete(int id)
        {
            var users = _db.Users.Where(x => x.DepartmentId == id).ToList();
            var query = from m in _db.Menus
                        join dm in _db.DepartmentInMenus
                        on m.MenuId equals dm.MenuId
                        join d in _db.Departments
                        on dm.DepartmentId equals d.DepartmentId
                        where d.DepartmentId == id
                        select new DepartmentViewModel()
                        {
                            Id = d.DepartmentId,
                            Name = d.Name
                        };
            var list = query.ToList();
            if(users.Count() > 0 || list.Count() > 0)
            {
                Notification.set_flash("Đã có tài khoản thuôc bộ môn hoặc bộ môn này đã được phân công quản lý danh mục!", "warning");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var model = _db.Departments.Where(x => x.DepartmentId == id).FirstOrDefault();
            _db.Departments.Remove(model);
            _db.SaveChanges();
            Notification.set_flash("Xóa thành công!", "success");
            return Json(true,JsonRequestBehavior.AllowGet);
        }
    }
}