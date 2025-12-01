using FluentValidation;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Domain.Validators
{
    public class UpdateProfileViewModelValidator : AbstractValidator<UpdateProfileViewModel>
    {
        public UpdateProfileViewModelValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Полное имя обязательно")
                .Length(2, 100).WithMessage("Полное имя должно содержать от 2 до 100 символов");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Неверный формат email");

            RuleFor(x => x.DisplayName)
                .Length(2, 50).WithMessage("Отображаемое имя должно содержать от 2 до 50 символов")
                .When(x => !string.IsNullOrEmpty(x.DisplayName));
        }
    }
}

