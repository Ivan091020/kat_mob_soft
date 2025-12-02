using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Models;
using kat_mob_soft.Service;

namespace kat_mob_soft.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountService _accountService;
        private readonly IWebHostEnvironment _appEnvironment;

        public HomeController(ILogger<HomeController> logger, IAccountService accountService, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _accountService = accountService;
            _appEnvironment = appEnvironment;
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

        // ------------------ Google Authentication ------------------
        public async Task AuthenticationGoogle(string returnUrl = "/")
        {
            // По умолчанию возвращаемся на главную
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse", new { returnUrl }), // Передаем returnUrl
                Parameters = { { "prompt", "select_account" } }
            });
        }

        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
                if (result?.Succeeded == true)
                {
                    var picturePath = await SaveImageInImageUser(result.Principal.FindFirst("picture")?.Value, result);
                    User model = new User
                    {
                        Login = result.Principal.FindFirst(ClaimTypes.Name)?.Value,
                        Email = result.Principal.FindFirst(ClaimTypes.Email)?.Value,
                        FullName = result.Principal.FindFirst(ClaimTypes.Name)?.Value,
                        PathImage = !string.IsNullOrEmpty(picturePath) ? picturePath : "/images/user.png"
                    };

                    var response = await _accountService.IsCreatedAccount(model);
                    if (response.StatusCode == kat_mob_soft.Domain.Enum.StatusCode.OK)
                    {
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(response.Data));
                        return Redirect(returnUrl);
                    }
                }
                return BadRequest("Аутентификация не удалась.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private async Task<string> SaveImageInImageUser(string imageUrl, AuthenticateResult result)
        {
            string filePath = "";
            if (!string.IsNullOrEmpty(imageUrl))
            {
                using (var httpClient = new HttpClient())
                {
                    var fileName = $"{result.Principal.FindFirst(ClaimTypes.Email)?.Value}-avatar.jpg";
                    var relativePath = Path.Combine("ImageUser", fileName);
                    var fullPath = Path.Combine(_appEnvironment.WebRootPath, relativePath);
                    
                    // Создаем директорию, если её нет
                    var directory = Path.GetDirectoryName(fullPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    
                    var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
                    await System.IO.File.WriteAllBytesAsync(fullPath, imageBytes);
                    
                    // Возвращаем относительный путь с "/" в начале
                    filePath = "/" + relativePath.Replace("\\", "/");
                }
            }
            return filePath;
        }
        // -------------------------------------------------
    }
}
