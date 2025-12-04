using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using kat_mob_soft.Service;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAppService _appService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IAppService appService, IWebHostEnvironment webHostEnvironment)
        {
            _appService = appService;
            _webHostEnvironment = webHostEnvironment;
        }

        // Проверка прав администратора
        private bool IsAdmin()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? User.FindFirst("Role")?.Value;
            return role?.ToLower() == "admin";
        }

        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                return Forbid();
            }
            return View();
        }

        [HttpGet]
        public IActionResult AddApp()
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            // Стандартные категории
            ViewBag.Categories = new List<string>
            {
                "Аркады",
                "Викторины",
                "Головоломки",
                "Гонки",
                "Казуальные",
                "Карточные",
                "Музыкальные",
                "Настольные",
                "Обучающие",
                "Приключения",
                "Ролевые",
                "Симуляторы",
                "Словесные",
                "Спортивные",
                "Стратегии",
                "Экшен",
                "Автомобили и Транспорт",
                "Бизнес",
                "Видеоплееры и Редактор",
                "Детям",
                "Еда и напитки",
                "Жилье и дом",
                "Здоровье и фитнес",
                "Инструменты"
            };

            return View(new AddAppViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddApp(AddAppViewModel model, Microsoft.AspNetCore.Http.IFormFile iconFile)
        {
            if (!IsAdmin())
            {
                return Forbid();
            }

            // Обработка цены из строки (если пришла как строка)
            if (Request.Form.ContainsKey("Price"))
            {
                var priceString = Request.Form["Price"].ToString();
                if (!string.IsNullOrEmpty(priceString))
                {
                    // Заменяем запятую на точку для правильного парсинга
                    priceString = priceString.Replace(",", ".");
                    if (decimal.TryParse(priceString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedPrice))
                    {
                        model.Price = parsedPrice;
                    }
                    else
                    {
                        ModelState.AddModelError("Price", "Неверный формат цены. Используйте точку как разделитель (например: 8.39)");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                // Стандартные категории
                ViewBag.Categories = new List<string>
                {
                    "Аркады",
                    "Викторины",
                    "Головоломки",
                    "Гонки",
                    "Казуальные",
                    "Карточные",
                    "Музыкальные",
                    "Настольные",
                    "Обучающие",
                    "Приключения",
                    "Ролевые",
                    "Симуляторы",
                    "Словесные",
                    "Спортивные",
                    "Стратегии",
                    "Экшен",
                    "Автомобили и Транспорт",
                    "Бизнес",
                    "Видеоплееры и Редактор",
                    "Детям",
                    "Еда и напитки",
                    "Жилье и дом",
                    "Здоровье и фитнес",
                    "Инструменты"
                };
                return View(model);
            }

            // Загрузка файла иконки (если передан)
            string iconFilePath = null;
            if (iconFile != null && iconFile.Length > 0)
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                var uploadsFolder = System.IO.Path.Combine(webRootPath, "images", "apps");
                if (!System.IO.Directory.Exists(uploadsFolder))
                {
                    System.IO.Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"app_{System.Guid.NewGuid()}{System.IO.Path.GetExtension(iconFile.FileName)}";
                var filePath = System.IO.Path.Combine(uploadsFolder, fileName);

                using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    await iconFile.CopyToAsync(stream);
                }

                iconFilePath = $"/images/apps/{fileName}";
            }

            var result = await _appService.CreateAppAsync(model, iconFilePath);

            if (result.StatusCode == Domain.Enum.StatusCode.OK)
            {
                TempData["SuccessMessage"] = "Приложение успешно добавлено!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Description);
            // Стандартные категории
            ViewBag.Categories = new List<string>
            {
                "Аркады",
                "Викторины",
                "Головоломки",
                "Гонки",
                "Казуальные",
                "Карточные",
                "Музыкальные",
                "Настольные",
                "Обучающие",
                "Приключения",
                "Ролевые",
                "Симуляторы",
                "Словесные",
                "Спортивные",
                "Стратегии",
                "Экшен",
                "Автомобили и Транспорт",
                "Бизнес",
                "Видеоплееры и Редактор",
                "Детям",
                "Еда и напитки",
                "Жилье и дом",
                "Здоровье и фитнес",
                "Инструменты"
            };
            return View(model);
        }
    }
}

