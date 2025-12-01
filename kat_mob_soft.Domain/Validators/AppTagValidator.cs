using FluentValidation;
using kat_mob_soft.Domain.Models;
using System;

namespace kat_mob_soft.Domain.Validators
{
    public class AppTagValidator : AbstractValidator<AppTag>
    {
        public AppTagValidator()
        {
            RuleFor(x => x.AppId)
                .NotEqual(Guid.Empty).WithMessage("ID приложения обязателен");

            RuleFor(x => x.TagId)
                .NotEqual(Guid.Empty).WithMessage("ID тега обязателен");
        }
    }
}


