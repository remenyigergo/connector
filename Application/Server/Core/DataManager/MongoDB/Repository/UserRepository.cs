using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.DataManager.Mongo.DbModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Configuration;
using MongoDB.Bson;
using Core.DataManager.Mongo.IRepository;
using Series.DataManagement.MongoDB.Models;

namespace Core.DataManager.Mongo.Repository
{
    public class UserRepository : IUserRepository
    {
        
        private readonly BaseMongoDbDataManager _context = null;

        public UserRepository(IOptions<MongoDbSettings> settings)
        {
            _context = new BaseMongoDbDataManager(settings);
        }

        public async Task<User> Get()
        {
            var users = await _context.Users.FindAsync(x => x.Username != null);
            return users.FirstOrDefault();
        }

    }
}
