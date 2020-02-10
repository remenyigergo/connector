using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Contracts.Models.Social.ExtendClasses
{
    public class InternalComment
    {
        public string PersonName;
        public string CommentText;
        public DateTime Date;
        public List<InternalLike> Likes;
    }
}
