using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Core.DataManager.Mongo.DbModels
{
    public class MongoDbSettings
    {
        public string ConnectionString;
        public string Database;
        public IConfiguration IConfigurationRoot;
    }
}
