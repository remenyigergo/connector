using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class MongoSeason
    {
        [BsonElement("Airdate")] public string Airdate;
        [BsonElement("Episodes")] public List<MongoEpisode> Episodes;
        [BsonElement("EpisodesCount")] public int EpisodesCount;
        [BsonElement("Name")] public string Name;
        [BsonElement("SeasonNumber")] public int SeasonNumber;
        [BsonElement("Summary")] public string Summary;
    }
}