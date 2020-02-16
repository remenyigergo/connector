using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Standard.Core;

namespace Series.Service.ServiceConfigurationExtensions
{
    public static class ServiceConfigurationExtension
    {
        public static void SetConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceConfiguration>(configuration.GetSection("ApplicationSettings"));
        }
    }
}
