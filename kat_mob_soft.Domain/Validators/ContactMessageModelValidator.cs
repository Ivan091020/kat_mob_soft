using FluentValidation;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Domain.Validators
{
    public class ContactMessageModelValidator : AbstractValidator<ContactMessageModel>
    {
        public ContactMessageModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Имя обязательно")
                .Length(2, 100).WithMessage("Имя должно содержать от 2 до 100 символов");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Неверный формат email");

            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Тема обязательна")
                .Length(3, 200).WithMessage("Тема должна содержать от 3 до 200 символов");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Сообщение обязательно")
                .Length(10, 1000).WithMessage("Сообщение должно содержать от 10 до 1000 символов");
        }
    }
}

