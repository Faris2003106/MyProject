using hr.Models;
using hr.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace hr.Controllers
{
    public class CPPublications : Controller
    {
        private readonly HRDbContext db;
        private readonly IWebHostEnvironment hostEnvironment;

        public CPPublications(HRDbContext db, IWebHostEnvironment hostEnvironment)
        {
            this.db = db;
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Index(string title)
        {
            var cr = db.Publications.ToList();
            if (!string.IsNullOrEmpty(title))
            {
                cr = cr.Where(e => e.Title.Contains(title)).ToList();
            }
            return View(cr);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(PublicationsViewModel model)
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
                ViewBag.Message = "يجب أن يكون حجم الصورة أقل من 1 ميقا بايت";
                return View(model);
            }
            string ImgName = string.Empty;

            string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/publications/");
            string uniqeName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
            string filePath = Path.Combine(ImgPath, uniqeName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Image.CopyTo(fileStream);
            }
            Publications publications = new Publications
            {
                Title = model.Title,
                CreateDate = model.CreateDate,
                ImageUrl = uniqeName,
                Department = model.Department,
                Description = model.Description,
                status = model.status
            };

            db.Publications.Add(publications);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ev = db.Publications.Find(id);

            if (ev == null)
            {
                return NotFound();
            }
            PublicationsViewModel publications = new PublicationsViewModel
            {
                Id = ev.Id,
                Title = ev.Title,
                CreateDate = ev.CreateDate,
                ImageUrl = ev.ImageUrl,
                Department = ev.Department,
                Description = ev.Description,        
                status = ev.status
            };
            return View(publications);
        }
        [HttpPost]
        public IActionResult Edit(PublicationsViewModel model)
        {
            string uniqeName = string.Empty;
            try
            {
                if (model.Image != null)
                {
                    if (model.ImageUrl == null)
                    {
                        string ImgName = string.Empty;

                        string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/publications/");
                        uniqeName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                        string filePath = Path.Combine(ImgPath, uniqeName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.Image.CopyTo(fileStream);
                        }
                    }
                    if (model.ImageUrl != null)
                    {
                        string path = Path.Combine(hostEnvironment.WebRootPath, "img/publications/", model.ImageUrl);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        string ImgName = string.Empty;

                        string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/publications/");
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
                Publications publications = new Publications
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                    CreateDate = model.CreateDate,
                    Department = model.Department,
                    ImageUrl = uniqeName,
                    status = model.status
                };

                db.Publications.Update(publications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(model);
            }


        }
        public IActionResult Delete(int id)
        {
            var dataToDelete = db.Publications.Find(id);
            if (dataToDelete != null)
            {
                db.Publications.Remove(dataToDelete);
                db.SaveChanges();
            }
            if (dataToDelete.ImageUrl != null)
            {
                string path = Path.Combine(hostEnvironment.WebRootPath, "img/publications/", dataToDelete.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("index");
        }
    }
}
