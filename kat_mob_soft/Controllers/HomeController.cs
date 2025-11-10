using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

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

        // ------------------ SendMessage ------------------
        // Синхронный вариант: не требует System.Threading.Tasks и не будет предупреждения CS1998.
        // Временно отключил проверку antiforgery для удобства при отправке JSON через fetch.
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult SendMessage([FromBody] ContactMessageModel model)
        {
            if (!ModelState.IsValid)
            {
                // Для корректной работы SelectMany требуется using System.Linq; (он есть вверху файла)
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { success = false, message = string.Join("; ", errors) });
            }

            // TODO: Здесь можно сохранять в БД или отправлять письмо.
            return Json(new { success = true });
        }
        // -------------------------------------------------
    }

    // ------------------ Модель (в том же файле, т.к. у тебя нет папки Models) ------------------
    // Если позже добавишь папку Models, можно перенести этот класс туда.
    public class ContactMessageModel
    {
        // Можно добавить атрибуты валидации, если хочешь:
        // [Required], [EmailAddress], [StringLength(...)] и т.д.
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
