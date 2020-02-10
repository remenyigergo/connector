using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Series.DataManagement.MongoDB.Models;
using Series.DataManagement.MongoDB.Models.Series;
using Standard.Core.DataManager.MongoDB.DbModels;

namespace Standard.Core.DataManager.MongoDB
{
    public class BaseMongoDbDataManager
    {
        protected readonly IMongoDatabase Database;

        public BaseMongoDbDataManager(IOptions<MongoDbSettings> settings)
        {
            Configuration = (IConfigurationRoot) settings.Value.IConfigurationRoot;
            settings.Value.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
            settings.Value.Database = Configuration.GetSection("MongoConnection:Database").Value;

            var client = new MongoClient(settings.Value.ConnectionString);
            Database = client.GetDatabase(settings.Value.Database);
        }

        private IConfigurationRoot Configuration { get; }

        public IMongoCollection<User> Users => Database.GetCollection<User>("users");

        public IMongoCollection<Feed> Feeds => Database.GetCollection<Feed>("feeds");

        public IMongoCollection<Chat> ChatMessages => Database.GetCollection<Chat>("chat");
    }
}