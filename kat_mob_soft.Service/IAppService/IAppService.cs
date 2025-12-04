using System.Collections.Generic;
using System.Threading.Tasks;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Response;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.Domain.Filter;

namespace kat_mob_soft.Service
{
    public interface IAppService
    {
        Task<BaseResponse<List<AppViewModel>>> GetAllAppsAsync();
        Task<BaseResponse<AppViewModel>> CreateAppAsync(AddAppViewModel model, string iconFilePath);
        BaseResponse<List<AppViewModel>> GetAppsByFilter(AppFilter filter);
        Task<BaseResponse<AppPageViewModel>> GetAppById(long id);
        BaseResponse<List<AppScreenshotViewModel>> GetScreenshotsByAppId(long id);
    }
}

