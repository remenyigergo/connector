using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Core.DataManager.Mongo.Models;

namespace Core.DataManager.Mongo.Models
{
    
    public class Season
    {
        
        [BsonElement("SeasonNumber")]
        public int SeasonNumber;
        [BsonElement("EpisodesCount")]
        public int EpisodesCount;
        [BsonElement("Episodes")]
        public List<Episode> Episodes;
    }
}
