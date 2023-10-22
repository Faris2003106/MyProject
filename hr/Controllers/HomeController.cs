using hr.Models;
using hr.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace hr.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HRDbContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public HomeController(ILogger<HomeController> logger,HRDbContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
    

    public IActionResult Index()
        {
            VMHome vmIndex = new VMHome();
            vmIndex.events = _context.events.Where(a => a.status == true).ToList();
            vmIndex.statics = _context.HomePage.Where(a => a.status == true).ToList();
            vmIndex.courses = _context.Courses.Where(a => a.status == true).ToList();
            vmIndex.honoring = _context.Honoring.Where(a => a.status == true).ToList();
            vmIndex.publications = _context.Publications.Where(a => a.status == true).ToList();
            vmIndex.discounts = _context.Discounts.Where(a => a.status == true).ToList();
            vmIndex.news = _context.News.Where(a => a.status == true).ToList();
            vmIndex.videos = _context.Videos.Where(a => a.status == true).ToList();
            return View(vmIndex);
        }
        public IActionResult Event()
        {
          
            var events =_context.events.ToList();
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
           
        }
        public IActionResult Training()
        {
            return View();
        }
        public IActionResult Discounts()
        {
            var a =_context.Discounts.ToList();

            return View(a);
        }

        public IActionResult News()
        {
            var News =_context.News.ToList();

            return View(News);
        }
        public IActionResult Pdetails(int id)
        {
            var News = _context.News.FirstOrDefault(n => n.Id == id);
            if(News == null)
            {
                return NotFound();
            }
            return View(News);
        }
        public IActionResult Courses()
        {
            var cor =_context.Courses.ToList();

            return View(cor);

        }
        public IActionResult Honoring()
        {
            var honorings = _context.Honoring.ToList();

            return View(honorings);
        }
       
        public IActionResult Register()
        {            

            return View();
        }
        public IActionResult Regulations()
        {           

            return View();
        }
        public IActionResult eventdetails(int id)
        {
            var eventDetail = _context.events.FirstOrDefault(e => e.Id == id);
            return View(eventDetail);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to IdentityUser
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "CPAddUser");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}