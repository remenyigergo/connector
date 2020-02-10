using Microsoft.Extensions.Configuration;

namespace Standard.Core.DataManager.MongoDB.DbModels
{
    public class MongoDbSettings
    {
        public string ConnectionString;
        public string Database;
        public IConfiguration IConfigurationRoot;
    }
}