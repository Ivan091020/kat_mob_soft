using FluentValidation;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Domain.Validators
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Неверный формат email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .Length(6, 100).WithMessage("Пароль должен содержать от 6 до 100 символов");
        }
    }
}

