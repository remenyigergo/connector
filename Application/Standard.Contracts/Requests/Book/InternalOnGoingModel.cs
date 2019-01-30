using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Standard.Contracts.Models.Books;

namespace Standard.Contracts.Requests.Book
{
    public class InternalOnGoingModel
    {

        public string Id;
        public int UserId;
        public int BookId;
        public InternalUpdateLog LastUpdate { get; set; }
        public List<InternalUpdateLog> Updates { get; set; }

    }
}
