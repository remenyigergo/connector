using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Social.DataManagement.MongoDB.Models;
using Social.DataManagement.MongoDB.Models.ExtendClasses;
using Standard.Contracts.Models.Social;
using Standard.Core.DataManager.MongoDB;
using Standard.Core.DataManager.MongoDB.DbModels;
using Standard.Core.DataManager.MongoDB.Extensions;

namespace Social.DataManagement.MongoDB.Repositories
{
    class SocialRepository : BaseMongoDbDataManager, ISocialRepository
    {

        public IMongoCollection<MongoFeed> Feeds => Database.GetCollection<MongoFeed>("Feeds");
        public IMongoCollection<MongoMessage> Messages => Database.GetCollection<MongoMessage>("Messages");
        public IMongoCollection<MongoGroupChat> GroupChats => Database.GetCollection<MongoGroupChat>("GroupChat");

        public SocialRepository(IOptions<MongoDbSettings> settings) : base(settings)
        {
        }

        public async Task<List<MongoFeed>> GetAllFeeds()
        {
            return await Feeds.FindAsynchronous(feed => feed == feed);
        }

        public async Task<List<MongoFeed>> GetAllFeedsByUserId(int userid)
        {
            return await Feeds.FindAsynchronous(feed => feed.UserId == userid);
        }

        public async Task<bool> Post(MongoFeed msg)
        {
            await Feeds.InsertOneAsync(msg);
            return true;
        }

        public async Task<List<MongoMessage>> GetAllMessagesByUserId(int userid)
        {
            return await Messages.FindAsynchronous(message => message.ToId == userid || message.FromId == userid);
        }

        public async Task<bool> SendMessage(MongoMessage msg)
        {
            await Messages.InsertOneAsync(msg);
            return true;
        }

        public async Task<bool> CreateGroup(List<int> UserIds)
        {
            await GroupChats.InsertOneAsync(new MongoGroupChat()
            {
                Messages = new List<MongoGroupMessage>(),
                Users = UserIds
            });

            return true;
        }

        public async Task<bool> SendGroupMessage(MongoGroupChat msg)
        {
            //TODO fejleszteni, hogy csak 1 db messaget adjak hozzá ne az egészet updateljem
            var updateDef = Builders<MongoGroupChat>.Update.Set(group => group.Messages, msg.Messages);

            var s = await GroupChats.UpdateOneAsync(group => (group.GroupId == msg.GroupId), updateDef);

            return s.ModifiedCount == 1;
        }

        public async Task<MongoGroupChat> GetGroupById(int groupId)
        {
            var groups = await GroupChats.FindAsynchronous(group => group.GroupId == groupId);
            return groups[0];
        }
    }
}
