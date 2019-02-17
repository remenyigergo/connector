using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Standard.Contracts.Models.Books;

namespace Book.DataManagement.MongoDB.Models
{
    public class OnGoingBook
    {
        [BsonId]
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public UpdateLog LastUpdate { get; set; }
        public List<UpdateLog> Updates { get; set; }

    }
}
