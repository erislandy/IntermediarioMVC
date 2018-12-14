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
    public class TransferController : Controller
    {
        private IntermediarioMVCContext db = new IntermediarioMVCContext();

        
        #region Purchase operations
        public ActionResult Purchase()
        {
            var purchases = db.Purchases.Include(pu => pu.Provider)
                                        .Include(pu => pu.Product)
                                        .ToList();
            return View(purchases);
        }

        public ActionResult NewPurchase()
        {
            ViewBag.ProviderId = new SelectList(db.Providers, "ProviderId", "FirstName");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");

            return View();
        }

        [HttpPost]
        public ActionResult NewPurchase(Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                

                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Add purchase to list of purchases
                        db.Purchases.Add(purchase);
                        db.SaveChanges();

                        //Add amount in stock table 

                        var productInStock = db.ProductInStocks
                                                    .Where(ps => ps.ProductId == purchase.ProductId &&
                                                                 ps.ProviderId == purchase.ProviderId &&
                                                                 ps.State == StateEnum.Available)
                                                    .FirstOrDefault();

                        if(productInStock == null)
                        {
                            productInStock = new ProductInStock()
                            {
                                Amount = purchase.Amount,
                                PriceInput = purchase.PriceInput,
                                ProductId = purchase.ProductId,
                                ProviderId = purchase.ProviderId,
                                State = StateEnum.Available
                            };
                            var code = GenerateCode(productInStock);
                            productInStock.Code = code;
                            db.ProductInStocks.Add(productInStock);
                        }
                        else
                        {
                            productInStock.Amount += purchase.Amount;
                            db.Entry(productInStock).State = EntityState.Modified;
                        }
                        
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {

                        transaction.Rollback();
                        ModelState.AddModelError(
                            string.Empty,
                            "Purchase can not be added, Database thrown an error"
                            );
                        return View(purchase);
                    }
                }

                return RedirectToAction("Purchase");
            }

            ViewBag.ProviderId = new SelectList(db.Providers, "PersonId", "FirstName",purchase.ProviderId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", purchase.ProductId);
            return View(purchase);
        }

        public string GenerateCode(ProductInStock productInStock)
        {
            var code = productInStock.ProductId.ToString() +
                       productInStock.ProviderId.ToString() +
                       productInStock.State.ToString();
            return code;
        }

        #endregion

        #region Stock action result
        public ActionResult Stock()
        {
            ViewBag.Alert = "";
            var productInStockList = db.ProductInStocks.Include(s => s.Provider)
                                                       .Include(s => s.Product);
                                                       
            return View(productInStockList.Where(ps => ps.Amount > 0).ToList());
        }
        #endregion

        #region Change state of product in stock

        public ActionResult ModifyState(int productInStockId)
        {
            var productInStock = db.ProductInStocks.Find(productInStockId);
            var changeState = new ChangeState()
            {
                BeforeState = productInStock.State,
                Date = DateTime.Now,
                ProductInStockId = productInStock.ProductInStockId,
                ProductInStock = productInStock,

            };


            return View(changeState);
        }

        [HttpPost]
        public ActionResult ModifyState(ChangeState changeState)
        {
            if (ModelState.IsValid)
            {

                if (changeState.BeforeState == changeState.NextState)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Before and Next state can not be same values"
                        );
                    return View(changeState);
                }

                if (changeState.Amount == 0)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Amount can not be equals  0");
                    return View(changeState);
                }

                var productInStock = db.ProductInStocks.Find(changeState.ProductInStockId);

                if (changeState.Amount > productInStock.Amount)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        string.Format("Amount exceed permissible value: {0}", productInStock.Amount)
                        );
                    return View(changeState);
                }



                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Add changeState to list ChangeStates in database

                        db.ChangeStates.Add(changeState);
                        db.SaveChanges();

                        //substract amount of changeState in productInStock where property State has BeforeState  

                        productInStock.Amount -= changeState.Amount;
                        db.Entry(productInStock).State = EntityState.Modified;
                        db.SaveChanges();

                        //Add amount of changeState in productInStock where property State has NextState 

                        var code = GenerateCode(

                            new ProductInStock()
                            {
                                ProductId = changeState.ProductInStock.ProductId,
                                ProviderId = changeState.ProductInStock.ProviderId,
                                State = changeState.NextState
                            });

                        var productInStockNextState = db.ProductInStocks.Where(p => p.Code.Equals(code))
                                                                        .FirstOrDefault();

                        if (productInStockNextState == null)
                        {
                            productInStockNextState = new ProductInStock()
                            {
                                Amount = changeState.Amount,
                                Code = code,
                                PriceInput = productInStock.PriceInput,
                                ProductId = productInStock.ProductId,
                                ProviderId = productInStock.ProviderId,
                                State = changeState.NextState
                            };
                            db.ProductInStocks.Add(productInStockNextState);
                        }
                        else
                        {
                            productInStockNextState.Amount += changeState.Amount;
                            db.Entry(productInStockNextState).State = EntityState.Modified;
                        }

                        db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("Stock");
                    }
                    catch (Exception)
                    {

                        transaction.Rollback();
                        ModelState.AddModelError(
                            string.Empty,
                            "Purchase can not be added, Database thrown an error"
                            );
                        return View(changeState);
                    }
                }

            }

            return View(changeState);
        }

        #endregion


        #region Sale operations

        //New sale method
        public ActionResult SaleAction(int productInStockId)
        {
            var productInStock = db.ProductInStocks.Find(productInStockId);
            var sale = new Sale()
            {
                DateOfSale = DateTime.Now,
                ProductInStockId = productInStock.ProductInStockId,
                ProductInStock = productInStock,
             };

            return View(sale);
        }

        //New sale method

        [HttpPost]
        public ActionResult SaleAction(Sale sale)
        {
            ViewBag.Alert = "";

            if (ModelState.IsValid)
            {

               
                if (sale.Amount == 0)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Amount can not be equals  0");
                    return View(sale);
                }

                var productInStock = db.ProductInStocks.Find(sale.ProductInStockId);

                if (sale.Amount > productInStock.Amount)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        string.Format("Amount exceed permissible value: {0}", productInStock.Amount)
                        );
                    return View(sale);
                }

                if (productInStock.State != StateEnum.Available)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "You should select an available product"
                        );
                    return View(sale);
                  
                }

                
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Add sale to list Sales in database

                        db.Sales.Add(sale);
                        db.SaveChanges();

                        //substract amount of sale in productInStock   

                        productInStock.Amount -= sale.Amount;
                        db.Entry(productInStock).State = EntityState.Modified;
                        db.SaveChanges();

                        transaction.Commit();

                        return RedirectToAction("Stock");
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        ModelState.AddModelError(
                            string.Empty,
                            string.Format("Sale can not be added, Database thrown an error:{0}", ex.InnerException.InnerException.Message)
                            );
                        return View(sale);
                    }
                }

              


            }


            return View(sale);
        }


        #endregion


    }
}