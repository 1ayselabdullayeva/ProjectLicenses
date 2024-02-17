using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Repositories.Specific;
using DataAccessLayer.Repositories;
using DataAccessLayer.Persistence;
using DataAccess.Repositories;

namespace DataAccessLayer
{
	public static class DataAccessInjection
	{
		public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<DbContext, ProjectDbContext>();
			services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });
           
            services.AddAutoMapper(typeof(DataAccessInjection));
            #region Register Repositories
            services.AddScoped<ITicketRepository, TicketRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<ILicensesRepository, LicensesRepository>();
			services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<IJWTManagerRepository, JWTManagerRepository>();
            #endregion

            return services;
		}
	}

}
