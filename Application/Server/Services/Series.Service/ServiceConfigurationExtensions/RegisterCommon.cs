using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Standard.Core.NetworkManager;

namespace Series.Service.ServiceConfigurationExtensions
{
    public static class RegisterCommon
    {
        public static void RegisterCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<IWebClientManager, WebClientManager>();

            var log = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().CreateLogger();
            services.AddSingleton<ILogger>(log);
        }
    }
}
