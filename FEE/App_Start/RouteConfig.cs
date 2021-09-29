using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FEE
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Danh sách",
               url: "danh-muc-{id}-{categoryId}-{tag}",
               defaults: new { controller = "Post", action = "Index", id = UrlParameter.Optional, categoryId = UrlParameter.Optional, tag = UrlParameter.Optional },
               namespaces: new[] { "FEE.Controllers" }
           );

            routes.MapRoute(
               name: "Tìm kiếm",
               url: "tim-kiem",
               defaults: new { controller = "Post", action = "Search" },
               namespaces: new[] { "FEE.Controllers" }
           );

            routes.MapRoute(
               name: "Phản hồi",
               url: "gui-phan-hoi",
               defaults: new { controller = "Contact", action = "Index" },
               namespaces: new[] { "FEE.Controllers" }
           );

            routes.MapRoute(
               name: "Tin nổi bật",
               url: "danh-sach-tin-noi-bat",
               defaults: new { controller = "Post", action = "PostHot" },
               namespaces: new[] { "FEE.Controllers" }
           );

            routes.MapRoute(
               name: "Tin",
               url: "{alias}-{id}",
               defaults: new { controller = "Post", action = "PostDetail", id = UrlParameter.Optional },
               namespaces: new[] { "FEE.Controllers" }
           );

            routes.MapRoute(
                name: "Trang chủ",
                url: "",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "FEE.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FEE.Controllers" }
            ); 
           
        }
    }
}
