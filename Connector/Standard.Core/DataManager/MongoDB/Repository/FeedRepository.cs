using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Standard.Core.DataManager.MongoDB.DbModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Standard.Core.DataManager.MongoDB.IRepository;
using Series.DataManagement.MongoDB.Models.Series;

namespace Standard.Core.DataManager.MongoDB.Repository
{
    public class FeedRepository : IFeedRepository
    {
        
        private readonly BaseMongoDbDataManager _context = null;

        public FeedRepository(IOptions<MongoDbSettings> settings)
        {
            _context = new BaseMongoDbDataManager(settings);
        }

        public async Task<Feed> GetFeeds()
        {

            var feeds = await _context.Feeds.FindAsync(x => true);
            return feeds.FirstOrDefault();

            //var feeds = await _context.Feeds.FindSync(x => true).ToListAsync();
            //return feeds.;
            //NEM VÁGOM
        }

        public async Task PostFeedMessage(Feed msg)
        {
            await _context.Feeds.InsertOneAsync(msg);
        }

       
    }
}
