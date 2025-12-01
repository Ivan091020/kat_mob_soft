using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("SiteInformation");
        }

        public IActionResult Services()
        {
            return View("Services");
        }

        public IActionResult Contacts()
        {
            return View("Contacts");
        }

        // ------------------ SendMessage ------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage([FromBody] ContactMessageModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return BadRequest(new { success = false, errors });
            }

            return Json(new { success = true });
        }
        // -------------------------------------------------
    }
}
