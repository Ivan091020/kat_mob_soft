using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using kat_mob_soft.DAL;

namespace kat_mob_soft.DAL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Тест соединения с PostgreSQL
            Console.WriteLine("=== Тестируем соединение с PostgreSQL ===");
            try
            {
                var connectionString = "Host=localhost;Port=5432;Database=kat_mob_soft;Username=postgres;Password=postgres";
                using var connection = new Npgsql.NpgsqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Соединение с PostgreSQL успешно!");
                
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT version()";
                var version = command.ExecuteScalar();
                Console.WriteLine($"Версия PostgreSQL: {version}");
                
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения к PostgreSQL: {ex.Message}");
            }
            Console.WriteLine("=== Тест соединения завершен ===");
            
            // Инициализация базы данных
            Console.WriteLine("=== Начинаем инициализацию базы данных ===");
            using (var scope = host.Services.CreateScope())
            {
                await Initializer.InitializeAsync(scope.ServiceProvider);
            }
            Console.WriteLine("=== Инициализация базы данных завершена ===");

            await host.RunAsync();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                   
                });
        
    }
}
