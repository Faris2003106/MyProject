using hr.Models;
using hr.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace hr.Controllers
{
    public class CPOffers : Controller
    {
        private readonly HRDbContext db;
        private readonly IWebHostEnvironment hostEnvironment;

        public CPOffers(HRDbContext db, IWebHostEnvironment hostEnvironment)
        {
            this.db = db;
            this.hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(string title)
        {
            var cr = db.Discounts.ToList();
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
        public IActionResult Create(DiscountsViewModel model)
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

            string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/discounts/");
            string uniqeName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
            string filePath = Path.Combine(ImgPath, uniqeName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Image.CopyTo(fileStream);
            }
            Discounts discounts = new Discounts
            {
                Name = model.Name,
                description = model.description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ImageUrl = uniqeName,
                FacebookUrl = model.FacebookUrl,
                InsatgramUrl = model.InsatgramUrl,
                SnapchatUrl = model.SnapchatUrl,
                TwitterUrl = model.TwitterUrl,
                status = model.status
                
            };

            db.Discounts.Add(discounts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ev = db.Discounts.Find(id);

            if (ev == null)
            {
                return NotFound();
            }
            DiscountsViewModel discounts = new DiscountsViewModel
            {
                Name = ev.Name,
                description = ev.description,
                StartDate = ev.StartDate,
                EndDate = ev.EndDate,
                ImageUrl = ev.ImageUrl,
                FacebookUrl = ev.FacebookUrl,
                InsatgramUrl = ev.InsatgramUrl,
                SnapchatUrl = ev.SnapchatUrl,
                TwitterUrl = ev.TwitterUrl,
                status = ev.status

            };
            return View(discounts);
        }
        [HttpPost]
        public IActionResult Edit(DiscountsViewModel model)
        {
            string uniqeName = string.Empty;
            try
            {
                if (model.Image != null)
                {
                    if (model.ImageUrl == null)
                    {
                        string ImgName = string.Empty;

                        string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/discounts/");
                        uniqeName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                        string filePath = Path.Combine(ImgPath, uniqeName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.Image.CopyTo(fileStream);
                        }
                    }
                    if (model.ImageUrl != null)
                    {
                        string path = Path.Combine(hostEnvironment.WebRootPath, "img/discounts/", model.ImageUrl);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        string ImgName = string.Empty;

                        string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/discounts/");
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

                Discounts discounts = new Discounts
                {
                    Id= model.Id,
                    Name = model.Name,
                    description = model.description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    ImageUrl = uniqeName,
                    FacebookUrl = model.FacebookUrl,
                    InsatgramUrl = model.InsatgramUrl,
                    SnapchatUrl = model.SnapchatUrl,
                    TwitterUrl = model.TwitterUrl,
                    status = model.status

                };

                db.Discounts.Update(discounts);
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
            var dataToDelete = db.Discounts.Find(id);
            if (dataToDelete != null)
            {
                db.Discounts.Remove(dataToDelete);
                db.SaveChanges();
            }
            if (dataToDelete.ImageUrl != null)
            {
                string path = Path.Combine(hostEnvironment.WebRootPath, "img/discounts/", dataToDelete.ImageUrl);
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
