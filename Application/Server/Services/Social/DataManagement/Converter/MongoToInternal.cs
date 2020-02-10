using System.Collections.Generic;
using Social.DataManagement.MongoDB.Models;
using Social.DataManagement.MongoDB.Models.ExtendClasses;
using Standard.Contracts.Models.Social;
using Standard.Contracts.Models.Social.ExtendClasses;

namespace Social.DataManagement.Converter
{
    public class MongoToInternal
    {
        public List<InternalFeed> Feed(List<MongoFeed> mongoFeed)
        {
            var mongoFeeds = new List<InternalFeed>();

            foreach (var feed in mongoFeed)
                mongoFeeds.Add(new InternalFeed
                {
                    Comments = Comments(feed.Comments),
                    Date = feed.Date,
                    Likes = Likes(feed.Likes),
                    PersonName = feed.PersonName,
                    Picture = feed.Picture,
                    PostText = feed.PostText,
                    TaggedPeople = feed.TaggedPeople
                });

            return mongoFeeds;
        }

        public List<InternalComment> Comments(List<MongoComment> mongoComments)
        {
            var internalComments = new List<InternalComment>();

            foreach (var mongoComment in mongoComments)
                internalComments.Add(new InternalComment
                {
                    CommentText = mongoComment.CommentText,
                    Date = mongoComment.Date,
                    Likes = Likes(mongoComment.Likes),
                    PersonName = mongoComment.PersonName
                });

            return internalComments;
        }


        public List<InternalLike> Likes(List<MongoLike> mongoLikes)
        {
            var internalLikes = new List<InternalLike>();

            foreach (var mongoLike in mongoLikes)
                internalLikes.Add(new InternalLike
                {
                    PersonName = mongoLike.PersonName
                });

            return internalLikes;
        }

        public List<InternalMessage> Messages(List<MongoMessage> mongoMessages)
        {
            var internalMessages = new List<InternalMessage>();

            foreach (var mongoMessage in mongoMessages)
                internalMessages.Add(new InternalMessage
                {
                    Date = mongoMessage.Date,
                    FromId = mongoMessage.FromId,
                    Message = mongoMessage.Message,
                    ToId = mongoMessage.ToId
                });

            return internalMessages;
        }
    }
}