using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.DataManager.Mongo.Models
{
    public class User
    {

        [BsonElement("username")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Username { get; set; }
    }
}
