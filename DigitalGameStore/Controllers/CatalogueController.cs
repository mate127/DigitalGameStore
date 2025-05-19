using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DigitalGameStore.Data;
using DigitalGameStore.Models;

namespace DigitalGameStore.Controllers
{
    public class CatalogueController : Controller
    {
        private readonly AppDbContext _context;

        public CatalogueController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Catalogue
        public async Task<IActionResult> Index()
        {
            return View(await _context.Catalogues.ToListAsync());
        }

        // GET: Catalogue/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalogue = await _context.Catalogues
                .FirstOrDefaultAsync(m => m.CatalogueId == id);
            if (catalogue == null)
            {
                return NotFound();
            }

            return View(catalogue);
        }

        // GET: Catalogue/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalogue/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatalogueId,Name,Description")] Catalogue catalogue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catalogue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(catalogue);
        }

        // GET: Catalogue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalogue = await _context.Catalogues.FindAsync(id);
            if (catalogue == null)
            {
                return NotFound();
            }
            return View(catalogue);
        }

        // POST: Catalogue/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatalogueId,Name,Description")] Catalogue catalogue)
        {
            if (id != catalogue.CatalogueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catalogue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatalogueExists(catalogue.CatalogueId))
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
            return View(catalogue);
        }

        // GET: Catalogue/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalogue = await _context.Catalogues
                .FirstOrDefaultAsync(m => m.CatalogueId == id);
            if (catalogue == null)
            {
                return NotFound();
            }

            return View(catalogue);
        }

        // POST: Catalogue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catalogue = await _context.Catalogues.FindAsync(id);
            if (catalogue != null)
            {
                _context.Catalogues.Remove(catalogue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatalogueExists(int id)
        {
            return _context.Catalogues.Any(e => e.CatalogueId == id);
        }
    }
}
