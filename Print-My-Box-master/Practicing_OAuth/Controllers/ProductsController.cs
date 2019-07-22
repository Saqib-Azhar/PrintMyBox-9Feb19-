using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Practicing_OAuth.Models;
using System.IO;
using PagedList;
using PagedList.Mvc;
using System.Text;

namespace Practicing_OAuth.Controllers
{
    [RequireHttps]
    public class ProductsController : Controller
    {
        class searchClass
        {
            public List<Product> searchedProdList { get; set; }
            public string searchedQuery { get; set; }
        }
        private Practicing_OAuthEntities db = new Practicing_OAuthEntities();
        private static List<Product> ProductsList = new List<Product>();
        private static searchClass searchObjectsList = new searchClass();
        private static Product prodObjToEdit = new Product();

        [Authorize(Roles = "Admin")]
        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Product product = db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    //var product = db.Products.Find(id);
        //    //  return Redirect(Url.Action("Item","Products")+"?slugURL=" + product.SlugURL);// "Item", "Products", new { product.SlugURL });
        //    var productSlug = product.SlugURL.Replace('_', '-');
        //    return RedirectToAction("Item", "Products", new { prodName = productSlug });


        //}
        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [Route("{prodName}/")]
        public ActionResult Item(string prodName)
        {

            string id = prodName;

            if (string.IsNullOrEmpty(id))
            {
                return Redirect("/ ");
            }
            if (id.Contains("_"))
                return HttpNotFound();

            id = id.Replace('-', '_');
            Product product = db.Products.FirstOrDefault(s => s.SlugURL == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Reviews = db.ProductsReviews.Where(s => s.ProductId == product.Id);
            var Categories = db.Categories.Where(s => s.Id == product.CategoryId).ToList();
            ViewBag.CategoriesList = Categories;
            var Products = db.Products.Where(s => s.CategoryId == product.CategoryId).ToList();
            ViewBag.ProductsList = Products;
            return View(product);
        }

        public ActionResult Search(string query, int? pageNo = 1, int? pageSize = 16)
        {
            try
            {
                if (searchObjectsList.searchedProdList == null && searchObjectsList.searchedQuery != query)
                {
                    searchObjectsList.searchedQuery = query;
                    searchObjectsList.searchedProdList = new List<Product>();

                }
                if (searchObjectsList.searchedProdList == null || (searchObjectsList.searchedProdList.Count == 0 || searchObjectsList.searchedQuery != query))
                {
                    var splitedQuery = query.Split(' ');
                    if (splitedQuery.Count() > 1)
                    {
                        foreach (var subQuery in splitedQuery)
                        {
                            if (subQuery == "Box" || subQuery == "BOX" || subQuery == "box" || subQuery == "package" || subQuery == "PACKAGE" || subQuery == "Package" || subQuery == "packages" || subQuery == "PACKAGES" || subQuery == "Packages") 
                            {
                                continue;
                            }
                            var prodsList = db.Products.Where(s => s.Name.Contains(subQuery) && s.IsEnabled == true).ToList();
                            foreach(var listItem in prodsList)
                            {
                                searchObjectsList.searchedProdList.Add(listItem);
                            }
                        }
                    }
                    else
                    {
                        searchObjectsList.searchedProdList = db.Products.Where(s => s.Name.Contains(query) && s.IsEnabled == true).ToList();
                    }
                    searchObjectsList.searchedQuery = query;
                }
                ViewBag.QueryString = query;
                return View(searchObjectsList.searchedProdList.ToPagedList(Convert.ToInt32(pageNo), Convert.ToInt32(pageSize)));


            }
            catch (Exception ex)
            {
                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);
                return RedirectToAction("NoResultFound","Products",query);
            }
        }
        public ActionResult NoResultFound(string query)
        {
            ViewBag.QueryString = query;
            return View();
        }
        [Authorize(Roles = "Admin")]
        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName");
            return View();
        }
        
