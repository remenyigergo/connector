using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Series.DataManagement.MongoDB.Models.Series.MongoSeriesModels;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class MongoSeriesDao
    {
        [BsonId]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("Id")]
        public string Id { get; set; }

        [BsonElement("Guid")]
        public string Guid { get; set; }

        [BsonElement("ExternalIds")]
        public Dictionary<string, string> ExternalIds { get; set; }

        [BsonElement("TvMazeId")]
        public string TvMazeId { get; set; }

        [BsonElement("TmdbId")]
        public string TmdbId { get; set; }

        [BsonElement("SeriesId")]
        public string SeriesId { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("Seasons")]
        public List<MongoSeason> Seasons { get; set; }

        [BsonElement("Runtime")]
        public List<string> Runtime { get; set; }

        [BsonElement("Rating")]
        public double? Rating { get; set; }

        [BsonElement("Year")]
        public string Year { get; set; }

        [BsonElement("Categories")]
        public List<string> Categories { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("TotalSeasons")]
        public int TotalSeasons { get; set; }

        [BsonElement("LastUpdated")]
        public string LastUpdated { get; set; }

        [BsonElement("Cast")]
        public MongoShowCast Cast { get; set; }


        //public List<string> ExternalIds { get; set; } TODO
        //TMDB MIATT
        [BsonElement("CreatedBy")]
        public List<MongoCreator> CreatedBy { get; set; }

        [BsonElement("EpisodeRunTime")]
        public List<string> EpisodeRunTime { get; set; }

        [BsonElement("FirstAirDate")]
        public string FirstAirDate { get; set; }

        [BsonElement("Genres")]
        public List<MongoSeriesGenre> Genres { get; set; }

        [BsonElement("OriginalLanguage")]
        public string OriginalLanguage { get; set; }

        [BsonElement("LastEpisodeSimpleToAir")]
        public MongoEpisodeSimple LastEpisodeSimpleToAir { get; set; }

        [BsonElement("Networks")]
        public List<MongoNetwork> Networks { get; set; }

        [BsonElement("Popularity")]
        public string Popularity { get; set; }

        [BsonElement("ProductionCompanies")]
        public List<MongoProductionCompany> ProductionCompanies { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("VoteCount")]
        public int VoteCount { get; set; }
    }
}