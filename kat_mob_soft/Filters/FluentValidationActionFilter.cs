using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace kat_mob_soft.Filters
{
    public class FluentValidationActionFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FluentValidationActionFilter> _logger;

        public FluentValidationActionFilter(
            IServiceProvider serviceProvider,
            ILogger<FluentValidationActionFilter> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Получаем все параметры действия
            foreach (var parameter in context.ActionDescriptor.Parameters)
            {
                if (context.ActionArguments.TryGetValue(parameter.Name, out var argument) && argument != null)
                {
                    var argumentType = argument.GetType();
                    
                    // Пытаемся найти валидатор для типа аргумента
                    var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
                    var validator = _serviceProvider.GetService(validatorType) as IValidator;

                    if (validator != null)
                    {
                        var validationContext = new ValidationContext<object>(argument);
                        var result = await validator.ValidateAsync(validationContext);

                        // Добавляем ошибки в ModelState
                        if (!result.IsValid)
                        {
                            _logger.LogWarning(
                                "Validation failed for {ModelType}: {ErrorCount} errors",
                                argumentType.Name,
                                result.Errors.Count);

                            foreach (var error in result.Errors)
                            {
                                context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                            }
                        }
                    }
                }
            }

            await next();
        }
    }
}

