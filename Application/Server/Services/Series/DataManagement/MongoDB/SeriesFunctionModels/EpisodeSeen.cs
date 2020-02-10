using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Series.DataManagement.MongoDB.SeriesFunctionModels
{
    public class EpisodeSeen
    {
        [BsonId]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int UserId { get; set; }
        public string TvMazeId { get; set; }
        public string TmdbId { get; set; }
        public int SeasonNumber { get; set; }
        public int EpisodeNumber { get; set; }

        public DateTime Date { get; set; }
    }
}