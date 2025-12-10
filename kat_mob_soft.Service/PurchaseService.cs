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
    public class PurchaseService : IPurchaseService
    {
        private readonly IBaseStorage<PurchaseDb> _purchaseStorage;
        private readonly AppCatalogDbContext _db;

        public PurchaseService(
            IBaseStorage<PurchaseDb> purchaseStorage,
            AppCatalogDbContext db)
        {
            _purchaseStorage = purchaseStorage;
            _db = db;
        }

        public async Task<BaseResponse<CartViewModel>> GetUserCartAsync(long userId)
        {
            try
            {
                var purchases = await _db.Purchases
                    .Include(p => p.App)
                        .ThenInclude(a => a.Icon)
                    .Include(p => p.User)
                    .Where(p => p.UserId == userId && p.Status == "pending")
                    .OrderByDescending(p => p.PurchasedAt)
                    .ToListAsync();

                var items = purchases.Select(p => new PurchaseViewModel
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = p.User?.DisplayName ?? p.User?.Username ?? "Пользователь",
                    AppId = p.AppId,
                    AppName = p.App?.Name ?? "Неизвестное приложение",
                    AppIconPath = p.App?.Icon?.FilePath ?? "/images/default-app.png",
                    PricePaid = p.PricePaid,
                    Currency = p.Currency ?? "USD",
                    Status = p.Status,
                    PurchasedAt = p.PurchasedAt
                }).ToList();

                var totalPrice = items.Sum(i => i.PricePaid);
                var currency = items.FirstOrDefault()?.Currency ?? "USD";

                var result = new CartViewModel
                {
                    Items = items,
                    TotalPrice = totalPrice,
                    Currency = currency
                };

                return new BaseResponse<CartViewModel>
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CartViewModel>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<PurchaseViewModel>> AddToCartAsync(long appId, long userId)
        {
            try
            {
                // Проверка существования приложения
                var app = await _db.Apps
                    .Include(a => a.Icon)
                    .FirstOrDefaultAsync(a => a.Id == appId);
                
                if (app == null)
                {
                    return new BaseResponse<PurchaseViewModel>
                    {
                        Description = "Приложение не найдено",
                        StatusCode = StatusCode.NotFound
                    };
                }

                // Проверка, не добавлено ли уже приложение в корзину
                var existingPurchase = await _db.Purchases
                    .FirstOrDefaultAsync(p => p.AppId == appId && p.UserId == userId && p.Status == "pending");
                
                if (existingPurchase != null)
                {
                    return new BaseResponse<PurchaseViewModel>
                    {
                        Description = "Приложение уже в корзине",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                // Проверка, не куплено ли уже приложение
                var completedPurchase = await _db.Purchases
                    .FirstOrDefaultAsync(p => p.AppId == appId && p.UserId == userId && p.Status == "completed");
                
                if (completedPurchase != null)
                {
                    return new BaseResponse<PurchaseViewModel>
                    {
                        Description = "Вы уже приобрели это приложение",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                // Простая валидация данных
                if (app.Price < 0)
                {
                    return new BaseResponse<PurchaseViewModel>
                    {
                        Description = "Цена не может быть отрицательной",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                // Создание покупки в корзине
                var purchaseDb = new PurchaseDb
                {
                    AppId = appId,
                    UserId = userId,
                    PricePaid = app.Price,
                    Currency = app.Currency ?? "USD",
                    Status = "pending",
                    PurchasedAt = DateTimeOffset.UtcNow
                };

                await _purchaseStorage.CreateAsync(purchaseDb);

                var result = new PurchaseViewModel
                {
                    Id = purchaseDb.Id,
                    UserId = purchaseDb.UserId,
                    UserName = "Вы",
                    AppId = purchaseDb.AppId,
                    AppName = app.Name,
                    AppIconPath = app.Icon?.FilePath ?? "/images/default-app.png",
                    PricePaid = purchaseDb.PricePaid,
                    Currency = purchaseDb.Currency,
                    Status = purchaseDb.Status,
                    PurchasedAt = purchaseDb.PurchasedAt
                };

                return new BaseResponse<PurchaseViewModel>
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<PurchaseViewModel>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<bool>> RemoveFromCartAsync(long purchaseId, long userId)
        {
            try
            {
                var purchase = await _db.Purchases.FindAsync(purchaseId);
                if (purchase == null)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "Покупка не найдена",
                        StatusCode = StatusCode.NotFound
                    };
                }

                // Проверка прав: только владелец может удалить из корзины
                if (purchase.UserId != userId)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "У вас нет прав на удаление этой покупки",
                        StatusCode = StatusCode.Forbidden
                    };
                }

                // Удаляем только если статус "pending"
                if (purchase.Status != "pending")
                {
                    return new BaseResponse<bool>
                    {
                        Description = "Нельзя удалить завершенную покупку",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                _db.Purchases.Remove(purchase);
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

        public async Task<BaseResponse<bool>> CompletePurchaseAsync(long purchaseId, long userId)
        {
            try
            {
                var purchase = await _db.Purchases
                    .Include(p => p.App)
                    .FirstOrDefaultAsync(p => p.Id == purchaseId);
                
                if (purchase == null)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "Покупка не найдена",
                        StatusCode = StatusCode.NotFound
                    };
                }

                // Проверка прав
                if (purchase.UserId != userId)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "У вас нет прав на завершение этой покупки",
                        StatusCode = StatusCode.Forbidden
                    };
                }

                // Проверка статуса
                if (purchase.Status != "pending")
                {
                    return new BaseResponse<bool>
                    {
                        Description = "Покупка уже завершена",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                // Завершаем покупку
                purchase.Status = "completed";
                purchase.PaymentProvider = "internal";
                purchase.PurchasedAt = DateTimeOffset.UtcNow;
                
                _db.Purchases.Update(purchase);
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

        public async Task<BaseResponse<List<PurchaseViewModel>>> GetUserPurchasesAsync(long userId)
        {
            try
            {
                var purchases = await _db.Purchases
                    .Include(p => p.App)
                        .ThenInclude(a => a.Icon)
                    .Include(p => p.User)
                    .Where(p => p.UserId == userId && p.Status == "completed")
                    .OrderByDescending(p => p.PurchasedAt)
                    .ToListAsync();

                var result = purchases.Select(p => new PurchaseViewModel
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = p.User?.DisplayName ?? p.User?.Username ?? "Пользователь",
                    AppId = p.AppId,
                    AppName = p.App?.Name ?? "Неизвестное приложение",
                    AppIconPath = p.App?.Icon?.FilePath ?? "/images/default-app.png",
                    PricePaid = p.PricePaid,
                    Currency = p.Currency ?? "USD",
                    Status = p.Status,
                    PurchasedAt = p.PurchasedAt
                }).ToList();

                return new BaseResponse<List<PurchaseViewModel>>
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<PurchaseViewModel>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}

