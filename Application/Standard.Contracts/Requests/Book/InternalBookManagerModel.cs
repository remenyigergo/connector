using Standard.Contracts.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book.Service.Models.Request
{
    public class InternalBookManagerModel
    {
        public int UserId;
        public InternalBook Book;
    }
}
