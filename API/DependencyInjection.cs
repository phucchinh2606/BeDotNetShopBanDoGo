using Application;
using Infrastructure;

namespace API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationDI()
                    .AddInfrastructureDI(configuration);

            return services;
        }
    }
}
