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

        public async Task<IActionResult> ListOfApps(string q, int page = 1, int pageSize = 12)
        {
            // Создаем фильтр
            var filter = new AppFilter
            {
                SearchQuery = q,
                Page = page,
                PageSize = pageSize
            };
            
            // Получаем отфильтрованные данные с пагинацией
            var result = _appService.GetAppsByFilter(filter);
            
            // Для подсчета общего количества получаем все приложения без пагинации
            var allApps = await _appService.GetAllAppsAsync();
            var allAppsList = allApps.Data ?? new List<AppViewModel>();
            
            // Применяем поиск для подсчета общего количества
            int totalCount;
            if (!string.IsNullOrWhiteSpace(q))
            {
                var searchLower = q.ToLower();
                totalCount = allAppsList.Count(a => 
                    (a.Name != null && a.Name.ToLower().Contains(searchLower)) ||
                    (a.ShortDescription != null && a.ShortDescription.ToLower().Contains(searchLower)) ||
                    (a.CategoryName != null && a.CategoryName.ToLower().Contains(searchLower))
                );
            }
            else
            {
                totalCount = allAppsList.Count;
            }
            
            var pagedResult = new PagedResult<AppViewModel>
            {
                Items = result.Data ?? new List<AppViewModel>(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
            
            ViewBag.SearchQuery = q;
            ViewBag.PagedResult = pagedResult;
            return View(result.Data ?? new List<AppViewModel>());
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

