using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Series.DataManagement.MongoDB.Models.Series
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
