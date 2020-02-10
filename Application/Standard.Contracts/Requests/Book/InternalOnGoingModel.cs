using System.Collections.Generic;
using Standard.Contracts.Models.Books;

namespace Standard.Contracts.Requests.Book
{
    public class InternalOnGoingModel
    {
        public int BookId;

        public string Id;
        public int UserId;
        public InternalUpdateLog LastUpdate { get; set; }
        public List<InternalUpdateLog> Updates { get; set; }
    }
}