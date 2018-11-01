using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Series.Parsers.Trakt.Models;
using Series.Parsers.Trakt.Models.TraktShowModels;

namespace Core.DataManager.Mongo.Models
{
    public class MongoSeries
    {
        [BsonId]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]        
        public string Id { get; set; }
        public string SeriesId { get; set; }
        public string Title { get; set; }
        public List<Season> Seasons { get; set; }
        public List<string> Runtime { get; set; }
        public double? Rating { get; set; }
        public string Year { get; set; }
        public List<string> Category { get; set; }
        public string Description { get; set; }
        public int TotalSeasons { get; set; }
        public string LastUpdated { get; set; }



        //public List<string> ExternalIds { get; set; } TODO
        //TMDB MIATT
        public List<InternalCreator> Created_by { get; set; }

        public List<string> Episode_run_time { get; set; }
        public string First_air_date { get; set; }
        public List<InternalGenre> Genres { get; set; }
        public string Original_language { get; set; }
        public InternalEpisodeSimple LastEpisodeSimpleToAir { get; set; }
        public List<InternalNetwork> Networks { get; set; }
        public string Popularity { get; set; }
        public List<InternalProductionCompany> Production_companies { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public int Vote_count { get; set; }
    }  
}
