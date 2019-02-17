using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Book.DataManagement.MongoDB.Models
{
    public class UpdateLog
    {
        public int PageNumber { get; set; }
        public int HoursRead { get; set; }
        public int MinutesRead { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UpDateTime { get; set; }
    }
}
