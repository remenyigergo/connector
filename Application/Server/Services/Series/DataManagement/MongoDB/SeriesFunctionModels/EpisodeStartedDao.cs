using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Series.Service.Models
{
    public class EpisodeStartedDao
    {
        public DateTime Date;
        public int EpisodeNumber;
        public int HoursElapsed;
        [BsonId] [BsonIgnoreIfNull] [BsonRepresentation(BsonType.ObjectId)] public string Id;
        public int MinutesElapsed;
        public int SeasonNumber;
        public int SecondsElapsed;
        public int TmdbId;
        public int TvMazeId;
        public int Userid;
        public double WatchedPercentage;
    }
}