using System.Threading.Tasks;
using Series.DataManagement.MongoDB.Models;

namespace Standard.Core.DataManager.MongoDB.IRepository
{
    public interface IUserRepository
    {
        Task<User> Get();
    }
}