using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practicing_OAuth.Controllers
{
    public class PriceQuoteController : Controller
    {
        // GET: PriceQuote
        public ActionResult PriceQuoteView(int? url = 1, int? url2 = 1, int? url3 = 1)
        {
            return View("~/Views/Home/PriceQuote.cshtml");
        }
    }
}