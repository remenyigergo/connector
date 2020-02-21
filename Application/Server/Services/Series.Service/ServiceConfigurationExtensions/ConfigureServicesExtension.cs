using Microsoft.Extensions.DependencyInjection;
using Series.DataManagement.MongoDB.Repositories;
using Standard.Core.DataManager.MongoDB.IRepository;
using Standard.Core.DataManager.MongoDB.Repository;

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
