using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Practicing_OAuth
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "Index",
                defaults: new { controller = "Home", action = "IndexView" }
            );
            routes.MapRoute(
                name: "ProductByCategory",
                url: "category/{category}/",
                defaults: new { controller = "Products", action = "Category", category = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Products",
                url: "{prodName}/",
                defaults: new { controller = "Products", action = "Item", prodName = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
  
}
