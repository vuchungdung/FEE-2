using FEE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEE.Library
{
    public static class Helper
    {
        public static IEnumerable<SelectListItem> ListCategories()
        {
            FEEDbContext db = new FEEDbContext();
            List<SelectListItem> lstSub = db.Categories
                .Select(m =>
                new SelectListItem
                {
                    Text = m.Name,
                    Value = m.CategoryId.ToString()
                }
                ).ToList();
            return lstSub;
        }
        public static IEnumerable<SelectListItem> ListDepartments()
        {
            FEEDbContext db = new FEEDbContext();
            List<SelectListItem> lstSub = db.Departments
                .Select(m =>
                new SelectListItem
                {
                    Text = m.Name,
                    Value = m.DepartmentId.ToString()
                }
                ).ToList();
            return lstSub;
        }
        public static IEnumerable<SelectListItem> ListRoles()
        {
            FEEDbContext db = new FEEDbContext();
            List<SelectListItem> lstSub = db.Roles.Where(x=>x.RoleId != 1)
                .Select(m =>
                new SelectListItem
                {
                    Text = m.Name,
                    Value = m.RoleId.ToString()
                }
                ).ToList();
            return lstSub;
        }
        public static IEnumerable<SelectListItem> ListTags()
        {
            FEEDbContext db = new FEEDbContext();
            List<SelectListItem> lstSub = db.Tags
                .Select(m =>
                new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                }
                ).ToList();
            return lstSub;
        }
    }
}