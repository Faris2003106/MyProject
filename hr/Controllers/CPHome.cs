using hr.Models;
using Microsoft.AspNetCore.Mvc;

namespace hr.Controllers
{
    public class CPHome : Controller
    {
        private readonly HRDbContext db;
        public CPHome(HRDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            var cr = db.HomePage.ToList();
            return View(cr);
        }
        [HttpGet]
        public IActionResult AddStats()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddStats(HomePage model)
        {
            db.HomePage.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditStatic(int id)
        {
            var ev = db.HomePage.Find(id);
            if (ev == null)
            {
                return NotFound();
            }
            return View(ev);
        }
        [HttpPost]
        public IActionResult EditStatic(HomePage model)
        {
            if (ModelState.IsValid)
            {
                db.HomePage.Update(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var c = db.HomePage.Find(id);
            if(c == null)
            {
                return NotFound();
            }

            db.HomePage.Remove(c);  
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}