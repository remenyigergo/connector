using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Core.DataManager.MongoDB.DbModels
{
    public class MongoDbSettings
    {
        public string ConnectionString;
        public string Database;
        public IConfiguration IConfigurationRoot;
    }
}
