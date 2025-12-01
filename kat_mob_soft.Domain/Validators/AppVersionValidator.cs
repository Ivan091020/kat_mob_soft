using FluentValidation;
using kat_mob_soft.Domain.Models;
using System;
using System.Text.RegularExpressions;

namespace kat_mob_soft.Domain.Validators
{
    public class AppVersionValidator : AbstractValidator<AppVersion>
    {
        public AppVersionValidator()
        {
            RuleFor(x => x.VersionNumber)
                .NotEmpty().WithMessage("Номер версии обязателен")
                .Matches(@"^\d+(\.\d+){0,2}$")
                .WithMessage("Номер версии должен быть в формате X.Y.Z (например, 1.0.0 или 2.5)");

            RuleFor(x => x.ReleaseDate)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Дата выпуска не может быть в будущем");

            RuleFor(x => x.AppId)
                .NotEqual(Guid.Empty).WithMessage("ID приложения обязателен");
        }
    }
}

