using System.Collections.Generic;
using Social.DataManagement.MongoDB.Models.ExtendClasses;

namespace Social.DataManagement.MongoDB.Models
{
    public class MongoGroupChat
    {
        public int GroupId;
        public List<MongoGroupMessage> Messages;
        public List<int> Users;
    }
}