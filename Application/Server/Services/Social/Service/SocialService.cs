using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Social.DataManagement.Converter;
using Social.DataManagement.MongoDB.Models.ExtendClasses;
using Social.DataManagement.MongoDB.Repositories;
using Standard.Contracts.Exceptions;
using Standard.Contracts.Models.Social;
using Standard.Core.Dependency;

namespace Social.Services
{
    public class SocialService
    {
        private readonly ISocialRepository _repo = ServiceDependency.Current.GetService<ISocialRepository>();

        public async Task<List<InternalFeed>> GetAllFeeds()
        {
            var feeds = await _repo.GetAllFeeds();
            if (feeds == null)
                throw new InternalException(660, "Null when getting feeds.");

            return new MongoToInternal().Feed(feeds);
        }

        public async Task<List<InternalFeed>> GetFeedsByUserId(int userid)
        {
            var feeds = await _repo.GetAllFeedsByUserId(userid);
            return new MongoToInternal().Feed(feeds);
        }

        public async Task<bool> Post(InternalFeed msg)
        {
            return await _repo.Post(new InternalToMongo().Feed(msg));
        }

        public async Task<List<InternalMessage>> GetAllMessagesByUserId(int userid)
        {
            var messages = await _repo.GetAllMessagesByUserId(userid);
            if (messages == null)
                throw new InternalException(661, "Messages null. Error");
            return new MongoToInternal().Messages(messages);
        }

        public async Task<bool> SendMessage(InternalMessage msg)
        {
            var result = await _repo.SendMessage(new InternalToMongo().Message(msg));
            return result;
        }

        public async Task<bool> CreateGroup(List<int> UserIds)
        {
            return await _repo.CreateGroup(UserIds);
        }

        public async Task<bool> SendGroupMessage(int groupId, string message, DateTime date, int userid)
        {
            var group = await _repo.GetGroupById(groupId);

            if (group == null)
                throw new InternalException(670, "Group wasnt found.");

            group.Messages.Add(new MongoGroupMessage
            {
                Date = date,
                Message = message,
                UserId = userid
            });
            return await _repo.SendGroupMessage(group);
        }
    }
}