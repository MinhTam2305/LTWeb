using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Figure2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
         
            routes.MapRoute(
                            name: "Trang chu",
                            url: "",
                            defaults: new { controller = "Figure", action = "Index", id = UrlParameter.Optional }
                        );
            routes.MapRoute(
            name: "XemChiTietDanhMuc",
            url: "XemChiTietDanhMuc/{action}/{id}",
            defaults: new { controller = "XemChiTietDanhMuc", action = "Index", id = UrlParameter.Optional }
        );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Figure", action = "Index", id = UrlParameter.Optional }
                ,namespaces: new string[] {"Figure.Controllers"}
            );
        }
    }
}
