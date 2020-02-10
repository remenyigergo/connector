using Standard.Contracts.Models.Social.ExtendClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Contracts.Models.Social
{
    public class InternalFeed
    {
        public string PersonName;
        public string PostText;
        public int UserId;
        public List<InternalComment> Comments;
        public string Picture;
        public DateTime Date;
        public List<string> TaggedPeople;
        public List<InternalLike> Likes;
    }
}
