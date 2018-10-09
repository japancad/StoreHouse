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
    public class ImportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int modifiesDb;

        public ImportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Imports
        public async Task<IActionResult> Index(string searchStringPartner, bool notUsed)
        {
            var applicationDbContext = _context.Import.Include(e => e.Partner).Include(e => e.Product);
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


            //return View(await applicationDbContext.ToListAsync());
            return View(await applicationDbContext.OrderByDescending(x => x.date).ToListAsync());
        }

        public IActionResult Search(string searchStringProduct, bool notUsed)
        {
            var applicationDbContext = _context.Import.Include(e => e.Partner).Include(e => e.Product);
            var products = _context.Product.ToList();
            var vm = new List<Import>();

            foreach (var import in applicationDbContext)
            {
                vm.Add(import);
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

        // GET: Imports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var import = await _context.Import
                .Include(i => i.Partner)
                .Include(i => i.Product)
                .SingleOrDefaultAsync(m => m.ImportID == id);
            if (import == null)
            {
                return NotFound();
            }

            return View(import);
        }

        // GET: Imports/Create
        public IActionResult Create()
        {
            ViewData["PartnerID"] = new SelectList(_context.Partner.OrderBy(x => x.name), "PartnerID", "name");
            ViewData["ProductID"] = new SelectList(_context.Product.OrderBy(x => x.name), "ProductID", "name");
            return View();
        }

        // POST: Imports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImportID,date,ProductID,quantity,PartnerID,sale_price")] Import import)
        {
            if (ModelState.IsValid)
            {
                _context.Add(import);
                if (!ModifyProduct.createImport(import, _context))
                {

                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PartnerID"] = new SelectList(_context.Partner, "PartnerID", "PartnerID", import.PartnerID);
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "name", import.ProductID);
            return View(import);
        }

        // GET: Imports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var import = await _context.Import.SingleOrDefaultAsync(m => m.ImportID == id);
            modifiesDb = import.quantity;
            if (import == null)
            {
                return NotFound();
            }
            ViewData["PartnerID"] = new SelectList(_context.Partner, "PartnerID", "PartnerID", import.PartnerID);
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "name", import.ProductID);
            return View(import);
        }

        // POST: Imports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImportID,date,ProductID,quantity,PartnerID,sale_price")] Import import)
        {
            if (id != import.ImportID)
            {
                return NotFound();
            }

            

            if (ModelState.IsValid)
            {
                try
                {
                    if (!ModifyProduct.editImport(import, modifiesDb, _context))
                    {

                    }
                    _context.Update(import);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImportExists(import.ImportID))
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
            ViewData["PartnerID"] = new SelectList(_context.Partner, "PartnerID", "PartnerID", import.PartnerID);
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "name", import.ProductID);
            return View(import);
        }

        // GET: Imports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var import = await _context.Import
                .Include(i => i.Partner)
                .Include(i => i.Product)
                .SingleOrDefaultAsync(m => m.ImportID == id);
            if (import == null)
            {
                return NotFound();
            }

            return View(import);
        }

        // POST: Imports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            
            var import = await _context.Import.SingleOrDefaultAsync(m => m.ImportID == id);
            //sajat kod 
            if (!ModifyProduct.deleteImport(import, _context))
            {

            }

            _context.Import.Remove(import);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImportExists(int id)
        {
            return _context.Import.Any(e => e.ImportID == id);
        }
    }
}
