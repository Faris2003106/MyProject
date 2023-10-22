using hr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hr.Controllers
{
    //[Authorize]
    public class CPVideos : Controller
    {
        private readonly HRDbContext db;

        public CPVideos(HRDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index(string title)
        {
            var cr = db.Videos.ToList();
            if (!string.IsNullOrEmpty(title))
            {
                cr = cr.Where(e => e.VideoTitle.Contains(title)).ToList();
            }
            return View(cr);
        }
        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Videos model)
        {
            db.Videos.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var ev = db.Videos.Find(id);

            if (ev == null)
            {
                return NotFound();
            }
            return View(ev);
        }
        [HttpPost]
        public IActionResult Edit(Videos model)
        {
            if (ModelState.IsValid)
            {
                db.Videos.Update(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var c = db.Videos.Find(id);
            if (c == null)
            {
                return NotFound();
            }

            db.Videos.Remove(c);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
