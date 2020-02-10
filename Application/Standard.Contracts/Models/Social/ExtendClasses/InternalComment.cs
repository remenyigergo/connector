using System;
using System.Collections.Generic;

namespace Standard.Contracts.Models.Social.ExtendClasses
{
    public class InternalComment
    {
        public string CommentText;
        public DateTime Date;
        public List<InternalLike> Likes;
        public string PersonName;
    }
}