using System.Collections.Generic;
using System.Threading.Tasks;
using Series.DataManagement.MongoDB.Models.Series;

namespace Standard.Core.DataManager.MongoDB.IRepository
{
    public interface IChatRepository
    {
        Task GetAllMessages();
        Task<List<Chat>> GetAllMessagesByUserId(int id);
        Task PostMessage(Chat msg);
    }
}