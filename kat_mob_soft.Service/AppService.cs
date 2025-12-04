using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.Domain.Response;
using kat_mob_soft.Domain.Enum;
using kat_mob_soft.Domain.Filter;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.DAL;

namespace kat_mob_soft.Service
{
    public class AppService : IAppService
    {
        private readonly IBaseStorage<AppDb> _appStorage;
        private readonly AppCatalogDbContext _db;
        private readonly IBaseStorage<CategoryDb> _categoryStorage;
        private readonly IBaseStorage<DeveloperDb> _developerStorage;
        private readonly IBaseStorage<AppScreenshotDb> _screenshotStorage;

        public AppService(
            IBaseStorage<AppDb> appStorage,
            AppCatalogDbContext db,
            IBaseStorage<CategoryDb> categoryStorage,
            IBaseStorage<DeveloperDb> developerStorage,
            IBaseStorage<AppScreenshotDb> screenshotStorage)
        {
            _appStorage = appStorage;
            _db = db;
            _categoryStorage = categoryStorage;
            _developerStorage = developerStorage;
            _screenshotStorage = screenshotStorage;
        }

        public async Task<BaseResponse<List<AppViewModel>>> GetAllAppsAsync()
        {
            try
            {
                var appsDb = await _appStorage.GetAllAsync();
                var appsList = appsDb
                    .Where(a => a.IsPublished) // Только опубликованные приложения
                    .OrderBy(a => a.CreatedAt)
                    .ToList();

                // Ручной маппинг для избежания проблем с версиями AutoMapper
                var result = new List<AppViewModel>();
                foreach (var app in appsList)
                {
                    var viewModel = new AppViewModel
                    {
                        Id = app.Id,
                        Name = app.Name,
                        ShortDescription = app.ShortDescription ?? "",
                        PathImg = app.Icon != null ? app.Icon.FilePath : "/images/default-app.png",
                        CountDownload = app.Downloads != null ? app.Downloads.Count : 0,
                        CategoryName = app.Category != null ? app.Category.Name : "Без категории",
                        AverageRating = app.AverageRating,
                        Price = app.Price,
                        Currency = app.Currency ?? "USD"
                    };
                    result.Add(viewModel);
                }

                if (result.Count == 0)
                {
                    return new BaseResponse<List<AppViewModel>>
                    {
                        Description = "Найдено 0 элементов",
                        StatusCode = StatusCode.OK,
                        Data = result
                    };
                }

                return new BaseResponse<List<AppViewModel>>
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AppViewModel>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<AppViewModel>> CreateAppAsync(AddAppViewModel model, string iconFilePath)
        {
            try
            {
                // Генерация slug из названия
                var slug = GenerateSlug(model.Name);

                // Проверка уникальности slug
                var existingApp = await _db.Apps.FirstOrDefaultAsync(a => a.Slug == slug);
                if (existingApp != null)
                {
                    return new BaseResponse<AppViewModel>
                    {
                        Description = "Приложение с таким названием уже существует",
                        StatusCode = StatusCode.AppAlreadyExists
                    };
                }

                // Поиск или создание разработчика
                var developer = await _db.Developers.FirstOrDefaultAsync(d => d.Name == model.DeveloperName);
                if (developer == null)
                {
                    developer = new DeveloperDb
                    {
                        Name = model.DeveloperName,
                        CreatedAt = DateTimeOffset.UtcNow
                    };
                    await _developerStorage.CreateAsync(developer);
                }

                // Поиск или создание категории
                var category = await _db.Categories.FirstOrDefaultAsync(c => c.Name == model.CategoryName);
                if (category == null)
                {
                    var categorySlug = GenerateSlug(model.CategoryName);
                    category = new CategoryDb
                    {
                        Name = model.CategoryName,
                        Slug = categorySlug,
                        CreatedAt = DateTimeOffset.UtcNow
                    };
                    await _categoryStorage.CreateAsync(category);
                }

                // Создание приложения
                var app = new AppDb
                {
                    Name = model.Name,
                    Slug = slug,
                    ShortDescription = model.ShortDescription,
                    FullDescription = model.FullDescription,
                    Price = model.Price,
                    Currency = model.Currency,
                    CategoryId = category.Id,
                    DeveloperId = developer.Id,
                    IsPublished = model.IsPublished,
                    PublishedAt = model.IsPublished ? DateTimeOffset.UtcNow : null,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };

                await _appStorage.CreateAsync(app);

                // Сохранение пути к иконке, если он передан
                if (!string.IsNullOrEmpty(iconFilePath))
                {
                    var appIcon = new AppIconDb
                    {
                        AppId = app.Id,
                        FilePath = iconFilePath,
                        UploadedAt = DateTimeOffset.UtcNow
                    };
                    _db.AppIcons.Add(appIcon);
                    await _db.SaveChangesAsync();
                }

                // Маппинг в ViewModel
                var viewModel = new AppViewModel
                {
                    Id = app.Id,
                    Name = app.Name,
                    PathImg = iconFilePath ?? "/images/default-app.png",
                    CountDownload = 0
                };

                return new BaseResponse<AppViewModel>
                {
                    Data = viewModel,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AppViewModel>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public BaseResponse<List<AppViewModel>> GetAppsByFilter(AppFilter filter)
        {
            try
            {
                // Получаем все опубликованные приложения из БД
                var appsDb = _db.Apps
                    .Include(a => a.Category)
                    .Include(a => a.Developer)
                    .Include(a => a.Icon)
                    .Include(a => a.Downloads)
                    .Where(a => a.IsPublished)
                    .ToList();

                var appsFilter = appsDb;

                // Применяем фильтры
                if (filter != null && appsFilter != null)
                {
                    // Фильтр по цене
                    if (filter.PriceMax != 0 || filter.PriceMin != 0)
                    {
                        appsFilter = appsFilter
                            .Where(a => a.Price >= filter.PriceMin && a.Price <= filter.PriceMax)
                            .ToList();
                    }

                    // Фильтр по категориям
                    if (filter.Categories != null && filter.Categories.Count > 0)
                    {
                        appsFilter = appsFilter
                            .Where(a => a.Category != null && filter.Categories.Contains(a.Category.Name))
                            .ToList();
                    }
                }

                // Маппинг в ViewModel
                var result = new List<AppViewModel>();
                foreach (var app in appsFilter)
                {
                    var viewModel = new AppViewModel
                    {
                        Id = app.Id,
                        Name = app.Name,
                        ShortDescription = app.ShortDescription ?? "",
                        PathImg = app.Icon != null ? app.Icon.FilePath : "/images/default-app.png",
                        CountDownload = app.Downloads != null ? app.Downloads.Count : 0,
                        CategoryName = app.Category != null ? app.Category.Name : "Без категории",
                        AverageRating = app.AverageRating,
                        Price = app.Price,
                        Currency = app.Currency ?? "USD"
                    };
                    result.Add(viewModel);
                }

                return new BaseResponse<List<AppViewModel>>
                {
                    Data = result,
                    Description = "Отфильтрованные данные",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AppViewModel>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private string GenerateSlug(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            // Транслитерация кириллицы в латиницу
            var transliteration = new Dictionary<char, string>
            {
                {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"},
                {'е', "e"}, {'ё', "yo"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"},
                {'й', "y"}, {'к', "k"}, {'л', "l"}, {'м', "m"}, {'н', "n"},
                {'о', "o"}, {'п', "p"}, {'р', "r"}, {'с', "s"}, {'т', "t"},
                {'у', "u"}, {'ф', "f"}, {'х', "h"}, {'ц', "ts"}, {'ч', "ch"},
                {'ш', "sh"}, {'щ', "sch"}, {'ъ', ""}, {'ы', "y"}, {'ь', ""},
                {'э', "e"}, {'ю', "yu"}, {'я', "ya"}
            };

            var slug = name.ToLower();
            var sb = new StringBuilder();

            foreach (char c in slug)
            {
                if (transliteration.ContainsKey(c))
                {
                    sb.Append(transliteration[c]);
                }
                else if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                }
                else if (c == ' ' || c == '-')
                {
                    sb.Append('-');
                }
            }

            // Удаляем множественные дефисы
            var result = Regex.Replace(sb.ToString(), @"-+", "-");
            // Удаляем дефисы в начале и конце
            result = result.Trim('-');

            return result;
        }

        public async Task<BaseResponse<AppPageViewModel>> GetAppById(long id)
        {
            try
            {
                var appDb = await _db.Apps
                    .Include(a => a.Category)
                    .Include(a => a.Developer)
                    .Include(a => a.Icon)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (appDb == null)
                {
                    return new BaseResponse<AppPageViewModel>()
                    {
                        Description = "Найдено 0 элементов",
                        StatusCode = StatusCode.OK
                    };
                }

                var result = new AppPageViewModel
                {
                    Id = appDb.Id,
                    Name = appDb.Name,
                    DeveloperName = appDb.Developer?.Name ?? "Неизвестный разработчик",
                    CategoryName = appDb.Category?.Name ?? "Без категории",
                    ShortDescription = appDb.ShortDescription ?? "",
                    FullDescription = appDb.FullDescription ?? "",
                    Price = appDb.Price,
                    Currency = appDb.Currency ?? "USD",
                    AverageRating = appDb.AverageRating,
                    PathImg = appDb.Icon?.FilePath ?? "/images/default-app.png",
                    Screenshots = new List<AppScreenshotViewModel>()
                };

                return new BaseResponse<AppPageViewModel>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AppPageViewModel>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public BaseResponse<List<AppScreenshotViewModel>> GetScreenshotsByAppId(long id)
        {
            try
            {
                var screenshotsDb = _db.AppScreenshots
                    .Where(x => x.AppId == id)
                    .OrderBy(x => x.SortOrder)
                    .ToList();

                var result = screenshotsDb.Select(s => new AppScreenshotViewModel
                {
                    Id = s.Id,
                    PathImg = s.FilePath,
                    Caption = s.Caption ?? ""
                }).ToList();

                if (result.Count == 0)
                {
                    return new BaseResponse<List<AppScreenshotViewModel>>()
                    {
                        Description = "Найдено 0 элементов",
                        StatusCode = StatusCode.OK,
                        Data = result
                    };
                }

                return new BaseResponse<List<AppScreenshotViewModel>>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AppScreenshotViewModel>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

    }
}

