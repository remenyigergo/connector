using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Series.DataManagement.MongoDB.Models.Series;

namespace Core.DataManager.Mongo.IRepository
{
    public interface IFeedRepository
    {
        Task<Feed> GetFeeds();
        Task PostFeedMessage(Feed msg);
    }
}
