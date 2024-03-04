using Application.FluentValidation;
using Core.Exceptions;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Pipeline
{
    public class ValidationInterceptor : IValidatorInterceptor
    {
        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
            {
                var errors = result.Errors.GroupBy(m => m.PropertyName)
                    .ToDictionary(k => k.Key, v => v.Select(m => m.ErrorMessage));
                //throw new BadRequestException("Model doesn't meet the requirements!", errors);
            }
            return result;
        }

        public ValidationResult AfterMvcValidation(ControllerContext controllerContext, ValidationContext validationContext, ValidationResult result)
        {
            throw new NotImplementedException();
        }

        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }

        public ValidationContext BeforeMvcValidation(ControllerContext controllerContext, ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
    public static class ValidationInterceptorExtension
    {
        public static IServiceCollection AddValidationInterceptor(this IServiceCollection services)
        {
            services.AddScoped<IValidatorInterceptor, ValidationInterceptor>();
            services.AddValidatorsFromAssemblyContaining<IValidatorExtension>();
            services.AddMvc().AddFluentValidation(cfg =>
            {  
                //cfg.DisableDataAnnotationsValidation = true;
            });
             
            return services;

        }
    }
}
