﻿using System;
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
    public class LicenceController : Controller
    {
        private readonly AppDbContext _context;

        public LicenceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Licence
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Licences.Include(l => l.Game).Include(l => l.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Licence/Details/5
        public async Task<IActionResult> Details(int? UserId, int? GameId)
        {
            if (UserId == null || GameId == null)
            {
                return NotFound();
            }

            var licence = await _context.Licences
                .Include(l => l.Game)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.UserId == UserId && m.GameId == GameId);
            if (licence == null)
            {
                return NotFound();
            }

            return View(licence);
        }

        // GET: Licence/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Discriminator");
            return View();
        }

        // POST: Licence/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,UserId,GameId")] Licence licence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(licence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", licence.GameId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Discriminator", licence.UserId);
            return View(licence);
        }

        // GET: Licence/Edit/5
        public async Task<IActionResult> Edit(int? UserId, int? GameId)
        {
            if (UserId == null || GameId == null)
            {
                return NotFound();
            }

            var licence = await _context.Licences.FindAsync(UserId, GameId);
            if (licence == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", licence.GameId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Discriminator", licence.UserId);
            return View(licence);
        }

        // POST: Licence/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int UserId, int GameId, [Bind("Name,Description,UserId,GameId")] Licence licence)
        {
            if (UserId != licence.UserId || GameId != licence.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(licence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LicenceExists(licence.UserId, licence.GameId))
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
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameId", licence.GameId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Discriminator", licence.UserId);
            return View(licence);
        }

        // GET: Licence/Delete/5
        public async Task<IActionResult> Delete(int? UserId, int? GameId)
        {
            if (UserId == null || GameId == null)
            {
                return NotFound();
            }

            var licence = await _context.Licences
                .Include(l => l.Game)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.UserId == UserId && m.GameId == GameId);
            if (licence == null)
            {
                return NotFound();
            }

            return View(licence);
        }

        // POST: Licence/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int UserId, int GameId)
        {
            var licence = await _context.Licences.FindAsync(UserId, GameId);
            if (licence != null)
            {
                _context.Licences.Remove(licence);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LicenceExists(int UserId, int GameId)
        {
            return _context.Licences.Any(e => e.UserId == UserId && e.GameId == GameId);
        }
    }
}
