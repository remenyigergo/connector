using System;
using System.Collections.Generic;
using Social.DataManagement.MongoDB.Models.ExtendClasses;

namespace Social.DataManagement.MongoDB.Models
{
    public class MongoFeed
    {
        public List<MongoComment> Comments;
        public DateTime Date;
        public List<MongoLike> Likes;
        public string PersonName;
        public string Picture;
        public string PostText;
        public List<string> TaggedPeople;
        public int UserId;
    }
}