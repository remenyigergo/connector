using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Series.DataManagement.MongoDB.Models.Series;
using Standard.Core;

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

            var client = new MongoClient(serviceConfiguration.Connection.ConnectionString);
            return client.GetDatabase(serviceConfiguration.Connection.Database);
        }

        public static void RegisterDbCollections(IServiceCollection services, IMongoDatabase db)
        {
            //Is this ok?
            var mongoSeriesCollection = db.GetCollection<MongoSeries>("Series");
            services.AddSingleton(db);
            services.AddSingleton(mongoSeriesCollection);
        }

    }
}
