using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Book.DataManagement.MongoDB.Models
{
    public class BookManagerModel
    {
        [BsonId]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public Book Book { get; set; }
        public int UserId { get; set; }
    }
}