using FluentValidation;
using kat_mob_soft.Domain.Models;
using System;

namespace kat_mob_soft.Domain.Validators
{
    public class AppScreenshotValidator : AbstractValidator<AppScreenshot>
    {
        public AppScreenshotValidator()
        {
            RuleFor(x => x.AppId)
                .NotEqual(Guid.Empty).WithMessage("ID приложения обязателен");

            RuleFor(x => x.Path)
                .NotEmpty().WithMessage("Путь к скриншоту обязателен")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute) || 
                             (!string.IsNullOrEmpty(uri) && (uri.StartsWith("/") || uri.StartsWith("~"))))
                .WithMessage("Некорректный путь к скриншоту");
        }
    }
}


