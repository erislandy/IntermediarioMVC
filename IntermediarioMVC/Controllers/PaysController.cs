using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntermediarioMVC.Models;

namespace IntermediarioMVC.Controllers
{
    public class PaysController : Controller
    {
        private IntermediarioMVCContext db = new IntermediarioMVCContext();

        public ActionResult ConfirmPay(int payId)
        {

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var pay = db.Pays.Find(payId);
                    

                    foreach (var sale in pay.Sales.ToList())
                    {
                        sale.SaleState = SaleState.Certificated;
                        db.Entry(sale).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    pay.Certificated = true;
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    
                }
               
            }
            return RedirectToAction("Index");
        }


        public ActionResult DeleteSale(int saleId, int payId)
        {
            var saleSelected = db.Sales.Find(saleId);
            saleSelected.SaleState = SaleState.PendingLiquidate;
            saleSelected.PayId = null;
            db.Entry(saleSelected).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction(string.Format("Details/{0}", payId));
        }
        public ActionResult AddSale(int saleId, int payId)
        {
            var saleSelected = db.Sales.Find(saleId);
            saleSelected.SaleState = SaleState.Liquidated;
            saleSelected.PayId = payId;
            db.Entry(saleSelected).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction(string.Format("Details/{0}", payId));
        }
        // GET: Pays
        public ActionResult Index()
        {
            var pays = db.Pays.Include(p => p.Provider);
            var list = pays.ToList();
            return View(pays.ToList());
        }

        // GET: Pays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pay pay = db.Pays.Find(id);
            var payDetailsView = new PayDetailsView()
            {
                PayId = pay.PayId,
                Certificated = pay.Certificated,
                Date = pay.Date,
                Provider = pay.Provider,
                ProviderId = pay.ProviderId,
                Sales = pay.Sales
            };

            payDetailsView.AvailableSales = db.Sales.Where(s => s.ProductInStock.ProviderId == pay.ProviderId &&
                                                                s.SaleState == SaleState.PendingLiquidate)
                                                    .ToList();
            if (pay == null)
            {
                return HttpNotFound();
            }
            return View(payDetailsView);
        }

        // GET: Pays/Create
        public ActionResult Create()
        {
            ViewBag.ProviderId = new SelectList(db.Providers, "ProviderId", "FirstName");
            return View();
        }

        // POST: Pays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PayId,ProviderId,Date,Certificated")] Pay pay)
        {
            if (ModelState.IsValid)
            {
                db.Pays.Add(pay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProviderId = new SelectList(db.Providers, "ProviderId", "FirstName", pay.ProviderId);
            return View(pay);
        }

        // GET: Pays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pay pay = db.Pays.Find(id);
            if (pay == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProviderId = new SelectList(db.Providers, "ProviderId", "FirstName", pay.ProviderId);
            return View(pay);
        }

        // POST: Pays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PayId,ProviderId,Date,Certificated")] Pay pay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProviderId = new SelectList(db.Providers, "ProviderId", "FirstName", pay.ProviderId);
            return View(pay);
        }

        // GET: Pays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pay pay = db.Pays.Find(id);
            if (pay == null)
            {
                return HttpNotFound();
            }
            return View(pay);
        }

        // POST: Pays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pay pay = db.Pays.Find(id);
            db.Pays.Remove(pay);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if(ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    ViewBag.Error = "The pay can not be deleted because it has related sales";
                }
                else
                {
                    ViewBag.Error = ex.Message;
                }
                return View(pay);
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
