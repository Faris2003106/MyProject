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
    public class DiscountsController : Controller
    {
        private readonly HRDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;


        public DiscountsController(HRDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;

        }

        // GET: Discounts
        public async Task<IActionResult> Index()
        {

            var Discounts = await _context.Discounts.ToListAsync();
            return View(Discounts);

            //  return _context.Discounts != null ? 
            //      View(await _context.Discounts.ToListAsync()) :
            //   Problem("Entity set 'HRDbContext.Discounts'  is null.");
        }

        // GET: Discounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Discounts == null)
            {
                return NotFound();
            }

            var discounts = await _context.Discounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discounts == null)
            {
                return NotFound();
            }

            return View(discounts);
        }

        // GET: Discounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Discounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscountsViewModel model)
        {


            var fileSize = model.Image.Length / 1024;
            if (fileSize > 2000)
            {
                ViewBag.Massage = ViewBag.Massage + "الحجم الأقصى المسموح به  للملف  هو 2 ميقا بايت";

                return View(model);
            }

            string UniqueFileName = ProcessUploadedFile(model);

            Discounts d = new Discounts
            {
                description = model.description,
                ImageUrl = UniqueFileName,
                Name = model.Name,
            };


            try
            {
                _context.Add(d);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View(model);
            }



        }

        private string ProcessUploadedFile(DiscountsViewModel model)
        {
            string UniqueFileName = null;
            if (model.Image != null)
            {
                string UploadFolder = Path.Combine(hostEnvironment.WebRootPath, "img/discounts/");
                UniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(UploadFolder, UniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }

            return UniqueFileName;
        }



        // POST: Discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DiscountsViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var dis = _context.Discounts.Where(u => u.Id == id).SingleOrDefault();

            if (dis == null )
            {
                return NotFound();

            }

            string oldImageUrl = dis.ImageUrl;

            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                string UploadFolder = Path.Combine(hostEnvironment.WebRootPath, "img/discounts/");
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
                    if (!DiscountsExists(model.Id))
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

        // GET: Discounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Discounts == null)
            {
                return NotFound();
            }

            var discounts = await _context.Discounts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discounts == null)
            {
                return NotFound();
            }

            return View(discounts);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Discounts == null)
            {
                return Problem("Entity set 'HRDbContext.Discounts'  is null.");
            }
            var discounts = await _context.Discounts.FindAsync(id);
            if (discounts != null)
            {
                _context.Discounts.Remove(discounts);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountsExists(int id)
        {
          return (_context.Discounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
