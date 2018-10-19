using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core.DataManager.Mongo.Models
{
    public class Series
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string SeriesId { get; set; }
        public string Title { get; set; }
        public List<Season> Seasons { get; set; }
        public int Runtime { get; set; }
        public double Rating { get; set; }
        public int Year { get; set; }
        public List<string> Category { get; set; }
        public string Description { get; set; }


    }
}
