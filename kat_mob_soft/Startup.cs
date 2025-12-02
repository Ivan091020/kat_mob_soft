using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using kat_mob_soft.DAL;
using kat_mob_soft.DAL.Interfaces.Storage;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.Service;
using AutoMapper;
using FluentValidation;
using kat_mob_soft.Domain.Validators;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Models;
using kat_mob_soft.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;


namespace kat_mob_soft.DAL
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<FluentValidationActionFilter>();
            });

            // Регистрация валидаторов FluentValidation для ViewModels
            services.AddScoped<IValidator<LoginViewModel>, LoginViewModelValidator>();
            services.AddScoped<IValidator<RegisterViewModel>, RegisterViewModelValidator>();
            services.AddScoped<IValidator<ChangePasswordViewModel>, ChangePasswordViewModelValidator>();
            services.AddScoped<IValidator<UpdateProfileViewModel>, UpdateProfileViewModelValidator>();
            services.AddScoped<IValidator<ContactMessageModel>, ContactMessageModelValidator>();

            // Регистрация валидаторов FluentValidation для доменных моделей
            services.AddScoped<IValidator<App>, AppValidator>();
            services.AddScoped<IValidator<Category>, CategoryValidator>();
            services.AddScoped<IValidator<Review>, ReviewValidator>();
            services.AddScoped<IValidator<Developer>, DeveloperValidator>();
            services.AddScoped<IValidator<Tag>, TagValidator>();
            services.AddScoped<IValidator<AppVersion>, AppVersionValidator>();
            services.AddScoped<IValidator<Report>, ReportValidator>();
            services.AddScoped<IValidator<AppIcon>, AppIconValidator>();
            services.AddScoped<IValidator<AppScreenshot>, AppScreenshotValidator>();
            services.AddScoped<IValidator<AppTag>, AppTagValidator>();
            services.AddScoped<IValidator<Purchase>, PurchaseValidator>();
            services.AddScoped<IValidator<User>, UserValidator>();

            services.AddDbContext<AppCatalogDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Настройка cookie-аутентификации
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/Login";
                    options.Cookie.Name = "AppCatalogAuth";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
                    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                    options.Cookie.Path = "/";
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                    options.SlidingExpiration = true;
                });

            // Регистрация AutoMapper
            services.AddAutoMapper(typeof(AppMappingProfile));

            // Регистрация MemoryCache для временного хранения данных регистрации
            services.AddMemoryCache();

            // Регистрация Storage
            services.AddScoped<IBaseStorage<UserDb>, UserStorage>();
            // Регистрация UserStorage напрямую для доступа из контроллеров
            services.AddScoped<UserStorage>();

            // Регистрация сервисов
            services.AddScoped<IAccountService, AccountService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)

        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
