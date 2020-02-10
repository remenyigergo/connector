using Social.DataManagement.MongoDB.Models.ExtendClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social.DataManagement.MongoDB.Models
{
    public class MongoFeed
    {
        public string PersonName;
        public int UserId;
        public string PostText;
        public List<MongoComment> Comments;
        public string Picture;
        public DateTime Date;
        public List<string> TaggedPeople;
        public List<MongoLike> Likes;
    }
}
