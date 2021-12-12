using FEE.Authorize;
using FEE.Dtos;
using FEE.Library;
using FEE.Models;
using FEE.ViewModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTEHY.Model.Constants;

namespace FEE.Areas.Admin.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private FEEDbContext _db = new FEEDbContext();
        public List<MenuViewModel> listMenus = new List<MenuViewModel>();
        public PostController()
        {

        }
        [ClaimRequirementFilter(Command = CommandCode.VIEW, Function = FunctionCode.CONTENT_POST)]
        public ActionResult IndexAll(FilterViewModel viewModel)
        {
            listMenus = new List<MenuViewModel>();
            var query = from x in _db.Posts
                        join u in _db.Users
                        on x.CreateBy equals u.Id
                        where x.Deleted == false
                        select new PostViewModel()
                        {
                            PostId = x.PostId,
                            CategoryId = x.CategoryId.ToString(),
                            Name = x.Name,
                            Img = x.Img,
                            HomeFlag = x.HomeFlag,
                            HotFlag = x.HotFlag,
                            IsShow = x.IsShow,
                            Status = x.Status,
                            CreateDate = x.CreateDate,
                            DepartmentId = x.DepartmentId,
                            MenuId = x.MenuId,
                            Author = u.Name
                        };
            var result = query.OrderByDescending(x => x.PostId).ToList();

            if (viewModel.MenuId != 0)
            {
                result = result.Where(x => x.MenuId == viewModel.MenuId).ToList();
            }
            if (viewModel.CategoryId != 0)
            {
                result = result.Where(x => x.CategoryId == viewModel.CategoryId.ToString()).ToList();
            }
            ViewBag.TrashCount = _db.Posts.Where(x => x.Deleted == true).Count();
            ViewData["PostAlls"] = result;
            ViewBag.ListMenus = Helper.ListMenus(this.Dropdown(0));
            ViewBag.ListCategories = new SelectList(_db.Categories.ToList(), "CategoryId", "Name", 0);
            return View();
        }
        [ClaimRequirementFilter(Command = CommandCode.VIEW, Function = FunctionCode.CONTENT_POST)]
        public ActionResult Index(FilterViewModel viewModel)
        {
            listMenus = new List<MenuViewModel>();

            var user = (UserSession)Session["USER"];

            var models = from x in _db.Posts
                         join u in _db.Users
                         on x.CreateBy equals u.Id
                         where x.Deleted == false && u.Id == user.Id
                         select new PostViewModel()
                         {
                             PostId = x.PostId,
                             CategoryId = x.CategoryId.ToString(),
                             Name = x.Name,
                             Img = x.Img,
                             HomeFlag = x.HomeFlag,
                             HotFlag = x.HotFlag,
                             IsShow = x.IsShow,
                             Status = x.Status,
                             CreateDate = x.CreateDate,
                             DepartmentId = x.DepartmentId,
                             MenuId = x.MenuId,
                             Author = u.Name
                         };
            var result = models.ToList().OrderByDescending(x => x.PostId).ToList();

            if (viewModel.MenuId != 0)
            {
                result = result.Where(x => x.MenuId == viewModel.MenuId).ToList();
            }
            if(viewModel.CategoryId != 0)
            {
                result = result.Where(x => x.CategoryId == viewModel.CategoryId.ToString()).ToList();
            }
            if (user.RoleId == 1)
            {
                ViewBag.TrashCount = _db.Posts.Where(x => x.Deleted == true).Count();
                ViewData["Posts"] = result;
                ViewBag.ListMenus = Helper.ListMenus(this.Dropdown(0));
                ViewBag.ListCategories = new SelectList(_db.Categories.ToList(), "CategoryId", "Name", 0);
                return View();
            }
            else
            {
                result = result.Where(x => x.DepartmentId == user.DepartmentId).ToList();
                ViewData["Posts"] = result;
                ViewBag.ListMenus = Helper.ListMenus(this.Dropdown(0));
                ViewBag.ListCategories = new SelectList(_db.Categories.ToList(), "CategoryId", "Name", 0);
                return View();
            }
        }
        public List<MenuViewModel> Dropdown(int? parentId, string text = "")
        {
            var user = (UserSession)Session["USER"];
            var list = new List<MenuViewModel>();
            
            if (user.DepartmentId == 0 && user.RoleId == 1)
            {
                list = _db.Menus.Select(x => new MenuViewModel()
                {
                    Id = x.MenuId,
                    Attr = false,
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
                        this.Dropdown(item.Id, text + "---");
                    }
                }
                return listMenus;
            }
            else
            {
                list = _db.Menus.Select(x => new MenuViewModel()
                {
                    Id = x.MenuId,
                    Name = x.Name,
                    ParentId = x.ParentId,
                    Attr = true,
                    DisplayOrder = x.DisplayOrder
                }).ToList().OrderBy(y => y.DisplayOrder).ToList();
                var query = from m in _db.Menus
                            join dm in _db.DepartmentInMenus
                            on m.MenuId equals dm.MenuId
                            join d in _db.Departments
                            on dm.DepartmentId equals d.DepartmentId
                            where d.DepartmentId == user.DepartmentId
                            select new MenuViewModel
                            {
                                Id = m.MenuId,
                                Name = m.Name,
                                ParentId = m.ParentId,
                                DepartmentId = d.DepartmentId
                            };
                
                foreach (var item in list)
                {
                    if (item.ParentId == parentId)
                    {
                        item.Name = text + item.Name;
                        foreach(var m in query.ToList())
                        {
                            if(item.Id == m.Id)
                            {
                                item.Attr = false;
                            }
                        }
                        listMenus.Add(item);
                        this.Dropdown(item.Id, text + "---");

                    }
                }
                return listMenus;
            }
            
        }
        [HttpGet]
        [ClaimRequirementFilter(Command = CommandCode.CREATE,Function = FunctionCode.CONTENT_POST)]
        public ActionResult Create()
        {
            listMenus = new List<MenuViewModel>();
            var model = new PostViewModel()
            {
                ListCategories = Helper.ListCategories().ToList(),
                Tags = _db.Tags.Select(x => new TagViewModel()
                {
                    TagId = x.Id,
                    Name = x.Name
                }).ToList(),
                ListMenus = Helper.ListMenus(this.Dropdown(0)).ToList(),
            };
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(PostViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = (UserSession)Session["USER"];
                    var model = new Post();
                    model.Name = viewModel.Name;
                    model.CategoryId = Convert.ToInt32(viewModel.CategoryId);
                    model.Status = viewModel.Status;
                    model.Tag = String.Join(",", viewModel.TagIds);
                    model.MenuId = viewModel.MenuId;
                    model.Description = viewModel.Description;
                    model.Content = viewModel.Content;
                    model.Deleted = false;
                    model.CreateBy = user.Id;
                    if(viewModel.CreateDate == null)
                    {
                        model.CreateDate = DateTime.Now;
                    }
                    else
                    {
                        model.CreateDate = viewModel.CreateDate;
                    }
                    model.DepartmentId = user.DepartmentId;
                    model.Alias = XString.ToAscii(model.Name);
                    model.Img = viewModel.Img;
                    _db.Posts.Add(model);
                    _db.SaveChanges();
                    Notification.set_flash("Lưu thành công!", "success");
                    ModelState.Clear();                  
                }
                else
                {
                    Notification.set_flash("Nhập đầy đủ thông tin!", "warning");
                    return RedirectToAction("Create");
                }
                viewModel = new PostViewModel();
                viewModel.ListCategories = Helper.ListCategories().ToList();
                viewModel.Tags = _db.Tags.Select(x => new TagViewModel()
                {
                    TagId = x.Id,
                    Name = x.Name
                }).ToList();
                listMenus = new List<MenuViewModel>();
                viewModel.ListMenus = Helper.ListMenus(this.Dropdown(0)).ToList();
                return RedirectToAction("Index");
            }
            catch
            {
                Notification.set_flash("Thêm thất bại!", "warning");
                throw;
            }
        }
        [HttpGet]
        [ClaimRequirementFilter(Command = CommandCode.UPDATE, Function = FunctionCode.CONTENT_POST)]
        public ActionResult Update(int id)
        {
            listMenus = new List<MenuViewModel>();
            var model = _db.Posts.Where(x => x.PostId == id).Select(x => new PostViewModel()
            {
                PostId = x.PostId,
                MenuId = x.MenuId,
                Name = x.Name,
                Description = x.Description,
                CategoryId = x.CategoryId.ToString(),
                Status = x.Status,
                Img = x.Img,
                CreateDate = x.CreateDate,
                Content = x.Content,
                TagId = x.Tag
            }).SingleOrDefault();
            model.Date = model.CreateDate.ToString("yyyy-MM-dd");
            ViewBag.Img2 = model.Img;
            model.ListCategories = Helper.ListCategories().ToList();
            model.ListMenus = Helper.ListMenus(this.Dropdown(0)).ToList();
            model.Tags = _db.Tags.Select(x => new TagViewModel()
            {
                TagId = x.Id,
                Name = x.Name
            }).ToList();
            model.TagIds = model.TagId.Split(',').ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(PostViewModel viewModel)
        {
            try
            {
                int id = viewModel.PostId;
                if (ModelState.IsValid)
                {
                    var model = _db.Posts.Find(viewModel.PostId);
                    var user = (UserSession)Session["USER"];
                    model.Name = viewModel.Name;
                    model.CategoryId = Convert.ToInt32(viewModel.CategoryId);
                    model.Status = viewModel.Status;
                    model.MenuId = viewModel.MenuId;
                    model.Description = viewModel.Description;
                    model.Content = viewModel.Content;
                    model.Tag = String.Join(",",viewModel.TagIds);
                    model.UpdateDate = DateTime.Now;
                    model.CreateDate = Convert.ToDateTime(viewModel.Date);
                    model.UpdateBy = user.Id;
                    model.Alias = XString.ToAscii(model.Name);
                    model.Img = viewModel.Img;
                    _db.SaveChanges();
                    Notification.set_flash("Lưu thành công!", "success");
                }
                else
                {
                    Notification.set_flash("Nhập đầy đủ thông tin!", "warning");
                }
                ViewBag.Img2 = viewModel.Img;
                listMenus = new List<MenuViewModel>();
                viewModel = new PostViewModel();
                viewModel.ListCategories = Helper.ListCategories().ToList();
                viewModel.Tags = _db.Tags.Select(x => new TagViewModel()
                {
                    TagId = x.Id,
                    Name = x.Name
                }).ToList();
                viewModel.TagIds = _db.Posts.Find(id).Tag.Split(',').ToList();
                viewModel.ListMenus = Helper.ListMenus(this.Dropdown(0)).ToList();
                return View(viewModel);
            }
            catch
            {
                Notification.set_flash("Thêm thất bại!", "warning");
                throw;
            }
        }
        [ClaimRequirementFilter(Command = CommandCode.DELETE, Function = FunctionCode.CONTENT_POST)]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var model = _db.Posts.Where(x => x.PostId == id).FirstOrDefault();
            _db.Posts.Remove(model);
            _db.SaveChanges();
            Notification.set_flash("Xóa vĩnh viễn!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Trash(int id, bool status)
        {
            var model = _db.Posts.Where(x => x.PostId == id).FirstOrDefault();
            model.Deleted = status;
            model.UpdateDate = DateTime.Now;
            model.UpdateBy = 1;
            _db.SaveChanges();
            Notification.set_flash("Xóa thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReTrash(int id, bool status)
        {
            var model = _db.Posts.Where(x => x.PostId == id).FirstOrDefault();
            model.Deleted = status;
            model.UpdateDate = DateTime.Now;
            model.UpdateBy = 1;
            _db.SaveChanges();
            Notification.set_flash("Thay đổi thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [ClaimRequirementFilter(Command = CommandCode.TRASH, Function = FunctionCode.CONTENT_POST)]
        public ActionResult TrashIndex()
        {
            var query = from x in _db.Posts
                        join u in _db.Users
                        on x.CreateBy equals u.Id
                        where x.Deleted == true
                        select new PostViewModel()
                        {
                            PostId = x.PostId,
                            CategoryId = x.CategoryId.ToString(),
                            Name = x.Name,
                            Img = x.Img,
                            HomeFlag = x.HomeFlag,
                            HotFlag = x.HotFlag,
                            IsShow = x.IsShow,
                            Status = x.Status,
                            CreateDate = x.CreateDate,
                            UpdateDate = x.UpdateDate,
                            DepartmentId = x.DepartmentId,
                            MenuId = x.MenuId,
                            Author = u.Name
                        };
            var result = query.OrderByDescending(x => x.UpdateDate).ToList();
            return View(result);
        }
        public JsonResult ChangeHome(int id, bool status)
        {
            var model = _db.Posts.Where(x => x.PostId == id).FirstOrDefault();
            if(model.Status != Enums.PostStatus.Actived)
            {
                Notification.set_flash("Bài đăng chưa được duyệt!", "warning");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            model.HomeFlag = status;
            _db.SaveChanges();
            Notification.set_flash("Cập nhật thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeShow(int id, bool status)
        {
            var model = _db.Posts.Where(x => x.PostId == id).FirstOrDefault();
            if (model.Status != Enums.PostStatus.Actived)
            {
                Notification.set_flash("Bài đăng chưa được duyệt!", "warning");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            model.IsShow = status;
            _db.SaveChanges();
            Notification.set_flash("Cập nhật thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeHot(int id, bool status)
        {
            foreach(var item in _db.Posts)
            {
                item.HotFlag = false;               
            }
            var model = _db.Posts.Where(x => x.PostId == id).FirstOrDefault();
            if (model.Status != Enums.PostStatus.Actived)
            {
                Notification.set_flash("Bài đăng chưa được duyệt!", "warning");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            model.HotFlag = status;
            _db.SaveChanges();
            Notification.set_flash("Cập nhật thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Item(int id)
        {
            var query = from x in _db.Posts
                        join u in _db.Users
                        on x.CreateBy equals u.Id
                        where x.PostId == id
                        select new PostViewModel()
                        {
                            Name = x.Name,
                            PostId = x.PostId,
                            Img = x.Img,
                            Description = x.Description,
                            Content = x.Content,
                            CreateDate = x.CreateDate,
                            CategoryId = x.CategoryId.ToString(),
                            Author = u.Name,
                            Alias = x.Alias
                        };
            var model = query.FirstOrDefault();
            return View(model);
        }
    }
}