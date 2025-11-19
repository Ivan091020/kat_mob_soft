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
                Console.WriteLine("Initializer: Начинаем инициализацию базы данных...");
                // Проверка соединения с базой данных
                var canConnect = await context.Database.CanConnectAsync();
                Console.WriteLine($"Initializer: Соединение с базой данных: {(canConnect ? "успешно" : "не удалось")}");
                // Применение миграций
                Console.WriteLine("Initializer: Применяем миграции...");
                await context.Database.MigrateAsync();
                Console.WriteLine("Initializer: Миграции успешно применены");
                
                // Проверка существования таблиц
                var tables = context.Database.GetDbConnection();
                tables.Open();
                using var command = tables.CreateCommand();
                command.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'";
                using var reader = command.ExecuteReader();
                Console.WriteLine("Initializer: Таблицы в базе данных:");
                while (reader.Read())
                {
                    Console.WriteLine($"  - {reader.GetString(0)}");
                }
                tables.Close();
                
                // Проверка, нужно ли инициализировать данные
                if (!context.Users.Any())
                {
                    Console.WriteLine("Initializer: Таблица Users пуста");
                    // Здесь можно добавить начальные данные, если нужно
                    // Например, создать администратора по умолчанию
                }
                else
                {
                    var usersCount = await context.Users.CountAsync();
                    Console.WriteLine($"Initializer: В таблице Users {usersCount} записей");
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

