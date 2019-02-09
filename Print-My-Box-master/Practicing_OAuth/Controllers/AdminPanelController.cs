using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practicing_OAuth.Models;

namespace Practicing_OAuth.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        private Practicing_OAuthEntities db = new Practicing_OAuthEntities();
        // GET: AdminPanel
        public ActionResult Panel()
        {
            var Userid = User.Identity;
            var UserObj = db.AspNetUsers.FirstOrDefault(s => s.UserName == Userid.Name);
            return View(UserObj);
        }
    }
}