using System.ComponentModel.DataAnnotations;

namespace kat_mob_soft.Domain.ViewModels
{
    public class ProfileViewModel 
    { 
        public long Id { get; set; } 
        public string FullName { get; set; } 
        public string Email { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string AvatarPath { get; set; }
    }

    public class RegisterViewModel 
    { 
        public string Email { get; set; } 
        public string Password { get; set; } 
        public string Username { get; set; }
    }

    public class LoginViewModel 
    { 
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; } 
        
        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordViewModel 
    { 
        public string OldPassword { get; set; } 
        public string NewPassword { get; set; } 
    }

    public class UpdateProfileViewModel 
    { 
        public string FullName { get; set; } 
        public string Email { get; set; } 
        public string DisplayName { get; set; }
    }

    public class TokenViewModel 
    { 
        public string Token { get; set; } 
        public string RefreshToken { get; set; } 
    }
}


