using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using kat_mob_soft.Service;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Filter;

namespace kat_mob_soft.Controllers
{
    public class AppsController : Controller
    {
        private readonly IAppService _appService;
        private readonly IMapper _mapper;

        public AppsController(IAppService appService, IMapper mapper)
        {
            _appService = appService;
            _mapper = mapper;
        }

        public async Task<IActionResult> ListOfApps()
        {
            var result = await _appService.GetAllAppsAsync();
            if (result.Data == null)
            {
                return View(new List<AppViewModel>());
            }
            return View(result.Data);
        }

        [HttpPost]
        public IActionResult Filter([FromBody] AppFilter filter)
        {
            var result = _appService.GetAppsByFilter(filter);
            return Json(result.Data);
        }

        public async Task<IActionResult> AppPage(long id)
        {
            var resultApp = await _appService.GetAppById(id);
            var resultScreenshots = _appService.GetScreenshotsByAppId(id);

            if (resultApp.Data == null)
            {
                return NotFound();
            }

            var appPageViewModel = resultApp.Data;
            appPageViewModel.Screenshots = resultScreenshots.Data ?? new List<AppScreenshotViewModel>();

            return View(appPageViewModel);
        }
    }
}

