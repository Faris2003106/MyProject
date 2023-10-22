using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hr.Controllers
{
    [Authorize]
    public class ControlPanel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
