using Microsoft.Extensions.DependencyInjection;
using Series.DataManagement.MongoDB.Repositories;
using Series.Parsers;
using Series.Parsers.TMDB;
using Series.Parsers.TVMAZE;

namespace Series.Service.ServiceConfigurationExtensions
{
    public static class ConfigureRepositoriesExtension
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ISeriesRepository, SeriesRepository>();

            services.AddSingleton<ITvMazeParser, TvMazeParser>();
            services.AddSingleton<ITmdbParser, TmdbParser>();
        }
    }
}
