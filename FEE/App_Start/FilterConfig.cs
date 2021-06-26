using FEE.Authorize;
using FEE.Dtos;
using FEE.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FEE
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SessionFilter());
        }
        
    }
    public class SessionFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.Session["UserID"] == null)
            {
                FEEDbContext db = new FEEDbContext();
                bool Exist = db.Users.Any(e => e.Username == HttpContext.Current.User.Identity.Name);
                if (Exist)
                {
                    var user = db.Users.Where(e => e.Username == HttpContext.Current.User.Identity.Name).First();
                    var userSession = new UserSession();
                    userSession.Id = user.Id;
                    userSession.Name = user.Name;
                    userSession.RoleId = user.RoleId;
                    userSession.Username = user.Username;
                    userSession.DepartmentId = user.DepartmentId;
                    userSession.Image = user.Image;
                    HttpContext.Current.Session.Add("USER", userSession);
                    HttpContext.Current.Session.Add("PERMISSION", AuthPermission.GetProfileService(user.Id));
                }
            }
            base.OnActionExecuting(filterContext);
        }

    }
}
