using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Response;
using kat_mob_soft.Domain.Enum;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL;

namespace kat_mob_soft.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IBaseStorage<ReviewDb> _reviewStorage;
        private readonly AppCatalogDbContext _db;

        public ReviewService(
            IBaseStorage<ReviewDb> reviewStorage,
            AppCatalogDbContext db)
        {
            _reviewStorage = reviewStorage;
            _db = db;
        }

        public async Task<BaseResponse<List<ReviewViewModel>>> GetReviewsByAppIdAsync(long appId)
        {
            try
            {
                var reviewsDb = await _db.Reviews
                    .Include(r => r.App)
                    .Include(r => r.User)
                    .Where(r => r.AppId == appId && r.IsApproved)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();

                var result = reviewsDb.Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    AppId = r.AppId,
                    AppName = r.App?.Name ?? "Неизвестное приложение",
                    UserId = r.UserId,
                    UserName = r.User?.DisplayName ?? r.User?.Username ?? "Анонимный пользователь",
                    Title = r.Title,
                    Body = r.Body,
                    Rating = r.Rating,
                    IsApproved = r.IsApproved,
                    CreatedAt = r.CreatedAt
                }).ToList();

                return new BaseResponse<List<ReviewViewModel>>
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ReviewViewModel>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<ReviewViewModel>> CreateReviewAsync(AddReviewViewModel model, long userId)
        {
            try
            {
                // Проверка существования приложения
                var app = await _db.Apps.FindAsync(model.AppId);
                if (app == null)
                {
                    return new BaseResponse<ReviewViewModel>
                    {
                        Description = "Приложение не найдено",
                        StatusCode = StatusCode.NotFound
                    };
                }

                // Проверка, не оставлял ли пользователь уже отзыв на это приложение
                var existingReview = await _db.Reviews
                    .FirstOrDefaultAsync(r => r.AppId == model.AppId && r.UserId == userId);
                
                if (existingReview != null)
                {
                    return new BaseResponse<ReviewViewModel>
                    {
                        Description = "Вы уже оставили отзыв на это приложение",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                // Простая валидация данных
                if (string.IsNullOrWhiteSpace(model.Body) || model.Body.Length < 10)
                {
                    return new BaseResponse<ReviewViewModel>
                    {
                        Description = "Текст отзыва должен содержать минимум 10 символов",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                if (model.Rating < 1 || model.Rating > 5)
                {
                    return new BaseResponse<ReviewViewModel>
                    {
                        Description = "Рейтинг должен быть от 1 до 5",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                // Создание отзыва в БД
                var reviewDb = new ReviewDb
                {
                    AppId = model.AppId,
                    UserId = userId,
                    Title = model.Title,
                    Body = model.Body,
                    Rating = model.Rating,
                    IsApproved = true, // Сразу одобрен и виден всем
                    CreatedAt = DateTimeOffset.UtcNow
                };

                await _reviewStorage.CreateAsync(reviewDb);

                // Обновление среднего рейтинга приложения (пересчитываем после сохранения нового отзыва)
                var allReviews = await _db.Reviews
                    .Where(r => r.AppId == model.AppId && r.IsApproved)
                    .ToListAsync();
                
                if (allReviews.Count > 0)
                {
                    app.AverageRating = (decimal)allReviews.Average(r => r.Rating);
                    app.TotalReviews = allReviews.Count;
                    _db.Apps.Update(app);
                    await _db.SaveChangesAsync();
                }

                var result = new ReviewViewModel
                {
                    Id = reviewDb.Id,
                    AppId = reviewDb.AppId,
                    AppName = app.Name,
                    UserId = reviewDb.UserId,
                    UserName = "Вы",
                    Title = reviewDb.Title,
                    Body = reviewDb.Body,
                    Rating = reviewDb.Rating,
                    IsApproved = reviewDb.IsApproved,
                    CreatedAt = reviewDb.CreatedAt
                };

                return new BaseResponse<ReviewViewModel>
                {
                    Data = result,
                    StatusCode = StatusCode.OK,
                    Description = "Отзыв успешно добавлен"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ReviewViewModel>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteReviewAsync(long reviewId, long userId)
        {
            try
            {
                var review = await _db.Reviews.FindAsync(reviewId);
                if (review == null)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "Отзыв не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                // Проверка прав: только автор может удалить свой отзыв
                if (review.UserId != userId)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "У вас нет прав на удаление этого отзыва",
                        StatusCode = StatusCode.Forbidden
                    };
                }

                // Используем прямой доступ к БД, так как нужен long Id
                _db.Reviews.Remove(review);
                await _db.SaveChangesAsync();

                return new BaseResponse<bool>
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}

