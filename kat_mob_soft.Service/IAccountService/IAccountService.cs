using System.Threading.Tasks;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Service
{
    public interface IAccountService
    {
        Task<ProfileViewModel> RegisterAsync(RegisterViewModel model);
        Task<TokenViewModel> LoginAsync(LoginViewModel model);
        Task<ProfileViewModel> GetProfileAsync(long userId);
        Task<ProfileViewModel> UpdateProfileAsync(long userId, UpdateProfileViewModel model);
        Task<bool> ChangePasswordAsync(long userId, ChangePasswordViewModel model);
    }
}
