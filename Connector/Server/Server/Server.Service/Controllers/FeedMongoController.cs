using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataManager.Mongo.DbModels;
using Core.DataManager.Mongo.IRepository;
using Core.DataManager.Mongo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Series.DataManagement.MongoDB.Models.Series;

namespace Server.Service.Controllers
{
    [Route("api/[controller]")]    
    public class FeedMongoController : Controller
    {
        private readonly IFeedRepository _feedRepository;
        
        public FeedMongoController(IFeedRepository feedRepository)
        {
            _feedRepository = feedRepository;
        }


        [HttpGet("messages")]
        public async Task<string> GetFeeds()
        {
            var a = await _feedRepository.GetFeeds();
            return a.ToString();
        }


        [HttpPost]
        public async Task<string> PostMessage([FromBody] Feed msg)
        {
            Feed feed = new Feed();
            feed.Message = msg.Message;
            feed.Picture = msg.Picture;
            await _feedRepository.PostFeedMessage(feed);
            return "";
        }

    }
}