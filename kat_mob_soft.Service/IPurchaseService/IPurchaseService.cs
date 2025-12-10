using System.Collections.Generic;
using System.Threading.Tasks;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Response;

namespace kat_mob_soft.Service
{
    public interface IPurchaseService
    {
        Task<BaseResponse<CartViewModel>> GetUserCartAsync(long userId);
        Task<BaseResponse<PurchaseViewModel>> AddToCartAsync(long appId, long userId);
        Task<BaseResponse<bool>> RemoveFromCartAsync(long purchaseId, long userId);
        Task<BaseResponse<bool>> CompletePurchaseAsync(long purchaseId, long userId);
        Task<BaseResponse<List<PurchaseViewModel>>> GetUserPurchasesAsync(long userId);
    }
}

