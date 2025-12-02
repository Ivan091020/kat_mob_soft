using System.Threading.Tasks;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Service
{
    public interface IAccountService
    {
        Task<string> RegisterAsync(RegisterViewModel model); // Возвращает код подтверждения
        Task<TokenViewModel> LoginAsync(LoginViewModel model);
        Task<ProfileViewModel> GetProfileAsync(long userId);
        Task<ProfileViewModel> UpdateProfileAsync(long userId, UpdateProfileViewModel model);
        Task<bool> ChangePasswordAsync(long userId, ChangePasswordViewModel model);
        Task SendConfirmationEmailAsync(string email, string confirmationToken, string username);
        Task<bool> ConfirmEmailAsync(string email, string code);
    }
}
