using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using kat_mob_soft.Service;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Controllers
{
    public class AppsController : Controller
    {
        private readonly IAppService _appService;

        public AppsController(IAppService appService)
        {
            _appService = appService;
        }

        public async Task<IActionResult> ListOfApps()
        {
            var result = await _appService.GetAllAppsAsync();
            if (result.Data == null)
            {
                return View(new System.Collections.Generic.List<AppViewModel>());
            }
            return View(result.Data);
        }
    }
}

