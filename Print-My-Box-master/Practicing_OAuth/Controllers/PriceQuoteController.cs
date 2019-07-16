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
        [Route("PriceQuote")]
        public ActionResult PriceQuoteView(int? url = 1, int? url2 = 1, int? url3 = 1)
        {
            ViewBag.FormSubmissionMessage = HomeController.FormSubmissionMessage_HomeController;
            HomeController.FormSubmissionMessage_HomeController = "";
            return View("~/Views/Home/PriceQuote.cshtml");
        }
    }
}