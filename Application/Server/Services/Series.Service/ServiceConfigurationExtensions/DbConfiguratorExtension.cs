using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Series.DataManagement.MongoDB.Models.Series;
using Series.DataManagement.MongoDB.SeriesFunctionModels;
using Standard.Core;
using Standard.Core.Configuration;

namespace Series.Service.ServiceConfigurationExtensions
{
    public static class DbConfiguratorExtension
    {
        public static void HandleDb(this IServiceCollection services, System.IServiceProvider serviceProvider)
        {
            var mongoConnection = SetupDbConnection(serviceProvider);
            RegisterDbCollections(services, mongoConnection);
        }

        public static IMongoDatabase SetupDbConnection(System.IServiceProvider serviceProvider)
        {
            var serviceConfiguration = serviceProvider.GetService<ServiceConfiguration>();
            
            var client = new MongoClient(serviceConfiguration.MongoConnection.ConnectionString);
            return client.GetDatabase(serviceConfiguration.MongoConnection.Database);
        }

        public static void RegisterDbCollections(IServiceCollection services, IMongoDatabase db)
        {
            //Is this ok? Yes it is :)
            var mongoSeriesCollection = db.GetCollection<MongoSeriesDao>("Series");
            var seenEpisodesCollection = db.GetCollection<EpisodeSeenDao>("SeenEpisodes");


            services.AddSingleton(db);
            services.AddSingleton(mongoSeriesCollection);
            services.AddSingleton(seenEpisodesCollection);

            //private IMongoCollection<AddedSeries> AddedSeries =>
            //    Database.GetCollection<AddedSeries>("AddedSeries");
            //private IMongoCollection<EpisodeStartedDao> EpisodeStarted =>
            //    Database.GetCollection<EpisodeStartedDao>("EpisodeStarted");
            //private IMongoCollection<FavoriteSeriesDao> FavoriteSeries =>
            //    Database.GetCollection<FavoriteSeriesDao>("FavoriteSeries");
            //private IMongoCollection<FavoriteEpisode> FavoriteEpisode =>
            //    Database.GetCollection<FavoriteEpisode>("FavoriteEpisode");
            //private IMongoCollection<SeriesComment> SeriesComments =>
            //    Database.GetCollection<SeriesComment>("SeriesComments");
            //private IMongoCollection<EpisodeComment> EpisodeComments =>
            //    Database.GetCollection<EpisodeComment>("EpisodeComments");
            //private IMongoCollection<SeriesRate> SeriesRates =>
            //    Database.GetCollection<SeriesRate>("SeriesRates");
            //private IMongoCollection<EpisodeRate> EpisodeRates =>
            //    Database.GetCollection<EpisodeRate>("EpisodeRates");
        }

    }
}
