using hr.Models;
using hr.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace hr.Controllers
{
    public class CPNews : Controller
    {
        private readonly HRDbContext db;
        private readonly IWebHostEnvironment hostEnvironment;

        public CPNews(HRDbContext db, IWebHostEnvironment hostEnvironment)
        {
            this.db = db;
            this.hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(string title)
        {
            var cr = db.News.ToList();
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
        public IActionResult Create(NewsViewModel model)
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

            string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/News/");
            string uniqeName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
            string filePath = Path.Combine(ImgPath, uniqeName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Image.CopyTo(fileStream);
            }
            News news = new News
            {
                Name = model.Name,
                CreateDate = model.CreateDate,
                Description = model.Description,
                ImageUrl = uniqeName,
                NewsUrl = model.NewsUrl,
                status = model.status
            } ;

            db.News.Add(news);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ev = db.News.Find(id);

            if (ev == null)
            {
                return NotFound();
            }
            NewsViewModel news = new NewsViewModel
            {
                Id = ev.Id,
                Name = ev.Name,
                CreateDate = ev.CreateDate,
                Description = ev.Description,
                ImageUrl = ev.ImageUrl,
                NewsUrl = ev.NewsUrl,
                status = ev.status
            };
            return View(news);
        }
        [HttpPost]
        public IActionResult Edit(NewsViewModel model)
        {
            string uniqeName = string.Empty;
            try
            {
                if (model.Image != null)
                {
                    if (model.ImageUrl == null)
                    {
                        string ImgName = string.Empty;

                        string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/news/");
                        uniqeName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                        string filePath = Path.Combine(ImgPath, uniqeName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.Image.CopyTo(fileStream);
                        }
                    }
                    if (model.ImageUrl != null)
                    {
                        string path = Path.Combine(hostEnvironment.WebRootPath, "img/news/", model.ImageUrl);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        string ImgName = string.Empty;

                        string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/news/");
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
                News news = new News
                {
                    Id = model.Id,
                    Name = model.Name,
                    CreateDate = model.CreateDate,
                    Description = model.Description,
                    ImageUrl = uniqeName,
                    NewsUrl = model.NewsUrl,
                    status = model.status
                };

                db.News.Update(news);
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
            var dataToDelete = db.News.Find(id);
            if (dataToDelete != null)
            {
                db.News.Remove(dataToDelete);
                db.SaveChanges();
            }
            if (dataToDelete.ImageUrl != null)
            {
                string path = Path.Combine(hostEnvironment.WebRootPath, "img/news/", dataToDelete.ImageUrl);
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
