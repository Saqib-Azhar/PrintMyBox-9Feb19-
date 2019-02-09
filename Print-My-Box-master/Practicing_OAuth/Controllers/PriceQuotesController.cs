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
    public class PriceQuotesController : Controller
    {
        private Practicing_OAuthEntities db = new Practicing_OAuthEntities();

        // GET: PriceQuotes
        public ActionResult Index()
        {
            return View(db.PriceQuotes.Where(s => s.IsDeleted == false).ToList());
        }

        // GET: PriceQuotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceQuote priceQuote = db.PriceQuotes.Find(id);
            if (priceQuote == null)
            {
                return HttpNotFound();
            }
            priceQuote.Seen = true;
            priceQuote.SeenTime = DateTime.Now;
            db.SaveChanges();
            return View(priceQuote);
        }

        // GET: PriceQuotes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PriceQuotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Email,PhoneNumber,Stock,Color,Quantity,Height,Width,Depth,Comments,File,SubmittedTime,Seen,IsDeleted,SeenTime")] PriceQuote priceQuote)
        {
            if (ModelState.IsValid)
            {
                db.PriceQuotes.Add(priceQuote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(priceQuote);
        }

        // GET: PriceQuotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceQuote priceQuote = db.PriceQuotes.Find(id);
            if (priceQuote == null)
            {
                return HttpNotFound();
            }
            return View(priceQuote);
        }

        // POST: PriceQuotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Email,PhoneNumber,Stock,Color,Quantity,Height,Width,Depth,Comments,File,SubmittedTime,Seen,IsDeleted,SeenTime")] PriceQuote priceQuote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(priceQuote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(priceQuote);
        }
        public ActionResult TemporaryDelete(int? id)
        {
            PriceQuote priceQuote = db.PriceQuotes.Find(id);
            priceQuote.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: PriceQuotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceQuote priceQuote = db.PriceQuotes.Find(id);
            if (priceQuote == null)
            {
                return HttpNotFound();
            }
            return View(priceQuote);
        }

        // POST: PriceQuotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PriceQuote priceQuote = db.PriceQuotes.Find(id);
            db.PriceQuotes.Remove(priceQuote);
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
