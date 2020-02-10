using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Series.DataManagement.MongoDB.Models.Series;
using Standard.Core.DataManager.MongoDB.DbModels;
using Standard.Core.DataManager.MongoDB.IRepository;

namespace Standard.Core.DataManager.MongoDB.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly BaseMongoDbDataManager _context;

        public ChatRepository(IOptions<MongoDbSettings> settings)
        {
            _context = new BaseMongoDbDataManager(settings);
        }

        public async Task GetAllMessages()
        {
            var chatList = new List<Chat>();
            await _context.ChatMessages.Find(x => true)
                .ForEachAsync(doc => chatList.Add(doc));

            foreach (var chat in chatList)
            {
            }
        }

        public async Task<List<Chat>> GetAllMessagesByUserId(int id)
        {
            var chatList = new List<Chat>();
            await _context.ChatMessages.Find(x => true)
                .ForEachAsync(doc => chatList.Add(doc));

            var searchedMessagesOfUser = new List<Chat>();
            foreach (var chat in chatList)
                if (chat.FromId == id || chat.ToId == id)
                    searchedMessagesOfUser.Add(chat);

            return searchedMessagesOfUser;
        }


        public async Task PostMessage(Chat msg)
        {
            await _context.ChatMessages.InsertOneAsync(msg);
        }
    }
}