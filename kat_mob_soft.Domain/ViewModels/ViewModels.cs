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
        public string Email { get; set; } 
        public string Password { get; set; } 
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

