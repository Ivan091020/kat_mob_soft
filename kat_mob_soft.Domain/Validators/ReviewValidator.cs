using FluentValidation;
using kat_mob_soft.Domain.Models;
using System;

namespace kat_mob_soft.Domain.Validators
{
    public class ReviewValidator : AbstractValidator<Review>
    {
        public ReviewValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Текст отзыва обязателен")
                .Length(10, 2000).WithMessage("Текст отзыва должен содержать от 10 до 2000 символов");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Рейтинг должен быть от 1 до 5");

            RuleFor(x => x.AppId)
                .NotEqual(Guid.Empty).WithMessage("ID приложения обязателен");

            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty).WithMessage("ID пользователя обязателен");
        }
    }
}

