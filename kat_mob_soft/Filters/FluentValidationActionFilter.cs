using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using kat_mob_soft.Domain.ViewModels;

namespace kat_mob_soft.Filters
{
    public class FluentValidationActionFilter : IAsyncActionFilter
    {
        private readonly IValidator<LoginViewModel> _loginValidator;
        private readonly IValidator<RegisterViewModel> _registerValidator;

        public FluentValidationActionFilter(
            IValidator<LoginViewModel> loginValidator,
            IValidator<RegisterViewModel> registerValidator)
        {
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Получаем все параметры действия
            foreach (var parameter in context.ActionDescriptor.Parameters)
            {
                if (context.ActionArguments.TryGetValue(parameter.Name, out var argument) && argument != null)
                {
                    ValidationResult result = null;

                    // Проверяем тип и используем соответствующий валидатор
                    if (argument is LoginViewModel loginModel)
                    {
                        result = await _loginValidator.ValidateAsync(loginModel);
                    }
                    else if (argument is RegisterViewModel registerModel)
                    {
                        result = await _registerValidator.ValidateAsync(registerModel);
                    }

                    // Добавляем ошибки в ModelState
                    if (result != null && !result.IsValid)
                    {
                        foreach (var error in result.Errors)
                        {
                            context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                        }
                    }
                }
            }

            await next();
        }
    }
}

