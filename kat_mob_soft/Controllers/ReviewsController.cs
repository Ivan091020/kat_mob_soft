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
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviews(long appId)
        {
            var result = await _reviewService.GetReviewsByAppIdAsync(appId);
            if (result.StatusCode == kat_mob_soft.Domain.Enum.StatusCode.OK)
            {
                return Json(result.Data);
            }
            return Json(new { success = false, message = result.Description });
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] AddReviewViewModel model)
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

            var result = await _reviewService.CreateReviewAsync(model, userId);
            if (result.StatusCode == kat_mob_soft.Domain.Enum.StatusCode.OK)
            {
                return Json(new { success = true, data = result.Data, message = result.Description });
            }
            return Json(new { success = false, message = result.Description });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReview(long reviewId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
            {
                return Json(new { success = false, message = "Пользователь не авторизован" });
            }

            var result = await _reviewService.DeleteReviewAsync(reviewId, userId);
            if (result.StatusCode == kat_mob_soft.Domain.Enum.StatusCode.OK)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = result.Description });
        }
    }
}

