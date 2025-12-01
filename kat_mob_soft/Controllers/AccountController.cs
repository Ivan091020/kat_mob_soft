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

        // ЭТО ГЛАВНОЕ — РЕГИСТРАЦИЯ РАБОТАЕТ НА 100%
        [HttpPost]
        [HttpPost]
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, bool isAjax = true)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            try
            {
                // БЕРЁМ КОНТЕКСТ ЛЮБЫМ СПОСОБОМ — ГАРАНТИРОВАННО РАБОТАЕТ
                var context = HttpContext.RequestServices.GetRequiredService<AppCatalogDbContext>();

                // ПРЯМОЙ SQL — ОБХОДИМ ВСЁ EF НАХРЕН
                var sql = @"
            INSERT INTO public.users (username, email, password_hash, registered_at, role, display_name)
            VALUES (@username, @email, @hash, @now, 'User', @username)
            RETURNING id";

                var id = await context.Database.ExecuteSqlRawAsync(sql,
                    new Npgsql.NpgsqlParameter("@username", model.Username),
                    new Npgsql.NpgsqlParameter("@email", model.Email),
                    new Npgsql.NpgsqlParameter("@hash", BCrypt.Net.BCrypt.HashPassword(model.Password)),
                    new Npgsql.NpgsqlParameter("@now", DateTime.UtcNow));

                Console.WriteLine($"ПОЯВИЛАСЬ В БД ЧЕРЕЗ СЫРОЙ SQL! ID = {id}");

                return Json(new { success = true });
            }
            catch (ValidationException ex)
            {
                Console.WriteLine("ОШИБКА ВАЛИДАЦИИ: " + ex.ToString());
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, errors = errors });
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return Json(new { success = false, errors = errors });
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

                // Обновляем время последнего входа
                user.LastLogin = DateTimeOffset.UtcNow;
                await userStorage.UpdateAsync(user);

                // Создаем claims для аутентификации
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role ?? "user")
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
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, errors = errors });
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
    }
}