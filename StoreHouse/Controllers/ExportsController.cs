using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreHouse.Data;
using StoreHouse.Models;

namespace StoreHouse.Controllers
{
    [Authorize]
    public class ExportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int modifiesDb;

        public ExportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //// GET: Exports
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Export.Include(e => e.Partner).Include(e => e.Product);
        //    //return View(await applicationDbContext.ToListAsync());
        //    return View(await applicationDbContext.OrderByDescending(x => x.date).ToListAsync());
        //}
        [HttpGet]
        public async Task<IActionResult> Index(string searchStringPartner, string searchStringProduct, bool notUsed)
        {
            var applicationDbContext = _context.Export.Include(e => e.Partner).Include(e => e.Product);
            var partners = _context.Partner.ToList();
            var products = _context.Product.ToList();


            if (!String.IsNullOrEmpty(searchStringPartner))
            {
                var partner = partners.SingleOrDefault(s => s.name.ToLowerInvariant().Contains(searchStringPartner.ToLowerInvariant()));
                if (!(partner == null))
                {
                    return View(await applicationDbContext.Where(x => x.PartnerID == partner.PartnerID).OrderByDescending(x => x.date).ToListAsync());
                }

            }
            if (!String.IsNullOrEmpty(searchStringProduct))
            {
                var product = products.Find(x => x.name.ToLowerInvariant().Contains(searchStringProduct.ToLowerInvariant()));
                if (!(product == null))
                {
                    return View(await applicationDbContext.Where(x => x.ProductID == product.ProductID).OrderByDescending(x => x.date).ToListAsync());
                }

            }

            //return View(await applicationDbContext.ToListAsync());
            return View(await applicationDbContext.OrderByDescending(x => x.date).ToListAsync());
        }

        //[HttpPost]
        //public async Task<IActionResult> Index( string searchStringPartner, string searchStringProduct, bool notUsed)
        //{
        //    var applicationDbContext = _context.Export.Include(e => e.Partner).Include(e => e.Product);
        //    var partners = _context.Partner.ToList();
        //    var products = _context.Product.ToList();
            

        //    if (!String.IsNullOrEmpty(searchStringPartner))
        //    {
        //        var partner = partners.SingleOrDefault(s => s.name.ToLowerInvariant().Contains(searchStringPartner.ToLowerInvariant()));
        //        if (!(partner == null))
        //        {
        //            return View(await applicationDbContext.Where(x => x.PartnerID == partner.PartnerID).OrderByDescending(x => x.date).ToListAsync());
        //        }
                
        //    }
        //    if (!String.IsNullOrEmpty(searchStringProduct))
        //    {
        //        var product = products.SingleOrDefault(s => s.name.ToLowerInvariant().Contains(searchStringProduct.ToLowerInvariant()));
        //        if (!(products == null))
        //        {
        //            return View(await applicationDbContext.Where(x => x.PartnerID == product.ProductID).OrderByDescending(x => x.date).ToListAsync());
        //        }
                
        //    }


        //    //return View(await applicationDbContext.ToListAsync());
        //    return View(await applicationDbContext.OrderByDescending(x => x.date).ToListAsync());
        //}

        
        public IActionResult Search(string searchStringProduct,bool notUsed)
        {
            var applicationDbContext = _context.Export.Include(e => e.Partner).Include(e => e.Product);
            var products = _context.Product.ToList();
            var vm = new List<Export>();

            foreach (var export in applicationDbContext)
            {
                vm.Add(export);
            }

            var er = searchStringProduct.Length == 0;
            
            var filteredVm = searchStringProduct.Length == 0
                                            ? vm
                                            : vm.Where(x => x.Product.name.ToLowerInvariant().Contains(searchStringProduct.ToLowerInvariant()))
                                            ;

            //if (!String.IsNullOrEmpty(searchStringProduct))
            //{
            //    var product = products.SingleOrDefault(s => s.name.Contains(searchStringProduct));
            //    if (!(products == null))
            //    {
            //        return View(await applicationDbContext.Where(x => x.PartnerID == product.ProductID).OrderByDescending(x => x.date).ToListAsync());
            //    }
            //}


            //return View(await applicationDbContext.ToListAsync());
            return View(filteredVm);
        }


        // GET: Exports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var export = await _context.Export
                .Include(e => e.Partner)
                .Include(e => e.Product)
                .SingleOrDefaultAsync(m => m.ExportID == id);
            if (export == null)
            {
                return NotFound();
            }

            return View(export);
        }

        // GET: Exports/Create
        public IActionResult Create()
        {
            ViewData["PartnerID"] = new SelectList(_context.Partner.OrderBy(x => x.name), "PartnerID", "name");
            ViewData["ProductID"] = new SelectList(_context.Product.OrderBy(x=>x.name), "ProductID", "name");
            return View();
        }

        // POST: Exports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExportID,date,ProductID,quantity,PartnerID,sale_price")] Export export)
        {
            if (ModelState.IsValid)
            {
                _context.Add(export);
                if (!ModifyProduct.createExport(export, _context))
                {

                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartnerID"] = new SelectList(_context.Partner, "PartnerID", "PartnerID", export.PartnerID);
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "name", export.ProductID);
            return View(export);
        }

        // GET: Exports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var export = await _context.Export.SingleOrDefaultAsync(m => m.ExportID == id);
            modifiesDb = export.quantity;
            if (export == null)
            {
                return NotFound();
            }
            ViewData["PartnerID"] = new SelectList(_context.Partner, "PartnerID", "PartnerID", export.PartnerID);
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "name", export.ProductID);
            return View(export);
        }

        // POST: Exports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExportID,date,ProductID,quantity,PartnerID,sale_price")] Export export)
        {
            if (id != export.ExportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!ModifyProduct.editExport(export, modifiesDb, _context))
                    {

                    }
                    _context.Update(export);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExportExists(export.ExportID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartnerID"] = new SelectList(_context.Partner, "PartnerID", "PartnerID", export.PartnerID);
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "name", export.ProductID);
            return View(export);
        }

        // GET: Exports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var export = await _context.Export
                .Include(e => e.Partner)
                .Include(e => e.Product)
                .SingleOrDefaultAsync(m => m.ExportID == id);
            if (export == null)
            {
                return NotFound();
            }

            return View(export);
        }

        // POST: Exports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var export = await _context.Export.SingleOrDefaultAsync(m => m.ExportID == id);
            if (!ModifyProduct.deleteExport(export, _context))
            {

            }
            _context.Export.Remove(export);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExportExists(int id)
        {
            return _context.Export.Any(e => e.ExportID == id);
        }
    }
}
