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
    public class ChatRepository : IChatRepository
    {
        
        private readonly ObjectContext _context = null;

        public ChatRepository(IOptions<Settings> settings)
        {
            _context = new ObjectContext(settings);
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
