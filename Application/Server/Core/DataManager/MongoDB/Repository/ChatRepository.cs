using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.DataManager.Mongo.DbModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Core.DataManager.Mongo.IRepository;
using Series.DataManagement.MongoDB.Models.Series;

namespace Core.DataManager.Mongo.Repository
{
    public class ChatRepository : IChatRepository
    {
        
        private readonly BaseMongoDbDataManager _context = null;

        public ChatRepository(IOptions<MongoDbSettings> settings)
        {
            _context = new BaseMongoDbDataManager(settings);
        }

        public async Task GetAllMessages()
        {
            List<Chat> chatList= new List<Chat>();
            await _context.ChatMessages.Find(x=>true)
                .ForEachAsync(doc => chatList.Add(doc));

            foreach (var chat in chatList)
            {
                
            }
        }

        public async Task<List<Chat>> GetAllMessagesByUserId(int id)
        {
            List<Chat> chatList = new List<Chat>();
            await _context.ChatMessages.Find(x => true)
                .ForEachAsync(doc => chatList.Add(doc));

            List<Chat> searchedMessagesOfUser = new List<Chat>();
            foreach (var chat in chatList)
            {
                if (chat.FromId == id || chat.ToId == id)
                {
                    searchedMessagesOfUser.Add(chat);
                }
            }

            return searchedMessagesOfUser;
        }

        
        public async Task PostMessage(Chat msg)
        {
            await _context.ChatMessages.InsertOneAsync(msg);
        }
    }
}
