using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace FEE
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["visitors"] = 0;
            Application["onlines"] = 0;

        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            Application.Lock();
            Application["Notification"] = "";
            Application["visitors"] = Convert.ToInt32(Application["visitors"]) + 1;
            Application["onlines"] = Convert.ToInt32(Application["onlines"]) + 1;
            Application.UnLock();
        }
        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["onlines"] = Convert.ToUInt32(Application["onlines"]) - 1;
            Application.UnLock();
        }
    }
}
