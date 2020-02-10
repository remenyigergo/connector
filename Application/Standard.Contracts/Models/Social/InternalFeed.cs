using System;
using System.Collections.Generic;
using Standard.Contracts.Models.Social.ExtendClasses;

namespace Standard.Contracts.Models.Social
{
    public class InternalFeed
    {
        public List<InternalComment> Comments;
        public DateTime Date;
        public List<InternalLike> Likes;
        public string PersonName;
        public string Picture;
        public string PostText;
        public List<string> TaggedPeople;
        public int UserId;
    }
}