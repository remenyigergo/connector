using System.Threading.Tasks;
using Series.DataManagement.MongoDB.Models.Series;

namespace Standard.Core.DataManager.MongoDB.IRepository
{
    public interface IFeedRepository
    {
        Task<Feed> GetFeeds();
        Task PostFeedMessage(Feed msg);
    }
}