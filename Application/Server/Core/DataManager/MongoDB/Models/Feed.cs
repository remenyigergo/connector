using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class Feed
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        
        public string Message { get; set; }

        
        public string Picture { get; set; }
        
    }
}
