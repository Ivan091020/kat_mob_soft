using System;
using System.Collections.Generic;
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

    public class ContactMessageModel
    {
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Тема обязательна")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Сообщение обязательно")]
        [StringLength(1000, ErrorMessage = "Сообщение не должно превышать 1000 символов")]
        public string Message { get; set; }
    }

    public class ConfirmEmailViewModel
    {
        [Required(ErrorMessage = "Введите код")]
        public string CodeConfirm { get; set; }

        public string GeneratedCode { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }

    public class AppViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string PathImg { get; set; }
        public int CountDownload { get; set; }
        public string CategoryName { get; set; }
        public decimal AverageRating { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }

    public class AddAppViewModel
    {
        [Required(ErrorMessage = "Название обязательно")]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public decimal Price { get; set; } = 0m;

        [MaxLength(3)]
        public string Currency { get; set; } = "USD";

        [Required(ErrorMessage = "Категория обязательна")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Разработчик обязателен")]
        [MaxLength(200)]
        public string DeveloperName { get; set; }

        public bool IsPublished { get; set; } = false;
    }

    public class AppPageViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DeveloperName { get; set; }
        public string CategoryName { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public decimal AverageRating { get; set; }
        public string PathImg { get; set; }
        public List<AppScreenshotViewModel> Screenshots { get; set; }
    }

    public class AppScreenshotViewModel
    {
        public long Id { get; set; }
        public string PathImg { get; set; }
        public string Caption { get; set; }
    }

    public class ReviewViewModel
    {
        public long Id { get; set; }
        public long AppId { get; set; }
        public string AppName { get; set; }
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public short Rating { get; set; }
        public bool IsApproved { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class AddReviewViewModel
    {
        [Required(ErrorMessage = "ID приложения обязателен")]
        public long AppId { get; set; }

        [Required(ErrorMessage = "Заголовок обязателен")]
        [MaxLength(250, ErrorMessage = "Заголовок не должен превышать 250 символов")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Текст отзыва обязателен")]
        [MinLength(10, ErrorMessage = "Текст отзыва должен содержать минимум 10 символов")]
        public string Body { get; set; }

        [Required(ErrorMessage = "Рейтинг обязателен")]
        [Range(1, 5, ErrorMessage = "Рейтинг должен быть от 1 до 5")]
        public short Rating { get; set; }
    }

    public class PurchaseViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long AppId { get; set; }
        public string AppName { get; set; }
        public string AppIconPath { get; set; }
        public decimal PricePaid { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTimeOffset PurchasedAt { get; set; }
    }

    public class AddToCartViewModel
    {
        [Required(ErrorMessage = "ID приложения обязателен")]
        public long AppId { get; set; }
    }

    public class CartViewModel
    {
        public List<PurchaseViewModel> Items { get; set; } = new List<PurchaseViewModel>();
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; } = "USD";
    }
}


