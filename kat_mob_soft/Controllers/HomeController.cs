using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.ComponentModel.DataAnnotations;

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

    public class ContactMessageModel
    {
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Тема обязательна")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Сообщение обязательно")]
        [StringLength(1000, ErrorMessage = "Сообщение не должно превышать 1000 символов")]
        public string Message { get; set; }
        // TODO: временно отключено, пока Identity не настроен
        // После подключения Identity восстановить полноценный метод Register

    }
}
