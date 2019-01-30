using System;
using System.Collections.Generic;
using System.Text;

namespace Book.DataManagement.MongoDB.Models
{
    public class BookUpdate
    {
        public int BookId;
        public int UserId;
        public int? HoursRead;
        public int? MinutesRead;
        public int PageNumber;
    }
}
