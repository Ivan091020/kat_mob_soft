using FluentValidation;
using kat_mob_soft.Domain.Models;

namespace kat_mob_soft.Domain.Validators
{
    public class TagValidator : AbstractValidator<Tag>
    {
        public TagValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название тега обязательно")
                .Length(2, 50).WithMessage("Название должно содержать от 2 до 50 символов");
        }
    }
}

