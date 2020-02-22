using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Standard.Core.Configuration;

namespace Series.Service.ServiceConfigurationExtensions
{
    public static class ServiceConfigurationExtension
    {
        public static void SetConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceConfig = configuration.GetSection("ServiceConfiguration").Get<ServiceConfiguration>();
            services.AddSingleton<IServiceConfiguration>(serviceConfig);
        }
    }
}
