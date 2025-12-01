using FluentValidation;
using kat_mob_soft.Domain.Models;
using System;

namespace kat_mob_soft.Domain.Validators
{
    public class PurchaseValidator : AbstractValidator<Purchase>
    {
        public PurchaseValidator()
        {
            RuleFor(x => x.AppId)
                .NotEqual(Guid.Empty).WithMessage("ID приложения обязателен");

            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty).WithMessage("ID пользователя обязателен");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Цена не может быть отрицательной");

            RuleFor(x => x.PurchasedAt)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Дата покупки не может быть в будущем");
        }
    }
}


