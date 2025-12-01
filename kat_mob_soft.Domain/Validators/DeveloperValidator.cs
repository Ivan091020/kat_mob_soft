using FluentValidation;
using kat_mob_soft.Domain.Models;
using System;

namespace kat_mob_soft.Domain.Validators
{
    public class DeveloperValidator : AbstractValidator<Developer>
    {
        public DeveloperValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название разработчика обязательно")
                .Length(2, 200).WithMessage("Название должно содержать от 2 до 200 символов");

            RuleFor(x => x.ContactEmail)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Неверный формат email");

            RuleFor(x => x.Website)
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Некорректный URL веб-сайта")
                .When(x => !string.IsNullOrEmpty(x.Website));
        }
    }
}

