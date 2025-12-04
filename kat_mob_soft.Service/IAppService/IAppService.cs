using System.Collections.Generic;
using System.Threading.Tasks;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Response;
using kat_mob_soft.Domain.Models.Db;

namespace kat_mob_soft.Service
{
    public interface IAppService
    {
        Task<BaseResponse<List<AppViewModel>>> GetAllAppsAsync();
        Task<BaseResponse<AppViewModel>> CreateAppAsync(AddAppViewModel model, string iconFilePath);
    }
}

