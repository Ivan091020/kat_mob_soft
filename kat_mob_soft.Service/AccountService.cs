using System.Threading.Tasks;
using kat_mob_soft.Domain.ViewModels;
using kat_mob_soft.Service;

namespace kat_mob_soft.Service
{
    public class AccountService : IAccountService
    {
        public Task<ProfileViewModel> RegisterAsync(RegisterViewModel model)
        {
            // TODO: Implement registration logic
            throw new System.NotImplementedException();
        }

        public Task<TokenViewModel> LoginAsync(LoginViewModel model)
        {
            // TODO: Implement login logic
            throw new System.NotImplementedException();
        }

        public Task<ProfileViewModel> GetProfileAsync(long userId)
        {
            // TODO: Implement get profile logic
            throw new System.NotImplementedException();
        }

        public Task<ProfileViewModel> UpdateProfileAsync(long userId, UpdateProfileViewModel model)
        {
            // TODO: Implement update profile logic
            throw new System.NotImplementedException();
        }

        public Task<bool> ChangePasswordAsync(long userId, ChangePasswordViewModel model)
        {
            // TODO: Implement change password logic
            throw new System.NotImplementedException();
        }
    }
}
