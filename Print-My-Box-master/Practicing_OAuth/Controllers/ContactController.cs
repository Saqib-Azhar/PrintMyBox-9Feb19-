using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practicing_OAuth.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult ContactView(int? url1 = 1, int? url2 = 1,int? url3 = 1, int? url4 = 1 )
        {
            return View("~/Views/Home/Contact.cshtml");
        }
    }
}