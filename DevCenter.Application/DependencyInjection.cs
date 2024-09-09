using DevCenter.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace DevCenter.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<UserServices>();
            return services;
        }
    }
}
