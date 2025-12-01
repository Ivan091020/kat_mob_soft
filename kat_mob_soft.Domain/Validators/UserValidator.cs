using FluentValidation;
using kat_mob_soft.Domain.Models;

namespace kat_mob_soft.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Неверный формат email");

            RuleFor(x => x.FullName)
                .Length(2, 200).WithMessage("Полное имя должно содержать от 2 до 200 символов")
                .When(x => !string.IsNullOrEmpty(x.FullName));

            // PasswordHash обычно не валидируется напрямую, т.к. это хеш
            // Валидация пароля происходит через ChangePasswordViewModel
        }
    }
}


