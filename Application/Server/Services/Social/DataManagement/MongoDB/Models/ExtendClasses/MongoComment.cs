using System;
using System.Collections.Generic;

namespace Social.DataManagement.MongoDB.Models.ExtendClasses
{
    public class MongoComment
    {
        public string CommentText;
        public DateTime Date;
        public List<MongoLike> Likes;
        public string PersonName;
    }
}