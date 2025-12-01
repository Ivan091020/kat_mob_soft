using FluentValidation;
using kat_mob_soft.Domain.Models;
using System;

namespace kat_mob_soft.Domain.Validators
{
    public class AppIconValidator : AbstractValidator<AppIcon>
    {
        public AppIconValidator()
        {
            RuleFor(x => x.AppId)
                .NotEqual(Guid.Empty).WithMessage("ID приложения обязателен");

            RuleFor(x => x.Path)
                .NotEmpty().WithMessage("Путь к иконке обязателен")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute) || 
                             (!string.IsNullOrEmpty(uri) && (uri.StartsWith("/") || uri.StartsWith("~"))))
                .WithMessage("Некорректный путь к иконке");
        }
    }
}


