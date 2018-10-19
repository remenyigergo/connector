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

namespace Core.DataManager.Mongo.Repository
{
    public class UserRepository : IUserRepository
    {
        
        private readonly ObjectContext _context = null;

        public UserRepository(IOptions<Settings> settings)
        {
            _context = new ObjectContext(settings);
        }

        public async Task<User> Get()
        {
            var users = await _context.Users.FindAsync(x => x.Username != null);
            return users.FirstOrDefault();
        }

    }
}
