using FluentValidation;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Domain.Validators
{
    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Текущий пароль обязателен")
                .Length(6, 100).WithMessage("Пароль должен содержать от 6 до 100 символов");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Новый пароль обязателен")
                .Length(6, 100).WithMessage("Новый пароль должен содержать от 6 до 100 символов")
                .NotEqual(x => x.OldPassword).WithMessage("Новый пароль должен отличаться от текущего");
        }
    }
}

