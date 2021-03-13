using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Series.DataManagement.MongoDB.Models.Series;
using Series.DataManagement.MongoDB.SeriesFunctionModels;
using Series.Service.Models;
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
            var serviceConfiguration = serviceProvider.GetService<IServiceConfiguration>();
            
            var client = new MongoClient(serviceConfiguration.MongoConnection.ConnectionString);
            return client.GetDatabase(serviceConfiguration.MongoConnection.Database);
        }

        public static void RegisterDbCollections(IServiceCollection services, IMongoDatabase db)
        {
            //Is this ok? Yes it is :)
            var mongoSeriesCollection = db.GetCollection<MongoSeriesDao>("Series");
            var seenEpisodesCollection = db.GetCollection<EpisodeSeenDao>("SeenEpisodes");
            var addedSeriesCollection = db.GetCollection<AddedSeriesDao>("AddedSeries");
            var episodeStartedCollection = db.GetCollection<EpisodeStartedDao>("EpisodeStarted");
            var favoriteSeriesCollection = db.GetCollection<FavoriteSeriesDao>("FavoriteSeries");
            var favoriteEpisodeCollection = db.GetCollection<FavoriteEpisodeDao>("FavoriteEpisode");
            var seriesCommentCollection = db.GetCollection<SeriesCommentDao>("SeriesComments");
            var episodeCommentCollection = db.GetCollection<EpisodeCommentDao>("EpisodeComments");
            var seriesRatesCollection = db.GetCollection<EpisodeCommentDao>("SeriesRates");
            var episodeRateCollection = db.GetCollection<EpisodeRateDao>("EpisodeRates");
            
            services.AddSingleton(db);
            services.AddSingleton(mongoSeriesCollection);
            services.AddSingleton(seenEpisodesCollection);
            services.AddSingleton(addedSeriesCollection);
            services.AddSingleton(episodeStartedCollection);
            services.AddSingleton(favoriteSeriesCollection);
            services.AddSingleton(favoriteEpisodeCollection);
            services.AddSingleton(seriesCommentCollection);
            services.AddSingleton(episodeCommentCollection);
            services.AddSingleton(seriesRatesCollection);
            services.AddSingleton(episodeRateCollection);

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
