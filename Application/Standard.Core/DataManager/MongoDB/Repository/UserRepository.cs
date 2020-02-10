using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Series.DataManagement.MongoDB.Models;
using Standard.Core.DataManager.MongoDB;
using Standard.Core.DataManager.MongoDB.DbModels;
using Standard.Core.DataManager.MongoDB.IRepository;

namespace Standard.Core.DataManager.Mongo.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BaseMongoDbDataManager _context;

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