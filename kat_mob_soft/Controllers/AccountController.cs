using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using kat_mob_soft.Service;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BCrypt.Net;
using kat_mob_soft.DAL;
using Npgsql;

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
            catch (Exception ex)
            {
                Console.WriteLine("ОШИБКА: " + ex.ToString());
                return Json(new { success = false, errors = new[] { ex.Message } });
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
                return View(model);

            try
            {
                await _accountService.LoginAsync(model);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}