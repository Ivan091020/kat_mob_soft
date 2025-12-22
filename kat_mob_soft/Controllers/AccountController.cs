using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using kat_mob_soft.Service;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BCrypt.Net;
using kat_mob_soft.DAL;
using Npgsql;
using FluentValidation;

namespace kat_mob_soft.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(kat_mob_soft.ViewModels.RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            try
            {
                // Преобразуем ViewModel из kat_mob_soft.ViewModels в Domain.ViewModels
                var domainModel = new Domain.ViewModels.RegisterViewModel
                {
                    Email = model.Email,
                    Password = model.Password,
                    Username = model.UserName // Преобразуем UserName -> Username
                };

                // Используем AccountService для регистрации (отправка письма с кодом)
                var generatedCode = await _accountService.RegisterAsync(domainModel);
                
                // Создаем ConfirmEmailViewModel с кодом
                var confirmEmailModel = new ConfirmEmailViewModel
                {
                    Email = model.Email,
                    Login = model.UserName,
                    GeneratedCode = generatedCode,
                    Password = model.Password,
                    PasswordConfirm = model.ConfirmPassword
                };

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Регистрация успешна. Проверьте почту для получения кода подтверждения.", data = confirmEmailModel });
                }

                return View("ConfirmEmail", confirmEmailModel);
            }
            catch (ValidationException ex)
            {
                Console.WriteLine("ОШИБКА ВАЛИДАЦИИ: " + ex.ToString());
                var errorMessages = ex.Errors.Select(e => e.ErrorMessage).ToList();
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, errors = errorMessages });
                }
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ОШИБКА: " + ex.ToString());
                var errorMessage = ex.Message;
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, errors = new[] { errorMessage } });
                }
                ModelState.AddModelError("", errorMessage);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Domain.ViewModels.LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, errors = errorMessages });
                }
                return View(model);
            }

            try
            {
                // Используем UserStorage через сервис (как в AccountService)
                var userStorage = HttpContext.RequestServices.GetRequiredService<kat_mob_soft.DAL.Interfaces.Storage.UserStorage>();
                var user = await userStorage.GetByEmailAsync(model.Email);
                
                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    var errorMsg = "Неверный email или пароль";
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, errors = new[] { errorMsg } });
                    }
                    ModelState.AddModelError("", errorMsg);
                    return View(model);
                }

                // Проверка подтверждения email
                if (!user.EmailConfirmed)
                {
                    var errorMsg = "Email не подтвержден. Пожалуйста, подтвердите email перед входом.";
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, errors = new[] { errorMsg } });
                    }
                    ModelState.AddModelError("", errorMsg);
                    return View(model);
                }

                // Обновляем время последнего входа
                user.LastLogin = DateTimeOffset.UtcNow;
                await userStorage.UpdateAsync(user);

                // Создаем claims для аутентификации
                var claims = new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Username ?? ""),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email ?? ""),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role ?? "user")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(24)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Логирование для отладки
                Console.WriteLine($"Пользователь {user.Username} (ID: {user.Id}) успешно аутентифицирован");
                Console.WriteLine($"Claims установлены: Name={user.Username}, Email={user.Email}, Role={user.Role}");

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Вход выполнен" });
                }

                return RedirectToAction("Index", "Home");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine("ОШИБКА ВАЛИДАЦИИ: " + ex.ToString());
                var errorMessages = ex.Errors.Select(e => e.ErrorMessage).ToList();
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, errors = errorMessages });
                }
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                var errorMsg = ex.Message;
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, errors = new[] { errorMsg } });
                }
                ModelState.AddModelError("", errorMsg);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Вы вышли" });
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Неверная ссылка подтверждения";
                return View();
            }

            try
            {
                var result = await _accountService.ConfirmEmailAsync(email, token);
                if (result)
                {
                    ViewBag.Success = "Email успешно подтвержден! Теперь вы можете войти в систему.";
                }
                else
                {
                    ViewBag.Error = "Неверный токен подтверждения или email уже подтвержден";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подтверждения email: {ex.Message}");
                ViewBag.Error = "Произошла ошибка при подтверждении email";
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.CodeConfirm) || string.IsNullOrEmpty(model.Email))
            {
                return Json(new { success = false, errors = new[] { "Код подтверждения и email обязательны" } });
            }

            try
            {
                var result = await _accountService.ConfirmEmailAsync(model.Email, model.CodeConfirm);
                if (result)
                {
                    return Json(new { success = true, message = "Email успешно подтвержден!" });
                }
                else
                {
                    return Json(new { success = false, errors = new[] { "Неверный код подтверждения" } });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подтверждения email: {ex.Message}");
                return Json(new { success = false, errors = new[] { "Произошла ошибка при подтверждении email" } });
            }
        }

        [HttpGet]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Profile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                return RedirectToAction("Login");
            }

            try
            {
                var profile = await _accountService.GetProfileAsync(userId);
                return View(profile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения профиля: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(Domain.ViewModels.UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToArray();
                    return Json(new { success = false, errors });
                }
                return RedirectToAction("Profile");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                return Json(new { success = false, message = "Пользователь не авторизован" });
            }

            try
            {
                var updatedProfile = await _accountService.UpdateProfileAsync(userId, model);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, data = updatedProfile, message = "Профиль успешно обновлен" });
                }
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления профиля: {ex.Message}");
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Ошибка при обновлении профиля" });
                }
                return RedirectToAction("Profile");
            }
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(Domain.ViewModels.ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToArray();
                    return Json(new { success = false, errors });
                }
                return RedirectToAction("Profile");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                return Json(new { success = false, message = "Пользователь не авторизован" });
            }

            try
            {
                var result = await _accountService.ChangePasswordAsync(userId, model);
                if (result)
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, message = "Пароль успешно изменен" });
                    }
                    return RedirectToAction("Profile");
                }
                else
                {
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Неверный текущий пароль" });
                    }
                    ModelState.AddModelError("", "Неверный текущий пароль");
                    return RedirectToAction("Profile");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка смены пароля: {ex.Message}");
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Ошибка при смене пароля" });
                }
                return RedirectToAction("Profile");
            }
        }
    }
}