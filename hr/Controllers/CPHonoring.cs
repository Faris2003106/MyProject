using hr.Models;
using hr.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace hr.Controllers
{
    public class CPHonoring : Controller
    {
        private readonly HRDbContext db;
        private readonly IWebHostEnvironment hostEnvironment;

        public CPHonoring(HRDbContext db, IWebHostEnvironment hostEnvironment)
        {
            this.db = db;
            this.hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(string title)
        {
            var cr = db.Honoring.ToList();
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
        public IActionResult Create(HonoringViewModel model)
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

            string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/honoring/");
            string uniqeName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
            string filePath = Path.Combine(ImgPath, uniqeName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Image.CopyTo(fileStream);
            }
            Honoring honoring = new Honoring
            {
                Name = model.Name,
                CreateDate = model.CreateDate,
                Description = model.Description,
                ImageUrl = uniqeName,
                status = model.status
            };


            db.Honoring.Add(honoring);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ev = db.Honoring.Find(id);

            if (ev == null)
            {
                return NotFound();
            }
            HonoringViewModel honoring = new HonoringViewModel
            {
                Id = ev.Id,
                Name = ev.Name,
                CreateDate = ev.CreateDate,
                Description = ev.Description,
                ImageUrl = ev.ImageUrl,               
                status = ev.status
            };
            return View(honoring);
        }
        [HttpPost]
        public IActionResult Edit(HonoringViewModel model)
        {
            string uniqeName = string.Empty;
            try
            {
                if (model.Image != null)
                {
                    if (model.ImageUrl == null)
                    {
                        string ImgName = string.Empty;
                        string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/honoring/");
                        uniqeName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                        string filePath = Path.Combine(ImgPath, uniqeName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.Image.CopyTo(fileStream);
                        }
                    }
                    if (model.ImageUrl != null)
                    {
                        string path = Path.Combine(hostEnvironment.WebRootPath, "img/honoring/", model.ImageUrl);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        string ImgName = string.Empty;

                        string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/honoring/");
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
                Honoring honoring = new Honoring
                {
                    Id = model.Id,
                    CreateDate = model.CreateDate,
                    Name = model.Name,
                    Description = model.Description,
                    ImageUrl = uniqeName,
                    status = model.status
                };

                db.Honoring.Update(honoring);
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
            var dataToDelete = db.Honoring.Find(id);
            if (dataToDelete != null)
            {
                db.Honoring.Remove(dataToDelete);
                db.SaveChanges();
            }
            if(dataToDelete.ImageUrl != null)
            {
                string path = Path.Combine(hostEnvironment.WebRootPath, "img/honoring/", dataToDelete.ImageUrl);
                if(System.IO.File.Exists(path))
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
