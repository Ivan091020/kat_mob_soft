using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using AutoMapper;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.Domain.Models;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL.Interfaces.Storage;
using FluentValidation;
using kat_mob_soft.Domain.Validators;
using kat_mob_soft.Domain.Response;
using kat_mob_soft.Domain.Enum;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using MailKit.Net.Smtp;
using MimeKit;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace kat_mob_soft.Service
{
    public class AccountService : IAccountService
    {
        private readonly IBaseStorage<UserDb> _userStorage;
        private readonly UserStorage _userStorageTyped;
        private readonly IMapper _mapper;
        private readonly IValidator<LoginViewModel> _loginValidator;
        private readonly IValidator<RegisterViewModel> _registerValidator;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        // Класс для временного хранения данных регистрации
        private class PendingRegistration
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string PasswordHash { get; set; }
            public string ConfirmationCode { get; set; }
        }

        public AccountService(
            IBaseStorage<UserDb> userStorage, 
            IMapper mapper,
            IValidator<LoginViewModel> loginValidator,
            IValidator<RegisterViewModel> registerValidator,
            IConfiguration configuration,
            IMemoryCache memoryCache)
        {
            _userStorage = userStorage;
            _userStorageTyped = userStorage as UserStorage;
            _mapper = mapper;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        public async Task<string> RegisterAsync(RegisterViewModel model) // Возвращает код подтверждения
        {
            Console.WriteLine($"AccountService: Начинаем регистрацию пользователя {model.Email}");
            
            // Валидация с помощью FluentValidation
            var validationResult = await _registerValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(validationResult.Errors);
            }
            
            // Проверка существования пользователя в БД
            if (_userStorageTyped != null)
            {
                Console.WriteLine("AccountService: Проверяем существование пользователя в БД...");
                var existingUser = await _userStorageTyped.GetByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    Console.WriteLine("AccountService: Пользователь уже существует в БД");
                    throw new InvalidOperationException("Пользователь с таким email уже существует");
                }
            }

            // Проверка существования пользователя в кэше (ожидающие подтверждения)
            var cacheKey = $"pending_registration_{model.Email.ToLowerInvariant()}";
            if (_memoryCache.TryGetValue(cacheKey, out _))
            {
                Console.WriteLine("AccountService: Регистрация уже ожидает подтверждения");
                throw new InvalidOperationException("Регистрация уже ожидает подтверждения. Проверьте почту.");
            }

            // Генерация кода подтверждения (6 цифр)
            var confirmationCode = GenerateConfirmationToken();
            
            // Сохраняем данные регистрации во временный кэш (НЕ в БД!)
            var pendingRegistration = new PendingRegistration
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                ConfirmationCode = confirmationCode
            };

            // Сохраняем в кэш на 15 минут
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };
            _memoryCache.Set(cacheKey, pendingRegistration, cacheOptions);
            
            Console.WriteLine("AccountService: Данные регистрации сохранены во временный кэш (НЕ в БД)");

            // Отправляем письмо с кодом подтверждения
            await SendConfirmationEmailAsync(model.Email, confirmationCode, model.Username);

            Console.WriteLine("AccountService: Письмо с кодом подтверждения отправлено");

            // Возвращаем код подтверждения для передачи в контроллер
            return confirmationCode;
        }

        public async Task<TokenViewModel> LoginAsync(LoginViewModel model)
        {
            // Валидация с помощью FluentValidation
            var validationResult = await _loginValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(validationResult.Errors);
            }
            
            if (_userStorageTyped == null)
                throw new InvalidOperationException("UserStorage не инициализирован");

            var user = await _userStorageTyped.GetByEmailAsync(model.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Неверный email или пароль");

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Неверный email или пароль");

            // Обновление времени последнего входа
            user.LastLogin = DateTimeOffset.UtcNow;
            await _userStorage.UpdateAsync(user);

            // TODO: Генерация JWT токена
            // Пока возвращаем заглушку
            return new TokenViewModel
            {
                Token = "temp_token_" + user.Id,
                RefreshToken = "temp_refresh_token_" + user.Id
            };
        }

        public async Task<ProfileViewModel> GetProfileAsync(long userId)
        {
            if (_userStorageTyped == null)
                throw new InvalidOperationException("UserStorage не инициализирован");

            var user = await _userStorageTyped.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");

            // Создаем ProfileViewModel вручную (без AutoMapper)
            return new ProfileViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                DisplayName = user.DisplayName,
                FullName = user.DisplayName ?? user.Username,
                AvatarPath = user.AvatarPath
            };
        }

        public async Task<ProfileViewModel> UpdateProfileAsync(long userId, UpdateProfileViewModel model)
        {
            if (_userStorageTyped == null)
                throw new InvalidOperationException("UserStorage не инициализирован");

            var user = await _userStorageTyped.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");

            // Обновление полей
            if (!string.IsNullOrEmpty(model.Email))
                user.Email = model.Email;
            if (!string.IsNullOrEmpty(model.DisplayName))
                user.DisplayName = model.DisplayName;
            if (!string.IsNullOrEmpty(model.FullName))
                user.DisplayName = model.FullName;

            await _userStorage.UpdateAsync(user);

            // Создаем ProfileViewModel вручную (без AutoMapper)
            return new ProfileViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                DisplayName = user.DisplayName,
                FullName = user.DisplayName ?? user.Username,
                AvatarPath = user.AvatarPath
            };
        }

        public async Task<bool> ChangePasswordAsync(long userId, ChangePasswordViewModel model)
        {
            if (_userStorageTyped == null)
                throw new InvalidOperationException("UserStorage не инициализирован");

            var user = await _userStorageTyped.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");

            // Проверка старого пароля
            if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.PasswordHash))
                return false;

            // Установка нового пароля
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            await _userStorage.UpdateAsync(user);

            return true;
        }

        /// <summary>
        /// Отправка письма подтверждения email
        /// </summary>
        public async Task SendConfirmationEmailAsync(string email, string confirmationToken, string username)
        {
            try
            {
                // Получаем настройки из конфигурации
                var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");

                // TODO: Вставьте здесь EMAIL_ОТПРАВИТЕЛЯ
                // Раскомментируйте строку ниже и вставьте ваш email, если хотите использовать прямое значение вместо appsettings.json:
                //var senderEmail = "zhurbaivan987@gmail.com";
                // ВАЖНО: Не коммитьте реальные пароли в код! Используйте appsettings.json или переменные окружения.
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                
                // TODO: Вставьте здесь PASSWORD_ПРИЛОЖЕНИЯ
                // Раскомментируйте строку ниже и вставьте пароль приложения, если хотите использовать прямое значение вместо appsettings.json:
                // var senderPassword = "your-app-password";
                // ВАЖНО: Не коммитьте реальные пароли в код! Используйте appsettings.json или переменные окружения.
                var senderPassword = _configuration["EmailSettings:SenderPassword"];
                
                var senderName = _configuration["EmailSettings:SenderName"] ?? "Каталог мобильных приложений";

                if (string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderPassword))
                {
                    throw new InvalidOperationException("Настройки email не заполнены. Заполните EmailSettings:SenderEmail и EmailSettings:SenderPassword в appsettings.json");
                }

                // Создаем сообщение
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(new MailboxAddress(username, email));
                message.Subject = "Подтверждение регистрации - Каталог мобильных приложений";

                // Текст письма с кодом подтверждения
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                        <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                            <h2 style='color: #2c3e50;'>Добро пожаловать в каталог мобильных приложений!</h2>
                            <p>Здравствуйте, {username}!</p>
                            <p>Спасибо за регистрацию в нашем каталоге мобильных приложений. Мы рады видеть вас в нашем сообществе!</p>
                            <p>Для завершения регистрации и подтверждения вашего email-адреса, пожалуйста, введите следующий код подтверждения:</p>
                            <p style='text-align: center; margin: 30px 0;'>
                                <span style='background-color: #3498db; color: white; padding: 15px 40px; font-size: 24px; font-weight: bold; border-radius: 5px; display: inline-block; letter-spacing: 5px;'>{confirmationToken}</span>
                            </p>
                            <p>Введите этот код в форму подтверждения на сайте для завершения регистрации.</p>
                            <p>После подтверждения email вы сможете:</p>
                            <ul style='color: #555;'>
                                <li>Просматривать и скачивать мобильные приложения</li>
                                <li>Оставлять отзывы и оценки</li>
                                <li>Следить за обновлениями ваших любимых приложений</li>
                                <li>Получать персональные рекомендации</li>
                            </ul>
                            <p>Если вы не регистрировались в нашем каталоге, просто проигнорируйте это письмо.</p>
                            <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
                            <p style='color: #7f8c8d; font-size: 12px;'>
                                С уважением,<br>
                                Команда каталога мобильных приложений<br>
                                Откройте для себя лучшие приложения для ваших устройств!
                            </p>
                        </div>
                    </body>
                    </html>";

                bodyBuilder.TextBody = $@"
Добро пожаловать в каталог мобильных приложений!

Здравствуйте, {username}!

Спасибо за регистрацию в нашем каталоге мобильных приложений. Мы рады видеть вас в нашем сообществе!

Для завершения регистрации и подтверждения вашего email-адреса, пожалуйста, введите следующий код подтверждения:

{confirmationToken}

Введите этот код в форму подтверждения на сайте для завершения регистрации.

После подтверждения email вы сможете:
- Просматривать и скачивать мобильные приложения
- Оставлять отзывы и оценки
- Следить за обновлениями ваших любимых приложений
- Получать персональные рекомендации

Если вы не регистрировались в нашем каталоге, просто проигнорируйте это письмо.

С уважением,
Команда каталога мобильных приложений
Откройте для себя лучшие приложения для ваших устройств!";

                message.Body = bodyBuilder.ToMessageBody();

                // Отправка через SMTP
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(senderEmail, senderPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                Console.WriteLine($"AccountService: Письмо подтверждения отправлено на {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AccountService: Ошибка при отправке письма: {ex.Message}");
                throw new InvalidOperationException($"Не удалось отправить письмо подтверждения: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Подтверждение email по коду - создает пользователя в БД только после подтверждения
        /// </summary>
        public async Task<bool> ConfirmEmailAsync(string email, string code)
        {
            if (_userStorageTyped == null)
                throw new InvalidOperationException("UserStorage не инициализирован");

            // Ищем данные регистрации в кэше
            var cacheKey = $"pending_registration_{email.ToLowerInvariant()}";
            if (!_memoryCache.TryGetValue(cacheKey, out PendingRegistration pendingRegistration))
            {
                Console.WriteLine($"AccountService: Данные регистрации не найдены в кэше для {email}");
                return false;
            }

            // Проверяем код
            if (pendingRegistration.ConfirmationCode != code)
            {
                Console.WriteLine($"AccountService: Неверный код подтверждения для {email}");
                return false;
            }

            // Проверяем, не создан ли уже пользователь в БД (на случай повторного подтверждения)
            var existingUser = await _userStorageTyped.GetByEmailAsync(email);
            if (existingUser != null)
            {
                // Пользователь уже существует - просто подтверждаем email
                if (!existingUser.EmailConfirmed)
                {
                    existingUser.EmailConfirmed = true;
                    existingUser.EmailConfirmationToken = null;
                    await _userStorage.UpdateAsync(existingUser);
                    Console.WriteLine($"AccountService: Email {email} подтвержден для существующего пользователя");
                }
                // Удаляем из кэша
                _memoryCache.Remove(cacheKey);
                return true;
            }

            // Создаем пользователя в БД только после подтверждения кода
            Console.WriteLine($"AccountService: Создаем пользователя в БД после подтверждения кода для {email}");
            var user = new UserDb
            {
                Username = pendingRegistration.Username,
                Email = pendingRegistration.Email,
                PasswordHash = pendingRegistration.PasswordHash,
                RegisteredAt = DateTimeOffset.UtcNow,
                Role = "user",
                EmailConfirmed = true, // Сразу подтвержден, т.к. код проверен
                EmailConfirmationToken = null
            };

            await _userStorage.CreateAsync(user);
            Console.WriteLine($"AccountService: Пользователь {email} успешно создан в БД после подтверждения");

            // Удаляем данные из кэша
            _memoryCache.Remove(cacheKey);

            return true;
        }

        /// <summary>
        /// Генерация кода подтверждения (6 цифр)
        /// </summary>
        private string GenerateConfirmationToken()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // 6-значный код
        }

        /// <summary>
        /// Проверяет, существует ли пользователь в БД, и если нет - создает его. Возвращает ClaimsIdentity для аутентификации.
        /// </summary>
        public async Task<BaseResponse<ClaimsIdentity>> IsCreatedAccount(User model)
        {
            try
            {
                var userDb = new UserDb();
                
                // Проверяем, существует ли пользователь с таким email
                if (_userStorageTyped != null)
                {
                    var existingUser = await _userStorageTyped.GetByEmailAsync(model.Email);
                    
                    if (existingUser == null)
                    {
                        // Пользователя нет - создаем нового
                        model.PasswordHash = BCrypt.Net.BCrypt.HashPassword("google");
                        
                        userDb = new UserDb
                        {
                            Username = model.Login ?? model.FullName ?? model.Email.Split('@')[0],
                            Email = model.Email,
                            PasswordHash = model.PasswordHash,
                            RegisteredAt = DateTimeOffset.UtcNow,
                            Role = "user",
                            EmailConfirmed = true, // Google подтвердил email
                            DisplayName = model.FullName ?? model.Login,
                            AvatarPath = model.PathImage
                        };
                        
                        await _userStorage.CreateAsync(userDb);
                        
                        // Обновляем LastLogin
                        userDb.LastLogin = DateTimeOffset.UtcNow;
                        await _userStorage.UpdateAsync(userDb);
                        
                        // Создаем ClaimsIdentity для нового пользователя
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, userDb.Id.ToString()),
                            new Claim(ClaimTypes.Name, userDb.Username ?? ""),
                            new Claim(ClaimTypes.Email, userDb.Email ?? ""),
                            new Claim(ClaimTypes.Role, userDb.Role ?? "user")
                        };
                        
                        var claimsIdentity = new ClaimsIdentity(claims, "Cookie");
                        
                        return new BaseResponse<ClaimsIdentity>
                        {
                            Data = claimsIdentity,
                            Description = "Объект добавился",
                            StatusCode = StatusCode.OK
                        };
                    }
                    else
                    {
                        // Пользователь уже существует - обновляем LastLogin и возвращаем его ClaimsIdentity
                        existingUser.LastLogin = DateTimeOffset.UtcNow;
                        await _userStorage.UpdateAsync(existingUser);
                        
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                            new Claim(ClaimTypes.Name, existingUser.Username ?? ""),
                            new Claim(ClaimTypes.Email, existingUser.Email ?? ""),
                            new Claim(ClaimTypes.Role, existingUser.Role ?? "user")
                        };
                        
                        var claimsIdentity = new ClaimsIdentity(claims, "Cookie");
                        
                        return new BaseResponse<ClaimsIdentity>
                        {
                            Data = claimsIdentity,
                            Description = "Объект уже был создан",
                            StatusCode = StatusCode.OK
                        };
                    }
                }
                else
                {
                    return new BaseResponse<ClaimsIdentity>
                    {
                        Description = "UserStorage не инициализирован",
                        StatusCode = StatusCode.InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
