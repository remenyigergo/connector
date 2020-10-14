using Microsoft.Extensions.DependencyInjection;

namespace Series.Service.ServiceConfigurationExtensions
{
    public static class ConfigureServicesExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<ISeries, Series>();

        }
    }
}
