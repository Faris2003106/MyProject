using hr.Models;
using hr.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.IO;

namespace hr.Controllers
{
    //[Authorize]
    public class CPCourses : Controller
    {
        private readonly HRDbContext db;
        private readonly IWebHostEnvironment hostEnvironment;

        public CPCourses(HRDbContext db, IWebHostEnvironment hostEnvironment)
        {
            this.db = db;
            this.hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(string title)
        {
            var cr = db.Courses.ToList();
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
        public IActionResult Create(CoursesViewModel model) 
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

            string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/courses/");
            string uniqeName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
            string filePath = Path.Combine(ImgPath, uniqeName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Image.CopyTo(fileStream);
            }
            Courses courses = new Courses
            {
                CourseDate = model.CourseDate,
                OrganizationLogoUrl = uniqeName,
                RegisterLink = model.RegisterLink,
                Name = model.Name,
                description = model.description,
                Organization = model.Organization,
                status = model.status
            };

            db.Courses.Add(courses);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ev = db.Courses.Find(id);

            if (ev == null)
            {
                return NotFound();
            }
            CoursesViewModel courses = new CoursesViewModel
            {
                Id = ev.Id,
                CourseDate = ev.CourseDate,
                OrganizationLogoUrl = ev.OrganizationLogoUrl,
                RegisterLink = ev.RegisterLink,
                Name = ev.Name,
                description = ev.description,
                Organization = ev.Organization,
                status = ev.status
            };
            return View(courses);

        }
        [HttpPost]
        public IActionResult Edit(CoursesViewModel model)
        {
            string uniqeName = string.Empty;
            try
            {                
                if(model.Image != null)
                {
                    if(model.OrganizationLogoUrl == null)
                    {
                        string ImgName = string.Empty;
                        string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/courses/");
                        uniqeName = Guid.NewGuid().ToString()+ "_" + model.Image.FileName;
                        string filePath = Path.Combine(ImgPath, uniqeName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.Image.CopyTo(fileStream);
                        }
                    }
                    if(model.OrganizationLogoUrl != null)
                    {
                        string path = Path.Combine(hostEnvironment.WebRootPath, "img/courses/", model.OrganizationLogoUrl);
                        if(System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        string ImgName = string.Empty;

                        string ImgPath = Path.Combine(hostEnvironment.WebRootPath, "img/courses/");
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
                    uniqeName = model.OrganizationLogoUrl;
                }
                Courses courses = new Courses
                {
                    Id = model.Id,
                    Name = model.Name,
                    CourseDate = model.CourseDate,
                    Organization = model.Organization,
                    description = model.description,
                    OrganizationLogoUrl = uniqeName,
                    RegisterLink = model.RegisterLink,
                    status = model.status
                };
                db.Courses.Update(courses);
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
            var c = db.Courses.Find(id);
            if (c == null)
            {
                return NotFound();
            }

            if (c.OrganizationLogoUrl != null)
            {
                string path = Path.Combine(hostEnvironment.WebRootPath, "img/courses/", c.OrganizationLogoUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            db.Courses.Remove(c);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
