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
    public class PostController : Controller
    {
        private FEEDbContext _db = new FEEDbContext();
        public List<MenuViewModel> listMenus = new List<MenuViewModel>();
        public PostController()
        {

        }
        [ClaimRequirementFilter(Command = CommandCode.VIEW, Function = FunctionCode.CONTENT_POST)]
        public ActionResult IndexAll()
        {           
            var result = _db.Posts.Where(x=>x.Deleted == false).Select(x => new PostViewModel()
            {
                PostId = x.PostId,
                Name = x.Name,
                Img = x.Img,
                HomeFlag = x.HomeFlag,
                HotFlag = x.HotFlag,
                IsShow = x.IsShow,
                Status = x.Status,
                CreateDate = x.CreateDate,
                Author = x.Author


            }).OrderBy(x => x.CreateDate).ToList();
            var user = (UserSession)Session["USER"];
            if (user.RoleId == 1)
            {
                ViewBag.TrashCount = _db.Posts.Where(x => x.Deleted == true).Count();
                return View(result);
            }
            else
            {
                result = result.Where(x => x.DepartmentId == user.DepartmentId).ToList();
                return View(result);
            }
        }
        [ClaimRequirementFilter(Command = CommandCode.VIEW, Function = FunctionCode.CONTENT_POST)]
        public ActionResult Index(string menuId = "")
        {
            var user = (UserSession)Session["USER"];

            var models = from x in _db.Posts
                         join u in _db.Users
                         on x.CreateBy equals u.Id
                         where x.Deleted == false && u.Id == user.Id
                         select new PostViewModel()
                         {
                             PostId = x.PostId,
                             Name = x.Name,
                             Img = x.Img,
                             HomeFlag = x.HomeFlag,
                             HotFlag = x.HotFlag,
                             IsShow = x.IsShow,
                             Status = x.Status,
                             CreateDate = x.CreateDate,
                             Author = x.Author,
                             DepartmentId = x.DepartmentId
                         };
            var result = models.ToList().OrderBy(x => x.CreateDate).ToList();

            if (!String.IsNullOrEmpty(menuId))
            {
                result = result.Where(x => x.MenuId == Convert.ToInt32(menuId)).ToList();
            }

            if (user.RoleId == 1)
            {
                ViewBag.TrashCount = _db.Posts.Where(x => x.Deleted == true).Count();
                return View(result);
            }
            else
            {
                result = result.Where(x => x.DepartmentId == user.DepartmentId).ToList();
                return View(result);
            }
        }
        public List<MenuViewModel> Dropdown(int? parentId, string text = "")
        {
            var user = (UserSession)Session["USER"];

            var query = from m in _db.Menus
                       join dm in _db.DepartmentInMenus
                       on m.MenuId equals dm.MenuId
                       join d in _db.Departments
                       on dm.DepartmentId equals d.DepartmentId
                       select new MenuViewModel
                       {
                           Id = m.MenuId,
                           Name = m.Name,
                           ParentId = m.ParentId,
                           DepartmentId = d.DepartmentId
                       };

            var list = new List<MenuViewModel>();
            if(user.DepartmentId == 0 && user.RoleId == 1)
            {
                list = query.ToList();
            }
            else if(user.DepartmentId != 0 && user.RoleId != 1)
            {
                list = query.Where(x => x.DepartmentId == user.DepartmentId).ToList();
            }
            foreach (var item in list)
            {
                if (item.ParentId == parentId)
                {
                    item.Name = text + item.Name;
                    listMenus.Add(item);
                    listMenus.Distinct();
                    this.Dropdown(item.Id, text + "---");
                }
            }
            return listMenus;
        }
        [HttpGet]
        [ClaimRequirementFilter(Command = CommandCode.CREATE,Function = FunctionCode.CONTENT_POST)]
        public ActionResult Create()
        {
            var model = new PostViewModel()
            {
                ListCategories = Helper.ListCategories().ToList(),
                Tags = _db.Tags.Select(x => new TagViewModel()
                {
                    TagId = x.Id,
                    Name = x.Name
                }).ToList(),
                ListMenus = new SelectList(this.Dropdown(0), "Id", "Name", 0)
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
                    model.Author = user.Name;
                    model.CreateDate = viewModel.CreateDate;
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
                }
                viewModel = new PostViewModel();
                viewModel.ListCategories = Helper.ListCategories().ToList();
                viewModel.Tags = _db.Tags.Select(x => new TagViewModel()
                {
                    TagId = x.Id,
                    Name = x.Name
                }).ToList();
                viewModel.ListMenus = new SelectList(this.Dropdown(0), "Id", "Name", 0);
                return View(viewModel);
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
            model.ListMenus = new SelectList(this.Dropdown(0), "Id", "Name", 0);
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
                    model.Name = viewModel.Name;
                    model.CategoryId = Convert.ToInt32(viewModel.CategoryId);
                    model.Status = viewModel.Status;
                    model.MenuId = viewModel.MenuId;
                    model.Description = viewModel.Description;
                    model.Content = viewModel.Content;
                    model.Deleted = false;
                    model.CreateBy = 1;
                    model.Tag = String.Join(",",viewModel.TagIds);
                    model.UpdateDate = DateTime.Now;
                    model.CreateDate = Convert.ToDateTime(viewModel.Date);
                    model.UpdateBy = 1;
                    model.DepartmentId = 0;
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
                viewModel = new PostViewModel();
                viewModel.ListCategories = Helper.ListCategories().ToList();
                viewModel.Tags = _db.Tags.Select(x => new TagViewModel()
                {
                    TagId = x.Id,
                    Name = x.Name
                }).ToList();
                viewModel.TagIds = _db.Posts.Find(id).Tag.Split(',').ToList();
                viewModel.ListMenus = new SelectList(this.Dropdown(0), "Id", "Name", 0);
                return View(viewModel);
            }
            catch
            {
                Notification.set_flash("Thêm thất bại!", "warning");
                throw;
            }
        }
        [ClaimRequirementFilter(Command = CommandCode.DELETE, Function = FunctionCode.CONTENT_POST)]
        public JsonResult Delete(int id)
        {
            var model = _db.Posts.Where(x => x.PostId == id).SingleOrDefault();
            _db.Posts.Remove(model);
            _db.SaveChanges();
            Notification.set_flash("Xóa vĩnh viễn!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Trash(int id, bool status)
        {
            var model = _db.Posts.Where(x => x.PostId == id).SingleOrDefault();
            model.Deleted = status;
            model.UpdateDate = DateTime.Now;
            model.UpdateBy = 1;
            _db.SaveChanges();
            Notification.set_flash("Xóa thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReTrash(int id, bool status)
        {
            var model = _db.Posts.Where(x => x.PostId == id).SingleOrDefault();
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
            var result = _db.Posts.Where(x => x.Deleted == true).Select(x => new PostViewModel()
            {
                PostId = x.PostId,
                Name = x.Name,
                Img = x.Img,
                HomeFlag = x.HomeFlag,
                HotFlag = x.HotFlag,
                IsShow = x.IsShow,
                Status = x.Status,
                UpdateDate = x.UpdateDate,
                Author = x.Author,
                CreateDate = x.CreateDate

            }).OrderBy(x => x.UpdateDate).ToList();
            return View(result);
        }
        public JsonResult ChangeHome(int id, bool status)
        {
            var model = _db.Posts.Where(x => x.PostId == id).SingleOrDefault();
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
            var model = _db.Posts.Where(x => x.PostId == id).SingleOrDefault();
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
            var model = _db.Posts.Where(x => x.PostId == id).SingleOrDefault();
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
            var model = _db.Posts.Where(x => x.PostId == id).Select(x => new PostViewModel()
            {
                PostId = x.PostId,
                MenuId = x.MenuId,
                Name = x.Name,
                Description = x.Description,
                CategoryId = x.CategoryId.ToString(),
                Status = x.Status,
                Img = x.Img,
                Content = x.Content,
                HomeFlag = x.HomeFlag,
                HotFlag = x.HotFlag,
                IsShow = x.IsShow,
                Author = x.Author,
                CreateDate = x.CreateDate

            }).FirstOrDefault();
            return View(model);
        }
    }
}