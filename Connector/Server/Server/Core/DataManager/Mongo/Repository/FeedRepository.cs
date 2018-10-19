using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.DataManager.Mongo.DbModels;
using Core.DataManager.Mongo.IRepository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Configuration;
using Core.DataManager.Mongo.Models;
using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace Core.DataManager.Mongo.Repository
{
    public class FeedRepository : IFeedRepository
    {
        
        private readonly ObjectContext _context = null;

        public FeedRepository(IOptions<Settings> settings)
        {
            _context = new ObjectContext(settings);
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
