using FluentValidation;
using kat_mob_soft.Domain.Models;
using System;

namespace kat_mob_soft.Domain.Validators
{
    public class ReportValidator : AbstractValidator<Report>
    {
        public ReportValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Описание жалобы обязательно")
                .Length(10, 2000).WithMessage("Описание должно содержать от 10 до 2000 символов");

            RuleFor(x => x.ReporterUserId)
                .NotEqual(Guid.Empty).WithMessage("ID пользователя обязателен");
        }
    }
}

