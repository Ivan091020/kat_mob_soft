using FluentValidation;
using kat_mob_soft.Domain.Models;
using System;

namespace kat_mob_soft.Domain.Validators
{
    public class AppValidator : AbstractValidator<App>
    {
        public AppValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название приложения обязательно")
                .Length(3, 200).WithMessage("Название должно содержать от 3 до 200 символов");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Описание обязательно")
                .Length(10, 5000).WithMessage("Описание должно содержать от 10 до 5000 символов");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Цена не может быть отрицательной");

            RuleFor(x => x.ReleaseDate)
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Дата выпуска не может быть в будущем");

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 5).WithMessage("Рейтинг должен быть от 0 до 5");

            // TODO: Проверка существования CategoryId через сервис/Storage
            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage("Категория обязательна")
                .NotEqual(Guid.Empty).WithMessage("Категория обязательна");

            // TODO: Проверка существования DeveloperId через сервис/Storage
            RuleFor(x => x.DeveloperId)
                .NotNull().WithMessage("Разработчик обязателен")
                .NotEqual(Guid.Empty).WithMessage("Разработчик обязателен");

            RuleFor(x => x.ImagePath)
                .Must(uri => string.IsNullOrEmpty(uri) || Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Некорректный URL изображения")
                .When(x => !string.IsNullOrEmpty(x.ImagePath));
        }
    }
}

