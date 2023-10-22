using Microsoft.AspNetCore.Mvc;
using hr.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using hr.ViewModel;
using Microsoft.Extensions.Hosting;

namespace hr.Controllers
{
    public class HonoringsController : Controller
    {
        private readonly HRDbContext _context;

        public HonoringsController(HRDbContext context)
        {
            _context = context;
        }

        // GET: Honorings
        public async Task<IActionResult> Index()
        {
              return _context.Honoring != null ? 
                          View(await _context.Honoring.ToListAsync()) :
                          Problem("Entity set 'HRDbContext.Honoring'  is null.");
        }

        // GET: Honorings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Honoring == null)
            {
                return NotFound();
            }

            var honoring = await _context.Honoring
                .FirstOrDefaultAsync(m => m.Id == id);
            if (honoring == null)
            {
                return NotFound();
            }

            return View(honoring);
        }

        // GET: Honorings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Honorings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreateDate")] Honoring honoring)
        {
            if (ModelState.IsValid)
            {
                _context.Add(honoring);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(honoring);
        }

        // GET: Honorings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Honoring == null)
            {
                return NotFound();
            }

            var honoring = await _context.Honoring.FindAsync(id);
            if (honoring == null)
            {
                return NotFound();
            }
            return View(honoring);
        }

        // POST: Honorings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreateDate")] Honoring honoring)
        {
            if (id != honoring.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(honoring);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HonoringExists(honoring.Id))
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
            return View(honoring);
        }

        // GET: Honorings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Honoring == null)
            {
                return NotFound();
            }

            var honoring = await _context.Honoring
                .FirstOrDefaultAsync(m => m.Id == id);
            if (honoring == null)
            {
                return NotFound();
            }

            return View(honoring);
        }

        // POST: Honorings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Honoring == null)
            {
                return Problem("Entity set 'HRDbContext.Honoring'  is null.");
            }
            var honoring = await _context.Honoring.FindAsync(id);
            if (honoring != null)
            {
                _context.Honoring.Remove(honoring);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HonoringExists(int id)
        {
          return (_context.Honoring?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
