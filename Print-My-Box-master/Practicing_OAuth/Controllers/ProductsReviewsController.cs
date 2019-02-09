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
    public class ProductsReviewsController : Controller
    {
        private Practicing_OAuthEntities db = new Practicing_OAuthEntities();

        // GET: ProductsReviews
        public ActionResult Index()
        {
            var productsReviews = db.ProductsReviews.Include(p => p.Product);
            return View(productsReviews.ToList());
        }

        // GET: ProductsReviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsReview productsReview = db.ProductsReviews.Find(id);
            if (productsReview == null)
            {
                return HttpNotFound();
            }
            return View(productsReview);
        }

        // GET: ProductsReviews/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: ProductsReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,ReviewerName,Review,Rating,CreatedAt")] ProductsReview productsReview)
        {
            if (ModelState.IsValid)
            {
                db.ProductsReviews.Add(productsReview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", productsReview.ProductId);
            return View(productsReview);
        }

        // GET: ProductsReviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsReview productsReview = db.ProductsReviews.Find(id);
            if (productsReview == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", productsReview.ProductId);
            return View(productsReview);
        }

        // POST: ProductsReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,ReviewerName,Review,Rating,CreatedAt")] ProductsReview productsReview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productsReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", productsReview.ProductId);
            return View(productsReview);
        }

        // GET: ProductsReviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsReview productsReview = db.ProductsReviews.Find(id);
            if (productsReview == null)
            {
                return HttpNotFound();
            }
            return View(productsReview);
        }

        // POST: ProductsReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductsReview productsReview = db.ProductsReviews.Find(id);
            db.ProductsReviews.Remove(productsReview);
            db.SaveChanges();
            return Redirect("/ProductsReviews/Index");
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
