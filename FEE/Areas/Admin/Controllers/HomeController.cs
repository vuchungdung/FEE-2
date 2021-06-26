using FEE.Dtos;
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
    public class HomeController : Controller
    {
        private FEEDbContext db = new FEEDbContext();

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var user = (UserSession)Session["USER"];               
                ViewBag.contacts = db.Contacts.Count();
                ViewBag.posts = db.Posts.Count(c => c.DepartmentId == user.DepartmentId);
                return View("Index");
            }
            return RedirectToAction("Login", "Auth");
        }
        public ActionResult Header()
        {
            return PartialView();
        }
        [Authorize]          
        public ActionResult SideBar()
        {
            var roles = db.Roles;
            var session = (UserSession)Session["USER"];
            var user = db.Users.Find(session.Id);
            var role = db.Roles.Find(session.RoleId).Name;
            var query = from f in db.Functions
                        join p in db.Permissions
                            on f.FunctionId equals p.FunctionId
                        join g in roles on p.RoleId equals g.RoleId
                        join c in db.Commands
                            on p.CommandId equals c.CommandId
                        where role.Contains(g.Name) && c.CommandId == CommandCode.VIEW
                        select new SideBarViewModel()
                        {
                            FunctionId = f.FunctionId,
                            Name = f.Name,
                            Url = f.Url,
                            ParentId = f.ParentId,
                            SortOrder = f.SortOrder,
                        };
            var data = query.Distinct()
                .OrderBy(x => x.ParentId)
                .ThenBy(x => x.SortOrder)
                .ToList();

            List<SideBarViewModel> listResult = new List<SideBarViewModel>();

            foreach (var item in data)
            {
                if (String.IsNullOrEmpty(item.ParentId))
                {
                    foreach (var menu in data)
                    {
                        if (menu.ParentId == item.FunctionId)
                        {
                            item.NavItem.Add(menu);
                        }

                    }
                    listResult.Add(item);
                }
                else
                {
                    foreach (var sub in data)
                    {
                        if (sub.ParentId == item.FunctionId)
                        {
                            item.NavItem.Add(sub);
                        }
                    }
                }
            }


            return PartialView(listResult);
        }
    }
}