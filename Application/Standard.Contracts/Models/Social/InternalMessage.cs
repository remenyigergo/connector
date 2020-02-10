using System;

namespace Standard.Contracts.Models.Social
{
    public class InternalMessage
    {
        public DateTime Date;
        public int FromId;
        public string Message;
        public int ToId;
    }
}