using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreHouse.Data;
using StoreHouse.Models;

namespace StoreHouse.Controllers
{
    [Authorize]
    public class StocksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StocksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stocks
        public ActionResult Index()
        {
            //var termeklista = new List<Product>();
            //termeklista = _context.Product.ToList();
            //var keszletlista = new List<Stock>();
            //var keszlet = new Stock();
            //var id = 0;
            //foreach (var termek in termeklista)
            //{
            //    keszlet.StockId = id;
            //    keszlet.product = _context.Product.SingleOrDefault(x => x.ProductID == termek.ProductID);
            //    var importSum = _context.Import
            //                            .Where(x => x.ProductID == termek.ProductID)
            //                            .ToArray()
            //                            .Select(xd => xd.quantity)
            //                            .Sum();
            //    var exportSum = _context.Export
            //                            .Where(x => x.ProductID == termek.ProductID)
            //                            .ToArray()
            //                            .Select(xd => xd.quantity)
            //                            .Sum();
            //    keszlet.stock = importSum - exportSum;

            //    keszletlista.Add(keszlet);
            //}


            return View(new List<Stock>());
        }

        // GET: Stocks/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Stocks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stocks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Stocks/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Stocks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Stocks/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Stocks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}