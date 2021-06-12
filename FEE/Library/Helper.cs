using FEE.Models;
using FEE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

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
        public static IEnumerable<SelectListItem> ListMenus(List<MenuViewModel> list)
        {
            List<SelectListItem> lstSub = list
                .Select(m =>
                new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString(),
                    Disabled = m.Attr
                }
                ).ToList();
            return lstSub;
        }
        public static MvcHtmlString MyDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionText, bool canEdit)
        {
            if (canEdit) return html.DropDownListFor(expression, selectList, optionText,new { @class = "form-control" });
            return html.DropDownListFor(expression, selectList, optionText, new { @class = "form-control", @disabled="" });
        }
    }
}