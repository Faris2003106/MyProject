using hr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Versioning;

namespace hr.Controllers
{

    public class CPAddUser : Controller
    {
        
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly HRDbContext db;
       
        public CPAddUser(HRDbContext db,UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        

        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync(); 
            return View(users);
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(RegisterViewModel model) 
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
                    return RedirectToAction("index", "home");
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
                    return RedirectToAction("index", "CPHome");
                }
                else 
                { 
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id) 
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new RegisterViewModel
            {
                Email = user.Email
            };

            return View(model); 
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                user.Email = model.Email;
                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);

        }
        public IActionResult Delete() 
        { 
            return View(); 
        }
    }
}
