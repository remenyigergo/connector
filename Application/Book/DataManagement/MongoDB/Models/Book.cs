using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Book.DataManagement.MongoDB.Models
{
    public class Book
    {
        [BsonId]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Writer { get; set; }
        public Enum Genre { get; set; }
        public int Pages { get; set; }
        public int PublicationYear { get; set; }
        public int OverallRating { get; set; }
        public string Sample { get; set; }
    }
}
