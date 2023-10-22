using hr.Migrations;
using hr.Models;
using hr.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace hr.Controllers
{
    public class CPEvents : Controller
    {


        private readonly HRDbContext db;
        private readonly IWebHostEnvironment hostEnvironment;

        public CPEvents(HRDbContext db, IWebHostEnvironment hostEnvironment)
        {
            this.db = db;
            this.hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(string title)
        {
            var cr = db.events.ToList();
            if (!string.IsNullOrEmpty(title))
            {
                cr = cr.Where(e => e.Name.Contains(title)).ToList();
            }
            return View(cr);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(eventsViewModel model)
        {
            string imgName;

            if (model.Image == null)
            {
                ViewBag.Message = "يجب رفع صورة ";
                return View(model);
            }

            long imgSize = model.Image.Length / 1024;
            if (imgSize > 1000)
            {
                ViewBag.Message = "يجب أن تكون حجم الصورة أقل من 1 ميقا بايت";
                return View(model);
            }

            string ImgName = string.Empty;

            string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/events/");
            string uniqeName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
            string filePath = Path.Combine(ImgPath, uniqeName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Image.CopyTo(fileStream);
            }


            events events = new events
            {
                CreateDate = model.CreateDate,
                description = model.description,
                ImageUrl = uniqeName,
                Name = model.Name,
                Organization = model.Organization,
                status = model.status
            };
            db.events.Add(events);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ev = db.events.Find(id);

            if (ev == null)
            {
                return NotFound();
            }
           
            eventsViewModel events = new eventsViewModel
            {
                Id = ev.Id,
                CreateDate = ev.CreateDate,
                description = ev.description,
                ImageUrl = ev.ImageUrl,
                Name = ev.Name,
                Organization = ev.Organization,
                status = ev.status
            };
            return View(events);
        }
        [HttpPost]
        public IActionResult Edit(eventsViewModel model)
        {
            string uniqeName = string.Empty;
            try
            {
                if (model.Image != null)
                {
                    if (model.ImageUrl == null)
                    {
                        string ImgName = string.Empty;

                        string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/events/");
                         uniqeName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                        string filePath = Path.Combine(ImgPath, uniqeName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.Image.CopyTo(fileStream);
                        }                        
                    }
                    if(model.ImageUrl != null)
                    {
                        string path = Path.Combine(hostEnvironment.WebRootPath, "img/events/", model.ImageUrl);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        string ImgName = string.Empty;

                        string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/events/");
                        uniqeName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                        string filePath = Path.Combine(ImgPath, uniqeName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.Image.CopyTo(fileStream);
                        }
                    }
                }
                else
                {
                    uniqeName = model.ImageUrl;
                }

                
                events events = new events
                {
                    Id = model.Id,
                    CreateDate = model.CreateDate,
                    description = model.description,
                    ImageUrl = uniqeName,
                    Name = model.Name,
                    Organization = model.Organization,
                    status = model.status
                };

                db.events.Update(events);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(model);
            }

           
        }
        public IActionResult delete(int id)
        {
            var c = db.events.Find(id);
            if (c == null)
            {
                return NotFound();
            }

            if (c.ImageUrl != null)
            {
                string path = Path.Combine(hostEnvironment.WebRootPath, "img/events/", c.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            db.events.Remove(c);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
