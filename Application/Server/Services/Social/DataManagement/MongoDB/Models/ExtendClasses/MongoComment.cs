using System;
using System.Collections.Generic;
using System.Text;

namespace Social.DataManagement.MongoDB.Models.ExtendClasses
{
    public class MongoComment
    {
        public string PersonName;
        public string CommentText;
        public DateTime Date;
        public List<MongoLike> Likes;
    }
}
