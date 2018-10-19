using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.DataManager.Mongo.Models;
using MongoDB.Driver;

namespace Core.DataManager.Mongo.IRepository
{
    public interface IFeedRepository
    {
        Task<Feed> GetFeeds();
        Task PostFeedMessage(Feed msg);
    }
}
