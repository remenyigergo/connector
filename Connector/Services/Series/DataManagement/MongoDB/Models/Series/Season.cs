using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Series.DataManagement.MongoDB.Models.Series
{
    
    public class Season
    {
        
        [BsonElement("SeasonNumber")]
        public int SeasonNumber;
        [BsonElement("EpisodesCount")]
        public int EpisodesCount;
        [BsonElement("Episodes")]
        public List<Episode> Episodes;
        [BsonElement("Summary")]
        public string Summary;
        [BsonElement("Airdate")]
        public string Airdate;
        [BsonElement("Name")]
        public string Name;

    }
}
