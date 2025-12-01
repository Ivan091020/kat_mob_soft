using FluentValidation;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Domain.Validators
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Имя пользователя обязательно")
                .Length(3, 50).WithMessage("Имя пользователя должно содержать от 3 до 50 символов");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Неверный формат email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .Length(6, 100).WithMessage("Пароль должен содержать от 6 до 100 символов");
        }
    }
}

