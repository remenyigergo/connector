using Social.DataManagement.MongoDB.Models.ExtendClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Social.DataManagement.MongoDB.Models
{
    public class MongoGroupChat
    {
        public int GroupId;
        public List<int> Users;
        public List<MongoGroupMessage> Messages;
    }
}
