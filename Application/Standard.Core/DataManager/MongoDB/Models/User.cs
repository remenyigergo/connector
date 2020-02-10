using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Series.DataManagement.MongoDB.Models
{
    public class User
    {
        [BsonElement("username")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Username { get; set; }
    }
}