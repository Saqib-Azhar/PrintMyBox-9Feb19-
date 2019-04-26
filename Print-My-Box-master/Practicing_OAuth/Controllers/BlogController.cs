using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practicing_OAuth.Controllers
{
    [RequireHttps]

    public class BlogController : Controller
    {
        // GET: Blog
        [Route("Blogs")]
        public ActionResult BlogView(int? url = 1)
        {
            return View("~/Views/Home/Blog.cshtml");
        }
    }
}