        [Authorize(Roles = "Admin")]
        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Name,Price,SlugURL,Description,IsEnabled,Image1,Image2,Image3,Image4,Image5,UploadedDate,CategoryId,Product_Description,Specifications,,MetaDescription,MetaTags")] Product product, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3, HttpPostedFileBase Image4, HttpPostedFileBase Image5)
        {
            if (Image1 != null)
            {
                string pic = System.IO.Path.GetFileName(Image1.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // file is uploaded
                Image1.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image1.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
                product.Image1 = Image1.FileName;

            }
            if (Image2 != null)
            {
                string pic = System.IO.Path.GetFileName(Image2.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // Image2 is uploaded
                Image2.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image2.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
                product.Image2 = Image2.FileName;

            }
            if (Image3 != null)
            {
                string pic = System.IO.Path.GetFileName(Image3.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // Image3 is uploaded
                Image3.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image3.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
                product.Image3 = Image3.FileName;

            }
            if (Image4 != null)
            {
                string pic = System.IO.Path.GetFileName(Image4.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // Image4 is uploaded
                Image4.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image4.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

                product.Image4 = Image4.FileName;
            }
            if (Image5 != null)
            {
                string pic = System.IO.Path.GetFileName(Image5.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // Image5 is uploaded
                Image5.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image5.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

                product.Image5 = Image5.FileName;
            }
            if (ModelState.IsValid)
            {
                product.SlugURL = product.Name.Replace(" ", "_").ToLower();
                product.SlugURL = product.SlugURL.Replace("-", "_");
                product.SlugURL = product.SlugURL.Replace("/", "_");
                product.SlugURL = product.SlugURL.Replace("\\", "_");
                product.Specifications = product.Description;
                product.UploadedDate = DateTime.Now;
                db.Products.Add(product);
                db.SaveChanges();
                return Redirect("/Products/Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryId);
            prodObjToEdit = product;
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,SlugURL,Description,IsEnabled,UploadedDate,CategoryId,Product_Description,Specifications,MetaDescription,MetaTags")] Product product, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3, HttpPostedFileBase Image4, HttpPostedFileBase Image5)
        {
            if (Image1 != null)
            {
                string pic = System.IO.Path.GetFileName(Image1.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // file is uploaded
                Image1.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image1.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
                product.Image1 = Image1.FileName;

            }
            if (Image2 != null)
            {
                string pic = System.IO.Path.GetFileName(Image2.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // Image2 is uploaded
                Image2.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image2.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
                product.Image2 = Image2.FileName;

            }
            if (Image3 != null)
            {
                string pic = System.IO.Path.GetFileName(Image3.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // Image3 is uploaded
                Image3.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image3.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
                product.Image3 = Image3.FileName;

            }
            if (Image4 != null)
            {
                string pic = System.IO.Path.GetFileName(Image4.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // Image4 is uploaded
                Image4.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image4.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

                product.Image4 = Image4.FileName;
            }
            if (Image5 != null)
            {
                string pic = System.IO.Path.GetFileName(Image5.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/UploadedProductImages"), pic);
                // Image5 is uploaded
                Image5.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    Image5.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

                product.Image5 = Image5.FileName;
            }
            if(Image1 == null && Image2 == null && Image3 == null &&Image4 == null && Image5 == null)
            {
                try
                {
                    product.Image1 = prodObjToEdit.Image1;
                    product.Image2 = prodObjToEdit.Image2;
                    product.Image3 = prodObjToEdit.Image3;
                    product.Image4 = prodObjToEdit.Image4;
                    product.Image5 = prodObjToEdit.Image5;
                }
                catch (Exception ex)
                {

                    HomeController.infoMessage(ex.Message);
                    HomeController.writeErrorLog(ex);
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("/Products/Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryId);
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return Redirect("/Products/Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //[AllowAnonymous]
        //public ActionResult productsByCategoryType(int? id, int? pageNo = 1, int? pageSize = 16)
        //{
        //    try
        //    {
        //        if (ProductsList.Count == 0)
        //        {
        //            var products = db.Products.ToList();
        //            ProductsList = products;
        //        }
        //        ViewBag.Id = id;
        //        var ProdSubList = ProductsList.Where(s => s.Category.CategoryTypeId == id && s.IsEnabled == true);
        //        return View(ProdSubList.ToPagedList(Convert.ToInt32(pageNo), Convert.ToInt32(pageSize)));

        //    }
        //    catch (Exception ex)
        //    {
        //        HomeController.infoMessage(ex.Message);
        //        HomeController.writeErrorLog(ex);
        //        if (ProductsList.Count == 0)
        //        {
        //            var products = db.Products.ToList();
        //            ProductsList = products;
        //        }
        //        ViewBag.Id = id;
        //        var ProdSubList = ProductsList.Where(s => s.Category.CategoryTypeId == id && s.IsEnabled == true);
        //        return View(ProdSubList.ToPagedList(Convert.ToInt32(pageNo), Convert.ToInt32(pageSize))); 
        //    }

        //}
        static int? categoryWisePageNo = 1;
        static int? categoryWisePageSize = 16;
        static int? categoryWiseId = -1;
        //public ActionResult productsByCategory(int id, int? pageNo = 1, int? pageSize = 16)
        //{
        //    categoryWisePageNo = pageNo;
        //    categoryWisePageSize = pageSize;
        //    categoryWiseId = id;
        //    var category = db.Categories.FirstOrDefault(s => s.Id == id);
        //    var categoryName = category.CategoryName.Replace(' ', '-');
            

        //    StringBuilder builder = new StringBuilder(categoryName);
        //    builder.Replace("-&-", "-And-");
        //    categoryName = builder.ToString();
        //    return RedirectToAction("Category", "Products", new { category = categoryName });
        //}

        [Route("category/{category}/")]
        public ActionResult Category(string category, int? pageNo = 1, int? pageSize = 16)
        {

            try
            {
                ViewBag.CategorySlug = category;
                if (ProductsList.Count == 0)
                {
                    var products = db.Products.ToList();
                    ProductsList = products;
                }
                //if(categoryWiseId == -1)
                //{

                    StringBuilder builder = new StringBuilder(category);
                    builder.Replace("-And-", "-&-");
                    category = builder.ToString();
                    category = category.Replace('-', ' ');
                    var categoryObj = db.Categories.FirstOrDefault(s => s.CategoryName == category);
                    categoryWiseId = categoryObj.Id;
                //}
                categoryWisePageNo = pageNo;
                categoryWisePageSize = pageSize;
                //var categoryRes = db.Categories.FirstOrDefault(s=>s.CategoryName == category);
                //categoryWiseId = categoryRes.Id;
                if (category.Contains("_"))
                    return HttpNotFound();
                ViewBag.Id = categoryWiseId;
                var ProdSubList = ProductsList.Where(s => s.CategoryId == categoryWiseId && s.IsEnabled == true);
                ViewBag.CategoryName = category;
                return View("productsByCategory",ProdSubList.ToPagedList(Convert.ToInt32(categoryWisePageNo), Convert.ToInt32(categoryWisePageSize)));
            }
            catch (Exception ex)
            {
                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);
                if (ProductsList.Count == 0)
                {
                    var products = db.Products.ToList();
                    ProductsList = products;
                }
                if (categoryWiseId == -1)
                {

                    if (category.Contains("_"))
                        return HttpNotFound();
                    StringBuilder builder = new StringBuilder(category);
                    builder.Replace("_And_", "&");
                    category = builder.ToString();
                    category = category.Replace('_', ' ');
                    var categoryObj = db.Categories.FirstOrDefault(s => s.CategoryName == category);
                    categoryWiseId = categoryObj.Id;
                }
                ViewBag.Id = categoryWiseId;
                ViewBag.CategoryName = category;
                var ProdSubList = ProductsList.Where(s => s.CategoryId == categoryWiseId && s.IsEnabled == true);
                return View("productsByCategory", ProdSubList.ToPagedList(Convert.ToInt32(categoryWisePageNo), Convert.ToInt32(categoryWisePageSize)));
            }
        }

        public ActionResult AllProducts(int? pageNo = 1,int? pageSize = 16)
        {
            if (ProductsList.Count == 0)
            {
                var products = db.Products.ToList();
                ProductsList = products;
            }
            return View(ProductsList.ToPagedList(Convert.ToInt32(pageNo), Convert.ToInt32(pageSize)));
        }

        [HttpPost]
        public ActionResult SubmitReview(FormCollection fc)
        {
            try
            {

                CaptchaResponse response = HomeController.ValidateCaptcha(Request["g-recaptcha-response"]);
                if (response.Success)
                {
                    var ProductId = Convert.ToInt32(fc["ProductId"]);
                    var Reviewer = fc["ReviewerName"];
                    var Rating = fc["Rating"];
                    var Review = fc["Review"];
                    var ReviewObj = new ProductsReview();
                    ReviewObj.ProductId = ProductId;
                    ReviewObj.Rating = Convert.ToInt32(Rating);
                    ReviewObj.Review = Review;
                    ReviewObj.ReviewerName = Reviewer;
                    ReviewObj.CreatedAt = DateTime.Now;
                    db.ProductsReviews.Add(ReviewObj);
                    db.SaveChanges();
                    int id = ProductId;
                    TempData["FormSubmitMessage"] = "Request Successfully Submitted!";
                    return Redirect(Request.UrlReferrer.ToString());
                }
                else
                {
                    TempData["FormSubmitMessage"] = "Error From Google ReCaptcha : " + response.ErrorMessage[0].ToString();
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
            catch (Exception ex)
            {
                HomeController.infoMessage(ex.Message);
                HomeController.writeErrorLog(ex);

                var ProductId = Convert.ToInt32(fc["ProductId"]);
                int id = ProductId;
                TempData["FormSubmitMessage"] = "Something went wrong please try again!";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}
