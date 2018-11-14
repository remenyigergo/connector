using Standard.Core.DataManager.MongoDB.DbModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Series.DataManagement.MongoDB.Models;
using Series.DataManagement.MongoDB.Models.Series;

namespace Standard.Core.DataManager.MongoDB
{
    public class BaseMongoDbDataManager
    {        
        private IConfigurationRoot Configuration { get; }
        protected readonly IMongoDatabase Database;

        public BaseMongoDbDataManager(IOptions<MongoDbSettings> settings)
        {
            Configuration = (IConfigurationRoot)settings.Value.IConfigurationRoot;
            settings.Value.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
            settings.Value.Database = Configuration.GetSection("MongoConnection:Database").Value;

            var client = new MongoClient(settings.Value.ConnectionString);
            Database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<User> Users
        {

            get
            {
                return Database.GetCollection<User>("users");
            }
        }

        public IMongoCollection<Feed> Feeds
        {

            get
            {
                return Database.GetCollection<Feed>("feeds");
            }
        }

        public IMongoCollection<Chat> ChatMessages
        {

            get
            {
                return Database.GetCollection<Chat>("chat");
            }
        }        
    }

    
}
