using System;
using System.Collections.Generic;
using System.Text;

namespace Social.DataManagement.MongoDB.Models
{
    public class MongoMessage
    {
        public int FromId;
        public int ToId;
        public string Message;
        public DateTime Date;
    }
}
