using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web; 
using System.Web.Mvc;
using IntermediarioMVC.Models;
using IntermediarioMVC.Helpers;

namespace IntermediarioMVC.Controllers
{
    public class ProductsController : Controller
    {
        private IntermediarioMVCContext db = new IntermediarioMVCContext();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
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

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductView productView)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Images";
                if (productView.ImageFile != null)
                {
                    pic = FileHelper.UploadPhoto(productView.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                Product product = ToProduct(productView);
                product.ImagePath = pic;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", productView.CategoryId);
            return View(productView);
        }

        private Product ToProduct(ProductView productView)
        {
            return new Product()
            {

                Category = productView.Category,
                CategoryId = productView.CategoryId,
                ImagePath = productView.ImagePath,
                ProductId = productView.ProductId,
                ProductName = productView.ProductName,
                Purchases = productView.Purchases,
                Remarks = productView.Remarks
            };
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
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", product.CategoryId);
            var productView = ToView(product);

            return View(productView);
        }

        private ProductView ToView(Product product)
        {
            return new ProductView()
            {
                Category = product.Category,
                CategoryId = product.CategoryId,
                ImagePath = product.ImagePath == null ? "~/Content/Images/noImage.png":product.ImagePath,
                ProductName = product.ProductName,
                ProductId = product.ProductId,
                Purchases = product.Purchases,
                Remarks = product.Remarks

            };
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductView productView)
        {
            if (ModelState.IsValid)
            {
                var pic = productView.ImagePath;
                var folder = "~/Content/Images";
                if (productView.ImageFile != null)
                {
                    pic = FileHelper.UploadPhoto(productView.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                Product product = ToProduct(productView);
                product.ImagePath = pic;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", productView.CategoryId);
            return View(productView);
        }

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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    ViewBag.Error = "The product can not be deleted because it has related records";
                }
                else
                {
                    ViewBag.Error = ex.Message;
                }
                return View(product);
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
