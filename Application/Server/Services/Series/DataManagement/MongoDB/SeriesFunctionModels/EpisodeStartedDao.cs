using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Series.Service.Models
{
    public class EpisodeStartedDao
    {
        [BsonElement("Id")]
        [BsonId]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id;

        [BsonElement("Date")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Date;

        [BsonElement("EpisodeNumber")]
        public int EpisodeNumber;

        [BsonElement("HoursElapsed")]
        public int HoursElapsed;

        [BsonElement("MinutesElapsed")]
        public int MinutesElapsed;

        [BsonElement("SeasonNumber")]
        public int SeasonNumber;

        [BsonElement("SecondsElapsed")]
        public int SecondsElapsed;

        [BsonElement("TmdbIb")]
        public int TmdbId;

        [BsonElement("TvMazeId")]
        public int TvMazeId;

        [BsonElement("UserId")]
        public int UserId;

        [BsonElement("WatchedPercentage")]
        public double WatchedPercentage;
    }
}