using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Movie.DataManagement.MongoDB.Models
{
    public class SeenMovie
    {
        [BsonId]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int UserId { get; set; }
        public int? TmdbId { get; set; }
        public string ImdbId { get; set; }
        public DateTime Date { get; set; }
    }
}