using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Series.DataManagement.MongoDB.Models;

namespace Core.DataManager.Mongo.IRepository
{
    public interface IUserRepository
    {
        Task<User> Get();
    }
}
