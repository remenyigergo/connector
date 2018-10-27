using System;
using System.Collections.Generic;
using System.Text;
using Core.DataManager.Mongo.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;


namespace Core.DataManager.Mongo.DbModels
{
    class ObjectContext
    {
        
        public IConfigurationRoot Configuration { get; }
        private IMongoDatabase _MongoDatabase = null;

        public ObjectContext(IOptions<Settings> settings)
        {
            Configuration = (IConfigurationRoot)settings.Value.IConfigurationRoot;
            settings.Value.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
            settings.Value.Database = Configuration.GetSection("MongoConnection:Database").Value;

            var client = new MongoClient(settings.Value.ConnectionString);

            if (client != null)
            {
                _MongoDatabase = client.GetDatabase(settings.Value.Database);
            }

            
        }



        public IMongoCollection<User> Users
        {

            get
            {
                return _MongoDatabase.GetCollection<User>("users");
            }
        }

        public IMongoCollection<Feed> Feeds
        {

            get
            {
                return _MongoDatabase.GetCollection<Feed>("feeds");
            }
        }

        public IMongoCollection<Chat> ChatMessages
        {

            get
            {
                return _MongoDatabase.GetCollection<Chat>("chat");
            }
        }

        public IMongoCollection<MongoSeries> Series
        {
            get
            {
                return _MongoDatabase.GetCollection<MongoSeries>("series");
            }
        }

        public IMongoCollection<Episode> Episodes
        {

            get
            {
                return _MongoDatabase.GetCollection<Episode>("episodes");
            }
        }
    }

    
}
