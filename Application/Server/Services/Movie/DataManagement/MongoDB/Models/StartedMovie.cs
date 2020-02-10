using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Movie.DataManagement.MongoDB.Models
{
    public class StartedMovie
    {
        public DateTime Date;
        public int HoursElapsed;
        [BsonId] [BsonIgnoreIfNull] [BsonRepresentation(BsonType.ObjectId)] public string Id;
        public string ImdbId;
        public int MinutesElapsed;
        public int SecondsElapsed;
        public int? TmdbId;
        public int UserId;
        public double WatchedPercentage;
    }
}