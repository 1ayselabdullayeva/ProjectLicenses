using Application.FluentValidation;
using Application.Services;
using Business.Services;
using Core.Services;
using DataAccessLayer;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Entities;
namespace Application
{
    public static class BusinessLogicInjection
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataAccess(configuration);
            #region Register Services
            services.AddTransient<IUserServices, UserService>();
            services.AddTransient<ITicketServices, TicketService>();
            services.AddTransient<IProductServices, ProductService>();
            services.AddTransient<ILicensesServices, LicensesService>();
            services.AddTransient<IRolesServices, RolesService>();
            services.AddTransient<IJWTServices, JWTService>();
            services.AddScoped<IEmailSenderServices, EmailSenderService>();
            services.AddScoped<IValidator<User>,UserValidator>();
            services.AddScoped<IValidator<Product>,ProductValidator>();
            services.AddScoped<IValidator<Ticket>, TicketValidator > ();
            services.AddScoped<IValidator<Licenses>, LicensesValidator>();
            #endregion
            return services;
        }
    }
}
