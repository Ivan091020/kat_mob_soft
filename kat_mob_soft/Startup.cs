using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using kat_mob_soft.DAL;
using kat_mob_soft.DAL.Interfaces.Storage;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.Service;
using AutoMapper;

namespace kat_mob_soft.DAL
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<AppCatalogDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Регистрация AutoMapper
            services.AddAutoMapper(typeof(AppMappingProfile));

            // Регистрация Storage
            services.AddScoped<IBaseStorage<UserDb>, UserStorage>();

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
