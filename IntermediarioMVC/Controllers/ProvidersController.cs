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
    public class ProvidersController : Controller
    {
        private IntermediarioMVCContext db = new IntermediarioMVCContext();

        // GET: Providers
        public ActionResult Index()
        {
           
            return View(db.Providers.ToList());
        }

        // GET: Providers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // GET: Providers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonId,FirstName,Lastname,PhoneNumber,Address,ImagePath")] Provider provider)
        {
            if (ModelState.IsValid)
            {
                db.Providers.Add(provider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(provider);
        }

        // GET: Providers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            ProviderView providerView = ToView(provider);

            return View(providerView);
        }

        private ProviderView ToView(Provider provider)
        {
            return new ProviderView()
            {
                Address = provider.Address,
                FirstName = provider.FirstName,
                ImagePath = provider.ImagePath == null ? "~/Content/Images/noImage.png" : provider.ImagePath,
                LastName = provider.LastName,
                PhoneNumber = provider.PhoneNumber,
                ProviderId = provider.ProviderId,
                Purchases = provider.Purchases,
            };
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProviderView providerView)
        {
            if (ModelState.IsValid)
            {
                var pic = providerView.ImagePath;
                var folder = "~/Content/Images";
                if (providerView.ImageFile != null)
                {
                    pic = FileHelper.UploadPhoto(providerView.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }
                Provider provider = ToProduct(providerView);
                provider.ImagePath = pic;
                db.Entry(provider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(providerView);
        }

        private Provider ToProduct(ProviderView providerView)
        {
            return new Provider()
            {

                Address = providerView.Address,
                FirstName = providerView.FirstName,
                ImagePath = providerView.ImagePath,
                PhoneNumber = providerView.PhoneNumber,
                LastName = providerView.LastName,
                ProviderId = providerView.ProviderId,
                Purchases = providerView.Purchases

            };
        }

        // GET: Providers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provider provider = db.Providers.Find(id);
            if (provider == null)
            {
                return HttpNotFound();
            }
            return View(provider);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Provider provider = db.Providers.Find(id);
            db.Providers.Remove(provider);
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
                    ViewBag.Error = "The provider can not be deleted because it has related records";
                }
                else
                {
                    ViewBag.Error = ex.Message;
                }
                return View(provider);
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
