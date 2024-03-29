using MalirosoBlog.Services.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MalirosoBlog.Services.Extensions
{
    public static class ConfigurationBinder
    {
        public static IServiceCollection BindConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            JWTConfiguration jwt = new();           

            configuration.GetSection("JWTConfiguration").Bind(jwt);

            services.AddSingleton(jwt);

            return services;
        }
    }
}
