using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hr.Models;
using hr.ViewModel;
using Microsoft.Extensions.Hosting;

namespace hr.Controllers
{
    public class eventsController : Controller
    {
        private readonly HRDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public eventsController(HRDbContext context , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        // GET: events
        public async Task<IActionResult> Index()
        {
            
            var events =await _context.events.ToListAsync();
            return View(events);
              //return _context.events != null ? 
              //            View(await _context.events.ToListAsync()) :
              //            Problem("Entity set 'HRDbContext.events'  is null.");
        }

        // GET: events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.events == null)
            {
                return NotFound();
            }

            var events = await _context.events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        // GET: events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( eventsViewModel model)
        {


            var fileSize = model.Image.Length / 1024;
            if (fileSize > 2000)
            {
                ViewBag.Massage = ViewBag.Massage + "الحجم الأقصى المسموح به  للملف  هو 2 ميقا بايت";
               
                return View(model);
            }

            string UniqueFileName = ProcessUploadedFile(model);

            events ev = new events
            {
                CreateDate = DateTime.Now,
                description = model.description,
                ImageUrl = UniqueFileName,
                Name = model.Name,
                
            };


            try
            {
                _context.Add(ev);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View(model);
            }



        }

        private string ProcessUploadedFile(eventsViewModel model)
        {
            string UniqueFileName = null;
            if (model.Image != null)
            {
                string UploadFolder = Path.Combine(hostEnvironment.WebRootPath, "img/events/");
                UniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(UploadFolder, UniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }

            return UniqueFileName;
        }

        // GET: events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.events == null)
            {
                return NotFound();
            }

            var events = await _context.events.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        // POST: events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, eventsViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            var dis = _context.Discounts.Where(u => u.Id == id).SingleOrDefault();

            if (dis == null)
            {
                return NotFound();

            }

            string oldImageUrl = dis.ImageUrl;

            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                string UploadFolder = Path.Combine(hostEnvironment.WebRootPath, "img/events/");
                string filePath = Path.Combine(UploadFolder, oldImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!eventsExists(model.Id))
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
            return View(model);
        }

        // GET: events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.events == null)
            {
                return NotFound();
            }

            var events = await _context.events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        // POST: events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.events == null)
            {
                return Problem("Entity set 'HRDbContext.events'  is null.");
            }
            var events = await _context.events.FindAsync(id);
            if (events != null)
            {
                _context.events.Remove(events);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool eventsExists(int id)
        {
          return (_context.events?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
