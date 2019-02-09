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
    public class CategoryTypesController : Controller
    {
        private Practicing_OAuthEntities db = new Practicing_OAuthEntities();

        // GET: CategoryTypes
        public ActionResult Index()
        {
            return View(db.CategoryTypes.ToList());
        }

        // GET: CategoryTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryType categoryType = db.CategoryTypes.Find(id);
            if (categoryType == null)
            {
                return HttpNotFound();
            }
            return View(categoryType);
        }

        // GET: CategoryTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryType1")] CategoryType categoryType)
        {
            if (ModelState.IsValid)
            {
                db.CategoryTypes.Add(categoryType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoryType);
        }

        // GET: CategoryTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryType categoryType = db.CategoryTypes.Find(id);
            if (categoryType == null)
            {
                return HttpNotFound();
            }
            return View(categoryType);
        }

        // POST: CategoryTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryType1")] CategoryType categoryType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoryType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoryType);
        }

        // GET: CategoryTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryType categoryType = db.CategoryTypes.Find(id);
            if (categoryType == null)
            {
                return HttpNotFound();
            }
            return View(categoryType);
        }

        // POST: CategoryTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoryType categoryType = db.CategoryTypes.Find(id);
            db.CategoryTypes.Remove(categoryType);
            db.SaveChanges();
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
