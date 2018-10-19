using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.DataManager.Mongo.Models
{
    public class Chat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public double FromId { get; set; }
        public double ToId { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }


    }
}
