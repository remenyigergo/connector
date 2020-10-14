using MongoDB.Bson.Serialization.Attributes;

namespace Series.DataManagement.MongoDB.Models.Series
{
    public class MongoCrew
    {
        [BsonElement("Department")]
        public string Department;

        [BsonElement("Job")]
        public string Job;

        [BsonElement("Name")]
        public string Name;
    }
}
