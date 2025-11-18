using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using kat_mob_soft.Service;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: Account/Register - отображение формы регистрации
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register - обработка регистрации
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var profile = await _accountService.RegisterAsync(model);
                return RedirectToAction("Login", "Account");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Account/Login - отображение формы входа
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login - обработка входа
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var tokens = await _accountService.LoginAsync(model);
                // TODO: Сохранение токена в cookie или session
                return RedirectToAction("Index", "Home");
            }
            catch (UnauthorizedAccessException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Profile(long userId)
        {
            try
            {
                var profile = await _accountService.GetProfileAsync(userId);
                return Ok(profile);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(long userId, [FromBody] UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var profile = await _accountService.UpdateProfileAsync(userId, model);
                return Ok(profile);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(long userId, [FromBody] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _accountService.ChangePasswordAsync(userId, model);
                if (result)
                {
                    return Ok(new { message = "Пароль успешно изменен" });
                }
                return BadRequest(new { error = "Неверный старый пароль" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}

