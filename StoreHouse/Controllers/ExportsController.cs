﻿using System;
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

        public ExportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exports
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Export.Include(e => e.Partner).Include(e => e.Product);
            return View(await applicationDbContext.ToListAsync());
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
            ViewData["PartnerID"] = new SelectList(_context.Partner, "PartnerID", "name");
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "name");
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
                if (!ModifyProduct.modifyProductExport(export, _context))
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