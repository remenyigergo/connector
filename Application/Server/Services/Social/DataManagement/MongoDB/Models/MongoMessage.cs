using System;

namespace Social.DataManagement.MongoDB.Models
{
    public class MongoMessage
    {
        public DateTime Date;
        public int FromId;
        public string Message;
        public int ToId;
    }
}