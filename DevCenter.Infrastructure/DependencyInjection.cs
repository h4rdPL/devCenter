using DevCenter.Domain.Users;
using DevCenter.Infrastructure.Data;
using DevCenter.Infrastructure.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevCenter.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the ApplicationDbContext with the appropriate database provider
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            // Register the repositories
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
