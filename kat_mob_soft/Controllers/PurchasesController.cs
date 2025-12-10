using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using kat_mob_soft.Service;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Controllers
{
    [Authorize]
    public class PurchasesController : Controller
    {
        private readonly IPurchaseService _purchaseService;

        public PurchasesController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _purchaseService.GetUserCartAsync(userId);
            if (result.StatusCode == kat_mob_soft.Domain.Enum.StatusCode.OK)
            {
                return View(result.Data);
            }
            return View(new CartViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> MyPurchases()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _purchaseService.GetUserPurchasesAsync(userId);
            if (result.StatusCode == kat_mob_soft.Domain.Enum.StatusCode.OK)
            {
                return View(result.Data);
            }
            return View(new System.Collections.Generic.List<PurchaseViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();
                return Json(new { success = false, errors });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                return Json(new { success = false, message = "Пользователь не авторизован" });
            }

            var result = await _purchaseService.AddToCartAsync(model.AppId, userId);
            if (result.StatusCode == kat_mob_soft.Domain.Enum.StatusCode.OK)
            {
                return Json(new { success = true, data = result.Data });
            }
            return Json(new { success = false, message = result.Description });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(long purchaseId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                return Json(new { success = false, message = "Пользователь не авторизован" });
            }

            var result = await _purchaseService.RemoveFromCartAsync(purchaseId, userId);
            if (result.StatusCode == kat_mob_soft.Domain.Enum.StatusCode.OK)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = result.Description });
        }

        [HttpPost]
        public async Task<IActionResult> CompletePurchase(long purchaseId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                return Json(new { success = false, message = "Пользователь не авторизован" });
            }

            var result = await _purchaseService.CompletePurchaseAsync(purchaseId, userId);
            if (result.StatusCode == kat_mob_soft.Domain.Enum.StatusCode.OK)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = result.Description });
        }
    }
}

