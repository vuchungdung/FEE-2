 using FEE.Authorize;
using FEE.Dtos;
using FEE.Enums;
using FEE.Library;
using FEE.Models;
using FEE.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTEHY.Model.Constants;

namespace FEE.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private FEEDbContext _db = new FEEDbContext();

        [ClaimRequirementFilter(Command = CommandCode.VIEW, Function = FunctionCode.SYSTEM_USER)]
        public ActionResult Index()
        {
            var result = from x in _db.Users
                         join d in _db.Departments
                         on x.DepartmentId equals d.DepartmentId
                         join r in _db.Roles
                         on x.RoleId equals r.RoleId
                         select new UserViewModel()
                         {
                             Id = x.Id,
                             Name = x.Name,
                             Username = x.Username,
                             Email = x.Email,
                             Phone = x.Phone,
                             Status = (UserStatus)x.Status,
                             CreateDate = x.CreateDate,
                             Role = r.Name,
                             Department = d.Name,
                             Image = x.Image
                         };
            var list = result.OrderByDescending(x => x.Id).ToList();
            return View(list);
        }
        [HttpGet]
        [ClaimRequirementFilter(Command = CommandCode.CREATE, Function = FunctionCode.SYSTEM_USER)]
        public ActionResult Create()
        {
            var model = new UserViewModel()
            {
                Departments = Helper.ListDepartments().ToList(),
                Roles = Helper.ListRoles().ToList()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(UserViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = (UserSession)Session["USER"];
                    var model = new User();
                    model.Name = viewModel.Name;
                    model.Status = (int)viewModel.Status;
                    model.Deleted = false;
                    model.CreateBy = user.Id;
                    model.CreateDate = DateTime.Now;
                    model.DepartmentId = viewModel.DepartmentId;
                    model.RoleId = viewModel.RoleId;
                    model.Phone = viewModel.Phone;
                    model.Password = XString.ToMD5(viewModel.Password);
                    model.Username = viewModel.Username;
                    model.Email = viewModel.Email;
                    if(viewModel.File == null)
                    {
                        model.Image = "/ImageUsers/unnamed.png";
                    }
                    else
                    {
                        string fileName = Path.GetFileName(viewModel.File.FileName);
                        model.Image = "~/ImageUsers/" + fileName;
                        string path = Path.Combine(Server.MapPath("~/ImageUsers"), fileName);
                        viewModel.File.SaveAs(path);
                    }
                    _db.Users.Add(model);
                    _db.SaveChanges();
                    Notification.set_flash("Lưu thành công!", "success");
                    ModelState.Clear();
                }
                else
                {
                    Notification.set_flash("Nhập đầy đủ thông tin!", "warning");
                }
                viewModel = new UserViewModel();
                viewModel.Departments = Helper.ListDepartments().ToList();
                viewModel.Roles = Helper.ListRoles().ToList();
                return RedirectToAction("Index");
            }
            catch
            {
                Notification.set_flash("Thêm thất bại!", "warning");
                throw;
            }
        }
        [HttpGet]
        [ClaimRequirementFilter(Command = CommandCode.UPDATE, Function = FunctionCode.SYSTEM_USER)]
        public ActionResult Update(int id)
        {
            var model = _db.Users.Where(x => x.Id == id).Select(x => new UserViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Username = x.Username,
                Email = x.Email,
                Phone = x.Phone,
                Status = (UserStatus)x.Status,
                CreateDate = x.CreateDate,
                RoleId = x.RoleId,
                DepartmentId = x.DepartmentId,
                Image = x.Image

            }).SingleOrDefault();
            model.Departments = Helper.ListDepartments().ToList();
            model.Roles = Helper.ListRoles().ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(UserViewModel viewModel)
        {
            try
            {
                var model = _db.Users.Find(viewModel.Id);
                if (ModelState.IsValid)
                {
                    var user = (UserSession)Session["USER"];                    
                    model.Name = viewModel.Name;
                    model.Status = (int)viewModel.Status;
                    model.Deleted = false;
                    model.UpdateBy = user.Id;
                    model.UpdateDate = DateTime.Now;
                    model.DepartmentId = viewModel.DepartmentId;
                    model.RoleId = viewModel.RoleId;
                    model.Phone = viewModel.Phone;
                    model.Email = viewModel.Email;
                    if (viewModel.File == null)
                    {
                        model.Image = viewModel.Image;
                    }
                    else
                    {
                        string fileName = Path.GetFileName(viewModel.File.FileName);
                        model.Image = "/ImageUsers/" + fileName;
                        string path = Path.Combine(Server.MapPath("~/ImageUsers"), fileName);
                        viewModel.File.SaveAs(path);
                    }
                    _db.SaveChanges();
                    Notification.set_flash("Lưu thành công!", "success");
                }
                else
                {
                    Notification.set_flash("Nhập đầy đủ thông tin!", "warning");
                }
                viewModel = new UserViewModel();
                viewModel.Departments = Helper.ListDepartments().ToList();
                viewModel.Roles = Helper.ListRoles().ToList();
                viewModel.Username = model.Username;
                return View(viewModel);
            }
            catch
            {
                Notification.set_flash("Lưu thất bại! Email thay thế không được trùng với Email của người dùng khác", "warning");
                throw;
            }
        }
        [ClaimRequirementFilter(Command = CommandCode.DELETE, Function = FunctionCode.SYSTEM_USER)]
        public JsonResult Delete(int id)
        {
            var model = _db.Users.Where(x => x.Id == id).FirstOrDefault();
            _db.Users.Remove(model);
            _db.SaveChanges();
            Notification.set_flash("Xóa vĩnh viễn!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult UserInfo()
        {
            var user = (UserSession)Session["USER"];
            var model = _db.Users.Where(x => x.Deleted == false && x.Id == user.Id).Select(x => new UserViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Phone = x.Phone,
                Image = x.Image
            }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult UserInfo(UserViewModel model)
        {
            try
            {
                var user = (UserSession)Session["USER"];
                var entity = _db.Users.Where(x => x.Id == user.Id && x.Deleted == false).FirstOrDefault();
                entity.Name = model.Name;
                entity.Email = model.Email;
                entity.Phone = model.Phone;
                entity.UpdateBy = user.Id;
                entity.UpdateDate = DateTime.Now;
                if (model.File == null)
                {
                    entity.Image = model.Image;
                }
                else
                {
                    string fileName = Path.GetFileName(model.File.FileName);
                    entity.Image = "/ImageUsers/" + fileName;
                    string path = Path.Combine(Server.MapPath("~/ImageUsers"), fileName);
                    model.File.SaveAs(path);
                }
                _db.SaveChanges();
                Notification.set_flash("Lưu thành công!", "success");
                model.Image = entity.Image;
                model.Username = entity.Username;
                return View(model);
            }
            catch(Exception ex)
            {
                Notification.set_flash("Lưu thất bại! Email thay thế không được trùng với Email của người dùng khác", "warning");
                return View();
            }
        }
        public JsonResult Refresh(int id)
        {
            var user = (UserSession)Session["USER"];
            var entity = _db.Users.Find(id);
            string pass = Guid.NewGuid().ToString().Substring(5, 10);
            entity.Password = XString.ToMD5(pass);
            entity.UpdateDate = DateTime.Now;
            entity.UpdateBy = entity.Id;
            _db.SaveChanges();
            return Json(pass, JsonRequestBehavior.AllowGet);
        }
    }
}