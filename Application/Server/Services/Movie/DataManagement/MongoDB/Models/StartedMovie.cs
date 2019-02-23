using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Movie.DataManagement.MongoDB.Models
{
    public class StartedMovie
    {
        [BsonId]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id;
        public int UserId;
        public int? TmdbId;
        public string ImdbId;
        public int HoursElapsed;
        public int MinutesElapsed;
        public int SecondsElapsed;
        public DateTime Date;
        public double WatchedPercentage;
    }
}
