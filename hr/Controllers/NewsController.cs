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
    public class NewsController : Controller
    {
        private readonly HRDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;
        public NewsController(HRDbContext context , IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;

        }

        // GET: News
        public async Task<IActionResult> Index()
        {

            var News = await _context.News.ToListAsync();
            return View(News);
            //return _context.events != null ? 
            //            View(await _context.News.ToListAsync()) :
            //            Problem("Entity set 'HRDbContext.News'  is null.");
        }


        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsViewModel model)
        {


            var fileSize = model.Image.Length / 1024;
            if (fileSize > 2000)
            {
                ViewBag.Massage = ViewBag.Massage + "الحجم الأقصى المسموح به  للملف  هو 2 ميقا بايت";

                return View(model);
            }

            string UniqueFileName = ProcessUploadedFile(model);

            News nw = new News
            {
                CreateDate = DateTime.Now,
                Description = model.Description,
                ImageUrl = UniqueFileName,
                Name = model.Name,
            };


            try
            {
                _context.Add(nw);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                return View(model);
            }



        }

        private string ProcessUploadedFile(NewsViewModel model)
        {
            string UniqueFileName = null;
            if (model.Image != null)
            {
                string UploadFolder = Path.Combine(hostEnvironment.WebRootPath, "img/News/");
                UniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(UploadFolder, UniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }

            return UniqueFileName;
        }


        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewsViewModel model )
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            var dis = _context.News.Where(u => u.Id == id).SingleOrDefault();

            if (dis == null)
            {
                return NotFound();

            }

            string oldImageUrl = dis.ImageUrl;

            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                string UploadFolder = Path.Combine(hostEnvironment.WebRootPath, "img/News/");
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
                    if (!NewsExists(model.Id))
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

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'HRDbContext.News'  is null.");
            }
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
          return (_context.News?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
