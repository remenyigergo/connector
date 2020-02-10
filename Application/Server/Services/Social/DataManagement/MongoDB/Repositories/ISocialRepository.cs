using Social.DataManagement.MongoDB.Models;
using Standard.Contracts.Models.Social;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Social.DataManagement.MongoDB.Repositories
{
    public interface ISocialRepository
    {
        Task<List<MongoFeed>> GetAllFeeds();
        Task<List<MongoFeed>> GetAllFeedsByUserId(int userid);
        Task<bool> Post(MongoFeed msg);
        Task<List<MongoMessage>> GetAllMessagesByUserId(int userid);
        Task<bool> SendMessage(MongoMessage msg);
        Task<bool> CreateGroup(List<int> Users);
        Task<bool> SendGroupMessage(MongoGroupChat group);
        Task<MongoGroupChat> GetGroupById(int groupId);
    }
}
