using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Series.DataManagement.MongoDB.Models;

namespace Standard.Core.DataManager.MongoDB.IRepository
{
    public interface IUserRepository
    {
        Task<User> Get();
    }
}
