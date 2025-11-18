using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kat_mob_soft.DAL
{
    public static class Initializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppCatalogDbContext>();

            try
            {
                // Применение миграций
                await context.Database.MigrateAsync();

                // Проверка, нужно ли инициализировать данные
                if (!context.Users.Any())
                {
                    // Здесь можно добавить начальные данные, если нужно
                    // Например, создать администратора по умолчанию
                }
            }
            catch (Exception)
            {
                // Ошибка при инициализации - пробрасываем дальше
                throw;
            }
        }
    }
}

