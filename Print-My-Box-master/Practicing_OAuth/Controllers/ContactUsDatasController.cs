using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Practicing_OAuth.Models;

namespace Practicing_OAuth.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContactUsDatasController : Controller
    {
        private Practicing_OAuthEntities db = new Practicing_OAuthEntities();

        // GET: ContactUsDatas
        public ActionResult Index()
        {
            List<ContactUsData> contactData;
            try
            {
                contactData = db.ContactUsDatas.Where(s => s.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);
                contactData = db.ContactUsDatas.Where(s => s.IsDeleted == false).ToList();
            }
            return View(contactData);
        }

        // GET: ContactUsDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUsData contactUsData = db.ContactUsDatas.Find(id);
            if (contactUsData == null)
            {
                return HttpNotFound();
            }

            contactUsData.Seen = true;
            contactUsData.SeenTime = DateTime.Now;
            db.SaveChanges();
            return View(contactUsData);
        }

        // GET: ContactUsDatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactUsDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Email,PhoneNumber,Message,SubmittedTime,Seen,SeenTime,IsDeleted")] ContactUsData contactUsData)
        {
            if (ModelState.IsValid)
            {
                db.ContactUsDatas.Add(contactUsData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contactUsData);
        }

        // GET: ContactUsDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUsData contactUsData = db.ContactUsDatas.Find(id);
            if (contactUsData == null)
            {
                return HttpNotFound();
            }
            return View(contactUsData);
        }

        // POST: ContactUsDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Email,PhoneNumber,Message,SubmittedTime,Seen,SeenTime,IsDeleted")] ContactUsData contactUsData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactUsData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contactUsData);
        }

        // GET: ContactUsDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUsData contactUsData = db.ContactUsDatas.Find(id);
            try
            {
                if (contactUsData == null)
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {

                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);
                throw;
            }
            return View(contactUsData);
        }

        // POST: ContactUsDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactUsData contactUsData = db.ContactUsDatas.Find(id);
            try
            {
                db.ContactUsDatas.Remove(contactUsData);
                db.SaveChanges();

            }
            catch (Exception ex)
            {

                db.ContactUsDatas.Remove(contactUsData);
                db.SaveChanges();
                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);
                throw;
            }
            return RedirectToAction("Index");
        }

        public ActionResult TemporaryDelete(int? id)
        {
            ContactUsData contactUsData = db.ContactUsDatas.Find(id);
            
            try
            {
                contactUsData.IsDeleted = true;
                db.SaveChanges();
            }
            catch (Exception ex)
            {

                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);
                throw;
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
