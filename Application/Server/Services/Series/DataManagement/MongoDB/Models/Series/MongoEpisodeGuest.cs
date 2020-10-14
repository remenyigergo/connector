using MongoDB.Bson.Serialization.Attributes;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class MongoEpisodeGuest
    {
        [BsonElement("Character")]
        public string Character;

        [BsonElement("Name")]
        public string Name;
    }
}
