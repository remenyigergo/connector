using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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
        public string Runtime { get; set; }
        public double? Rating { get; set; }
        public string Year { get; set; }
        public List<string> Category { get; set; }
        public string Description { get; set; }
        public int TotalSeasons { get; set; }
        public string ImdbId { get; set; }
        public List<string> ExternalIds { get; set; }
        public string Language { get; set; }
        public string Status { get; set; }
        public string LastUpdated { get; set; }
    }  
}
