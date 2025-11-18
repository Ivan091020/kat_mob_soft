using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Domain.Models.Db;
using kat_mob_soft.DAL.Interfaces;
using kat_mob_soft.DAL.Interfaces.Storage;

namespace kat_mob_soft.Service
{
    public class AccountService : IAccountService
    {
        private readonly IBaseStorage<UserDb> _userStorage;
        private readonly UserStorage _userStorageTyped;
        private readonly IMapper _mapper;

        public AccountService(IBaseStorage<UserDb> userStorage, IMapper mapper)
        {
            _userStorage = userStorage;
            _userStorageTyped = userStorage as UserStorage;
            _mapper = mapper;
        }

        public async Task<ProfileViewModel> RegisterAsync(RegisterViewModel model)
        {
            // Проверка существования пользователя
            if (_userStorageTyped != null)
            {
                var existingUser = await _userStorageTyped.GetByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("Пользователь с таким email уже существует");
                }
            }

            // Создание нового пользователя
            var user = _mapper.Map<UserDb>(model);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            user.RegisteredAt = DateTimeOffset.UtcNow;
            user.Role = "user";

            await _userStorage.CreateAsync(user);

            return _mapper.Map<ProfileViewModel>(user);
        }

        public async Task<TokenViewModel> LoginAsync(LoginViewModel model)
        {
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

            return _mapper.Map<ProfileViewModel>(user);
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

            return _mapper.Map<ProfileViewModel>(user);
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
    }
}
