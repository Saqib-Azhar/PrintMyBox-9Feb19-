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
                name: "Blog",
                url: "Blog",
                defaults: new { controller = "Blog", action = "BlogView", url = 1 }
            );
            routes.MapRoute(
                name: "About",
                url: "About",
                defaults: new { controller = "About", action = "AboutView", url = 1, url2 = 1 }
            );
            routes.MapRoute(
                name: "PriceQuote",
                url: "PriceQuote",
                defaults: new { controller = "PriceQuote", action = "PriceQuoteViewPriceQuoteView", url = 1, url2 = 1, url3 = 1 }
            );
            routes.MapRoute(
                name: "Contact",
                url: "Contact",
                defaults: new { controller = "Contact", action = "ContactView", url1 = 1, url2 = 1, url3 = 1, url4 = 1 }
            );
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
