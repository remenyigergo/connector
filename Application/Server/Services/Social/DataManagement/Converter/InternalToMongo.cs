using Social.DataManagement.MongoDB.Models;
using Social.DataManagement.MongoDB.Models.ExtendClasses;
using Standard.Contracts.Models.Social;
using Standard.Contracts.Models.Social.ExtendClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social.DataManagement.Converter
{
    public class InternalToMongo
    {
        public MongoFeed Feed(InternalFeed internalFeed)
        {
            return new MongoFeed()
            {
                Comments = this.Comments(internalFeed.Comments),
                Date = internalFeed.Date,
                Likes = this.Likes(internalFeed.Likes),
                PersonName = internalFeed.PersonName,
                Picture = internalFeed.Picture,
                PostText = internalFeed.PostText,
                TaggedPeople = internalFeed.TaggedPeople,
                UserId = internalFeed.UserId
            };
        }


        public List<MongoComment> Comments(List<InternalComment> internalComment)
        {
            List<MongoComment> mongoComments = new List<MongoComment>();

            foreach (var comment in internalComment)
            {
                mongoComments.Add(new MongoComment()
                {
                    CommentText = comment.CommentText,
                    Date = comment.Date,
                    Likes = this.Likes(comment.Likes),
                    PersonName = comment.PersonName
                });
            }

            return mongoComments;
        }

        public List<MongoLike> Likes(List<InternalLike> internalLikes)
        {
            List<MongoLike> mongoLikes = new List<MongoLike>();

            foreach (var internalLike in internalLikes)
            {
                mongoLikes.Add(new MongoLike()
                {
                    PersonName = internalLike.PersonName
                });
            }

            return mongoLikes;
        }


        public MongoMessage Message(InternalMessage msg)
        {
            return new MongoMessage()
            {
                Date = msg.Date,
                FromId = msg.FromId,
                Message = msg.Message,
                ToId = msg.ToId
            };
        }
    }
}