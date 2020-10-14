using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class MongoEpisode
    {
        [BsonElement("AirDate")]
        public string AirDate;

        [BsonElement("Crew")]
        public List<MongoCrew> Crew;

        [BsonElement("Description")]
        public string Description;

        [BsonElement("EpisodeNumber")]
        public int EpisodeNumber;

        [BsonElement("GuestStars")]
        public List<MongoEpisodeGuest> GuestStars;

        [BsonElement("Length")]
        public string Length;

        [BsonElement("Rating")]
        public double? Rating;

        [BsonElement("SeasonNumber")]
        public int SeasonNumber;

        [BsonElement("Title")]
        public string Title;

        [BsonElement("TmdbShowId")]  //need to be deleted / merged
        public string TmdbShowId;

        [BsonElement("VoteCount")]
        public int VoteCount;
    }
}