using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Core.DataManager.Mongo.DbModels
{
    public class Settings
    {
        public string ConnectionString;
        public string Database;
        public IConfiguration IConfigurationRoot;
    }
}
