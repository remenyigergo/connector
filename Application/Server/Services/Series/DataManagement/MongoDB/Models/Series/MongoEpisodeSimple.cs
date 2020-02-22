using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class MongoEpisodeSimple
    {
        [BsonElement("Air_date")]
        public string AirDate;

        [BsonElement("Episode_number")]
        public int EpisodeNumber;

        [BsonElement("Name")]
        public string Name;

        [BsonElement("Overview")]
        public string Overview;

        [BsonElement("Season_number")]
        public int SeasonNumber;

        [BsonElement("Vote_average")]
        public double? VoteAverage;

        [BsonElement("Vote_count")]
        public int VoteCount;
    }
}
