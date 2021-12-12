using FEE.Authorize;
using FEE.Library;
using FEE.Models;
using FEE.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTEHY.Model.Constants;

namespace FEE.Areas.Admin.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private FEEDbContext _db = new FEEDbContext();
        public List<MenuViewModel> listMenus = new List<MenuViewModel>();
        public MenuController()
        {

        }
        [ClaimRequirementFilter(Command = CommandCode.VIEW, Function = FunctionCode.MORE_MENU)]
        public ActionResult Index()
        {
            var result = _db.Menus.Select(x => new MenuViewModel()
            {
                Id = x.MenuId,
                Name = x.Name,
                ParentId = x.ParentId,
                DisplayOrder = x.DisplayOrder,
                URL = x.URL,
                Status = x.Status,
                CreatedDate = x.CreateDate

            }).OrderBy(x=>x.DisplayOrder).ToList();

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
                    item.SubItem.OrderBy(x=>x.DisplayOrder).ToList();
                }
            }
            var listMenu = listResult.ToList().OrderBy(x => x.DisplayOrder).ToList();
            return View(listMenu);
        }

        [HttpGet]
        [ClaimRequirementFilter(Command = CommandCode.CREATE, Function = FunctionCode.MORE_MENU)]
        public ActionResult Create()
        {
            listMenus = new List<MenuViewModel>();
            MenuViewModel model = new MenuViewModel()
            {
                ListMenus = new SelectList(this.Dropdown(0), "Id", "Name", 0)
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MenuViewModel viewmodel)
        {
            try
            {
                var model = new Menu();
                model.Name = viewmodel.Name;
                model.ParentId = viewmodel.ParentId;
                model.Link = viewmodel.Link;
                model.Status = viewmodel.Status;
                model.CreateDate = DateTime.Now;
                model.DisplayOrder = viewmodel.DisplayOrder;
                _db.Menus.Add(model);
                _db.SaveChanges();
                Notification.set_flash("Lưu thành công!", "success");
                ModelState.Clear();
                viewmodel = new MenuViewModel();
                viewmodel.ListMenus = new SelectList(this.Dropdown(0), "Id", "Name", 0);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Notification.set_flash("Thêm thất bại!", "warning");
                throw ex;
            }
        }

        public List<MenuViewModel> Dropdown(int? parentId, string text = "")
        {
            var list = _db.Menus.Select(x => new MenuViewModel()
            {
                Id = x.MenuId,
                Name = x.Name,
                ParentId = x.ParentId,
                DisplayOrder = x.DisplayOrder
            }).ToList().OrderBy(y => y.DisplayOrder).ToList();

            foreach (var item in list)
            {
                if (item.ParentId == parentId)
                {
                    item.Name = text + item.Name;
                    listMenus.Add(item);
                    listMenus = listMenus.Distinct().ToList();
                    this.Dropdown(item.Id, text + "---");
                }
            }
            return listMenus;
        }

        [HttpGet]
        [ClaimRequirementFilter(Command = CommandCode.UPDATE, Function = FunctionCode.MORE_MENU)]
        public ActionResult Update(int id)
        {
            listMenus = new List<MenuViewModel>();
            var model = _db.Menus.Where(x => x.MenuId == id).FirstOrDefault();
            var viewModel = new MenuViewModel();
            viewModel.Id = model.MenuId;
            viewModel.Link = model.Link;
            viewModel.Name = model.Name;
            viewModel.ParentId = model.ParentId;
            viewModel.Status = model.Status;
            viewModel.DisplayOrder = model.DisplayOrder;
            viewModel.ListMenus = new SelectList(this.Dropdown(0), "Id", "Name", 0);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Update(MenuViewModel viewModel)
        {
            try
            {
                var model = _db.Menus.Where(x => x.MenuId == viewModel.Id).FirstOrDefault();
                model.Name = viewModel.Name;
                model.ParentId = viewModel.ParentId;
                model.Status = viewModel.Status;
                model.Link = viewModel.Link;
                model.UpdateDate = DateTime.Now;
                model.DisplayOrder = viewModel.DisplayOrder;
                _db.SaveChanges();
                Notification.set_flash("Cập nhật thành công!", "success");
                viewModel = new MenuViewModel();
                viewModel.ListMenus = new SelectList(this.Dropdown(0), "Id", "Name", 0);
                return RedirectToAction("Index", "Menu");
            }
            catch(Exception ex)
            {
                return View(viewModel);
            }           
        }
        [ClaimRequirementFilter(Command = CommandCode.DELETE, Function = FunctionCode.MORE_MENU)]
        public JsonResult Delete(int id)
        {
            var posts = _db.Posts.Where(x => x.MenuId == id).ToList();
            if(posts.Count() > 0)
            {
                Notification.set_flash("Không thể xóa!", "warning");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            var model = _db.Menus.Where(x => x.MenuId == id).FirstOrDefault();
            _db.Menus.Remove(model);
            _db.SaveChanges();
            Notification.set_flash("Xóa thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(int id, bool status)
        {
            var model = _db.Menus.Where(x => x.MenuId == id).FirstOrDefault();
            model.Status = status;
            _db.SaveChanges();
            Notification.set_flash("Cập nhật thành công!", "success");
            return Json(true,JsonRequestBehavior.AllowGet);
        }
    }
}