using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class MongoSeries
    {
        [BsonId]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string TvMazeId { get; set; }
        public string TmdbId { get; set; }
        public string SeriesId { get; set; }
        public string Title { get; set; }
        public List<MongoSeason> Seasons { get; set; }
        public List<string> Runtime { get; set; }
        public double? Rating { get; set; }
        public string Year { get; set; }
        public List<string> Categories { get; set; }
        public string Description { get; set; }
        public int TotalSeasons { get; set; }
        public string LastUpdated { get; set; }
        public InternalShowCast Cast { get; set; }


        //public List<string> ExternalIds { get; set; } TODO
        //TMDB MIATT
        public List<InternalCreator> CreatedBy { get; set; }

        public List<string> EpisodeRunTime { get; set; }
        public string FirstAirDate { get; set; }
        public List<InternalGenre> Genres { get; set; }
        public string OriginalLanguage { get; set; }
        public InternalEpisodeSimple LastEpisodeSimpleToAir { get; set; }
        public List<InternalNetwork> Networks { get; set; }
        public string Popularity { get; set; }
        public List<InternalProductionCompany> ProductionCompanies { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int VoteCount { get; set; }
    }
}