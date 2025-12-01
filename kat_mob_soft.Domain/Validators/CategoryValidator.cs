using FluentValidation;
using kat_mob_soft.Domain.Models;
using System.Text.RegularExpressions;

namespace kat_mob_soft.Domain.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название категории обязательно")
                .Length(2, 100).WithMessage("Название должно содержать от 2 до 100 символов");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug обязателен")
                .Length(2, 100).WithMessage("Slug должен содержать от 2 до 100 символов")
                .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage("Slug может содержать только строчные латинские буквы, цифры и дефисы");

            RuleFor(x => x.Description)
                .Length(10, 1000).WithMessage("Описание должно содержать от 10 до 1000 символов")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}

