using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practicing_OAuth.Controllers
{
    public class AboutController : Controller
    {
        // GET: About
        public ActionResult AboutView(int? url = 1, int? url2 = 1)
        {
            return View("~/Views/Home/About.cshtml");
        }
    }
}