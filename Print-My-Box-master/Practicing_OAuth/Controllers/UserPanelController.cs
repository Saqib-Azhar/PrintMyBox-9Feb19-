using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Practicing_OAuth.Models;
using Microsoft.AspNet.Identity;

namespace Practicing_OAuth.Controllers
{
    [Authorize]
    public class UserPanelController : Controller
    {
        private Practicing_OAuthEntities db = new Practicing_OAuthEntities();
        // GET: UserPanel
        //public ActionResult Panel()
        //{
        //    var Userid = User.Identity;
        //    var UserObj = db.AspNetUsers.FirstOrDefault(s => s.UserName == Userid.Name);
        //    return View(UserObj);
        //}

        // GET: UserPanel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserPanel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserPanel/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AspNetUser aspNetUser, HttpPostedFileBase Images)
        {
            if (ModelState.IsValid)
            {
                if (Images != null)
                {
                    aspNetUser.ProfilePicture = new byte[Images.ContentLength];
                    Images.InputStream.Read(aspNetUser.ProfilePicture, 0, Images.ContentLength);
                }
                if (this.User.IsInRole("User") && aspNetUser.Id == null)
                {
                    aspNetUser.Id = User.Identity.GetUserId();
                }
                var currUser = this.User.Identity.GetUserId();

                var UserObj = db.AspNetUsers.FirstOrDefault(s => s.Id == currUser);
                UserObj.UserName = aspNetUser.UserName;
                UserObj.PhoneNumber = aspNetUser.PhoneNumber;
                UserObj.Email = aspNetUser.Email;
                UserObj.BackupEmail = aspNetUser.BackupEmail;
                UserObj.ProfilePicture = aspNetUser.ProfilePicture;
                UserObj.PhoneNumber = aspNetUser.PhoneNumber;
                UserObj.Address = aspNetUser.Address;
                UserObj.Gender = aspNetUser.Gender;
                UserObj.DateOfBirth = aspNetUser.DateOfBirth;
                db.SaveChanges();
            }
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Panel", "AdminPanel");
            }
            else
                return RedirectToAction("Panel", "UserPanel");
        }

        // GET: UserPanel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserPanel/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
