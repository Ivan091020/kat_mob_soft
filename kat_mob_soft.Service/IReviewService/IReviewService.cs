using System.Collections.Generic;
using System.Threading.Tasks;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Response;

namespace kat_mob_soft.Service
{
    public interface IReviewService
    {
        Task<BaseResponse<List<ReviewViewModel>>> GetReviewsByAppIdAsync(long appId);
        Task<BaseResponse<ReviewViewModel>> CreateReviewAsync(AddReviewViewModel model, long userId);
        Task<BaseResponse<bool>> DeleteReviewAsync(long reviewId, long userId);
    }
}

